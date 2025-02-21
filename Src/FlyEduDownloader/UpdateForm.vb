'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：UpdateForm.vb
'描述：更新相关组件
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
Imports System.Net

Public Class UpdateForm
    Public UpdateLink64 As String
    Public UpdateLink32 As String
    Public DownloadClient As New WebClient '下载器对象
    Dim a As Integer
    Private Sub UpdateForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        With DownloadClient
            '    .Accept = "*/*"
            '    .Headers.Set("accept-encoding", "gzip, deflate, br, zstd")
            '    .Headers.Set("accept-language", "zh-CN,zh;q=0.9")
            '    .Headers.Set("origin", "https://basic.smartedu.cn")
            '    .Referer = "https://basic.smartedu.cn/"
            '    .Headers.Set("sec-ch-ua", """Not(A:Brand"";v=""99"", ""Google Chrome"";v=""133"", ""Chromium"";v=""133""")
            '    .Headers.Set("sec-ch-ua-mobile", "?0")
            '    .Headers.Set("sec-ch-ua-platform", """Windows""")
            '    .Headers.Set("sec-fetch-dest", "empty")
            '    .Headers.Set("sec-fetch-mode", "cors")
            '    .Headers.Set("sec-fetch-site", "cross-site")
            .Headers.Set("useragent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36")
        End With
        a = 0
        AddHandler DownloadClient.DownloadProgressChanged, AddressOf DownloadClient_DownloadProgressChanged
        AddHandler DownloadClient.DownloadFileCompleted, AddressOf DownloadClient_DownloadFileCompleted
    End Sub

    '下载进度同步
    Private Sub DownloadClient_DownloadProgressChanged(ByVal sender As Object, ByVal e As System.Net.DownloadProgressChangedEventArgs)
        Me.ProgressBar1.Value = e.ProgressPercentage
        Me.TextBox1.Text = "正在下载 " & e.ProgressPercentage & "%" & vbCrLf & "已经下载 " & Int(e.BytesReceived / 1024 / 1024).ToString & " MB，共 " & Int(e.TotalBytesToReceive / 1024 / 1024).ToString & " MB"
    End Sub
    '下载状态处理
    Private Sub DownloadClient_DownloadFileCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        If e.Error IsNot Nothing Then
            MainForm.MessageBoxError("下载错误" & vbCrLf & e.Error.Message, "飞翔教学资源助手 - 错误", True)
            If Me.Button2.Text = "退出" Then
                End
            Else
                a = 1
                Me.Close()
            End If
            'DownFormvb.Button1.Visible = True
        ElseIf e.Cancelled = True Then
            MainForm.MessageBoxError("下载已取消", "飞翔教学资源助手 - 提示", False)
            If Me.Button2.Text = "退出" Then
                End
            Else
                a = 1
                Me.Close()
            End If
            'DownFormvb.Button1.Visible = True
        Else
            Me.TextBox1.Text = "正在更新中……"
            If System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & " \Temp\FlyEduDownUpdatePack.exe") Then
                Try
                    System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & " \Temp\FlyEduDownUpdatePack.exe", "/verysilent /suppressmsgboxes /norestart")
                    End
                Catch ex As Exception
                    MainForm.MessageBoxError("更新失败。" & vbCrLf & ex.Message, "飞翔教学资源助手 - 错误", True)
                    If Me.Button2.Text = "退出" Then
                        End
                    Else
                        a = 1
                        Me.Close()
                    End If
                End Try
            End If
            a = 1
            Me.Close()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        '初始化下载器
        Me.TextBox1.Text = "正在下载 0%" & vbCrLf & "已经下载 0 MB， 共 0 MB"
        'DownFormvb.Button1.Text = "取消"
        'DownFormvb.Button1.Visible = False
        Me.ProgressBar1.Value = 0

        If Not System.IO.Directory.Exists(Application.StartupPath) Then
            Try
                System.IO.Directory.CreateDirectory(Application.StartupPath)
            Catch ex As Exception
                MainForm.MessageBoxError("更新失败： 无法创建下载目录。", "飞翔教学资源助手 - 错误", True)
                If Me.Button2.Text = "退出" Then
                    End
                Else
                    a = 1
                    Me.Close()
                End If
            End Try
        End If

        If System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & " \Temp\FlyEduDownUpdatePack.exe") Then
            Try
                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & " \Temp\FlyEduDownUpdatePack.exe")
            Catch ex As Exception
                MainForm.MessageBoxError("更新失败：无法删除已存在的文件。", "飞翔教学资源助手 - 错误", True)
                If Me.Button2.Text = "退出" Then
                    End
                Else
                    a = 1
                    Me.Close()
                End If
            End Try
        End If

        If Environment.Is64BitProcess = True Then
            If UpdateLink64 = "" Then
                MainForm.MessageBoxError("更新失败：无法获取有效的更新链接。", "飞翔教学资源助手 - 错误", True)
                If Me.Button2.Text = "退出" Then
                    End
                Else
                    a = 1
                    Me.Close()
                End If
            End If
            '开始下载
            Try
                Button1.Enabled = False
                Button2.Enabled = False
                ProgressBar1.Visible = True
                DownloadClient.DownloadFileAsync(New Uri(UpdateLink64), Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & " \Temp\FlyEduDownUpdatePack.exe")
            Catch ex As Exception
                MainForm.MessageBoxError("下载更新错误。" & vbCrLf & ex.Message, "飞翔教学资源助手 - 错误", True)
                If Me.Button2.Text = "退出" Then
                    End
                Else
                    a = 1
                    Me.Close()
                End If
            End Try
            'DownFormvb.ShowDialog() '显示下载进度
            'MainForm.DisbMsg = 1
            'Call MainForm.DownSub(UpdateLink64, Application.StartupPath & "\FlyEduDownUpdatePack.exe")
            'MainForm.DisbMsg = 0
        Else
            If UpdateLink32 = "" Then
                MainForm.MessageBoxError("更新失败：无法获取有效的更新链接。", "飞翔教学资源助手 - 错误", True)
                If Me.Button2.Text = "退出" Then
                    End
                Else
                    a = 1
                    Me.Close()
                End If
            End If

            '开始下载
            Try
                Button1.Enabled = False
                Button2.Enabled = False
                ProgressBar1.Visible = True
                DownloadClient.DownloadFileAsync(New Uri(UpdateLink32), Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & " \Temp\FlyEduDownUpdatePack.exe")
            Catch ex As Exception
                MainForm.MessageBoxError("下载更新错误。" & vbCrLf & ex.Message, "飞翔教学资源助手 - 错误", True)
                If Me.Button2.Text = "退出" Then
                    End
                Else
                    a = 1
                    Me.Close()
                End If
            End Try
            'DownFormvb.ShowDialog() '显示下载进度
            'MainForm.DisbMsg = 1
            ''Call MainForm.DownSub(UpdateLink32, Application.StartupPath & "\FlyEduDownUpdatePack.exe")
            ''Invoke(New MainForm.DownUpdateForm(AddressOf MainForm.DownUpdate), UpdateLink32, Application.StartupPath & "\FlyEduDownUpdatePack.exe")
            'MainForm.DisbMsg = 0
        End If

    End Sub
    Private Sub UpdateForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyClass.FormClosing
        If a = 0 Then
            e.Cancel = True
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        If Me.Button2.Text = "退出" Then
            End
        Else
            a = 1
            Me.Close()
        End If
    End Sub
End Class