'==========================================
'项目：SmartEduDownloader
'作者：CJH
'文件：DownLinkForm.vb
'描述：批量下载链接
'==========================================
Imports Newtonsoft.Json.Linq
Imports System.Net

Public Class DownLinkForm
    Dim st As Integer
    Dim mld As Integer
    Public DownloadClient As New WebClientPro
    Dim DownLinks() As String
    Dim dline As Integer
    'Dim lgtmp As String
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TextBox2.Text = ""
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Label3.Text = "当前下载目录：" & FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub DownLinkForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ServicePointManager.SecurityProtocol = CType(192, SecurityProtocolType) Or CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)
        st = 0
        mld = 1
        CheckBox1.Checked = True
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
            MessageBox.Show("至少输入一个地址。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        DownLinks = Split(TextBox1.Text, vbCrLf)
        Dim iu As Integer
        iu = DownLinks.Length
        For i = 0 To iu - 1
            dline = i + 1
            TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "正在处理第" & i + 1 & "行链接。" & vbCrLf & TextBox2.Text
            If DownLinks(i) = "" Then
                Exit Sub
            Else
                Call smartedudown(DownLinks(i))
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
        st = 0
    End Sub

    Sub smartedudown(ByVal BookLink As String)
        Try
            If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = False Then
                IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath)
            End If

            Dim fn As String
            Dim fn2() As String

            If BookLink.Substring(BookLink.Length - 1, 1) = "/" Then
                fn2(0) = ""
            Else
                fn2 = Split(BookLink, ".")
            End If

            Dim fn3() As String
            fn3 = Split(BookLink, "/")
            fn3(fn3.Count - 1) = fn3(fn3.Count - 1).Substring(0, fn3(fn3.Count - 1).Length - fn2(fn2.Count - 1).Length - 1)

            If CheckBox2.Checked = True Then
                fn = FolderBrowserDialog1.SelectedPath & "\" & fn3(fn3.Count - 1) & "_" & Format(Now, "yyyy-MM-dd-HH-mm-ss")
            Else
                fn = FolderBrowserDialog1.SelectedPath & "\" & fn3(fn3.Count - 1)
            End If
            If (fn & "." & fn2(fn2.Count - 1)).Length > 260 Then
                fn = fn.Substring(0, 260 - fn2(fn2.Count - 1).Length - 1)
            End If
            fn = fn & "." & fn2(fn2.Count - 1)
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
                Dim DownloadClient1 As New WebClientPro
                If MainForm.DownloadMode = 0 Then
                    DownloadClient1.Headers.Set("x-nd-auth", MainForm.XNdAuth)
                End If
                DownloadClient1.Timeout = 30000
                AddHandler DownloadClient1.DownloadFileCompleted, AddressOf DownloadClient_DownloadFileCompleted
                DownloadClient1.DownloadFileAsync(New Uri(BookLink), fn)
            Else
                DownloadClient.DownloadFile(New Uri(BookLink), fn)
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
            TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "第" & dline & "个" & e.Error.Message & vbCrLf & TextBox2.Text
            'lgtmp = ""
        ElseIf e.Cancelled = True Then
            TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "第" & dline & "个下载已被取消" & vbCrLf & TextBox2.Text
            'lgtmp = ""
        Else
            TextBox2.Text = Format(Now, "[yyyy-MM-dd HH:mm:ss] ") & "第" & dline & "个下载完成！" & vbCrLf & TextBox2.Text
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
End Class