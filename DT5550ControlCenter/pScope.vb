'Imports System.IO
'Imports OxyPlot
'Imports OxyPlot.Series

'Public Class pScope

'    Public running As Boolean = False
'    Dim inhibit = True
'    Dim osc_ch As Integer
'    Public addressData As List(Of UInt32)
'    Public addressDecimator As List(Of UInt32)
'    Public addressPre As List(Of UInt32)
'    Public addressMode As List(Of UInt32)
'    Public addressLevel As List(Of UInt32)
'    Public addressArm As List(Of UInt32)
'    Public addressStatus As List(Of UInt32)
'    Public addressPosition As List(Of UInt32)
'    Public nsamples As UInt32
'    Public wavecount = 0
'    Dim DecimatorValue As Double
'    Dim PreTriggerValue As Double
'    Dim LevelTriggerValue As Double
'    Dim TriggerModeValue As UInt32


'    Public EnabledChannel() As Boolean
'    Public fileEnable As Boolean = False
'    Public fileName As String

'    Dim title() As Label
'    Dim plots() As OxyPlot.WindowsForms.PlotView
'    Dim models() As PlotModel
'    Dim seriesAnalog() As OxyPlot.Series.LineSeries
'    Dim seriesAnalog2() As OxyPlot.Series.LineSeries
'    Dim seriesDigital0() As OxyPlot.Series.LineSeries
'    Dim seriesDigital1() As OxyPlot.Series.LineSeries
'    Dim seriesDigital2() As OxyPlot.Series.LineSeries
'    Dim seriesDigital3() As OxyPlot.Series.LineSeries
'    Dim ChList_name As New List(Of String)

'    Private Sub pScope_Load(sender As Object, e As EventArgs) Handles MyBase.Load

'        For o = 0 To Connection.ComClass._n_oscilloscope - 1
'            addressData(o) = MainForm.CurrentOscilloscopes(o).Address
'        Next
'        nsamples = MainForm.CurrentOscilloscopes(0).nsamples

'        osc_ch = Connection.ComClass._n_ch * Connection.ComClass._nBoard - 1
'        For i = 0 To osc_ch
'            ChList_name.Add(MainForm.acquisition.CHList(i).name)
'        Next
'        pScope_ReLoad(MainForm.acquisition.CHList.Count)

'    End Sub

'    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged
'        MainForm.acquisition.CHList(CheckedListBox1.SelectedIndex).scope_checked = IIf(CheckedListBox1.GetItemCheckState(CheckedListBox1.SelectedIndex).ToString = "Checked", True, False)
'    End Sub

'    Public Sub pScope_ReLoad(n As Integer)

'        CheckedListBox1.Items.Clear()
'        ReDim EnabledChannel(n - 1)
'        For i = 0 To n - 1
'            CheckedListBox1.Items.Add(ChList_name(i), MainForm.acquisition.CHList(i).scope_checked)
'            EnabledChannel(i) = False
'        Next

'        Me.Panel1.Controls.Clear()

'        ReDim title(n - 1)
'        ReDim plots(n - 1)
'        ReDim models(n - 1)
'        ReDim seriesAnalog(n - 1)
'        ReDim seriesDigital0(n - 1)
'        ReDim seriesDigital1(n - 1)
'        ReDim seriesDigital2(n - 1)
'        ReDim seriesDigital3(n - 1)

'        Dim Model = New PlotModel()


