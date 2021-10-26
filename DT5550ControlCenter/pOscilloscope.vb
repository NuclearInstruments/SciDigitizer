Imports System.IO
Imports System.Threading
Imports System.ComponentModel
Imports Gigasoft.ProEssentials
Imports Gigasoft.ProEssentials.Enums
Imports DT5550ControlCenter.AcquisitionClass

Public Class pOscilloscope

    Public running As Boolean = False
    Dim inhibit = True
    Dim osc_ch As Integer
    Dim n_osc As Integer
    Dim n_ch_osc As Integer
    Public addressData As New List(Of UInt32)
    Public addressDecimator As New List(Of UInt32)
    Public addressPre As New List(Of UInt32)
    Public addressMode As New List(Of UInt32)
    Public addressLevel As New List(Of UInt32)
    Public addressArm As New List(Of UInt32)
    Public addressStatus As New List(Of UInt32)
    Public addressPosition As New List(Of UInt32)
    Dim nsamples As UInt32
    Dim tot_points As Integer
    Dim length As Integer
    Dim position As UInt32
    'Public wavecount = 0
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
    Public totalACQ As Integer = 0
    Dim plotS As Integer = 0
    Dim _ch_checked_modified = False
    Dim startTime As DateTime

    Dim Thread1 As System.Threading.Thread

    Dim colorList() As Color = {Color.Red, Color.Yellow, Color.Lime, Color.Cyan, Color.Magenta, Color.Blue, Color.BlueViolet, Color.Violet, Color.Peru, Color.Orange, Color.White,
                                Color.DarkRed, Color.Gold, Color.DarkGreen, Color.Teal, Color.HotPink, Color.RoyalBlue, Color.Purple, Color.Sienna, Color.Chocolate, Color.LightSlateGray,
                                Color.Tomato, Color.Moccasin, Color.PaleGreen, Color.PaleTurquoise, Color.Plum, Color.DeepSkyBlue, Color.MediumVioletRed, Color.RosyBrown, Color.LightSalmon, Color.Silver, Color.Olive}
    Dim n_ch As Integer

    Dim sampling_factor As Double
    Private Sub pScope_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
            sampling_factor = 1000 / 80
        ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Then
            sampling_factor = 1000 / 125
        ElseIf Connection.ComClass._boardModel = communication.tModel.DT5560SE Or Connection.ComClass._boardModel = communication.tModel.R5560SE Then
            sampling_factor = 1000 / 125
        ElseIf Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            sampling_factor = 1000 / 60
        End If

        n_osc = Connection.ComClass._n_oscilloscope
        For o = 0 To n_osc - 1
            addressData.Add(MainForm.CurrentOscilloscopes(o).Address)
        Next
        nsamples = MainForm.CurrentOscilloscopes(0).nsamples
        tot_points = 5 * nsamples
        n_ch_osc = Connection.ComClass._n_ch_oscilloscope
        osc_ch = Connection.ComClass._n_ch * Connection.ComClass._nBoard
        length = nsamples * n_ch_osc
        pScope_ReLoad()
        DisegnaGrafico()


    End Sub


    Public Sub pScope_ReLoad()

        CheckedListBox1.Items.Clear()
        ChList_name.Clear()
        ReDim EnabledChannel(MainForm.acquisition.CHList.Count - 1)
        ReDim EnabledChannel_id(MainForm.acquisition.CHList.Count - 1)
        CheckedListBox1.Items.Add("ALL", False)
        For i = 0 To MainForm.acquisition.CHList.Count - 1
            ChList_name.Add(MainForm.acquisition.CHList(i).name)
            CheckedListBox1.Items.Add(ChList_name(i), MainForm.acquisition.CHList(i).scope_checked)
            EnabledChannel(i) = False
        Next

    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged
        If running Then
            _ch_checked_modified = True
        Else
            ChangeLegendList()
        End If

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

        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
            Pesgo1.PeString.YAxisLabel = "ANALOG"

            Pesgo1.PeGrid.WorkingAxis = 1
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
        ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.DT5560SE Or Connection.ComClass._boardModel = communication.tModel.R5560SE Or Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            Pesgo1.PeString.YAxisLabel = "ANALOG"

            Pesgo1.PeGrid.WorkingAxis = 1
            Pesgo1.PeString.YAxisLabel = "TRAPEZOIDAL"

            Pesgo1.PeGrid.WorkingAxis = 2

            Pesgo1.PeString.YAxisLabel = "Trigger"
            Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
            Pesgo1.PeGrid.Configure.ManualMinY = 0
            Pesgo1.PeGrid.Configure.ManualMaxY = 1.1
            Pesgo1.PeGrid.WorkingAxis = 3

            Pesgo1.PeString.YAxisLabel = "Energy Sample"
            Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
            Pesgo1.PeGrid.Configure.ManualMinY = 0
            Pesgo1.PeGrid.Configure.ManualMaxY = 1.1
            Pesgo1.PeGrid.WorkingAxis = 4

            Pesgo1.PeString.YAxisLabel = "Baseline Hold"
            Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
            Pesgo1.PeGrid.Configure.ManualMinY = 0
            Pesgo1.PeGrid.Configure.ManualMaxY = 1.1
        End If

        Pesgo1.PeGrid.WorkingAxis = 0

        Dim tmpXData(nsamples * 5) As Single
        For i = 0 To nsamples - 1
            tmpXData(i) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
            tmpXData(i + (nsamples * 1)) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
            tmpXData(i + (nsamples * 2)) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
            tmpXData(i + (nsamples * 3)) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
            tmpXData(i + (nsamples * 4)) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
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

        Pesgo1.PeGrid.WorkingAxis = 0
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        If Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            Pesgo1.PeGrid.Configure.ManualMaxY = 4096
        Else
            Pesgo1.PeGrid.Configure.ManualMaxY = 16384
        End If


        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
            Pesgo1.PeGrid.MultiAxesProportions(0) = 0.8
            Pesgo1.PeGrid.MultiAxesProportions(1) = 0.05
            Pesgo1.PeGrid.MultiAxesProportions(2) = 0.05
            Pesgo1.PeGrid.MultiAxesProportions(3) = 0.05
            Pesgo1.PeGrid.MultiAxesProportions(4) = 0.05
        ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.DT5560SE Or Connection.ComClass._boardModel = communication.tModel.R5560SE Or Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            Pesgo1.PeGrid.MultiAxesProportions(0) = 0.65F
            Pesgo1.PeGrid.MultiAxesProportions(1) = 0.2F
            Pesgo1.PeGrid.MultiAxesProportions(2) = 0.05F
            Pesgo1.PeGrid.MultiAxesProportions(3) = 0.05F
            Pesgo1.PeGrid.MultiAxesProportions(4) = 0.05F
        End If


        Pesgo1.PeFunction.ReinitializeResetImage()


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
        startTime = Now

        For ch_id = 1 To CheckedListBox1.Items.Count - 1

            If EnabledChannel(ch_id - 1) Then
                CheckedListBox1.SetItemChecked(ch_id, True)
            Else
                CheckedListBox1.SetItemChecked(ch_id, False)
            End If
        Next

        If running Then
            _ch_checked_modified = True
        Else
            ChangeLegendList()
        End If


        If fileEnable = False Then
            fileName = file
            fileEnable = True
            objRawWriter = New StreamWriter(fileName)
            MainForm.plog.TextBox1.AppendText("Starting Oscilloscope Waveform Recording..." & vbCrLf)
        End If
        Timer1.Interval = 1
        MutexFile.ReleaseMutex()

    End Sub

    Public Sub StopDataCaptureOnFile()
        Timer1.Interval = 100
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
        MainForm.plog.TextBox1.AppendText("Starting Oscilloscope Free Running Acquisition..." & vbCrLf)
        Timer1.Enabled = True
        running = True
        'MainForm.sets.Apply.Enabled = False

    End Sub

    Public Sub SingleShot()

        Pesgo1.PeLegend.Show = True
        copyOscilloscopeParam()
        setOscilloscopeParam()
        MainForm.plog.TextBox1.AppendText("Oscilloscope Single Shot Acquisition!" & vbCrLf)

        SingleShotA()

    End Sub

    Public Sub ChangeLegendList()
        Checked_id.Clear()

        If CheckedListBox1.CheckedItems.Contains("ALL") Then
            n_ch = osc_ch
        Else
            n_ch = CheckedListBox1.CheckedIndices.Count
        End If

        'If CheckedListBox1.SelectedIndex >= 0 Then
        If CheckedListBox1.SelectedIndex = 0 Then
            Dim state = IIf(CheckedListBox1.GetItemCheckState(0).ToString = "Checked", True, False)
            For i = 1 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemChecked(i, state)
                MainForm.acquisition.CHList(i - 1).scope_checked = state
                If state Then
                    Checked_id.Add(MainForm.acquisition.CHList(i - 1).id)
                End If
            Next
        Else
            MainForm.acquisition.CHList(CheckedListBox1.SelectedIndex - 1).scope_checked = IIf(CheckedListBox1.GetItemCheckState(CheckedListBox1.SelectedIndex).ToString = "Checked", True, False)
                If CheckedListBox1.GetItemCheckState(0).ToString = "Checked" And CheckedListBox1.GetItemCheckState(CheckedListBox1.SelectedIndex).ToString = "Unchecked" Then
                    CheckedListBox1.SetItemChecked(0, False)
                End If
            Dim all_checked = True
            For i = 1 To CheckedListBox1.Items.Count - 1
                Dim state = IIf(CheckedListBox1.GetItemCheckState(i).ToString = "Checked", True, False)
                MainForm.acquisition.CHList(i - 1).scope_checked = state
                If state Then
                    Checked_id.Add(MainForm.acquisition.CHList(i - 1).id)
                Else
                    all_checked = False
                    ' Exit For
                End If
            Next
            If all_checked Then
                    CheckedListBox1.SetItemChecked(0, True)
                End If
            End If
        'Else

        ' End If
        'For Each scope_ch In MainForm.acquisition.CHList
        '    If scope_ch.scope_checked Then
        '        Checked_id.Add(scope_ch.id)
        '    End If
        'Next
        Pesgo1.PeData.Subsets = 5 * n_ch
        Pesgo1.PeLegend.SubsetsToLegend.Clear()
        Pesgo1.PeString.SubsetLabels.Clear()
        For n = 0 To Checked_id.Count - 1
            Pesgo1.PeLegend.SubsetsToLegend(n) = n
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
            ' If CheckedListBox1.CheckedItems.Contains("ALL") Then
            'Pesgo1.PeString.SubsetLabels(n) = CheckedListBox1.CheckedItems(n + 1)
            Pesgo1.PeString.SubsetLabels(n) = MainForm.acquisition.CHList(Checked_id(n) - 1).name
            'Else
            'Pesgo1.PeString.SubsetLabels(n) = CheckedListBox1.CheckedItems(n)
            'End If
            Pesgo1.PeString.SubsetLabels(n_ch * 1 + n) = ""
            Pesgo1.PeString.SubsetLabels(n_ch * 2 + n) = ""
            Pesgo1.PeString.SubsetLabels(n_ch * 3 + n) = ""
            Pesgo1.PeString.SubsetLabels(n_ch * 4 + n) = ""
        Next
        Pesgo1.PeLegend.SubsetsToLegend(Checked_id.Count - 1) = Checked_id.Count - 1
        Pesgo1.Invalidate()
        Pesgo1.PeFunction.ReinitializeResetImage()
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
            Dim idc = MainForm.acquisition.General_settings.TriggerChannelOscilloscope
            If Connection.ComClass._boardModel = communication.tModel.SCIDK Then
                idc = idc * 2
            End If
            wword = ((idc) << 8) + (MainForm.acquisition.General_settings.TriggerOscilloscopeEdges << 3) + 1
            TriggerModeValue = wword
        End If

    End Sub

    Public Sub setOscilloscopeParam()

        For j = 0 To Connection.ComClass._nBoard - 1
            For i = 0 To n_osc - 1
                If Connection.ComClass.SetRegister(addressDecimator(i), DecimatorValue - 1, j) = 0 Then
                    If Connection.ComClass.SetRegister(addressPre(i), Math.Floor(PreTriggerValue * nsamples / 100), j) = 0 Then
                        If Connection.ComClass.SetRegister(addressMode(i), TriggerModeValue, j) = 0 Then

                            Dim LevelTriggerValue_arr As Integer()
                            ReDim LevelTriggerValue_arr(n_osc)
                            If Connection.ComClass._boardModel = communication.tModel.DT5560SE Or Connection.ComClass._boardModel = communication.tModel.R5560SE Then
                                LevelTriggerValue_arr(i) = MainForm.acquisition.CHList(i + j * n_osc).TriggerOscilloscopeLevel
                            Else
                                LevelTriggerValue_arr(i) = LevelTriggerValue
                            End If

                            If Connection.ComClass.SetRegister(addressLevel(i), LevelTriggerValue_arr(i), j) = 0 Then
                                If Connection.ComClass.SetRegister(addressArm(i), 0, j) = 0 Then
                                    If Connection.ComClass.SetRegister(addressArm(i), 1, j) = 0 Then
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
            Next
        Next
    End Sub
    Dim lock = False
    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub SingleShotA()
        lock = True
        Static lastPlot As DateTime = Now
        Dim offsetLSB = (MainForm.acquisition.General_settings.AFEOffset + 2) / 4 * (4095 - 1650) + 1650
        Dim jjj As New ClassCalibration() 'My.Settings.AFECalibration)
        Dim coor = jjj.GetCorrectionFactors(offsetLSB)
        copyOscilloscopeParam()

        Dim WaveNN = 0

        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
            'Pesgo1.PeLegend.SubsetsToLegend.Clear()
            setOscilloscopeParam()
            Dim status As UInt32 = 0
            Dim tt = Now
            While status <> 1
                Connection.ComClass.GetRegister(addressStatus(0), status, 0)
                Application.DoEvents()
                ' If MainForm.__Running_OSC = False Then
                'lock = False
                'Exit Sub
                'End If
                If (Now - tt).TotalMilliseconds > 2000 Then
                    lock = False
                    Exit Sub
                End If
            End While

            Dim position As UInt32
            Connection.ComClass.GetRegister(addressPosition(0), position, 0)

            Dim data(length) As UInt32
            Dim read_data As UInt32
            Dim valid_data As UInt32
            If Connection.ComClass.ReadData(addressData(0), data, length, 0, 1000, read_data, valid_data, 0) = 0 Then

                If n_ch > 0 Then
                    Try
                        Me.Invoke(Sub()
                                      Pesgo1.PeData.Points = nsamples
                                      Pesgo1.PeGrid.MultiAxesSubsets(0) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(1) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(2) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(3) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(4) = n_ch
                                  End Sub)
                    Catch ex As Exception
                        Console.WriteLine("Exception Invoke" & ex.Message)


                    End Try


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
                            tmpXData(i + (nsamples * (n_ch * 0 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                            tmpXData(i + (nsamples * (n_ch * 1 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                            tmpXData(i + (nsamples * (n_ch * 2 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                            tmpXData(i + (nsamples * (n_ch * 3 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                            tmpXData(i + (nsamples * (n_ch * 4 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                        Next
                    Next

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

                    Try

                        Me.Invoke(Sub()
                                      Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.XData, tmpXData, TOTpoints)

                                      Array.Copy(AnalogArray, 0, tmpYData2, 0, nsamples * n_ch)
                                      Array.Copy(Digital1Array, 0, tmpYData2, nsamples * (n_ch * 1), nsamples * n_ch)
                                      Array.Copy(Digital2Array, 0, tmpYData2, nsamples * (n_ch * 2), nsamples * n_ch)
                                      Array.Copy(Digital3Array, 0, tmpYData2, nsamples * (n_ch * 3), nsamples * n_ch)
                                      Array.Copy(Digital4Array, 0, tmpYData2, nsamples * (n_ch * 4), nsamples * n_ch)

                                      Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.YData, tmpYData2, TOTpoints)

                                      Pesgo1.PeString.YAxisLabel = "ANALOG"

                                      Pesgo1.PeGrid.WorkingAxis = 1
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      If Connection.CustomFirmware Then
                                          Pesgo1.PeString.YAxisLabel = "Digital 0"
                                      Else
                                          Pesgo1.PeString.YAxisLabel = "Integration Gate"
                                      End If
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.WorkingAxis = 2
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      If Connection.CustomFirmware Then
                                          Pesgo1.PeString.YAxisLabel = "Digital 1"
                                      Else
                                          Pesgo1.PeString.YAxisLabel = "Baseline gate"
                                      End If
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.WorkingAxis = 3
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      If Connection.CustomFirmware Then
                                          Pesgo1.PeString.YAxisLabel = "Digital 2"
                                      Else
                                          Pesgo1.PeString.YAxisLabel = "Trigger"
                                      End If
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.WorkingAxis = 4
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      If Connection.CustomFirmware Then
                                          Pesgo1.PeString.YAxisLabel = "Digital 3"
                                      Else
                                          Pesgo1.PeString.YAxisLabel = "Pile Up"
                                      End If
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.MultiAxesProportions(0) = 0.8F
                                      Pesgo1.PeGrid.MultiAxesProportions(1) = 0.05F
                                      Pesgo1.PeGrid.MultiAxesProportions(2) = 0.05F
                                      Pesgo1.PeGrid.MultiAxesProportions(3) = 0.05F
                                      Pesgo1.PeGrid.MultiAxesProportions(4) = 0.05F


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
                                      lastPlot = Now
                                  End Sub)
                    Catch ex As Exception
                        Console.WriteLine("Exception Invoke" & ex.Message)


                    End Try
                End If

                ' wavecount += 1

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
                            objRawWriter.WriteLine(totalACQ & ";" & k + 1 & ";" & nsamples & ";" & 5 & ";" & String.Join(";", A) & String.Join(";", D0) & String.Join(";", D1) & String.Join(";", D2) & String.Join(";", D3))
                            MutexFile.ReleaseMutex()
                        End If
                    Next
                    If TargetMode = 1 Then
                        Try
                            Me.Invoke(Sub()
                                          If totalACQ >= TargetEvent Then
                                              MainForm.ProgressBar.Value = 100
                                              StopDataCaptureOnFile()
                                              MainForm.SaveData.Enabled = True
                                              MainForm.StopSaveData.Enabled = False
                                          Else
                                              MainForm.ProgressBar.Value = totalACQ / TargetEvent * 100
                                          End If
                                      End Sub)
                        Catch ex As Exception
                            Console.WriteLine("Exception Invoke" & ex.Message)


                        End Try

                    End If
                End If
            End If

        ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Then
            If n_ch > 0 Then

                Dim TOTpoints = tot_points * n_ch
                Dim tmpYData2(TOTpoints) As Single
                Static AnalogArray(nsamples * CheckedListBox1.Items.Count) As Single
                Static AnalogArray2(nsamples * CheckedListBox1.Items.Count) As Single
                Static Digital1Array(nsamples * CheckedListBox1.Items.Count) As Single
                Static Digital2Array(nsamples * CheckedListBox1.Items.Count) As Single
                Static Digital3Array(nsamples * CheckedListBox1.Items.Count) As Single

                Dim tmpXData(TOTpoints) As Single
                For j = 0 To n_ch - 1
                    For i = 0 To nsamples - 1
                        tmpXData(i + (nsamples * (n_ch * 0 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                        tmpXData(i + (nsamples * (n_ch * 1 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                        tmpXData(i + (nsamples * (n_ch * 2 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                        tmpXData(i + (nsamples * (n_ch * 3 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                        tmpXData(i + (nsamples * (n_ch * 4 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                    Next
                Next

                WaveNN += 1
                Dim n = 0

                Dim status As UInt32 = 0
                Dim position As UInt32
                Dim read_data As UInt32
                Dim valid_data As UInt32
                Dim test(length) As UInt32
                Dim gdata(4096, length) As UInt32
                Dim gstatus(4096) As UInt32
                Dim gposition(4096) As UInt32

                For Each ch In Checked_id
                    Dim ch_id = ch - 1
                    Dim ind = MainForm.acquisition.CHList(ch_id).board_number
                    Dim ch_addr = MainForm.acquisition.CHList(ch_id).ch_id - 1

                    Connection.ComClass.GetRegister(addressStatus(ch_addr), status, ind)
                    gstatus(ch_id) = status
                    Connection.ComClass.GetRegister(addressPosition(ch_addr), position, ind)
                    gposition(ch_id) = position
                    Connection.ComClass.ReadData(addressData(ch_addr), test, length, 0, 1000, read_data, valid_data, ind)
                    For t = 0 To length - 1
                        gdata(ch_id, t) = test(t)
                    Next

                Next

                For Each ch In Checked_id
                    Dim ch_id = ch - 1
                    Dim ind = MainForm.acquisition.CHList(ch_id).board_number
                    Dim ch_addr = MainForm.acquisition.CHList(ch_id).ch_id - 1
                    'If gstatus(qq) <> 1 Then
                    '    n += 1
                    '    Continue For
                    'End If
                    '
                    If gstatus(ch_id) = 1 Then
                        Connection.ComClass.SetRegister(addressArm(ch_addr), 0, ind)
                        Connection.ComClass.SetRegister(addressArm(ch_addr), 1, ind)
                    End If
                Next


                For Each ch In Checked_id
                    Dim ch_id = ch - 1
                    Dim ind = MainForm.acquisition.CHList(ch_id).board_number
                    Dim ch_addr = MainForm.acquisition.CHList(ch_id).ch_id - 1

                    'Dim status As UInt32 = 0
                    ' Dim tt = Now
                    ' While status <> 1
                    'Connection.ComClass.GetRegister(addressStatus(ch_addr), status, ind)
                    'If status <> 1 Then
                    ' n += 1
                    'Continue For
                    'End If

                    ' Application.DoEvents()
                    'If MainForm.__Running_OSC = False Then
                    ' lock = False
                    ' Exit Sub
                    ' End If

                    'End While


                    'Dim position As UInt32
                    'Connection.ComClass.GetRegister(addressPosition(ch_addr), position, ind)

                    'Dim data(length) As UInt32
                    'Dim read_data As UInt32
                    'Dim valid_data As UInt32
                    'If Connection.ComClass.ReadData(addressData(ch_addr), data, length, 0, 1000, read_data, valid_data, ind) = 0 Then
                    'If Connection.ComClass.SetRegister(addressArm(ch_addr), 0, ind) = 0 Then

                    'End If
                    '       If Connection.ComClass.SetRegister(addressArm(ch_addr), 1, ind) = 0 Then

                    ' End If
                    If gstatus(ch_id) = 1 Then
                        Dim curr As Integer = gposition(ch_id) - Math.Floor(PreTriggerValue * nsamples / 100)
                        If curr > 0 Then
                            Dim k = 0
                            For i = curr To nsamples - 2
                                Dim d = gdata(ch_id, i + nsamples)
                                AnalogArray(k + nsamples * n) = (gdata(ch_id, i) And 65535) '+ coor(ch)
                                If ((d And 65535) > 32767) Then
                                    AnalogArray2(k + nsamples * n) = -(65535 - (d And 65535))
                                Else
                                    AnalogArray2(k + nsamples * n) = d And 65535
                                End If
                                Digital1Array(k + nsamples * n) = gdata(ch_id, i) >> 16 And 1
                                Digital2Array(k + nsamples * n) = gdata(ch_id, i) >> 17 And 1
                                Digital3Array(k + nsamples * n) = gdata(ch_id, i) >> 18 And 1
                                k += 1
                            Next
                            For i = 0 To curr - 1
                                Dim d = gdata(ch_id, i + nsamples)
                                AnalogArray(k + nsamples * n) = (gdata(ch_id, i) And 65535) '+ coor(ch)
                                If ((d And 65535) > 32767) Then
                                    AnalogArray2(k + nsamples * n) = -(65535 - (d And 65535))
                                Else
                                    AnalogArray2(k + nsamples * n) = d And 65535
                                End If
                                Digital1Array(k + nsamples * n) = gdata(ch_id, i) >> 16 And 1
                                Digital2Array(k + nsamples * n) = gdata(ch_id, i) >> 17 And 1
                                Digital3Array(k + nsamples * n) = gdata(ch_id, i) >> 18 And 1
                                k += 1
                            Next
                        Else
                            Dim k = 0
                            For i = nsamples + curr To nsamples - 2
                                Dim d = gdata(ch_id, i + nsamples)
                                AnalogArray(k + nsamples * n) = (gdata(ch_id, i) And 65535) '+ coor(ch)
                                If ((d And 65535) > 32767) Then
                                    AnalogArray2(k + nsamples * n) = -(65535 - (d And 65535))
                                Else
                                    AnalogArray2(k + nsamples * n) = d And 65535
                                End If
                                Digital1Array(k + nsamples * n) = gdata(ch_id, i) >> 16 And 1
                                Digital2Array(k + nsamples * n) = gdata(ch_id, i) >> 17 And 1
                                Digital3Array(k + nsamples * n) = gdata(ch_id, i) >> 18 And 1
                                k += 1
                            Next
                            For i = 0 To nsamples + curr - 1
                                Dim d = gdata(ch_id, i + nsamples)
                                AnalogArray(k + nsamples * n) = (gdata(ch_id, i) And 65535) '+ coor(ch)
                                If ((d And 65535) > 32767) Then
                                    AnalogArray2(k + nsamples * n) = -(65535 - (d And 65535))
                                Else
                                    AnalogArray2(k + nsamples * n) = d And 65535
                                End If
                                Digital1Array(k + nsamples * n) = gdata(ch_id, i) >> 16 And 1
                                Digital2Array(k + nsamples * n) = gdata(ch_id, i) >> 17 And 1
                                Digital3Array(k + nsamples * n) = gdata(ch_id, i) >> 18 And 1
                                k += 1
                            Next
                        End If


                        If fileEnable = True Then
                            'For k = 0 To osc_ch - 1
                            If EnabledChannel(ch_id) Then
                                Dim A(nsamples), A2(nsamples), D0(nsamples), D1(nsamples), D2(nsamples) As Single
                                'For j = 0 To nsamples - 1
                                Array.Copy(AnalogArray, nsamples * n, A, 0, nsamples)
                                'Array.Copy(AnalogArray2, nsamples * n, A2, 0, nsamples)
                                'Array.Copy(Digital1Array, nsamples * n, D0, 0, nsamples)
                                'Array.Copy(Digital2Array, nsamples * n, D1, 0, nsamples)
                                'Array.Copy(Digital3Array, nsamples * n, D2, 0, nsamples)
                                '    Next
                                MutexFile.WaitOne()
                                objRawWriter.WriteLine((Now - startTime).TotalMilliseconds / 1000.0 & ";" & ch_id + 1 & ";" & nsamples & ";" & 1 & ";" & String.Join(";", A)) ' & String.Join(";", A2) & String.Join(";", D0) & String.Join(";", D1) & String.Join(";", D2))
                                MutexFile.ReleaseMutex()
                            End If
                            ' Next
                            If TargetMode = 1 Then
                                Try
                                    Me.Invoke(Sub()
                                                  If totalACQ >= TargetEvent Then
                                                      MainForm.ProgressBar.Value = 100
                                                      StopDataCaptureOnFile()
                                                      MainForm.SaveData.Enabled = True
                                                      MainForm.StopSaveData.Enabled = False
                                                  Else
                                                      MainForm.ProgressBar.Value = totalACQ / TargetEvent * 100
                                                  End If
                                              End Sub)
                                Catch ex As Exception
                                    Console.WriteLine("Exception Invoke" & ex.Message)


                                End Try

                            End If
                        End If

                    End If
                    n += 1
                    'Console.WriteLine((Now - lastPlot).TotalMilliseconds)
                    'End If
                Next

                For i = 1 To n_ch
                    For q = 0 To 4
                        AnalogArray(nsamples * i - q) = AnalogArray(nsamples * i - 4)
                        AnalogArray2(nsamples * i - q) = AnalogArray2(nsamples * i - 4)
                        Digital1Array(nsamples * i - q) = Digital1Array(nsamples * i - 4)
                        Digital2Array(nsamples * i - q) = Digital2Array(nsamples * i - 4)
                        Digital3Array(nsamples * i - q) = Digital3Array(nsamples * i - 4)
                    Next

                Next

                If (Now - lastPlot).TotalMilliseconds > 90 Then
                    Try
                        Me.Invoke(Sub()
                                      Pesgo1.PeData.Points = nsamples
                                      Pesgo1.PeGrid.MultiAxesSubsets(0) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(1) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(2) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(3) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(4) = n_ch
                                  End Sub)
                    Catch ex As Exception
                        Console.WriteLine("Exception Invoke" & ex.Message)


                    End Try

                    Try
                        Me.Invoke(Sub()
                                      Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.XData, tmpXData, TOTpoints)

                                      Array.Copy(AnalogArray, 0, tmpYData2, 0, nsamples * n_ch)
                                      Array.Copy(AnalogArray2, 0, tmpYData2, nsamples * (n_ch * 1), nsamples * n_ch)
                                      Array.Copy(Digital1Array, 0, tmpYData2, nsamples * (n_ch * 2), nsamples * n_ch)
                                      Array.Copy(Digital2Array, 0, tmpYData2, nsamples * (n_ch * 3), nsamples * n_ch)
                                      Array.Copy(Digital3Array, 0, tmpYData2, nsamples * (n_ch * 4), nsamples * n_ch)

                                      Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.YData, tmpYData2, TOTpoints)

                                      Pesgo1.PeString.YAxisLabel = "ANALOG"

                                      Pesgo1.PeGrid.WorkingAxis = 1
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeString.YAxisLabel = "TRAPEZOIDAL"

                                      Pesgo1.PeGrid.WorkingAxis = 2
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeString.YAxisLabel = "Trigger"
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.WorkingAxis = 3
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeString.YAxisLabel = "Energy Sample"
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.WorkingAxis = 4
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeString.YAxisLabel = "Baseline Hold"
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.MultiAxesProportions(0) = 0.65F
                                      Pesgo1.PeGrid.MultiAxesProportions(1) = 0.2F
                                      Pesgo1.PeGrid.MultiAxesProportions(2) = 0.05F
                                      Pesgo1.PeGrid.MultiAxesProportions(3) = 0.05F
                                      Pesgo1.PeGrid.MultiAxesProportions(4) = 0.05F


                                      If (Pesgo1.PeConfigure.RenderEngine = RenderEngine.Direct3D) Then
                                          Pesgo1.PeFunction.Force3dxVerticeRebuild = True
                                          Pesgo1.PeFunction.Force3dxNewColors = True
                                      Else
                                          Pesgo1.PeFunction.Reinitialize()
                                          Pesgo1.PeFunction.ResetImage(0, 0)
                                      End If
                                      totalACQ += WaveNN
                                      Pesgo1.PeString.MainTitle = "Real Time Oscilloscope (" & totalACQ & ")"
                                      Pesgo1.Invalidate()
                                      Pesgo1.PeFunction.ReinitializeResetImage()
                                      lastPlot = Now
                                  End Sub)
                    Catch ex As Exception
                        Console.WriteLine("Exception Invoke" & ex.Message)


                    End Try

                End If


            End If
        ElseIf Connection.ComClass._boardModel = communication.tModel.DT5560SE Or Connection.ComClass._boardModel = communication.tModel.R5560SE Then
            If n_ch > 0 Then

                Dim TOTpoints = tot_points * n_ch
                Dim tmpYData2(TOTpoints - 1) As Single
                Dim AnalogArray(nsamples * CheckedListBox1.Items.Count - 1) As Single
                Dim AnalogArray2(nsamples * CheckedListBox1.Items.Count - 1) As Single
                Dim Digital1Array(nsamples * CheckedListBox1.Items.Count - 1) As Single
                Dim Digital2Array(nsamples * CheckedListBox1.Items.Count - 1) As Single
                Dim Digital3Array(nsamples * CheckedListBox1.Items.Count - 1) As Single

                Dim tmpXData(TOTpoints - 1) As Single
                For j = 0 To n_ch - 1
                    For i = 0 To nsamples - 1
                        tmpXData(i + (nsamples * (n_ch * 0 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                        tmpXData(i + (nsamples * (n_ch * 1 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                        tmpXData(i + (nsamples * (n_ch * 2 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                        tmpXData(i + (nsamples * (n_ch * 3 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                        tmpXData(i + (nsamples * (n_ch * 4 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                    Next
                Next

                WaveNN += 1
                Dim n = 0

                Dim status As UInt32 = 0
                Dim position As UInt32
                Dim read_data As UInt32
                Dim valid_data As UInt32
                Dim test(length - 1) As UInt32
                Dim gdata(MainForm.acquisition.CHList.Count - 1, length - 1) As UInt32
                Dim gstatus(MainForm.acquisition.CHList.Count - 1) As UInt32
                Dim gposition(MainForm.acquisition.CHList.Count - 1) As UInt32

                For qq = 0 To MainForm.acquisition.CHList.Count - 1
                    Connection.ComClass.GetRegister(addressStatus(MainForm.acquisition.CHList(qq).ch_id - 1), status, MainForm.acquisition.CHList(qq).board_number - 1)
                    gstatus(qq) = status
                    Connection.ComClass.GetRegister(addressPosition(MainForm.acquisition.CHList(qq).ch_id - 1), position, MainForm.acquisition.CHList(qq).board_number - 1)
                    gposition(qq) = position
                    Connection.ComClass.ReadData(addressData(MainForm.acquisition.CHList(qq).ch_id - 1), test, length, 0, 1000, read_data, valid_data, MainForm.acquisition.CHList(qq).board_number - 1)
                    For t = 0 To length - 1
                        gdata(qq, t) = test(t)
                    Next

                Next

                For qq = 0 To MainForm.acquisition.CHList.Count - 1
                    'If gstatus(qq) <> 1 Then
                    '    n += 1
                    '    Continue For
                    'End If
                    Connection.ComClass.SetRegister(addressArm(MainForm.acquisition.CHList(qq).ch_id - 1), 0, MainForm.acquisition.CHList(qq).board_number - 1)
                    Connection.ComClass.SetRegister(addressArm(MainForm.acquisition.CHList(qq).ch_id - 1), 1, MainForm.acquisition.CHList(qq).board_number - 1)
                Next

                For Each ch In Checked_id
                    Dim ch_id = ch - 1
                    Dim ind = MainForm.acquisition.CHList(ch_id).board_number
                    Dim ch_addr = MainForm.acquisition.CHList(ch_id).ch_id - 1
                    If gstatus(ch_id) = 1 Then
                        Dim curr As Integer = gposition(ch_id) - Math.Floor(PreTriggerValue * nsamples / 100)
                        If curr > 0 Then
                            Dim k = 0
                            For i = curr To nsamples - 2
                                Dim d = gdata(ch_id, i + nsamples)
                                AnalogArray(k + nsamples * n) = (gdata(ch_id, i) And 65535) '+ coor(ch)
                                If ((d And 65535) > 32767) Then
                                    AnalogArray2(k + nsamples * n) = -(65535 - (d And 65535))
                                Else
                                    AnalogArray2(k + nsamples * n) = d And 65535
                                End If
                                Digital1Array(k + nsamples * n) = gdata(ch_id, i) >> 16 And 1
                                Digital2Array(k + nsamples * n) = gdata(ch_id, i) >> 17 And 1
                                Digital3Array(k + nsamples * n) = gdata(ch_id, i) >> 18 And 1
                                k += 1
                            Next
                            For i = 0 To curr - 1
                                Dim d = gdata(ch_id, i + nsamples)
                                AnalogArray(k + nsamples * n) = (gdata(ch_id, i) And 65535) '+ coor(ch)
                                If ((d And 65535) > 32767) Then
                                    AnalogArray2(k + nsamples * n) = -(65535 - (d And 65535))
                                Else
                                    AnalogArray2(k + nsamples * n) = d And 65535
                                End If
                                Digital1Array(k + nsamples * n) = gdata(ch_id, i) >> 16 And 1
                                Digital2Array(k + nsamples * n) = gdata(ch_id, i) >> 17 And 1
                                Digital3Array(k + nsamples * n) = gdata(ch_id, i) >> 18 And 1
                                k += 1
                            Next
                        Else
                            Dim k = 0
                            For i = nsamples + curr To nsamples - 2
                                Dim d = gdata(ch_id, i + nsamples)
                                AnalogArray(k + nsamples * n) = (gdata(ch_id, i) And 65535) '+ coor(ch)
                                If ((d And 65535) > 32767) Then
                                    AnalogArray2(k + nsamples * n) = -(65535 - (d And 65535))
                                Else
                                    AnalogArray2(k + nsamples * n) = d And 65535
                                End If
                                Digital1Array(k + nsamples * n) = gdata(ch_id, i) >> 16 And 1
                                Digital2Array(k + nsamples * n) = gdata(ch_id, i) >> 17 And 1
                                Digital3Array(k + nsamples * n) = gdata(ch_id, i) >> 18 And 1
                                k += 1
                            Next
                            For i = 0 To nsamples + curr - 1
                                Dim d = gdata(ch_id, i + nsamples)
                                AnalogArray(k + nsamples * n) = (gdata(ch_id, i) And 65535) '+ coor(ch)
                                If ((d And 65535) > 32767) Then
                                    AnalogArray2(k + nsamples * n) = -(65535 - (d And 65535))
                                Else
                                    AnalogArray2(k + nsamples * n) = d And 65535
                                End If
                                Digital1Array(k + nsamples * n) = gdata(ch_id, i) >> 16 And 1
                                Digital2Array(k + nsamples * n) = gdata(ch_id, i) >> 17 And 1
                                Digital3Array(k + nsamples * n) = gdata(ch_id, i) >> 18 And 1
                                k += 1
                            Next
                        End If

                        If (MainForm.acquisition.CHList(ch_id).polarity = signal_polarity.NEGATIVE) Then
                            AnalogArray(nsamples * n) = -AnalogArray(nsamples * n)
                        End If

                        If fileEnable = True Then
                                If EnabledChannel(ch_id) Then
                                    Dim A(nsamples), A2(nsamples), D0(nsamples), D1(nsamples), D2(nsamples) As Single

                                    Array.Copy(AnalogArray, nsamples * n, A, 0, nsamples)

                                    objRawWriter.WriteLine((Now - startTime).TotalMilliseconds / 1000.0 & ";" & ch_id & ";" & nsamples & ";" & 1 & ";" & String.Join(";", A)) ' & String.Join(";", A2) & String.Join(";", D0) & String.Join(";", D1) & String.Join(";", D2))

                                End If

                                If TargetMode = 1 Then
                                    Try
                                        Me.Invoke(Sub()
                                                      If totalACQ >= TargetEvent Then
                                                          MainForm.ProgressBar.Value = 100
                                                          StopDataCaptureOnFile()
                                                          MainForm.SaveData.Enabled = True
                                                          MainForm.StopSaveData.Enabled = False
                                                      Else
                                                          MainForm.ProgressBar.Value = totalACQ / TargetEvent * 100
                                                      End If
                                                  End Sub)
                                    Catch ex As Exception
                                        Console.WriteLine("Exception Invoke" & ex.Message)


                                    End Try

                                End If
                            End If
                        End If
                        n += 1
                Next

                'For Each ch In Checked_id
                '    Dim ch_id = ch - 1
                '    Dim ind = MainForm.acquisition.CHList(ch_id).board_number
                '    Dim ch_addr = MainForm.acquisition.CHList(ch_id).ch_id - 1


                '    Dim tt = Now
                '    ' While status <> 1
                '    Connection.ComClass.GetRegister(addressStatus(ch_addr), status, 0)
                '    If status <> 1 Then
                '        n += 1
                '        Continue For
                '    End If



                '    ' Application.DoEvents()
                '    '  If MainForm.__Running_OSC = False Then
                '    '  lock = False
                '    '  Exit Sub
                '    ' End If

                '    'End While


                '    Connection.ComClass.GetRegister(addressPosition(ch_addr), position, 0)

                '    Dim data(length) As UInt32





                '    If Connection.ComClass.ReadData(addressData(ch_addr), data, length, 0, 1000, read_data, valid_data, 0) = 0 Then
                '            If Connection.ComClass.SetRegister(addressArm(ch_addr), 0, 0) = 0 Then

                '            End If
                '            If Connection.ComClass.SetRegister(addressArm(ch_addr), 1, 0) = 0 Then

                '            End If

                '            WaveNN += 1
                '            Dim curr As Integer = position - Math.Floor(PreTriggerValue * nsamples / 100)
                '            If curr > 0 Then
                '                Dim k = 0
                '                For i = curr To nsamples - 2
                '                    Dim d = data(i + nsamples)
                '                    AnalogArray(k + nsamples * n) = (data(i) And 65535) '+ coor(ch)
                '                    If ((d And 65535) > 32767) Then
                '                        AnalogArray2(k + nsamples * n) = -(65535 - (d And 65535))
                '                    Else
                '                        AnalogArray2(k + nsamples * n) = d And 65535
                '                    End If
                '                    Digital1Array(k + nsamples * n) = data(i) >> 16 And 1
                '                    Digital2Array(k + nsamples * n) = data(i) >> 17 And 1
                '                    Digital3Array(k + nsamples * n) = data(i) >> 18 And 1
                '                    k += 1
                '                Next
                '                For i = 0 To curr - 1
                '                    Dim d = data(i + nsamples)
                '                    AnalogArray(k + nsamples * n) = (data(i) And 65535) '+ coor(ch)
                '                    If ((d And 65535) > 32767) Then
                '                        AnalogArray2(k + nsamples * n) = -(65535 - (d And 65535))
                '                    Else
                '                        AnalogArray2(k + nsamples * n) = d And 65535
                '                    End If
                '                    Digital1Array(k + nsamples * n) = data(i) >> 16 And 1
                '                    Digital2Array(k + nsamples * n) = data(i) >> 17 And 1
                '                    Digital3Array(k + nsamples * n) = data(i) >> 18 And 1
                '                    k += 1
                '                Next
                '            Else
                '                Dim k = 0
                '                For i = nsamples + curr To nsamples - 2
                '                    Dim d = data(i + nsamples)
                '                    AnalogArray(k + nsamples * n) = (data(i) And 65535) '+ coor(ch)
                '                    If ((d And 65535) > 32767) Then
                '                        AnalogArray2(k + nsamples * n) = -(65535 - (d And 65535))
                '                    Else
                '                        AnalogArray2(k + nsamples * n) = d And 65535
                '                    End If
                '                    Digital1Array(k + nsamples * n) = data(i) >> 16 And 1
                '                    Digital2Array(k + nsamples * n) = data(i) >> 17 And 1
                '                    Digital3Array(k + nsamples * n) = data(i) >> 18 And 1
                '                    k += 1
                '                Next
                '                For i = 0 To nsamples + curr - 1
                '                    Dim d = data(i + nsamples)
                '                    AnalogArray(k + nsamples * n) = (data(i) And 65535) '+ coor(ch)
                '                    If ((d And 65535) > 32767) Then
                '                        AnalogArray2(k + nsamples * n) = -(65535 - (d And 65535))
                '                    Else
                '                        AnalogArray2(k + nsamples * n) = d And 65535
                '                    End If
                '                    Digital1Array(k + nsamples * n) = data(i) >> 16 And 1
                '                    Digital2Array(k + nsamples * n) = data(i) >> 17 And 1
                '                    Digital3Array(k + nsamples * n) = data(i) >> 18 And 1
                '                    k += 1
                '                Next
                '            End If


                '            If fileEnable = True Then
                '                'For k = 0 To osc_ch - 1
                '                If EnabledChannel(ch_id) Then
                '                    Dim A(nsamples), A2(nsamples), D0(nsamples), D1(nsamples), D2(nsamples) As Single
                '                    'For j = 0 To nsamples - 1
                '                    Array.Copy(AnalogArray, nsamples * n, A, 0, nsamples)
                '                    'Array.Copy(AnalogArray2, nsamples * n, A2, 0, nsamples)
                '                    'Array.Copy(Digital1Array, nsamples * n, D0, 0, nsamples)
                '                    'Array.Copy(Digital2Array, nsamples * n, D1, 0, nsamples)
                '                    'Array.Copy(Digital3Array, nsamples * n, D2, 0, nsamples)
                '                    '    Next
                '                    'MutexFile.WaitOne()
                '                    objRawWriter.WriteLine((Now - startTime).TotalMilliseconds / 1000.0 & ";" & ch_id & ";" & nsamples & ";" & 1 & ";" & String.Join(";", A)) ' & String.Join(";", A2) & String.Join(";", D0) & String.Join(";", D1) & String.Join(";", D2))
                '                    'MutexFile.ReleaseMutex()
                '                End If
                '                ' Next
                '                If TargetMode = 1 Then
                '                    If totalACQ >= TargetEvent Then
                '                        MainForm.ProgressBar.Value = 100
                '                        StopDataCaptureOnFile()
                '                        MainForm.SaveData.Enabled = True
                '                        MainForm.StopSaveData.Enabled = False
                '                    Else
                '                        MainForm.ProgressBar.Value = totalACQ / TargetEvent * 100
                '                    End If
                '                End If
                '            End If

                '            n += 1

                '            'Console.WriteLine((Now - lastPlot).TotalMilliseconds)


                '        End If
                '    Next

                For i = 1 To n_ch
                    For q = 0 To 4
                        AnalogArray(nsamples * i - q) = AnalogArray(nsamples * i - 4)
                        AnalogArray2(nsamples * i - q) = AnalogArray2(nsamples * i - 4)
                        Digital1Array(nsamples * i - q) = Digital1Array(nsamples * i - 4)
                        Digital2Array(nsamples * i - q) = Digital2Array(nsamples * i - 4)
                        Digital3Array(nsamples * i - q) = Digital3Array(nsamples * i - 4)
                    Next

                Next

                If (Now - lastPlot).TotalMilliseconds > 90 Then
                    Try
                        Me.Invoke(Sub()
                                      Pesgo1.PeData.Points = nsamples
                                      Pesgo1.PeGrid.MultiAxesSubsets(0) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(1) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(2) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(3) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(4) = n_ch
                                  End Sub)
                    Catch ex As Exception
                        Console.WriteLine("Exception Invoke" & ex.Message)


                    End Try

                    Try
                        Me.Invoke(Sub()
                                      Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.XData, tmpXData, TOTpoints)

                                      Array.Copy(AnalogArray, 0, tmpYData2, 0, nsamples * n_ch)
                                      Array.Copy(AnalogArray2, 0, tmpYData2, nsamples * (n_ch * 1), nsamples * n_ch)
                                      Array.Copy(Digital1Array, 0, tmpYData2, nsamples * (n_ch * 2), nsamples * n_ch)
                                      Array.Copy(Digital2Array, 0, tmpYData2, nsamples * (n_ch * 3), nsamples * n_ch)
                                      Array.Copy(Digital3Array, 0, tmpYData2, nsamples * (n_ch * 4), nsamples * n_ch)

                                      Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.YData, tmpYData2, TOTpoints)

                                      Pesgo1.PeString.YAxisLabel = "ANALOG"

                                      Pesgo1.PeGrid.WorkingAxis = 1
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeString.YAxisLabel = "TRAPEZOIDAL"

                                      Pesgo1.PeGrid.WorkingAxis = 2
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeString.YAxisLabel = "Trigger"
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.WorkingAxis = 3
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeString.YAxisLabel = "Energy Sample"
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.WorkingAxis = 4
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeString.YAxisLabel = "Baseline Hold"
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.MultiAxesProportions(0) = 0.65F
                                      Pesgo1.PeGrid.MultiAxesProportions(1) = 0.2F
                                      Pesgo1.PeGrid.MultiAxesProportions(2) = 0.05F
                                      Pesgo1.PeGrid.MultiAxesProportions(3) = 0.05F
                                      Pesgo1.PeGrid.MultiAxesProportions(4) = 0.05F


                                      If (Pesgo1.PeConfigure.RenderEngine = RenderEngine.Direct3D) Then
                                          Pesgo1.PeFunction.Force3dxVerticeRebuild = True
                                          Pesgo1.PeFunction.Force3dxNewColors = True
                                      Else
                                          Pesgo1.PeFunction.Reinitialize()
                                          Pesgo1.PeFunction.ResetImage(0, 0)
                                      End If
                                      totalACQ += WaveNN
                                      Pesgo1.PeString.MainTitle = "Real Time Oscilloscope (" & totalACQ & ")"
                                      Pesgo1.Invalidate()
                                      Pesgo1.PeFunction.ReinitializeResetImage()
                                      lastPlot = Now
                                  End Sub)
                    Catch ex As Exception
                        Console.WriteLine("Exception Invoke" & ex.Message)


                    End Try

                End If


            End If


        ElseIf Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            Dim status As UInt32 = 0
            Dim tt = Now
            While status <> 1
                Connection.ComClass.GetRegister(addressStatus(0), status, 0)
                'Application.DoEvents()
                'If MainForm.__Running_OSC = False Then
                ' lock = False
                ' Exit Sub
                'End If
                If (Now - tt).TotalMilliseconds > 2000 Then
                    lock = False
                    Exit Sub
                End If
            End While

            Dim position As UInt32
            Connection.ComClass.GetRegister(addressPosition(0), position, 0)

            Dim data(length * 2) As UInt32
            Dim read_data As UInt32
            Dim valid_data As UInt32
            If Connection.ComClass.ReadData(addressData(0), data, length * 2, 0, 1000, read_data, valid_data, 0) = 0 Then
                If Connection.ComClass.SetRegister(addressArm(0), 0, 0) = 0 Then

                End If
                If Connection.ComClass.SetRegister(addressArm(0), 1, 0) = 0 Then

                End If
                If n_ch > 0 Then
                    Try
                        Me.Invoke(Sub()
                                      Pesgo1.PeData.Points = nsamples
                                      Pesgo1.PeGrid.MultiAxesSubsets(0) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(1) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(2) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(3) = n_ch
                                      Pesgo1.PeGrid.MultiAxesSubsets(4) = n_ch
                                  End Sub)
                    Catch ex As Exception
                        Console.WriteLine("Exception Invoke" & ex.Message)


                    End Try


                    Dim TOTpoints As Integer = 5 * nsamples * n_ch
                    Dim tmpYData2(TOTpoints) As Single
                    Dim AnalogArray(nsamples * n_ch) As Single
                    Dim AnalogArray2(nsamples * n_ch) As Single
                    Dim Digital1Array(nsamples * n_ch) As Single
                    Dim Digital2Array(nsamples * n_ch) As Single
                    Dim Digital3Array(nsamples * n_ch) As Single
                    Dim Digital4Array(nsamples * n_ch) As Single

                    Dim tmpXData(TOTpoints) As Single
                    For j = 0 To n_ch - 1
                        For i = 0 To nsamples - 1
                            tmpXData(i + (nsamples * (n_ch * 0 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                            tmpXData(i + (nsamples * (n_ch * 1 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                            tmpXData(i + (nsamples * (n_ch * 2 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                            tmpXData(i + (nsamples * (n_ch * 3 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                            tmpXData(i + (nsamples * (n_ch * 4 + j))) = i * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * sampling_factor
                        Next
                    Next

                    Dim curr As Integer = position - Math.Floor(PreTriggerValue * nsamples / 100)
                    Dim n = 0
                    For Each ch_id In Checked_id
                        If ch_id <> 0 Then
                            Dim ch = (ch_id - 1) * 2
                            If curr > 0 Then
                                Dim k = 0
                                For i = curr To nsamples - 2
                                    AnalogArray(k + nsamples * n) = (data(i + nsamples * ch) And 65535) + coor(ch)
                                    Dim d = (data(i + nsamples * (ch + 1)) And 65535)
                                    AnalogArray2(k + nsamples * n) = IIf(d > 32767, d = -(65535 - d), d)
                                    Digital1Array(k + nsamples * n) = data(i + nsamples * (ch + 1)) >> 16 And 1
                                    Digital2Array(k + nsamples * n) = data(i + nsamples * (ch + 1)) >> 17 And 1
                                    Digital3Array(k + nsamples * n) = data(i + nsamples * (ch + 1)) >> 18 And 1
                                    k += 1
                                Next
                                For i = 0 To curr - 1
                                    AnalogArray(k + nsamples * n) = (data(i + nsamples * ch) And 65535) + coor(ch)
                                    Dim d = (data(i + nsamples * (ch + 1)) And 65535)
                                    AnalogArray2(k + nsamples * n) = IIf(d > 32767, d = -(65535 - d), d)
                                    Digital1Array(k + nsamples * n) = data(i + nsamples * (ch + 1)) >> 16 And 1
                                    Digital2Array(k + nsamples * n) = data(i + nsamples * (ch + 1)) >> 17 And 1
                                    Digital3Array(k + nsamples * n) = data(i + nsamples * (ch + 1)) >> 18 And 1
                                    k += 1
                                Next
                            Else
                                Dim k = 0
                                For i = nsamples + curr To nsamples - 2
                                    AnalogArray(k + nsamples * n) = (data(i + nsamples * ch) And 65535) + coor(ch)
                                    Dim d = (data(i + nsamples * (ch + 1)) And 65535)
                                    AnalogArray2(k + nsamples * n) = IIf(d > 32767, d = -(65535 - d), d)
                                    Digital1Array(k + nsamples * n) = data(i + nsamples * (ch + 1)) >> 16 And 1
                                    Digital2Array(k + nsamples * n) = data(i + nsamples * (ch + 1)) >> 17 And 1
                                    Digital3Array(k + nsamples * n) = data(i + nsamples * (ch + 1)) >> 18 And 1
                                    k += 1
                                Next
                                For i = 0 To nsamples + curr - 1
                                    AnalogArray(k + nsamples * n) = (data(i + nsamples * ch) And 65535) + coor(ch)
                                    Dim d = (data(i + nsamples * (ch + 1)) And 65535)
                                    AnalogArray2(k + nsamples * n) = IIf(d > 32767, d = -(65535 - d), d)
                                    Digital1Array(k + nsamples * n) = data(i + nsamples * (ch + 1)) >> 16 And 1
                                    Digital2Array(k + nsamples * n) = data(i + nsamples * (ch + 1)) >> 17 And 1
                                    Digital3Array(k + nsamples * n) = data(i + nsamples * (ch + 1)) >> 18 And 1
                                    k += 1
                                Next
                            End If



                            n += 1
                        End If
                    Next
                    For i = 1 To n_ch
                        For q = 0 To 4
                            AnalogArray(nsamples * i - q) = AnalogArray(nsamples * i - 4)
                            AnalogArray2(nsamples * i - q) = AnalogArray2(nsamples * i - 4)
                            Digital1Array(nsamples * i - q) = Digital1Array(nsamples * i - 4)
                            Digital2Array(nsamples * i - q) = Digital2Array(nsamples * i - 4)
                            Digital3Array(nsamples * i - q) = Digital3Array(nsamples * i - 4)
                            Digital4Array(nsamples * i - q) = Digital4Array(nsamples * i - 4)
                        Next

                    Next


                    Try
                        Me.Invoke(Sub()
                                      Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.XData, tmpXData, TOTpoints)

                                      Array.Copy(AnalogArray, 0, tmpYData2, 0, nsamples * n_ch)
                                      Array.Copy(AnalogArray2, 0, tmpYData2, nsamples * (n_ch * 1), nsamples * n_ch)
                                      Array.Copy(Digital1Array, 0, tmpYData2, nsamples * (n_ch * 2), nsamples * n_ch)
                                      Array.Copy(Digital2Array, 0, tmpYData2, nsamples * (n_ch * 3), nsamples * n_ch)
                                      Array.Copy(Digital3Array, 0, tmpYData2, nsamples * (n_ch * 4), nsamples * n_ch)
                                      'Array.Copy(Digital4Array, 0, tmpYData2, nsamples * (n_ch * 4), nsamples * n_ch)



                                      Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.YData, tmpYData2, TOTpoints)
                                      Pesgo1.PeString.YAxisLabel = "ANALOG"

                                      Pesgo1.PeGrid.WorkingAxis = 1
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeString.YAxisLabel = "TRAPEZOIDAL"

                                      Pesgo1.PeGrid.WorkingAxis = 2
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeString.YAxisLabel = "Trigger"
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.WorkingAxis = 3
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeString.YAxisLabel = "Energy Sample"
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.WorkingAxis = 4
                                      Pesgo1.PeGrid.Configure.YAxisWholeNumbers = True
                                      Pesgo1.PeString.TruncateYAxisLabels = True
                                      Pesgo1.PeString.YAxisLabel = "Baseline Hold"
                                      Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
                                      Pesgo1.PeGrid.Configure.ManualMinY = 0
                                      Pesgo1.PeGrid.Configure.ManualMaxY = 1.1

                                      Pesgo1.PeGrid.MultiAxesProportions(0) = 0.65F
                                      Pesgo1.PeGrid.MultiAxesProportions(1) = 0.2F
                                      Pesgo1.PeGrid.MultiAxesProportions(2) = 0.05F
                                      Pesgo1.PeGrid.MultiAxesProportions(3) = 0.05F
                                      Pesgo1.PeGrid.MultiAxesProportions(4) = 0.05F


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
                                  End Sub)
                    Catch ex As Exception
                        Console.WriteLine("Exception Invoke" & ex.Message)


                    End Try



                End If

                ' wavecount += 1

                If fileEnable = True Then
                    For k = 0 To osc_ch - 1
                        If EnabledChannel(k) Then
                            Dim A(nsamples), D0(nsamples), D1(nsamples), D2(nsamples), D3(nsamples) As Single
                            For j = 0 To nsamples - 1
                                A(j) = data(j + nsamples * (EnabledChannel_id(k) - 1) * 2) And 65535
                            Next
                            MutexFile.WaitOne()
                            objRawWriter.WriteLine(totalACQ & ";" & k + 1 & ";" & nsamples & ";" & 1 & ";" & String.Join(";", A))
                            MutexFile.ReleaseMutex()
                        End If
                    Next
                    If TargetMode = 1 Then
                        Try
                            Me.Invoke(Sub()
                                          If totalACQ >= TargetEvent Then
                                              MainForm.ProgressBar.Value = 100
                                              StopDataCaptureOnFile()
                                              MainForm.SaveData.Enabled = True
                                              MainForm.StopSaveData.Enabled = False
                                          Else
                                              MainForm.ProgressBar.Value = totalACQ / TargetEvent * 100
                                          End If
                                      End Sub)
                        Catch ex As Exception
                            Console.WriteLine("Exception Invoke" & ex.Message)


                        End Try

                    End If
                End If
            End If
        End If
        lock = False
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If lock = True Then
            Exit Sub
        End If
        If _ch_checked_modified Then
            Timer1.Enabled = False
            ChangeLegendList()
            _ch_checked_modified = False
            Timer1.Enabled = True
        End If

        Task.Factory.StartNew(Sub()
                                  SingleShotA()
                              End Sub)

        'Thread1 = New System.Threading.Thread(AddressOf SingleShotA)

        'Thread1.Start()


    End Sub

    Private Sub pScope_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel

        Dim mwe As HandledMouseEventArgs = DirectCast(e, HandledMouseEventArgs)
        mwe.Handled = True

    End Sub

    Private Sub Pesgo1_Click(sender As Object, e As EventArgs) Handles Pesgo1.Click

    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork

    End Sub

    Private Sub TableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles TableLayoutPanel1.Paint

    End Sub
End Class

