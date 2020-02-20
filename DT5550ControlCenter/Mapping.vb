Public Class Mapping

    Dim CURRENTMAP(,) As String
    Dim ROWS As Integer = 2
    Dim COLS As Integer = 16
    Dim BLOCKSIE = 55
    Dim ptb As New List(Of Label)
    Dim button1 As New Button
    Dim lab1 As New Label
    Dim lab2 As New Label
    Dim numeric_col As New NumericUpDown
    Dim numeric_row As New NumericUpDown
    Dim inhibit = True



    Private Sub Mapping_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
            MainForm.acquisition.currentMAP.cols = COLS
            MainForm.acquisition.currentMAP.rows = ROWS

            AddHandler numeric_col.ValueChanged, AddressOf COL_VALUE_CHANGE
            AddHandler numeric_row.ValueChanged, AddressOf ROW_VALUE_CHANGE
            AddHandler button1.Click, AddressOf CLICK_EVENT_BUTTON
            inhibit = False
        End If
    End Sub

    Private Sub Mapping_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Connection.ComClass._boardModel = communication.tModel.DT5550 Then
            ResizeMaps(ROWS, COLS)
        End If
    End Sub

    Public Sub ResizeMaps(ROWS As Integer, COLS As Integer)
        ptb.Clear()
        ReDim CURRENTMAP(ROWS - 1, COLS - 1)
        For i = 0 To ROWS - 1
            For j = 0 To COLS - 1
                ' If i * COLS + j + 1 <= TOTALCHANNEL Then
                CURRENTMAP(i, j) = MainForm.acquisition.currentMAP.positions(i, j)
                ' Else
                '    CURRENTMAP(i, j) = "NC"
                'End If
            Next
        Next

        '    If BLOCKSIE * ROWS > Me.Width Then
        BLOCKSIE = Math.Min((Me.Height - 100) / ROWS, (Me.Width - 100) / COLS)
        ' End If

        ' If BLOCKSIE * COLS > Me.Height Then
        ' BLOCKSIE = (Me.Width - 100) / COLS
        ' End If
        Dim OFSTX = (Me.Width - BLOCKSIE * COLS) / 2
        Dim OFSTY = (Me.Height - BLOCKSIE * ROWS) / 2
        For i = 0 To ROWS - 1
            For j = 0 To COLS - 1
                Dim lst As New Label
                lst.AutoSize = False
                lst.Width = BLOCKSIE
                lst.Height = BLOCKSIE
                lst.Left = OFSTX + BLOCKSIE * j
                lst.Top = OFSTY + BLOCKSIE * i
                lst.TextAlign = ContentAlignment.MiddleCenter
                lst.BorderStyle = BorderStyle.FixedSingle
                AddHandler lst.Click, AddressOf CLICK_EVENT_BOX
                lst.Name = "PX_" & i & "_" & j
                If CURRENTMAP(i, j) = "NC" Then
                    lst.BackColor = Color.DarkRed

                Else

                    lst.BackColor = Color.Green
                End If

                lst.Text = CURRENTMAP(i, j)
                ptb.Add(lst)
            Next
        Next

        Me.Controls.Clear()

        button1.Text = "CONFIGURE"
        button1.Width = 80
        button1.Top = Me.Height - 25
        button1.Left = Me.Width - 100
        button1.Anchor = AnchorStyles.Right + AnchorStyles.Bottom

        lab1.Text = "N° Rows"
        lab2.Text = "N° Columns"
        lab1.Top = 5
        lab2.Top = 5
        lab1.Left = 5
        lab2.Left = 150
        lab1.Width = 60
        lab2.Width = 80
        lab1.Anchor = AnchorStyles.Left + AnchorStyles.Top
        lab2.Anchor = AnchorStyles.Left + AnchorStyles.Top

        numeric_col.Minimum = 0
        numeric_row.Minimum = 0
        numeric_col.Maximum = 32
        numeric_row.Maximum = 32
        numeric_row.Value = ROWS
        numeric_col.Value = COLS
        numeric_col.Top = 5
        numeric_row.Top = 5
        numeric_col.Left = 230
        numeric_row.Left = 70
        numeric_col.Width = 50
        numeric_row.Width = 50



        Me.Controls.Add(lab1)
        Me.Controls.Add(lab2)
        Me.Controls.Add(numeric_col)
        Me.Controls.Add(numeric_row)
        Me.Controls.Add(button1)


        For Each LST In ptb
            Me.Controls.Add(LST)
        Next
    End Sub


    Public Sub CLICK_EVENT_BOX(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim SENDERt As Label = sender
        Dim n As New ChooseCh
        '  n.Show()
        If n.ShowDialog = DialogResult.OK Then
            SENDERt.Text = n.ch_chosen
            Dim NARRAY = SENDERt.Name.Split("_")
            CURRENTMAP(NARRAY(1), NARRAY(2)) = n.ch_chosen
            If SENDERt.Text = "NC" Then
                SENDERt.BackColor = Color.DarkRed
            Else
                SENDERt.BackColor = Color.Green
            End If

        End If


    End Sub


    'Public Sub checkCh()
    '    Dim l(ROWS * COLS - 1) As String
    '    ' ReDim l(ROWS * COLS - 1)
    '    Dim k = 0
    '    For i = 0 To ROWS - 1
    '        For j = 0 To COLS - 1
    '            If l.Contains(CURRENTMAP(i, j)) And CURRENTMAP(i, j) <> "NC" Then
    '                MsgBox("The channel " & CURRENTMAP(i, j) & " has been added multiple times.")

    '                Exit Sub
    '            Else
    '                l(k) = CURRENTMAP(i, j)
    '                k += 1
    '            End If
    '        Next
    '    Next

    'End Sub

    'Public Sub CLICK_EVENT_BUTTON(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim ch_to_remove As New List(Of Integer)

    '    Dim l(ROWS * COLS - 1) As String
    '    Dim k = -1
    '    Dim n As New Integer
    '    Dim j As New Integer

    '    For n = 0 To ROWS - 1
    '        For j = 0 To COLS - 1
    '            If l.Contains(CURRENTMAP(n, j)) And CURRENTMAP(n, j) <> "NC" Then
    '                MsgBox("Channel " & CURRENTMAP(n, j) & " has been added multiple times.")
    '                Exit Sub
    '            Else
    '                k += 1
    '                l(k) = CURRENTMAP(n, j)

    '            End If
    '        Next
    '    Next



    '    For i = 0 To MainForm.oscilloscope.CHList.Count - 1
    '            If l.Contains(MainForm.oscilloscope.CHList(i).id) Then
    '            Dim index As Integer = Array.IndexOf(l, MainForm.oscilloscope.CHList(i).id.ToString)
    '            Dim x = 0
    '                Dim y = 0
    '            MainForm.oscilloscope.FindPosition(COLS, index + 1, x, y)
    '            MainForm.oscilloscope.CHList(i).name = "CHANNEL " + (index + 1).ToString
    '            MainForm.oscilloscope.CHList(i).id = index + 1
    '            MainForm.oscilloscope.CHList(i).x_position = x
    '            MainForm.oscilloscope.CHList(i).y_position = y
    '            Else
    '                ch_to_remove.Add(i)
    '            End If
    '        Next
    '        For Each ch In ch_to_remove
    '            MainForm.oscilloscope.CHList.Remove(MainForm.oscilloscope.CHList(ch))
    '        Next

    '    For t = 0 To l.Count - 1
    '        If l(t) <> "NC" Then
    '            Dim addch = True
    '            For ch = 0 To MainForm.oscilloscope.CHList.Count - 1
    '                If MainForm.oscilloscope.CHList(ch).id = l(t) Then
    '                    addch = False
    '                    Exit For
    '                Else
    '                End If
    '            Next
    '            If addch = True Then
    '                Dim x = 0
    '                Dim y = 0
    '                MainForm.oscilloscope.FindPosition(COLS, t + 1, x, y)
    '                MainForm.oscilloscope.CHList.Add(New OscilloscopeClass.Channel("CHANNEL " + l(t).ToString, l(t), x, y))
    '            End If
    '        End If
    '    Next
    '    MainForm.stat.Statistics_ReLoad(MainForm.oscilloscope.CHList.Count)
    '    MainForm.sets.Grid_ReLoad(MainForm.oscilloscope.CHList.Count)
    '    MainForm.pRT3.pScope_ReLoad(MainForm.oscilloscope.CHList.Count)
    '    MainForm.pRT.pImmediate_ReLoad(ROWS, COLS)
    '    MainForm.pRT2.pImmediate_ReLoad(ROWS, COLS)
    'End Sub

    Public Sub CLICK_EVENT_BUTTON(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If inhibit = False Then

            Dim l(ROWS * COLS - 1) As String
            Dim k = -1
            Dim n As New Integer
            Dim j As New Integer

            For n = 0 To ROWS - 1
                For j = 0 To COLS - 1
                    If l.Contains(CURRENTMAP(n, j)) And CURRENTMAP(n, j) <> "NC" Then
                        MsgBox("Channel " & CURRENTMAP(n, j) & " has been added multiple times.")
                        Exit Sub
                    Else
                        k += 1
                        l(k) = CURRENTMAP(n, j)
                    End If
                Next
            Next

            'Dim tmpchlist As New List(Of AcquisitionClass.Channel)
            'For Each ch In MainForm.acquisition.CHList
            '    tmpchlist.Add(ch)
            'Next

            'Dim tmpchid(MainForm.acquisition.CHList.Count - 1) As String
            MainForm.acquisition.CHList.Clear()
            'For p = 0 To tmpchlist.Count - 1
            '    tmpchid(p) = tmpchlist(p).id.ToString
            'Next
            'For i = 0 To l.Count - 1
            '    Dim x = 0
            '    Dim y = 0
            '    MainForm.acquisition.FindPosition(COLS, i + 1, x, y)
            '    If l(i) <> "NC" Then
            '        If tmpchid.Contains(l(i)) Then
            '            Dim index As Integer = Array.IndexOf(tmpchid, l(i))
            '            tmpchlist(index).x_position = x
            '            tmpchlist(index).y_position = y
            '            MainForm.acquisition.CHList.Add(tmpchlist(index))
            '        Else
            '            MainForm.acquisition.CHList.Add(New AcquisitionClass.Channel("CHANNEL " + (i + 1).ToString, l(i), x, y))
            '        End If
            '    End If
            'Next

            For i = 0 To l.Count - 1
                Dim x = 0
                Dim y = 0
                MainForm.acquisition.FindPosition(COLS, i + 1, x, y)
                If l(i) <> "NC" Then
                    MainForm.acquisition.CHList.Add(New AcquisitionClass.Channel("CHANNEL " + (i + 1).ToString, l(i), i + 1, x, y, communication.tModel.DT5550, 1))
                End If
            Next

            ReDim MainForm.acquisition.currentMAP.list(ROWS * COLS - 1)
            MainForm.acquisition.currentMAP.list = l

            ReDim MainForm.acquisition.currentMAP.positions(ROWS, COLS)
            MainForm.acquisition.currentMAP.positions = CURRENTMAP

            MainForm.sets.Grid_ReLoad()
            MainForm.scope.pScope_ReLoad()
            MainForm.pImm1.pImmediate_ReLoad(ROWS, COLS)
            MainForm.pImm2.pImmediate_ReLoad(ROWS, COLS)
            MainForm.spect.reload()
            MainForm.plog.TextBox1.AppendText(vbCrLf & "Channel Mapping Configured!")
        End If

    End Sub

    Public Sub COL_VALUE_CHANGE(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If inhibit = False Then

            Dim sender_t As NumericUpDown = sender
            If sender_t.Value > 32 Then
                sender_t.Value = 32
            End If
            If sender_t.Value < 1 Then
                sender_t.Value = 1
            End If
            COLS = sender_t.Value
            ChangeCols(COLS)
            ResizeMaps(ROWS, COLS)
        End If

    End Sub

    Public Sub ChangeCols(c As Integer)
        Dim tmpsize = MainForm.acquisition.currentMAP.cols * MainForm.acquisition.currentMAP.rows
        Dim newsize = c * ROWS
        Array.Resize(MainForm.acquisition.currentMAP.list, newsize)
        For i = tmpsize To newsize - 1
            MainForm.acquisition.currentMAP.list(i) = "NC"
        Next
        MainForm.acquisition.currentMAP.cols = c
        ReDim MainForm.acquisition.currentMAP.positions(ROWS, c)
        Dim k = 0
        For el = 0 To newsize - 1
            Dim x = 0
            Dim y = 0
            MainForm.acquisition.FindPosition(c, el + 1, x, y)

            MainForm.acquisition.currentMAP.positions(y, x) = MainForm.acquisition.currentMAP.list(el)
            If MainForm.acquisition.currentMAP.list(el) <> "NC" Then
                MainForm.acquisition.CHList(k).x_position = x
                MainForm.acquisition.CHList(k).y_position = y
                k += 1
            End If
        Next
    End Sub
    Public Sub ChangeRows(r As Integer)
        Dim tmpsize = MainForm.acquisition.currentMAP.cols * MainForm.acquisition.currentMAP.rows
        Dim newsize = COLS * r
        Array.Resize(MainForm.acquisition.currentMAP.list, newsize)
        For i = tmpsize To newsize - 1
            MainForm.acquisition.currentMAP.list(i) = "NC"
        Next

        ReDim MainForm.acquisition.currentMAP.positions(r, COLS)
        'For il = 0 To MainForm.oscilloscope.currentMAP.rows - 1
        '    For c = 0 To COLS - 1
        '        MainForm.oscilloscope.currentMAP.positions(il, c) = CURRENTMAP(il, c)
        '    Next
        'Next
        'For el = MainForm.oscilloscope.currentMAP.rows To r - 1
        '    For c = 0 To COLS - 1
        '        MainForm.oscilloscope.currentMAP.positions(el, c) = "NC"
        '    Next
        'Next
        MainForm.acquisition.currentMAP.rows = r
        For el = 0 To newsize - 1
            Dim x = 0
            Dim y = 0
            MainForm.acquisition.FindPosition(COLS, el + 1, x, y)
            ' If MainForm.oscilloscope.currentMAP.list(el) <> "NC" Then
            '  MainForm.oscilloscope.CHList(el).x_position = x
            ' MainForm.oscilloscope.CHList(el).y_position = y
            'End If
            MainForm.acquisition.currentMAP.positions(y, x) = MainForm.acquisition.currentMAP.list(el)
        Next
    End Sub

    Public Sub ROW_VALUE_CHANGE(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If inhibit = False Then

            Dim sender_t As NumericUpDown = sender
            If sender_t.Value > 32 Then
                sender_t.Value = 32
            End If
            If sender_t.Value < 1 Then
                sender_t.Value = 1
            End If
            ROWS = sender_t.Value
            ChangeRows(ROWS)
            ResizeMaps(ROWS, COLS)
        End If
    End Sub

End Class
