'==========================================
'项目：SmartEduDownloader
'作者：CJH
'文件：TextSp.vb
'描述：文本分割
'==========================================
Public Class TextSp

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox3.Text = "" Then
            MessageBox.Show("必须指定一个分隔符。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        TextBox2.Text = ""
        Dim a() As String
        a = Split(TextBox1.Text, TextBox3.Text)
        For i = 0 To a.Count - 1
            If CheckBox1.Checked = True Then
                If i = a.Count - 1 Then
                    TextBox2.Text = TextBox2.Text & a(i)
                Else
                    TextBox2.Text = TextBox2.Text & a(i) & vbCrLf
                End If
            Else
                If i = a.Count - 1 Then
                    TextBox2.Text = TextBox2.Text & a(i)
                Else
                    TextBox2.Text = TextBox2.Text & a(i) & " "
                End If
            End If
        Next
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class