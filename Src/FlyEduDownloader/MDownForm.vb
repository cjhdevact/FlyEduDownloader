﻿'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：MDownForm.vb
'描述：批量解析并下载链接
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
Imports System.Net

Public Class MDownForm
    Dim st As Integer
    Dim mld As Integer
    Public DownloadClient As New WebClientPro
    Dim DownLinks() As String
    Dim dline As Integer
    Dim strmode As Integer
    'Dim lgtmp As String
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TextBox2.Text = ""
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Label3.Text = "当前下载目录：" & FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub MDownForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

        strmode = 0
        ServicePointManager.SecurityProtocol = CType(192, SecurityProtocolType) Or CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)
        st = 0
        mld = 1
        'CheckBox1.Checked = False
        DownloadClient.Timeout = 30000
        'AddHandler DownloadClient.DownloadProgressChanged, AddressOf DownloadClient_DownloadProgressChanged
        AddHandler DownloadClient.DownloadFileCompleted, AddressOf DownloadClient_DownloadFileCompleted
        'FolderBrowserDialog1.SelectedPath = Application.StartupPath & "\BookDownloads"
        FolderBrowserDialog1.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\BookDownloads"
        Me.Label3.Text = "当前下载目录：" & FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Shell("explorer.exe """ & FolderBrowserDialog1.SelectedPath & """", AppWinStyle.NormalFocus, False)
        'Process.Start("explorer.exe", FolderBrowserDialog1.SelectedPath)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Then
            MainForm.MessageBoxError("至少输入一个地址。", "飞翔教学资源助手 - 错误", False)
            'MessageBox.Show("至少输入一个地址。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        st = 1
        TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "**************开始下载**************" & vbCrLf & TextBox2.Text
        If MainForm.DownloadMode = 0 Then
            DownloadClient.Headers.Set("x-nd-auth", MainForm.XNdAuth)
        End If
        TextBox1.Enabled = False
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = False
        CheckBox1.Enabled = False
        CheckBox2.Enabled = False
        DownLinks = Split(TextBox1.Text, vbCrLf)
        Dim iu As Integer
        iu = DownLinks.Length
        For i = 0 To iu - 1
            dline = i + 1
            TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "正在处理第" & i + 1 & "行链接。" & vbCrLf & TextBox2.Text
            If DownLinks(i) = "" Then
                Exit Sub
            ElseIf Not InStr(DownLinks(i), "basic.smartedu.cn") > 0 Then
                TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "错误：不支持下载当前链接。" & vbCrLf & TextBox2.Text
            Else
                If InStr(DownLinks(i), "basic.smartedu.cn") > 0 Then Call smartedudown(DownLinks(i))
            End If
        Next
        If mld = 1 Then
            TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "**************全部开始下载**************" & vbCrLf & TextBox2.Text
        Else
            TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "**************下载完成**************" & vbCrLf & TextBox2.Text
        End If
        TextBox1.Enabled = True
        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button4.Enabled = True
        CheckBox1.Enabled = True
        CheckBox2.Enabled = True
        st = 0
    End Sub

    Sub smartedudown(ByVal BookLink As String)
        Dim k As Integer
        k = InStr(BookLink, "contentId=")
        If k <= 0 Then
            TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "错误：链接解析失败。无法获取BookID。" & vbCrLf & TextBox2.Text
            Exit Sub
        End If
        k = k + 9
        Dim bookid As String = BookLink.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
        If Len(bookid) < 36 Then
            TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "错误：链接解析失败。无法获取BookID。" & vbCrLf & TextBox2.Text
            Exit Sub
        End If
        Dim booknameurl As String
        '从官方服务器获取资源信息
        If BookLink.Contains("thematic_course") = True Then '资源包教材
            booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrs/special_edu/thematic_course/" & bookid & "/resources/list.json"
        Else
            '普通教材
            booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrv2/resources/tch_material/details/" & bookid & ".json"
        End If
        Dim bookinforeq As String

        bookinforeq = MainForm.GetSource(booknameurl)
        If bookinforeq = "" Then
            TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "错误：获取电子书信息失败。" & vbCrLf & TextBox2.Text
            Exit Sub
        End If
        Try
            Dim BookInfoObject As JObject
            If bookinforeq.Substring(0, 3).Contains("[") = True Then
                BookInfoObject = JArray.Parse(bookinforeq)(0)
            Else
                BookInfoObject = JObject.Parse(bookinforeq)
            End If
            Dim BookItemsObject As JArray = BookInfoObject("ti_items")
            Dim DownBookLinkPri As String = ""
            If CheckBox2.Checked = True Then '强制获取旧版教材
                DownBookLinkPri = "https://r1-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/" & bookid & ".pkg/pdf.pdf"
            Else
                For i = 0 To BookItemsObject.Count - 1
                    If BookItemsObject(i)("ti_format") = "pdf" Then
                        DownBookLinkPri = CStr((BookItemsObject(i)("ti_storages"))(0)) '获取官方教材链接
                        Exit For
                    End If
                Next
            End If
            Dim DownBookLink As String
            If MainForm.DownloadMode = 1 Then
                DownBookLink = Replace(DownBookLinkPri, "ndr-private.ykt.cbern.com.cn", "ndr.ykt.cbern.com.cn")
            Else
                DownBookLink = DownBookLinkPri
            End If


            'lgtmp = TextBox2.Text

            Dim BookNameObject As JObject = BookInfoObject("global_title")
            Dim DownBookName As String = CStr(BookNameObject("zh-CN"))
            Dim BookIDGet As String = BookInfoObject("id")


            If CheckBox2.Checked = True Then
                DownBookName = Replace(DownBookName, "（根据2022年版课程标准修订）", "")
            End If

            If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then
                IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath)
            End If

            Dim fn As String
            fn = FolderBrowserDialog1.SelectedPath & "\" & DownBookName & "_" & BookIDGet & "_" & Format(Now, "yyyy-MM-dd-HH-mm-ss")

            If (fn & ".pdf").Length > 260 Then
                fn = fn.Substring(0, 260 - 4)
            End If
            fn = fn & ".pdf"

            '处理文件名，去除非法字符\/:*?"<>|
            Dim ffn() As String
            ffn = Split(fn, "\")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "\", "_")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "/", "_")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), ":", "-")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "*", "-")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "?", "")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), """", "")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "<", "")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), ">", "")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "|", "_")
            ffn(ffn.Length - 1) = MainForm.EnsureValidFileName(ffn(ffn.Length - 1))
            fn = Join(ffn, "\")
            'fn = MainForm.EnsureValidFileName(fn)
            If IO.File.Exists(fn) Then
                Try
                    IO.File.Delete(fn)
                Catch ex As Exception
                    TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "错误：写入文件错误，存在同名文件。" & vbCrLf & TextBox2.Text
                End Try
            End If

            TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "正在下载第" & dline & "个 " & vbCrLf & TextBox2.Text

            'DownloadClient.DownloadFileAsync(New Uri(DownBookLink), (fn))
            If mld = 1 Then
                strmode = 1
                Dim DownloadClient1 As New WebClientPro
                If MainForm.DownloadMode = 0 Then
                    DownloadClient1.Headers.Set("x-nd-auth", MainForm.XNdAuth)
                End If
                DownloadClient1.Timeout = 30000
                AddHandler DownloadClient1.DownloadFileCompleted, AddressOf DownloadClient_DownloadFileCompleted
                Threading.Thread.Sleep(500)
                DownloadClient1.DownloadFileAsync(New Uri(DownBookLink), fn, dline)
            Else
                strmode = 0
                Threading.Thread.Sleep(500)
                DownloadClient.DownloadFile(New Uri(DownBookLink), fn)
            End If
        Catch ex As Exception
            TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "错误：" & ex.Message & vbCrLf & TextBox2.Text
        End Try
    End Sub

    'Private Sub DownloadClient_DownloadProgressChanged(ByVal sender As Object, ByVal e As System.Net.DownloadProgressChangedEventArgs)
    '    TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "正在下载第" & dline & "个 " & e.ProgressPercentage & "% 已经下载 " & Int(e.BytesReceived / 1024 / 1024).ToString & " MB，共 " & Int(e.TotalBytesToReceive / 1024 / 1024).ToString & " MB" & vbCrLf & TextBox2.Text
    'End Sub

    Private Sub DownloadClient_DownloadFileCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        If e.Error IsNot Nothing Then
            If strmode = 1 Then
                TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "第" & e.UserState & "个" & e.Error.Message & vbCrLf & TextBox2.Text
            Else
                TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "第" & dline & "个" & e.Error.Message & vbCrLf & TextBox2.Text
            End If

            'lgtmp = ""
        ElseIf e.Cancelled = True Then
            If strmode = 1 Then
                TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "第" & e.UserState & "个下载已被取消" & vbCrLf & TextBox2.Text
            Else
                TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "第" & dline & "个下载已被取消" & vbCrLf & TextBox2.Text
            End If
            'lgtmp = ""
        Else
            If strmode = 1 Then
                TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "第" & e.UserState & "个下载完成！" & vbCrLf & TextBox2.Text
            Else
                TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "第" & dline & "个下载完成！" & vbCrLf & TextBox2.Text
            End If
            'lgtmp = ""
        End If
    End Sub

    Private Sub DownFormvb_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyClass.FormClosing
        If st = 1 Then
            e.Cancel = True
        Else
            e.Cancel = False
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            mld = 1
        Else
            mld = 0
        End If
    End Sub

    Private Sub CheckBox1_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckStateChanged
        If CheckBox1.CheckState = CheckState.Checked Then
            If MessageBox.Show("使用多线程下载可能会导致性能问题，虽然多线程下载可以一次下载多个文件，且在处理少量链接下可以避免界面卡死，但如果处理大量链接时，可能会出现程序卡死现象。如果你要使用该模式，请确保你的电脑性能可以支持该模式，并且不要一次性下载过多的链接。" & vbCrLf & "是否使用多线程下载模式？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                CheckBox1.Checked = CheckState.Unchecked
            End If
        End If
    End Sub
End Class