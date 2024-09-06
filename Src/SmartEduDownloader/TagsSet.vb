'==========================================
'项目：SmartEduDownloader
'作者：CJH
'文件：TagsSet.vb
'描述：选择资源包里要下载的内容
'==========================================
Public Class TagsSet
    Public ec As Integer
    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        ec = 0
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        ec = 1
        Me.Close()
    End Sub

    Private Sub TagsSet_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ec = 0
    End Sub
End Class