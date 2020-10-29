Imports System.IO
Imports System.Threading
Imports System.ComponentModel
Imports Gigasoft.ProEssentials
Imports Gigasoft.ProEssentials.Enums
Imports MathNet.Numerics

Public Class pSpectra

    Public SpectrumLength As UInt32 = 16384
    Dim MaxSpectrumLength As UInt32 = 65536
    Dim MaxNumberOfChannel As UInt32 = 32
    Public logmode As Boolean = False
    Public addressData As UInt32
    Public addressWait As UInt32
    Public addressMask As UInt32
    Public addressMode As UInt32
    Public addressArm As UInt32
    Public addressStatus As UInt32
    Public addressValidWords As UInt32

    Public addressSync As UInt32
    Public addressRUN As UInt32

    'Public addressDataR As New List(Of UInt32)
    'Public addressArmR As New List(Of UInt32)
    'Public addressStatusR As New List(Of UInt32)
    'Public addressSyncR As New List(Of UInt32)

    Public n_ch As Integer
    Dim _n_ch As Integer
    Dim MutexSpe As New Mutex
    Dim MutexCumulative As New Mutex
    Dim MutexFile As New Mutex
    Public spectra(MaxNumberOfChannel, MaxSpectrumLength) As Double
    Public Rebinned_spectra(MaxNumberOfChannel, MaxSpectrumLength) As Double
    Public realtimeimage(MaxNumberOfChannel) As Double
    Public integralimage(MaxNumberOfChannel) As Double
    Dim fileName As String
    Public fileEnable As Boolean = False
    Public running As Boolean = False
    Public spectracount As ULong = 0
    Public EnabledChannel() As Boolean
    Public EnabledChannel_id() As Integer
    Public TargetEvent As UInt32
    Public TargetMode As Integer
    Dim objRawWriter As StreamWriter
    Dim T_spect As Thread
    Dim StopT_spect As Boolean = False
    Dim ChList_name As New List(Of String)
    Dim Checked_id As New List(Of Integer)
    Dim freq = 80000000
    Dim T0 As UInt64
    Dim T As UInt64
    Dim isRunning As Boolean = False
    Dim plotS As Integer = 0
    Public method As SGraphPlottingMethods
    Dim colorList() As Color = {Color.Red, Color.Yellow, Color.Lime, Color.Cyan, Color.Magenta, Color.Blue, Color.BlueViolet, Color.Violet, Color.Peru, Color.Orange, Color.White,
                                Color.DarkRed, Color.Gold, Color.DarkGreen, Color.Teal, Color.HotPink, Color.RoyalBlue, Color.Purple, Color.Sienna, Color.Chocolate, Color.LightSlateGray,
                                Color.Tomato, Color.Moccasin, Color.PaleGreen, Color.PaleTurquoise, Color.Plum, Color.DeepSkyBlue, Color.MediumVioletRed, Color.RosyBrown, Color.LightSalmon, Color.Silver, Color.Olive}



    Class Evento
        Public timecode As UInt64
        Public energy() As Double
        Public valid As Boolean
        Public eventId As UInt32
        Public size
        Public ValidEvent As UInt32
        Public SingleChannelTriggerId As UInt32
        Public Sub New(_size As Integer)
            size = _size
            ReDim energy(size)
        End Sub
    End Class


    Private Sub pSpectra_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DisegnaGrafico(Pesgo1, SGraphPlottingMethod.Step)
        _n_ch = Connection.ComClass._n_ch
        n_ch = MainForm.acquisition.CHList.Count
        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
            addressData = MainForm.CurrentMCA.Address
        ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            addressData = (MainForm.CurrentCP.Address)
            MaxNumberOfChannel = _n_ch * Connection.ComClass._nBoard
            ReDim spectra(MaxNumberOfChannel, MaxSpectrumLength)
            ReDim Rebinned_spectra(MaxNumberOfChannel, MaxSpectrumLength)
            ReDim realtimeimage(MaxNumberOfChannel)
            ReDim integralimage(MaxNumberOfChannel)
        End If

        reload()

    End Sub

    Public Sub reload()
        CheckedListBox1.Items.Clear()
        ChList_name.Clear()
        ReDim EnabledChannel(n_ch - 1)
        ReDim EnabledChannel_id(n_ch - 1)
        CheckedListBox1.Items.Add("ALL", False)
        For i = 0 To n_ch - 1
            ChList_name.Add(MainForm.acquisition.CHList(i).name)
            CheckedListBox1.Items.Add(ChList_name(i), MainForm.acquisition.CHList(i).spectra_checked)
            EnabledChannel(i) = False
        Next

    End Sub

    Public Sub Producer()
        Dim PCC(Connection.ComClass._nBoard) As Queue(Of UInt32)
        For i = 0 To Connection.ComClass._nBoard - 1
            PCC(i) = New Queue(Of UInt32)
        Next
        While StopT_spect = False
            Dim npacket = 100
            Dim read_data As UInt32
            Dim valid_data As UInt32
            Dim status As UInt32

            If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
                Dim lenght = (n_ch + 9) * npacket
                Dim data(lenght) As UInt32

                Dim status_error = Connection.ComClass.GetRegister(addressStatus, status, 0)
                If status_error = 0 And status = 2 Then
                    If Connection.ComClass.ReadData(addressData, data, lenght, 1, 1000, read_data, valid_data, 0) = 0 Then
                        UnpackDataPacket(data, lenght, n_ch)
                    Else
                        MainForm.plog.TextBox1.AppendText("Communication error" & vbCrLf)
                        Exit Sub
                    End If
                End If

            ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Then
                Dim lenght = (_n_ch + 3) * npacket
                Dim data(lenght) As UInt32
                For cp = 0 To Connection.ComClass._nBoard - 1
                    Dim status_error = Connection.ComClass.GetRegister(addressStatus, status, cp)
                    If status_error = 0 And status <> 3 Then

                        If Connection.ComClass.ReadDataFifo(addressData, data, lenght, addressStatus, 1, 100, read_data, valid_data, cp, addressValidWords) = 0 Then

                            For i = 0 To read_data - 1
                                PCC(cp).Enqueue(data(i))
                            Next

                            UnpackDataPacketR(PCC(cp), cp)
                        Else
                            MainForm.plog.TextBox1.AppendText("Communication error" & vbCrLf)
                            Exit Sub
                        End If
                    End If
                Next
            ElseIf Connection.ComClass._boardModel = communication.tModel.SCIDK Then
                Dim lenght = 16384
                Dim data(lenght) As UInt32
                For cp = 0 To Connection.ComClass._nBoard - 1
                    Dim status_error = Connection.ComClass.GetRegister(addressStatus, status, cp)
                    If status_error = 0 And status <> 3 Then

                        If Connection.ComClass.ReadDataFifo(addressData, data, lenght, addressStatus, 1, 100, read_data, valid_data, cp, addressValidWords) = 0 Then
                            Console.WriteLine(valid_data)
                            For i = 0 To read_data - 1
                                PCC(cp).Enqueue(data(i))
                            Next

                            UnpackDataPacketS(PCC(cp), cp)
                        Else
                            MainForm.plog.TextBox1.AppendText("Communication error" & vbCrLf)
                            Exit Sub
                        End If
                    End If
                Next
            End If

        End While

    End Sub

    Public Sub StartSpectraReceiverThread()

        StopT_spect = False
        Dim pd1 As New ThreadStart(AddressOf Producer)
        T_spect = New Thread(pd1)
        T_spect.Start()

    End Sub

    Public Sub StopSpectraReceiverThread()

        StopT_spect = True

    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged

        Checked_id.Clear()
        If CheckedListBox1.SelectedIndex = 0 Then
            Dim state = IIf(CheckedListBox1.GetItemCheckState(0).ToString = "Checked", True, False)
            For i = 1 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemChecked(i, state)
                MainForm.acquisition.CHList(i - 1).spectra_checked = state
            Next
        Else
            MainForm.acquisition.CHList(CheckedListBox1.SelectedIndex - 1).spectra_checked = IIf(CheckedListBox1.GetItemCheckState(CheckedListBox1.SelectedIndex).ToString = "Checked", True, False)
            If CheckedListBox1.GetItemCheckState(0).ToString = "Checked" And CheckedListBox1.GetItemCheckState(CheckedListBox1.SelectedIndex).ToString = "Unchecked" Then
                CheckedListBox1.SetItemChecked(0, False)
            End If
            Dim all_checked = True
            For i = 1 To CheckedListBox1.Items.Count - 1
                If CheckedListBox1.GetItemCheckState(i).ToString = "Checked" Then
                Else
                    all_checked = False
                    Exit For
                End If
            Next
            If all_checked Then
                CheckedListBox1.SetItemChecked(0, True)
            End If
        End If
        For Each scope_ch In MainForm.acquisition.CHList
            If scope_ch.spectra_checked Then
                Checked_id.Add(scope_ch.id)
            End If
        Next

    End Sub

    Public Sub StartDataCaptureOnFile(file As String)



        MutexFile.WaitOne()
        If fileEnable = False Then
            fileName = file
            fileEnable = True
            objRawWriter = New StreamWriter(fileName)
            MainForm.plog.TextBox1.AppendText("Starting Spectrum Recording..." & vbCrLf)
            isRunning = True
        End If
        MutexFile.ReleaseMutex()

    End Sub

    Public Sub StopDataCaptureOnFile()

        MutexFile.WaitOne()
        fileEnable = False
        If IsNothing(objRawWriter) Then
        Else
            objRawWriter.Close()
            MainForm.plog.TextBox1.AppendText("Stopping Spectrum Recording." & vbCrLf)
        End If
        isRunning = False
        MutexFile.ReleaseMutex()

    End Sub

    Public Sub resetspectrum()

        MutexSpe.WaitOne()
        For k = 0 To n_ch - 1
            For i = 0 To MaxSpectrumLength - 1
                spectra(k, i) = 0
            Next
        Next
        MutexSpe.ReleaseMutex()
        MutexCumulative.WaitOne()
        For k = 0 To n_ch - 1
            integralimage(k) = 0
        Next
        MainForm.plog.TextBox1.AppendText("Reset Spectrum" & vbCrLf)
        MutexCumulative.ReleaseMutex()

    End Sub

    Public Sub stopspectrum()


        If IsNothing(T_spect) Then
            Exit Sub
        End If
        StopSpectraReceiverThread()
        While T_spect.IsAlive
            Application.DoEvents()
            System.Threading.Thread.Sleep(10)
        End While

        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
            If Connection.ComClass.SetRegister(addressArm, 0, 0) = 0 Then
                Timer1.Enabled = False
                MainForm.plog.TextBox1.AppendText("Stopping Spectrum Acquisition." & vbCrLf)
            Else
                MainForm.plog.TextBox1.AppendText("Error on CONFIG_ARM" & vbCrLf)
            End If
        ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            For cp = 0 To Connection.ComClass._nBoard - 1
                If Connection.ComClass.SetRegister(addressRUN, 0, cp) = 0 Then

                End If
            Next
            For cp = 0 To Connection.ComClass._nBoard - 1
                If Connection.ComClass.SetRegister(addressArm, 0, cp) = 0 Then
                    Timer1.Enabled = False
                    MainForm.plog.TextBox1.AppendText("Stopping Spectrum Acquisition." & vbCrLf)
                Else
                    MainForm.plog.TextBox1.AppendText("Error on CONFIG_ARM" & vbCrLf)
                End If
            Next
        End If


        running = False
        MainForm.sets.Apply.Enabled = True

    End Sub

    Public Sub startspectrum()
        Dim _start = False

        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then


            If Connection.ComClass.SetRegister(addressSync, 0, 0) = 0 Then
                If Connection.ComClass.SetRegister(addressWait, 0, 0) = 0 Then
                    Dim mode_value As UInt32
                    If MainForm.acquisition.General_settings.TriggerSource = 1 Then
                        mode_value = Convert.ToInt32("010", 2)
                    ElseIf MainForm.acquisition.General_settings.TriggerSource = 0 Then
                        mode_value = Convert.ToInt32("001", 2)
                    End If
                    If Connection.ComClass.SetRegister(addressMode, mode_value, 0) = 0 Then
                        If Connection.ComClass.SetRegister(addressMask, &HFFFFFFFF&, 0) = 0 Then
                            If Connection.ComClass.SetRegister(addressArm, 2, 0) = 0 Then
                                If Connection.ComClass.SetRegister(addressArm, 0, 0) = 0 Then
                                    If Connection.ComClass.SetRegister(addressArm, 1, 0) = 0 Then
                                        StartSpectraReceiverThread()
                                        Timer1.Enabled = True
                                        MainForm.plog.TextBox1.AppendText("Starting Spectrum Acquisition..." & vbCrLf)
                                        running = True
                                        MainForm.sets.Apply.Enabled = False
                                    Else
                                        MainForm.plog.TextBox1.AppendText("Error on CONFIG_ARM!" & vbCrLf)
                                    End If
                                Else
                                    MainForm.plog.TextBox1.AppendText("Error on CONFIG_ARM!" & vbCrLf)
                                End If
                            Else
                                MainForm.plog.TextBox1.AppendText("Error on CONFIG_ARM!" & vbCrLf)
                            End If
                        Else
                            MainForm.plog.TextBox1.AppendText("Error on CONFIG_T0_MASK!" & vbCrLf)
                        End If
                    Else
                        MainForm.plog.TextBox1.AppendText("Error on CONFIG_TRIGGER_MODE!" & vbCrLf)
                    End If
                Else
                    MainForm.plog.TextBox1.AppendText("Error on CONFIG_WAIT!" & vbCrLf)
                End If
            Else
                MainForm.plog.TextBox1.AppendText("Error on CONFIG_SYNC!" & vbCrLf)
            End If

            For cp = 0 To Connection.ComClass._nBoard - 1
                If Connection.ComClass.SetRegister(addressRUN, 1, cp) = 0 Then

                End If
            Next
        ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.SCIDK Then

            For cp = 0 To Connection.ComClass._nBoard - 1
                If Connection.ComClass.SetRegister(addressRUN, 0, cp) = 0 Then

                End If
            Next


            Dim data(1000000) As UInt32
            Dim status As Integer
            Dim rd, vd As Integer
            For cp = 0 To Connection.ComClass._nBoard - 1
                Dim status_error = Connection.ComClass.GetRegister(addressStatus, status, cp)
                If status_error = 0 And status <> 3 Then

                    If Connection.ComClass.ReadDataFifo(addressData, data, 1000000 - 1, addressStatus, 1, 100, rd, vd, cp, addressValidWords) = 0 Then

                    Else
                        MainForm.plog.TextBox1.AppendText("Communication error" & vbCrLf)
                        Exit Sub
                    End If
                End If
            Next

            For cp = 0 To Connection.ComClass._nBoard - 1
                Connection.ComClass.SetRegister(addressArm, 2, cp)
                If Connection.ComClass.SetRegister(addressArm, 0, cp) = 0 Then
                    Connection.ComClass.SetRegister(addressRUN, 1, cp)                      'APPENA AGGIUNTO PROVA
                    If Connection.ComClass.SetRegister(addressArm, 1, cp) = 0 Then
                        _start = True
                    Else
                        MainForm.plog.TextBox1.AppendText("Error on CONFIG_ARM!" & vbCrLf)
                    End If
                Else
                    MainForm.plog.TextBox1.AppendText("Error on CONFIG_ARM!" & vbCrLf)
                End If
            Next

            If _start Then
                StartSpectraReceiverThread()
                Timer1.Enabled = True
                MainForm.plog.TextBox1.AppendText("Starting Spectrum Acquisition..." & vbCrLf)
                running = True
                MainForm.sets.Apply.Enabled = False
            End If

        End If



    End Sub

    Public Sub saveSpectrum(f As String)
        MainForm.plog.TextBox1.AppendText("Saving Spectrum..." & vbCrLf)
        objRawWriter = New StreamWriter(f)
        For i = 0 To SpectrumLength - 1
            For Each s In Checked_id
                If s <> 0 Then
                    objRawWriter.Write(Rebinned_spectra(s - 1, i) & ";")
                End If
            Next
            objRawWriter.WriteLine()
        Next
        objRawWriter.Close()
        MainForm.plog.TextBox1.AppendText("Done." & vbCrLf)
    End Sub

    Public Sub saveImage(f As String)
        MainForm.plog.TextBox1.AppendText("Saving Image..." & vbCrLf)
        objRawWriter = New StreamWriter(f)
        For i = 0 To MaxNumberOfChannel - 1
            objRawWriter.Write(integralimage(i) & ";")
        Next
        For Each ch In MainForm.acquisition.CHList
            objRawWriter.Write(integralimage(ch.id - 1) & ";")
        Next
        objRawWriter.Close()
        MainForm.plog.TextBox1.AppendText("Done." & vbCrLf)
    End Sub
    Dim timeStart As DateTime
    Public Sub setStartTimeNow()
        timeStart = Now
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If SpectrumLength <> MaxSpectrumLength Then
            Rebinned_spectra = Rebin(spectra, SpectrumLength)
        Else
            Rebinned_spectra = spectra
        End If

        If fileEnable Then
            If TargetMode = 0 Then
                MainForm.ProgressBar.Value = 0
            End If
            If TargetMode = 1 Then
                If spectracount >= TargetEvent Then
                    MainForm.ProgressBar.Value = 100
                    StopDataCaptureOnFile()
                    MainForm.SaveData.Enabled = True
                    MainForm.StopSaveData.Enabled = False
                Else
                    MainForm.ProgressBar.Value = spectracount / TargetEvent * 100
                End If
            End If
            If TargetMode = 2 Then
                'If (T / freq) >= TargetEvent Then
                If (Now - timeStart).TotalMilliseconds / 1000.0 >= TargetEvent Then

                    MainForm.ProgressBar.Value = 100
                    StopDataCaptureOnFile()
                    MainForm.SaveData.Enabled = True
                    MainForm.StopSaveData.Enabled = False
                Else
                    MainForm.ProgressBar.Value = (Now - timeStart).TotalSeconds / TargetEvent * 100
                End If
            End If
        End If

        Dim checked_ch As Integer
        If CheckedListBox1.CheckedItems.Contains("ALL") Then
            checked_ch = n_ch
        Else
            checked_ch = CheckedListBox1.CheckedIndices.Count
        End If

        Pesgo1.PeAnnotation.Line.XAxis.Clear()

        If MainForm.fit_enabled = False Then
            Dim l = 0
            Pesgo1.PeData.Points = SpectrumLength
            Pesgo1.PeData.Subsets = checked_ch
            Dim LinearArray(SpectrumLength * n_ch) As Single
            Dim LinearArrayX(SpectrumLength * n_ch) As Single
            For Each s In Checked_id
                If s <> 0 Then


                    For p = 0 To SpectrumLength - 1
                        LinearArrayX(l * SpectrumLength + p) = p
                        If logmode = False Then
                            LinearArray(l * SpectrumLength + p) = Rebinned_spectra(s - 1, p)
                        Else
                            LinearArray(l * SpectrumLength + p) = Rebinned_spectra(s - 1, p) + 1

                        End If
                    Next p

                    If (CheckedListBox1.CheckedItems.Count > 0) Then
                        Pesgo1.PePlot.Methods(l) = method 'SGraphPlottingMethods.Step
                        Pesgo1.PeColor.SubsetColors(l) = colorList(l Mod 32)
                        Pesgo1.PePlot.SubsetLineTypes(l) = LineType.ThinSolid
                        If CheckedListBox1.CheckedItems.Contains("ALL") Then
                            Pesgo1.PeString.SubsetLabels(l) = CheckedListBox1.CheckedItems(l + 1)
                        Else
                            Pesgo1.PeString.SubsetLabels(l) = CheckedListBox1.CheckedItems(l)
                        End If
                        l = l + 1
                    End If
                End If
            Next
            If (CheckedListBox1.CheckedItems.Count > 0) Then
                Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.XData, LinearArrayX, Convert.ToInt32(SpectrumLength * l))
                Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.YData, LinearArray, Convert.ToInt32(SpectrumLength * l))
            End If

            If logmode = False Then
                Pesgo1.PeGrid.Configure.YAxisScaleControl = Gigasoft.ProEssentials.Enums.ScaleControl.Normal
            Else
                Pesgo1.PeGrid.Configure.YAxisScaleControl = Gigasoft.ProEssentials.Enums.ScaleControl.Log
            End If
            If (CheckedListBox1.CheckedItems.Count > 0) Then
                Pesgo1.Refresh()
            End If
        Else
                Dim l = 0

            Dim n_fit As Integer = 0
            For r = 0 To MainForm.fit.DataGridView1.Rows.Count - 1
                If MainForm.fit.DataGridView1.Rows(r).Cells("Channel").Value IsNot Nothing Then
                    If (MainForm.fit.DataGridView1.Rows(r).Cells("Cursor 1").Value IsNot Nothing) Or (MainForm.fit.DataGridView1.Rows(r).Cells("Cursor 2").Value IsNot Nothing) Then
                        n_fit += 1
                    End If
                End If
            Next
            Dim n_spettri = checked_ch + n_fit
            Pesgo1.PeData.Points = SpectrumLength
            Pesgo1.PeData.Subsets = n_spettri
            Dim graficisullospettro((n_spettri) * SpectrumLength) As Single
            Dim graficisullospettro_x((n_spettri) * SpectrumLength) As Single
            For Each s In Checked_id
                If s <> 0 Then
                    For p = 0 To SpectrumLength - 1
                        If logmode = False Then
                            graficisullospettro(p + SpectrumLength * (l)) = Rebinned_spectra(s - 1, p)
                        Else
                            graficisullospettro(p + SpectrumLength * (l)) = Rebinned_spectra(s - 1, p) + 1
                        End If
                        graficisullospettro_x(p + SpectrumLength * (l)) = p
                    Next
                    If (CheckedListBox1.CheckedItems.Count > 0) Then
                        Pesgo1.PePlot.Methods(l) = method 'SGraphPlottingMethods.Step
                        Pesgo1.PeColor.SubsetColors(l) = colorList(l Mod 32)
                        Pesgo1.PePlot.SubsetLineTypes(l) = LineType.ThinSolid
                        If CheckedListBox1.CheckedItems.Contains("ALL") Then
                            Pesgo1.PeString.SubsetLabels(l) = CheckedListBox1.CheckedItems(l + 1)
                        Else
                            Pesgo1.PeString.SubsetLabels(l) = CheckedListBox1.CheckedItems(l)
                        End If
                        l = l + 1
                    End If
                End If
            Next
            For k = 0 To n_fit - 1
                Dim sx, ex As Integer
                Try
                    sx = CInt(MainForm.fit.DataGridView1.Rows(k).Cells("Cursor 1").Value)
                    ex = CInt(MainForm.fit.DataGridView1.Rows(k).Cells("Cursor 2").Value)
                    Dim dataproc_y(ex - sx) As Double
                    Dim dataproc_x(ex - sx) As Double
                    Dim mean As Double = 0
                    Dim q = 0
                    Dim std_dev As Double = 0
                    Dim max As Integer = 0
                    Dim idx = 0
                    Dim mu1, sg1, A1 As Double
                    Dim areaUpeak As Double = 0
                    Dim areaFit As Double = 0
                    Dim dataCorretto As Double
                    Dim selected_ch, selected_ch_id As Integer
                    selected_ch = CInt(MainForm.fit.DataGridView1.Rows(k).Cells("Channel").Value.ToString.Replace("CHANNEL ", ""))
                    selected_ch_id = MainForm.acquisition.CHList(selected_ch - 1).id - 1
                    For i = sx To ex
                        dataCorretto = Rebinned_spectra(selected_ch_id, i)
                        mean = mean + dataCorretto * i
                        q = q + dataCorretto
                        If dataCorretto > max Then
                            max = dataCorretto
                        End If
                        dataproc_y(idx) = Math.Log(dataCorretto + 1)
                        If Double.IsNaN(dataproc_y(idx)) Then
                            If idx > 0 Then
                                dataproc_y(idx) = dataproc_y(idx - 1)
                            Else
                                dataproc_y(idx) = 0
                            End If
                        Else
                            If Double.IsNegativeInfinity(dataproc_y(idx)) Then
                                If idx > 0 Then
                                    dataproc_y(idx) = dataproc_y(idx - 1)
                                Else
                                    dataproc_y(idx) = 0
                                End If
                            End If
                        End If
                        dataproc_x(idx) = i
                        idx = idx + 1
                    Next
                    mean = mean / q
                    q = 0
                    For i = sx To ex
                        dataCorretto = Rebinned_spectra(selected_ch_id, i)
                        std_dev = std_dev + dataCorretto * Math.Pow(i - mean, 2)
                        q = q + dataCorretto
                    Next
                    areaUpeak = q
                    std_dev = std_dev / q
                    std_dev = Math.Sqrt(std_dev)
                    Dim fitres As Double() = Fit.Polynomial(dataproc_x, dataproc_y, 2)
                    mu1 = -1 * fitres(1) / (2 * fitres(2))
                    sg1 = Math.Sqrt(-1 / (2 * fitres(2)))
                    A1 = Math.Exp(fitres(0) - (Math.Pow(fitres(1), 2) / (4 * fitres(2))))
                    For i = sx To ex
                        areaFit += A1 * Math.Exp(-Math.Pow((i - mu1), 2) / (2 * sg1 * sg1))
                    Next
                    If Double.IsNaN(mu1) Then
                        sg1 = sg1
                    End If
                    MainForm.fit.DataGridView1.Rows(k).Cells("mean").Value = Math.Round(mean, 2)
                    MainForm.fit.DataGridView1.Rows(k).Cells("fitmu").Value = Math.Round(mu1, 2)
                    MainForm.fit.DataGridView1.Rows(k).Cells("sigma").Value = Math.Round(std_dev, 2) '& " (" & Math.Round(2.35 * std_dev * 100 / mean, 2) & "%)"
                    MainForm.fit.DataGridView1.Rows(k).Cells("fitsigma").Value = Math.Round(sg1, 3) ' & " (" & Math.Round(2.35 * sg1 / mu1 * 100, 2) & "%)"
                    MainForm.fit.DataGridView1.Rows(k).Cells("fwhm").Value = Math.Round(2.35 * sg1, 3)
                    MainForm.fit.DataGridView1.Rows(k).Cells("Resolution").Value = Math.Round(2.35 * sg1 / mu1 * 100, 2) & "%"
                    MainForm.fit.DataGridView1.Rows(k).Cells("areaUpeak").Value = areaUpeak
                    MainForm.fit.DataGridView1.Rows(k).Cells("areaFit").Value = Math.Round(areaFit)
                    Dim point As Double
                    Dim startx = sx - 15
                    Dim endx = ex + 15
                    If startx < 0 Then
                        startx = 0
                    End If
                    If endx > SpectrumLength - 1 Then
                        endx = SpectrumLength - 1
                    End If
                    Dim g = 0
                    For g = 0 To SpectrumLength
                        If logmode Then
                            graficisullospettro((k + checked_ch) * SpectrumLength + g) = 1
                        Else
                            graficisullospettro((k + checked_ch) * SpectrumLength + g) = 0
                        End If
                        graficisullospettro_x((k + checked_ch) * SpectrumLength + g) = g
                    Next
                    For i = sx To ex
                        point = A1 * Math.Exp(-1 * Math.Pow(i - mu1, 2) / (2 * Math.Pow(sg1, 2))) '+ meanBG' fitrect(0) + fitrect(1) * i
                        If logmode = False Then
                            graficisullospettro((k + checked_ch) * SpectrumLength + i) = point
                        Else
                            graficisullospettro((k + checked_ch) * SpectrumLength + i) = point + 1
                        End If
                        graficisullospettro_x((k + checked_ch) * SpectrumLength + i) = i
                    Next
                    If (CheckedListBox1.CheckedItems.Count > 0) Then
                        Pesgo1.PePlot.Methods(k + checked_ch) = SGraphPlottingMethods.SplineArea
                        Pesgo1.PeColor.SubsetColors(k + checked_ch) = Color.FromArgb(5, 0, 255, 50)
                        Pesgo1.PeString.SubsetLabels(k + checked_ch) = "Fit " & k + 1
                    End If
                Catch exc As Exception

                End Try
                If (CheckedListBox1.CheckedItems.Count > 0) Then
                    Pesgo1.PeAnnotation.Line.XAxis(2 * k) = sx
                    Pesgo1.PeAnnotation.Line.XAxisType(2 * k) = LineAnnotationType.ThinSolid
                    Pesgo1.PeAnnotation.Line.XAxisColor(2 * k) = Color.White
                    Pesgo1.PeAnnotation.Line.XAxisText(2 * k) = k + 1
                    Pesgo1.PeAnnotation.Line.XAxis(2 * k + 1) = ex
                    Pesgo1.PeAnnotation.Line.XAxisType(2 * k + 1) = LineAnnotationType.ThinSolid
                    Pesgo1.PeAnnotation.Line.XAxisColor(2 * k + 1) = Color.White
                    Pesgo1.PeAnnotation.Line.XAxisText(2 * k + 1) = k + 1
                End If
            Next
            If (CheckedListBox1.CheckedItems.Count > 0) Then
                If logmode Then
                    Pesgo1.PeGrid.Configure.YAxisScaleControl = Gigasoft.ProEssentials.Enums.ScaleControl.Log
                Else
                    Pesgo1.PeGrid.Configure.YAxisScaleControl = Gigasoft.ProEssentials.Enums.ScaleControl.Normal
                End If

                Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.XData, graficisullospettro_x, graficisullospettro_x.Length)
                Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.YData, graficisullospettro, graficisullospettro.Length)
                Pesgo1.Refresh()
            End If

        End If

    End Sub

    Public Function Rebin(data(,) As Double, new_spectrum_length As Integer)
        Dim rebin_factor = MaxSpectrumLength / new_spectrum_length
        Dim Rebinned_data(MaxNumberOfChannel, new_spectrum_length) As Double
        For n = 0 To MaxNumberOfChannel - 1
            Dim k = 0
            For i = 0 To (MaxSpectrumLength - rebin_factor) Step rebin_factor
                For j = 0 To rebin_factor - 1
                    Rebinned_data(n, k) += data(n, i + j)
                Next
                k += 1
            Next
        Next
        Return Rebinned_data
    End Function

    Public Sub UnpackDataPacket(ByRef data As UInt32(), ssize As Integer, packsize As Integer)

        Dim ev As New Evento(packsize)
        Dim insync = 0
        Dim pxcnt As Integer = 0
        Dim packid
        Dim mpe
        Dim discard As Boolean = False
        freq = 80000000
        For iiii = 0 To ssize - 1
            mpe = data(iiii)
            Select Case insync
                Case 0
                    If mpe = &HFFFFFFFF& Then
                        ev = New Evento(packsize)
                        For i = 0 To packsize - 1
                            ev.energy(i) = 0
                        Next
                        discard = False
                        insync = 1
                    End If
                Case 1
                    insync = 2
                    If mpe <> &H12345678& Then
                        insync = 0
                    End If
                Case 2
                    ev.timecode = mpe << 32
                    insync = 3
                Case 3
                    ev.timecode += mpe
                    insync = 4
                Case 4
                    insync = 5
                Case 5
                    insync = 6
                    ev.eventId = mpe
                Case 6
                    insync = 7
                Case 7
                    packid = mpe
                    insync = 8
                    pxcnt = 0
                Case 8
                    insync = 9
                    ev.ValidEvent = mpe
                Case 9
                    If mpe = &HFFFFFFFF& Then
                        ev = New Evento(packsize)
                        For i = 0 To packsize - 1
                            ev.energy(i) = 0
                        Next
                        insync = 1
                    Else
                        Dim uy As Integer = mpe And (&HFFFFFF)
                        If uy > &H800000 Then
                            uy = 0
                        End If
                        ev.energy(pxcnt) = uy
                        If (ev.ValidEvent >> pxcnt) And &H1 Then
                            MutexSpe.WaitOne()
                            spectra(pxcnt, ev.energy(pxcnt)) += 1
                            MutexSpe.ReleaseMutex()
                            MutexCumulative.WaitOne()
                            realtimeimage(pxcnt) = ev.energy(pxcnt)
                            integralimage(pxcnt) += ev.energy(pxcnt)
                            MutexCumulative.ReleaseMutex()
                        Else
                            realtimeimage(pxcnt) = 0
                        End If
                        pxcnt += 1
                        If (pxcnt = packsize) Then
                            MutexFile.WaitOne()
                            If fileEnable Then
                                Try
                                    Dim a As New Evento(packsize - 2)
                                    For i = 0 To packsize - 1
                                        If EnabledChannel(i) Then
                                            If i < ev.energy.Length Then
                                                a.energy(i) = ev.energy(EnabledChannel_id(i) - 1)
                                            End If
                                        End If
                                    Next
                                    a.timecode = ev.timecode
                                    a.size = ev.size
                                    a.eventId = ev.eventId
                                    a.valid = True
                                    If spectracount = 0 Then
                                        T0 = a.timecode
                                        T = 0
                                    Else
                                        T = Math.Abs(a.timecode - T0)
                                    End If
                                    spectracount += 1
                                    If IsRunning = True Then
                                        objRawWriter.WriteLine(a.eventId & ";" & a.timecode.ToString & ";" & String.Join(";", a.energy).Replace(",", "."))
                                    End If
                                Catch ex As Exception

                                End Try
                            End If
                            MutexFile.ReleaseMutex()
                            insync = 0
                        End If
                    End If
            End Select
        Next

    End Sub


    Public Sub UnpackDataPacketR(ByRef data As Queue(Of UInt32), cp As Integer)

        Dim packetsize = 5
        Dim ev As Evento
        Dim insync = 0
        Dim pxcnt As Integer = 0
        Dim packid
        Dim mpe
        Dim discard As Boolean = False
        freq = 125000000
        If (data.Count < packetsize) Then
            Return
        End If

        While data.Count > 0
            mpe = data.Dequeue

            Select Case insync
                Case 0
                    If mpe = &H80000000& Then
                        ev = New Evento(1)

                        ev.energy(0) = 0
                        discard = False
                        insync = 1
                    Else
                        If (data.Count < packetsize) Then
                            Return
                        End If
                    End If
                Case 1
                    Dim eee = mpe
                    IIf(eee < 0, 0, eee)
                    IIf(eee > 65535, 65535, eee)
                    ev.energy(0) = eee

                    insync = 2
                Case 2
                    ev.timecode = mpe
                    insync = 3
                Case 3
                    ev.timecode += mpe << 32L
                    insync = 4
                Case 4
                    Dim trigger_id As UInt32 = mpe

                    trigger_id = trigger_id >> 24
                    ev.SingleChannelTriggerId = trigger_id

                    If (ev.energy(0) < 65535) Then
                        ev.valid = True
                    Else
                        ev.energy(0) = 0
                        ev.valid = False
                    End If

                    If (trigger_id < 32) Then
                        MutexSpe.WaitOne()
                        spectra(trigger_id + (cp * _n_ch), ev.energy(0)) += 1
                        MutexSpe.ReleaseMutex()
                        MutexCumulative.WaitOne()
                        realtimeimage(trigger_id + (cp * _n_ch)) = ev.energy(0)
                        integralimage(trigger_id + (cp * _n_ch)) += ev.energy(0)
                        T = ev.timecode
                        MutexCumulative.ReleaseMutex()
                    Else

                    End If

                    If fileEnable Then
                        spectracount += 1
                        Try
                            If Array.IndexOf(EnabledChannel_id, trigger_id + (cp * _n_ch)) Then
                                If IsRunning = True Then
                                    objRawWriter.WriteLine(trigger_id + (cp * _n_ch) & ";" & ev.timecode.ToString & ";" & String.Join(";", ev.energy(0)).Replace(",", "."))
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                    End If



                    insync = 0

                    If (data.Count < packetsize) Then
                        Return
                    End If



            End Select

        End While


    End Sub



    Public Sub UnpackDataPacketS(ByRef data As Queue(Of UInt32), cp As Integer)
        Static last_timecodes() As Int64 = {0, 0}
        Dim packetsize = 5
        Dim ev As Evento
        Dim insync = 0
        Dim pxcnt As Integer = 0
        Dim packid
        Dim mpe
        Dim discard As Boolean = False
        freq = 60000000
        If (data.Count < packetsize) Then
            Return
        End If

        While data.Count > 0
            mpe = data.Dequeue

            Select Case insync
                Case 0
                    If mpe = &HFFFFFFFF& Then
                        ev = New Evento(1)

                        ev.energy(0) = 0
                        discard = False
                        insync = 1
                    Else
                        If (data.Count < packetsize) Then
                            Return
                        End If
                    End If
                Case 1
                    Dim eee = mpe
                    IIf(eee < 0, 0, eee)
                    IIf(eee > 65535, 65535, eee)
                    ev.energy(0) = eee

                    insync = 2
                Case 2
                    ev.timecode = mpe
                    insync = 3
                Case 3
                    ev.timecode += mpe << 32L
                    insync = 4
                Case 4
                    Dim trigger_id As UInt32 = mpe

                    trigger_id = trigger_id >> 24
                    ev.SingleChannelTriggerId = trigger_id

                    If (ev.energy(0) < 65535) Then
                        ev.valid = True
                    Else
                        ev.energy(0) = 0
                        ev.valid = False
                    End If

                    If (trigger_id < 2) Then
                        Dim b = MainForm.acquisition.CHList(trigger_id).pileup_enable
                        Dim t = MainForm.acquisition.CHList(trigger_id).pileup_time / (1000000000 / freq)
                        Dim pileup As Boolean = False
                        If b Then
                            If ev.timecode - last_timecodes(trigger_id) < t Then
                                pileup = True
                            End If
                        End If
                        last_timecodes(trigger_id) = ev.timecode
                        If pileup = False Then
                            MutexSpe.WaitOne()
                            spectra(trigger_id + (cp * _n_ch), ev.energy(0)) += 1
                            MutexSpe.ReleaseMutex()
                            MutexCumulative.WaitOne()
                            realtimeimage(trigger_id + (cp * _n_ch)) = ev.energy(0)
                            integralimage(trigger_id + (cp * _n_ch)) += ev.energy(0)
                            t = ev.timecode
                            MutexCumulative.ReleaseMutex()
                        End If
                    End If

                    If fileEnable Then
                        spectracount += 1
                        Try
                            If Array.IndexOf(EnabledChannel_id, trigger_id + (cp * _n_ch)) Then
                                If isRunning = True Then
                                    objRawWriter.WriteLine(trigger_id + (cp * _n_ch) & ";" & ev.timecode.ToString & ";" & String.Join(";", ev.energy(0)).Replace(",", "."))
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                    End If



                    insync = 0

                    If (data.Count < packetsize) Then
                        Return
                    End If



            End Select

        End While


    End Sub
    'End Sub
    'Public Sub UnpackDataPacketR(ByRef data As UInt32(), ssize As Integer, cp As Integer)

    '    Dim ev As Evento
    '    Dim insync = 0
    '    Dim pxcnt As Integer = 0
    '    Dim packid
    '    Dim mpe
    '    Dim discard As Boolean = False
    '    For iiii = 0 To ssize - 1
    '        mpe = data(iiii)
    '        Select Case insync
    '            Case 0
    '                If mpe = &H80000000& Then
    '                    ev = New Evento(1)

    '                    ev.energy(0) = 0
    '                    discard = False
    '                    insync = 1
    '                End If
    '            Case 1
    '                ev.energy(0) = mpe

    '                insync = 2
    '            Case 2
    '                ev.timecode = mpe
    '                insync = 3
    '            Case 3
    '                ev.timecode += mpe << 32L
    '                insync = 4
    '            Case 4
    '                Dim trigger_id As UInt32 = mpe

    '                trigger_id = trigger_id >> 24
    '                ev.SingleChannelTriggerId = trigger_id

    '                If (ev.energy(0) < 65535) Then
    '                    ev.valid = True
    '                Else
    '                    ev.energy(0) = 0
    '                    ev.valid = False
    '                End If
    '                MutexSpe.WaitOne()
    '                spectra(trigger_id + (cp * _n_ch), ev.energy(0)) += 1
    '                MutexSpe.ReleaseMutex()
    '                MutexCumulative.WaitOne()
    '                realtimeimage(trigger_id + (cp * _n_ch)) = ev.energy(0)
    '                integralimage(trigger_id + (cp * _n_ch)) += ev.energy(0)


    '                If fileEnable Then
    '                    spectracount += 1
    '                    Try
    '                        If Array.IndexOf(EnabledChannel_id, trigger_id + (cp * _n_ch)) Then
    '                            If (TargetMode = 1 And spectracount <= TargetEvent) Or (TargetMode = 2 And (T / freq <= TargetEvent)) Or TargetMode = 0 Then
    '                                objRawWriter.WriteLine(trigger_id + (cp * _n_ch) & ";" & ev.timecode.ToString & ";" & String.Join(";", ev.energy(0)).Replace(",", "."))
    '                            End If
    '                        End If
    '                    Catch ex As Exception

    '                    End Try
    '                End If

    '                MutexCumulative.ReleaseMutex()

    '                insync = 0


    '        End Select
    '    Next

    'End Sub





    Private Sub DisegnaGrafico(ByRef grafico As Pesgo, Gtyle As SGraphPlottingMethod)

        Dim p As Integer
        grafico.PePlot.Option.BarGlassEffect = False
        grafico.PePlot.Option.AreaGradientStyle = PlotGradientStyle.NoGradient
        grafico.PePlot.Option.AreaBevelStyle = BevelStyle.None
        grafico.PePlot.Option.SplineGradientStyle = PlotGradientStyle.NoGradient
        grafico.PePlot.Option.SplineBevelStyle = SplineBevelStyle.None
        grafico.PeConfigure.PrepareImages = True
        grafico.PeData.Subsets = 1
        grafico.PeData.Points = 65536

        For p = 0 To 65535
            grafico.PeData.X(0, p) = p
            grafico.PeData.Y(0, p) = 0
        Next p

        grafico.PePlot.DataShadows = DataShadows.None
        grafico.PeUserInterface.Allow.FocalRect = False
        grafico.PeGrid.LineControl = GridLineControl.Both
        grafico.PeGrid.Style = GridStyle.Dot
        grafico.PeString.MainTitle = "Real Time Spectra"
        grafico.PeString.SubTitle = ""
        grafico.PeString.YAxisLabel = ""
        grafico.PeString.XAxisLabel = "(channels)"
        grafico.PeColor.SubsetColors(0) = Color.FromArgb(255, 255, 0, 0)
        grafico.PeLegend.SubsetLineTypes(0) = LineType.ThinSolid
        grafico.PeLegend.SimpleLine = True
        grafico.PeLegend.Style = LegendStyle.OneLine
        grafico.PeGrid.Option.MultiAxisStyle = MultiAxisStyle.SeparateAxes
        grafico.PeFont.Fixed = True
        grafico.PePlot.Option.GradientBars = 8
        grafico.PeConfigure.TextShadows = TextShadows.BoldText
        grafico.PeFont.MainTitle.Bold = True
        grafico.PeFont.MainTitle.Font = "Microsoft Sans Serif"
        grafico.PeFont.SubTitle.Bold = True
        grafico.PeFont.Label.Bold = True
        grafico.PeFont.FontSize = FontSize.Large
        grafico.PeData.Precision = DataPrecision.OneDecimal
        grafico.PeSpecial.DpiX = 600
        grafico.PeSpecial.DpiY = 600
        grafico.PeColor.BitmapGradientMode = True
        grafico.PeColor.QuickStyle = QuickStyle.DarkNoBorder
        grafico.PeGrid.Option.YAxisLongTicks = True
        grafico.PePlot.Method = Gtyle
        grafico.PeUserInterface.Allow.Zooming = AllowZooming.Horizontal
        grafico.PeUserInterface.Dialog.PlotCustomization = False
        grafico.PeData.SpeedBoost = 10
        grafico.PeGrid.Configure.AutoMinMaxPadding = 1
        grafico.PeData.AutoScaleData = False
        grafico.PePlot.Option.LineShadows = False
        grafico.PeConfigure.RenderEngine = RenderEngine.GdiTurbo
        grafico.PeConfigure.AntiAliasGraphics = False
        grafico.PeConfigure.AntiAliasText = True
        grafico.PeAnnotation.Line.TextSize = 80
        grafico.PeAnnotation.Show = True
        grafico.PeUserInterface.Allow.ZoomStyle = ZoomStyle.Ro2Not
        grafico.PeUserInterface.Cursor.Mode = CursorMode.FloatingXY
        grafico.PeUserInterface.Cursor.PromptTracking = True
        grafico.PeUserInterface.Cursor.PromptStyle = CursorPromptStyle.XYValues
        grafico.PeUserInterface.HotSpot.Data = True
        grafico.PeUserInterface.Cursor.MouseCursorControl = True
        grafico.PeUserInterface.Scrollbar.MouseDraggingX = True
        grafico.PeUserInterface.Scrollbar.MouseDraggingY = True
        grafico.PeUserInterface.Scrollbar.ScrollingHorzZoom = True
        grafico.PeUserInterface.Scrollbar.ScrollingVertZoom = True
        grafico.PePlot.ZoomWindow.Show = True
        grafico.PeData.NullDataValue = -10000000000
        grafico.Refresh()
        grafico.PeFunction.ReinitializeResetImage()

    End Sub

    Private Sub Pesgo1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Pesgo1.KeyPress

        If e.KeyChar = "u" Or e.KeyChar = "U" Then
            Pesgo1.PeFunction.UndoZoom()
        End If

        If e.KeyChar = "v" Or e.KeyChar = "V" Then
            Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.Vertical
        End If

        If e.KeyChar = "h" Or e.KeyChar = "H" Then
            Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.Horizontal
        End If

        If e.KeyChar = "z" Or e.KeyChar = "Z" Then
            Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.HorzAndVert
        End If

        If e.KeyChar = "o" Or e.KeyChar = "O" Then
            plotS = plotS + 1
            If plotS = 9 Then
                plotS = 0
            End If
            Select Case plotS
                Case 0
                    method = SGraphPlottingMethods.Step
                Case 1
                    method = SGraphPlottingMethods.Line
                Case 2
                    method = SGraphPlottingMethods.Spline
                Case 3
                    method = SGraphPlottingMethods.Bar
                Case 4
                    method = SGraphPlottingMethods.Area
                Case 5
                    method = SGraphPlottingMethods.SplineArea
                Case 6
                    method = SGraphPlottingMethods.Point
                Case 7
                    method = SGraphPlottingMethods.PointsPlusLine
                Case 8
                    method = SGraphPlottingMethods.PointsPlusSpline
            End Select
        End If

        If e.KeyChar = "l" Or e.KeyChar = "L" Then
            logmode = Not logmode
            If logmode Then
                Pesgo1.PeGrid.Configure.YAxisScaleControl = Gigasoft.ProEssentials.Enums.ScaleControl.Log
            End If
        End If

        If e.KeyChar = "c" Or e.KeyChar = "C" Then
            '   MainForm.fit.DataGridView1.Rows(1).Cells("Cursor 1").Value = Pesgo1.PeUserInterface.Cursor.d
        End If

        Pesgo1.Refresh()

    End Sub

    Public Sub FlushFifo()
        For cp = 0 To Connection.ComClass._nBoard - 1
            If Connection.ComClass.SetRegister(addressArm, 2, cp) = 0 Then
            Else
                MainForm.plog.TextBox1.AppendText("Error on CONFIG_ARM!" & vbCrLf)
            End If
        Next

        For cp = 0 To Connection.ComClass._nBoard - 1
            If Connection.ComClass.SetRegister(addressArm, 1, cp) = 0 Then

            Else
                MainForm.plog.TextBox1.AppendText("Error on CONFIG_ARM!" & vbCrLf)
            End If
        Next
    End Sub

    Private Sub Pesgo1_Click(sender As Object, e As EventArgs) Handles Pesgo1.Click

    End Sub
End Class