'        For i = 0 To n - 1
'            seriesAnalog(i) = New OxyPlot.Series.LineSeries() With {
'            .Title = "Analog",
'             .MarkerType = MarkerType.None',  .ItemsSource = dc(i).dataAnalog
'            }
'            If Connection.CustomFirmware Then
'                seriesDigital0(i) = New OxyPlot.Series.LineSeries() With {
'                .Title = "Digital1",
'                 .MarkerType = MarkerType.None', .ItemsSource = dc(i).dataDigital0
'                }
'                seriesDigital1(i) = New OxyPlot.Series.LineSeries() With {
'                .Title = "Digital2",
'                 .MarkerType = MarkerType.None',.ItemsSource = dc(i).dataDigital1
'                }
'                seriesDigital2(i) = New OxyPlot.Series.LineSeries() With {
'                .Title = "Digital3",
'                 .MarkerType = MarkerType.None'    .ItemsSource = dc(i).dataDigital2
'                }
'                seriesDigital3(i) = New OxyPlot.Series.LineSeries() With {
'                .Title = "Digital4",
'                .MarkerType = MarkerType.None,'          .ItemsSource = dc(i).dataDigital3
'                .Color = OxyColor.FromArgb(255, 255, 114, 166)
'                    }
'            Else
'                models(i) = New PlotModel()
'                ' ChannelToLabel(i, col, row)
'                ' models(i).Title = col & " - " & row

'                seriesDigital0(i) = New OxyPlot.Series.LineSeries() With {
'            .Title = "Integration",
'             .MarkerType = MarkerType.None', .ItemsSource = dc(i).dataDigital0
'            }
'                seriesDigital1(i) = New OxyPlot.Series.LineSeries() With {
'                .Title = "Baseline",
'                 .MarkerType = MarkerType.None',.ItemsSource = dc(i).dataDigital1
'                }
'                seriesDigital2(i) = New OxyPlot.Series.LineSeries() With {
'                .Title = "Trigger",
'                 .MarkerType = MarkerType.None'    .ItemsSource = dc(i).dataDigital2
'                }
'                seriesDigital3(i) = New OxyPlot.Series.LineSeries() With {
'                .Title = "Pile Up",
'                .MarkerType = MarkerType.None'          .ItemsSource = dc(i).dataDigital3
'                 }
'            End If
'            models(i).Axes.Add(New Axes.LinearAxis() With {
'        .Position = Axes.AxisPosition.Bottom,
'        .Maximum = nsamples * (MainForm.acquisition.General_settings.OscilloscopeDecimator) * 1000 / 80,
'        .Minimum = 0,
'        .FontSize = 10,
'        .Key = "x",
'        .Title = "Time (ns)"
'    })

'            models(i).Axes.Add(New Axes.LinearAxis() With {
'    .Position = Axes.AxisPosition.Left,
'    .Maximum = 66000,
'    .Minimum = 0,
'     .FontSize = 10,
'     .Key = "y",
'     .Title = "Amplitude (lsb)"
'})
'            'seriesE(i) = New OxyPlot.Series.LineSeries() With {
'            '.Title = "Pileup INIB",
'            ' .MarkerType = MarkerType.None,
'            '    .ItemsSource = dc(i).dataE
'            '}
'            models(i).Series.Add(seriesAnalog(i))
'            models(i).Series.Add(seriesDigital0(i))
'            models(i).Series.Add(seriesDigital1(i))
'            models(i).Series.Add(seriesDigital2(i))
'            models(i).Series.Add(seriesDigital3(i))
'            'models(i).Series.Add(seriesE(i))

'            title(i) = New Label
'            '  trig = New ComboBox
'            title(i).Height = 20

'            plots(i) = New OxyPlot.WindowsForms.PlotView
'            plots(i).Height = 450
'            plots(i).Top = (i * (plots(i).Height + title(i).Height) + 30)
'            plots(i).Anchor = AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top
'            plots(i).Width = Me.Width - 20
'            plots(i).Left = 10
'            plots(i).Model = models(i)
'            plots(i).Refresh()
'            title(i).Text = ChList_name(i) '"CHANNEL " & (i + 1).ToString

'            title(i).Width = 90
'            title(i).Font = New Font(title(i).Font, FontStyle.Bold)
'            title(i).Top = (i * (plots(i).Height + title(i).Height) + 10)
'            title(i).Anchor = AnchorStyles.Left + AnchorStyles.Top
'            title(i).Left = 10

