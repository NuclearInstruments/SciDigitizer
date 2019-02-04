Imports MathNet.Numerics

Public Class OffsetCalibration
    Public addressDecimator As UInt32
    Public addressPre As UInt32
    Public addressMode As UInt32
    Public addressLevel As UInt32
    Public addressArm As UInt32
    Public addressStatus As UInt32
    Public addressPosition As UInt32


    Public addressData As UInt32
    Dim nsamples As UInt32
    Dim tot_points As Integer
    Dim length As Integer
    Dim position As UInt32
    Dim osc_ch As Integer
    Const basedac = 2000
    Const baseStep = 500
    Const totStep = 5
    Private Sub OffsetCalibration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DGW1.Columns.Clear()
        DGW1.Columns.Add("CH", "CH")
        For run = 0 To totStep - 1
            DGW1.Columns.Add("L" & basedac + baseStep * run, basedac + baseStep * run)
        Next

        DGW1.Columns.Add("m", "m")
        DGW1.Columns.Add("q", "q")

        For i = 0 To 31
            DGW1.Rows.Add(i, 0, 0, 0, 0, 0, 0, 0, 0)
        Next

        addressData = MainForm.CurrentOscilloscope.Address
        nsamples = MainForm.CurrentOscilloscope.nsamples
        tot_points = 5 * nsamples
        osc_ch = MainForm.acquisition.CHList.Count
        length = nsamples * osc_ch
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click



        Dim AnalogArray(nsamples * 32) As Single
        Dim offsets(32) As Double


        For run = 0 To totStep - 1
            Dim no = basedac + baseStep * run
            Connection.ComClass.AFE_SetOffset(False, no)
            Connection.ComClass.AFE_SetOffset(True, no)
            System.Threading.Thread.Sleep(2000)
            Dim data(length) As UInt32
            Dim read_data As UInt32
            Dim valid_data As UInt32
            Dim status As UInt32 = 0
            Dim tt = Now
            For zzz = 0 To 3
                If Connection.ComClass.SetRegister(addressDecimator, 0) = 0 Then
                    If Connection.ComClass.SetRegister(addressPre, 1) = 0 Then
                        If Connection.ComClass.SetRegister(addressMode, Convert.ToInt32("1000" & MainForm.acquisition.General_settings.TriggerOscilloscopeEdges & "010", 2)) = 0 Then
                            If Connection.ComClass.SetRegister(addressLevel, 0) = 0 Then
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

                While status <> 1
                    Connection.ComClass.GetRegister(addressStatus, status)
                    If (Now - tt).TotalMilliseconds > 1000 Then
                        Exit Sub
                    End If
                End While

                Dim position As UInt32
                Connection.ComClass.GetRegister(addressPosition, position)
            Next

            If Connection.ComClass.ReadData(addressData, data, length, 0, 1000, read_data, valid_data) = 0 Then
                Dim curr As Integer = position - Math.Floor(1 * nsamples / 100)
                Dim n = 0

                For ch_id = 1 To 32
                    If ch_id <> 0 Then
                        Dim ch = ch_id - 1
                        If curr > 0 Then
                            Dim k = 0
                            For i = curr To nsamples - 2
                                AnalogArray(k + nsamples * n) = data(i + nsamples * ch) And 65535
                                k += 1
                            Next
                            For i = 0 To curr - 1
                                AnalogArray(k + nsamples * n) = data(i + nsamples * ch) And 65535
                                k += 1
                            Next
                        Else
                            Dim k = 0
                            For i = nsamples + curr To nsamples - 2
                                AnalogArray(k + nsamples * n) = data(i + nsamples * ch) And 65535
                                k += 1
                            Next
                            For i = 0 To nsamples + curr - 1
                                AnalogArray(k + nsamples * n) = data(i + nsamples * ch) And 65535
                                k += 1
                            Next
                        End If


                        n += 1
                    End If
                Next

                For ch = 0 To 31
                    offsets(ch) = 0
                    For s = 0 To nsamples - 1
                        offsets(ch) += AnalogArray(s + nsamples * ch)
                    Next
                    offsets(ch) /= nsamples
                    DGW1.Item(run + 1, ch).Value = Math.Round(offsets(ch))
                Next

                Application.DoEvents()
            End If
        Next

        For ch = 0 To 31
            Dim valuesX(totStep - 1) As Double
            Dim valuesY(totStep - 1) As Double
            For j = 0 To totStep - 1
                valuesX(j) = basedac + baseStep * j
                valuesY(j) = DGW1.Item(j + 1, ch).Value
            Next
            Dim fitres = Fit.Line(valuesx, valuesy)
            DGW1.Item("q", ch).Value = fitres.Item1
            DGW1.Item("m", ch).Value = fitres.Item2
        Next

        MsgBox("Calibration procedure completed. Remember to save settings", vbOKOnly + vbInformation)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim jSclass As New ClassCalibration(32, totStep)
        For ch = 0 To 31
            jSclass.chCalibs(ch).m = DGW1.Item("m", ch).Value
            jSclass.chCalibs(ch).q = DGW1.Item("q", ch).Value
            For j = 0 To totStep - 1
                jSclass.chCalibs(ch).points(j).X = basedac + baseStep * j
                jSclass.chCalibs(ch).points(j).Y = DGW1.Item(j + 1, ch).Value
            Next
        Next

        Dim jsOut = jSclass.GetJSON()

        My.Settings.AFECalibration = jsOut

        MsgBox("Calibration has been saved in application setitng. Apply configuration to transfer calibration on the board", vbOKOnly + vbInformation)
    End Sub
End Class