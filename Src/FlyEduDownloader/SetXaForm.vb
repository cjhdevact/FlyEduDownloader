'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：SetXaForm.vb
'描述：X-Nd-Auth 设置
'License：
'SmartEduDownloader
'Copyright (C) 2024 CJH.

'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.

'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License for more details.

'You should have received a copy of the GNU General Public License
'along with this program.  If not, see <https://www.gnu.org/licenses/>.
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
            Application.Exit()
            'End
        Else
            Me.Close()
        End If
    End Sub

    Private Sub SetXaForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyClass.FormClosing
        If Button4.Text = "退出" Then
            Application.Exit()
            'End
        End If
    End Sub

    Private Sub LinkLabel3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        If MainForm.DownloadMode = 1 Then
            System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/FlyEduDownloader/Help/index.html")
        Else
            System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/FlyEduDownloader/Help/login.html")
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If MessageBox.Show("免登录模式可能在未来会失效，所以建议仅你不想登录下载时才使用该模式，如果免登录模式无法下载，请使用登录模式下载。" & vbCrLf & "是否要使用免登录模式？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            System.Diagnostics.Process.Start(Application.ExecutablePath, "/unloginmode")
            Application.Exit()
            'End
        End If
    End Sub
End Class