'Public Class RunStart
'    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles pMRun.TextChanged

'    End Sub

'    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles pRun.TextChanged

'    End Sub

'    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles pBoard.TextChanged

'    End Sub


'    Private Sub RunStart_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        cTargetMode.Items.Clear()
'        cTargetMode.Items.Add("Free running")
'        cTargetMode.Items.Add("Counts")
'        cTargetMode.Items.Add("Time (s)")
'        cTargetMode.SelectedIndex = 0
'        pRun.Text = My.Settings.parRun + 1
'        pMRun.Text = My.Settings.parAcq + 1
'        pTemp.Text = "34.2 °C"
'        pBias.Text = "53.2 V"
'        pBoard.Text = 1
'        pAcq.Text = 0
'        pDT.Text = Now
'        TextBox1.Text = My.Settings.folder
'        Dim tmp As Integer
'        Dim DETECTOR_SN As String = GetEEPROMString(&H52, 0, 8)
'        Dim MODELLO_SENSORE As String = GetEEPROMString(&H52, 8, 16)
'        Dim SN_SENSORE As String = GetEEPROMString(&H52, 32, 16)
'        Dim SN_MANUF As String = GetEEPROMString(&H52, 64, 16)
'        Dim bias As Double = 0
'        Dim tmp32 As Integer
'        I2CEEpromRead(&H52, 128, tmp)
'        tmp32 = (tmp And &HFF) << 8
'        I2CEEpromRead(&H52, 129, tmp)
'        bias = (tmp32 + (tmp And &HFF)) / 10

'        Dim SN_AFE As String = GetEEPROMString(&H50, 0, 8)
'        Dim res As Double = 0
'        I2CEEpromRead(&H50, &H14, tmp)
'        res = tmp << 8
'        I2CEEpromRead(&H50, &H15, tmp)
'        res = (res + tmp)
'        Dim cap As Double = 0
'        I2CEEpromRead(&H50, &H16, tmp)
'        cap = tmp << 8
'        I2CEEpromRead(&H50, &H17, tmp)
'        cap = (cap + tmp)

'        pBoard.Text = DETECTOR_SN
'        TextBox2.Text = MODELLO_SENSORE
'        TextBox3.Text = SN_SENSORE
'        TextBox4.Text = bias & " V"
'        pAcq.Text = SN_AFE
'        TextBox5.Text = res & "R - " & cap & " pF"

'        Dim temp As Double
'        I2CTempRead(&H48, temp)
'        pTemp.Text = temp
'        I2CTempRead(&H4A, temp)
'        pTemp2.Text = temp

'        'If MainForm.pprop.bBias.Checked Then
'        '    pBias.Text = MainForm.pprop.vBias.Value
'        'Else
'        '    pBias.Text = "0"
'        'End If

'        TextBox1.Text = MainForm.SaveFolderDefault
'        Me.StartPosition = FormStartPosition.CenterScreen
'        Me.Height = 700
'    End Sub

'    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
'        My.Settings.parRun = pRun.Text
'        My.Settings.parAcq = pMRun.Text
'        MainForm.SaveFolderDefault = TextBox1.Text
'        My.Settings.folderpos = MainForm.SaveFolderDefault

'        My.Settings.Save()
'        MainForm.board1.FPGAWriteReg(1, &H10)
'        MainForm.board1.FPGAWriteReg(0, &H10)
'        If MainForm.nboard > 1 Then
'            MainForm.board2.FPGAWriteReg(1, &H10)
'            MainForm.board2.FPGAWriteReg(0, &H10)
'        End If
'        ' MainForm.rdc.ScaleFactor = 4 'MainForm.pprop.iTime.Value / MainForm.Ts
'        Dim header As String
'        Dim build_date As Date = Date.FromOADate(Val(My.Application.Info.Version.Build) + 36526)

