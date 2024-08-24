Imports System.Net

Public Class WebClientPro
    Inherits WebClient
    Public Property Timeout As Integer
    'Public Sub New(Optional ByVal timeout As Integer = 10000)
    '     timeout = timeout
    ' End Sub

    Protected Overrides Function GetWebRequest(ByVal address As Uri) As WebRequest
        Dim request As HttpWebRequest = CType(MyBase.GetWebRequest(address), HttpWebRequest)
        request.Timeout = Timeout
        request.ReadWriteTimeout = Timeout
        Return request
    End Function
End Class