'            Me.Panel1.Controls.Add(title(i))
'            ' Me.Controls.Add(trig)

'            'models(i).DefaultYAxis.IsZoomEnabled = False
'            Me.Panel1.Controls.Add(plots(i))
'            plots(i).InvalidatePlot(True)
'        Next

'        inhibit = False

'    End Sub

'    Private Sub autosizey(plot As OxyPlot.WindowsForms.PlotView, max As Integer)
'        For Each a In plot.Model.Axes
'            If a.Key = "y" Then
'                a.Maximum = max + max * 0.1
'            End If
'        Next
'    End Sub

'    Private Sub autosizex(max As Integer)
'        For i = 0 To plots.Count - 1
'            For Each a In plots(i).Model.Axes
'                If a.Key = "x" Then
'                    a.Maximum = max
'                End If
'            Next
'        Next
'    End Sub


'    'Public Sub CLICK_EVENT_COMBOBOX(ByVal sender As System.Object, ByVal e As System.EventArgs)
'    '    If inhibit = False Then
'    '        If sender.selectedindex = 0 Then
'    '            MainForm.acquisition.CHList(sender.name).trigtype = AcquisitionClass.Channel.triggertype.AUTO
'    '        Else
'    '            MainForm.acquisition.CHList(sender.name).trigtype = AcquisitionClass.Channel.triggertype.MAIN
'    '        End If
'    '    End If
'    'End Sub

'    Public Sub StartFreeRunningMultiAcquisition()
'        copyOscilloscopeParam()
'        setOscilloscopeParam()
'        Timer1.Enabled = True
'        running = True
'        MainForm.sets.Apply.Enabled = False
'        MainForm.plog.TextBox1.AppendText(vbCrLf & "Starting Oscilloscope Free Running Acquisition...")
'    End Sub

'    'Public Sub StartTriggeredMultiAcquisition()
'    '    selfTrigger = False
'    '    setOscilloscopeParam()
'    '    Timer1.Enabled = True
'    '    running = True
'    '    MainForm.sets.Button1.Enabled = False
'    'End Sub

'    Public Sub SingleShot()
'        copyOscilloscopeParam()
'        setOscilloscopeParam()
'        MainForm.plog.TextBox1.AppendText(vbCrLf & "Oscilloscope Single Shot Acquisition!")
'        SingleShotA()
'    End Sub

'    Public Sub StopAcquisition()
'        Timer1.Enabled = False
'        running = False
'        MainForm.sets.Apply.Enabled = True
'        MainForm.plog.TextBox1.AppendText(vbCrLf & "Stopping Oscilloscope Free Running Acquisition.")

'    End Sub

'    Public Sub copyOscilloscopeParam()
'        DecimatorValue = MainForm.acquisition.General_settings.OscilloscopeDecimator
'        PreTriggerValue = MainForm.acquisition.General_settings.OscilloscopePreTrigger
'        LevelTriggerValue = MainForm.acquisition.General_settings.TriggerOscilloscopeLevel
'        If MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.EXTERNAL Then
'            TriggerModeValue = Convert.ToInt32(MainForm.acquisition.General_settings.TriggerOscilloscopeEdges & "000", 2)
'        ElseIf MainForm.acquisition.General_settings.TriggerSourceOscilloscope = AcquisitionClass.trigger_source.INTERNAL Then
'            TriggerModeValue = Convert.ToInt32("1000" & MainForm.acquisition.General_settings.TriggerOscilloscopeEdges & "010", 2)
'        Else
'            Dim wword As UInt32
'            wword = ((osc_ch - MainForm.acquisition.General_settings.TriggerChannelOscilloscope) << 8) + (MainForm.acquisition.General_settings.TriggerOscilloscopeEdges << 3) + 1
'            TriggerModeValue = wword
'        End If

'    End Sub

'    Public Sub setOscilloscopeParam()

'        ' Dim MuxAddress As New UInt32

