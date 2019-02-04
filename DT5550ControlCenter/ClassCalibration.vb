Imports Newtonsoft.Json

Public Class ClassCalibration
    Public Class PointXY
        Public X As Double
        Public Y As Double
    End Class

    Public Class CalibCh
        Public points() As PointXY
        Public Property m As Double
        Public Property q As Double
        Public Sub New(_pointCh)
            ReDim points(_pointCh - 1)
            For i = 0 To _pointCh - 1
                points(i) = New PointXY
            Next
        End Sub
    End Class

    Public chCalibs() As CalibCh
    Public Sub New(_channel_count, point_per_channel)
        ReDim chCalibs(_channel_count - 1)
        For i = 0 To _channel_count - 1
            chCalibs(i) = New CalibCh(point_per_channel)
        Next
    End Sub

    Public Sub New()

    End Sub

    Public Sub New(JsL As String)
        Try
            LoadJSON(JsL)
        Catch ex As Exception

        End Try

    End Sub

    Public Function GetJSON() As String
        Return JsonConvert.SerializeObject(chCalibs)
    End Function

    Public Sub LoadJSON(json As String)
        chCalibs = JsonConvert.DeserializeObject(Of CalibCh())(json)
    End Sub

    Public Function GetCorrectionFactors(dac_value As Double) As Integer()
        Dim mean As Double = 0
        If chCalibs Is Nothing Then
            Dim coorr(128) As Integer
            For i = 0 To 128
                coorr(i) = 0
            Next
            Return coorr
        End If
        Dim returnVector(chCalibs.Count - 1) As Integer
        For i = 0 To chCalibs.Count - 1
            mean += chCalibs(i).m * dac_value + chCalibs(i).q
        Next
        mean = mean / chCalibs.Count

        For i = 0 To chCalibs.Count - 1
            returnVector(i) = Math.Round(mean - (chCalibs(i).m * dac_value + chCalibs(i).q))
        Next
        Return returnVector
    End Function
End Class
