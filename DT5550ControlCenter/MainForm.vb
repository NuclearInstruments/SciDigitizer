Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
Imports WeifenLuo.WinFormsUI.Docking
Imports Newtonsoft.Json
Imports System.Xml
Imports Gigasoft.ProEssentials
Imports Gigasoft.ProEssentials.Enums
Imports DT5550ControlCenter.AcquisitionClass

Public Class MainForm

    Public Ts As Double = 12.5
    Dim msgcoda As New Queue
    Dim CurrentResources As SciCompilerExportClass
    Public CurrentOscilloscope As New Oscilloscope
    Public CurrentMCA As New FrameTransfers
    Public CurrentI2C As New IdueC
    Public CurrentRegisterList As New List(Of Register)
    Public fit_enabled = False
    Dim list_dockPanel As New List(Of DockContent)
    Public dockPanel As New DockPanel()
    Public plog As New logwin
    Public fit As fit_win
    Public isChargeIntegration As Boolean = False
    Public spect As New pSpectra
    Public map As New Mapping
    Public sets As New Settings
    Public scope As New pOscilloscope
    Public pImm1 As New pImmediate(True)
    Public pImm2 As New pImmediate(False)
    Public Shared acquisition As New AcquisitionClass(32)
    Public isSpectra = False

    Public Sub AppendToLog(text As String)
        msgcoda.Enqueue(text)
    End Sub

    Public Sub MDIParent_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "SCI-5550 Readout Software (NI - CAEN ) "
        Dim file As String = Connection.Jsonfile
        Create(file)

        acquisition = New AcquisitionClass(CurrentOscilloscope.Channels)

        If Connection.CustomFirmware = False Then
            DefaultFirmwareMapping()
        End If

        CreateGUI()

    End Sub

    Public Sub Create(file As String)

        Dim sr As New IO.StreamReader(file)
        CurrentResources = Nothing
        CurrentResources = JsonConvert.DeserializeObject(Of SciCompilerExportClass)(sr.ReadToEnd)
        sr.Close()

        If Connection.ComClass.IsFileCompatible(CurrentResources.Device) Then

            If CurrentResources.MMCComponents.Oscilloscopes IsNot Nothing Then
                CurrentOscilloscope.Name = CurrentResources.MMCComponents.Oscilloscopes(0).Name
                CurrentOscilloscope.Address = CurrentResources.MMCComponents.Oscilloscopes(0).Address
                CurrentOscilloscope.Channels = CurrentResources.MMCComponents.Oscilloscopes(0).Channels
                CurrentOscilloscope.Version = CurrentResources.MMCComponents.Oscilloscopes(0).Version
                CurrentOscilloscope.nsamples = CurrentResources.MMCComponents.Oscilloscopes(0).nsamples
                CurrentOscilloscope.Registers = CurrentResources.MMCComponents.Oscilloscopes(0).Registers

                For Each r In CurrentOscilloscope.Registers
                    If r.Name = "CONFIG_DECIMATOR" Then
                        scope.addressDecimator = r.Address
                    End If
                    If r.Name = "CONFIG_PRETRIGGER" Then
                        scope.addressPre = r.Address
                    End If
                    If r.Name = "CONFIG_TRIGGER_MODE" Then
                        scope.addressMode = r.Address
                    End If
                    If r.Name = "CONFIG_TRIGGER_LEVEL" Then
                        scope.addressLevel = r.Address
                    End If
                    If r.Name = "CONFIG_ARM" Then
                        scope.addressArm = r.Address
                    End If
                    If r.Name = "READ_STATUS" Then
                        scope.addressStatus = r.Address
                    End If
                    If r.Name = "READ_POSITION" Then
                        scope.addressPosition = r.Address
                    End If
                Next
            End If

            For Each r In CurrentResources.Registers
                CurrentRegisterList.Add(r)
                If r.Name = "IS_CHARGE_INT" Then
                    Dim value As Integer
                    If Connection.ComClass.GetRegister(r.Address, value) = 0 Then
                        If value = 0 Then
                            isChargeIntegration = False
                        Else
                            isChargeIntegration = True
                        End If
                    End If
                End If
            Next

            If CurrentResources.MMCComponents.FrameTransfer IsNot Nothing Then
                CurrentMCA.Name = CurrentResources.MMCComponents.FrameTransfer(0).Name
                CurrentMCA.Address = CurrentResources.MMCComponents.FrameTransfer(0).Address
                CurrentMCA.Channels = CurrentResources.MMCComponents.FrameTransfer(0).Channels
                CurrentMCA.Version = CurrentResources.MMCComponents.FrameTransfer(0).Version
                CurrentMCA.Registers = CurrentResources.MMCComponents.FrameTransfer(0).Registers

                For Each r In CurrentMCA.Registers
                    If r.Name = "CONFIG_T0_MASK" Then
                        spect.addressMask = r.Address
                    End If
                    If r.Name = "CONFIG_WAIT" Then
                        spect.addressWait = r.Address
                    End If
                    If r.Name = "CONFIG_TRIGGER_MODE" Then
                        spect.addressMode = r.Address
                    End If
                    If r.Name = "CONFIG_ARM" Then
                        spect.addressArm = r.Address
                    End If
                    If r.Name = "READ_STATUS" Then
                        spect.addressStatus = r.Address
                    End If
                    If r.Name = "CONFIG_SYNC" Then
                        spect.addressSync = r.Address
                    End If
                Next
            End If

            If CurrentResources.MMCComponents.i2c IsNot Nothing Then
                CurrentI2C.Name = CurrentResources.MMCComponents.i2c(0).Name
                CurrentI2C.Address = CurrentResources.MMCComponents.i2c(0).Address
                CurrentI2C.Version = CurrentResources.MMCComponents.i2c(0).Version
                CurrentI2C.Registers = CurrentResources.MMCComponents.i2c(0).Registers
                Dim regstatus As New UInt32
                Dim regcontrol As New UInt32

                For Each r In CurrentI2C.Registers
                    If r.Name = "REG_CTRL" Then
                        regcontrol = r.Address
                    End If
                    If r.Name = "REG_MON" Then
                        regstatus = r.Address
                    End If
                Next
                Connection.ComClass.AFE_SetIICBaseAddress(regcontrol, regstatus)
            Else
                plog.TextBox1.AppendText(vbCrLf & "I2C missing!" & vbCrLf)
            End If
        Else
            MsgBox("The file is not compatible with the selected board!", vbCritical + vbOKOnly)
            Exit Sub
        End If

    End Sub

    Private Function GetDockContentForm(name As String, showHint As DockState, backColour As Color) As DockContent

        Dim content1 As New DockContent()
        content1.Name = name
        content1.TabText = name
        content1.Text = name
        content1.ShowHint = showHint
        content1.BackColor = backColour
        Return content1

    End Function

    Public Sub CreateGUI()

        dockPanel.Dock = DockStyle.Fill
        dockPanel.BackColor = Color.White
        Controls.Add(dockPanel)
        dockPanel.BringToFront()
        If CurrentMCA IsNot Nothing Then
            Dim content1a As DockContent = GetDockContentForm("Spectrum", DockState.Document, Color.White)
            content1a.Show(dockPanel)
            content1a.CloseButtonVisible = False
            content1a.Controls.Add(spect)
            spect.Dock = DockStyle.Fill
            list_dockPanel.Add(content1a)
        End If
        If CurrentOscilloscope IsNot Nothing Then
            Dim content1b As DockContent = GetDockContentForm("Oscilloscope", DockState.Document, Color.White)
            content1b.Show(dockPanel)
            content1b.CloseButtonVisible = False
            content1b.Controls.Add(scope)
            scope.Dock = DockStyle.Fill
            list_dockPanel.Add(content1b)
        End If
        Dim content1c As DockContent = GetDockContentForm("Settings", DockState.Document, Color.White)
        content1c.Show(dockPanel)
        content1c.CloseButtonVisible = False
        content1c.Controls.Add(sets)
        sets.Dock = DockStyle.Fill
        list_dockPanel.Add(content1c)
        If Connection.CustomFirmware Then
            Dim content1d As DockContent = GetDockContentForm("Mapping", DockState.Document, Color.White)
            content1d.Show(dockPanel)
            content1d.CloseButtonVisible = False
            content1d.Controls.Add(map)
            map.Dock = DockStyle.Fill
            list_dockPanel.Add(content1d)
        End If
        If CurrentMCA IsNot Nothing Then
            Dim content2 As DockContent = GetDockContentForm("Real Time View", DockState.DockRight, Color.White)
            content2.Show(dockPanel)
            content2.CloseButtonVisible = False
            content2.Controls.Add(pImm1)
            pImm1.Dock = DockStyle.Fill
            list_dockPanel.Add(content2)
            Dim content3 As DockContent = GetDockContentForm("Cumulative", DockState.Float, Color.White)
            content3.Show(dockPanel)
            content3.CloseButtonVisible = False
            content3.DockHandler.FloatPane.DockTo(dockPanel.DockWindows(DockState.DockRight))
            content3.Controls.Add(pImm2)
            pImm2.Dock = DockStyle.Fill
            list_dockPanel.Add(content3)
        End If
        Dim content4 As DockContent = GetDockContentForm("Log File", DockState.DockBottom, Color.White)
        content4.Show(dockPanel)
        content4.CloseButtonVisible = False
        plog.Dock = DockStyle.Fill
        content4.Controls.Add(plog)
        dockPanel.DockBottomPortion = 0.15
        list_dockPanel.Add(content4)

    End Sub

    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click, OpenToolStripButton.Click

        Dim oDialog As New OpenFileDialog
        oDialog.Filter = "XML File (*.xml)|*.xml"

        Try
            If oDialog.ShowDialog() = DialogResult.OK Then
                Dim xmldoc As New XmlDocument()
                'Dim xmlnode As XmlNodeList
                'Dim i As Integer
                'Dim str As String
                Dim fs As New FileStream(oDialog.FileName, FileMode.Open, FileAccess.Read)
                xmldoc.Load(fs)
                acquisition.General_settings.AFEImpedance = IIf(xmldoc.SelectSingleNode("Settings/General_Settings/AFE_Impedance").InnerText.ToString = "50 Ohm", True, False)
                acquisition.General_settings.AFEOffset = xmldoc.SelectSingleNode("Settings/General_Settings/AFE_Offset").InnerText
                acquisition.General_settings.SignalOffset = xmldoc.SelectSingleNode("Settings/General_Settings/Signal_Offset").InnerText
                acquisition.General_settings.TriggerSource = IIf(xmldoc.SelectSingleNode("Settings/General_Settings/Trigger_Source").InnerText = "Internal", trigger_source.INTERNAL, trigger_source.EXTERNAL)
                acquisition.General_settings.TriggerDelay = xmldoc.SelectSingleNode("Settings/General_Settings/Trigger_Delay").InnerText
                acquisition.General_settings.TriggerMode = IIf(xmldoc.SelectSingleNode("Settings/General_Settings/Trigger_Mode").InnerText = "Threshold", trigger_mode.THRESHOLD, trigger_mode.DERIVATIVE)
                acquisition.General_settings.Sampling = IIf(xmldoc.SelectSingleNode("Settings/General_Settings/Sampling_Method").InnerText = "Common", sampling_method.COMMON, sampling_method.INDIPENDENT)
                Dim isChargeIntegrationCurrent = xmldoc.SelectSingleNode("Settings/General_Settings/IsChargeIntegrator").InnerText
                If xmldoc.SelectSingleNode("Settings/Oscilloscope_Settings/Oscilloscope_Trigger_Source").InnerText = "Internal" Then
                    acquisition.General_settings.TriggerSourceOscilloscope = trigger_source.INTERNAL
                ElseIf xmldoc.SelectSingleNode("Settings/Oscilloscope_Settings/Oscilloscope_Trigger_Source").InnerText = "External" Then
                    acquisition.General_settings.TriggerSourceOscilloscope = trigger_source.EXTERNAL
                Else
                    acquisition.General_settings.TriggerSourceOscilloscope = trigger_source.LEVEL
                    acquisition.General_settings.TriggerChannelOscilloscope = xmldoc.SelectSingleNode("Settings/Oscilloscope_Settings/Oscilloscope_Trigger_Source").InnerText.Replace("CHANNEL ", "") - 1
                End If
                acquisition.General_settings.TriggerOscilloscopeEdges = IIf(xmldoc.SelectSingleNode("Settings/Oscilloscope_Settings/Oscilloscope_Trigger_Edge").InnerText = "Rising", edge.RISING, edge.FALLING)
                acquisition.General_settings.TriggerOscilloscopeLevel = xmldoc.SelectSingleNode("Settings/Oscilloscope_Settings/Oscilloscope_Trigger_Level").InnerText
                acquisition.General_settings.OscilloscopePreTrigger = xmldoc.SelectSingleNode("Settings/Oscilloscope_Settings/Oscilloscope_Pre_Trigger").InnerText
                acquisition.General_settings.OscilloscopeDecimator = CType(xmldoc.SelectSingleNode("Settings/Oscilloscope_Settings/Oscilloscope_Trigger_Decimator").InnerText, Double) / 12.5
                Dim k = 0
                For Each node In xmldoc.SelectSingleNode("Settings").LastChild.ChildNodes
                    acquisition.CHList(k).polarity = IIf(node.ChildNodes.Item(0).innertext = "Positive", signal_polarity.POSITIVE, signal_polarity.NEGATIVE)
                    acquisition.CHList(k).trigger_level = node.ChildNodes.Item(1).innertext
                    acquisition.CHList(k).trigger_inhibit = node.ChildNodes.Item(2).innertext
                    If isChargeIntegrationCurrent Then
                        acquisition.CHList(k).integration_time = node.ChildNodes.Item(3).innertext
                        acquisition.CHList(k).pre_gate = node.ChildNodes.Item(4).innertext
                        acquisition.CHList(k).gain = node.ChildNodes.Item(5).innertext
                        acquisition.CHList(k).pileup_enable = CBool(node.ChildNodes.Item(6).innertext)
                        acquisition.CHList(k).pileup_time = node.ChildNodes.Item(7).innertext
                        acquisition.CHList(k).baseline_inhibit = node.ChildNodes.Item(8).innertext
                        acquisition.CHList(k).baseline_sample = node.ChildNodes.Item(9).innertext
                    Else
                        acquisition.CHList(k).decay_constant = node.ChildNodes.Item(3).innertext
                        acquisition.CHList(k).peaking_time = node.ChildNodes.Item(4).innertext
                        acquisition.CHList(k).flat_top = node.ChildNodes.Item(5).innertext
                        acquisition.CHList(k).energy_sample = node.ChildNodes.Item(6).innertext
                        acquisition.CHList(k).gain = node.ChildNodes.Item(7).innertext
                        acquisition.CHList(k).pileup_enable = node.ChildNodes.Item(8).innertext
                        acquisition.CHList(k).pileup_time = node.ChildNodes.Item(9).innertext
                        acquisition.CHList(k).baseline_inhibit = node.ChildNodes.Item(10).innertext
                        acquisition.CHList(k).baseline_sample = node.ChildNodes.Item(11).innertext
                    End If
                    k += 1
                Next
                fs.Close()
                sets.Settings_reload()
                sets.Grid_ReLoad()
                plog.TextBox1.AppendText("Settings loaded from file." & vbCrLf)
            End If
        Catch ex As Exception
            plog.TextBox1.AppendText("Error: " & ex.Message & vbCrLf)
        End Try

    End Sub

    Private Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click

        plog.TextBox1.Select()
        Dim sDialog As New SaveFileDialog()
        sDialog.DefaultExt = ".xml"
        sDialog.Filter = "XML File (*.xml)|*.xml"

        Try
            If sDialog.ShowDialog() = DialogResult.OK Then
                Dim writer As New XmlTextWriter(sDialog.FileName, System.Text.Encoding.UTF8)
                writer.WriteStartDocument(True)
                writer.Formatting = Xml.Formatting.Indented
                writer.Indentation = 2
                writer.WriteStartElement("Settings")
                writer.WriteStartElement("General_Settings")
                writer.WriteElementString("AFE_Impedance", sets.Impedance.SelectedItem)
                writer.WriteElementString("AFE_Offset", sets.Offset.Value)
                writer.WriteElementString("Signal_Offset", sets.SignalOffset.Value)
                writer.WriteElementString("Trigger_Source", sets.TriggerSource.SelectedItem)
                writer.WriteElementString("Trigger_Delay", sets.TriggerDelay.Value)
                writer.WriteElementString("Trigger_Mode", sets.TriggerMode.SelectedItem)
                writer.WriteElementString("Sampling_Method", sets.sampling.SelectedItem)
                writer.WriteElementString("IsChargeIntegrator", isChargeIntegration)
                writer.WriteEndElement()
                writer.WriteStartElement("Oscilloscope_Settings")
                writer.WriteElementString("Oscilloscope_Trigger_Source", sets.TriggerSourceOscilloscope.SelectedItem)
                writer.WriteElementString("Oscilloscope_Trigger_Edge", sets.TriggerEdge.SelectedItem)
                writer.WriteElementString("Oscilloscope_Trigger_Level", sets.TriggerLevelOscilloscope.Text)
                writer.WriteElementString("Oscilloscope_Trigger_Decimator", sets.Horizontal.Text)
                writer.WriteElementString("Oscilloscope_Pre_Trigger", sets.PreTrigger.Text)
                writer.WriteEndElement()
                writer.WriteStartElement("Channels_Settings")
                For i = 0 To CurrentOscilloscope.Channels - 1
                    writer.WriteStartElement(sets.DataGridView1.Rows(i).Cells("Channel").Value.Replace(" ", "_"))
                    writer.WriteElementString("Polarity", sets.DataGridView1.Rows(i).Cells("Polarity").Value)
                    writer.WriteElementString("Trigger_Level", sets.DataGridView1.Rows(i).Cells("Trigger Level").Value)
                    writer.WriteElementString("Trigger_Hold-Off", sets.DataGridView1.Rows(i).Cells("Trigger Hold-Off").Value)
                    If isChargeIntegration Then
                        writer.WriteElementString("Integration_Time", sets.DataGridView1.Rows(i).Cells("Integration Time").Value)
                        writer.WriteElementString("Pre-gate", sets.DataGridView1.Rows(i).Cells("Pre-gate").Value)
                    Else
                        writer.WriteElementString("Decay_Constant", sets.DataGridView1.Rows(i).Cells("Decay Constant").Value)
                        writer.WriteElementString("Peaking_Time", sets.DataGridView1.Rows(i).Cells("Peaking Time").Value)
                        writer.WriteElementString("Flat_Top", sets.DataGridView1.Rows(i).Cells("Flat Top").Value)
                        writer.WriteElementString("Energy_Sample", sets.DataGridView1.Rows(i).Cells("Energy Sample").Value)
                    End If
                    writer.WriteElementString("Gain", sets.DataGridView1.Rows(i).Cells("Gain").Value)
                    writer.WriteElementString("Pileup_Rejection", sets.DataGridView1.Rows(i).Cells("Pileup Rejection").Value)
                    writer.WriteElementString("Pileup_Rejection_Time", sets.DataGridView1.Rows(i).Cells("Pileup Rejection Time").Value)
                    writer.WriteElementString("Baseline_Inhibit_Time", sets.DataGridView1.Rows(i).Cells("Baseline Inhibit Time").Value)
                    writer.WriteElementString("Baseline_Lenght", sets.DataGridView1.Rows(i).Cells("Baseline Lenght").Value)
                    writer.WriteEndElement()
                Next
                writer.WriteEndElement()
                writer.WriteEndDocument()
                writer.Close()
                plog.TextBox1.AppendText("Setting file saved." & vbCrLf)
            End If
        Catch ex As Exception
            plog.TextBox1.AppendText("Error: " & ex.Message & vbCrLf)
        End Try

    End Sub

    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
        Me.ToolStrip.Visible = Me.ToolBarToolStripMenuItem.Checked
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        Me.StatusStrip.Visible = Me.StatusBarToolStripMenuItem.Checked
    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

    Private Sub FreeRunStart_Click(sender As Object, e As EventArgs) Handles FreeRunStart.Click

        If IsNothing(scope) Then
        Else
            scope.StartFreeRunningMultiAcquisition()
            FreeRunStart.Enabled = False
            FreeRunStop.Enabled = True
            SingleShot.Enabled = False
            StartSpectrum.Enabled = False
            StopSpectrum.Enabled = False
            SaveData.Enabled = True
            StopSaveData.Enabled = False
        End If

    End Sub

    Private Sub FreeRunStop_Click(sender As Object, e As EventArgs) Handles FreeRunStop.Click

        If scope.fileEnable = True Then
            scope.StopDataCaptureOnFile()
        End If
        scope.StopAcquisition()
        FreeRunStart.Enabled = True
        FreeRunStop.Enabled = False
        SingleShot.Enabled = True
        StartSpectrum.Enabled = True
        StopSpectrum.Enabled = False
        SaveData.Enabled = True
        StopSaveData.Enabled = False

    End Sub

    Private Sub SingleShot_Click(sender As Object, e As EventArgs) Handles SingleShot.Click

        If IsNothing(scope) Then
        Else
            If scope.running = False Then
                scope.SingleShot()
            Else
                scope.StopAcquisition()
            End If
        End If

    End Sub

    Private Sub StartSpectrum_Click(sender As Object, e As EventArgs) Handles StartSpectrum.Click

        If IsNothing(spect) Then
        Else
            spect.startspectrum()
            pImm1.Timer1.Enabled = True
            pImm2.Timer1.Enabled = True

            FreeRunStart.Enabled = False
            FreeRunStop.Enabled = False
            SingleShot.Enabled = False
            StartSpectrum.Enabled = False
            StopSpectrum.Enabled = True
            SaveData.Enabled = True
            StopSaveData.Enabled = False
        End If

    End Sub

    Private Sub ResetSpectrum_Click_1(sender As Object, e As EventArgs) Handles ResetSpectrum.Click

        If IsNothing(spect) Then
        Else
            spect.resetspectrum()
        End If

    End Sub

    Private Sub StopSpectrum_Click(sender As Object, e As EventArgs) Handles StopSpectrum.Click

        If spect.fileEnable Then
            spect.StopDataCaptureOnFile()
        End If
        spect.stopspectrum()
        pImm1.Timer1.Enabled = False
        pImm2.Timer1.Enabled = False

        FreeRunStart.Enabled = True
        FreeRunStop.Enabled = False
        SingleShot.Enabled = True
        StartSpectrum.Enabled = True
        StopSpectrum.Enabled = False
        SaveData.Enabled = True
        StopSaveData.Enabled = False

    End Sub

    Private Sub SaveData_Click(sender As Object, e As EventArgs) Handles SaveData.Click

        Dim g As New WaveformCaptureSelect
        If g.ShowDialog = DialogResult.OK Then
            If isSpectra Then
                If IsNothing(spect) Then
                Else
                    For i = 0 To g.ChList.Items.Count - 1
                        spect.EnabledChannel(i) = g.ChList.GetItemCheckState(i)
                        spect.EnabledChannel_id(i) = acquisition.CHList(i).id
                    Next
                    spect.spectracount = 0
                    spect.TargetMode = g.TargetMode.SelectedIndex
                    If spect.TargetMode = 1 Then
                        spect.TargetEvent = g.TargetValue.Text
                    End If
                    If spect.TargetMode = 2 Then
                        If g.TargetValueUnit.SelectedIndex = 0 Then
                            spect.TargetEvent = g.TargetValue.Text
                        ElseIf g.TargetValueUnit.SelectedIndex = 1 Then
                            spect.TargetEvent = g.TargetValue.Text * 60
                        ElseIf g.TargetValueUnit.SelectedIndex = 2 Then
                            spect.TargetEvent = g.TargetValue.Text * 60 * 60
                        End If
                    End If
                    If scope.running Then
                        scope.StopAcquisition()
                        System.Threading.Thread.Sleep(1000)
                    End If
                    spect.StartDataCaptureOnFile(g.FileName.Text)
                    If spect.running = False Then
                        spect.startspectrum()
                        pImm1.Timer1.Enabled = True
                        pImm2.Timer1.Enabled = True
                    End If
                    FreeRunStart.Enabled = False
                    FreeRunStop.Enabled = False
                    SingleShot.Enabled = False
                    StartSpectrum.Enabled = False
                    StopSpectrum.Enabled = True
                    SaveData.Enabled = False
                    StopSaveData.Enabled = True
                End If
            Else
                If IsNothing(scope) Then
                Else
                    For i = 0 To g.ChList.Items.Count - 1
                        scope.EnabledChannel(i) = g.ChList.GetItemCheckState(i)
                        scope.EnabledChannel_id(i) = acquisition.CHList(i).id
                    Next
                    scope.wavecount = 0
                    scope.TargetMode = g.TargetMode.SelectedIndex
                    If scope.TargetMode = 1 Then
                        scope.TargetEvent = g.TargetValue.Text
                    End If
                    If spect.running Then
                        spect.stopspectrum()
                        System.Threading.Thread.Sleep(1000)
                    End If
                    If scope.running = False Then
                        scope.StartFreeRunningMultiAcquisition()
                    End If
                    scope.StartDataCaptureOnFile(g.FileName.Text)
                    FreeRunStart.Enabled = False
                    FreeRunStop.Enabled = True
                    SingleShot.Enabled = False
                    StartSpectrum.Enabled = False
                    StopSpectrum.Enabled = False
                    SaveData.Enabled = False
                    StopSaveData.Enabled = True
                End If
            End If

        End If
    End Sub

    Private Sub StopSaveSpectrum_Click(sender As Object, e As EventArgs) Handles StopSaveData.Click

        If isSpectra Then
            spect.StopDataCaptureOnFile()
        Else
            scope.StopDataCaptureOnFile()
        End If
        SaveData.Enabled = True
        StopSaveData.Enabled = False

    End Sub

    Private Sub MainForm_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        End
    End Sub

    Private Sub MainForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        spect.Pesgo1.Dispose()
        spect.Timer1.Enabled = False
        spect.Dispose()
        scope.Pesgo1.Dispose()
        scope.Timer1.Enabled = False
        scope.Dispose()
        System.Threading.Thread.Sleep(1000)

    End Sub

    Private Sub FittingSpectrum_Click(sender As Object, e As EventArgs) Handles FittingSpectrum.Click

        If fit_enabled = False Then
            fit_enabled = True
            fit = New fit_win
            Dim content5 As DockContent = GetDockContentForm("Fit", DockState.DockBottom, Color.White)
            content5.Show(dockPanel)
            content5.CloseButtonVisible = False
            fit.Dock = DockStyle.Fill
            content5.Controls.Add(fit)
            dockPanel.DockBottomPortion = 0.15
            list_dockPanel.Add(content5)
        Else
            fit_enabled = False
            For i = 0 To list_dockPanel.Count - 1
                If list_dockPanel(i).Name = "Fit" Then
                    list_dockPanel(i).Dispose()
                    list_dockPanel.RemoveAt(i)
                    Exit For
                End If
            Next
        End If

    End Sub

    Public Sub DefaultFirmwareMapping()

        For i = 1 To 32
            Dim new_id As Integer
            Select Case i
                Case 1
                    new_id = 23
                Case 2
                    new_id = 27
                Case 3
                    new_id = 25
                Case 4
                    new_id = 31
                Case 5
                    new_id = 29
                Case 6
                    new_id = 30
                Case 7
                    new_id = 32
                Case 8
                    new_id = 26
                Case 9
                    new_id = 28
                Case 10
                    new_id = 21
                Case 11
                    new_id = 22
                Case 12
                    new_id = 24
                Case 13
                    new_id = 17
                Case 14
                    new_id = 18
                Case 15
                    new_id = 19
                Case 16
                    new_id = 20
                Case 17
                    new_id = 13
                Case 18
                    new_id = 14
                Case 19
                    new_id = 15
                Case 20
                    new_id = 16
                Case 21
                    new_id = 9
                Case 22
                    new_id = 10
                Case 23
                    new_id = 11
                Case 24
                    new_id = 12
                Case 25
                    new_id = 6
                Case 26
                    new_id = 5
                Case 27
                    new_id = 8
                Case 28
                    new_id = 7
                Case 29
                    new_id = 2
                Case 30
                    new_id = 1
                Case 31
                    new_id = 4
                Case 32
                    new_id = 3
            End Select
            acquisition.CHList(i - 1).id = new_id
        Next

    End Sub

    Private Sub SaveSpectrum_Click(sender As Object, e As EventArgs) Handles SaveSpectrum.Click
        Dim sfd As New SaveFileDialog
        sfd.Title = "Save Spectrum File"
        sfd.DefaultExt = ".csv"
        sfd.Filter = " (*.csv)|*.csv"
        If sfd.ShowDialog = DialogResult.OK Then
            spect.saveSpectrum(sfd.FileName)
        End If
        Dim sfd3 As New SaveFileDialog
        sfd3.Title = "Save Cumulative Image File"
        sfd3.DefaultExt = ".csv"
        sfd3.Filter = " (*.csv)|*.csv"
        If sfd3.ShowDialog = DialogResult.OK Then
            spect.saveImage(sfd3.FileName)
        End If
    End Sub



    Private Sub LogLinToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogLinToolStripMenuItem.Click
        spect.logmode = Not spect.logmode
        If spect.logmode Then
            spect.Pesgo1.PeGrid.Configure.YAxisScaleControl = Gigasoft.ProEssentials.Enums.ScaleControl.Log
        End If
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub StepToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StepToolStripMenuItem.Click
        spect.method = SGraphPlottingMethods.Step
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub LineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineToolStripMenuItem.Click
        spect.method = SGraphPlottingMethods.Line
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub LineWithInterpolationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineWithInterpolationToolStripMenuItem.Click
        spect.method = SGraphPlottingMethods.Spline
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub DotToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DotToolStripMenuItem.Click
        spect.method = SGraphPlottingMethods.Point
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub DotWithLineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DotWithLineToolStripMenuItem.Click
        spect.method = SGraphPlottingMethods.PointsPlusLine
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub DotWithInterpolationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DotWithInterpolationToolStripMenuItem.Click
        spect.method = SGraphPlottingMethods.PointsPlusSpline
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub AreaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AreaToolStripMenuItem.Click
        spect.method = SGraphPlottingMethods.Area
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub AreaWithInterpolationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AreaWithInterpolationToolStripMenuItem.Click
        spect.method = SGraphPlottingMethods.SplineArea
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub BarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BarToolStripMenuItem.Click
        spect.method = SGraphPlottingMethods.Bar
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub AreaToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AreaToolStripMenuItem1.Click
        spect.Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.HorzAndVert
        scope.Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.HorzAndVert
        spect.Pesgo1.Refresh()
        scope.Pesgo1.Refresh()
    End Sub

    Private Sub HorizontalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HorizontalToolStripMenuItem.Click
        spect.Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.Horizontal
        scope.Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.Horizontal
        spect.Pesgo1.Refresh()
        scope.Pesgo1.Refresh()
    End Sub

    Private Sub VerticalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerticalToolStripMenuItem.Click
        spect.Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.Vertical
        scope.Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.Vertical
        spect.Pesgo1.Refresh()
        scope.Pesgo1.Refresh()
    End Sub

    Private Sub UnzoomToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnzoomToolStripMenuItem.Click
        spect.Pesgo1.PeFunction.UndoZoom()
        scope.Pesgo1.PeFunction.UndoZoom()
        spect.Pesgo1.Refresh()
        scope.Pesgo1.Refresh()
    End Sub

    Private Sub OscilloscopeImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OscilloscopeImageToolStripMenuItem.Click
        scope.Pesgo1.PeFunction.Dialog.Export()
    End Sub

    Private Sub SpectrumImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SpectrumImageToolStripMenuItem.Click
        spect.Pesgo1.PeFunction.Dialog.Export()
    End Sub

    Private Sub OscilloscopeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OscilloscopeToolStripMenuItem.Click
        scope.Pesgo1.PeFunction.Dialog.Print(True, 0, 0)
    End Sub

    Private Sub SpectrumToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SpectrumToolStripMenuItem1.Click
        spect.Pesgo1.PeFunction.Dialog.Print(True, 0, 0)
    End Sub

    Private Sub RebinToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RebinToolStripMenuItem.Click

    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        spect.SpectrumLength = 65536
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        spect.SpectrumLength = 32768
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
        spect.SpectrumLength = 16384
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub ToolStripMenuItem5_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem5.Click
        spect.SpectrumLength = 8192
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub ToolStripMenuItem6_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem6.Click
        spect.SpectrumLength = 4096
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub ToolStripMenuItem7_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem7.Click
        spect.SpectrumLength = 2048
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub ToolStripMenuItem8_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem8.Click
        spect.SpectrumLength = 1024
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub ToolStripMenuItem9_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem9.Click
        spect.SpectrumLength = 512
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub ToolStripMenuItem10_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem10.Click
        spect.SpectrumLength = 256
        spect.Pesgo1.Refresh()
    End Sub

    Private Sub WavedumpToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim g As New WaveformCaptureSelect
        If g.ShowDialog = DialogResult.OK Then
            If isSpectra Then
                If IsNothing(spect) Then
                Else
                    For i = 0 To g.ChList.Items.Count - 1
                        spect.EnabledChannel(i) = g.ChList.GetItemCheckState(i)
                        spect.EnabledChannel_id(i) = acquisition.CHList(i).id
                    Next
                    spect.spectracount = 0
                    spect.TargetMode = g.TargetMode.SelectedIndex
                    If spect.TargetMode = 1 Then
                        spect.TargetEvent = g.TargetValue.Text
                    End If
                    If spect.TargetMode = 2 Then
                        If g.TargetValueUnit.SelectedIndex = 0 Then
                            spect.TargetEvent = g.TargetValue.Text
                        ElseIf g.TargetValueUnit.SelectedIndex = 1 Then
                            spect.TargetEvent = g.TargetValue.Text * 60
                        ElseIf g.TargetValueUnit.SelectedIndex = 2 Then
                            spect.TargetEvent = g.TargetValue.Text * 60 * 60
                        End If
                    End If
                    If scope.running Then
                        scope.StopAcquisition()
                        System.Threading.Thread.Sleep(1000)
                    End If
                    spect.StartDataCaptureOnFile(g.FileName.Text)
                    If spect.running = False Then
                        spect.startspectrum()
                        pImm1.Timer1.Enabled = True
                        pImm2.Timer1.Enabled = True
                    End If
                    FreeRunStart.Enabled = False
                    FreeRunStop.Enabled = False
                    SingleShot.Enabled = False
                    StartSpectrum.Enabled = False
                    StopSpectrum.Enabled = True
                    SaveData.Enabled = False
                    StopSaveData.Enabled = True
                End If
            Else
                If IsNothing(scope) Then
                Else
                    For i = 0 To g.ChList.Items.Count - 1
                        scope.EnabledChannel(i) = g.ChList.GetItemCheckState(i)
                        scope.EnabledChannel_id(i) = acquisition.CHList(i).id
                    Next
                    scope.wavecount = 0
                    scope.TargetMode = g.TargetMode.SelectedIndex
                    If scope.TargetMode = 1 Then
                        scope.TargetEvent = g.TargetValue.Text
                    End If
                    If spect.running Then
                        spect.stopspectrum()
                        System.Threading.Thread.Sleep(1000)
                    End If
                    If scope.running = False Then
                        scope.StartFreeRunningMultiAcquisition()
                    End If
                    scope.StartDataCaptureOnFile(g.FileName.Text)
                    FreeRunStart.Enabled = False
                    FreeRunStop.Enabled = True
                    SingleShot.Enabled = False
                    StartSpectrum.Enabled = False
                    StopSpectrum.Enabled = False
                    SaveData.Enabled = False
                    StopSaveData.Enabled = True
                End If
            End If

        End If
    End Sub

    Private Sub FieToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FieToolStripMenuItem.Click

    End Sub
End Class