'        'For i = 0 To MainForm.acquisition.CHList.Count - 1
'        '    Dim index = MainForm.acquisition.CHList(i).id - 1
'        '    For Each r In MainForm.CurrentRegisterList
'        '        If r.Name = "MMUX" & index Then
'        '            MuxAddress = r.Address
'        '        End If
'        '    Next
'        '    'If Connection.ComClass.SetRegister(MuxAddress, MainForm.acquisition.General_settings.mux) = 0 Then
'        '    'Else
'        '    '    MainForm.plog.TextBox1.AppendText(vbCrLf & "Mux Set Register Error!")
'        '    'End If
'        'Next
'        For i = 0 To Connection.ComClass._n_oscilloscope - 1
'            If Connection.ComClass.SetRegister(addressDecimator(i), DecimatorValue - 1) = 0 Then
'                If Connection.ComClass.SetRegister(addressPre(i), Math.Floor(PreTriggerValue * nsamples / 100)) = 0 Then

'                    ' If MainForm.oscilloscope.General_settings.mux <> 4 Then
'                    'Dim chaddr As Integer
'                    'chaddr = MainForm.oscilloscope.General_settings.mux
'                    'mode_value = Convert.ToInt32(chaddr & "0000" & MainForm.oscilloscope.General_settings.edges & "001", 2)
'                    'End If

'                    If Connection.ComClass.SetRegister(addressMode(i), TriggerModeValue) = 0 Then
'                        If Connection.ComClass.SetRegister(addressLevel(i), LevelTriggerValue) = 0 Then
'                            If Connection.ComClass.SetRegister(addressArm(i), 0) = 0 Then
'                                If Connection.ComClass.SetRegister(addressArm(i), 1) = 0 Then

'                                Else
'                                    MainForm.plog.TextBox1.AppendText(vbCrLf & "Error on CONFIG_ARM!")
'                                End If
'                            Else
'                                MainForm.plog.TextBox1.AppendText(vbCrLf & "Error on CONFIG_ARM!")
'                            End If
'                        Else
'                            MainForm.plog.TextBox1.AppendText(vbCrLf & "Error on CONFIG_TRIGGER_LEVEL!")
'                        End If
'                    Else
'                        MainForm.plog.TextBox1.AppendText(vbCrLf & "Error on CONFIG_TRIGGER_NODE!")
'                    End If
'                Else
'                    MainForm.plog.TextBox1.AppendText(vbCrLf & "Error on CONFIG_PRETRIGGER!")
'                End If
'            Else
'                MainForm.plog.TextBox1.AppendText(vbCrLf & "Error on CONFIG_DECIMATOR!")
'            End If
'        Next
'    End Sub

'    Public Sub SingleShotA()


'        setOscilloscopeParam()

'        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then


'            For i = 0 To osc_ch
'                seriesAnalog(i).Points.Clear()
'                seriesDigital0(i).Points.Clear()
'                seriesDigital1(i).Points.Clear()
'                seriesDigital2(i).Points.Clear()
'                seriesDigital3(i).Points.Clear()
'                plots(i).Model.Series.Clear()
'            Next

'            Dim status As UInt32 = 0
'            Dim tt = Now
'            While status <> 1
'                Connection.ComClass.GetRegister(addressStatus(0), status)
'                If (Now - tt).TotalMilliseconds > 2000 Then
'                    Exit Sub
'                End If
'            End While

'            Dim position As UInt32
'            Connection.ComClass.GetRegister(addressPosition(0), position)

'            Dim lenght = nsamples * (osc_ch + 1)
'            Dim data(lenght) As UInt32
'            Dim read_data As UInt32
'            Dim valid_data As UInt32
'            If Connection.ComClass.ReadData(addressData(0), data, lenght, 0, 1000, read_data, valid_data) = 0 Then