'        header = "<XML_NUCLEAR_INSTRUMENTS_MADA>" & vbCrLf
'        header = header & vbTab & "<SYSTEM_INFO>" & vbCrLf
'        header = header & vbTab & vbTab & "<APPLICATION_NAME>Nuclear Instruments MADA Readout System For SiPM</APPLICATION_NAME>" & vbCrLf
'        header = header & vbTab & vbTab & "<COMPANY>Nuclear Instruments SRL, Lambrugo COMO, IT-22045</COMPANY>" & vbCrLf
'        header = header & vbTab & vbTab & "<COMPANY_WEBSITE>http://www.nuclearinstruments.eu</COMPANY_WEBSITE>" & vbCrLf
'        header = header & vbTab & vbTab & "<APPLICATION_VERSION>" & Application.ProductVersion & "</APPLICATION_VERSION>" & vbCrLf
'        header = header & vbTab & vbTab & "<BUILD_DATE>" & build_date.ToString("dd-MM-yy") & "</BUILD_DATE>"
'        header = header & vbTab & "</SYSTEM_INFO>" & vbCrLf
'        header = header & vbTab & "<ACQUISITION_INFO>" & vbCrLf
'        header = header & vbTab & vbTab & "<RUN_INFO>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<RUN_ID>" & My.Settings.parRun & "</RUN_ID>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<MACHINE_RUN_ID>" & My.Settings.parAcq & "</MACHINE_RUN_ID>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<DATETIME>" & pDT.Text & "</DATETIME>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<TEMPERATURE>" & pTemp.Text & "</TEMPERATURE>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<TEMPERATURE2>" & pTemp2.Text & "</TEMPERATURE2>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<BIAS>" & pBias.Text & "</BIAS>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<DETECTOR_BOARD_SN>" & pBoard.Text & "</DETECTOR_BOARD_SN>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<DETECTOR_MODEL>" & TextBox2.Text & "</DETECTOR_MODEL>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<DETECTOR_SN>" & TextBox3.Text & "</DETECTOR_SN>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<DETECTOR_VOP>" & TextBox4.Text & "</DETECTOR_VOP>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<ACQUISITION_BOARD_SN>" & pAcq.Text & "</ACQUISITION_BOARD_SN>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<ACQUISITION_BOARD_RC>" & TextBox5.Text & "</ACQUISITION_BOARD_RC>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<BOARD_COUNT>" & MainForm.nboard & "</BOARD_COUNT>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<BOARD1_ID>" & MainForm.board1_id & "</BOARD1_ID>" & vbCrLf
'        header = header & vbTab & vbTab & vbTab & "<BOARD2_ID>" & MainForm.board2_id & "</BOARD2_ID>" & vbCrLf
'        header = header & vbTab & vbTab & "</RUN_INFO>" & vbCrLf
'        header = header & vbTab & vbTab & "<CONFIG_INFO>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<TRIGGER_MODE>" & MainForm.pprop.trMode.Text & "</TRIGGER_MODE>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<TRIGGER_LEVEL>" & MainForm.pprop.trLevel.Text & "</TRIGGER_LEVEL>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<SEPARATE_LEVEL_TRIGGER>" & MainForm.pprop.bSeparateTrigger.Checked & "</SEPARATE_LEVEL_TRIGGER>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<DATA_DELAY_1>" & MainForm.pprop.dDelay.Text & "</DATA_DELAY_1>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<TRIGGER_DELAY_1>" & MainForm.pprop.trDelay.Text & "</TRIGGER_DELAY_1>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<DATA_DELAY_2>" & MainForm.pprop.dDelay2.Text & "</DATA_DELAY_2>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<TRIGGER_DELAY_2>" & MainForm.pprop.trDelay2.Text & "</TRIGGER_DELAY_2>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<TRIGGER_HOLD>" & MainForm.pprop.trHold.Text & "</TRIGGER_HOLD>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<POLARITY>" & MainForm.pprop.Polarity.Text & "</POLARITY>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<PILEUP_REJECTOR>" & MainForm.pprop.prEn.Checked & "</PILEUP_REJECTOR>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<PILEUP_TIME>" & MainForm.pprop.prTime.Text + MainForm.pprop.iTime.Value & "</PILEUP_TIME>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<INTEGRATION_TIME>" & MainForm.pprop.iTime.Text & "</INTEGRATION_TIME>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<BASELINE_CORRECTION>" & MainForm.pprop.blCor.Text & "</BASELINE_CORRECTION>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<BASELINE_COSTANT>" & MainForm.pprop.blOffset.Text & "</BASELINE_COSTANT>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<NOISE_FILTER>" & MainForm.pprop.nf_b.Checked & "</NOISE_FILTER>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<DIGITAL_GAIN>" & MainForm.pprop.dGain.Value & "</DIGITAL_GAIN>" & vbCrLf
'        'header = header & vbTab & vbTab & vbTab & "<CORRELATED_BOARD>" & MainForm.pprop.cBoard.Checked & "</CORRELATED_BOARD>" & vbCrLf
'        'header = header & vbTab & vbTab & "</CONFIG_INFO>" & vbCrLf
'        'header = header & vbTab & vbTab & "<BIAS_INFO>" & vbCrLf
'        'For i = 0 To 63
'        '    header = header & vbTab & vbTab & vbTab & "<DET" & i & ">" & MainForm.pprop.nudBiasVar(i).Value & "</DET" & i & ">" & vbCrLf
'        'Next
'        'header = header & vbTab & vbTab & "</BIAS_INFO>" & vbCrLf

