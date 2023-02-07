Imports OxyPlot
Imports OxyPlot.Axes
Imports OxyPlot.Series

Public Class pImmediate
    Dim immediateCumulative As Boolean
    Dim model As New PlotModel()
    Dim data(,) As Double

    Public Sub New(ic)
        immediateCumulative = ic
        InitializeComponent()
    End Sub

    Private Shared Function _formatter(d As Double) As String
        If d < 1000.0 Then
            Return [String].Format("{0}", d)
        ElseIf d >= 1000.0 AndAlso d < 1000000.0 Then
            Return [String].Format("{0}K", d / 1000.0)
        ElseIf d >= 1000000.0 AndAlso d < 1000000000.0 Then
            Return [String].Format("{0}M", d / 1000000.0)
        ElseIf d >= 1000000000.0 Then
            Return [String].Format("{0}B", d / 1000000000.0)
        Else
            Return [String].Format("{0}", d)
        End If
    End Function

    Private Sub pImmediate_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        pImmediate_ReLoad(MainForm.acquisition.currentMAP.rows, MainForm.acquisition.currentMAP.cols)
    End Sub

    Public Sub pImmediate_ReLoad(r As Integer, c As Integer)

        ReDim data(c - 1, r - 1)
        Dim xlabel(c - 1) As String
        Dim ylabel(r - 1) As String
        For i = 0 To c - 1
            If Connection.ComClass._boardModel = communication.tModel.DT5560SE Or Connection.ComClass._boardModel = communication.tModel.R5560SE Then
                xlabel(i) = (i).ToString
            Else
                xlabel(i) = (i + 1).ToString
            End If
        Next
        For j = 0 To r - 1
            If Connection.ComClass._boardModel = communication.tModel.DT5560SE Or Connection.ComClass._boardModel = communication.tModel.R5560SE Then
                ylabel(j) = (j).ToString
            Else
                ylabel(j) = (j + 1).ToString
            End If
        Next
        model.Axes.Clear()
        model.Series.Clear()
        model.Axes.Add(New CategoryAxis() With {
            .Position = AxisPosition.Bottom,
            .Key = "pxb",
            .ItemsSource = xlabel
        })
        model.Axes.Add(New CategoryAxis() With {
            .Position = AxisPosition.Left,
            .Key = "pxl",
        .ItemsSource = ylabel
        })
        model.Axes.Add(New LinearColorAxis() With {
         .Palette = OxyPalettes.Jet(200),
        .Key = "color"
         })
        'Dim rand = New Random()

        For x As Integer = 0 To c - 1
            For y As Integer = 0 To r - 1
                data(x, y) = 0
            Next
        Next
        ' neccessary to display the label
        Dim heatMapSeries
        If immediateCumulative = True Then
            Dim fsize = 0
            If r >= 2 And c >= 2 Then
                fsize = 0.05
            End If
            heatMapSeries = New HeatMapSeries() With {
            .X0 = 0,
            .X1 = c - 1,
            .Y0 = 0,
            .Y1 = r - 1,
            .XAxisKey = "pxb",
            .YAxisKey = "pxl",
            .RenderMethod = HeatMapRenderMethod.Rectangles,
            .LabelFontSize = fsize,
            .Data = data
        }
        Else
            heatMapSeries = New HeatMapSeries() With {
              .X0 = 0,
              .X1 = c - 1,
              .Y0 = 0,
              .Y1 = r - 1,
              .XAxisKey = "pxb",
              .YAxisKey = "pxl",
              .RenderMethod = HeatMapRenderMethod.Rectangles,
              .LabelFontSize = 0.0,
              .Data = data
          }
        End If

        model.Series.Add(heatMapSeries)

        plot1.Model = model
        '  model.ResetAllAxes()
        plot1.InvalidatePlot(True)


    End Sub

    Public Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            Exit Sub
        End If
        Timer1.Enabled = False
        Dim tempdata() As Double
        Dim scale As Double = 1
        Dim max As Double = 1
        Dim rescale As Boolean = True
        If immediateCumulative = True Then

            Dim tmp = MainForm.spect.realtimeimage

            Array.Resize(tmp, MainForm.acquisition.CHList.Count)
            tempdata = tmp
            rescale = False
            ' Dim q = 0

            For Each ch In MainForm.acquisition.CHList
                ' If q = ch.id - 1 Then
                data(ch.x_position, ch.y_position) = tempdata(ch.id - 1)
                '  q += 1
                '  End If
            Next
            If Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.DT5560SE Or Connection.ComClass._boardModel = communication.tModel.R5560SE Then
                MainForm.spect.ClearRealtime()
            End If

        Else
            Dim tmp() = MainForm.spect.integralimage
            Array.Resize(tmp, MainForm.acquisition.CHList.Count)
            tempdata = tmp
            rescale = True
            'Dim q = 0

            For Each ch In MainForm.acquisition.CHList
                '   If q = ch.id - 1 Then
                data(ch.x_position, ch.y_position) = tempdata(ch.id - 1)
                '      q += 1
                ' End If
            Next
        End If

        '  For q = 0 To MainForm.oscilloscope.CHList.Count - 1
        ' max = Math.Max(tempdata(q), max)
        'Next
        '
        'q = 0

        '   For i = 0 To MainForm.oscilloscope.currentMAP.cols - 1
        '  For j = 0 To MainForm.oscilloscope.currentMAP.rows - 1


        '   Next
        'Next
        plot1.InvalidatePlot(True)

        Timer1.Enabled = True

    End Sub

    Public Sub Reload_Image()
        If Connection.ComClass._boardModel = communication.tModel.SCIDK Then
            Exit Sub
        End If
        Dim tempdata() As Double
        Dim scale As Double = 1
        Dim max As Double = 1
        Dim rescale As Boolean = True
        If immediateCumulative = True Then

            Dim tmp = MainForm.spect.realtimeimage

            Array.Resize(tmp, MainForm.acquisition.CHList.Count)
            tempdata = tmp
            rescale = False
            ' Dim q = 0

            For Each ch In MainForm.acquisition.CHList
                ' If q = ch.id - 1 Then
                data(ch.x_position, ch.y_position) = tempdata(ch.id - 1)
                '  q += 1
                '  End If
            Next
            If Connection.ComClass._boardModel = communication.tModel.R5560 Or Connection.ComClass._boardModel = communication.tModel.DT5560SE Or Connection.ComClass._boardModel = communication.tModel.R5560SE Then
                MainForm.spect.ClearRealtime()
            End If

        Else
            Dim tmp() = MainForm.spect.integralimage
            Array.Resize(tmp, MainForm.acquisition.CHList.Count)
            tempdata = tmp
            rescale = True
            'Dim q = 0

            For Each ch In MainForm.acquisition.CHList
                '   If q = ch.id - 1 Then
                data(ch.x_position, ch.y_position) = tempdata(ch.id - 1)
                '      q += 1
                ' End If
            Next
        End If

        '  For q = 0 To MainForm.oscilloscope.CHList.Count - 1
        ' max = Math.Max(tempdata(q), max)
        'Next
        '
        'q = 0

        '   For i = 0 To MainForm.oscilloscope.currentMAP.cols - 1
        '  For j = 0 To MainForm.oscilloscope.currentMAP.rows - 1


        '   Next
        'Next
        plot1.InvalidatePlot(True)

    End Sub

    Private Sub plot1_Click(sender As Object, e As EventArgs) Handles plot1.Click

    End Sub
End Class