'                Dim curr As Integer = position - Math.Floor(PreTriggerValue * nsamples / 100)
'                If curr > 0 Then
'                    For ch = 0 To osc_ch
'                        If CheckedListBox1.GetItemCheckState(ch) Then
'                            Dim k = 0
'                            Dim mmax = 0
'                            For i = 0 To nsamples - 1
'                                mmax = Math.Max(data(i + nsamples * ch) And &HFFFF, mmax)
'                            Next
'                            For i = curr To nsamples - 1
'                                seriesAnalog(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, data(i + nsamples * ch) And 65535))
'                                seriesDigital0(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 16 And 1) * mmax))
'                                seriesDigital1(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 17 And 1) * mmax))
'                                seriesDigital2(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 18 And 1) * mmax))
'                                seriesDigital3(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 19 And 1) * mmax))
'                                k += 1
'                            Next
'                            For i = 0 To curr - 1
'                                seriesAnalog(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, data(i + nsamples * ch) And 65535))
'                                seriesDigital0(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 16 And 1) * mmax))
'                                seriesDigital1(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 17 And 1) * mmax))
'                                seriesDigital2(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 18 And 1) * mmax))
'                                seriesDigital3(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 19 And 1) * mmax))
'                                k += 1
'                            Next

'                            autosizey(plots(osc_ch - ch), mmax)
'                            plots(osc_ch - ch).Model.Series.Add(seriesAnalog(ch))
'                            plots(osc_ch - ch).Model.Series.Add(seriesDigital0(ch))
'                            plots(osc_ch - ch).Model.Series.Add(seriesDigital1(ch))
'                            plots(osc_ch - ch).Model.Series.Add(seriesDigital2(ch))
'                            plots(osc_ch - ch).Model.Series.Add(seriesDigital3(ch))
'                        End If
'                    Next
'                Else
'                    For ch = 0 To osc_ch
'                        If CheckedListBox1.CheckedIndices.Contains(ch) Then
'                            Dim k = 0
'                            Dim mmax = 0
'                            For i = 0 To nsamples - 1
'                                mmax = Math.Max(data(i + nsamples * ch) And &HFFFF, mmax)
'                            Next
'                            For i = nsamples + curr To nsamples - 1
'                                seriesAnalog(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, data(i + nsamples * ch) And 65535))
'                                seriesDigital0(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 16 And 1) * mmax))
'                                seriesDigital1(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 17 And 1) * mmax))
'                                seriesDigital2(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 18 And 1) * mmax))
'                                seriesDigital3(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 19 And 1) * mmax))
'                                k += 1
'                            Next
'                            For i = 0 To nsamples + curr - 1
'                                seriesAnalog(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, data(i + nsamples * ch) And 65535))
'                                seriesDigital0(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 16 And 1) * mmax))
'                                seriesDigital1(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 17 And 1) * mmax))
'                                seriesDigital2(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 18 And 1) * mmax))
'                                seriesDigital3(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 19 And 1) * mmax))
'                                k += 1
'                            Next

'                            autosizey(plots(osc_ch - ch), mmax)
'                            plots(osc_ch - ch).Model.Series.Add(seriesAnalog(ch))
'                            plots(osc_ch - ch).Model.Series.Add(seriesDigital0(ch))
'                            plots(osc_ch - ch).Model.Series.Add(seriesDigital1(ch))
'                            plots(osc_ch - ch).Model.Series.Add(seriesDigital2(ch))
'                            plots(osc_ch - ch).Model.Series.Add(seriesDigital3(ch))
'                        End If
'                    Next
'                End If

'            Else
'                MainForm.plog.TextBox1.AppendText(vbCrLf & "Error data read!")
'            End If


'            ' Else
'            'MsgBox("Error on CONFIG_STATUS!", vbCritical + vbOKOnly)
'            'End If

'            For i = 0 To osc_ch
'                If title(i).Text = ChList_name(i) Then
'                    plots(i).Refresh()
'                    plots(i).InvalidatePlot(True)
'                End If
'            Next


