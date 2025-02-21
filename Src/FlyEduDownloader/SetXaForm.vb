'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：SetXaForm.vb
'描述：X-Nd-Auth 设置
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
Public Class SetXaForm
    Public TempXa As String
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If LoginHelp.ShowDialog() = DialogResult.OK Then
            If TempXa = "" Then
                MainForm.MessageBoxError("获取登录信息失败。" & vbCrLf & "请重新尝试手动设置信息，或使用直接登录方式登录。", "飞翔教学资源助手 - 错误", True)
            Else
                Call Button3_Click(sender, e)
            End If
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If TempXa = "" Then
            MainForm.MessageBoxError("没有登录信息，请先登录。", "飞翔教学资源助手 - 错误", False)
            'MessageBox.Show("X-Nd-Auth值不能为空。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        MainForm.XNdAuth = TempXa
        Button4.Text = "取消"
        Button5.Visible = False
        If CheckBox1.Checked = True Then
            Dim Str As String = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(MainForm.XNdAuth))
            AddReg("Software\CJH\FlyEduDownloader", "LoginState", Str, Microsoft.Win32.RegistryValueKind.String, "HKCU")
        Else
            DelReg("Software\CJH\FlyEduDownloader", "LoginState", "HKCU")
        End If
        '清理Cookies缓存
        Try
            System.IO.File.Delete(Application.StartupPath & "\cookies.dat")
        Catch ex As Exception
        End Try
        Try
            System.IO.Directory.Delete(Application.StartupPath & "\LocalStorage")
        Catch ex As Exception
        End Try
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Close()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If Button4.Text = "退出" Then
            'Application.Exit()
            End
        Else
            Me.Close()
        End If
    End Sub

    Private Sub SetXaForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        '清理Cookies缓存
        Try
            System.IO.File.Delete(Application.StartupPath & "\cookies.dat")
        Catch ex As Exception
        End Try
        Try
            System.IO.Directory.Delete(Application.StartupPath & "\LocalStorage")
        Catch ex As Exception
        End Try
        If Button4.Text = "退出" Then
            'Application.Exit()
            End
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
        If MessageBox.Show("免登录模式只能下载旧版链接教材或课程，即勾选了强制使用旧版链接选项，如果要下载最新教材，请使用登录模式下载。" & vbCrLf & "如果免登录模式无法下载，请使用登录模式下载。" & vbCrLf & "是否要使用免登录模式？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            '运行自己加参数
            System.Diagnostics.Process.Start(Application.ExecutablePath, "/unloginmode")
            'Application.Exit()
            End
        End If
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If WebLoginForm.ShowDialog() = DialogResult.OK Then
            If TempXa = "" Then
                MainForm.MessageBoxError("获取登录信息失败。" & vbCrLf & "请重新尝试登录，或使用手动设置信息方式登录。", "飞翔教学资源助手 - 错误", True)
            Else
                Call Button3_Click(sender, e)
            End If
        End If
    End Sub

    Private Sub SetXaForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'TempXa = ""
        LoginHelp.TextBox1.Text = TempXa
    End Sub
End Class