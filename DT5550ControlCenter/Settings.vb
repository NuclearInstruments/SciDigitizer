Imports System.ComponentModel
Imports System.Globalization
Imports System.Reflection
Imports DT5550ControlCenter.AcquisitionClass

Public Class Settings

    Dim inhibit = True
    Dim inhibit2 = True
    Dim l As Integer = 814
    Dim n As Integer
    Dim sampling_factor As Double
    Dim AnalogOffsetControl As NumericUpDown
    Public DataGridView2 As New DataGridView
    Dim shaper As New ComboBox
    Dim first_load = True

    'Dim gain_list = {1, 1.06, 1.1, 1.2, 1.26, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2, 2.1, 2.2, 2.4, 2.5, 2.7, 2.8, 3, 3.2, 3.3, 3.5, 3.8, 4, 4.2, 4.5, 4.7, 5, 5.3, 5.6, 6, 6.3, 6.7, 7.1, 7.5, 7.9, 8.4, 8.9, 9.4, 10, 10.6, 11.2, 11.9, 12.6, 13.3,
    '                14.1, 15, 15.8, 16.8, 17.8, 18.8, 20, 21.1, 22.4, 23.7, 25.1, 26.6, 28.2, 29.9, 31.6, 33.5, 37.6, 39.8, 42.2, 44.7, 47.3, 50.1, 53.1, 56.2, 59.6, 63.1, 66.8, 70.8, 75, 79.4, 84.1, 89.1, 94.4, 100}

    Dim gain_list = {1, 2, 3, 4, 5, 6, 10, 15, 20, 30, 50, 75, 100}

    Public Sub UpdateButtonStatusApply()
        Apply.Enabled = True
        Apply.BackColor = Color.DodgerBlue
    End Sub
    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
            sampling_factor = 1000 / 80
        ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Then
            sampling_factor = 1000 / 125
        ElseIf Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
            sampling_factor = 1000 / 125
        ElseIf Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            sampling_factor = 1000 / 60
        End If
        Horizontal.Increment = sampling_factor

        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then

            Impedance.Items.Clear()
            Impedance.Items.Add("50 Ohm")
            Impedance.Items.Add("1 kOhm")

            TriggerMode.Items.Clear()
            TriggerMode.Items.Add("Derivate")
            TriggerMode.Items.Add("Threshold")

            TriggerSource.Items.Clear()
            TriggerSource.Items.Add("Internal")
            TriggerSource.Items.Add("External")

            TriggerDelay.Enabled = False
            TriggerDelayLabel.Enabled = False

            sampling.Items.Clear()
            sampling.Items.Add("Common")
            sampling.Items.Add("Independent")

            TriggerSourceOscilloscope.Items.Clear()
            TriggerSourceOscilloscope.Items.Add("Internal")
            TriggerSourceOscilloscope.Items.Add("External")
            TriggerSourceOscilloscope.Items.Add("Free Running")

            For Each i In MainForm.acquisition.CHList
                TriggerSourceOscilloscope.Items.Add(i.name)
            Next

        ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Then
            GroupBox1.SetBounds(0, 0, 0, 0)
            TableLayoutPanel1.RowStyles(0).Height = 110
            TriggerSourceOscilloscope.Items.Clear()
            TriggerSourceOscilloscope.Items.Add("Analog Signal")
            TriggerSourceOscilloscope.Items.Add("MCA Trigger")
            TriggerSourceOscilloscope.Items.Add("Free Running")
            TriggerLevelOscilloscope.Visible = True
            Label14.Visible = True
        ElseIf Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
            GroupBox1.SetBounds(0, 0, 0, 0)
            TableLayoutPanel1.RowStyles(0).Height = 110
            TriggerSourceOscilloscope.Items.Clear()
            TriggerSourceOscilloscope.Items.Add("Analog Signal")
            TriggerSourceOscilloscope.Items.Add("MCA Trigger")
            TriggerSourceOscilloscope.Items.Add("Free Running")
            TriggerLevelOscilloscope.Visible = False
            Label14.Visible = False
        ElseIf Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            Panel2.Controls.Clear()
            Dim lbl As New Label
            lbl.Top = 10
            lbl.Left = 10
            lbl.Text = "Analog OFFSET"
            AnalogOffsetControl = New NumericUpDown
            AnalogOffsetControl.Top = 10
            AnalogOffsetControl.Left = lbl.Width + lbl.Left + 15
            AnalogOffsetControl.Increment = 1
            AnalogOffsetControl.DecimalPlaces = 0
            AnalogOffsetControl.Maximum = 4095
            AnalogOffsetControl.Minimum = 0
            AnalogOffsetControl.Value = 2048
            AddHandler AnalogOffsetControl.Validated, AddressOf UpdateButtonStatusApply
            Apply.Enabled = True
            Apply.BackColor = Color.DodgerBlue

            Panel2.Controls.Add(lbl)
            Panel2.Controls.Add(AnalogOffsetControl)

            GroupBox1.Height = 65
            TableLayoutPanel1.RowStyles(0).Height = 10 + GroupBox1.Height + GroupBox2.Height
            TriggerSource.Items.Add("Internal")
            TriggerSource.Items.Add("External")

            TriggerSourceOscilloscope.Items.Clear()
            TriggerSourceOscilloscope.Items.Add("Internal")
            TriggerSourceOscilloscope.Items.Add("External")
            TriggerSourceOscilloscope.Items.Add("Free Running")

            For Each i In MainForm.acquisition.CHList
                TriggerSourceOscilloscope.Items.Add(i.name)
            Next
        End If

        TriggerEdge.Items.Clear()
        TriggerEdge.Items.Add("Rising")
        TriggerEdge.Items.Add("Falling")

        inhibit = True
        inhibit2 = True

        DataGridView1.Columns.Clear()
        DataGridView1.Columns.Add("Channel", "Channel")
        Dim pol_column As New DataGridViewComboBoxColumn()
        pol_column.HeaderText = "Polarity"
        pol_column.Name = "Polarity"
        pol_column.MaxDropDownItems = 2
        pol_column.Items.Add("Positive")
        pol_column.Items.Add("Negative")
        DataGridView1.Columns.Add(pol_column)

        If Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
            DataGridView1.Columns.Add("Oscilloscope Trigger Level", "Oscilloscope Trigger Level (lsb)")
        End If

        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
            DataGridView1.Columns.Add("Trigger Level", "Trigger Level (lsb)")
            DataGridView1.Columns.Add("Trigger Hold-Off", "Trigger Hold-Off (ns)")
        ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.SCIDK Or Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
            DataGridView1.Columns.Add("Offset", "Offset (lsb)")
            DataGridView1.Columns.Add("Trigger Level", "Trigger Level (lsb)")
            DataGridView1.Columns.Add("Trigger Peaking", "Trigger Peaking Time (ns)")
            DataGridView1.Columns.Add("Trigger Flat", "Trigger Flat Top (ns)")
        End If

        If MainForm.isChargeIntegration Then
            DataGridView1.Columns.Add("Integration Time", "Integration Time (ns)")
            DataGridView1.Columns.Add("Pre-gate", "Pre-gate (ns)")
        Else
            DataGridView1.Columns.Add("Decay Constant", "Signal Decay Constant (ns)")
            DataGridView1.Columns.Add("Peaking Time", "Peaking Time (ns)")
            DataGridView1.Columns.Add("Flat Top", "Flat Top (ns)")
            DataGridView1.Columns.Add("Energy Sample", "Energy Sample (ns)")
        End If

        DataGridView1.Columns.Add("Gain", "Gain")

        If Connection.ComClass._boardModel = communication.tModel.DT5550 Or Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            Dim pileup As New DataGridViewCheckBoxColumn
            pileup.HeaderText = "Pileup Rejection"
            pileup.Name = "Pileup Rejection"
            DataGridView1.Columns.Add(pileup)
            DataGridView1.Columns.Add("Pileup Rejection Time", "Pileup Rejection Time (ns)")
        End If

        DataGridView1.Columns.Add("Baseline Inhibit Time", "Baseline Inhibit Time (ns)")

        Dim baseline As New DataGridViewComboBoxColumn()
        baseline.HeaderText = "Baseline Lenght (samples)"
        baseline.Name = "Baseline Lenght"
        baseline.MaxDropDownItems = 4
        baseline.Items.Add("16")
        baseline.Items.Add("64")
        baseline.Items.Add("256")
        baseline.Items.Add("1024")
        DataGridView1.Columns.Add(baseline)

        If Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
            If (first_load) Then
                Dim title As String = "AFE Settings"
                Dim myTabPage As TabPage = New TabPage(title)

                Dim myTablelayout As New TableLayoutPanel
                myTablelayout.ColumnCount = 1
                myTablelayout.RowCount = 2
                myTablelayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 100))
                myTablelayout.BackColor = Color.White
                myTablelayout.Dock = DockStyle.Fill

                Dim p1 As New Panel
                Dim p2 As New Panel
                p1.BackColor = Color.White
                p2.BackColor = Color.White
                p1.Dock = DockStyle.Fill
                p2.Dock = DockStyle.Fill

                Dim l As New Label
                l.Text = "Shaper"
                l.Location = New Point(30, 30)
                l.Width = 50

                shaper.Items.Clear()
                shaper.Items.Add("DC")
                shaper.Items.Add("AC 1us")
                shaper.Items.Add("AC 10us")
                shaper.Items.Add("AC 30us")
                shaper.SelectedIndex = 0
                shaper.Width = 100
                shaper.DropDownStyle = ComboBoxStyle.DropDownList
                'shaper.FlatStyle = FlatStyle.Flat
                shaper.Location = New Point(120, 30)
                AddHandler shaper.SelectedIndexChanged, AddressOf Shaper_SelectedIndexChanged


                p1.Controls.Add(l)
                p1.Controls.Add(shaper)

                AddHandler DataGridView2.Resize, AddressOf DataGridView2_Resize
                AddHandler DataGridView2.CellValueChanged, AddressOf DataGridView2_CellValueChanged

                DataGridView2.BackgroundColor = Color.White
                DataGridView2.AllowUserToAddRows = False
                DataGridView2.AllowUserToDeleteRows = False
                DataGridView2.AllowUserToOrderColumns = False
                DataGridView2.AllowUserToResizeColumns = False
                DataGridView2.AllowUserToResizeRows = False
                DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                DataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
                DataGridView2.Dock = DockStyle.Fill
                DataGridView2.EditMode = DataGridViewEditMode.EditOnEnter
                DataGridView2.RowHeadersVisible = False

                DataGridView2.Columns.Clear()
                DataGridView2.Columns.Add("Channel", "Channel")

                Dim imp As New DataGridViewComboBoxColumn()
                imp.HeaderText = "Termination"
                imp.Name = "Termination"
                imp.MaxDropDownItems = 2
                Dim i As Integer
                imp.Items.Add("50 Ohm")
                imp.Items.Add("1 kOhm")
                DataGridView2.Columns.Add(imp)
                'Dim imp As New DataGridViewCheckBoxColumn
                'imp.HeaderText = "50 Ohm Termination"
                'imp.Name = "Termination"
                'DataGridView2.Columns.Add(imp)
                Dim div As New DataGridViewCheckBoxColumn
                div.HeaderText = "Division by 5"
                div.Name = "Division"
                DataGridView2.Columns.Add(div)
                DataGridView2.Columns.Add("OffsetEven", "Offset Even (mV)")
                DataGridView2.Columns.Add("OffsetOdd", "Offset Odd (mV)")
                Dim gain As New DataGridViewComboBoxColumn()
                gain.HeaderText = "Gain"
                gain.Name = "Gain"
                gain.MaxDropDownItems = 80
                For i = 0 To gain_list.Length - 1
                    gain.Items.Add(gain_list(i).ToString())
                Next
                gain.MaxDropDownItems = 10
                DataGridView2.Columns.Add(gain)

                p2.Controls.Add(DataGridView2)
                myTablelayout.Controls.Add(p1, 0, 0)

                myTablelayout.RowStyles.Add(New RowStyle(SizeType.Percent, 90))
                myTablelayout.Controls.Add(p2, 0, 1)

                myTabPage.Controls.Add(myTablelayout)
                TabControl1.TabPages.Add(myTabPage)
                Grid2_ReLoad()
                first_load = False
            End If
        End If

        Settings_reload()
        Grid_ReLoad()

    End Sub

    Public Sub Settings_reload()
        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then

            If MainForm.acquisition.General_settings.AFEImpedance Then
                Impedance.SelectedIndex = 0
            Else
                Impedance.SelectedIndex = 1
            End If

            Offset.Value = MainForm.acquisition.General_settings.AFEOffset
            SignalOffset.Value = MainForm.acquisition.General_settings.SignalOffset

            If MainForm.acquisition.General_settings.TriggerMode = trigger_mode.THRESHOLD Then
                TriggerMode.SelectedIndex = 1
            Else
                TriggerMode.SelectedIndex = 0
            End If

            If MainForm.acquisition.General_settings.TriggerSource = trigger_source.INTERNAL Then
                TriggerSource.SelectedIndex = 0
                TriggerDelay.Enabled = False
                TriggerDelayLabel.Enabled = False
            Else
                TriggerSource.SelectedIndex = 1
                TriggerDelay.Enabled = True
                TriggerDelayLabel.Enabled = True
            End If

            If MainForm.acquisition.General_settings.Sampling = sampling_method.COMMON Then
                sampling.SelectedIndex = 0
            Else
                sampling.SelectedIndex = 1
            End If

            TriggerDelay.Value = MainForm.acquisition.General_settings.TriggerDelay
        End If

        If MainForm.acquisition.General_settings.TriggerSourceOscilloscope = trigger_source.FREE Then
            TriggerSourceOscilloscope.SelectedIndex = 2
        ElseIf MainForm.acquisition.General_settings.TriggerSourceOscilloscope = trigger_source.EXTERNAL Then
            TriggerSourceOscilloscope.SelectedIndex = 1
        ElseIf MainForm.acquisition.General_settings.TriggerSourceOscilloscope = trigger_source.LEVEL Then
            If Connection.ComClass._boardModel = communication.tModel.DT5550 Or
                Connection.ComClass._boardModel = communication.tModel.SCIDK Then
                TriggerSourceOscilloscope.SelectedIndex = MainForm.acquisition.General_settings.TriggerChannelOscilloscope + 3
            ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
                TriggerSourceOscilloscope.SelectedIndex = 0
            End If
        ElseIf MainForm.acquisition.General_settings.TriggerSourceOscilloscope = trigger_source.INTERNAL Then
                TriggerSourceOscilloscope.SelectedIndex = 0
        End If

        If MainForm.acquisition.General_settings.TriggerOscilloscopeEdges = DT5550ControlCenter.AcquisitionClass.edge.RISING Then
            TriggerEdge.SelectedIndex = 0
        Else
            TriggerEdge.SelectedIndex = 1
        End If

        TriggerLevelOscilloscope.Text = MainForm.acquisition.General_settings.TriggerOscilloscopeLevel
        PreTrigger.Text = MainForm.acquisition.General_settings.OscilloscopePreTrigger
        Horizontal.Text = Math.Round(MainForm.acquisition.General_settings.OscilloscopeDecimator * sampling_factor, 1)

        If Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
            shaper.SelectedItem = MainForm.acquisition.General_settings.AFEShaper
        End If

    End Sub

    Public Sub Grid_ReLoad()
        '  SetDoubleBuffered(DataGridView1)
        n = MainForm.acquisition.CHList.Count
        DataGridView1.Rows.Clear()
        For i = 0 To n - 1
            DataGridView1.Rows.Add()
            DataGridView1.Rows(i).Cells("Channel").Value = MainForm.acquisition.CHList(i).name '"CHANNEL " & (i + 1).ToString
            If MainForm.acquisition.CHList(i).polarity = signal_polarity.POSITIVE Then
                DataGridView1.Rows(i).Cells("Polarity").Value = "Positive"
            Else
                DataGridView1.Rows(i).Cells("Polarity").Value = "Negative"
            End If

            If Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
                DataGridView1.Rows(i).Cells("Oscilloscope Trigger Level").Value = MainForm.acquisition.CHList(i).TriggerOscilloscopeLevel
            End If

            DataGridView1.Rows(i).Cells("Trigger Level").Value = MainForm.acquisition.CHList(i).trigger_level '"CHANNEL " & (i + 1).ToString

            If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
                DataGridView1.Rows(i).Cells("Trigger Hold-Off").Value = MainForm.acquisition.CHList(i).trigger_inhibit '"CHANNEL " & (i + 1).ToString
                DataGridView1.Rows(i).Cells("Pileup Rejection").Value = MainForm.acquisition.CHList(i).pileup_enable
                If DataGridView1.Rows(i).Cells("Pileup Rejection").Value Then
                    DataGridView1.Rows(i).Cells("Pileup Rejection Time").ReadOnly = False
                    DataGridView1.Rows(i).Cells("Pileup Rejection Time").Value = MainForm.acquisition.CHList(i).pileup_time
                Else
                    DataGridView1.Rows(i).Cells("Pileup Rejection Time").ReadOnly = True
                End If

            End If
            If Connection.ComClass._boardModel = communication.tModel.SCIDK Then
                DataGridView1.Rows(i).Cells("Pileup Rejection").Value = MainForm.acquisition.CHList(i).pileup_enable
                DataGridView1.Rows(i).Cells("Pileup Rejection Time").Value = MainForm.acquisition.CHList(i).pileup_time
            End If
            If Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.SCIDK Or Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
                DataGridView1.Rows(i).Cells("Trigger Peaking").Value = MainForm.acquisition.CHList(i).trigger_peaking
                DataGridView1.Rows(i).Cells("Trigger Flat").Value = MainForm.acquisition.CHList(i).trigger_flat
                DataGridView1.Rows(i).Cells("Offset").Value = MainForm.acquisition.CHList(i).offset
            End If

            DataGridView1.Rows(i).Cells("Baseline Inhibit Time").Value = MainForm.acquisition.CHList(i).baseline_inhibit
            DataGridView1.Rows(i).Cells("Baseline Lenght").Value = MainForm.acquisition.CHList(i).baseline_sample.ToString
            If MainForm.isChargeIntegration Then
                DataGridView1.Rows(i).Cells("Integration Time").Value = MainForm.acquisition.CHList(i).integration_time
                DataGridView1.Rows(i).Cells("Pre-gate").Value = MainForm.acquisition.CHList(i).pre_gate
            Else
                DataGridView1.Rows(i).Cells("Decay Constant").Value = MainForm.acquisition.CHList(i).decay_constant
                DataGridView1.Rows(i).Cells("Peaking Time").Value = MainForm.acquisition.CHList(i).peaking_time
                DataGridView1.Rows(i).Cells("Flat Top").Value = MainForm.acquisition.CHList(i).flat_top
                DataGridView1.Rows(i).Cells("Energy Sample").Value = MainForm.acquisition.CHList(i).energy_sample
            End If
            DataGridView1.Rows(i).Cells("Gain").Value = MainForm.acquisition.CHList(i).gain
        Next
        inhibit = False

    End Sub

    Public Sub Grid2_ReLoad()
        n = MainForm.acquisition.CHList.Count
        DataGridView2.Rows.Clear()
        Dim k = 0
        For i = 0 To n - 1 Step 2
            DataGridView2.Rows.Add()
            DataGridView2.Rows(k).Cells("Channel").Value = "CHANNELS " + i.ToString() + "-" + (i + 1).ToString() '"CHANNEL " & (i + 1).ToString
            DataGridView2.Rows(k).Cells("Termination").Value = IIf(MainForm.acquisition.CHList(i).Afe_set.Termination = True, "50 Ohm", "1 kOhm")
            DataGridView2.Rows(k).Cells("Division").Value = MainForm.acquisition.CHList(i).Afe_set.Division
            DataGridView2.Rows(k).Cells("OffsetEven").Value = MainForm.acquisition.CHList(i).Afe_set.Offset
            DataGridView2.Rows(k).Cells("OffsetOdd").Value = MainForm.acquisition.CHList(i + 1).Afe_set.Offset

            'Dim index = 0
            'For k = 0 To gain_list.Length - 1
            '    If gain_list(k) = MainForm.acquisition.CHList(i).Afe_set.Gain Then
            '        index = k
            '        Exit For
            '    End If
            'Next
            'DataGridView2.Rows(i).Cells("Gain").Value = index
            DataGridView2.Rows(k).Cells("Gain").Value = MainForm.acquisition.CHList(i).Afe_set.Gain.ToString()
            k += 1
        Next

        inhibit2 = False

    End Sub

    Private Sub DataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        Apply.Enabled = True
        Apply.BackColor = Color.DodgerBlue

        If inhibit = False Then
            If DataGridView1.SelectedCells.Count > 1 Then
                inhibit = True
                For Each r As DataGridViewCell In DataGridView1.SelectedCells
                    If r.RowIndex <> e.RowIndex Then
                        DataGridView1.Rows(r.RowIndex).Cells(r.ColumnIndex).Value = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                    End If
                Next
                inhibit = False
            End If
            If Connection.ComClass._boardModel = communication.tModel.DT5550 Then

                If DataGridView1.Rows(e.RowIndex).Cells("Pileup Rejection").Value = True Then
                    DataGridView1.Rows(e.RowIndex).Cells("Pileup Rejection Time").ReadOnly = False
                Else
                    DataGridView1.Rows(e.RowIndex).Cells("Pileup Rejection Time").ReadOnly = True
                End If
            End If
        End If
        If Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.DT5560SE Then

            If (CInt(DataGridView1.Rows(e.RowIndex).Cells("Peaking Time").Value) + CInt(DataGridView1.Rows(e.RowIndex).Cells("Flat Top").Value)) / sampling_factor > 512 Then
                DataGridView1.Rows(e.RowIndex).Cells("Peaking Time").Style.BackColor = Color.Red
                DataGridView1.Rows(e.RowIndex).Cells("Flat Top").Style.BackColor = Color.Red
                Apply.Enabled = False
                Apply.BackColor = Color.LightGray
                MainForm.plog.TextBox1.AppendText("Peaking Time plus Flat Top Time could not exceeds 4096 ns!" & vbCrLf)
            Else
                DataGridView1.Rows(e.RowIndex).Cells("Peaking Time").Style.BackColor = Color.White
                DataGridView1.Rows(e.RowIndex).Cells("Flat Top").Style.BackColor = Color.White
            End If

            If (CInt(DataGridView1.Rows(e.RowIndex).Cells("Trigger Peaking").Value) + CInt(DataGridView1.Rows(e.RowIndex).Cells("Trigger Flat").Value)) / sampling_factor > 128 Then
                DataGridView1.Rows(e.RowIndex).Cells("Trigger Peaking").Style.BackColor = Color.Red
                DataGridView1.Rows(e.RowIndex).Cells("Trigger Flat").Style.BackColor = Color.Red
                Apply.Enabled = False
                Apply.BackColor = Color.LightGray
                MainForm.plog.TextBox1.AppendText("Trigger Peaking Time plus Trigger Flat Top Time could not exceeds 1024 ns!" & vbCrLf)
            Else
                DataGridView1.Rows(e.RowIndex).Cells("Trigger Peaking").Style.BackColor = Color.White
                DataGridView1.Rows(e.RowIndex).Cells("Trigger Flat").Style.BackColor = Color.White
            End If

            If (CInt(DataGridView1.Rows(e.RowIndex).Cells("Oscilloscope Trigger Level").Value) > 16384) Then
                DataGridView1.Rows(e.RowIndex).Cells("Oscilloscope Trigger Level").Style.BackColor = Color.Red
                Apply.Enabled = False
                Apply.BackColor = Color.LightGray
                MainForm.plog.TextBox1.AppendText("Oscilloscope Trigger Level could not exceeds 16384 lsb!" & vbCrLf)
            Else
                DataGridView1.Rows(e.RowIndex).Cells("Oscilloscope Trigger Level").Style.BackColor = Color.White
            End If

            If (CInt(DataGridView1.Rows(e.RowIndex).Cells("Offset").Value) > 16384) Then
                DataGridView1.Rows(e.RowIndex).Cells("Offset").Style.BackColor = Color.Red
                Apply.Enabled = False
                Apply.BackColor = Color.LightGray
                MainForm.plog.TextBox1.AppendText("Offset could not exceeds 16384 lsb!" & vbCrLf)
            Else
                DataGridView1.Rows(e.RowIndex).Cells("Offset").Style.BackColor = Color.White
            End If

            If (CInt(DataGridView1.Rows(e.RowIndex).Cells("Trigger Level").Value) > 16384) Then
                DataGridView1.Rows(e.RowIndex).Cells("Trigger Level").Style.BackColor = Color.Red
                Apply.Enabled = False
                Apply.BackColor = Color.LightGray
                MainForm.plog.TextBox1.AppendText("Trigger Level could not exceeds 16384 lsb!" & vbCrLf)
            Else
                DataGridView1.Rows(e.RowIndex).Cells("Trigger Level").Style.BackColor = Color.White
            End If

            If (CInt(DataGridView1.Rows(e.RowIndex).Cells("Energy Sample").Value) > 4096) Then
                DataGridView1.Rows(e.RowIndex).Cells("Energy Sample").Style.BackColor = Color.Red
                Apply.Enabled = False
                Apply.BackColor = Color.LightGray
                MainForm.plog.TextBox1.AppendText("Energy Sample could not exceeds 4096 ns!" & vbCrLf)
            Else
                DataGridView1.Rows(e.RowIndex).Cells("Energy Sample").Style.BackColor = Color.White
            End If

            If (CInt(DataGridView1.Rows(e.RowIndex).Cells("Baseline Inhibit Time").Value) > 520000) Then
                DataGridView1.Rows(e.RowIndex).Cells("Baseline Inhibit Time").Style.BackColor = Color.Red
                Apply.Enabled = False
                Apply.BackColor = Color.LightGray
                MainForm.plog.TextBox1.AppendText("Baseline Inhibit Time could not exceeds 520000 ns!" & vbCrLf)
            Else
                DataGridView1.Rows(e.RowIndex).Cells("Baseline Inhibit Time").Style.BackColor = Color.White
            End If

        End If
    End Sub

    Private Sub DataGridView2_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        Apply.Enabled = True
        Apply.BackColor = Color.DodgerBlue

        If inhibit2 = False Then
            If DataGridView2.SelectedCells.Count > 1 Then
                inhibit2 = True
                For Each r As DataGridViewCell In DataGridView2.SelectedCells
                    If r.RowIndex <> e.RowIndex Then
                        DataGridView2.Rows(r.RowIndex).Cells(r.ColumnIndex).Value = DataGridView2.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                    End If
                Next
                inhibit2 = False
            End If

        End If

        If ((DataGridView2.Rows(e.RowIndex).Cells("OffsetEven").Value) > 2000) Or ((DataGridView2.Rows(e.RowIndex).Cells("OffsetEven").Value) < -2000) Then
            DataGridView2.Rows(e.RowIndex).Cells("OffsetEven").Style.BackColor = Color.Red
            Apply.Enabled = False
            Apply.BackColor = Color.LightGray
            MainForm.plog.TextBox1.AppendText("Offset out of range!" & vbCrLf)
        Else
            DataGridView2.Rows(e.RowIndex).Cells("OffsetEven").Style.BackColor = Color.White
        End If

        If ((DataGridView2.Rows(e.RowIndex).Cells("OffsetOdd").Value) > 2000) Or ((DataGridView2.Rows(e.RowIndex).Cells("OffsetOdd").Value) < -2000) Then
            DataGridView2.Rows(e.RowIndex).Cells("OffsetOdd").Style.BackColor = Color.Red
            Apply.Enabled = False
            Apply.BackColor = Color.LightGray
            MainForm.plog.TextBox1.AppendText("Offset out of range!" & vbCrLf)
        Else
            DataGridView2.Rows(e.RowIndex).Cells("OffsetOdd").Style.BackColor = Color.White
        End If
    End Sub


    'Private Sub Mode_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    If Mode.SelectedItem = "Derivate" Then
    '        Label7.Enabled = True
    '        derivative.Enabled = True
    '    Else
    '        Label7.Enabled = False
    '        derivative.Enabled = False
    '    End If
    'End Sub

    Private Sub Source_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TriggerSource.SelectedIndexChanged

        If TriggerSource.SelectedItem = "External" Then
            TriggerDelayLabel.Enabled = True
            TriggerDelay.Enabled = True
        Else
            TriggerDelayLabel.Enabled = False
            TriggerDelay.Enabled = False
        End If

    End Sub

    Private Sub Sampling_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sampling.SelectedIndexChanged

        If sampling.SelectedItem = "Independent" Then
            MainForm.pImm1.Hide()
            MainForm.pImm2.Hide()
        Else
            MainForm.pImm1.Show()
            MainForm.pImm2.Show()
        End If

    End Sub

    Private Sub Apply_Click(sender As Object, e As EventArgs) Handles Apply.Click

        UpdateSettings()
        UploadSettings()

    End Sub

    Public Sub UpdateSettings()

        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then

            If Impedance.SelectedItem = "50 Ohm" Then
                MainForm.acquisition.General_settings.AFEImpedance = True
            Else
                MainForm.acquisition.General_settings.AFEImpedance = False
            End If

            MainForm.acquisition.General_settings.SignalOffset = SignalOffset.Value
            MainForm.acquisition.General_settings.AFEOffset = Offset.Value

            If TriggerMode.SelectedItem = "Threshold" Then
                MainForm.acquisition.General_settings.TriggerMode = AcquisitionClass.trigger_mode.THRESHOLD
            Else
                MainForm.acquisition.General_settings.TriggerMode = AcquisitionClass.trigger_mode.DERIVATIVE
            End If

            If TriggerSource.SelectedItem = "Internal" Then
                MainForm.acquisition.General_settings.TriggerSource = AcquisitionClass.trigger_source.INTERNAL
            Else
                MainForm.acquisition.General_settings.TriggerSource = AcquisitionClass.trigger_source.EXTERNAL
                MainForm.acquisition.General_settings.TriggerDelay = TriggerDelay.Value
            End If

            If sampling.SelectedItem = "Indipendent" Then
                MainForm.acquisition.General_settings.Sampling = AcquisitionClass.sampling_method.INDIPENDENT
            Else
                MainForm.acquisition.General_settings.Sampling = AcquisitionClass.sampling_method.COMMON
            End If

        End If

        If TriggerEdge.SelectedItem = "Rising" Then
            MainForm.acquisition.General_settings.TriggerOscilloscopeEdges = AcquisitionClass.edge.RISING
        Else
            MainForm.acquisition.General_settings.TriggerOscilloscopeEdges = AcquisitionClass.edge.FALLING
        End If
        If TriggerSourceOscilloscope.SelectedItem = "Internal" Then
            MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.INTERNAL
        ElseIf TriggerSourceOscilloscope.SelectedItem = "External" Or TriggerSourceOscilloscope.SelectedItem = "MCA Trigger" Then
            MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.EXTERNAL
        ElseIf TriggerSourceOscilloscope.SelectedItem = "Free Running" Then
            MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.FREE
        ElseIf TriggerSourceOscilloscope.SelectedItem = "Analog Signal" Then
            MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.LEVEL
            MainForm.acquisition.General_settings.TriggerChannelOscilloscope = 0
        Else
            MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.LEVEL
            MainForm.acquisition.General_settings.TriggerChannelOscilloscope = MainForm.acquisition.CHList(TriggerSourceOscilloscope.SelectedIndex - 3).id - 1
        End If
        'If selectmux.SelectedItem = "Analog" Then
        '    MainForm.acquisition.General_settings.mux = AcquisitionClass.muxmode.ANALOG
        'ElseIf selectmux.SelectedItem = "Baseline" Then
        '    MainForm.acquisition.General_settings.mux = AcquisitionClass.muxmode.BASELINE
        'ElseIf selectmux.SelectedItem = "Trapezoidal" Then
        '    MainForm.acquisition.General_settings.mux = AcquisitionClass.muxmode.TRAPEZOIDAL
        'ElseIf selectmux.SelectedItem = "Energy" Then
        '    MainForm.acquisition.General_settings.mux = AcquisitionClass.muxmode.ENERGY

        'End If
        ' MainForm.acquisition.General_settings.trigholdoff = holdoff.Value

        MainForm.acquisition.General_settings.OscilloscopePreTrigger = PreTrigger.Text
        MainForm.acquisition.General_settings.OscilloscopeDecimator = Math.Round(Horizontal.Text / sampling_factor)
        MainForm.acquisition.General_settings.TriggerOscilloscopeLevel = TriggerLevelOscilloscope.Text

        If Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            MainForm.acquisition.General_settings.SignalOffset = AnalogOffsetControl.Value
        End If
        For i = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells("Polarity").Value = "Positive" Then
                MainForm.acquisition.CHList(i).polarity = signal_polarity.POSITIVE
            Else
                MainForm.acquisition.CHList(i).polarity = signal_polarity.NEGATIVE
            End If

            If Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
                MainForm.acquisition.CHList(i).TriggerOscilloscopeLevel = DataGridView1.Rows(i).Cells("Oscilloscope Trigger Level").Value
            End If

            MainForm.acquisition.CHList(i).trigger_level = DataGridView1.Rows(i).Cells("Trigger Level").Value

            If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
                MainForm.acquisition.CHList(i).trigger_inhibit = DataGridView1.Rows(i).Cells("Trigger Hold-Off").Value
                If DataGridView1.Rows(i).Cells("Pileup Rejection").Value Then
                    MainForm.acquisition.CHList(i).pileup_enable = True
                    MainForm.acquisition.CHList(i).pileup_time = DataGridView1.Rows(i).Cells("Pileup Rejection Time").Value
                Else
                    MainForm.acquisition.CHList(i).pileup_enable = False
                End If
            End If

            If Connection.ComClass._boardModel = communication.tModel.SCIDK Then
                If DataGridView1.Rows(i).Cells("Pileup Rejection").Value Then
                    MainForm.acquisition.CHList(i).pileup_enable = True
                    MainForm.acquisition.CHList(i).pileup_time = DataGridView1.Rows(i).Cells("Pileup Rejection Time").Value
                Else
                    MainForm.acquisition.CHList(i).pileup_enable = False
                End If
            End If

            If MainForm.isChargeIntegration Then
                '    MainForm.oscilloscope.CHList(i).energyfilter = OscilloscopeClass.Channel.energyfiltermode.INTEGRATION
                MainForm.acquisition.CHList(i).integration_time = DataGridView1.Rows(i).Cells("Integration Time").Value
                MainForm.acquisition.CHList(i).pre_gate = DataGridView1.Rows(i).Cells("Pre-gate").Value
            Else
                MainForm.acquisition.CHList(i).decay_constant = DataGridView1.Rows(i).Cells("Decay Constant").Value
                MainForm.acquisition.CHList(i).peaking_time = DataGridView1.Rows(i).Cells("Peaking Time").Value
                MainForm.acquisition.CHList(i).flat_top = DataGridView1.Rows(i).Cells("Flat Top").Value
                MainForm.acquisition.CHList(i).energy_sample = DataGridView1.Rows(i).Cells("Energy Sample").Value
            End If
            MainForm.acquisition.CHList(i).gain = DataGridView1.Rows(i).Cells("Gain").Value

            MainForm.acquisition.CHList(i).baseline_inhibit = DataGridView1.Rows(i).Cells("Baseline Inhibit Time").Value
            MainForm.acquisition.CHList(i).baseline_sample = CType(DataGridView1.Rows(i).Cells("Baseline Lenght").Value, Integer)

            If Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.SCIDK Or Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
                MainForm.acquisition.CHList(i).offset = DataGridView1.Rows(i).Cells("Offset").Value
                MainForm.acquisition.CHList(i).trigger_peaking = DataGridView1.Rows(i).Cells("Trigger Peaking").Value
                MainForm.acquisition.CHList(i).trigger_flat = DataGridView1.Rows(i).Cells("Trigger Flat").Value
            End If
        Next

        If Connection.ComClass._boardModel = communication.tModel.DT5560SE Then

            MainForm.acquisition.General_settings.AFEShaper = shaper.SelectedIndex
            Dim k = 0
            For i = 0 To DataGridView2.Rows.Count - 1
                If DataGridView2.Rows(i).Cells("Termination").Value = "50 Ohm" Then
                    MainForm.acquisition.CHList(k).Afe_set.Termination = True
                    MainForm.acquisition.CHList(k + 1).Afe_set.Termination = True
                Else
                    MainForm.acquisition.CHList(k).Afe_set.Termination = False
                    MainForm.acquisition.CHList(k + 1).Afe_set.Termination = False
                End If

                If DataGridView2.Rows(i).Cells("Division").Value Then
                    MainForm.acquisition.CHList(k).Afe_set.Division = True
                    MainForm.acquisition.CHList(k + 1).Afe_set.Division = True
                Else
                    MainForm.acquisition.CHList(k).Afe_set.Division = False
                    MainForm.acquisition.CHList(k + 1).Afe_set.Division = False
                End If

                MainForm.acquisition.CHList(k).Afe_set.Offset = DataGridView2.Rows(i).Cells("OffsetEven").Value
                MainForm.acquisition.CHList(k + 1).Afe_set.Offset = DataGridView2.Rows(i).Cells("OffsetOdd").Value

                MainForm.acquisition.CHList(k).Afe_set.Gain = CType(DataGridView2.Rows(i).Cells("Gain").Value, Double)
                MainForm.acquisition.CHList(k + 1).Afe_set.Gain = CType(DataGridView2.Rows(i).Cells("Gain").Value, Double)
                k += 2
            Next
        End If
    End Sub

    Private Function SingleToHex(ByVal sing As Single)

        Dim arr = BitConverter.GetBytes(sing)
        Array.Reverse(arr)
        Return Convert.ToUInt32(BitConverter.ToString(arr).Replace("-", ""), 16)

    End Function

    Private Sub UploadSettings()

        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then

            If Connection.ComClass.AFE_SetImpedence(MainForm.acquisition.General_settings.AFEImpedance) = 0 Then
            Else
                MainForm.plog.TextBox1.AppendText("Signal Impedence Set Register Error!" & vbCrLf)
            End If

            Dim offsetLSB = (MainForm.acquisition.General_settings.AFEOffset + 2) / 4 * 4095 '* (4095 - 1650) + 1650

            If Connection.ComClass.AFE_SetOffset(False, offsetLSB) = 0 Then
            Else
                MainForm.plog.TextBox1.AppendText("Signal Offset Set Register Error!" & vbCrLf)
            End If
            If Connection.ComClass.AFE_SetOffset(True, offsetLSB) = 0 Then
            Else
                MainForm.plog.TextBox1.AppendText("Signal Offset Set Register Error!" & vbCrLf)
            End If

            Dim TrModeAddress As New UInt32
            Dim TrSourceAddress As New UInt32
            Dim TrDelayAddress As New UInt32
            Dim TrOffsetAddress As New UInt32
            Dim TrOscilloscopeAddress As New UInt32

            For Each r In MainForm.CurrentRegisterList
                If r.Name = "TRMODE" Then
                    TrModeAddress = r.Address
                End If
                If r.Name = "TRSEL" Then
                    TrSourceAddress = r.Address
                End If
                If r.Name = "TDELAY" Then
                    TrDelayAddress = r.Address
                End If
                If r.Name = "OFFSET" Then
                    TrOffsetAddress = r.Address
                End If
                If r.Name = "OSC_TRIGGER" Then
                    TrOscilloscopeAddress = r.Address
                End If
            Next

            If Connection.ComClass.SetRegister(TrModeAddress, MainForm.acquisition.General_settings.TriggerMode, 0) = 0 Then
            Else
                MainForm.plog.TextBox1.AppendText("Trigger Mode Set Register Error!" & vbCrLf)
            End If
            If Connection.ComClass.SetRegister(TrSourceAddress, MainForm.acquisition.General_settings.TriggerSource, 0) = 0 Then
            Else
                MainForm.plog.TextBox1.AppendText("Trigger Source Set Register Error!" & vbCrLf)
            End If
            If Connection.ComClass.SetRegister(TrDelayAddress, MainForm.acquisition.General_settings.TriggerDelay, 0) = 0 Then
            Else
                MainForm.plog.TextBox1.AppendText("Trigger Delay Set Register Error!" & vbCrLf)
            End If
            If Connection.ComClass.SetRegister(TrOffsetAddress, MainForm.acquisition.General_settings.SignalOffset, 0) = 0 Then
            Else
                MainForm.plog.TextBox1.AppendText("Trigger Offset Set Register Error!" & vbCrLf)
            End If
            If MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.INTERNAL Or MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.EXTERNAL Then
                If Connection.ComClass.SetRegister(TrOscilloscopeAddress, MainForm.acquisition.General_settings.TriggerSourceOscilloscope, 0) = 0 Then
                Else
                    MainForm.plog.TextBox1.AppendText("Trigger Oscilloscope Set Register Error!" & vbCrLf)
                End If
            End If

            Dim jjj As New ClassCalibration(My.Settings.AFECalibration)
            Dim coor = jjj.GetCorrectionFactors(offsetLSB)

            Dim ANOFS As New UInt32
            ANOFS = 0
            For i = 0 To MainForm.acquisition.CHList.Count - 1
                For Each r In MainForm.CurrentRegisterList
                    If r.Name = "ANOFS_" & i Then
                        ANOFS = r.Address
                    End If

                Next
                If ANOFS > 0 Then
                    Connection.ComClass.SetRegister(ANOFS, IIf(coor(31 - i) >= 0, coor(31 - i), &HFFFFFFFF& + coor(31 - i)), 0)

                End If
            Next
        End If

        Dim n_ok = 0
        For i = 0 To MainForm.acquisition.CHList.Count - 1

            Dim index As Integer
            If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
                index = MainForm.acquisition.CHList.Count - MainForm.acquisition.CHList(i).id
            ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
                index = MainForm.acquisition.CHList(i).ch_id - 1
            ElseIf Connection.ComClass._boardModel = communication.tModel.SCIDK Then
                index = MainForm.acquisition.CHList(i).ch_id - 1
            End If
            Dim ind = MainForm.acquisition.CHList(i).board_number

            Dim PolarityAddress As New UInt32
            Dim TriggerAddress As New UInt32
            Dim BaselineAddress As New UInt32
            Dim BaseHoldAddress As New UInt32
            Dim IntAddress As New UInt32
            Dim PreIntAddress As New UInt32
            Dim FlatAddress As New UInt32
            Dim PeakAddress As New UInt32
            Dim EnergyAddress As New UInt32
            Dim DecayAddress As New UInt32
            Dim MAddress As New UInt32
            Dim KAddress As New UInt32
            Dim SampleAddress As New UInt32
            Dim DeconvAddress As New UInt32
            Dim GainAddress As New UInt32
            Dim TRHoldOffAddress As New UInt32
            Dim PileUpAddress As New UInt32
            Dim OffsetAddress As New UInt32
            Dim TriggerKAddress As New UInt32
            Dim TriggerMAddress As New UInt32
            Dim RunAddress As New UInt32
            Dim AnalogOffset As New UInt32

            For Each r In MainForm.CurrentRegisterList

                If r.Name = "ANALOG_OFFSET" Then
                    AnalogOffset = r.Address
                End If

                If r.Name = "POL_" & index Then
                    PolarityAddress = r.Address
                End If
                If r.Name = "OFS_" & index Then
                    OffsetAddress = r.Address
                End If
                If (r.Name = "TH_" & index) Or (r.Name = "THR_" & index) Then
                    TriggerAddress = r.Address
                End If
                If r.Name = "TRAP_K_" & index Then
                    TriggerKAddress = r.Address
                End If
                If r.Name = "TRAP_M_" & index Then
                    TriggerMAddress = r.Address
                End If
                If (r.Name = "BLLEN_" & index) Or (r.Name = "BL_LEN_" & index) Then
                    BaselineAddress = r.Address
                End If
                If (r.Name = "SIGLEN_" & index) Or (r.Name = "BL_INIB_" & index) Then
                    BaseHoldAddress = r.Address
                End If
                If MainForm.isChargeIntegration Then
                    If r.Name = "INTT_" & index Then
                        IntAddress = r.Address
                    End If
                    If r.Name = "PREINT_" & index Then
                        PreIntAddress = r.Address
                    End If
                Else
                    If (r.Name = "FLAT_" & index) Then
                        FlatAddress = r.Address
                    End If
                    If (r.Name = "PEAK_" & index) Then
                        PeakAddress = r.Address
                    End If
                    If (r.Name = "ENERGY_" & index) Then
                        EnergyAddress = r.Address
                    End If
                    If (r.Name = "DECAY_" & index) Then
                        DecayAddress = r.Address
                    End If
                    If (r.Name = "M_" & index) Then
                        MAddress = r.Address
                    End If
                    If (r.Name = "K_" & index) Then
                        KAddress = r.Address
                    End If
                    If (r.Name = "SAMPLE_" & index) Then
                        SampleAddress = r.Address
                    End If
                    If (r.Name = "DECONV_" & index) Then
                        DeconvAddress = r.Address
                    End If
                End If
                If r.Name = "GAIN_" & index Then
                    GainAddress = r.Address
                End If
                If r.Name = "TRINIB" & index Then
                    TRHoldOffAddress = r.Address
                End If
                If r.Name = "TRINIB_" & index Then
                    TRHoldOffAddress = r.Address
                End If
                If r.Name = "PUP_" & index Then
                    PileUpAddress = r.Address
                End If
                If r.Name = "RUN_" & index Then
                    RunAddress = r.Address
                End If

                If r.Name = "RUNCFG" Then
                    RunAddress = r.Address
                End If

            Next

            If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
                If Connection.ComClass.SetRegister(PolarityAddress, MainForm.acquisition.CHList(i).polarity, ind) = 0 Then
                    If Connection.ComClass.SetRegister(TriggerAddress, MainForm.acquisition.CHList(i).trigger_level, ind) = 0 Then
                        If Connection.ComClass.SetRegister(BaselineAddress, Math.Log(MainForm.acquisition.CHList(i).baseline_sample) / Math.Log(2), ind) = 0 Then
                            If Connection.ComClass.SetRegister(BaseHoldAddress, MainForm.acquisition.CHList(i).baseline_inhibit / sampling_factor, ind) = 0 Then
                                If Connection.ComClass.SetRegister(GainAddress, MainForm.acquisition.CHList(i).gain / MainForm.acquisition.CHList(i).integration_time * 65535, ind) = 0 Then
                                    If Connection.ComClass.SetRegister(TRHoldOffAddress, MainForm.acquisition.CHList(i).trigger_inhibit / sampling_factor, ind) = 0 Then
                                        Dim pup_value = IIf(MainForm.acquisition.CHList(i).pileup_enable = True, MainForm.acquisition.CHList(i).pileup_time / sampling_factor, ind)
                                        If Connection.ComClass.SetRegister(PileUpAddress, pup_value, ind) = 0 Then

                                            If MainForm.isChargeIntegration Then
                                                If Connection.ComClass.SetRegister(IntAddress, MainForm.acquisition.CHList(i).integration_time / sampling_factor, 0) = 0 Then
                                                    If Connection.ComClass.SetRegister(PreIntAddress, MainForm.acquisition.CHList(i).pre_gate / sampling_factor, 0) = 0 Then
                                                    Else
                                                        MainForm.plog.TextBox1.AppendText("PreGate Set Register Error!" & vbCrLf)
                                                    End If
                                                Else
                                                    MainForm.plog.TextBox1.AppendText("Integration Time Set Register Error!" & vbCrLf)
                                                End If
                                            Else
                                                If Connection.ComClass.SetRegister(FlatAddress, MainForm.acquisition.CHList(i).flat_top / sampling_factor, ind) = 0 Then
                                                    If Connection.ComClass.SetRegister(PeakAddress, MainForm.acquisition.CHList(i).peaking_time / sampling_factor, ind) = 0 Then
                                                        If Connection.ComClass.SetRegister(EnergyAddress, MainForm.acquisition.CHList(i).energy_sample / sampling_factor, ind) = 0 Then
                                                            If Connection.ComClass.SetRegister(DecayAddress, MainForm.acquisition.CHList(i).decay_constant / sampling_factor, ind) = 0 Then
                                                            Else
                                                                MainForm.plog.TextBox1.AppendText("Decay Constant Set Register Error!" & vbCrLf)
                                                            End If
                                                        Else
                                                            MainForm.plog.TextBox1.AppendText("Energy Sampling Time Set Register Error!" & vbCrLf)
                                                        End If
                                                    Else
                                                        MainForm.plog.TextBox1.AppendText("Peaking Time Set Register Error!" & vbCrLf)
                                                    End If
                                                Else
                                                    MainForm.plog.TextBox1.AppendText("Flat Top Set Register Error!" & vbCrLf)
                                                End If
                                            End If
                                            n_ok += 1
                                            MainForm.plog.TextBox1.AppendText(MainForm.acquisition.CHList(i).name & " parameters applied successfully!" & vbCrLf)
                                        Else
                                            MainForm.plog.TextBox1.AppendText("PileUp Set Register Error!" & vbCrLf)
                                        End If
                                    Else
                                        MainForm.plog.TextBox1.AppendText("Trigger Hold Off Set Register Error!" & vbCrLf)
                                    End If
                                Else
                                    MainForm.plog.TextBox1.AppendText("Gain Set Register Error!" & vbCrLf)
                                End If

                            Else
                                MainForm.plog.TextBox1.AppendText("Baseline Hold Off Set Register Error!" & vbCrLf)
                            End If
                        Else
                            MainForm.plog.TextBox1.AppendText("Baseline Samples Set Register Error!" & vbCrLf)
                        End If
                    Else
                        MainForm.plog.TextBox1.AppendText("Trigger Threshold Set Register Error!" & vbCrLf)
                    End If
                Else
                    MainForm.plog.TextBox1.AppendText("Signal Polarity Set Register Error!" & vbCrLf)
                End If

            ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Then
                If Connection.ComClass.SetRegister(RunAddress, 0, ind) = 0 Then
                    If Connection.ComClass.SetRegister(GainAddress, MainForm.acquisition.CHList(i).gain * 65536, ind) = 0 Then
                        If Connection.ComClass.SetRegister(OffsetAddress, MainForm.acquisition.CHList(i).offset, ind) = 0 Then
                            If Connection.ComClass.SetRegister(TriggerKAddress, MainForm.acquisition.CHList(i).trigger_peaking / sampling_factor, ind) = 0 Then
                                If Connection.ComClass.SetRegister(TriggerMAddress, (MainForm.acquisition.CHList(i).trigger_peaking + MainForm.acquisition.CHList(i).trigger_flat) / sampling_factor, ind) = 0 Then
                                    If Connection.ComClass.SetRegister(PolarityAddress, IIf(MainForm.acquisition.CHList(i).polarity = signal_polarity.NEGATIVE, signal_polarity.POSITIVE, signal_polarity.NEGATIVE), ind) = 0 Then
                                        If Connection.ComClass.SetRegister(TriggerAddress, MainForm.acquisition.CHList(i).trigger_level, ind) = 0 Then
                                            If Connection.ComClass.SetRegister(BaselineAddress, Math.Log(MainForm.acquisition.CHList(i).baseline_sample) / Math.Log(2), ind) = 0 Then
                                                If Connection.ComClass.SetRegister(BaseHoldAddress, MainForm.acquisition.CHList(i).baseline_inhibit / sampling_factor, ind) = 0 Then
                                                    If Connection.ComClass.SetRegister(MAddress, (MainForm.acquisition.CHList(i).peaking_time + MainForm.acquisition.CHList(i).flat_top) / sampling_factor, ind) = 0 Then
                                                        If Connection.ComClass.SetRegister(KAddress, MainForm.acquisition.CHList(i).peaking_time / sampling_factor, ind) = 0 Then
                                                            If Connection.ComClass.SetRegister(SampleAddress, MainForm.acquisition.CHList(i).energy_sample / sampling_factor, ind) = 0 Then
                                                                If Connection.ComClass.SetRegister(DeconvAddress, Math.Floor(256 / (Math.Exp(8 / MainForm.acquisition.CHList(i).decay_constant) - 1)), ind) = 0 Then
                                                                    If Connection.ComClass.SetRegister(RunAddress, 1, ind) = 0 Then
                                                                        MainForm.plog.TextBox1.AppendText(MainForm.acquisition.CHList(i).name & " parameters applied successfully!" & vbCrLf)
                                                                        n_ok += 1
                                                                    Else
                                                                        MainForm.plog.TextBox1.AppendText("Run Set Register Error!" & vbCrLf)
                                                                    End If
                                                                Else
                                                                    MainForm.plog.TextBox1.AppendText("Decay Constant Set Register Error!" & vbCrLf)
                                                                End If
                                                            Else
                                                                MainForm.plog.TextBox1.AppendText("Energy Sampling Time Set Register Error!" & vbCrLf)
                                                            End If
                                                        Else
                                                            MainForm.plog.TextBox1.AppendText("Peaking Time Set Register Error!" & vbCrLf)
                                                        End If
                                                    Else
                                                        MainForm.plog.TextBox1.AppendText("Flat Top Set Register Error!" & vbCrLf)
                                                    End If
                                                Else
                                                    MainForm.plog.TextBox1.AppendText("Baseline Hold Off Set Register Error!" & vbCrLf)
                                                End If
                                            Else
                                                MainForm.plog.TextBox1.AppendText("Baseline Samples Set Register Error!" & vbCrLf)
                                            End If
                                        Else
                                            MainForm.plog.TextBox1.AppendText("Trigger Threshold Set Register Error!" & vbCrLf)
                                        End If
                                    Else
                                        MainForm.plog.TextBox1.AppendText("Signal Polarity Set Register Error!" & vbCrLf)
                                    End If
                                Else
                                    MainForm.plog.TextBox1.AppendText("Trigger M Set Register Error!" & vbCrLf)
                                End If
                            Else
                                MainForm.plog.TextBox1.AppendText("Trigger K Set Register Error!" & vbCrLf)
                            End If
                        Else
                            MainForm.plog.TextBox1.AppendText("Offset Set Register Error!" & vbCrLf)
                        End If
                    Else
                        MainForm.plog.TextBox1.AppendText("Gain Set Register Error!" & vbCrLf)
                    End If
                Else
                    MainForm.plog.TextBox1.AppendText("Run Set Register Error!" & vbCrLf)
                End If

            ElseIf Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
                If Connection.ComClass.SetRegister(RunAddress, 0, ind - 1) = 0 Then
                    If Connection.ComClass.SetRegister(GainAddress, MainForm.acquisition.CHList(i).gain * 65536, ind - 1) = 0 Then
                        If Connection.ComClass.SetRegister(OffsetAddress, MainForm.acquisition.CHList(i).offset, ind - 1) = 0 Then
                            If Connection.ComClass.SetRegister(TriggerKAddress, MainForm.acquisition.CHList(i).trigger_peaking / sampling_factor, ind - 1) = 0 Then
                                If Connection.ComClass.SetRegister(TriggerMAddress, (MainForm.acquisition.CHList(i).trigger_peaking + MainForm.acquisition.CHList(i).trigger_flat) / sampling_factor, ind - 1) = 0 Then
                                    If Connection.ComClass.SetRegister(PolarityAddress, IIf(MainForm.acquisition.CHList(i).polarity = signal_polarity.NEGATIVE, signal_polarity.POSITIVE, signal_polarity.NEGATIVE), ind - 1) = 0 Then
                                        If Connection.ComClass.SetRegister(TriggerAddress, MainForm.acquisition.CHList(i).trigger_level, ind - 1) = 0 Then
                                            If Connection.ComClass.SetRegister(BaselineAddress, Math.Log(MainForm.acquisition.CHList(i).baseline_sample) / Math.Log(2), ind - 1) = 0 Then
                                                If Connection.ComClass.SetRegister(BaseHoldAddress, MainForm.acquisition.CHList(i).baseline_inhibit / sampling_factor, ind - 1) = 0 Then
                                                    If Connection.ComClass.SetRegister(MAddress, (MainForm.acquisition.CHList(i).peaking_time + MainForm.acquisition.CHList(i).flat_top) / sampling_factor, ind - 1) = 0 Then
                                                        If Connection.ComClass.SetRegister(KAddress, MainForm.acquisition.CHList(i).peaking_time / sampling_factor, ind - 1) = 0 Then
                                                            If Connection.ComClass.SetRegister(SampleAddress, MainForm.acquisition.CHList(i).energy_sample / sampling_factor, ind - 1) = 0 Then
                                                                If Connection.ComClass.SetRegister(DeconvAddress, Math.Floor(256 / (Math.Exp(8 / MainForm.acquisition.CHList(i).decay_constant) - 1)), ind - 1) = 0 Then
                                                                    If Connection.ComClass.SetRegister(RunAddress, 1, ind - 1) = 0 Then
                                                                        MainForm.plog.TextBox1.AppendText(MainForm.acquisition.CHList(i).name & " parameters applied successfully!" & vbCrLf)
                                                                        n_ok += 1
                                                                    Else
                                                                        MainForm.plog.TextBox1.AppendText("Run Set Register Error!" & vbCrLf)
                                                                    End If
                                                                Else
                                                                    MainForm.plog.TextBox1.AppendText("Decay Constant Set Register Error!" & vbCrLf)
                                                                End If
                                                            Else
                                                                MainForm.plog.TextBox1.AppendText("Energy Sampling Time Set Register Error!" & vbCrLf)
                                                            End If
                                                        Else
                                                            MainForm.plog.TextBox1.AppendText("Peaking Time Set Register Error!" & vbCrLf)
                                                        End If
                                                    Else
                                                        MainForm.plog.TextBox1.AppendText("Flat Top Set Register Error!" & vbCrLf)
                                                    End If
                                                Else
                                                    MainForm.plog.TextBox1.AppendText("Baseline Hold Off Set Register Error!" & vbCrLf)
                                                End If
                                            Else
                                                MainForm.plog.TextBox1.AppendText("Baseline Samples Set Register Error!" & vbCrLf)
                                            End If
                                        Else
                                            MainForm.plog.TextBox1.AppendText("Trigger Threshold Set Register Error!" & vbCrLf)
                                        End If
                                    Else
                                        MainForm.plog.TextBox1.AppendText("Signal Polarity Set Register Error!" & vbCrLf)
                                    End If
                                Else
                                    MainForm.plog.TextBox1.AppendText("Trigger M Set Register Error!" & vbCrLf)
                                End If
                            Else
                                MainForm.plog.TextBox1.AppendText("Trigger K Set Register Error!" & vbCrLf)
                            End If
                        Else
                            MainForm.plog.TextBox1.AppendText("Offset Set Register Error!" & vbCrLf)
                        End If
                    Else
                        MainForm.plog.TextBox1.AppendText("Gain Set Register Error!" & vbCrLf)
                    End If
                Else
                    MainForm.plog.TextBox1.AppendText("Run Set Register Error!" & vbCrLf)
                End If
            ElseIf Connection.ComClass._boardModel = communication.tModel.SCIDK Then

                Connection.ComClass.SetRegister(AnalogOffset, MainForm.acquisition.General_settings.SignalOffset, ind)

                Connection.ComClass.SetRegister(RunAddress, 0, ind)
                Connection.ComClass.SetRegister(PolarityAddress, 1 - MainForm.acquisition.CHList(i).polarity, ind)
                Connection.ComClass.SetRegister(TriggerAddress, MainForm.acquisition.CHList(i).trigger_level, ind)
                Connection.ComClass.SetRegister(TriggerKAddress, MainForm.acquisition.CHList(i).trigger_peaking / sampling_factor, ind)
                Connection.ComClass.SetRegister(TriggerMAddress, (MainForm.acquisition.CHList(i).trigger_peaking + MainForm.acquisition.CHList(i).trigger_flat) / sampling_factor, ind)
                Connection.ComClass.SetRegister(BaselineAddress, Math.Log(MainForm.acquisition.CHList(i).baseline_sample) / Math.Log(2), ind)
                Connection.ComClass.SetRegister(BaseHoldAddress, MainForm.acquisition.CHList(i).baseline_inhibit / sampling_factor, ind)
                Connection.ComClass.SetRegister(GainAddress, MainForm.acquisition.CHList(i).gain * 65536, ind)
                Connection.ComClass.SetRegister(MAddress, (MainForm.acquisition.CHList(i).peaking_time + MainForm.acquisition.CHList(i).flat_top) / sampling_factor, ind)
                Connection.ComClass.SetRegister(KAddress, MainForm.acquisition.CHList(i).peaking_time / sampling_factor, ind)
                Connection.ComClass.SetRegister(DecayAddress, Math.Floor(256 / (Math.Exp(16.6 / MainForm.acquisition.CHList(i).decay_constant) - 1)), ind)
                Connection.ComClass.SetRegister(SampleAddress, MainForm.acquisition.CHList(i).energy_sample / sampling_factor, ind)

                Connection.ComClass.SetRegister(RunAddress, 1, ind)
                n_ok += 1
                MainForm.plog.TextBox1.AppendText(MainForm.acquisition.CHList(i).name & " parameters applied successfully!" & vbCrLf)
            End If
        Next

        If Connection.ComClass._boardModel = communication.tModel.DT5560SE Then
            Dim j = 0
            Dim n_2 = n / 2 - 1
            Dim term(n_2) As Integer
            Dim divis(n_2) As Integer
            Dim off(n) As Integer
            Dim g(n_2) As Integer
            Dim ch_2(n_2) As Integer
            Dim ch(n - 1) As Integer
            Dim k = 0
            For j = 0 To n - 2 Step 2
                If MainForm.acquisition.CHList(j).Afe_set.Termination Then
                    term(k) = 1
                Else
                    term(k) = 0
                End If

                If MainForm.acquisition.CHList(j).Afe_set.Division Then
                    divis(k) = 0
                Else
                    divis(k) = 1
                End If
                ch_2(k) = j + 1
                ch(j) = j
                ch(j + 1) = j + 1
                off(j) = MainForm.acquisition.CHList(j).Afe_set.Offset
                off(j + 1) = MainForm.acquisition.CHList(j + 1).Afe_set.Offset

                'Dim index = 0
                'For l = 0 To gain_list.Length - 1
                '    If gain_list(l) = MainForm.acquisition.CHList(j).Afe_set.Gain Then
                '        index = l
                '        Exit For
                '    End If
                'Next
                'g(k) = index

                g(k) = CType(Math.Log10(MainForm.acquisition.CHList(j).Afe_set.Gain) * 40, Int16)

                k += 1
            Next

            Dim s As String = ""
            If MainForm.acquisition.General_settings.AFEShaper = 0 Then
                s = "dc"
            ElseIf MainForm.acquisition.General_settings.AFEShaper = 1 Then
                s = "1u"
            ElseIf MainForm.acquisition.General_settings.AFEShaper = 2 Then
                s = "10u"
            ElseIf MainForm.acquisition.General_settings.AFEShaper = 3 Then
                s = "30u"
            End If
            If Connection.ComClass.SetShaper(s) Then
                MainForm.plog.TextBox1.AppendText("AFE Settings: Shaper applied successfully!" & vbCrLf)
            Else
                MainForm.plog.TextBox1.AppendText("AFE Settings: Shaper applied successfully!" & vbCrLf)
            End If

            If Connection.ComClass.SetAfeParam("Termination", "Division", term, divis, ch_2, ch_2, 1) Then
                MainForm.plog.TextBox1.AppendText("AFE Settings: Termination applied successfully!" & vbCrLf)
                MainForm.plog.TextBox1.AppendText("AFE Settings: Division applied successfully!" & vbCrLf)
            Else
                MainForm.plog.TextBox1.AppendText("AFE Settings: Termination Error!" & vbCrLf)
                MainForm.plog.TextBox1.AppendText("AFE Settings: Division Error!" & vbCrLf)
            End If

            If Connection.ComClass.SetAfeParam("Offset", "Gain", off, g, ch, ch_2, 1) Then
                MainForm.plog.TextBox1.AppendText("AFE Settings: Offset applied successfully!" & vbCrLf)
                MainForm.plog.TextBox1.AppendText("AFE Settings: Gain applied successfully!" & vbCrLf)
            Else
                MainForm.plog.TextBox1.AppendText("AFE Settings: Offset Error!" & vbCrLf)
                MainForm.plog.TextBox1.AppendText("AFE Settings: Gain Error!" & vbCrLf)
            End If

        End If

            If n_ok = MainForm.acquisition.CHList.Count Then
            Apply.Enabled = False
            Apply.BackColor = Color.LightGray
        Else
            Apply.Enabled = False
            Apply.BackColor = Color.Red
        End If

    End Sub

    Private Sub Horizontal_validating(sender As Object, e As EventArgs) Handles Horizontal.Validating
        Apply.Enabled = True
        Apply.BackColor = Color.DodgerBlue
        If Horizontal.Text <> "" Then
            If Horizontal.Text < sampling_factor Then
                Horizontal.Text = sampling_factor
            End If
            If Horizontal.Text > 65535 Then
                Horizontal.Text = 65535
            End If
            Dim dec = Math.Round(Horizontal.Text / sampling_factor)
            Horizontal.Text = Math.Round(dec * sampling_factor, 1)
        End If

    End Sub

    Private Sub DataGridView1_Resize(sender As Object, e As EventArgs) Handles DataGridView1.Resize

        If inhibit = False Then
            If l < DataGridView1.Width Then
                DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Else
                DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader
            End If
        End If

    End Sub

    Private Sub DataGridView2_Resize(sender As Object, e As EventArgs)

        If inhibit2 = False Then
            If l < DataGridView2.Width Then
                DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Else
                DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader
            End If
        End If

    End Sub

    Private Sub PreTrigger_TextChanged(sender As Object, e As EventArgs) Handles PreTrigger.TextChanged
        Apply.Enabled = True
        Apply.BackColor = Color.DodgerBlue
        If PreTrigger.Text <> "" Then
            If PreTrigger.Text > 100 Then
                PreTrigger.Text = 100
            End If
            If PreTrigger.Text < 0 Then
                PreTrigger.Text = 0
            End If
        End If

    End Sub

    Private Sub TriggerLevelOscilloscope_TextChanged(sender As Object, e As EventArgs) Handles TriggerLevelOscilloscope.TextChanged
        Apply.Enabled = True
        Apply.BackColor = Color.DodgerBlue
        If TriggerLevelOscilloscope.Text <> "" Then
            If TriggerLevelOscilloscope.Text > 65535 Then
                TriggerLevelOscilloscope.Text = 65535
            End If
            If TriggerLevelOscilloscope.Text < 0 Then
                TriggerLevelOscilloscope.Text = 0
            End If
        End If

    End Sub

    Private Sub Shaper_SelectedIndexChanged(sender As Object, e As EventArgs)
        Apply.Enabled = True
        Apply.BackColor = Color.DodgerBlue
    End Sub

    Private Sub TriggerEdge_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TriggerEdge.SelectedIndexChanged
        Apply.Enabled = True
        Apply.BackColor = Color.DodgerBlue
    End Sub

    Private Sub TriggerSourceOscilloscope_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TriggerSourceOscilloscope.SelectedIndexChanged
        Apply.Enabled = True
        Apply.BackColor = Color.DodgerBlue
    End Sub

    Public Shared Sub SetDoubleBuffered(ByVal control As Control)
        GetType(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty Or BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, control, New Object() {True})
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MainForm.ofsc.ShowDialog()
    End Sub

    Private Sub Horizontal_ValueChanged(sender As Object, e As EventArgs) Handles Horizontal.ValueChanged
        Apply.Enabled = True
        Apply.BackColor = Color.DodgerBlue
    End Sub

    Private Sub Panel5_Paint(sender As Object, e As PaintEventArgs) Handles Panel5.Paint

    End Sub
End Class