'            If fileEnable = True Then
'                Using sw As IO.StreamWriter = File.AppendText(fileName)
'                    For k = 0 To osc_ch
'                        If EnabledChannel(k) Then
'                            sw.WriteLine(wavecount & ";" & k + 1 & ";" & "A" & ";" & String.Join(";", seriesAnalog(k).Points))
'                            sw.WriteLine(wavecount & ";" & k + 1 & ";" & "D0" & ";" & String.Join(";", seriesDigital0(k).Points))
'                            sw.WriteLine(wavecount & ";" & k + 1 & ";" & "D1" & ";" & String.Join(";", seriesDigital1(k).Points))
'                            sw.WriteLine(wavecount & ";" & k + 1 & ";" & "D2" & ";" & String.Join(";", seriesDigital2(k).Points))
'                            sw.WriteLine(wavecount & ";" & k + 1 & ";" & "D3" & ";" & String.Join(";", seriesDigital3(k).Points))
'                        End If
'                    Next
'                    wavecount += 1
'                    sw.WriteLine("")
'                End Using
'            End If
'        ElseIf Connection.ComClass._boardModel = communication.tModel.R5560 Then

'            For i = 0 To osc_ch
'                seriesAnalog(i).Points.Clear()
'                seriesAnalog2(i).Points.Clear()
'                seriesDigital0(i).Points.Clear()
'                seriesDigital1(i).Points.Clear()
'                seriesDigital2(i).Points.Clear()
'                plots(i).Model.Series.Clear()
'            Next

'            For o = 0 To Connection.ComClass._n_oscilloscope - 1
'                Dim status As UInt32 = 0
'                Dim tt = Now
'                While status <> 1
'                    Connection.ComClass.GetRegister(addressStatus(o), status)
'                    If (Now - tt).TotalMilliseconds > 2000 Then
'                        Exit Sub
'                    End If
'                End While

'                Dim position As UInt32
'                Connection.ComClass.GetRegister(addressPosition(o), position)

'                Dim lenght = nsamples '* (osc_ch + 1)
'                Dim data(lenght) As UInt32
'                Dim read_data As UInt32
'                Dim valid_data As UInt32
'                If Connection.ComClass.ReadData(addressData(0), data, lenght, 0, 1000, read_data, valid_data) = 0 Then

'                    Dim curr As Integer = position - Math.Floor(PreTriggerValue * nsamples / 100)
'                    If curr > 0 Then
'                        For ch = 0 To osc_ch
'                            If CheckedListBox1.GetItemCheckState(ch) Then
'                                Dim k = 0
'                                Dim mmax = 0
'                                For i = 0 To nsamples - 1
'                                    mmax = Math.Max(data(i + nsamples * ch) And &HFFFF, mmax)
'                                Next
'                                For i = curr To nsamples - 1
'                                    seriesAnalog(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, data(i + nsamples * ch) And 65535))
'                                    seriesDigital0(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 16 And 1) * mmax))
'                                    seriesDigital1(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 17 And 1) * mmax))
'                                    seriesDigital2(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 18 And 1) * mmax))
'                                    seriesDigital3(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 19 And 1) * mmax))
'                                    k += 1
'                                Next
'                                For i = 0 To curr - 1
'                                    seriesAnalog(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, data(i + nsamples * ch) And 65535))
'                                    seriesDigital0(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 16 And 1) * mmax))
'                                    seriesDigital1(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 17 And 1) * mmax))
'                                    seriesDigital2(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 18 And 1) * mmax))
'                                    seriesDigital3(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 19 And 1) * mmax))
'                                    k += 1
'                                Next

