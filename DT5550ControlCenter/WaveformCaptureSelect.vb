Public Class WaveformCaptureSelect

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Me.DialogResult = DialogResult.Cancel
        Me.Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If FileName.Text <> "" Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MsgBox("Please set a file path.")
        End If

    End Sub

    Private Sub WaveformCaptureSelect_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        FileName.Enabled = False
        FileName.ReadOnly = True
        DataType.Items.Clear()
        DataType.Items.Add("Oscilloscope")
        DataType.Items.Add("Frame")
        DataType.SelectedIndex = 0
        For i = 0 To MainForm.acquisition.CHList.Count - 1
            ChList.Items.Add(MainForm.acquisition.CHList(i).name)
        Next
        For i = 0 To ChList.Items.Count - 1
            ChList.SetItemChecked(i, True)
        Next

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            FileName.Text = SaveFileDialog1.FileName
            FileName.Enabled = True
            FileName.ReadOnly = False
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        For i = 0 To ChList.Items.Count - 1
            ChList.SetItemChecked(i, True)
        Next

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        For i = 0 To ChList.Items.Count - 1
            ChList.SetItemChecked(i, False)
        Next

    End Sub

    Private Sub TargetMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TargetMode.SelectedIndexChanged

        If TargetMode.SelectedIndex = 0 Then
            Label3.Enabled = False
            TargetValue.Enabled = False
            TargetValueUnit.Enabled = False
        Else
            Label3.Enabled = True
            TargetValue.Enabled = True
            If TargetMode.SelectedIndex = 1 Then
                TargetValueUnit.Enabled = False
            Else
                TargetValueUnit.Enabled = True
                TargetValueUnit.Items.Clear()
                TargetValueUnit.Items.Add("s")
                TargetValueUnit.Items.Add("min")
                TargetValueUnit.Items.Add("h")
                TargetValueUnit.SelectedIndex = 0
            End If
        End If

    End Sub

    Private Sub DataType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DataType.SelectedIndexChanged

        If DataType.SelectedIndex = 0 Then
            MainForm.isSpectra = False
        Else
            MainForm.isSpectra = True
        End If
        TargetMode.Items.Clear()
        TargetMode.Items.Add("Free")
        TargetMode.Items.Add("Events")
        If MainForm.isSpectra Then
            TargetMode.Items.Add("Time")
        End If
        TargetMode.SelectedIndex = 0
    End Sub

End Class