Imports System.ComponentModel

Public Class Connection

    Dim selected_board = communication.tModel.DT5550
    Dim selected_connection = communication.tConnectionMode.USB
    Public Shared ComClass As New communication()
    Public CustomFirmware As Boolean = False
    Public Jsonfile As String

    Private Sub Connection_Load(sender As Object, e As EventArgs) Handles Me.Load
        sW.Text = "Software version: " & Application.ProductVersion
        Me.Text = "SCI-5550 Readout Software"
        Dim t As New communication.tError
        t = ComClass.Disconnect()
        If t = communication.tError.OK Or t = communication.tError.NOT_CONNECTED Or t = communication.tError.ALREADY_DISCONNECTED Then
        Else
            ComClass.GetMessage(t)
        End If

        Firmware_selection.Items.Clear()
        Firmware_selection.Items.Add("Standard")
        Firmware_selection.Items.Add("Custom")
        Firmware_selection.SelectedIndex = 0

        JsonFilePath.Text = ""

        Connection_selection.Items.Clear()
        Connection_selection.Items.Add("USB")
        '  Connection_selection.Items.Add("ETHERNET")
        ' Connection_selection.Items.Add("VME")
        Connection_selection.SelectedIndex = 0

        ' IP_SN.Text = My.Settings.SN

        LabelDeviceList.Enabled = True
        DeviceList.Enabled = True
        LabelIP.Enabled = False
        IP.Enabled = False
    End Sub

    Private Sub Browse_Click(sender As Object, e As EventArgs) Handles Browse.Click

        Dim OpenFileDialog1 As New OpenFileDialog()
        OpenFileDialog1.InitialDirectory = Application.StartupPath
        OpenFileDialog1.Filter = "Json (*.json)|*.json"
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            JsonFilePath.Text = OpenFileDialog1.FileName
        Else
            Exit Sub
        End If

    End Sub

    Private Sub Connect_Click(sender As Object, e As EventArgs) Handles Connect.Click

        If Connection_selection.SelectedIndex = 1 And IP.Text = "" Then
            MsgBox("Please insert the IP!", vbCritical + vbOKOnly)
            Exit Sub
        End If
        If CustomFirmware And JsonFilePath.Text = "" Then
            MsgBox("Please select a Json File!", vbCritical + vbOKOnly)
            Exit Sub
        End If
        Dim parameter As String = ""
        If selected_connection = communication.tConnectionMode.USB Then
            If DeviceList.Items.Count = 0 Then

                MsgBox("No devices connected. Please check cable connections", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
                Exit Sub
            End If
            parameter = DeviceList.SelectedItem.ToString
            If selected_connection = communication.tConnectionMode.ETHERNET Then
                parameter = IP.Text
            Else

            End If
        End If
        Dim r As New communication.tError
        r = ComClass.Connect(selected_connection, selected_board, parameter)
        If CustomFirmware Then
            My.Settings.JsnFile = JsonFilePath.Text
            Jsonfile = JsonFilePath.Text
        Else
            Jsonfile = My.Application.Info.DirectoryPath & "\RegisterFile.json"
        End If
        ' My.Settings.SN = IP.Text
        My.Settings.Save()

        If r = communication.tError.OK Then
            MainForm.Show()
            Me.Hide()
        ElseIf r = communication.tError.ALREADY_CONNECTED Then
            ComClass.GetMessage(r)
            Me.Hide()
        Else
            ComClass.GetMessage(r)
        End If

    End Sub

    Private Sub Connection_selection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Connection_selection.SelectedIndexChanged

        If Connection_selection.SelectedIndex = 0 Then
            selected_connection = communication.tConnectionMode.USB
            LabelDeviceList.Enabled = True
            DeviceList.Enabled = True
            LabelIP.Enabled = False
            IP.Enabled = False
            CreateDeviceList()
        ElseIf Connection_selection.SelectedIndex = 1 Then
            selected_connection = communication.tConnectionMode.ETHERNET
            LabelDeviceList.Enabled = False
            DeviceList.Enabled = False
            LabelIP.Enabled = True
            IP.Enabled = True
            CreateDeviceList()
        ElseIf Connection_selection.SelectedIndex = 2 Then
            selected_connection = communication.tConnectionMode.VME
            LabelDeviceList.Enabled = False
            DeviceList.Enabled = False
            LabelIP.Enabled = False
            IP.Enabled = False
            CreateDeviceList()
        End If

    End Sub

    Private Sub Connection_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        Me.Hide()

    End Sub

    Public Sub CreateDeviceList()

        DeviceList.Items.Clear()
        Dim listofdev As String = ""
        Dim countdev As New Integer

        ComClass.ListDevices(selected_connection, selected_board, listofdev, countdev)
        If listofdev <> "" Then

            Dim dev_sn = listofdev.Split(";")

            For i = 0 To countdev - 1
                DeviceList.Items.Add(dev_sn(i))
            Next
            DeviceList.SelectedIndex = 0
        End If
    End Sub

    Private Sub Firmware_selection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Firmware_selection.SelectedIndexChanged

        If Firmware_selection.SelectedIndex = 1 Then
            JsonFilePath.Enabled = True
            Label1.Enabled = True
            Browse.Enabled = True
            CustomFirmware = True
            JsonFilePath.Text = My.Settings.JsnFile
        Else
            JsonFilePath.Enabled = False
            Label1.Enabled = False
            Browse.Enabled = False
            CustomFirmware = False
            JsonFilePath.Text = ""
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CreateDeviceList()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://www.scicompiler.cloud")


    End Sub
End Class