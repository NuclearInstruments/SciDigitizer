
Public Class SciCompiler2019ExportClass
    Public Property Device As String
    Public Property Registers As Register()
    Public Property MMCComponents As MMCComponent2019()
End Class

Public Class MMCComponent2019

    Public Property Name As String
    Public Property Type As String
    Public Property Address As UInt32
    Public Property Version As String
    Public Property WordSize As UInt32
    Public Property nsamples As Integer
    Public Property bins As UInt32
    Public Property CountsBit As UInt32
    Public Property UseDMA As Boolean
    Public Property Registers As Register()
    Public Property Channels As Integer

End Class






