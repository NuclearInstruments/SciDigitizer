'Imports OxyPlot

'Public Class DCR
'    Dim Model = New PlotModel()
'    Class DataSpe
'        Public Sub New(samples)
'            ReDim data(samples)
'        End Sub
'        Public data() As DataPoint
'    End Class

'    Dim datas(64) As DataSpe
'    Dim series(64) As OxyPlot.Series.LineSeries


'    Private Sub DCR_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        CheckedListBox1.Items.Clear()
'        For i = 1 To 64
'            CheckedListBox1.Items.Add(i)
'        Next

'        Model.Title = "DCR"
'        plot1.Refresh()



'        For i = 0 To 63
'            datas(i) = New DataSpe(10)


'            series(i) = New OxyPlot.Series.StairStepSeries() With {
'                .Title = "CH " & i + 1,
'                 .MarkerType = MarkerType.Circle,
'                    .ItemsSource = datas(i).data
'        }
'        Next
'    End Sub

'    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged

'        Model.Series.clear
'        For Each s In CheckedListBox1.CheckedItems
'            Model.Series.Add(series(s - 1))
'        Next

'        plot1.Model = Model

'        plot1.InvalidatePlot(True)
'        plot1.Refresh()
'    End Sub

'    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
'        Dim nsample = (ep.Value - sp.Value) / ss.Value

'        For i = 0 To 63
'            datas(i) = New DataSpe(nsample - 1)


'            series(i) = New OxyPlot.Series.StairStepSeries() With {
'                .Title = "CH " & i + 1,
'                 .MarkerType = MarkerType.Circle,
'                    .ItemsSource = datas(i).data
'        }
'        Next
'        Model.Series.clear
'        For Each s In CheckedListBox1.CheckedItems
'            Model.Series.Add(series(s - 1))
'        Next

'        For i = 0 To 63
'            'ReDim datas(i).data(nsample - 1)
'            For k = 0 To nsample - 1
'                datas(i).data(k) = New DataPoint(sp.Value + ss.Value * k, 0)
'            Next
'        Next
'        MainForm.board1.FPGAWriteReg(0, &H701)
'        If MainForm.nboard > 1 Then
'            MainForm.board2.FPGAWriteReg(0, &H701)
'        End If
'        MainForm.board1.FPGAWriteReg(0, &H600)

'        If MainForm.nboard > 1 Then
'            MainForm.board2.FPGAWriteReg(0, &H600)
'        End If
'        Dim rates1(32), rates2(32) As UInteger
'        Dim drates1(32), drates2(32) As Double
'        Dim orates1(32), orates2(32) As UInteger
'        For k = 0 To nsample - 1
'            ProgressBar1.Value = k / nsample * 100
'            'programma il trigger
'            For i = 0 To 31
'                MainForm.board1.FPGAWriteReg(sp.Value + ss.Value * k, &H450 + i)
'            Next

'            If MainForm.nboard > 1 Then
'                For i = 0 To 31
'                    MainForm.board2.FPGAWriteReg(sp.Value + ss.Value * k, &H450 + i)
'                Next
'            End If

'            System.Threading.Thread.Sleep(2000)


'            MainForm.board1.FPGAReadBurstUint(rates1, 32, &H0F000100&)
'            For i = 0 To 31
'                drates1(i) = ((rates1(i) / 150 * 100) + 1)
'                datas(i).data(k) = New DataPoint(Convert.ToDouble(sp.Value + ss.Value * k), Convert.ToDouble(drates1(i)))
'            Next
'            If MainForm.nboard > 1 Then
'                MainForm.board2.FPGAReadBurstUint(rates2, 32, &H0F000100&)
'                For i = 0 To 31
'                    drates2(i) = ((rates2(i) / 150 * 100) + 1)
'                    datas(i + 32).data(k) = New DataPoint(Convert.ToDouble(sp.Value + ss.Value * k), Convert.ToDouble(drates2(i)))
'                Next
'            End If



'            plot1.InvalidatePlot(True)

'            Application.DoEvents()
'        Next

'    End Sub
'End Class