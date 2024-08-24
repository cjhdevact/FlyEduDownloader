Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class MainForm
    Dim DownBookLink As String
    Dim DownBookImgLink As String
    Dim DownBookName As String
    Public DownloadClient As New WebClientPro

    Public MyArch As String
    Public Const AppBuildTime As String = "20240824"
    Public Const AppBuildChannel As String = "O"

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Environment.Is64BitProcess = True Then
            MyArch = "x64"
        Else
            MyArch = "x86"
        End If
        Me.Text = "SmartEduDownloader " & My.Application.Info.Version.ToString & " (" & MainForm.AppBuildTime & ") " & MyArch & " " & AppBuildChannel
        DownloadClient.Timeout = 30000
        DownBookLink = ""
        DownBookName = ""
        DownBookImgLink = ""
        AddHandler DownloadClient.DownloadProgressChanged, AddressOf DownloadClient_DownloadProgressChanged
        AddHandler DownloadClient.DownloadFileCompleted, AddressOf DownloadClient_DownloadFileCompleted
        'AddHandler DownloadClient.DownloadDataCompleted, AddressOf DownloadClient_DownloadFileCompleted
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Dim a As Integer
        'a = -1
        'a = InStr(TextBox1.Text, "basic.smartedu.cn")
        'Dim b As Integer
        'b = -1
        'b = InStr(TextBox1.Text, "book.pep.com.cn")
        'If b > 0 Then a = b
        If TextBox1.Text = "" Then
            MessageBox.Show("页面链接不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        ElseIf Not InStr(TextBox1.Text, "basic.smartedu.cn") > 0 Then
            MessageBox.Show("不支持下载当前链接。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Button2.Enabled = True
        Button3.Enabled = True
        Button4.Enabled = True
        If InStr(TextBox1.Text, "basic.smartedu.cn") > 0 Then Call smartedudown(TextBox1.Text)
    End Sub
    Sub smartedudown(ByVal BookLink As String)
        'On Error Resume Next

        'Link Exp.
        'https://basic.smartedu.cn/tchMaterial/detail?contentType=assets_document&contentId=1e04e52e-41df-4689-b12a-f3b7315a4243&catalogType=tchMaterial&subCatalog=tchMaterial
        'Books Link
        'https://r1-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/bdc00134-465d-454b-a541-dcd0cec4d86e.pkg/pdf.pdf
        'https://r2-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/bdc00134-465d-454b-a541-dcd0cec4d86e.pkg/pdf.pdf
        'https://r3-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/bdc00134-465d-454b-a541-dcd0cec4d86e.pkg/pdf.pdf
        Dim k As Integer
        k = InStr(BookLink, "contentId=")
        If k <= 0 Then
            MessageBox.Show("链接解析失败。无法获取BookID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        k = k + 9
        Dim bookid As String = BookLink.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
        If Len(bookid) < 36 Then
            MessageBox.Show("链接解析失败。无法获取BookID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim booknameurl As String
        booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrv2/resources/tch_material/details/" & bookid & ".json"
        Dim bookinforeq As String

        bookinforeq = GetSource(booknameurl)
        If bookinforeq = "" Then
            MessageBox.Show("获取电子书信息失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            Dim BookInfoObject As JObject = JObject.Parse(bookinforeq)

            Dim BookNameObject As JObject = BookInfoObject("global_title")
            DownBookName = CStr(BookNameObject("zh-CN"))
            Dim BookIDGet As String = BookInfoObject("id")
            Dim BookTagArray As JArray = BookInfoObject("tag_list")
            Dim BookTag As String = ""
            For i = 0 To BookTagArray.Count - 1
                BookTag = BookTag & BookTagArray(i)("tag_name").ToString & " "
            Next

            Dim BookItemsObject As JArray = BookInfoObject("ti_items")

            Dim BookDownLinkPri As String = CStr((BookItemsObject(1)("ti_storages"))(0)) & vbCrLf & CStr((BookItemsObject(1)("ti_storages"))(1)) & vbCrLf & CStr((BookItemsObject(1)("ti_storages"))(2))
            Dim BookDownLink As String = Replace(BookDownLinkPri, "ndr-private.ykt.cbern.com.cn", "ndr.ykt.cbern.com.cn")

            Dim DownBookLinkPri As String = CStr((BookItemsObject(1)("ti_storages"))(0))
            DownBookLink = Replace(DownBookLinkPri, "ndr-private.ykt.cbern.com.cn", "ndr.ykt.cbern.com.cn")

            'Dim BookPreviewLinkPri As String = CStr((BookItemsObject(3)("ti_storages"))(0))
            'DownBookImgLink = Replace(BookPreviewLinkPri, "ndr-private.ykt.cbern.com.cn", "ndr.ykt.cbern.com.cn")
            DownBookImgLink = CStr(((BookInfoObject("custom_properties"))("thumbnails"))(0))

            TextBox2.Text = BookDownLink

            BookNameLabel.Text = "书籍名称：" & DownBookName
            BookIDLabel.Text = "书籍ID：" & BookIDGet
            BookTagLabel.Text = "书籍标签：" & BookTag
        Catch ex As Exception
            MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Try
            Dim whttpReq As System.Net.HttpWebRequest 'HttpWebRequest 类对 WebRequest 中定义的属性和方法提供支持'，也对使用户能够直接与使用 HTTP 的服务器交互的附加属性和方法提供支持。
            Dim whttpURL As New System.Uri(DownBookImgLink)
            whttpReq = CType(WebRequest.Create(whttpURL), HttpWebRequest)
            whttpReq.Timeout = 30000
            whttpReq.Method = "GET"
            Dim res As WebResponse = whttpReq.GetResponse()
            Dim shi As New Bitmap(res.GetResponseStream)
            PictureBox1.Image = Nothing
            PictureBox1.Image = shi
        Catch ex As Exception
            MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetSource(ByVal url As String) As String
        Try
            'Need .Net Framework 4.5 +
            'ServicePointManager.Expect100Continue = True
            'ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
            'ServicePointManager.ServerCertificateValidationCallback = Function(sender, certificate, chain, errors) True
            ServicePointManager.SecurityProtocol = CType(192, SecurityProtocolType) Or CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)

            Dim httpReq As System.Net.HttpWebRequest 'HttpWebRequest 类对 WebRequest 中定义的属性和方法提供支持'，也对使用户能够直接与使用 HTTP 的服务器交互的附加属性和方法提供支持。
            Dim httpResp As System.Net.HttpWebResponse ' HttpWebResponse 类用于生成发送 HTTP 请求和接收 HTTP 响'应的 HTTP 独立客户端应用程序。
            Dim httpURL As New System.Uri(url)
            httpReq = CType(WebRequest.Create(httpURL), HttpWebRequest)
            httpReq.Timeout = 30000
            httpReq.Method = "GET"
            httpResp = CType(httpReq.GetResponse(), HttpWebResponse)
            'Dim reader As StreamReader = New StreamReader(httpResp.GetResponseStream, System.Text.Encoding.GetEncoding("GB2312")) '如是中文，要设置编码格式为“GB2312”。
            Dim reader As StreamReader = New StreamReader(httpResp.GetResponseStream, System.Text.Encoding.UTF8)
            Dim respHTML As String = reader.ReadToEnd() 'respHTML就是网页源代码
            Return respHTML
            httpResp.Close()
        Catch e As Exception
            MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Public Function GetSource2(ByVal url As String) As String
        Try
            'Need .Net Framework 4.5 +
            'ServicePointManager.Expect100Continue = True
            'ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
            'ServicePointManager.ServerCertificateValidationCallback = Function(sender, certificate, chain, errors) True
            ServicePointManager.SecurityProtocol = CType(192, SecurityProtocolType) Or CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)
            Dim stream As IO.Stream = WebRequest.Create(url).GetResponse().GetResponseStream()
            Dim sr As StreamReader = New StreamReader(stream, System.Text.Encoding.UTF8)
            'Label1.Text = Regex.Match(sr.ReadToEnd, "回答采纳率").ToString
            Dim respHTML As String = sr.ReadToEnd()
            Return respHTML
            sr.Dispose() '关闭流
        Catch e As Exception
            MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Sub FindBooksB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindBooksB.Click
        System.Diagnostics.Process.Start("https://basic.smartedu.cn/tchMaterial")
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        SaveFileDialog1.Filter = "PDF 文件(*.pdf)|*.pdf"
        SaveFileDialog1.FileName = DownBookName & ".pdf"
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            DownFormvb.Label1.Text = "正在下载 0%" & vbCrLf & "已经下载 0 MB，共 0 MB"
            DownFormvb.Button1.Text = "取消"
            'DownFormvb.Button1.Visible = False
            DownFormvb.st = 0
            DownFormvb.ProgressBar1.Value = 0
            Try
                DownloadClient.DownloadFileAsync(New Uri(DownBookLink), SaveFileDialog1.FileName)
            Catch ex As Exception
                DownFormvb.Label1.Text = ex.Message
            End Try
            DownFormvb.ShowDialog()
        End If
    End Sub
    'Private Sub ShowDownProgress(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
    'Invoke(New Action(Of Integer)(Sub(i) DownFormvb.ProgressBar1.Value = i), e.ProgressPercentage)
    'Invoke(New Action(Of Integer)(Sub(i) DownFormvb.Label1.Text = "正在下载 " & i & "%"), e.ProgressPercentage)
    'End Sub

    Private Sub DownloadClient_DownloadProgressChanged(ByVal sender As Object, ByVal e As System.Net.DownloadProgressChangedEventArgs)
        DownFormvb.ProgressBar1.Value = e.ProgressPercentage
        DownFormvb.Label1.Text = "正在下载 " & e.ProgressPercentage & "%" & vbCrLf & "已经下载 " & Int(e.BytesReceived / 1024 / 1024).ToString & " MB，共 " & Int(e.TotalBytesToReceive / 1024 / 1024).ToString & " MB"
    End Sub

    Private Sub DownloadClient_DownloadFileCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        If e.Error IsNot Nothing Then
            DownFormvb.Label1.Text = e.Error.Message
            DownFormvb.Button1.Text = "确定"
            'DownFormvb.Button1.Visible = True
        ElseIf e.Cancelled = True Then
            DownFormvb.Label1.Text = "下载已被取消"
            DownFormvb.Button1.Text = "确定"
            'DownFormvb.Button1.Visible = True
        Else
            DownFormvb.Label1.Text = "下载完成！"
            DownFormvb.Button1.Text = "确定"
            'DownFormvb.Button1.Visible = True
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        SaveFileDialog1.Filter = "JPG 文件(*.jpg)|*.jpg"
        SaveFileDialog1.FileName = DownBookName & "_" & Format(Now, "yyyy-MM-dd-HH-mm-ss") & ".jpg"
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            DownFormvb.Label1.Text = "正在下载 0%" & vbCrLf & "已经下载 0 MB，共 0 MB"
            DownFormvb.Button1.Text = "取消"
            'DownFormvb.Button1.Visible = False
            DownFormvb.st = 0
            DownFormvb.ProgressBar1.Value = 0
            Try
                DownloadClient.DownloadFileAsync(New Uri(DownBookImgLink), SaveFileDialog1.FileName)
            Catch ex As Exception
                DownFormvb.Label1.Text = ex.Message
            End Try
            DownFormvb.ShowDialog()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        SaveFileDialog1.Filter = "文本文件(*.txt)|*.txt"
        SaveFileDialog1.FileName = DownBookName & "_" & Format(Now, "yyyy-MM-dd-HH-mm-ss") & ".txt"
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If IO.File.Exists(SaveFileDialog1.FileName) Then
                Try
                    IO.File.Delete(SaveFileDialog1.FileName)
                Catch ex As Exception
                    MessageBox.Show("写入文件错误，存在同名文件。" & vbCrLf & ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End Try
            End If
            Dim fs As New System.IO.FileStream(SaveFileDialog1.FileName, IO.FileMode.Create)
            fs.Close()
            Dim fs2 As New IO.StreamWriter(SaveFileDialog1.FileName)
            fs2.Write(vbCrLf)
            fs2.Write("书籍名称：" & DownBookName)
            fs2.Write(vbCrLf & BookIDLabel.Text)
            fs2.Write(vbCrLf & BookTagLabel.Text)
            fs2.Write(vbCrLf & "下载链接：")
            fs2.Write(vbCrLf & TextBox2.Text)
            fs2.Close()
        End If
    End Sub

    Private Sub aboutm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles aboutm.Click
        AboutForm.ShowDialog()
    End Sub

    Private Sub readmm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readmm.Click
        MGetForm.Show()
    End Sub

    Private Sub downmm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles downmm.Click
        MDownForm.Show()
    End Sub

    Private Sub hlpdocm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hlpdocm.Click
        System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/SmartEduDownloader/Help/index.html")
    End Sub

    Private Sub ofpm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ofpm.Click
        System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/SmartEduDownloader/index.html")
    End Sub
End Class
