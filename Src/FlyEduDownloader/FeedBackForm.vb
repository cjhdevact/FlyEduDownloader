'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：FeedBackForm.vb
'描述：反馈对话框
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
Imports Newtonsoft.Json.Linq

Public Class FeedBackForm
    Public FeedBackInfo As String
    Public MyFeedLink As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        System.Diagnostics.Process.Start(MyFeedLink)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        MessageBox.Show("使用Github反馈需要先登录GitHub账号，因此我们更建议你使用问卷反馈。如果使用Github反馈请在反馈内容后附上程序生成的反馈报告。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        System.Diagnostics.Process.Start(My.Resources.IssueG)
    End Sub
    '获取系统版本函数
    Function GetOSVersion() As String
        Dim strBuild1, strBuild2, strBuild3, strBuild4 As String
        Try
            Dim regKey As Microsoft.Win32.RegistryKey
            regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion")
            strBuild1 = regKey.GetValue("CurrentMajorVersionNumber").ToString
            strBuild2 = regKey.GetValue("CurrentMinorVersionNumber").ToString
            strBuild3 = regKey.GetValue("CurrentBuild").ToString
            strBuild4 = regKey.GetValue("UBR").ToString
            regKey.Close()
        Catch ex As Exception
            Return Environment.OSVersion.Version.ToString
            Exit Function
        End Try
        Return strBuild1 & "." & strBuild2 & "." & strBuild3 & "." & strBuild4
    End Function

    Private Sub FeedBackForm_Loads(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim feedbacklink As String
        feedbacklink = MainForm.GetSource(My.Resources.IssueA)
        Try
            Dim NoticeObject As JObject = JObject.Parse(feedbacklink)
            MyFeedLink = CStr(NoticeObject("link"))
        Catch ex As Exception
            MyFeedLink = My.Resources.IssueT
        End Try
        If MyFeedLink = "" Then
            MyFeedLink = My.Resources.IssueT
        End If

        Dim sa As String
        If Environment.Is64BitOperatingSystem = True Then
            sa = "x64"
        Else
            sa = "x86"
        End If
        Dim st As String
        If MainForm.DownloadMode = 1 Then
            st = "免登录下载模式"
        Else
            st = "登录下载模式"
        End If
        TextBox1.Text = "程序版本：" & My.Application.Info.Version.ToString & " (" & MainForm.AppBuildTime & ") " & MainForm.MyArch & " " & MainForm.AppBuildChannel
        TextBox1.Text = TextBox1.Text & vbCrLf & “下载模式：" & st
        TextBox1.Text = TextBox1.Text & vbCrLf & “错误内容：" & FeedBackInfo
        TextBox1.Text = TextBox1.Text & vbCrLf & vbCrLf & “处理链接：" & vbCrLf & MainForm.TextBox1.Text
        'TextBox1.Text = TextBox1.Text & vbCrLf & “资源信息：" & vbCrLf & MainForm.BookNameLabel.Text & " " & MainForm.BookIDLabel.Text

        If CheckBox1.Checked = True Then
            TextBox1.Text = TextBox1.Text & vbCrLf & vbCrLf & "系统信息：" & My.Computer.Info.OSFullName & " " & sa & " " & GetOSVersion()
            TextBox1.Text = TextBox1.Text & vbCrLf & vbCrLf & "程序启动目录：" & Application.StartupPath
            TextBox1.Text = TextBox1.Text & vbCrLf & "程序执行文件：" & Application.ExecutablePath
        End If
        TextBox1.Text = TextBox1.Text & vbCrLf & vbCrLf & "生成时间：" & Format(Now, "yyyy-MM-dd HH:mm:ss")
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Dim sa As String
        If Environment.Is64BitOperatingSystem = True Then
            sa = "x64"
        Else
            sa = "x86"
        End If
        Dim st As String
        If MainForm.DownloadMode = 1 Then
            st = "免登录下载模式"
        Else
            st = "登录下载模式"
        End If
        If Me.CheckBox1.Checked = False Then
            TextBox1.Text = "程序版本：" & My.Application.Info.Version.ToString & " (" & MainForm.AppBuildTime & ") " & MainForm.MyArch & " " & MainForm.AppBuildChannel
            TextBox1.Text = TextBox1.Text & vbCrLf & “下载模式：" & st
            TextBox1.Text = TextBox1.Text & vbCrLf & “错误内容：" & FeedBackInfo
            TextBox1.Text = TextBox1.Text & vbCrLf & vbCrLf & “处理链接：" & vbCrLf & MainForm.TextBox1.Text
            'TextBox1.Text = TextBox1.Text & vbCrLf & “资源信息：" & vbCrLf & MainForm.BookNameLabel.Text & " " & MainForm.BookIDLabel.Text

            TextBox1.Text = TextBox1.Text & vbCrLf & vbCrLf & "生成时间：" & Format(Now, "yyyy-MM-dd HH:mm:ss")
        Else
            TextBox1.Text = "程序版本：" & My.Application.Info.Version.ToString & " (" & MainForm.AppBuildTime & ") " & MainForm.MyArch & " " & MainForm.AppBuildChannel
            TextBox1.Text = TextBox1.Text & vbCrLf & “下载模式：" & st
            TextBox1.Text = TextBox1.Text & vbCrLf & “错误内容：" & FeedBackInfo
            TextBox1.Text = TextBox1.Text & vbCrLf & vbCrLf & “处理链接：" & vbCrLf & MainForm.TextBox1.Text
            'TextBox1.Text = TextBox1.Text & vbCrLf & “资源信息：" & vbCrLf & MainForm.BookNameLabel.Text & " " & MainForm.BookIDLabel.Text

            If CheckBox1.Checked = True Then
                TextBox1.Text = TextBox1.Text & vbCrLf & vbCrLf & "系统信息：" & My.Computer.Info.OSFullName & " " & sa & " " & GetOSVersion()
                TextBox1.Text = TextBox1.Text & vbCrLf & vbCrLf & "程序启动目录：" & Application.StartupPath
                TextBox1.Text = TextBox1.Text & vbCrLf & "程序执行文件：" & Application.ExecutablePath
            End If
            TextBox1.Text = TextBox1.Text & vbCrLf & vbCrLf & "生成时间：" & Format(Now, "yyyy-MM-dd HH:mm:ss")
        End If
    End Sub
End Class