'==========================================
'项目：SmartEduDownloader
'作者：CJH
'文件：MainForm.vb
'描述：主程序部分（解析下载资源）
'==========================================
Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class MainForm
    Dim DownBookLink As String '电子课本链接
    Dim DownBookLinks() As String '资源包链接集合
    Dim DownBookImgLink As String '封面图片链接
    Dim DownBookName As String '资源名称

    Dim NtTi As String '公告标题
    Dim NtTx As String '公告内容

    Public DownloadMode As Integer '下载模式 0=登录 1=免登录

    Public DownloadClient As New WebClientPro '下载器对项

    Public XNdAuth As String 'X-Nd-Auth 标头

    '多线程获取公告
    Dim NoticeThread As New Threading.Thread(AddressOf GetNotice)

    '程序版本信息
    Public MyArch As String
    Public Const AppBuildTime As String = "20240906"
    Public Const AppBuildChannel As String = "O"

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '解决无法建立安全的TLS/SSL连接问题
        ServicePointManager.SecurityProtocol = CType(192, SecurityProtocolType) Or CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)
        'DPI控件微调
        Dim disi As Graphics = Me.CreateGraphics()
        TextBox2.Height = disi.DpiX * 0.01 * 135
        '程序架构判断
        If Environment.Is64BitProcess = True Then
            MyArch = "x64"
        Else
            MyArch = "x86"
        End If

        '初始化
        DownloadClient.Timeout = 30000
        DownBookLink = ""
        DownBookName = ""
        DownBookImgLink = ""
        AddHandler DownloadClient.DownloadProgressChanged, AddressOf DownloadClient_DownloadProgressChanged
        AddHandler DownloadClient.DownloadFileCompleted, AddressOf DownloadClient_DownloadFileCompleted
        'AddHandler DownloadClient.DownloadDataCompleted, AddressOf DownloadClient_DownloadFileCompleted

        '登录模式设置
        If Command.ToLower = "/loginmode" Then
            Me.Text = "SmartEduDownloader " & My.Application.Info.Version.ToString & " (" & MainForm.AppBuildTime & ") " & MyArch & " " & AppBuildChannel & " （登录下载模式）"
            DownloadMode = 0
            sxam.Visible = True
            logmm.Text = "使用免登录模式下载(&L)"
            Label3.Visible = True
            Label1.Text = "下载链接：（可能存在多个下载链接，选择一个能用的粘贴到的实用工具-下载链接菜单下载即可）"
            SetXaForm.Button4.Text = "退出"
            SetXaForm.StartPosition = FormStartPosition.CenterScreen
            SetXaForm.ShowDialog()
        Else
            Me.Text = "SmartEduDownloader " & My.Application.Info.Version.ToString & " (" & MainForm.AppBuildTime & ") " & MyArch & " " & AppBuildChannel & " （免登录下载模式）"
            DownloadMode = 1
            sxam.Visible = False
            logmm.Text = "使用登录模式下载(&L)"
            Label3.Visible = False
        End If
        NoticeThread.Start()
    End Sub
    '获取公告
    Sub GetNotice()
        Dim ntic As String
        ntic = GetSource("https://cjhdevact.github.io/otherprojects/SmartEduDownloader/noticemsg.json")
        Try
            Dim NoticeObject As JObject = JObject.Parse(ntic)
            NtTi = CStr(NoticeObject("title"))
            NoticeForm.Text = NtTi
            Dim InfoText As JArray = NoticeObject("text")
            For i = 0 To InfoText.Count - 1
                If i = InfoText.Count - 1 Then
                    NtTx = NtTx & InfoText(i).ToString
                Else
                    NtTx = NtTx & InfoText(i).ToString & vbCrLf
                End If
            Next
            NoticeForm.TextBox1.Text = NtTx
            NoticeForm.ShowDialog()
        Catch ex As Exception
            noticem.Visible = False
        End Try
    End Sub

    '解析链接操作
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Dim a As Integer
        'a = -1
        ''资源包
        'a = InStr(TextBox1.Text, "activityId=")
        'Dim b As Integer
        'b = -1
        ''教材
        'b = InStr(TextBox1.Text, "contentId=")
        'If b > 0 Then a = b
        'Dim c As Integer
        'c = -1
        ''资源包
        'c = InStr(TextBox1.Text, "courseId=")
        'If c > 0 Then a = c
        If TextBox1.Text = "" Then
            MessageBox.Show("页面链接不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
            'ElseIf Not InStr(TextBox1.Text, "basic.smartedu.cn") > 0 Then
        End If
        If InStr(TextBox1.Text.ToLower, "activityid=") > 0 Then
            Call smartedudownless(TextBox1.Text)
            Exit Sub
        End If
        If InStr(TextBox1.Text.ToLower, "courseid=") > 0 Then
            Call smartedudownless(TextBox1.Text)
            Exit Sub
        End If
        If InStr(TextBox1.Text.ToLower, "lessonid=") > 0 Then
            Call smartedudownless(TextBox1.Text)
            Exit Sub
        End If
        If InStr(TextBox1.Text.ToLower, "contentid=") > 0 Then
            Call smartedudown(TextBox1.Text)
            Exit Sub
        End If
        MessageBox.Show("不支持下载当前链接。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Exit Sub
    End Sub
    '文本去重函数
    Function rmoe(ByVal s As String)
        Dim a, i, j, k
        Dim b()
        a = Split(s, vbCrLf)
        For i = 0 To UBound(a) - 1
            For j = i + 1 To UBound(a)
                If a(i) = a(j) Then a(j) = ""
            Next
        Next
        For i = 0 To UBound(a)
            If a(i) <> "" Then
                ReDim Preserve b(k)
                b(k) = a(i)
                k = k + 1
            End If
        Next
        rmoe = b
    End Function
    '资源包下载
    Sub smartedudownless(ByVal BookLink As String)
        'On Error Resume Next

        'Link Exp.
        'https://basic.smartedu.cn/syncClassroom/classActivity?activityId=e5a2847c-5ebb-481c-ab60-ede38a992ca5&chapterId=d2f0c245-97f5-3dd2-be40-765ac83b61d4&teachingmaterialId=0c0a0241-11f9-4b79-83d9-e8715f9c1573&fromPrepare=0&classHourId=lesson_1
        'https://basic.smartedu.cn/qualityCourse?courseId=8ae7e48f-842c-12fc-0184-35dacdee016f&chapterId=8ae5c0d4-cfd4-34d1-9757-0295bd0c55ed&teachingmaterialId=4a4aa279-8dc6-4098-b45f-dd3f7d5a61b2&fromPrepare=0&classHourId=lesson_1
        Dim k As Integer
        k = InStr(BookLink.ToLower, "activityid=")
        Dim m As Integer
        m = InStr(BookLink.ToLower, "courseid=")
        Dim j As Integer
        j = InStr(BookLink.ToLower, "lessonid=")
        If k <= 0 Then
            If m <= 0 Then
                If j <= 0 Then
                    MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        If k > 0 Then
            k = k + 10
        End If

        If m > 0 Then
            k = m + 8
        End If

        If j > 0 Then
            k = j + 8
        End If

        Dim bookid As String = BookLink.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
        If Len(bookid) < 36 Then
            MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim booknameurl As String
        '从官方服务器获取资源信息
        booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrv2/national_lesson/resources/details/" & bookid & ".json"
        If m > 0 Then
            booknameurl = "https://s-file-2.ykt.cbern.com.cn/zxx/ndrv2/resources/" & bookid & ".json"
        ElseIf j > 0 Then
            booknameurl = "https://s-file-2.ykt.cbern.com.cn/zxx/ndrv2/prepare_lesson/resources/details/" & bookid & ".json"
        End If
        Dim bookinforeq As String

        bookinforeq = GetSource(booknameurl)
        If bookinforeq = "" Then
            MessageBox.Show("获取资源包信息失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        '解析Json信息
        ' Try
        Dim BookInfoObject As JObject = JObject.Parse(bookinforeq)

            Dim BookNameObject As JObject = BookInfoObject("global_title")
            DownBookName = CStr(BookNameObject("zh-CN")) '标题
            Dim BookIDGet As String = BookInfoObject("id") 'id
            Dim BookTagArray As JArray = BookInfoObject("tag_list")
            Dim BookTag As String = ""
            For i = 0 To BookTagArray.Count - 1
                BookTag = BookTag & BookTagArray(i)("tag_name").ToString & " " '标签
            Next

            Dim BookResObj1 As JObject = BookInfoObject("relations")
            Dim BookResObj2 As JArray
            Dim BookResObj3 As JArray = Nothing
            Dim BookResObj4 As JArray = Nothing
            If m > 0 Then
                BookResObj2 = BookResObj1("course_resource")
            ElseIf j > 0
                BookResObj2 = BookResObj1("teaching_assets")
                BookResObj3 = BookResObj1("lesson_plan_design")
                BookResObj4 = BookResObj1("classroom_record")
            Else
                BookResObj2 = BookResObj1("national_course_resource")
            End If

        'Dim BookItemsObject As JArray '= BookInfoObject("un_ti_items")

        Dim BookResInfo(BookResObj2.Count - 1) As String
            TagsSet.ListBox1.Items.Clear()

            TextBox2.Text = ""



            Dim DownBookLink01 As String = ""
            Dim DownBookLink02 As String = ""
            Dim DownBookLink03 As String = ""

        If j > 0 Then
            DownBookLink01 = Join(findlist(BookResObj2, BookResInfo, 1), vbCrLf)
            DownBookLink02 = Join(findlist(BookResObj3, BookResInfo, 1), vbCrLf)
            DownBookLink03 = Join(findlist(BookResObj4, BookResInfo, 1), vbCrLf)
        Else
            DownBookLink01 = Join(findlist(BookResObj2, BookResInfo, 0), vbCrLf)
        End If
        Dim DownBookLink0() As String
        If j > 0 Then
            If DownBookLink03 = "" And Not DownBookLink02 = "" Then
                DownBookLink0 = Split(DownBookLink01 & vbCrLf & DownBookLink02, vbCrLf)
            ElseIf DownBookLink02 = "" And Not DownBookLink03 = "" Then
                DownBookLink0 = Split(DownBookLink01 & vbCrLf & DownBookLink03, vbCrLf)
            ElseIf Not DownBookLink02 = "" And Not DownBookLink03 = "" Then
                DownBookLink0 = Split(DownBookLink01 & vbCrLf & DownBookLink02 & vbCrLf & DownBookLink03, vbCrLf)
            Else
                DownBookLink0 = Split(DownBookLink01, vbCrLf)
            End If
        Else
            DownBookLink0 = Split(DownBookLink01, vbCrLf)
        End If


            '去除多余的空格分隔符
            Dim c As String
            c = Join(DownBookLink0, "  ")
            c = Replace(c, "   ", " ")
            c = Replace(c, "  ", " ")
            c = Replace(c, "   ", " ")
            c = Replace(c, "  ", " ")

            DownBookLinks = Split(c, " ")
            Dim am As Integer = 0
            Dim bm As Integer = 0

            'Dim DownBookLink1(DownBookLinks.Count - 1) As String
            'For i = 0 To DownBookLinks.Count - 1
            '    MsgBox(DownBookLinks(i))
            '    If DownBookLinks(i).Length = 0 Then
            '        If DownBookLinks(i + 1).Length = 0 Then
            '            If DownBookLinks(i + 2).Length = 0 Then
            '                If DownBookLinks(i + 3).Length = 0 Then
            '                    DownBookLink1(i - am) = DownBookLinks(i + 4)
            '                    bm = 4
            '                Else
            '                    DownBookLink1(i - am) = DownBookLinks(i + 3)
            '                    bm = 3
            '                End If
            '            Else
            '                DownBookLink1(i - am) = DownBookLinks(i + 2)
            '                bm = 2
            '            End If
            '        Else
            '            DownBookLink1(i - am) = DownBookLinks(i + 1)
            '            bm = 1
            '        End If
            '        am = am + bm
            '    Else
            '        DownBookLink1(i - am) = DownBookLinks(i)
            '    End If
            'Next
            '链接列表去除多余换行符
            Dim DownBookLink1 As String = ""
            For i = 0 To DownBookLinks.Count - 1
                If DownBookLinks(i).Length <> 0 Then
                    DownBookLink1 = DownBookLink1 & DownBookLinks(i) & vbCrLf
                End If
            Next
            DownBookLink1 = DownBookLink1.Substring(0, DownBookLink1.Length - 2)
            DownBookLinks = Split(DownBookLink1, vbCrLf)
            DownBookLink1 = ""
            For i = 0 To DownBookLinks.Count - 1
                If DownBookLinks(i).Length <> 0 Then
                    DownBookLink1 = DownBookLink1 & DownBookLinks(i) & vbCrLf
                End If
            Next
            DownBookLink1 = DownBookLink1.Substring(0, DownBookLink1.Length - 2)
        DownBookLinks = Split(DownBookLink1, vbCrLf)

        '处理显示的下载链接，使用去重函数处理重复项
        Dim a
            a = rmoe(TextBox2.Text)
            TextBox2.Text = Join(a, vbCrLf)

            '获取文件格式，但有些项没有这个值，会出错
            'Dim BookResFormat(BookResObj2.Count - 1) As String
            'For i = 0 To BookResObj2.Count - 1
            '    MsgBox(BookResObj2(i)("custom_properties")("format").ToString)
            '    BookResFormat(i) = BookResObj2(i)("custom_properties")("format").ToString
            'Next

            '获取封面链接
            DownBookImgLink = CStr(((BookInfoObject("custom_properties"))("thumbnails"))(0))

            '设置信息
            BookNameLabel.Text = "资源包名称：" & DownBookName
            BookIDLabel.Text = "资源包ID：" & BookIDGet
            BookTagLabel.Text = "资源包标签：" & BookTag

            '针对资源包程序布局显示优化
            PictureBox1.Location = New Point(405, 184)
            PictureBox1.Size = New Point(266, 150)
            Button2.Text = "保存资源信息"
            Button3.Text = "保存资源包"
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            Button5.Enabled = True
        ' Catch ex As Exception
        ' MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
        '下载封面
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
    '获取链接并过滤jpg、json等无用文件
    Function findlist(ByVal BookResObj2, ByVal BookResInfo, ByVal mode)
        Dim DownBookLink0(BookResObj2.Count - 1) As String
        For i = 0 To BookResObj2.Count - 1
            If mode = 0 Then
                BookResInfo(i) = BookResObj2(i)("custom_properties")("alias_name").ToString
            Else
                BookResInfo(i) = BookResObj2(i)("global_label")("zh-CN")(0).ToString
            End If
            If BookResInfo(i) = "视频课程" Or BookResInfo(i) = "微课视频" Or BookResInfo(i) = "课堂实录" Then
                Dim lt As String = ""
                For e = 0 To BookResObj2(i)("ti_items").Count - 1
                    Dim fsq() As String = Split(BookResObj2(i)("ti_items")(e)("ti_storages")(0), "/")
                    If fsq(fsq.Count - 1) = "html" Then
                    ElseIf fsq(fsq.Count - 1) = "thumbnail" Then
                    ElseIf fsq(fsq.Count - 1) = "image" Then
                    ElseIf Split(fsq(fsq.Count - 1), ".")(Split(fsq(fsq.Count - 1), ".").Count - 1) = "jpg" Then
                    ElseIf Split(fsq(fsq.Count - 1), ".")(Split(fsq(fsq.Count - 1), ".").Count - 1) = "json" Then
                    Else
                        'If Not fsq(fsq.Count - 1) = "html" Or fsq(fsq.Count - 1) = "thumbnail" Or fsq(fsq.Count - 1) = "image" Or Split(fsq(fsq.Count - 1), ".")(Split(fsq(fsq.Count - 1), ".").Count - 1) = ".jpg" Or Split(fsq(fsq.Count - 1), ".")(Split(fsq(fsq.Count - 1), ".").Count - 1) = ".json" Then
                        lt = lt & BookResObj2(i)("ti_items")(e)("ti_storages")(0).ToString & vbCrLf
                        If DownloadMode = 1 Then
                            lt = Replace(lt, "r1-ndr-private", "r1-ndr")
                        End If
                    End If
                Next
                TextBox2.Text = TextBox2.Text & BookResInfo(i) & " M3U8 下载链接：" & vbCrLf & lt
            Else
                Dim lt As String = ""
                For e = 0 To BookResObj2(i)("ti_items").Count - 1
                    Dim fsq() As String = Split(BookResObj2(i)("ti_items")(e)("ti_storages")(0), "/")
                    If fsq(fsq.Count - 1) = "html" Then
                    ElseIf fsq(fsq.Count - 1) = "thumbnail" Then
                    ElseIf fsq(fsq.Count - 1) = "image" Then
                    ElseIf Split(fsq(fsq.Count - 1), ".")(Split(fsq(fsq.Count - 1), ".").Count - 1) = "jpg" Then
                    ElseIf Split(fsq(fsq.Count - 1), ".")(Split(fsq(fsq.Count - 1), ".").Count - 1) = "json" Then
                    Else
                        'If Not fsq(fsq.Count - 1) = "html" Or fsq(fsq.Count - 1) = "thumbnail" Or fsq(fsq.Count - 1) = "image" Or Split(fsq(fsq.Count - 1), ".")(Split(fsq(fsq.Count - 1), ".").Count - 1) = ".jpg" Or Split(fsq(fsq.Count - 1), ".")(Split(fsq(fsq.Count - 1), ".").Count - 1) = ".json" Then
                        lt = lt & BookResObj2(i)("ti_items")(e)("ti_storages")(0).ToString & vbCrLf
                        If DownloadMode = 1 Then
                            lt = Replace(lt, "r1-ndr-private", "r1-ndr")
                        End If
                        DownBookLink0(i) = lt
                        If mode = 0 Then
                            TagsSet.ListBox1.Items.AddRange({BookResObj2(i)("global_title")("zh-CN").ToString & " " & BookResObj2(i)("custom_properties")("alias_name").ToString})
                        Else
                            TagsSet.ListBox1.Items.AddRange({BookResObj2(i)("global_title")("zh-CN").ToString & " " & BookResObj2(i)("global_label")("zh-CN")(0).ToString})
                        End If
                    End If
                Next
                TextBox2.Text = TextBox2.Text & BookResInfo(i) & "下载链接：" & vbCrLf & lt
            End If
        Next
        Return DownBookLink0
    End Function

    '教材下载
    Sub smartedudown(ByVal BookLink As String)
        'On Error Resume Next

        'Link Exp.
        'https://basic.smartedu.cn/tchMaterial/detail?contentType=assets_document&contentId=1e04e52e-41df-4689-b12a-f3b7315a4243&catalogType=tchMaterial&subCatalog=tchMaterial
        'Books Link
        'https://r1-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/bdc00134-465d-454b-a541-dcd0cec4d86e.pkg/pdf.pdf
        'https://r2-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/bdc00134-465d-454b-a541-dcd0cec4d86e.pkg/pdf.pdf
        'https://r3-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/bdc00134-465d-454b-a541-dcd0cec4d86e.pkg/pdf.pdf
        '解析链接
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
        '从官方服务器获取资源信息
        booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrv2/resources/tch_material/details/" & bookid & ".json"
        Dim bookinforeq As String

        bookinforeq = GetSource(booknameurl)
        If bookinforeq = "" Then
            MessageBox.Show("获取电子书信息失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        '处理Json文件
        Try
            Dim BookInfoObject As JObject = JObject.Parse(bookinforeq)

            Dim BookNameObject As JObject = BookInfoObject("global_title")
            DownBookName = CStr(BookNameObject("zh-CN")) '标题
            Dim BookIDGet As String = BookInfoObject("id") 'id
            Dim BookTagArray As JArray = BookInfoObject("tag_list")
            Dim BookTag As String = ""
            For i = 0 To BookTagArray.Count - 1
                BookTag = BookTag & BookTagArray(i)("tag_name").ToString & " " '标签
            Next

            Dim BookItemsObject As JArray = BookInfoObject("ti_items")

            'Dim BookDownLinkPri As String = CStr((BookItemsObject(1)("ti_storages"))(0)) & vbCrLf & CStr((BookItemsObject(1)("ti_storages"))(1)) & vbCrLf & CStr((BookItemsObject(1)("ti_storages"))(2))
            Dim BookDownLinkPri As String = ""
            Dim BookDownLinkPriArr As JArray = BookItemsObject(1)("ti_storages")
            '获取下载链接显示
            For i = 0 To BookDownLinkPriArr.Count - 1
                If i = BookDownLinkPriArr.Count - 1 Then
                    BookDownLinkPri = BookDownLinkPri & BookDownLinkPriArr(i).ToString
                Else
                    BookDownLinkPri = BookDownLinkPri & BookDownLinkPriArr(i).ToString & vbCrLf
                End If
            Next
            '如果没有登录则替换私域链接到公域
            If DownloadMode = 1 Then
                Dim BookDownLink As String = Replace(BookDownLinkPri, "ndr-private.ykt.cbern.com.cn", "ndr.ykt.cbern.com.cn")
                TextBox2.Text = BookDownLink
            Else
                TextBox2.Text = BookDownLinkPri
            End If

            '获取下载链接（程序下载）
            Dim DownBookLinkPri As String = CStr((BookItemsObject(1)("ti_storages"))(0))
            If DownloadMode = 1 Then
                DownBookLink = Replace(DownBookLinkPri, "ndr-private.ykt.cbern.com.cn", "ndr.ykt.cbern.com.cn")
            Else
                DownBookLink = DownBookLinkPri
            End If

            '获取封面链接
            'Dim BookPreviewLinkPri As String = CStr((BookItemsObject(3)("ti_storages"))(0))
            'DownBookImgLink = Replace(BookPreviewLinkPri, "ndr-private.ykt.cbern.com.cn", "ndr.ykt.cbern.com.cn")
            DownBookImgLink = CStr(((BookInfoObject("custom_properties"))("thumbnails"))(0))

            '设置信息
            BookNameLabel.Text = "书籍名称：" & DownBookName
            BookIDLabel.Text = "书籍ID：" & BookIDGet
            BookTagLabel.Text = "书籍标签：" & BookTag
            '针对教材的程序页面优化
            Button2.Text = "保存书籍信息"
            Button3.Text = "保存电子书"
            PictureBox1.Location = New Point(487, 122)
            PictureBox1.Size = New Point(180, 240)
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            Button5.Enabled = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        '下载封面
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
    '获取Json内容函数
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
            httpReq.Timeout = 15000
            httpReq.Method = "GET"
            httpResp = CType(httpReq.GetResponse(), HttpWebResponse)
            'Dim reader As StreamReader = New StreamReader(httpResp.GetResponseStream, System.Text.Encoding.GetEncoding("GB2312")) '如是中文，要设置编码格式为“GB2312”。
            Dim reader As StreamReader = New StreamReader(httpResp.GetResponseStream, System.Text.Encoding.UTF8)
            Dim respHTML As String = reader.ReadToEnd() 'respHTML就是网页源代码
            Return respHTML
            httpResp.Close()
        Catch e As Exception
            'MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return e.Message
        End Try
    End Function
    '获取Json内容函数（方法2）
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
            'MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return e.Message
        End Try
    End Function
    '保存电子书按钮
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        '保存教材
        If Button3.Text = "保存电子书" Then
            '初始化保存对话框
            SaveFileDialog1.Filter = "PDF 文件(*.pdf)|*.pdf"
            SaveFileDialog1.FileName = DownBookName & ".pdf"
            If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '初始化下载器
                DownFormvb.Label1.Text = "正在下载 0%" & vbCrLf & "已经下载 0 MB，共 0 MB"
                DownFormvb.Button1.Text = "取消"
                'DownFormvb.Button1.Visible = False
                DownFormvb.st = 0
                DownFormvb.ProgressBar1.Value = 0
                '登录模式添加标头
                If DownloadMode = 0 Then
                    DownloadClient.Headers.Set("x-nd-auth", XNdAuth)
                End If
                '开始下载
                Try
                    DownloadClient.DownloadFileAsync(New Uri(DownBookLink), SaveFileDialog1.FileName)
                Catch ex As Exception
                    DownFormvb.Label1.Text = ex.Message
                End Try
                DownFormvb.ShowDialog() '显示下载进度
            End If
        Else
            '保存资源包
            TagsSet.ListBox1.SelectedIndex = 0
            TagsSet.ShowDialog()
            If TagsSet.ec = 1 Then
                '初始化保存对话框
                '处理扩展名
                Dim fn2() As String = Nothing
                If DownBookLinks(TagsSet.ListBox1.SelectedIndex).Substring(DownBookLinks(TagsSet.ListBox1.SelectedIndex).Length - 1, 1) = "/" Then
                    fn2(0) = ""
                Else
                    fn2 = Split(DownBookLinks(TagsSet.ListBox1.SelectedIndex), ".")
                    fn2(fn2.Count - 1) = Replace(fn2(fn2.Count - 1), "/", "_")
                End If
            If fn2(fn2.Count - 1) = "" Then
                    SaveFileDialog1.Filter = "文件(*.*)|*.*"
                    SaveFileDialog1.FileName = TagsSet.ListBox1.SelectedItem.ToString
                Else
                    SaveFileDialog1.Filter = fn2(fn2.Count - 1).ToUpper & " 文件(*." & fn2(fn2.Count - 1) & ")|*." & fn2(fn2.Count - 1)
                    SaveFileDialog1.FileName = TagsSet.ListBox1.SelectedItem.ToString & "." & fn2(fn2.Count - 1)
                End If
                If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        '初始化下载器
                        DownFormvb.Label1.Text = "正在下载 0%" & vbCrLf & "已经下载 0 MB，共 0 MB"
                        DownFormvb.Button1.Text = "取消"
                        'DownFormvb.Button1.Visible = False
                        DownFormvb.st = 0
                        DownFormvb.ProgressBar1.Value = 0
                        '登录模式添加标头
                        If DownloadMode = 0 Then
                            DownloadClient.Headers.Set("x-nd-auth", XNdAuth)
                        End If
                        '开始下载
                        Try
                            DownloadClient.DownloadFileAsync(New Uri(DownBookLinks(TagsSet.ListBox1.SelectedIndex)), SaveFileDialog1.FileName)
                        Catch ex As Exception
                            DownFormvb.Label1.Text = ex.Message
                        End Try
                        DownFormvb.ShowDialog() '显示下载进度
                    End If
                End If
            End If
    End Sub
    'Private Sub ShowDownProgress(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
    'Invoke(New Action(Of Integer)(Sub(i) DownFormvb.ProgressBar1.Value = i), e.ProgressPercentage)
    'Invoke(New Action(Of Integer)(Sub(i) DownFormvb.Label1.Text = "正在下载 " & i & "%"), e.ProgressPercentage)
    'End Sub
    '下载进度同步
    Private Sub DownloadClient_DownloadProgressChanged(ByVal sender As Object, ByVal e As System.Net.DownloadProgressChangedEventArgs)
        DownFormvb.ProgressBar1.Value = e.ProgressPercentage
        DownFormvb.Label1.Text = "正在下载 " & e.ProgressPercentage & "%" & vbCrLf & "已经下载 " & Int(e.BytesReceived / 1024 / 1024).ToString & " MB，共 " & Int(e.TotalBytesToReceive / 1024 / 1024).ToString & " MB"
    End Sub
    '下载状态处理
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
    '保存封面
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
    '保存信息
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        SaveFileDialog1.Filter = "文本文件(*.txt)|*.txt"
        SaveFileDialog1.FileName = DownBookName & "_" & Format(Now, "yyyy-MM-dd-HH-mm-ss") & ".txt"
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            '处理重复文件
            If IO.File.Exists(SaveFileDialog1.FileName) Then
                Try
                    IO.File.Delete(SaveFileDialog1.FileName)
                Catch ex As Exception
                    MessageBox.Show("写入文件错误，存在同名文件。" & vbCrLf & ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End Try
            End If
            '写入文本
            Dim fs As New System.IO.FileStream(SaveFileDialog1.FileName, IO.FileMode.Create)
            fs.Close()
            Dim fs2 As New IO.StreamWriter(SaveFileDialog1.FileName)
            fs2.Write(vbCrLf)
            fs2.Write(BookNameLabel.Text)
            fs2.Write(vbCrLf & BookIDLabel.Text)
            fs2.Write(vbCrLf & BookTagLabel.Text)
            fs2.Write(vbCrLf & "下载链接：")
            fs2.Write(vbCrLf & TextBox2.Text)
            fs2.Close()
        End If
    End Sub
    '链接批量解析
    Private Sub readmm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readmm.Click
        MGetForm.Show()
    End Sub
    '链接批量解析下载
    Private Sub downmm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles downmm.Click
        MDownForm.Show()
    End Sub
    '帮助
    Private Sub hlpdocm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hlpdocm.Click
        '两种模式判断
        If DownloadMode = 1 Then
            System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/SmartEduDownloader/Help/index.html")
        Else
            System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/SmartEduDownloader/Help/login.html")
        End If
    End Sub

    Private Sub ofpm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ofpm.Click
        System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/SmartEduDownloader/index.html")
    End Sub
    '（免）登录模式切换
    Private Sub logmm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles logmm.Click
        If DownloadMode = 1 Then
            '运行自己加参数
            System.Diagnostics.Process.Start(Application.ExecutablePath, "/loginmode")
            Me.Close()
        Else
            System.Diagnostics.Process.Start(Application.ExecutablePath)
            Me.Close()
        End If
    End Sub
    '关于
    Private Sub aboutm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles aboutm.Click
        AboutForm.ShowDialog()
    End Sub
    '设置Xa标头信息
    Private Sub sxam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sxam.Click
        SetXaForm.ShowDialog()
    End Sub
    '批量下载链接
    Private Sub dllinm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dllinm.Click
        DownLinkForm.Show()
    End Sub
    '文本合并
    Private Sub txtcnvm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtcnvm.Click
        TextCnv.Show()
    End Sub
    '文本分割
    Private Sub txtspm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtspm.Click
        TextSp.Show()
    End Sub
    '寻找教材
    Private Sub Fbooksm_Click(sender As System.Object, e As System.EventArgs) Handles Fbooksm.Click
        System.Diagnostics.Process.Start("https://basic.smartedu.cn/tchMaterial")
    End Sub
    '寻找资源包
    Private Sub findlessm_Click(sender As System.Object, e As System.EventArgs) Handles findlessm.Click
        System.Diagnostics.Process.Start("https://basic.smartedu.cn/syncClassroom")
    End Sub
    '保存官方服务器上的Json信息文件
    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        Dim booknameurl As String
        If InStr(TextBox1.Text.ToLower, "activityid=") > 0 Then
            Dim k As Integer
            k = InStr(TextBox1.Text.ToLower, "activityid=")
            If k <= 0 Then
                MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            k = k + 10
            Dim bookid As String = TextBox1.Text.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
            If Len(bookid) < 36 Then
                MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrv2/national_lesson/resources/details/" & bookid & ".json"
        ElseIf InStr(TextBox1.Text.ToLower, "courseid=") > 0 Then
            Dim k As Integer
            k = InStr(TextBox1.Text.ToLower, "courseid=")
            If k <= 0 Then
                MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            k = k + 8
            Dim bookid As String = TextBox1.Text.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
            If Len(bookid) < 36 Then
                MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            booknameurl = "https://s-file-2.ykt.cbern.com.cn/zxx/ndrv2/resources/" & bookid & ".json"
        ElseIf InStr(TextBox1.Text.ToLower, "lessonid=") > 0 Then
            Dim k As Integer
            k = InStr(TextBox1.Text.ToLower, "lessonid=")
            If k <= 0 Then
                MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            k = k + 8
            Dim bookid As String = TextBox1.Text.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
            If Len(bookid) < 36 Then
                MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            booknameurl = "https://s-file-2.ykt.cbern.com.cn/zxx/ndrv2/prepare_lesson/resources/details/" & bookid & ".json"
        Else
            Dim k As Integer
            k = InStr(TextBox1.Text.ToLower, "contentid=")
            If k <= 0 Then
                MessageBox.Show("链接解析失败。无法获取BookID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            k = k + 9
            Dim bookid As String = TextBox1.Text.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
            If Len(bookid) < 36 Then
                MessageBox.Show("链接解析失败。无法获取BookID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrv2/resources/tch_material/details/" & bookid & ".json"
        End If
        SaveFileDialog1.Filter = "Json 文件(*.json)|*.json"
        SaveFileDialog1.FileName = DownBookName & "_" & Format(Now, "yyyy-MM-dd-HH-mm-ss") & ".json"
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            DownFormvb.Label1.Text = "正在下载 0%" & vbCrLf & "已经下载 0 MB，共 0 MB"
            DownFormvb.Button1.Text = "取消"
            'DownFormvb.Button1.Visible = False
            DownFormvb.st = 0
            DownFormvb.ProgressBar1.Value = 0
            Try
                DownloadClient.DownloadFileAsync(New Uri(booknameurl), SaveFileDialog1.FileName)
            Catch ex As Exception
                DownFormvb.Label1.Text = ex.Message
            End Try
            DownFormvb.ShowDialog()
        End If


        ''If InStr(TextBox1.Text, "contentId=") > 0 Then Call smartedudown(TextBox1.Text)
    End Sub
    '显示公告栏
    Private Sub noticem_Click(sender As System.Object, e As System.EventArgs) Handles noticem.Click
        NoticeForm.Text = NtTi
        NoticeForm.TextBox1.Text = NtTx
        NoticeForm.Show()
    End Sub
End Class