'                                autosizey(plots(osc_ch - ch), mmax)
'                                plots(osc_ch - ch).Model.Series.Add(seriesAnalog(ch))
'                                plots(osc_ch - ch).Model.Series.Add(seriesAnalog2(ch))
'                                plots(osc_ch - ch).Model.Series.Add(seriesDigital0(ch))
'                                plots(osc_ch - ch).Model.Series.Add(seriesDigital1(ch))
'                                plots(osc_ch - ch).Model.Series.Add(seriesDigital2(ch))
'                            End If
'                        Next
'                    Else
'                        For ch = 0 To osc_ch
'                            If CheckedListBox1.CheckedIndices.Contains(ch) Then
'                                Dim k = 0
'                                Dim mmax = 0
'                                For i = 0 To nsamples - 1
'                                    mmax = Math.Max(data(i + nsamples * ch) And &HFFFF, mmax)
'                                Next
'                                For i = nsamples + curr To nsamples - 1
'                                    seriesAnalog(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, data(i + nsamples * ch) And 65535))
'                                    seriesDigital0(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 16 And 1) * mmax))
'                                    seriesDigital1(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 17 And 1) * mmax))
'                                    seriesDigital2(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 18 And 1) * mmax))
'                                    seriesDigital3(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 19 And 1) * mmax))
'                                    k += 1
'                                Next
'                                For i = 0 To nsamples + curr - 1
'                                    seriesAnalog(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, data(i + nsamples * ch) And 65535))
'                                    seriesDigital0(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 16 And 1) * mmax))
'                                    seriesDigital1(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 17 And 1) * mmax))
'                                    seriesDigital2(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 18 And 1) * mmax))
'                                    seriesDigital3(ch).Points.Add(New DataPoint(k * (DecimatorValue) * 1000 / 80, (data(i + nsamples * ch) >> 19 And 1) * mmax))
'                                    k += 1
'                                Next

'                                autosizey(plots(osc_ch - ch), mmax)
'                                plots(osc_ch - ch).Model.Series.Add(seriesAnalog(ch))
'                                plots(osc_ch - ch).Model.Series.Add(seriesAnalog2(ch))
'                                plots(osc_ch - ch).Model.Series.Add(seriesDigital0(ch))
'                                plots(osc_ch - ch).Model.Series.Add(seriesDigital1(ch))
'                                plots(osc_ch - ch).Model.Series.Add(seriesDigital2(ch))
'                            End If
'                        Next
'                    End If

'                Else
'                    MainForm.plog.TextBox1.AppendText(vbCrLf & "Error data read!")
'                End If


'                ' Else
'                'MsgBox("Error on CONFIG_STATUS!", vbCritical + vbOKOnly)
'                'End If

'                'For i = 0 To osc_ch
'                If title(i).Text = ChList_name(i) Then
'                    plots(i).Refresh()
'                    plots(i).InvalidatePlot(True)
'                End If
'                'Next
'            Next

'            If fileEnable = True Then
'                Using sw As IO.StreamWriter = File.AppendText(fileName)
'                    For k = 0 To osc_ch
'                        If EnabledChannel(k) Then
'                            sw.WriteLine(wavecount & ";" & k + 1 & ";" & "A1" & ";" & String.Join(";", seriesAnalog(k).Points))
'                            sw.WriteLine(wavecount & ";" & k + 1 & ";" & "A2" & ";" & String.Join(";", seriesAnalog2(k).Points))
'                            sw.WriteLine(wavecount & ";" & k + 1 & ";" & "D0" & ";" & String.Join(";", seriesDigital0(k).Points))
'                            sw.WriteLine(wavecount & ";" & k + 1 & ";" & "D1" & ";" & String.Join(";", seriesDigital1(k).Points))
'                            sw.WriteLine(wavecount & ";" & k + 1 & ";" & "D2" & ";" & String.Join(";", seriesDigital2(k).Points))
'                        End If
'                    Next
'                    wavecount += 1
'                    sw.WriteLine("")
'                End Using
'            End If

'        End If

'    End Sub
'    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
'        SingleShotA()

'    End Sub

'    Private Sub pScope_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
'        Dim mwe As HandledMouseEventArgs = DirectCast(e, HandledMouseEventArgs)
'        mwe.Handled = True
'    End Sub
'End Class
