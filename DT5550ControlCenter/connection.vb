Imports System.ComponentModel
Imports System.Net

Public Class Connection

    Dim selected_board = communication.tModel.DT5550
    Dim selected_connection = communication.tConnectionMode.USB
    Public Shared ComClass As New communication()
    Public CustomFirmware As Boolean = False
    Public Jsonfile As String


    Private Sub Connection_Load(sender As Object, e As EventArgs) Handles Me.Load
        sW.Text = "Software version: " & Application.ProductVersion
        Me.Text = "SCI-55X0 Readout Software"
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


        Dim connect_column As New DataGridViewComboBoxColumn()
        Dim type_column As New DataGridViewComboBoxColumn()
        DataGridView1.Columns.Clear()
        connect_column.HeaderText = "Connection"
        connect_column.Name = "ConnectionType"
        connect_column.MaxDropDownItems = 2
        connect_column.Items.Add("Ethernet")
        connect_column.Items.Add("USB")
        DataGridView1.Columns.Add(connect_column)

        type_column.HeaderText = "Board"
        type_column.Name = "BoardType"
        type_column.MaxDropDownItems = 2
        type_column.Items.Add("Single DAQ")
        type_column.Items.Add("Baseboard")
        DataGridView1.Columns.Add(type_column)
        DataGridView1.Columns.Add("IP", "IP Address")
        DataGridView1.Columns.Add("Status", "Status")

        Dim connect_column2 As New DataGridViewComboBoxColumn()
        DataGridView2.Columns.Clear()
        connect_column2.HeaderText = "Connection"
        connect_column2.Name = "ConnectionType"
        connect_column2.MaxDropDownItems = 2
        connect_column2.Items.Add("Ethernet")
        connect_column2.Items.Add("USB")
        DataGridView2.Columns.Add(connect_column2)

        DataGridView2.Columns.Add("IP", "IP Address")
        DataGridView2.Columns.Add("Status", "Status")

    End Sub
    'Private Sub DataGridView1_EditingControlShowing(ByVal sender As System.Object, ByVal e As DataGridViewEditingControlShowingEventArgs) Handles DataGridView1.EditingControlShowing

    '    Dim editingComboBox As ComboBox = e.Control
    '    Dim i = DataGridView1.CurrentRow.Index
    '    Dim j = DataGridView1.CurrentCell.ColumnIndex

    '    If j = 0 Then
    '        If editingComboBox.SelectedText = "USB" Or DataGridView1.Rows(i).Cells("BoardType").Value = "Baseboard" Then
    '            Connect_R5560.Enabled = False
    '        ElseIf editingComboBox.SelectedText = "Ethernet" And DataGridView1.Rows(i).Cells("BoardType").Value = "Single DAQ" Then
    '            Connect_R5560.Enabled = True
    '        End If

    '    End If

    '    If j = 1 Then
    '        If DataGridView1.Rows(i).Cells("ConnectionType").Value = "USB" Or editingComboBox.SelectedText = "Baseboard" Then
    '            Connect_R5560.Enabled = False
    '        ElseIf DataGridView1.Rows(i).Cells("ConnectionType").Value = "Ethernet" And editingComboBox.SelectedText = "Single DAQ" Then
    '            Connect_R5560.Enabled = True
    '        End If

    '    End If
    '    'If editingComboBox IsNot Nothing Then
    '    '    Select Case Me.DataGridView1.CurrentCellAddress.X
    '    '        Case 1
    '    '            AddHandler editingComboBox.SelectedIndexChanged, AddressOf ComboBox1_SelectedIndexChanged
    '    '        Case 2
    '    '            AddHandler editingComboBox.SelectedIndexChanged, AddressOf ComboBox2_SelectedIndexChanged
    '    '    End Select
    '    'End If

    'End Sub
    'Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)


    '    Dim i = DataGridView1.CurrentRow.Index

    '    If DataGridView1.Rows(i).Cells("ConnectionType").Value = "USB" Or DataGridView1.Rows(i).Cells("BoardType").Value = "Baseboard" Then
    '        Connect_R5560.Enabled = False
    '    ElseIf DataGridView1.Rows(i).Cells("ConnectionType").Value = "Ethernet" And DataGridView1.Rows(i).Cells("BoardType").Value = "Single DAQ" Then
    '        Connect_R5560.Enabled = True
    '    End If
    'End Sub

    'Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim i = DataGridView1.CurrentRow.Index

    '    If DataGridView1.Rows(i).Cells("ConnectionType").Value = "USB" Or DataGridView1.Rows(i).Cells("BoardType").Value = "Baseboard" Then
    '        Connect_R5560.Enabled = False
    '    ElseIf DataGridView1.Rows(i).Cells("ConnectionType").Value = "Ethernet" And DataGridView1.Rows(i).Cells("BoardType").Value = "Single DAQ" Then
    '        Connect_R5560.Enabled = True
    '    End If
    'End Sub

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
        selected_board = communication.tModel.DT5550
        ComClass._nBoard = 1
        ComClass._n_ch = 32
        ComClass._n_ch_oscilloscope = 32
        ComClass._n_oscilloscope = 1
        ComClass.StartConnection(1, selected_board)

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
        r = ComClass.Connect(selected_connection, selected_board, parameter, 0)
        If CustomFirmware Then
            My.Settings.JsnFile = JsonFilePath.Text
            Jsonfile = JsonFilePath.Text
        Else
            Jsonfile = My.Application.Info.DirectoryPath & "\RegisterFileDT5550.json"
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

    Public Sub CreateDeviceListSciDK()

        DeviceListSciDK.Items.Clear()
        Dim listofdev As String = ""
        Dim countdev As New Integer

        ComClass.ListDevices(communication.tConnectionMode.USB, communication.tModel.SCIDK, listofdev, countdev)
        If listofdev <> "" Then

            Dim dev_sn = listofdev.Split(";")

            For i = 0 To countdev - 1
                DeviceListSciDK.Items.Add(dev_sn(i))
            Next
            DeviceListSciDK.SelectedIndex = 0
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


    Private Sub Connect_R5560_Click(sender As Object, e As EventArgs) Handles Connect_R5560.Click
        If DataGridView1.RowCount > 0 Then
            selected_board = communication.tModel.R5560
            selected_connection = communication.tConnectionMode.ETHERNET2
        Connect_R5560.Enabled = False
        ComClass.StartConnection(DataGridView1.RowCount, selected_board)
            Dim _connected_board = 0
            For d = 0 To DataGridView1.RowCount - 1
                If DataGridView1.Rows(d).Cells("ConnectionType").Value = "USB" Then
                    MsgBox("USB Connection not supported yet")
                    Exit For
                End If
                If DataGridView1.Rows(d).Cells("BoardType").Value = "Baseboard" Then
                    MsgBox("Baseboard Connection not supported yet")
                    Exit For
                End If
                Dim a As String() = DataGridView1.Rows(d).Cells("IP").Value.split(".")
                If a.Length <> 4 Then
                    MsgBox("IP Not Valid")
                    Exit For
                End If
                For i = 0 To 3
                    If a(i) < 0 Or a(i) > 255 Then
                        MsgBox("IP Not Valid")
                        Exit For
                    End If
                Next

                Dim r As New communication.tError
                r = ComClass.Connect(selected_connection, selected_board, DataGridView1.Rows(d).Cells("IP").Value, d)
                If r = communication.tError.OK Then
                    _connected_board += 1
                    DataGridView1.Rows(d).Cells("Status").Value = "OK"
                Else
                    DataGridView1.Rows(d).Cells("Status").Value = "ERROR"
                    ComClass.GetMessage(r)
                End If
            Next
            Connect_R5560.Enabled = True

            Jsonfile = My.Application.Info.DirectoryPath & "\RegisterFileR5560.json"
            My.Settings.IP1 = DataGridView1.Rows(0).Cells("IP").Value
            My.Settings.Save()

            If _connected_board = DataGridView1.RowCount And _connected_board <> 0 Then
                ComClass._nBoard = _connected_board
                ComClass._n_ch = 32
                ComClass._n_ch_oscilloscope = 2
                ComClass._n_oscilloscope = 32
                MainForm.Show()
                Me.Hide()
            End If
        Else
            MsgBox("Please add a device!")
        End If
    End Sub

    Private Sub Connect_DT5560SE_Click(sender As Object, e As EventArgs) Handles Connect_DT5560SE.Click
        If DataGridView2.RowCount > 0 Then

            selected_board = communication.tModel.DT5560SE
            selected_connection = communication.tConnectionMode.ETHERNET2
            Connect_DT5560SE.Enabled = False

            ComClass.StartConnection(DataGridView2.RowCount, selected_board)
            Dim _connected_board = 0

            For d = 0 To DataGridView2.RowCount - 1
                'If DataGridView2.Rows(d).Cells("ConnectionType").Value = "USB" Then
                '    MsgBox("USB Connection not supported yet")
                '    Exit For
                'End If
                Dim a As String() = DataGridView2.Rows(d).Cells("IP").Value.split(".")
                If a.Length <> 4 Then
                    MsgBox("IP Not Valid")
                    Exit For
                End If
                For i = 0 To 3
                    If a(i) < 0 Or a(i) > 255 Then
                        MsgBox("IP Not Valid")
                        Exit For
                    End If
                Next

                Dim r As New communication.tError
                r = ComClass.Connect(selected_connection, selected_board, DataGridView2.Rows(d).Cells("IP").Value, d)
                If r = communication.tError.OK Then
                    _connected_board += 1
                    DataGridView2.Rows(d).Cells("Status").Value = "OK"
                Else
                    DataGridView2.Rows(d).Cells("Status").Value = "ERROR"
                    ComClass.GetMessage(r)
                End If
            Next
            Connect_DT5560SE.Enabled = True

            Jsonfile = My.Application.Info.DirectoryPath & "\RegisterFileDT5560SE.json"
            My.Settings.IP1 = DataGridView2.Rows(0).Cells("IP").Value
            My.Settings.Save()

            If _connected_board = DataGridView2.RowCount And _connected_board <> 0 Then
                ComClass._nBoard = _connected_board
                ComClass._n_ch = 32
                ComClass._n_ch_oscilloscope = 2
                ComClass._n_oscilloscope = 32
                MainForm.Show()
                Me.Hide()
            End If
        Else
            MsgBox("Please add a device!")
        End If
    End Sub

    Private Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click
        Dim nRow = DataGridView1.RowCount
        DataGridView1.Rows.Add()
        DataGridView1.Rows(nRow).Cells("ConnectionType").Value = "Ethernet"
        DataGridView1.Rows(nRow).Cells("BoardType").Value = "Single DAQ"
        If nRow = 0 Then
            DataGridView1.Rows(nRow).Cells("IP").Value = My.Settings.IP1
        ElseIf nRow > 0 Then
            DataGridView1.Rows(nRow).Cells("IP").Value = DataGridView1.Rows(nRow - 1).Cells("IP").Value
        End If

    End Sub

    Private Sub RemoveButton_Click(sender As Object, e As EventArgs) Handles RemoveButton.Click
        If DataGridView1.SelectedCells.Count <> 0 Then
            Dim nSel = DataGridView1.SelectedCells.Item(0)
            DataGridView1.Rows.RemoveAt(nSel.RowIndex)
        End If
    End Sub

    Private Sub AddDT5560SE_Click(sender As Object, e As EventArgs) Handles Add_DT5560SE.Click
        Dim nRow = DataGridView2.RowCount
        DataGridView2.Rows.Add()
        DataGridView2.Rows(nRow).Cells("ConnectionType").Value = "Ethernet"
        If nRow = 0 Then
            DataGridView2.Rows(nRow).Cells("IP").Value = My.Settings.IP1
        ElseIf nRow > 0 Then
            DataGridView2.Rows(nRow).Cells("IP").Value = DataGridView2.Rows(nRow - 1).Cells("IP").Value
        End If

    End Sub

    Private Sub RemoveDT5560SE_Click(sender As Object, e As EventArgs) Handles Remove_DT5560SE.Click
        If DataGridView2.SelectedCells.Count <> 0 Then
            Dim nSel = DataGridView2.SelectedCells.Item(0)
            DataGridView2.Rows.RemoveAt(nSel.RowIndex)
        End If
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedIndex = 2 Then
            CreateDeviceListSciDK()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        CreateDeviceListSciDK()
    End Sub

    Private Sub ConnectSciDK_Click(sender As Object, e As EventArgs) Handles ConnectSciDK.Click
        selected_board = communication.tModel.SCIDK
        ComClass._nBoard = 1
        ComClass._n_ch = 2
        ComClass._n_ch_oscilloscope = 2
        ComClass._n_oscilloscope = 1
        ComClass.StartConnection(1, selected_board)


        If DeviceListSciDK.Items.Count = 0 Then

            MsgBox("No devices connected. Please check cable connections", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
            Exit Sub
        End If


        Dim r As New communication.tError
        r = ComClass.Connect(selected_connection, selected_board, DeviceListSciDK.Text, 0)
        Jsonfile = My.Application.Info.DirectoryPath & "\RegisterFileSCIDK_MCA.json"

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



End Class