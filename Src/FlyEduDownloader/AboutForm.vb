Imports System.IO
Imports System.Reflection

'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：AboutForm.vb
'描述：关于对话框
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
Public Class AboutForm

    Public AppBuildPETime As DateTime

    '获取编译时间函数
    Private Function GetPe32Time(ByVal fileName As String) As DateTime
        Dim num As Integer
        Using reader As BinaryReader = New BinaryReader(New FileStream(fileName, FileMode.Open, FileAccess.Read))
            Dim buffer As Byte() = reader.ReadBytes(2)
            Dim message As String = "Error in PE32 file."
            If (buffer.Length <> 2) Then
                'Throw New Exception(message)
            End If
            If ((buffer(0) <> &H4D) OrElse (buffer(1) <> 90)) Then
                'Throw New Exception(message)
            End If
            reader.BaseStream.Seek(60, SeekOrigin.Begin)
            Dim num2 As Byte = reader.ReadByte
            reader.BaseStream.Seek(CLng(num2), SeekOrigin.Begin)
            buffer = reader.ReadBytes(4)
            If (buffer.Length <> 4) Then
                'Throw New Exception(message)
            End If
            If ((((buffer(0) <> 80) OrElse (buffer(1) <> &H45)) OrElse (buffer(2) <> 0)) OrElse (buffer(3) <> 0)) Then
                'Throw New Exception(message)
            End If
            If (reader.ReadBytes(4).Length <> 4) Then
                'Throw New Exception(message)
            End If
            num = reader.ReadInt32
        End Using
        Return DateTime.SpecifyKind(New DateTime(&H7B2, 1, 1), DateTimeKind.Utc).AddSeconds(CDbl(num)).ToLocalTime
    End Function

    Private Sub LinkLabel3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        If MainForm.DownloadMode = 1 Then
            System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/FlyEduDownloader/Help/index.html")
        Else
            System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/FlyEduDownloader/Help/login.html")
        End If
    End Sub
    '初始化
    Private Sub AboutForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Label1.Text = "飞翔教学资源助手"
        Dim sa As String
        If Environment.Is64BitOperatingSystem = True Then
            sa = "x64"
        Else
            sa = "x86"
        End If
        Dim st As String
        If MainForm.DownloadMode = 1 Then
            st = "（免登录下载模式）"
        Else
            st = "（登录下载模式）"
        End If
        AppBuildPETime = GetPe32Time(Application.ExecutablePath)
        Label3.Text = "版本 " & My.Application.Info.Version.ToString & " (" & MainForm.AppBuildTime & ") " & MainForm.MyArch & " " & MainForm.AppBuildChannel & " " & st
        Label6.Text = "系统信息 " & My.Computer.Info.OSFullName & " " & sa & " " & GetOSVersion()
        Label8.Text = "编译时间 " & Format(AppBuildPETime, "yyyy-MM-dd HH:mm:ss")
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

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        'System.Diagnostics.Process.Start("https://github.com/cjhdevact/FlyEduDownloader/issues")
        FeedBackForm.FeedBackInfo = ""
        FeedBackForm.ShowDialog()
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/FlyEduDownloader/index.html")
    End Sub

    Private Sub LinkLabel4_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        System.Diagnostics.Process.Start("https://github.com/cjhdevact/FlyEduDownloader")
    End Sub

    Private Sub LinkLabel5_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        LicenseForm.ShowDialog()
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        If Label1.Text = "飞翔教学资源助手" Then
            Label1.Text = "授人以鱼，不如授人以渔"
        Else
            Label1.Text = "飞翔教学资源助手"
        End If
    End Sub
End Class