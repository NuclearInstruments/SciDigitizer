Imports System.IO
Imports System.Threading
Imports System.ComponentModel
Imports Gigasoft.ProEssentials
Imports Gigasoft.ProEssentials.Enums

Public Class pOscilloscope

    Public running As Boolean = False
    Dim inhibit = True
    Dim osc_ch As Integer
    Public addressData As UInt32
    Public addressDecimator As UInt32
    Public addressPre As UInt32
    Public addressMode As UInt32
    Public addressLevel As UInt32
    Public addressArm As UInt32
    Public addressStatus As UInt32
    Public addressPosition As UInt32
    Dim nsamples As UInt32
    Dim tot_points As Integer
    Dim length As Integer
    Dim position As UInt32
    Public wavecount = 0
    Dim DecimatorValue As Double
    Dim PreTriggerValue As Double
    Dim LevelTriggerValue As Double
    Dim TriggerModeValue As UInt32
    Public EnabledChannel() As Boolean
    Public EnabledChannel_id() As Integer
    Public TargetEvent As UInt32
    Public TargetMode As Integer
    Public fileEnable As Boolean = False
    Public fileName As String
    Dim ChList_name As New List(Of String)
    Dim Checked_id As New List(Of Integer)
    Dim MutexFile As New Mutex
    Dim objRawWriter As StreamWriter
    Dim totalACQ As Integer = 0
    Dim plotS As Integer = 0
    Dim colorList() As Color = {Color.Red, Color.Yellow, Color.Lime, Color.Cyan, Color.Magenta, Color.Blue, Color.BlueViolet, Color.Violet, Color.Peru, Color.Orange, Color.White,
                                Color.DarkRed, Color.Gold, Color.DarkGreen, Color.Teal, Color.HotPink, Color.RoyalBlue, Color.Purple, Color.Sienna, Color.Chocolate, Color.LightSlateGray,
                                Color.Tomato, Color.Moccasin, Color.PaleGreen, Color.PaleTurquoise, Color.Plum, Color.DeepSkyBlue, Color.MediumVioletRed, Color.RosyBrown, Color.LightSalmon, Color.Silver, Color.Olive}

    Private Sub pScope_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        addressData = MainForm.CurrentOscilloscope.Address
        nsamples = MainForm.CurrentOscilloscope.nsamples
        tot_points = 5 * nsamples
        osc_ch = MainForm.acquisition.CHList.Count
        length = nsamples * osc_ch
        pScope_ReLoad()
        DisegnaGrafico()

    End Sub


    Public Sub pScope_ReLoad()

        CheckedListBox1.Items.Clear()
        ChList_name.Clear()
        ReDim EnabledChannel(osc_ch - 1)
        ReDim EnabledChannel_id(osc_ch - 1)
        CheckedListBox1.Items.Add("ALL", False)
        For i = 0 To osc_ch - 1
            ChList_name.Add(MainForm.acquisition.CHList(i).name)
            CheckedListBox1.Items.Add(ChList_name(i), MainForm.acquisition.CHList(i).scope_checked)
            EnabledChannel(i) = False
        Next

    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged

        Checked_id.Clear()
        If CheckedListBox1.SelectedIndex = 0 Then
            Dim state = IIf(CheckedListBox1.GetItemCheckState(0).ToString = "Checked", True, False)
            For i = 1 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemChecked(i, state)
                MainForm.acquisition.CHList(i - 1).scope_checked = state
            Next
        Else
            MainForm.acquisition.CHList(CheckedListBox1.SelectedIndex - 1).scope_checked = IIf(CheckedListBox1.GetItemCheckState(CheckedListBox1.SelectedIndex).ToString = "Checked", True, False)
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
            If scope_ch.scope_checked Then
                Checked_id.Add(scope_ch.id)
            End If
        Next

    End Sub

    Public Sub DisegnaGrafico()

        Pesgo1.PeData.Subsets = 5
        Pesgo1.PeData.Points = nsamples

        Pesgo1.PeGrid.MultiAxesSubsets(0) = 1
        Pesgo1.PeGrid.MultiAxesSubsets(1) = 1
        Pesgo1.PeGrid.MultiAxesSubsets(2) = 1
        Pesgo1.PeGrid.MultiAxesSubsets(3) = 1
        Pesgo1.PeGrid.MultiAxesSubsets(4) = 1

        Pesgo1.PeUserInterface.Allow.MultiAxesSizing = True
        Pesgo1.PeGrid.MultiAxesProportions(0) = 0.8
        Pesgo1.PeGrid.MultiAxesProportions(1) = 0.05
        Pesgo1.PeGrid.MultiAxesProportions(2) = 0.05
        Pesgo1.PeGrid.MultiAxesProportions(3) = 0.05
        Pesgo1.PeGrid.MultiAxesProportions(4) = 0.05

        Pesgo1.PeLegend.Show = False
        Pesgo1.PeLegend.SimpleLine = True
        Pesgo1.PeLegend.Style = LegendStyle.OneLine
        Pesgo1.PeLegend.SubsetsToLegend(0) = 0
        Pesgo1.PeConfigure.CacheBmp = True
        Pesgo1.PeConfigure.PrepareImages = True
        Pesgo1.PeColor.BitmapGradientMode = False

        Pesgo1.PeColor.QuickStyle = QuickStyle.DarkNoBorder
        Pesgo1.PeConfigure.AntiAliasGraphics = False
        Pesgo1.PeFont.Fixed = True
        Pesgo1.PePlot.Option.BarGlassEffect = False
        Pesgo1.PePlot.Option.AreaGradientStyle = PlotGradientStyle.NoGradient
        Pesgo1.PePlot.Option.AreaBevelStyle = BevelStyle.None
        Pesgo1.PePlot.Option.SplineGradientStyle = PlotGradientStyle.NoGradient
        Pesgo1.PePlot.Option.SplineBevelStyle = SplineBevelStyle.None
        Pesgo1.PeGrid.LineControl = GridLineControl.Both
        Pesgo1.PeGrid.Style = GridStyle.Dot
        Pesgo1.PeString.MainTitle = "Real Time Oscilloscope"
        Pesgo1.PeString.SubTitle = ""
        Pesgo1.PeString.YAxisLabel = ""
        Pesgo1.PeString.XAxisLabel = "Time (ns)"
        Pesgo1.PeFont.MainTitle.Bold = True
        Pesgo1.PeFont.MainTitle.Font = "Microsoft Sans Serif"
        Pesgo1.PeFont.SubTitle.Bold = True
        Pesgo1.PeFont.Label.Bold = True
        Pesgo1.PeFont.FontSize = FontSize.Large
        Pesgo1.PeUserInterface.Cursor.PromptTracking = True
        Pesgo1.PeUserInterface.Cursor.PromptStyle = CursorPromptStyle.XYValues
        Pesgo1.PeUserInterface.HotSpot.Data = True
        Pesgo1.PePlot.DataShadows = DataShadows.None
        Pesgo1.PePlot.SubsetLineTypes(0) = LineType.ThickSolid
        Pesgo1.PePlot.SubsetLineTypes(1) = LineType.ThickSolid
        Pesgo1.PePlot.SubsetLineTypes(2) = LineType.ThickSolid
        Pesgo1.PePlot.SubsetLineTypes(3) = LineType.ThickSolid
        Pesgo1.PePlot.SubsetLineTypes(4) = LineType.ThickSolid
        Pesgo1.PeColor.SubsetColors(0) = Color.Red
        Pesgo1.PeColor.SubsetColors(1) = Color.Red
        Pesgo1.PeColor.SubsetColors(2) = Color.Red
        Pesgo1.PeColor.SubsetColors(3) = Color.Red
        Pesgo1.PeColor.SubsetColors(4) = Color.Red
        Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.Horizontal
        Pesgo1.PeSpecial.DpiX = 600
        Pesgo1.PeSpecial.DpiY = 600
        Pesgo1.PePlot.Option.NullDataGaps = False
        Pesgo1.PeData.Filter2D = Filter2D.Fastest
        Pesgo1.PeAnnotation.Show = True
        ' Pesgo1.PeAnnotation.Line.RightMargin = "XXXXXXXXXXXX"
        Pesgo1.PeConfigure.ImageAdjustLeft = 100
        Pesgo1.PeSpecial.AutoImageReset = False '  // important For Direct3D rendering 
        Pesgo1.PeUserInterface.Allow.ZoomStyle = ZoomStyle.FramedRect
        Pesgo1.PeUserInterface.Cursor.Mode = CursorMode.FloatingXY
        If (Pesgo1.IsDxAvailable) Then
            Pesgo1.PeConfigure.RenderEngine = RenderEngine.Direct3D
        Else
            Pesgo1.PeConfigure.RenderEngine = RenderEngine.GdiTurbo
        End If
        Pesgo1.PeConfigure.Composite2D3D = Composite2D3D.Foreground
        Pesgo1.PeData.NullDataValue = Double.MinValue
        Pesgo1.PeGrid.WorkingAxis = 0
        Pesgo1.PeString.YAxisLabel = "ANALOG"


        If Connection.CustomFirmware Then
            Pesgo1.PeString.YAxisLabel = "Digital 0"
        Else
            Pesgo1.PeString.YAxisLabel = "Integration Gate"
        End If
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        Pesgo1.PeGrid.Configure.ManualMaxY = 1.1
        Pesgo1.PeGrid.WorkingAxis = 2
        If Connection.CustomFirmware Then
            Pesgo1.PeString.YAxisLabel = "Digital 1"
        Else
            Pesgo1.PeString.YAxisLabel = "Baseline gate"
        End If
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        Pesgo1.PeGrid.Configure.ManualMaxY = 1.1
        Pesgo1.PeGrid.WorkingAxis = 3
        If Connection.CustomFirmware Then
            Pesgo1.PeString.YAxisLabel = "Digital 2"
        Else
            Pesgo1.PeString.YAxisLabel = "Trigger"
        End If
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        Pesgo1.PeGrid.Configure.ManualMaxY = 1.1
        Pesgo1.PeGrid.WorkingAxis = 4
        If Connection.CustomFirmware Then
            Pesgo1.PeString.YAxisLabel = "Digital 3"
        Else
            Pesgo1.PeString.YAxisLabel = "Pile Up"
        End If
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

        Pesgo1.PeGrid.WorkingAxis = 0

        Dim tmpXData(nsamples * 5) As Single
        For i = 0 To nsamples - 1
            tmpXData(i) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * 1000 / 80
            tmpXData(i + (nsamples * 1)) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * 1000 / 80
            tmpXData(i + (nsamples * 2)) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * 1000 / 80
            tmpXData(i + (nsamples * 3)) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * 1000 / 80
            tmpXData(i + (nsamples * 4)) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * 1000 / 80
        Next
        Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.XData, tmpXData, tot_points)

        Dim y1(nsamples), y2(nsamples), y3(nsamples), y4(nsamples), y5(nsamples) As Single
        Dim tmpYData2(nsamples * 5) As Single
        For i = 0 To nsamples - 1
            y1(i) = 0
            y2(i) = 0
            y3(i) = 0
            y4(i) = 0
            y5(i) = 0
        Next
        Array.Copy(y1, 0, tmpYData2, 0, nsamples)
        Array.Copy(y2, 0, tmpYData2, nsamples, nsamples)
        Array.Copy(y3, 0, tmpYData2, nsamples * 2, nsamples)
        Array.Copy(y4, 0, tmpYData2, nsamples * 3, nsamples)
        Array.Copy(y5, 0, tmpYData2, nsamples * 4, nsamples)

        Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.YData, tmpYData2, tot_points)

        Pesgo1.PeFunction.Force3dxVerticeRebuild = True
        Pesgo1.PeFunction.Force3dxNewColors = True
        Pesgo1.PeFunction.ReinitializeResetImage()


        Pesgo1.PeGrid.WorkingAxis = 0
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        Pesgo1.PeGrid.Configure.ManualMaxY = 16384
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
        Pesgo1.Invalidate()
        Pesgo1.PeFunction.ReinitializeResetImage()

    End Sub

    Public Sub StartDataCaptureOnFile(file As String)
        totalACQ = 0
        MutexFile.WaitOne()
        If fileEnable = False Then
            fileName = file
            fileEnable = True
            objRawWriter = New StreamWriter(fileName)
            MainForm.plog.TextBox1.AppendText("Starting Oscilloscope Waveform Recording..." & vbCrLf)
        End If
        MutexFile.ReleaseMutex()

    End Sub

    Public Sub StopDataCaptureOnFile()

        MutexFile.WaitOne()
        fileEnable = False
        If IsNothing(objRawWriter) Then
        Else
            objRawWriter.Close()
            MainForm.plog.TextBox1.AppendText("Stopping Oscilloscope Waveform Recording." & vbCrLf)
        End If
        MutexFile.ReleaseMutex()

    End Sub


    Public Sub StartFreeRunningMultiAcquisition()

        Pesgo1.PeLegend.Show = True
        copyOscilloscopeParam()
        setOscilloscopeParam()
        Timer1.Enabled = True
        running = True
        'MainForm.sets.Apply.Enabled = False
        MainForm.plog.TextBox1.AppendText("Starting Oscilloscope Free Running Acquisition..." & vbCrLf)

    End Sub

    Public Sub SingleShot()

        Pesgo1.PeLegend.Show = True
        copyOscilloscopeParam()
        setOscilloscopeParam()
        MainForm.plog.TextBox1.AppendText("Oscilloscope Single Shot Acquisition!" & vbCrLf)
        SingleShotA()

    End Sub

    Public Sub StopAcquisition()

        Timer1.Enabled = False
        running = False
        MainForm.sets.Apply.Enabled = True
        MainForm.plog.TextBox1.AppendText("Stopping Oscilloscope Free Running Acquisition." & vbCrLf)

    End Sub

    Public Sub copyOscilloscopeParam()

        DecimatorValue = MainForm.acquisition.General_settings.OscilloscopeDecimator
        PreTriggerValue = MainForm.acquisition.General_settings.OscilloscopePreTrigger
        LevelTriggerValue = MainForm.acquisition.General_settings.TriggerOscilloscopeLevel
        If MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.EXTERNAL Then
            TriggerModeValue = Convert.ToInt32(MainForm.acquisition.General_settings.TriggerOscilloscopeEdges & "000", 2)

        ElseIf MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.FREE Then
            TriggerModeValue = Convert.ToInt32("1000" & MainForm.acquisition.General_settings.TriggerOscilloscopeEdges & "010", 2)
        ElseIf MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.INTERNAL Then
            ' Dim wword As UInt32
            ' wword = ((MainForm.acquisition.General_settings.TriggerChannelOscilloscope) << 8) + (MainForm.acquisition.General_settings.TriggerOscilloscopeEdges << 3) + 6
            TriggerModeValue = Convert.ToInt32(MainForm.acquisition.General_settings.TriggerOscilloscopeEdges & "000", 2)
        Else
            Dim wword As UInt32
            wword = ((MainForm.acquisition.General_settings.TriggerChannelOscilloscope) << 8) + (MainForm.acquisition.General_settings.TriggerOscilloscopeEdges << 3) + 1
            TriggerModeValue = wword
        End If

    End Sub

    Public Sub setOscilloscopeParam()

        If Connection.ComClass.SetRegister(addressDecimator, DecimatorValue - 1) = 0 Then
            If Connection.ComClass.SetRegister(addressPre, Math.Floor(PreTriggerValue * nsamples / 100)) = 0 Then
                If Connection.ComClass.SetRegister(addressMode, TriggerModeValue) = 0 Then
                    If Connection.ComClass.SetRegister(addressLevel, LevelTriggerValue) = 0 Then
                        If Connection.ComClass.SetRegister(addressArm, 0) = 0 Then
                            If Connection.ComClass.SetRegister(addressArm, 1) = 0 Then
                            Else
                                MainForm.plog.TextBox1.AppendText("Error on CONFIG_ARM!" & vbCrLf)
                            End If
                        Else
                            MainForm.plog.TextBox1.AppendText("Error on CONFIG_ARM!" & vbCrLf)
                        End If
                    Else
                        MainForm.plog.TextBox1.AppendText("Error on CONFIG_TRIGGER_LEVEL!" & vbCrLf)
                    End If
                Else
                    MainForm.plog.TextBox1.AppendText("Error on CONFIG_TRIGGER_MODE!" & vbCrLf)
                End If
            Else
                MainForm.plog.TextBox1.AppendText("Error on CONFIG_PRETRIGGER!" & vbCrLf)
            End If
        Else
            MainForm.plog.TextBox1.AppendText("Error on CONFIG_DECIMATOR!" & vbCrLf)
        End If

    End Sub

    Public Sub SingleShotA()

        Dim offsetLSB = (MainForm.acquisition.General_settings.AFEOffset + 2) / 4 * (4095 - 1650) + 1650
        Dim jjj As New ClassCalibration() 'My.Settings.AFECalibration)
        Dim coor = jjj.GetCorrectionFactors(offsetLSB)
        copyOscilloscopeParam()
        setOscilloscopeParam()
        Pesgo1.PeLegend.SubsetsToLegend.Clear()
        Dim n_ch As Integer
        If CheckedListBox1.CheckedItems.Contains("ALL") Then
            n_ch = osc_ch
        Else
            n_ch = CheckedListBox1.CheckedIndices.Count
        End If

        Dim status As UInt32 = 0
        Dim tt = Now
        While status <> 1
            Connection.ComClass.GetRegister(addressStatus, status)
            Application.DoEvents()
            If MainForm.__Running_OSC = False Then
                Exit Sub
            End If
            If (Now - tt).TotalMilliseconds > 2000 Then
                Exit Sub
            End If
        End While

        Dim position As UInt32
        Connection.ComClass.GetRegister(addressPosition, position)

        Dim data(length) As UInt32
        Dim read_data As UInt32
        Dim valid_data As UInt32
        If Connection.ComClass.ReadData(addressData, data, length, 0, 1000, read_data, valid_data) = 0 Then

            If n_ch > 0 Then
                Pesgo1.PeData.Subsets = 5 * n_ch
                Pesgo1.PeData.Points = nsamples
                Pesgo1.PeGrid.MultiAxesSubsets(0) = n_ch
                Pesgo1.PeGrid.MultiAxesSubsets(1) = n_ch
                Pesgo1.PeGrid.MultiAxesSubsets(2) = n_ch
                Pesgo1.PeGrid.MultiAxesSubsets(3) = n_ch
                Pesgo1.PeGrid.MultiAxesSubsets(4) = n_ch

                Dim TOTpoints = tot_points * n_ch
                Dim tmpYData2(TOTpoints) As Single
                Dim AnalogArray(nsamples * n_ch) As Single
                Dim Digital1Array(nsamples * n_ch) As Single
                Dim Digital2Array(nsamples * n_ch) As Single
                Dim Digital3Array(nsamples * n_ch) As Single
                Dim Digital4Array(nsamples * n_ch) As Single

                Dim tmpXData(TOTpoints) As Single
                For j = 0 To n_ch - 1
                    For i = 0 To nsamples - 1
                        tmpXData(i + (nsamples * (n_ch * 0 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * 1000 / 80
                        tmpXData(i + (nsamples * (n_ch * 1 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * 1000 / 80
                        tmpXData(i + (nsamples * (n_ch * 2 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * 1000 / 80
                        tmpXData(i + (nsamples * (n_ch * 3 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * 1000 / 80
                        tmpXData(i + (nsamples * (n_ch * 4 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * 1000 / 80
                    Next
                Next
                Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.XData, tmpXData, TOTpoints)

                Dim curr As Integer = position - Math.Floor(PreTriggerValue * nsamples / 100)
                Dim n = 0
                For Each ch_id In Checked_id
                    If ch_id <> 0 Then
                        Dim ch = ch_id - 1
                        If curr > 0 Then
                            Dim k = 0
                            For i = curr To nsamples - 2
                                AnalogArray(k + nsamples * n) = (data(i + nsamples * ch) And 65535) + coor(ch)
                                Digital1Array(k + nsamples * n) = data(i + nsamples * ch) >> 16 And 1
                                Digital2Array(k + nsamples * n) = data(i + nsamples * ch) >> 17 And 1
                                Digital3Array(k + nsamples * n) = data(i + nsamples * ch) >> 18 And 1
                                Digital4Array(k + nsamples * n) = data(i + nsamples * ch) >> 19 And 1
                                k += 1
                            Next
                            For i = 0 To curr - 1
                                AnalogArray(k + nsamples * n) = (data(i + nsamples * ch) And 65535) + coor(ch)
                                Digital1Array(k + nsamples * n) = data(i + nsamples * ch) >> 16 And 1
                                Digital2Array(k + nsamples * n) = data(i + nsamples * ch) >> 17 And 1
                                Digital3Array(k + nsamples * n) = data(i + nsamples * ch) >> 18 And 1
                                Digital4Array(k + nsamples * n) = data(i + nsamples * ch) >> 19 And 1
                                k += 1
                            Next
                        Else
                            Dim k = 0
                            For i = nsamples + curr To nsamples - 2
                                AnalogArray(k + nsamples * n) = (data(i + nsamples * ch) And 65535) + coor(ch)
                                Digital1Array(k + nsamples * n) = data(i + nsamples * ch) >> 16 And 1
                                Digital2Array(k + nsamples * n) = data(i + nsamples * ch) >> 17 And 1
                                Digital3Array(k + nsamples * n) = data(i + nsamples * ch) >> 18 And 1
                                Digital4Array(k + nsamples * n) = data(i + nsamples * ch) >> 19 And 1
                                k += 1
                            Next
                            For i = 0 To nsamples + curr - 1
                                AnalogArray(k + nsamples * n) = (data(i + nsamples * ch) And 65535) + coor(ch)
                                Digital1Array(k + nsamples * n) = data(i + nsamples * ch) >> 16 And 1
                                Digital2Array(k + nsamples * n) = data(i + nsamples * ch) >> 17 And 1
                                Digital3Array(k + nsamples * n) = data(i + nsamples * ch) >> 18 And 1
                                Digital4Array(k + nsamples * n) = data(i + nsamples * ch) >> 19 And 1
                                k += 1
                            Next
                        End If

                        Pesgo1.PeColor.SubsetColors(n) = colorList(n Mod 32)
                        Pesgo1.PeColor.SubsetColors(n_ch * 1 + n) = colorList(n Mod 32)
                        Pesgo1.PeColor.SubsetColors(n_ch * 2 + n) = colorList(n Mod 32)
                        Pesgo1.PeColor.SubsetColors(n_ch * 3 + n) = colorList(n Mod 32)
                        Pesgo1.PeColor.SubsetColors(n_ch * 4 + n) = colorList(n Mod 32)
                        Pesgo1.PePlot.SubsetLineTypes(n) = LineType.ThickSolid
                        Pesgo1.PePlot.SubsetLineTypes(n_ch * 1 + n) = LineType.ThickSolid
                        Pesgo1.PePlot.SubsetLineTypes(n_ch * 2 + n) = LineType.ThickSolid
                        Pesgo1.PePlot.SubsetLineTypes(n_ch * 3 + n) = LineType.ThickSolid
                        Pesgo1.PePlot.SubsetLineTypes(n_ch * 4 + n) = LineType.ThickSolid
                        If CheckedListBox1.CheckedItems.Contains("ALL") Then
                            Pesgo1.PeString.SubsetLabels(n) = CheckedListBox1.CheckedItems(n + 1)
                        Else
                            Pesgo1.PeString.SubsetLabels(n) = CheckedListBox1.CheckedItems(n)
                        End If
                        Pesgo1.PeString.SubsetLabels(n_ch * 1 + n) = ""
                        Pesgo1.PeString.SubsetLabels(n_ch * 2 + n) = ""
                        Pesgo1.PeString.SubsetLabels(n_ch * 3 + n) = ""
                        Pesgo1.PeString.SubsetLabels(n_ch * 4 + n) = ""
                        Pesgo1.PeLegend.SubsetsToLegend(n) = n

                        n += 1
                    End If
                Next
                For i = 1 To n_ch
                    For q = 0 To 4
                        AnalogArray(nsamples * i - q) = AnalogArray(nsamples * i - 4)
                        Digital1Array(nsamples * i - q) = Digital1Array(nsamples * i - 4)
                        Digital2Array(nsamples * i - q) = Digital2Array(nsamples * i - 4)
                        Digital3Array(nsamples * i - q) = Digital3Array(nsamples * i - 4)
                        Digital4Array(nsamples * i - q) = Digital4Array(nsamples * i - 4)
                    Next

                Next

                Array.Copy(AnalogArray, 0, tmpYData2, 0, nsamples * n_ch)
                Array.Copy(Digital1Array, 0, tmpYData2, nsamples * (n_ch * 1), nsamples * n_ch)
                Array.Copy(Digital2Array, 0, tmpYData2, nsamples * (n_ch * 2), nsamples * n_ch)
                Array.Copy(Digital3Array, 0, tmpYData2, nsamples * (n_ch * 3), nsamples * n_ch)
                Array.Copy(Digital4Array, 0, tmpYData2, nsamples * (n_ch * 4), nsamples * n_ch)



                Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.YData, tmpYData2, TOTpoints)

                If (Pesgo1.PeConfigure.RenderEngine = RenderEngine.Direct3D) Then
                    Pesgo1.PeFunction.Force3dxVerticeRebuild = True
                    Pesgo1.PeFunction.Force3dxNewColors = True
                Else
                    Pesgo1.PeFunction.Reinitialize()
                    Pesgo1.PeFunction.ResetImage(0, 0)
                End If
                totalACQ += 1
                Pesgo1.PeString.MainTitle = "Real Time Oscilloscope (" & totalACQ & ")"
                Pesgo1.Invalidate()
                Pesgo1.PeFunction.ReinitializeResetImage()
            End If

            wavecount += 1

            If fileEnable = True Then
                For k = 0 To osc_ch - 1
                    If EnabledChannel(k) Then
                        Dim A(nsamples), D0(nsamples), D1(nsamples), D2(nsamples), D3(nsamples) As Single
                        For j = 0 To nsamples - 1
                            A(j) = data(j + nsamples * (EnabledChannel_id(k) - 1)) And 65535
                            D0(j) = data(j + nsamples * (EnabledChannel_id(k) - 1)) >> 16 And 1
                            D1(j) = data(j + nsamples * (EnabledChannel_id(k) - 1)) >> 17 And 1
                            D2(j) = data(j + nsamples * (EnabledChannel_id(k) - 1)) >> 18 And 1
                            D3(j) = data(j + nsamples * (EnabledChannel_id(k) - 1)) >> 19 And 1
                        Next
                        MutexFile.WaitOne()
                        objRawWriter.WriteLine(wavecount & ";" & k + 1 & ";" & nsamples & ";" & 5 & ";" & String.Join(";", A) & String.Join(";", D0) & String.Join(";", D1) & String.Join(";", D2) & String.Join(";", D3))
                        MutexFile.ReleaseMutex()
                    End If
                Next
                If TargetMode = 1 Then
                    If wavecount >= TargetEvent Then
                        MainForm.ProgressBar.Value = 100
                        StopDataCaptureOnFile()
                        MainForm.SaveData.Enabled = True
                        MainForm.StopSaveData.Enabled = False
                    Else
                        MainForm.ProgressBar.Value = wavecount / TargetEvent * 100
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        SingleShotA()

    End Sub

    Private Sub pScope_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel

        Dim mwe As HandledMouseEventArgs = DirectCast(e, HandledMouseEventArgs)
        mwe.Handled = True

    End Sub

    Private Sub Pesgo1_Click(sender As Object, e As EventArgs) Handles Pesgo1.Click

    End Sub
End Class

