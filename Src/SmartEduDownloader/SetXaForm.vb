'==========================================
'项目：SmartEduDownloader
'作者：CJH
'文件：SetXaForm.vb
'描述：X-Nd-Auth 设置
'==========================================
Public Class SetXaForm

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        System.Diagnostics.Process.Start("https://auth.smartedu.cn/uias/login")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        System.Diagnostics.Process.Start("https://basic.smartedu.cn/tchMaterial")
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If TextBox1.Text = "" Then
            MessageBox.Show("X-Nd-Auth值不能为空。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        MainForm.XNdAuth = TextBox1.Text
        Button4.Text = "取消"
        Button5.Visible = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Close()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If Button4.Text = "退出" Then
            End
        Else
            Me.Close()
        End If
    End Sub

    Private Sub SetXaForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyClass.FormClosing
        If Button4.Text = "退出" Then
            End
        End If
    End Sub

    Private Sub LinkLabel3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        If MainForm.DownloadMode = 1 Then
            System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/SmartEduDownloader/Help/index.html")
        Else
            System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/SmartEduDownloader/Help/login.html")
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        System.Diagnostics.Process.Start(Application.ExecutablePath)
        End
    End Sub
End Class