'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：MainForm.vb
'描述：主程序部分（解析下载资源）
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
Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Microsoft.Win32

Public Class MainForm
    Dim DownBookLink As String '电子课本链接
    Dim DownBookLinks() As String '资源包链接集合
    Dim DownBookAudioLinks() As String '课本音频集合
    Dim DownBookImgLink As String '封面图片链接
    Dim DownBookName As String '资源名称

    Dim NtTi As String '公告标题
    Dim NtTx As String '公告内容

    Public DownloadMode As Integer '下载模式 0=登录 1=免登录

    Public AGetNotice As Integer '是否获取公告 0=不获取 1=获取
    Public AGetUpdate As Integer '是否自动获取更新 0=不获取 1=获取
    Public BootModeTip As Integer

    Public DownloadClient As New WebClientPro '下载器对象

    Public DownloadWinState As Integer '下载窗口处理

    Public XNdAuth As String 'X-Nd-Auth 标头

    'Public DisbMsg As Integer = 0

    Public scaleX As Single 'DPI X
    Public scaleY As Single 'DPI Y
    '多线程获取公告
    Dim NoticeThread As New Threading.Thread(AddressOf GetNotice)
    Delegate Sub HideNoticeMenu()
    '多线程获取更新
    Delegate Sub GetUpdateForm(ByVal app32link As String, ByVal app64link As String, ByVal text As String, ByVal focupdate As Integer)
    Delegate Sub MessageBoxForm(ByVal title As String, ByVal text As String, ByVal buttom As MessageBoxButtons, ByVal icon As MessageBoxIcon)
    Delegate Sub MessageBoxErrForm(ByVal ErrText As String, ByVal ErrTitle As String, ByVal ShowFeedBack As Boolean)
    'Delegate Sub DownUpdateForm(ByVal link As String, ByVal path As String)

    '程序版本信息
    Public MyArch As String
    Public Const AppBuildTime As String = "20250316"
    Public AppBuildChannel As String = My.Resources.AppBuildChannel
    Public Const AppBuildNumber As Integer = 5
 
    '初始化
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BootModeTip = 1
        DownloadWinState = 0
        '解决无法建立安全的TLS/SSL连接问题
        ServicePointManager.SecurityProtocol = CType(192, SecurityProtocolType) Or CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)
        ' 获取当前窗体的 DPI
        Dim currentDpiX As Single = Me.CreateGraphics().DpiX
        Dim currentDpiY As Single = Me.CreateGraphics().DpiY

        'If currentDpiX <> 96 OrElse currentDpiY <> 96 Then
        'End If
        '计算缩放比例
        scaleX = currentDpiX / 96
        scaleY = currentDpiY / 96

        'DPI控件微调
        'TableLayoutPanel1.Height = TableLayoutPanel1.Height * scaleY
        '程序架构判断
        If Environment.Is64BitProcess = True Then
            MyArch = "x64"
        Else
            MyArch = "x86"
        End If

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

        '初始化
        DownloadClient.Timeout = 30000
        FolderBrowserDialog1.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\BookDownloads"
        DownBookLink = ""
        DownBookName = ""
        DownBookImgLink = ""
        AddHandler DownloadClient.DownloadProgressChanged, AddressOf DownloadClient_DownloadProgressChanged
        AddHandler DownloadClient.DownloadFileCompleted, AddressOf DownloadClient_DownloadFileCompleted
        'AddHandler DownloadClient.DownloadDataCompleted, AddressOf DownloadClient_DownloadFileCompleted
        Try
            AddKey("Software\CJH", "HKCU")
            AddKey("Software\CJH\FlyEduDownloader", "HKCU")
        Catch ex As Exception
        End Try
        Dim mykey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\CJH\FlyEduDownloader", True)
        Dim myt As Integer
        Try
            If (Not mykey Is Nothing) Then
                myt = mykey.GetValue("AcceptLicense", -1)
                If myt < 0 Then
                    myt = 0
                    AddReg("Software\CJH\FlyEduDownloader", "AcceptLicense", 0, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
                ElseIf myt > 1 Then
                    myt = 0
                    AddReg("Software\CJH\FlyEduDownloader", "AcceptLicense", 0, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
                End If
            Else
                myt = 0
                AddReg("Software\CJH\FlyEduDownloader", "AcceptLicense", 0, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
            End If
        Catch ex As Exception
            myt = 0
            AddReg("Software\CJH\FlyEduDownloader", "AcceptLicense", 0, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
        End Try

        Try
            If (Not mykey Is Nothing) Then
                AGetNotice = mykey.GetValue("GetNotice", -1)
                If AGetNotice < 0 Then
                    AGetNotice = 1
                    AddReg("Software\CJH\FlyEduDownloader", "GetNotice", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
                ElseIf AGetNotice > 1 Then
                    AGetNotice = 1
                    AddReg("Software\CJH\FlyEduDownloader", "GetNotice", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
                End If
            Else
                AGetNotice = 1
                AddReg("Software\CJH\FlyEduDownloader", "GetNotice", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
            End If
        Catch ex As Exception
            AGetNotice = 1
            AddReg("Software\CJH\FlyEduDownloader", "GetNotice", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
        End Try

        Dim xndbase64 As String
        Dim xndent As String
        Try
            If (Not mykey Is Nothing) Then
                xndbase64 = mykey.GetValue("LoginState", "")
                Try
                    Dim Str As String = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(xndbase64))
                    xndent = Str
                    If xndent = "" Then
                        DelReg("Software\CJH\FlyEduDownloader", "LoginState", "HKCU")
                    End If
                Catch ex As Exception
                    xndent = ""
                    DelReg("Software\CJH\FlyEduDownloader", "LoginState", "HKCU")
                End Try
            Else
                xndent = ""
                DelReg("Software\CJH\FlyEduDownloader", "LoginState", "HKCU")
            End If
        Catch ex As Exception
            xndent = ""
            DelReg("Software\CJH\FlyEduDownloader", "LoginState", "HKCU")
        End Try


        Try
            If (Not mykey Is Nothing) Then
                AGetUpdate = mykey.GetValue("AutoGetUpdate", -1)
                If AGetUpdate < 0 Then
                    AGetUpdate = 1
                    AddReg("Software\CJH\FlyEduDownloader", "AutoGetUpdate", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
                ElseIf AGetUpdate > 1 Then
                    AGetUpdate = 1
                    AddReg("Software\CJH\FlyEduDownloader", "AutoGetUpdate", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
                End If
            Else
                AGetUpdate = 1
                AddReg("Software\CJH\FlyEduDownloader", "AutoGetUpdate", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
            End If
        Catch ex As Exception
            AGetUpdate = 1
            AddReg("Software\CJH\FlyEduDownloader", "AutoGetUpdate", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
        End Try

        If (Not mykey Is Nothing) Then
            mykey.Close()
        End If

        If myt = 0 Then
            LicenseForm.ShowDialog()
        End If

        'Dim cmds As String()
        'cmds = Split(Command.ToLower, " ")
        'Dim sett As Integer
        '登录模式设置
        If Command.ToLower.Contains("/unloginmode") = True Then
            Me.Text = "飞翔教学资源助手 " & My.Application.Info.Version.ToString & " (" & AppBuildTime & ") " & MyArch & " " & AppBuildChannel & " （免登录下载模式）"
            DownloadMode = 1
            sxam.Visible = False
            logmm.Text = "使用登录模式下载(&L)"
            Label3.Visible = False
            'sett = 1
        Else
            Me.Text = "飞翔教学资源助手 " & My.Application.Info.Version.ToString & " (" & AppBuildTime & ") " & MyArch & " " & AppBuildChannel & " （登录下载模式）"
            DownloadMode = 0
            sxam.Visible = True
            logmm.Text = "使用免登录模式下载(&L)"
            Label3.Visible = True
            Label1.Text = "下载链接：（可能存在多个下载链接，如果手动下载选择一个能用的粘贴到的实用工具-下载链接菜单下载即可）"
            If xndent <> "" Then
                SetXaForm.TempXa = xndent
                XNdAuth = xndent
                SetXaForm.Button5.Visible = False
            Else
                SetXaForm.Button4.Text = "退出"
                SetXaForm.StartPosition = FormStartPosition.CenterScreen
                SetXaForm.ShowDialog()
            End If
        End If
        If Not Command.ToLower.Contains("/noupdates") = True Then
            If AGetUpdate = 1 Then
                Dim GetUpdateThread As New Threading.Thread(AddressOf GetUpdate)
                GetUpdateThread.Start()
            End If
        End If
        If AGetNotice = 1 Then
            NoticeThread.Start()
        Else
            noticem.Visible = False
        End If
        BootModeTip = 0
    End Sub
    '自定义错误对话框
    Public Sub MessageBoxError(ByVal ErrText As String, ByVal ErrTitle As String, ByVal ShowFeedBack As Boolean)
        ErrMsgForm.Text = ErrTitle
        ErrMsgForm.TextBox1.Text = ErrText
        If ShowFeedBack = True Then
            ErrMsgForm.Button2.Visible = True
        Else
            ErrMsgForm.Button2.Visible = False
        End If
        ErrMsgForm.ShowDialog()
    End Sub

    '多线程MessageBox
    Sub MessageBoxThread(ByVal title As String, ByVal text As String, ByVal buttom As MessageBoxButtons, ByVal icon As MessageBoxIcon)
        MessageBox.Show(text, title, buttom, icon)
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'End
        'Application.ExitThread()
        'Application.Exit()
        System.Environment.Exit(0)
        'NoticeThread.Interrupt()
        'NoticeThread.Join()
    End Sub

    '下载接口
    'Public Sub DownSub(ByVal link As String, ByVal path As String)
    '    '开始下载
    '    Try
    '        'DownloadClient.DownloadFileAsync(New Uri(link), path)
    '        MsgBox(UpdateForm.UpdateLink32)
    '        DownloadClient.DownloadFileAsync(New Uri("https://gitee.com/cjhdevact/FlyEduDownloader/releases/download/1.0.4.24101/FlyEduDownloader_1.0.4.24101_x86_setup.exe"), Application.StartupPath & "\FlyEduDownUpdatePack.exe")
    '    Catch ex As Exception
    '        'DownFormvb.Label1.Text = ex.Message
    '        MessageBoxError("下载错误" & vbCrLf & ex.Message, "飞翔教学资源助手 - 错误", False)
    '    End Try
    '    DownFormvb.ShowDialog() '显示下载进度
    'End Sub

    'Public Sub DownUpdate(ByVal link As String, ByVal path As String)
    '    DownSub(link, path)
    'End Sub
    Sub HideNoticeMenu1()
        noticem.Visible = False
    End Sub
    '获取公告
    Sub GetNotice()
        Try
            Dim ntic As String
            ntic = GetSource(My.Resources.NoticeMsg)
            Dim reget As String = ""
            Try
                Dim NoticeObject As JObject = JObject.Parse(ntic)
                reget = CStr(NoticeObject("rurl"))
                If reget <> "" Then
                    Exit Try
                End If
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
                Me.Invoke(New HideNoticeMenu(AddressOf HideNoticeMenu1))
            End Try
            If reget <> "" Then
                Try
                    Dim ntic1 As String = ""
                    ntic1 = GetSource(reget)
                    Dim NoticeObject As JObject = JObject.Parse(ntic1)
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
                    Me.Invoke(New HideNoticeMenu(AddressOf HideNoticeMenu1))
                End Try
            End If
        Catch ex As Threading.ThreadInterruptedException
        End Try
    End Sub

    '获取更新
    Function GetUpdate()
        '版本检查
        Dim verf As String
        Dim verstr As String = ""
        Dim vernum As Integer
        If System.Environment.OSVersion.Version.Major >= 6 And System.Environment.OSVersion.Version.Minor >= 1 Then
            verf = GetSource(My.Resources.UpdateVer)
        Else
            verf = GetSource(My.Resources.UpdateVerSp)
        End If
        If verf = "" Then
            Return (2)
            Exit Function
        End If
        Try
            Dim NoticeObject As JObject = JObject.Parse(verf)
            vernum = CInt(NoticeObject("verm"))
            verstr = CStr(NoticeObject("ver"))
        Catch ex As Exception
        End Try
        Dim needupt As Boolean
        If vernum > AppBuildNumber Then
            needupt = True
        Else
            needupt = False
        End If
        '获取更新信息
        If needupt = True Then
            Dim veri As String
            If System.Environment.OSVersion.Version.Major >= 6 And System.Environment.OSVersion.Version.Minor >= 1 Then
                veri = GetSource(My.Resources.UpdateInfo)
            Else
                veri = GetSource(My.Resources.UpdateInfoSp)
            End If
            If veri = "" Then
                Return (2)
                Exit Function
            End If
            Dim app64link As String = ""
            Dim app32link As String = ""
            Dim updateinfo As String = ""
            Dim focupdate As Integer
            Dim rurl As String = ""
            Try
                Dim VerObject As JObject = JObject.Parse(veri)

                rurl = CStr(VerObject("rurl"))
                If rurl <> "" Then
                    Exit Try
                End If

                app32link = CStr(VerObject("up32"))
                app64link = CStr(VerObject("up64"))
                Dim InfoText As JArray = VerObject("upt")
                For i = 0 To InfoText.Count - 1
                    If i = InfoText.Count - 1 Then
                        updateinfo = updateinfo & InfoText(i).ToString
                    Else
                        updateinfo = updateinfo & InfoText(i).ToString & vbCrLf
                    End If
                Next
                'updateinfo = CStr(VerObject("updateinfo"))
                focupdate = CStr(VerObject("uf"))
                'verstr = CStr(NoticeObject("ver"))
            Catch ex As Exception
            End Try
            '获取更新信息（重定向）
            If rurl <> "" Then
                Dim veri2 As String
                veri2 = GetSource(rurl)
                If veri2 = "" Then
                    Return (2)
                    Exit Function
                End If
                Try
                    Dim VerObject As JObject = JObject.Parse(veri2)
                    app32link = CStr(VerObject("up32"))
                    app64link = CStr(VerObject("up64"))

                    Dim InfoText As JArray = VerObject("upt")
                    For i = 0 To InfoText.Count - 1
                        If i = InfoText.Count - 1 Then
                            updateinfo = updateinfo & InfoText(i).ToString
                        Else
                            updateinfo = updateinfo & InfoText(i).ToString & vbCrLf
                        End If
                    Next
                    'updateinfo = CStr(VerObject("updateinfo"))
                    focupdate = CStr(VerObject("uf"))
                    'verstr = CStr(NoticeObject("ver"))
                Catch ex As Exception
                End Try
            End If
            Dim verin As String
            verin = "版本：" & verstr & vbCrLf & "更新内容：" & vbCrLf & updateinfo
            'If focupdate = 1 Then
            '    UpdateForm.Button2.Text = "退出"
            'End If
            'UpdateForm.UpdateLink32 = app32link
            'UpdateForm.UpdateLink64 = app64link
            Me.Invoke(New GetUpdateForm(AddressOf ShowUpdateForm), app32link, app64link, verin, focupdate)
            'UpdateForm.ShowDialog()
            Return (0)
        Else
            '    If BootModeTip = 0 Then
            '        MessageBox.Show("当前已是最新版本。", "更新", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    End If
            Return (1)
        End If
    End Function

    Sub GetUpdate2()
        Dim a As Integer
        a = GetUpdate()
        If a = 1 Then
            'MessageBox.Show("当前已是最新版本。", "更新", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Invoke(New MessageBoxForm(AddressOf MessageBoxThread), "飞翔教学资源助手 - 程序更新", "当前已是最新版本。", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf a = 2 Then
            Me.Invoke(New MessageBoxErrForm(AddressOf MessageBoxError), "获取更新失败！", "飞翔教学资源助手 - 错误", True)
        End If
    End Sub

    Sub ShowUpdateForm(ByVal app32link As String, ByVal app64link As String, ByVal text As String, ByVal focupdate As Integer)
        UpdateForm.TextBox1.Text = text
        If focupdate = 1 Then
            UpdateForm.Button2.Text = "退出"
        End If
        UpdateForm.UpdateLink32 = app32link
        UpdateForm.UpdateLink64 = app64link
        UpdateForm.ShowDialog()
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
            MessageBoxError("页面链接不能为空！", "飞翔教学资源助手 - 错误", False)
            'MessageBox.Show("页面链接不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            If CheckBox2.Checked = True Then
                Call bookaudioresdown(TextBox1.Text)
            Else
                Call smartedudown(TextBox1.Text)
            End If
            Exit Sub
        End If
        'MessageBox.Show("不支持下载当前链接。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        MessageBoxError("不支持下载当前链接。", "飞翔教学资源助手 - 错误", False)
        Exit Sub
    End Sub
    '下载教材听力
    Sub bookaudioresdown(ByVal BookLink As String)
        'Exp.
        'https://s-file-1.ykt.cbern.com.cn/zxx/ndrs/resources/d199388d-afe7-4ff2-912d-cea6ac4b9b1b/relation_audios.json
        '解析链接
        Dim k As Integer
        k = InStr(BookLink, "contentId=")
        If k <= 0 Then
            MessageBoxError("链接解析失败。无法获取BookID。", "飞翔教学资源助手 - 错误", True)
            'MessageBox.Show("链接解析失败。无法获取BookID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        k = k + 9
        Dim bookid As String = BookLink.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
        If Len(bookid) < 36 Then
            'MessageBox.Show("链接解析失败。无法获取BookID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            MessageBoxError("链接解析失败。无法获取BookID。", "飞翔教学资源助手 - 错误", True)
            Exit Sub
        End If
        Dim booknameurl As String
        booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrs/resources/" & bookid & "/relation_audios.json"
        Dim bookinforeq As String
        bookinforeq = GetSource2(booknameurl)
        If bookinforeq = "" Then
            'MessageBox.Show("获取电子书信息失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            MessageBoxError("未获取电子书对应的听力音频。", "飞翔教学资源助手 - 错误", True)
            Exit Sub
        End If
        '处理Json文件
        Try
            Dim BookInfoObject As JArray
            BookInfoObject = JArray.Parse(bookinforeq)
            If BookInfoObject.Count = 0 Then
                MessageBoxError("未获取电子书对应的听力音频。", "飞翔教学资源助手 - 错误", False)
                Exit Sub
            End If
            Dim AudioListName(BookInfoObject.Count - 1) As String
            For i = 0 To BookInfoObject.Count - 1
                AudioListName(i) = BookInfoObject(i)("global_title")("zh-CN").ToString
            Next

            Dim AudioListUrl(BookInfoObject.Count - 1) As String
            For i = 0 To BookInfoObject.Count - 1
                AudioListUrl(i) = BookInfoObject(i)("ti_items")(0)("ti_storages")(0).ToString
            Next
            DownBookAudioLinks = AudioListUrl
            'MsgBox(Join(AudioListName, vbCrLf))
            'MsgBox(Join(AudioListUrl, vbCrLf))
            TagsSet.ListBox1.Items.Clear()
            For i = 0 To BookInfoObject.Count - 1
                TagsSet.ListBox1.Items.AddRange({AudioListName(i)})
            Next

        Catch ex As Exception
            MessageBoxError(ex.Message, "飞翔教学资源助手 - 错误", True)
            Exit Sub
        End Try

        TagsSet.Label1.Text = "请选择你要下载的内容。"
        TagsSet.ListBox1.SelectedIndex = 0
        TagsSet.ShowDialog()
        If TagsSet.ec = 1 Then
            '初始化保存对话框
            'If fn2(fn2.Count - 1) = "" Then
            '    SaveFileDialog1.Filter = "文件(*.*)|*.*"
            '    SaveFileDialog1.FileName = TagsSet.ListBox1.SelectedItem.ToString
            'Else
            '    SaveFileDialog1.Filter = fn2(fn2.Count - 1).ToUpper & " 文件(*." & fn2(fn2.Count - 1) & ")|*." & fn2(fn2.Count - 1)
            '    SaveFileDialog1.FileName = TagsSet.ListBox1.SelectedItem.ToString & "." & fn2(fn2.Count - 1)
            'End If
            If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                'If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '初始化下载器
                'DownFormvb.Button1.Visible = False
                '登录模式添加标头
                If DownloadMode = 0 Then
                    DownloadClient.Headers.Set("x-nd-auth", XNdAuth)
                End If
                Dim ii As Integer = 0
                '开始下载
                For Each item As Integer In TagsSet.ListBox1.CheckedIndices
                    '处理扩展名
                    Dim fn2() As String = Nothing
                    If DownBookAudioLinks(item).Substring(DownBookAudioLinks(item).Length - 1, 1) = "/" Then
                        fn2(0) = ""
                    Else
                        fn2 = Split(DownBookAudioLinks(item), ".")
                        fn2(fn2.Count - 1) = Replace(fn2(fn2.Count - 1), "/", "_")
                    End If
                    ii = ii + 1
                    If ii = TagsSet.ListBox1.CheckedIndices.Count Then
                        DownloadWinState = 0
                    Else
                        DownloadWinState = 1
                    End If
                    Try
                        DownFormvb.st = 0
                        DownFormvb.ProgressBar1.Value = 0
                        DownFormvb.Label1.Text = "正在下载" & vbCrLf & "已下载 0%" & vbCrLf & "已经下载 0 MB，共 0 MB"
                        DownFormvb.Button1.Text = "取消"
                        DownFormvb.Button1.Text = "取消"
                        Threading.Thread.Sleep(500)
                        'DownloadClient.DownloadFileAsync(New Uri(DownBookLinks(TagsSet.ListBox1.SelectedIndex)), SaveFileDialog1.FileName)
                        Dim dfn As String
                        dfn = TagsSet.ListBox1.Items(item).ToString & "." & fn2(fn2.Count - 1)
                        '处理文件名，去除非法字符\/:*?"<>|
                        Dim ffn() As String
                        ffn = Split(dfn, "\")
                        ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "\", "_")
                        ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "/", "_")
                        ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), ":", "-")
                        ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "*", "-")
                        ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "?", "")
                        ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), """", "")
                        ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "<", "")
                        ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), ">", "")
                        ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "|", "_")
                        ffn(ffn.Length - 1) = EnsureValidFileName(ffn(ffn.Length - 1))
                        dfn = Join(ffn, "\")
                        'fn = MainForm.EnsureValidFileName(fn)
                        DownloadClient.DownloadFileAsync(New Uri(DownBookAudioLinks(item)), FolderBrowserDialog1.SelectedPath & "\" & dfn, ii & "/" & TagsSet.ListBox1.CheckedIndices.Count & " " & TagsSet.ListBox1.Items(item).ToString & "." & fn2(fn2.Count - 1))
                    Catch ex As Exception
                        DownFormvb.Label1.Text = ex.Message
                    End Try
                    DownFormvb.ShowDialog() '显示下载进度
                Next
                'End If
                DownloadWinState = 0
            End If
        End If
    End Sub

    '文本去重函数
    Function rmoe(ByVal s As String)
        Dim a, i, j, k
        Dim b()
        k = Nothing
        b = Nothing
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
                    'MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    MessageBoxError("链接解析失败。无法获取资源包ID。", "飞翔教学资源助手 - 错误", True)
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
            'MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            MessageBoxError("链接解析失败。无法获取资源包ID。", "飞翔教学资源助手 - 错误", True)
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
            'MessageBox.Show("获取资源包信息失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            MessageBoxError("获取资源包信息失败。", "飞翔教学资源助手 - 错误", True)
            Exit Sub
        End If
        '解析Json信息
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

            Dim BookResObj1 As JObject = BookInfoObject("relations")
            Dim BookResObj2 As JArray
            Dim BookResObj3 As JArray = Nothing
            Dim BookResObj4 As JArray = Nothing
            If m > 0 Then
                BookResObj2 = BookResObj1("course_resource")
            ElseIf j > 0 Then
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
            PictureBox1.Location = New Point(405 * scaleX, 184 * scaleY)
            PictureBox1.Size = New Point(266 * scaleX, 150 * scaleY)
            Button2.Text = "保存资源信息"
            Button3.Text = "保存资源包"
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            Button5.Enabled = True
        Catch ex As Exception
            MessageBoxError(ex.Message, "飞翔教学资源助手 - 错误", True)
            'MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        '下载封面
        Try
            Dim whttpReq As System.Net.HttpWebRequest 'HttpWebRequest 类对 WebRequest 中定义的属性和方法提供支持'，也对使用户能够直接与使用 HTTP 的服务器交互的附加属性和方法提供支持。
            Dim whttpURL As New System.Uri(DownBookImgLink)
            whttpReq = CType(WebRequest.Create(whttpURL), HttpWebRequest)
            With whttpReq
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
                .UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36"
            End With
            whttpReq.Timeout = 30000
            whttpReq.Method = "GET"
            Dim res As WebResponse = whttpReq.GetResponse()
            Dim shi As New Bitmap(res.GetResponseStream)
            PictureBox1.Image = Nothing
            PictureBox1.Image = shi
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            MessageBoxError(ex.Message, "飞翔教学资源助手 - 错误", True)
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
        'https://basic.smartedu.cn/tchMaterial/detail?contentType=thematic_course&contentId=2afcdb56-6fce-8c99-0bc9-e9dd33b5c51c&catalogType=tchMaterial&subCatalog=tchMaterial
        'Books Link
        'https://r1-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/bdc00134-465d-454b-a541-dcd0cec4d86e.pkg/pdf.pdf
        'https://r2-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/bdc00134-465d-454b-a541-dcd0cec4d86e.pkg/pdf.pdf
        'https://r3-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/bdc00134-465d-454b-a541-dcd0cec4d86e.pkg/pdf.pdf
        'https://r1-ndr-doc-private.ykt.cbern.com.cn/edu_product/esp/assets/11446212-4b7b-4094-afe3-bd5b4f2b3f0c.pkg/义务教育教科书 道德与法治 七年级 上册_1725097530934.pdf
        '解析链接
        Dim k As Integer
        k = InStr(BookLink, "contentId=")
        If k <= 0 Then
            MessageBoxError("链接解析失败。无法获取BookID。", "飞翔教学资源助手 - 错误", True)
            'MessageBox.Show("链接解析失败。无法获取BookID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        k = k + 9
        Dim bookid As String = BookLink.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
        If Len(bookid) < 36 Then
            'MessageBox.Show("链接解析失败。无法获取BookID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            MessageBoxError("链接解析失败。无法获取BookID。", "飞翔教学资源助手 - 错误", True)
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

        bookinforeq = GetSource2(booknameurl)

        If bookinforeq = "" Then
            'MessageBox.Show("获取电子书信息失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            MessageBoxError("获取电子书信息失败。", "飞翔教学资源助手 - 错误", True)
            Exit Sub
        End If
        '处理Json文件
        Try
            Dim BookInfoObject As JObject
            If bookinforeq.Substring(0, 3).Contains("[") = True Then
                BookInfoObject = JArray.Parse(bookinforeq)(0)
            Else
                BookInfoObject = JObject.Parse(bookinforeq)
            End If

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

            Dim BookDownLinkPriArr As JArray = Nothing

            For i = 0 To BookItemsObject.Count - 1
                If BookItemsObject(i)("ti_format") = "pdf" Then
                    BookDownLinkPriArr = BookItemsObject(i)("ti_storages")
                    Exit For
                End If
            Next



            '获取下载链接（程序下载）
            Dim DownBookLinkPri As String = ""
            For i = 0 To BookItemsObject.Count - 1
                If BookItemsObject(i)("ti_format") = "pdf" Then
                    DownBookLinkPri = CStr((BookItemsObject(i)("ti_storages"))(0)) '获取官方教材链接
                    Exit For
                End If
            Next
            Dim bb() As String
            bb = Split(DownBookLinkPri, "/")
            If CheckBox1.Checked = True Then '强制获取老版教材
                bb(bb.Length - 1) = "pdf.pdf"
            End If

            If CheckBox3.Checked = True Then '强制获取旧链接教材
                DownBookLinkPri = "https://r1-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/" & bookid & ".pkg/" & bb(bb.Length - 1)
            Else
                DownBookLinkPri = Join(bb, "/")
            End If

            'If CheckBox1.Checked = True And CheckBox3.Checked = True Then '强制获取老版教材
            '    DownBookLinkPri = "https://r1-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/" & bookid & ".pkg/pdf.pdf"
            'ElseIf CheckBox1.Checked = True And CheckBox3.Checked = False Then '强制获取旧链接教材
            '    Dim aa As String = ""
            '    For i = 0 To BookItemsObject.Count - 1
            '        If BookItemsObject(i)("ti_format") = "pdf" Then
            '            aa = CStr((BookItemsObject(i)("ti_storages"))(0)) '获取官方教材链接
            '            Exit For
            '        End If
            '    Next
            '    Dim bb() As String
            '    bb = Split(aa, "/")
            '    DownBookLinkPri = "https://r1-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/" & bookid & ".pkg/" & bb(bb.Length - 1)
            'Else
            '    For i = 0 To BookItemsObject.Count - 1
            '        If BookItemsObject(i)("ti_format") = "pdf" Then
            '            DownBookLinkPri = CStr((BookItemsObject(i)("ti_storages"))(0)) '获取官方教材链接
            '            Exit For
            '        End If
            '    Next
            'End If


            If DownloadMode = 1 Then
                DownBookLink = Replace(DownBookLinkPri, "ndr-private.ykt.cbern.com.cn", "ndr.ykt.cbern.com.cn")
            Else
                DownBookLink = DownBookLinkPri
            End If
            Dim dwns As String()
            Dim GetOlds As Integer = 0
            dwns = Split(DownBookLink, "/")
            'If DownBookName.Contains("根据2022年版课程标准修订") = True Then
            '    'If MessageBox.Show("该链接为新课标教材链接，但可能存在旧版本教材版本。" & vbCrLf & "如果选择下载旧版本教材失败，说明对应的旧版本教材可能已被删除。" & vbCrLf & vbCrLf & "如果要下载新课标版本教材请点击""是""，如果要下载旧版本教材请点击""否""。", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.No Then
            '    If CheckBox1.Checked = True Then
            '        dwns(dwns.Count - 1) = "pdf.pdf"
            '        GetOlds = 1
            '        DownBookLink = Join(dwns, "/")
            '    Else
            '        GetOlds = 0
            '    End If
            'Else
            '    GetOlds = 0
            'End If
            If CheckBox1.Checked = True Then
                dwns(dwns.Count - 1) = "pdf.pdf"
                GetOlds = 1
                DownBookLink = Join(dwns, "/")
            Else
                GetOlds = 0
            End If


            For i = 0 To BookDownLinkPriArr.Count - 1
                Dim cc() As String
                cc = Split(BookDownLinkPriArr(i), "/")
                'BookDownLinkPriArr(i) = "https://r" & i + 1 & "-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/" & bookid & ".pkg/pdf.pdf"
                If CheckBox1.Checked = True Then '强制获取老版教材
                    cc(cc.Length - 1) = "pdf.pdf"
                End If
                If CheckBox3.Checked = True Then '强制获取旧链接教材
                    BookDownLinkPriArr(i) = "https://r" & i + 1 & "-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/" & bookid & ".pkg/" & bb(bb.Length - 1)
                Else
                    BookDownLinkPriArr(i) = Join(cc, "/")
                End If
            Next
            '获取下载链接显示
            For i = 0 To BookDownLinkPriArr.Count - 1
                'Dim ad As String()
                'ad = Split(BookDownLinkPriArr(i).ToString, "/")
                'If GetOlds = 1 Then
                '    ad(ad.Count - 1) = "pdf.pdf"
                'End If
                'Dim newurl As String
                'newurl = Join(ad, "/")
                If i = BookDownLinkPriArr.Count - 1 Then
                    'If DownBookName.Contains("根据2022年版课程标准修订") = True Then
                    '    If GetOlds = 1 Then
                    '        BookDownLinkPri = BookDownLinkPri & newurl
                    '    Else
                    '        BookDownLinkPri = BookDownLinkPri & BookDownLinkPriArr(i).ToString
                    '    End If
                    'Else
                    BookDownLinkPri = BookDownLinkPri & BookDownLinkPriArr(i).ToString
                    'End If
                Else
                    'If DownBookName.Contains("根据2022年版课程标准修订") = True Then
                    '    If GetOlds = 1 Then
                    '        BookDownLinkPri = BookDownLinkPri & newurl & vbCrLf
                    '    Else
                    '        BookDownLinkPri = BookDownLinkPri & BookDownLinkPriArr(i).ToString & vbCrLf
                    '    End If
                    'Else
                    BookDownLinkPri = BookDownLinkPri & BookDownLinkPriArr(i).ToString & vbCrLf
                    'End If
                End If
            Next

            '如果没有登录则替换私域链接到公域
            If DownloadMode = 1 Then
                Dim BookDownLink As String = Replace(BookDownLinkPri, "ndr-private.ykt.cbern.com.cn", "ndr.ykt.cbern.com.cn")
                TextBox2.Text = BookDownLink
            Else
                TextBox2.Text = BookDownLinkPri
            End If

            '获取封面链接
            'Dim BookPreviewLinkPri As String = CStr((BookItemsObject(3)("ti_storages"))(0))
            'DownBookImgLink = Replace(BookPreviewLinkPri, "ndr-private.ykt.cbern.com.cn", "ndr.ykt.cbern.com.cn")
            DownBookImgLink = CStr(((BookInfoObject("custom_properties"))("thumbnails"))(0))

            If GetOlds = 1 Then
                DownBookName = Replace(DownBookName, "（根据2022年版课程标准修订）", "")
            End If

            '设置信息
            BookNameLabel.Text = "书籍名称：" & DownBookName
            BookIDLabel.Text = "书籍ID：" & BookIDGet
            BookTagLabel.Text = "书籍标签：" & BookTag
            '针对教材的程序页面优化
            Button2.Text = "保存书籍信息"
            Button3.Text = "保存电子书"
            PictureBox1.Location = New Point(487 * scaleX, 138 * scaleY)
            PictureBox1.Size = New Point(180 * scaleX, 240 * scaleY)
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            Button5.Enabled = True
        Catch ex As Exception
            MessageBoxError(ex.Message, "飞翔教学资源助手 - 错误", True)
            'MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        '下载封面
        Try
            Dim whttpReq As System.Net.HttpWebRequest 'HttpWebRequest 类对 WebRequest 中定义的属性和方法提供支持'，也对使用户能够直接与使用 HTTP 的服务器交互的附加属性和方法提供支持。
            Dim whttpURL As New System.Uri(DownBookImgLink)
            whttpReq = CType(WebRequest.Create(whttpURL), HttpWebRequest)
            whttpReq.Timeout = 30000
            With whttpReq
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
                .UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36"
            End With
            whttpReq.Method = "GET"
            Dim res As WebResponse = whttpReq.GetResponse()
            Dim shi As New Bitmap(res.GetResponseStream)
            PictureBox1.Image = Nothing
            PictureBox1.Image = shi
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            MessageBoxError(ex.Message, "飞翔教学资源助手 - 错误", True)
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
            With httpReq
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
                .UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36"
            End With
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
            Dim dfn As String
            dfn = DownBookName
            '处理文件名，去除非法字符\/:*?"<>|
            Dim ffn() As String
            ffn = Split(dfn, "\")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "\", "_")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "/", "_")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), ":", "-")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "*", "-")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "?", "")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), """", "")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "<", "")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), ">", "")
            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "|", "_")
            ffn(ffn.Length - 1) = EnsureValidFileName(ffn(ffn.Length - 1))
            dfn = Join(ffn, "\")
            'fn = MainForm.EnsureValidFileName(fn)
            SaveFileDialog1.FileName = dfn & ".pdf"
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
                    DownloadClient.DownloadFileAsync(New Uri(DownBookLink), SaveFileDialog1.FileName, DownBookName)
                Catch ex As Exception
                    DownFormvb.Label1.Text = ex.Message
                End Try
                DownFormvb.ShowDialog() '显示下载进度
            End If
        Else
            '保存资源包
            TagsSet.Label1.Text = "请选择你要下载的内容，有多个相同选项是因为从官网获取到的信息包含重复项，选择一个即可，有些是空地址，请自行辨别。" & vbCrLf _
                    & "暂时不支持下载M3U8视频, 如果要下载, 请手动复制地址使用M3U8下载器（如N_m3u8DL-CLI）下载。"
            TagsSet.ListBox1.SelectedIndex = 0
            TagsSet.ShowDialog()
            If TagsSet.ec = 1 Then
                '初始化保存对话框
                'If fn2(fn2.Count - 1) = "" Then
                '    SaveFileDialog1.Filter = "文件(*.*)|*.*"
                '    SaveFileDialog1.FileName = TagsSet.ListBox1.SelectedItem.ToString
                'Else
                '    SaveFileDialog1.Filter = fn2(fn2.Count - 1).ToUpper & " 文件(*." & fn2(fn2.Count - 1) & ")|*." & fn2(fn2.Count - 1)
                '    SaveFileDialog1.FileName = TagsSet.ListBox1.SelectedItem.ToString & "." & fn2(fn2.Count - 1)
                'End If
                If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    'If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    '登录模式添加标头
                    If DownloadMode = 0 Then
                        DownloadClient.Headers.Set("x-nd-auth", XNdAuth)
                    End If
                    Dim ii As Integer = 0
                    '开始下载
                    For Each item As Integer In TagsSet.ListBox1.CheckedIndices
                        '处理扩展名
                        Dim fn2() As String = Nothing
                        If DownBookLinks(item).Substring(DownBookLinks(item).Length - 1, 1) = "/" Then
                            fn2(0) = ""
                        Else
                            fn2 = Split(DownBookLinks(item), ".")
                            fn2(fn2.Count - 1) = Replace(fn2(fn2.Count - 1), "/", "_")
                        End If
                        ii = ii + 1
                        If ii = TagsSet.ListBox1.CheckedIndices.Count Then
                            DownloadWinState = 0
                        Else
                            DownloadWinState = 1
                        End If
                        'MsgBox(DownBookLinks(item))
                        'MsgBox(TagsSet.ListBox1.Items(item).ToString & "." & fn2(fn2.Count - 1))
                        Try
                            DownFormvb.st = 0
                            DownFormvb.ProgressBar1.Value = 0
                            DownFormvb.Label1.Text = "正在下载" & vbCrLf & "已下载 0%" & vbCrLf & "已经下载 0 MB，共 0 MB"
                            DownFormvb.Button1.Text = "取消"
                            Threading.Thread.Sleep(500)
                            Dim dfn As String
                            dfn = TagsSet.ListBox1.Items(item).ToString & "." & fn2(fn2.Count - 1)
                            '处理文件名，去除非法字符\/:*?"<>|
                            Dim ffn() As String
                            ffn = Split(dfn, "\")
                            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "\", "_")
                            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "/", "_")
                            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), ":", "-")
                            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "*", "-")
                            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "?", "")
                            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), """", "")
                            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "<", "")
                            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), ">", "")
                            ffn(ffn.Length - 1) = Replace(ffn(ffn.Length - 1), "|", "_")
                            ffn(ffn.Length - 1) = EnsureValidFileName(ffn(ffn.Length - 1))
                            dfn = Join(ffn, "\")
                            'fn = MainForm.EnsureValidFileName(fn)
                            'DownloadClient.DownloadFileAsync(New Uri(DownBookLinks(TagsSet.ListBox1.SelectedIndex)), SaveFileDialog1.FileName)
                            DownloadClient.DownloadFileAsync(New Uri(DownBookLinks(item)), FolderBrowserDialog1.SelectedPath & "\" & dfn, ii & "/" & TagsSet.ListBox1.CheckedIndices.Count & " " & TagsSet.ListBox1.Items(item).ToString & "." & fn2(fn2.Count - 1))
                        Catch ex As Exception
                            DownFormvb.Label1.Text = ex.Message
                        End Try
                        DownFormvb.ShowDialog() '显示下载进度
                    Next
                    DownloadWinState = 0
                    'End If
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
        'DownFormvb.Label1.Text = "正在下载 " & e.ProgressPercentage & "%" & vbCrLf & "已经下载 " & Int(e.BytesReceived / 1024 / 1024).ToString & " MB，共 " & Int(e.TotalBytesToReceive / 1024 / 1024).ToString & " MB"
        DownFormvb.Label1.Text = "正在下载 " & e.UserState & vbCrLf & "已下载 " & e.ProgressPercentage & "%" & vbCrLf & "已经下载 " & Int(e.BytesReceived / 1024 / 1024).ToString & " MB，共 " & Int(e.TotalBytesToReceive / 1024 / 1024).ToString & " MB"
    End Sub
    '下载状态处理
    Private Sub DownloadClient_DownloadFileCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        If e.Error IsNot Nothing Then
            If IO.File.Exists(SaveFileDialog1.FileName) Then
                Try
                    IO.File.Delete(SaveFileDialog1.FileName)
                Catch ex As Exception
                End Try
            End If
            If e.Error.Message.ToString.Contains("404") Then
                DownFormvb.Label1.Text = "下载失败 " & e.UserState & vbCrLf & "找不到对应的资源。" & e.Error.Message
            ElseIf e.Error.Message.ToString.Contains("401") Then
                DownFormvb.Label1.Text = "下载失败 " & e.UserState & vbCrLf & "登录状态无效，请尝试重新登录。" & e.Error.Message
            Else
                DownFormvb.Label1.Text = e.Error.Message
            End If
            DownFormvb.Button1.Text = "确定"
            'If DownloadWinState = 1 Then
            '    DownFormvb.Close()
            'End If
            'DownFormvb.Button1.Visible = True
            'If DisbMsg = 1 Then
            '    DownFormvb.Close()
            'End If
        ElseIf e.Cancelled = True Then
            If IO.File.Exists(SaveFileDialog1.FileName) Then
                Try
                    IO.File.Delete(SaveFileDialog1.FileName)
                Catch ex As Exception
                End Try
            End If
            DownFormvb.Label1.Text = e.UserState & vbCrLf & "下载已被取消。"
            DownFormvb.Button1.Text = "确定"
            'DownFormvb.Button1.Visible = True
            'If DisbMsg = 1 Then
            '    DownFormvb.Close()
            'End If
        Else
            DownFormvb.Label1.Text = e.UserState & vbCrLf & "下载完成！"
            DownFormvb.Button1.Text = "确定"
            'DownFormvb.Button1.Visible = True
            'If DisbMsg = 1 Then
            '    DownFormvb.Close()
            'End If
            If DownloadWinState = 1 Then
                DownFormvb.Close()
            End If
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
                DownloadClient.DownloadFileAsync(New Uri(DownBookImgLink), SaveFileDialog1.FileName, DownBookName & "_" & Format(Now, "yyyy-MM-dd-HH-mm-ss") & ".jpg")
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
                    'MessageBox.Show("写入文件错误，存在同名文件。" & vbCrLf & ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    MessageBoxError("写入文件错误，存在同名文件。" & vbCrLf & ex.Message, "飞翔教学资源助手 - 错误", False)
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

    '合法文件名函数
    Public Function CreateValidFileName(ByVal originalName As String) As String
        Dim invalidChars As String = New String(Path.GetInvalidFileNameChars()) ' 获取所有非法字符
        Dim newName As String = originalName

        ' 移除非法字符
        For Each c As Char In invalidChars
            newName = newName.Replace(c, "")
        Next

        ' 确保文件名不是空的或者全是空格
        If String.IsNullOrWhiteSpace(newName) Then
            newName = "DefaultFileName" ' 如果原名称无效，则使用默认名称
        End If

        Return newName
    End Function
    '截取文件名函数
    Function EnsureValidFileName(ByVal fileName As String) As String
        Dim maxLength As Integer = 255
        If fileName.Length > maxLength Then
            ' 截断文件名，但不包括扩展名
            Dim extension As String = Path.GetExtension(fileName)
            Dim baseNameLength As Integer = maxLength - extension.Length
            If baseNameLength <= 0 Then
                'Throw New ArgumentException("文件扩展名过长。")
            End If
            Dim baseName As String = Path.GetFileNameWithoutExtension(fileName)
            If baseName.Length > baseNameLength Then
                baseName = baseName.Substring(0, baseNameLength)
            End If
            fileName = baseName & extension
        End If
        Return fileName
    End Function

    '链接批量解析
    Private Sub readmm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readmm.Click
        MGetForm.Show()
    End Sub
    '链接批量解析下载
    Private Sub downmm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles downmm.Click
        MDownForm.Show()
    End Sub
    '帮助
    Private Sub hlpdocm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Hlpdocm.Click
        '两种模式判断
        If DownloadMode = 1 Then
            System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/FlyEduDownloader/Help/index.html")
        Else
            System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/FlyEduDownloader/Help/login.html")
        End If
    End Sub

    Private Sub ofpm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ofpm.Click
        System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/FlyEduDownloader/index.html")
    End Sub
    '（免）登录模式切换
    Private Sub logmm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles logmm.Click
        If DownloadMode = 0 Then
            If MessageBox.Show("免登录模式只能下载旧版链接教材或课程，即勾选了强制使用旧版链接选项，如果要下载最新教材，请使用登录模式下载。" & vbCrLf & "如果免登录模式无法下载，请使用登录模式下载。" & vbCrLf & "是否要使用免登录模式？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                '运行自己加参数
                System.Diagnostics.Process.Start(Application.ExecutablePath, "/unloginmode /noupdates")
                Me.Close()
            End If
        Else
            System.Diagnostics.Process.Start(Application.ExecutablePath, "/noupdates")
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
    Private Sub Fbooksm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Fbooksm.Click
        System.Diagnostics.Process.Start("https://basic.smartedu.cn/tchMaterial")
    End Sub
    '寻找资源包
    Private Sub findlessm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles findlessm.Click
        System.Diagnostics.Process.Start("https://basic.smartedu.cn/syncClassroom")
    End Sub
    '保存官方服务器上的Json信息文件
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim booknameurl As String
        If InStr(TextBox1.Text.ToLower, "activityid=") > 0 Then
            Dim k As Integer
            k = InStr(TextBox1.Text.ToLower, "activityid=")
            If k <= 0 Then
                'MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                MessageBoxError("链接解析失败。无法获取资源包ID。", "飞翔教学资源助手 - 错误", True)
                Exit Sub
            End If
            k = k + 10
            Dim bookid As String = TextBox1.Text.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
            If Len(bookid) < 36 Then
                'MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                MessageBoxError("链接解析失败。无法获取资源包ID。", "飞翔教学资源助手 - 错误", True)
                Exit Sub
            End If
            booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrv2/national_lesson/resources/details/" & bookid & ".json"
        ElseIf InStr(TextBox1.Text.ToLower, "courseid=") > 0 Then
            Dim k As Integer
            k = InStr(TextBox1.Text.ToLower, "courseid=")
            If k <= 0 Then
                'MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                MessageBoxError("链接解析失败。无法获取资源包ID。", "飞翔教学资源助手 - 错误", True)
                Exit Sub
            End If
            k = k + 8
            Dim bookid As String = TextBox1.Text.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
            If Len(bookid) < 36 Then
                'MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                MessageBoxError("链接解析失败。无法获取资源包ID。", "飞翔教学资源助手 - 错误", True)
                Exit Sub
            End If
            booknameurl = "https://s-file-2.ykt.cbern.com.cn/zxx/ndrv2/resources/" & bookid & ".json"
        ElseIf InStr(TextBox1.Text.ToLower, "lessonid=") > 0 Then
            Dim k As Integer
            k = InStr(TextBox1.Text.ToLower, "lessonid=")
            If k <= 0 Then
                'MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                MessageBoxError("链接解析失败。无法获取资源包ID。", "飞翔教学资源助手 - 错误", True)
                Exit Sub
            End If
            k = k + 8
            Dim bookid As String = TextBox1.Text.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
            If Len(bookid) < 36 Then
                'MessageBox.Show("链接解析失败。无法获取资源包ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                MessageBoxError("链接解析失败。无法获取资源包ID。", "飞翔教学资源助手 - 错误", True)
                Exit Sub
            End If
            booknameurl = "https://s-file-2.ykt.cbern.com.cn/zxx/ndrv2/prepare_lesson/resources/details/" & bookid & ".json"
        Else
            Dim k As Integer
            k = InStr(TextBox1.Text.ToLower, "contentid=")
            If k <= 0 Then
                'MessageBox.Show("链接解析失败。无法获取BookID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                MessageBoxError("链接解析失败。无法获取BookID。", "飞翔教学资源助手 - 错误", True)
                Exit Sub
            End If
            k = k + 9
            Dim bookid As String = TextBox1.Text.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
            If Len(bookid) < 36 Then
                'MessageBox.Show("链接解析失败。无法获取BookID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                MessageBoxError("链接解析失败。无法获取BookID。", "飞翔教学资源助手 - 错误", True)
                Exit Sub
            End If
            '从官方服务器获取资源信息
            If TextBox1.Text.Contains("thematic_course") = True Then '资源包教材
                booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrs/special_edu/thematic_course/" & bookid & "/resources/list.json"
            Else
                '普通教材
                booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrv2/resources/tch_material/details/" & bookid & ".json"
            End If
            'booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrv2/resources/tch_material/details/" & bookid & ".json"
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
    Private Sub noticem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles noticem.Click
        NoticeForm.Text = NtTi
        NoticeForm.TextBox1.Text = NtTx
        NoticeForm.Show()
    End Sub
    'PDF转图片
    Private Sub PDFToPicm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PDFToPicm.Click
        PDFToImg.Show()
    End Sub
    '设置
    Private Sub settingm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles settingm.Click
        SettingForm.ShowDialog()
    End Sub
    '检查更新
    Private Sub FindUpdatem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles FindUpdatem.Click
        Dim GetUpdateThread As New Threading.Thread(AddressOf GetUpdate2)
        GetUpdateThread.Start()
    End Sub
    '反馈问题
    Private Sub feedbackm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles feedbackm.Click
        FeedBackForm.FeedBackInfo = ""
        FeedBackForm.ShowDialog()
    End Sub
    '帮助
    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        MessageBox.Show("你可以直接粘贴教材或资源包链接来解析，也可以通过输入ID的方式来下载。输入ID的方式是资源包输入""courseid=""或""activityid=""或""lessonid=""+资源包ID，教材输入""contentid=""+教材ID（没有引号，如果四个同时有，activityid>courseid>lessonid>contentid优先级），当教材页面的教材暂时下架，可以使用ID来下载。", "帮助", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    '使用旧版教材下载选项
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.CheckState = CheckState.Checked Then
            If CheckBox3.Checked = CheckState.Unchecked Then
                CheckBox3.Checked = CheckState.Checked
            End If
        End If
    End Sub

    '使用旧版教材下载选项
    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox1.CheckState = CheckState.Checked And CheckBox3.CheckState = CheckState.Unchecked Then
            If MessageBox.Show("如果取消强制使用旧版链接，将无法下载老版本教材。" & vbCrLf & "是否取消强制使用旧版链接？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                CheckBox1.Checked = CheckState.Unchecked
            Else
                CheckBox3.Checked = CheckState.Checked
            End If
        End If
    End Sub
End Class
