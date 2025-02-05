'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：LoginHelp.vb
'描述：手动登录帮助
'License：
'FlyEduDownloader
'Copyright (C) 2024-2025 CJH.

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
Public Class LoginHelp

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If TextBox1.Text = "" Then
            MainForm.MessageBoxError("X-Nd-Auth值不能为空。", "飞翔教学资源助手 - 错误", False)
            'MessageBox.Show("X-Nd-Auth值不能为空。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        SetXaForm.TempXa = Me.TextBox1.Text
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        System.Diagnostics.Process.Start("https://auth.smartedu.cn/uias/login")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        System.Diagnostics.Process.Start("https://basic.smartedu.cn/tchMaterial")
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    Private Sub LinkLabel3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        If MainForm.DownloadMode = 1 Then
            System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/FlyEduDownloader/Help/index.html")
        Else
            System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/FlyEduDownloader/Help/login.html")
        End If
    End Sub
End Class