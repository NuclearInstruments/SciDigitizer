Public Class fit_win

    Dim is_editing = False

    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit

        If MainForm.acquisition.fit_list.Count < DataGridView1.Rows.Count Then
            Dim f As New AcquisitionClass.Fitting
            MainForm.acquisition.fit_list.Add(f)
        Else
            If DataGridView1.Columns(e.ColumnIndex).HeaderText = "Channel" Then
                MainForm.acquisition.fit_list(e.RowIndex).channel_number = DataGridView1.Rows(e.RowIndex).Cells("Channel").Value
            ElseIf DataGridView1.Columns(e.ColumnIndex).HeaderText = "Cursor 1" Then
                MainForm.acquisition.fit_list(e.RowIndex).cursor1 = CInt(DataGridView1.Rows(e.RowIndex).Cells("Cursor 1").Value)
            ElseIf DataGridView1.Columns(e.ColumnIndex).HeaderText = "Cursor 2" Then
                MainForm.acquisition.fit_list(e.RowIndex).cursor2 = CInt(DataGridView1.Rows(e.RowIndex).Cells("Cursor 2").Value)
            End If
        End If

    End Sub

    Private Sub DataGridView1_CellValidated(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValidated

        If DataGridView1.Columns(e.ColumnIndex).HeaderText = "Cursor 1" Or DataGridView1.Columns(e.ColumnIndex).HeaderText = "Cursor 2" Then
            If IsNumeric(DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
            Else
                DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0
            End If
        End If

    End Sub

    Private Sub DataGridView1_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles DataGridView1.RowsRemoved

        If is_editing = True Then
            If MainForm.acquisition.fit_list.Count > 0 Then
                MainForm.acquisition.fit_list.RemoveAt(e.RowIndex)
            End If
        End If

    End Sub

    Private Sub fit_win_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        is_editing = False
        DataGridView1.Columns.Clear()
        Dim ch As New DataGridViewComboBoxColumn()
        ch.HeaderText = "Channel"
        ch.Name = "Channel"
        If MainForm.spect.CheckedListBox1.CheckedIndices.Count <> 0 Then
            ch.MaxDropDownItems = MainForm.spect.CheckedListBox1.CheckedIndices.Count
            For Each i In MainForm.spect.CheckedListBox1.CheckedItems
                ch.Items.Add(i)
            Next
        Else
            ch.MaxDropDownItems = 1
        End If
        DataGridView1.Columns.Add(ch)
        DataGridView1.Columns.Add("Cursor 1", "Cursor 1")
        DataGridView1.Columns.Add("Cursor 2", "Cursor 2")
        DataGridView1.Columns.Add("mean", "Mean")
        DataGridView1.Columns.Add("fitmu", "Mean Fit")
        DataGridView1.Columns.Add("sigma", "STD")
        DataGridView1.Columns.Add("fitsigma", "STD Fit")
        DataGridView1.Columns.Add("fwhm", "FWHM")
        DataGridView1.Columns.Add("Resolution", "R")
        DataGridView1.Columns.Add("areaUpeak", "Area")
        DataGridView1.Columns.Add("areaFit", "Area fit")
        DataGridView1.Columns("mean").ReadOnly = True
        DataGridView1.Columns("fitmu").ReadOnly = True
        DataGridView1.Columns("sigma").ReadOnly = True
        DataGridView1.Columns("fitsigma").ReadOnly = True
        DataGridView1.Columns("fwhm").ReadOnly = True
        DataGridView1.Columns("Resolution").ReadOnly = True
        DataGridView1.Columns("areaUpeak").ReadOnly = True
        DataGridView1.Columns("areaFit").ReadOnly = True
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        DataGridView1.Rows.Clear()
        If MainForm.acquisition.fit_list.Count > 0 Then
            For fit_el = 0 To MainForm.acquisition.fit_list.Count - 1
                If MainForm.acquisition.fit_list(fit_el).channel_number <> Nothing Then
                    DataGridView1.Rows.Add()
                    Dim comboBoxCell As New DataGridViewComboBoxCell
                    comboBoxCell = (DataGridView1.Rows(fit_el).Cells("Channel"))
                    comboBoxCell.Value = MainForm.acquisition.fit_list(fit_el).channel_number
                    DataGridView1.Rows(fit_el).Cells("Cursor 1").Value = MainForm.acquisition.fit_list(fit_el).cursor1
                    DataGridView1.Rows(fit_el).Cells("Cursor 2").Value = MainForm.acquisition.fit_list(fit_el).cursor2
                End If
            Next
        End If
        is_editing = True

    End Sub

End Class
