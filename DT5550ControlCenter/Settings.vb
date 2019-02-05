Imports System.ComponentModel
Imports System.Globalization
Imports System.Reflection
Imports DT5550ControlCenter.AcquisitionClass

Public Class Settings

    Dim inhibit = True
    Dim l As Integer = 814
    Dim n As Integer

    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load


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

        TriggerEdge.Items.Clear()
        TriggerEdge.Items.Add("Rising")
        TriggerEdge.Items.Add("Falling")

        inhibit = True

        DataGridView1.Columns.Clear()
        DataGridView1.Columns.Add("Channel", "Channel")
        Dim pol_column As New DataGridViewComboBoxColumn()
        pol_column.HeaderText = "Polarity"
        pol_column.Name = "Polarity"
        pol_column.MaxDropDownItems = 2
        pol_column.Items.Add("Positive")
        pol_column.Items.Add("Negative")
        DataGridView1.Columns.Add(pol_column)
        DataGridView1.Columns.Add("Trigger Level", "Trigger Level (lsb)")
        DataGridView1.Columns.Add("Trigger Hold-Off", "Trigger Hold-Off (ns)")

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

        Dim pileup As New DataGridViewCheckBoxColumn
        pileup.HeaderText = "Pileup Rejection"
        pileup.Name = "Pileup Rejection"
        DataGridView1.Columns.Add(pileup)

        DataGridView1.Columns.Add("Pileup Rejection Time", "Pileup Rejection Time (ns)")
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

        Settings_reload()
        Grid_ReLoad()

    End Sub

    Public Sub Settings_reload()

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

        If MainForm.acquisition.General_settings.TriggerSourceOscilloscope = trigger_source.FREE Then
            TriggerSourceOscilloscope.SelectedIndex = 2
        ElseIf MainForm.acquisition.General_settings.TriggerSourceOscilloscope = trigger_source.EXTERNAL Then
            TriggerSourceOscilloscope.SelectedIndex = 1
        ElseIf MainForm.acquisition.General_settings.TriggerSourceOscilloscope = trigger_source.LEVEL Then
            TriggerSourceOscilloscope.SelectedIndex = MainForm.acquisition.General_settings.TriggerChannelOscilloscope + 3
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
        Horizontal.Text = MainForm.acquisition.General_settings.OscilloscopeDecimator * 12.5

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
            DataGridView1.Rows(i).Cells("Trigger Level").Value = MainForm.acquisition.CHList(i).trigger_level '"CHANNEL " & (i + 1).ToString
            DataGridView1.Rows(i).Cells("Trigger Hold-Off").Value = MainForm.acquisition.CHList(i).trigger_inhibit '"CHANNEL " & (i + 1).ToString
            DataGridView1.Rows(i).Cells("Baseline Inhibit Time").Value = MainForm.acquisition.CHList(i).baseline_inhibit
            DataGridView1.Rows(i).Cells("Pileup Rejection").Value = MainForm.acquisition.CHList(i).pileup_enable
            If DataGridView1.Rows(i).Cells("Pileup Rejection").Value Then
                DataGridView1.Rows(i).Cells("Pileup Rejection Time").ReadOnly = False
                DataGridView1.Rows(i).Cells("Pileup Rejection Time").Value = MainForm.acquisition.CHList(i).pileup_time
            Else
                DataGridView1.Rows(i).Cells("Pileup Rejection Time").ReadOnly = True
            End If
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

    Private Sub DataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged

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
            If DataGridView1.Rows(e.RowIndex).Cells("Pileup Rejection").Value = True Then
                DataGridView1.Rows(e.RowIndex).Cells("Pileup Rejection Time").ReadOnly = False
            Else
                DataGridView1.Rows(e.RowIndex).Cells("Pileup Rejection Time").ReadOnly = True
            End If
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

        If TriggerEdge.SelectedItem = "Rising" Then
            MainForm.acquisition.General_settings.TriggerOscilloscopeEdges = AcquisitionClass.edge.RISING
        Else
            MainForm.acquisition.General_settings.TriggerOscilloscopeEdges = AcquisitionClass.edge.FALLING
        End If
        If TriggerSourceOscilloscope.SelectedItem = "Internal" Then
            MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.INTERNAL
        ElseIf TriggerSourceOscilloscope.SelectedItem = "External" Then
            MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.EXTERNAL
        ElseIf TriggerSourceOscilloscope.SelectedItem = "Free Running" Then
            MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.FREE
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
        MainForm.acquisition.General_settings.OscilloscopeDecimator = Math.Round((Horizontal.Text).Replace(".", ",") / 12.5)
        MainForm.acquisition.General_settings.TriggerOscilloscopeLevel = TriggerLevelOscilloscope.Text

        For i = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells("Polarity").Value = "Positive" Then
                MainForm.acquisition.CHList(i).polarity = signal_polarity.POSITIVE
            Else
                MainForm.acquisition.CHList(i).polarity = signal_polarity.NEGATIVE
            End If
            MainForm.acquisition.CHList(i).trigger_level = DataGridView1.Rows(i).Cells("Trigger Level").Value
            MainForm.acquisition.CHList(i).trigger_inhibit = DataGridView1.Rows(i).Cells("Trigger Hold-Off").Value

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

            If DataGridView1.Rows(i).Cells("Pileup Rejection").Value Then
                MainForm.acquisition.CHList(i).pileup_enable = True
                MainForm.acquisition.CHList(i).pileup_time = DataGridView1.Rows(i).Cells("Pileup Rejection Time").Value
            Else
                MainForm.acquisition.CHList(i).pileup_enable = False
            End If
            MainForm.acquisition.CHList(i).baseline_inhibit = DataGridView1.Rows(i).Cells("Baseline Inhibit Time").Value
            MainForm.acquisition.CHList(i).baseline_sample = CType(DataGridView1.Rows(i).Cells("Baseline Lenght").Value, Integer)
        Next

    End Sub

    Private Function SingleToHex(ByVal sing As Single)

        Dim arr = BitConverter.GetBytes(sing)
        Array.Reverse(arr)
        Return Convert.ToUInt32(BitConverter.ToString(arr).Replace("-", ""), 16)

    End Function

    Private Sub UploadSettings()


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

        If Connection.ComClass.SetRegister(TrModeAddress, MainForm.acquisition.General_settings.TriggerMode) = 0 Then
        Else
            MainForm.plog.TextBox1.AppendText("Trigger Mode Set Register Error!" & vbCrLf)
        End If
        If Connection.ComClass.SetRegister(TrSourceAddress, MainForm.acquisition.General_settings.TriggerSource) = 0 Then
        Else
            MainForm.plog.TextBox1.AppendText("Trigger Source Set Register Error!" & vbCrLf)
        End If
        If Connection.ComClass.SetRegister(TrDelayAddress, MainForm.acquisition.General_settings.TriggerDelay) = 0 Then
        Else
            MainForm.plog.TextBox1.AppendText("Trigger Delay Set Register Error!" & vbCrLf)
        End If
        If Connection.ComClass.SetRegister(TrOffsetAddress, MainForm.acquisition.General_settings.SignalOffset) = 0 Then
        Else
            MainForm.plog.TextBox1.AppendText("Trigger Offset Set Register Error!" & vbCrLf)
        End If
        If MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.INTERNAL Or MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.EXTERNAL Then
            If Connection.ComClass.SetRegister(TrOscilloscopeAddress, MainForm.acquisition.General_settings.TriggerSourceOscilloscope) = 0 Then
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
                Connection.ComClass.SetRegister(ANOFS, IIf(coor(31 - i) >= 0, coor(31 - i), &HFFFFFFFF& + coor(31 - i)))

            End If
        Next

        For i = 0 To MainForm.acquisition.CHList.Count - 1
            Dim index = MainForm.acquisition.CHList.Count - MainForm.acquisition.CHList(i).id
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
            Dim GainAddress As New UInt32
            Dim TRHoldOffAddress As New UInt32
            Dim PileUpAddress As New UInt32

            For Each r In MainForm.CurrentRegisterList
                If r.Name = "POL_" & index Then
                    PolarityAddress = r.Address
                End If
                If r.Name = "TH_" & index Then
                    TriggerAddress = r.Address
                End If
                If r.Name = "BLLEN_" & index Then
                    BaselineAddress = r.Address
                End If
                If r.Name = "SIGLEN_" & index Then
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
                    If r.Name = "FLAT_" & index Then
                        FlatAddress = r.Address
                    End If
                    If r.Name = "PEAK_" & index Then
                        PeakAddress = r.Address
                    End If
                    If r.Name = "ENERGY_" & index Then
                        EnergyAddress = r.Address
                    End If
                    If r.Name = "DECAY_" & index Then
                        DecayAddress = r.Address
                    End If
                End If
                If r.Name = "GAIN_" & index Then
                    GainAddress = r.Address
                End If
                If r.Name = "TRINIB" & index Then
                    TRHoldOffAddress = r.Address
                End If
                If r.Name = "PUP_" & index Then
                    PileUpAddress = r.Address
                End If


            Next


            If Connection.ComClass.SetRegister(PolarityAddress, MainForm.acquisition.CHList(i).polarity) = 0 Then
                If Connection.ComClass.SetRegister(TriggerAddress, MainForm.acquisition.CHList(i).trigger_level) = 0 Then
                    If Connection.ComClass.SetRegister(BaselineAddress, Math.Log(MainForm.acquisition.CHList(i).baseline_sample) / Math.Log(2)) = 0 Then
                        If Connection.ComClass.SetRegister(BaseHoldAddress, MainForm.acquisition.CHList(i).baseline_inhibit * 80 / 1000) = 0 Then
                            If MainForm.isChargeIntegration Then
                                If Connection.ComClass.SetRegister(IntAddress, MainForm.acquisition.CHList(i).integration_time * 80 / 1000) = 0 Then
                                    If Connection.ComClass.SetRegister(PreIntAddress, MainForm.acquisition.CHList(i).pre_gate * 80 / 1000) = 0 Then
                                    Else
                                        MainForm.plog.TextBox1.AppendText("PreGate Set Register Error!" & vbCrLf)
                                    End If
                                Else
                                    MainForm.plog.TextBox1.AppendText("Integration Time Set Register Error!" & vbCrLf)
                                End If
                            Else
                                If Connection.ComClass.SetRegister(FlatAddress, MainForm.acquisition.CHList(i).flat_top * 80 / 1000) = 0 Then
                                    If Connection.ComClass.SetRegister(PeakAddress, MainForm.acquisition.CHList(i).peaking_time * 80 / 1000) = 0 Then
                                        If Connection.ComClass.SetRegister(EnergyAddress, MainForm.acquisition.CHList(i).energy_sample * 80 / 1000) = 0 Then
                                            If Connection.ComClass.SetRegister(DecayAddress, MainForm.acquisition.CHList(i).decay_constant * 80 / 1000) = 0 Then
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
                            If Connection.ComClass.SetRegister(GainAddress, MainForm.acquisition.CHList(i).gain / MainForm.acquisition.CHList(i).integration_time * 65535) = 0 Then
                                If Connection.ComClass.SetRegister(TRHoldOffAddress, MainForm.acquisition.CHList(i).trigger_inhibit * 80 / 1000) = 0 Then
                                    Dim pup_value = IIf(MainForm.acquisition.CHList(i).pileup_enable = True, MainForm.acquisition.CHList(i).pileup_time * 80 / 1000, 0)
                                    If Connection.ComClass.SetRegister(PileUpAddress, pup_value) = 0 Then
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
        Next

    End Sub

    Private Sub Horizontal_validating(sender As Object, e As EventArgs) Handles Horizontal.Validating

        If Horizontal.Text <> "" Then
            If Horizontal.Text < 12.5 Then
                Horizontal.Text = 12.5
            End If
            If Horizontal.Text > 65535 Then
                Horizontal.Text = 65535
            End If
            Dim dec = Math.Round(Horizontal.Text / 12.5)
            Horizontal.Text = dec * 1000 / 80
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

    Private Sub PreTrigger_TextChanged(sender As Object, e As EventArgs) Handles PreTrigger.TextChanged

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

        If TriggerLevelOscilloscope.Text <> "" Then
            If TriggerLevelOscilloscope.Text > 65535 Then
                TriggerLevelOscilloscope.Text = 65535
            End If
            If TriggerLevelOscilloscope.Text < 0 Then
                TriggerLevelOscilloscope.Text = 0
            End If
        End If

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Public Shared Sub SetDoubleBuffered(ByVal control As Control)
        GetType(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty Or BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, control, New Object() {True})
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MainForm.ofsc.ShowDialog()
    End Sub

    Private Sub Offset_ValueChanged(sender As Object, e As EventArgs) Handles Offset.ValueChanged
        'If System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator = "," Then
        '    sender.value = sender.Text.Replace(".", ",")
        'End If

        'If System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator = "." Then
        '    sender.Text = sender.Text.Replace(",", ".")
        'End If


    End Sub

    Private Sub Offset_Validating(sender As Object, e As CancelEventArgs) Handles Offset.Validating

    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Panel3.Paint

    End Sub
End Class