'        'header = header & vbTab & vbTab & "<GAIN_OFFSET_INFO>" & vbCrLf
'        'For i = 0 To 63
'        '    header = header & vbTab & vbTab & vbTab & "<OFFSET" & i & ">" & MainForm.pprop.nudOffset(i).Value & "</OFFSET" & i & ">" & vbCrLf
'        '    header = header & vbTab & vbTab & vbTab & "<GAIN" & i & ">" & MainForm.pprop.nudGain(i).Value & "</GAIN" & i & ">" & vbCrLf
'        'Next

'        'header = header & vbTab & vbTab & "</GAIN_OFFSET_INFO>" & vbCrLf
'        'header = header & vbTab & vbTab & "<SINGULAR_TH>" & vbCrLf
'        'For i = 0 To 63
'        '    header = header & vbTab & vbTab & vbTab & "<THRESHOLD" & i & ">" & MainForm.pprop.nThresholdTrigger(i).Value & "</THRESHOLD" & i & ">" & vbCrLf
'        'Next
'        'header = header & vbTab & vbTab & "</SINGULAR_TH>" & vbCrLf
'        'header = header & vbTab & "</ACQUISITION_INFO>" & vbCrLf
'        'header = header & vbTab & "<START_NOTE>" & vbCrLf
'        'header = header & vbTab & pNote.Text & vbCrLf
'        'header = header & vbTab & "</START_NOTE>" & vbCrLf
'        'MainForm.rdc.Header = header
'        'MainForm.rdc.StartAcquisition(MainForm.nboard)
'        'MainForm.rdc.StartDataCaptureOnFile(TextBox1.Text & "\RUN" & pRun.Text & ".txt")
'        'MainForm.rdc.SS_runid = pRun.Text
'        'MainForm.rdc.SyncMode = MainForm.pprop.psyncmode.SelectedIndex
'        'MainForm.rdc.cTargetMode = cTargetMode.SelectedIndex
'        'Try
'        '    MainForm.rdc.cTargetValue = Convert.ToInt32(cTargetValue.Tex
'        'Catch ex As Exception
'        '    MainForm.rdc.cTargetValue = 0
'        'End Try

'        Me.DialogResult = DialogResult.OK
'        Me.Close()
'    End Sub

'    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
'        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
'            TextBox1.Text = FolderBrowserDialog1.SelectedPath
'            My.Settings.folder = TextBox1.Text
'        End If
'    End Sub

'    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
'        Me.DialogResult = DialogResult.Abort
'        Me.Close()
'    End Sub

'    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

'    End Sub

'    Private Sub pAcq_TextChanged(sender As Object, e As EventArgs) Handles pAcq.TextChanged

'    End Sub

'    Private Sub cTargetMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cTargetMode.SelectedIndexChanged

'    End Sub
'End Class