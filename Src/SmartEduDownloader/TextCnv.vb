'==========================================
'项目：SmartEduDownloader
'作者：CJH
'文件：TextCnv.vb
'描述：文本合并
'==========================================
Public Class TextCnv

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        TextBox2.Text = Replace(TextBox1.Text, vbCrLf, "")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class