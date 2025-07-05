'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：WebLoginForm.vb
'描述：网页登录
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
Imports MB
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Runtime.InteropServices
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Text.RegularExpressions
Imports System.Security.Cryptography

Public Class WebLoginForm
    Public Webb As New WebView
    Public Xda As String
    Dim TmpPath As String

    <DllImport("Imm32.dll")>
    Private Shared Function ImmSimulateHotKey(ByVal hWnd As IntPtr, ByVal lKey As Integer) As Boolean
    End Function

    Private Const IME_CHOTKEY_SHAPE_TOGGLE As Integer = &H2B

    Private Sub WebLoginForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Interval = 500
        Timer1.Enabled = True
        Me.Text = "飞翔教学资源助手 - 登录"
        Label1.Visible = False
        Panel1.Visible = True
        ImmSimulateHotKey(Me.Handle, IME_CHOTKEY_SHAPE_TOGGLE) '把输入法切换成英语模式，避免输入密码输入中文
        Webb.Bind(Me.Panel1)
        'Webb.SetCookieJarPath("E:\cktest")
        TmpPath = System.IO.Path.GetTempPath()
        Webb.SetLocalStorageFullPath(TmpPath)
        'Webb.LoadHTML(My.Resources.String1)
        Webb.LoadURL("https://auth.smartedu.cn/uias/login")
        'Webb.LoadURL("https://www.smartedu.cn/")
        Xda = ""
        Webb.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36")
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Webb.GetURL = "https://www.smartedu.cn/" Then
            Webb.StopLoading()
            Timer1.Stop()
            Panel1.Visible = False
            Label1.Visible = True
            Me.ControlBox = False
            'Me.Hide()
            'AddHandler Webb.OnLoadUrlBegin, AddressOf OnLoadUrlBegin
            'AddHandler Webb.OnLoadUrlEnd, AddressOf OnLoadUrlEnd
            'AddHandler Webb.OnNetResponse, AddressOf OnNetResponse
            AddHandler Webb.OnLoadUrlFail, AddressOf OnLoadUrlFail
            ''这里参考了AnyTextbookDownloader项目的抓取登录信息原理（已废弃）
            ''Webb.LoadURL("https://basic.smartedu.cn/tchMaterial/detail?contentType=assets_document&contentId=1c73b348-e8b6-47d6-84b0-6dbacbe28268&catalogType=tchMaterial&subCatalog=tchMaterial")

            '通过解析cookie方式获取登录信息
            '感谢52beijixing/smartedu-download项目的x-nd-auth生成思路
            Dim filePath As String = TmpPath & "https_auth.smartedu.cn.localstorage"
            Dim lgcookies As String = ""
            Try
                lgcookies = System.IO.File.ReadAllText(filePath)
            Catch ex As Exception
                Call FailSub(0)
            End Try

            If lgcookies = "" Then Call FailSub(0)
            Dim lgcookiesi() As String
            Dim lgauth As String = ""
            lgcookiesi = Split(lgcookies, vbLf)
            For i = 0 To lgcookiesi.Length - 1
                If lgcookiesi(i).Contains("{""value"":""{\""source_token_account_type\""") = True Then
                    lgauth = lgcookiesi(i)
                End If
            Next
            If lgauth = "" Then Call FailSub(0)

            '去除多余cookie格式
            'lgauth = Replace(lgauth, """,""expire"":1752327172277}--mb-sep--", "")
            lgauth = Regex.Replace(lgauth, """,""expire"":(.*)}--mb-sep--", "")
            lgauth = Replace(lgauth, "{""value"":""", "")
            lgauth = Replace(lgauth, "\""", """") '恢复转义内容
            Dim CookieObj As JObject
            If lgauth.Substring(0, 3).Contains("[") = True Then
                CookieObj = JArray.Parse(lgauth)(0)
            Else
                CookieObj = JObject.Parse(lgauth)
            End If
            Dim lgaccess_token As String = "" 'access_token
            Dim lgmac_key As String = "" 'mac_key
            Dim lgmethod_type As String = "GET" 'method_type
            Dim lgdiff As String 'diff
            Dim lgurl As String
            Dim lgdiffget As New Random
            lgdiff = lgdiffget.Next(700, 900)
            Try
                lgaccess_token = CookieObj("access_token")
                lgmac_key = CookieObj("mac_key")
            Catch ex As Exception
            End Try
            lgurl = "https://basic.smartedu.cn/tchMaterial/detail?contentType=assets_document&contentId=1c73b348-e8b6-47d6-84b0-6dbacbe28268&catalogType=tchMaterial&subCatalog=tchMaterial"
            Xda = auth_encrypt(lgurl, lgaccess_token, lgmac_key, lgdiff, lgmethod_type) '生成请求头
        End If
        'Try '清理cookie文件
        '    'File.Delete(TmpPath & "https_auth.smartedu.cn.localstorage")
        '    'File.Delete(TmpPath & "https_turing.captcha.gtimg.com.localstorag")
        'Catch ex As Exception
        'End Try
        If Xda <> "" Then
            SetXaForm.TempXa = Xda
            Xda = ""
            Webb.StopLoading() '无法在OnLoadUrlBegin事件使用，会崩溃，只能通过定时器线程停止加载
            Label1.Visible = False
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub WebLoginForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If Label1.Visible = True Then
            e.Cancel = True
            'Else
            '    Webb.LoadHTML("")
        End If
    End Sub

    '生成一个基于HMAC-SHA256的认证签名字符串
    Function auth_encrypt(ByVal urln As String, ByVal access_token As String, ByVal mac_key As String, ByVal diff As String, ByVal method_type As String) As String
        '生成一个基于HMAC-SHA256的认证签名字符串。
        ':param url: 请求的URL。
        ':param access_token: 访问令牌。
        ':param mac_key: MAC密钥。
        ':param diff: 时间差值（整数）。
        ':return: 格式化的认证签名字符串。
        '（感谢52beijixing/smartedu-download项目的x-nd-auth生成思路）

        '获取当前时间的时间戳（毫秒）
        Dim current_time_ms As Integer = DateTime.Now.Second * 1000
        '将参数 diff 转换为整数
        Dim diff_int As Integer = Int(diff)
        '生成随机字符串
        Dim random_str As String = MakeRandomString(8)
        '拼接时间戳、整数部分和随机字符串
        Dim nonce As String = current_time_ms & diff_int & ":" & random_str

        '解析 URL
        Dim parsed_url As New System.Uri(urln)

        ' 构建 relative_path
        Dim relative_path As String = parsed_url.AbsolutePath &
            If(Not String.IsNullOrEmpty(parsed_url.Query), "?" & parsed_url.Query.Substring(1), "") &
            If(Not String.IsNullOrEmpty(parsed_url.Fragment), parsed_url.Fragment, "")

        Dim authority As String = parsed_url.Host

        '构造签名字符串
        Dim signature_string As String = nonce & vbLf & method_type & vbLf & relative_path & vbLf & authority & vbLf

        '计算 HMAC-SHA256
        Dim hmac_sha256 As Byte()
        Using hmac As New HMACSHA256(Encoding.UTF8.GetBytes(mac_key))
            hmac_sha256 = hmac.ComputeHash(Encoding.UTF8.GetBytes(signature_string))
        End Using

        '转换为 Base64 编码的字符串
        Dim base64_encoded As String = Convert.ToBase64String(hmac_sha256)

        '返回认证签名字符串
        Return "MAC id=""" & access_token & """,nonce=""" & nonce & """,mac=""" & base64_encoded & """"
    End Function

    '生成随机字符串
    Function MakeRandomString(ByVal length As Integer) As String
        Dim random As New Random()
        Dim chars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim result As New System.Text.StringBuilder(length)

        For i As Integer = 0 To length - 1
            Dim randomIndex As Integer = random.Next(chars.Length)
            result.Append(chars(randomIndex))
        Next

        Return result.ToString()
    End Function

    ''旧版获取请求头方法（已废弃）
    'Private Sub OnLoadUrlBegin(ByVal sender As Object, ByVal e As LoadUrlBeginEventArgs)
    '    On Error Resume Next
    '    Dim strUrl As String = e.URL
    '    Dim strHttpHead As String = Webb.NetGetHTTPHeaderField(e.Job, "x-nd-auth")
    '    Dim RequestMethod As wkeRequestType = Webb.GetRequestMethod(e.Job)
    '    Dim rawHead As wkeSlist = Webb.NetGetRawHttpHead(e.Job)
    '    Dim headList As List(Of String) = New List(Of String)()

    '    While True
    '        Dim strHead As String = rawHead.str.UTF8PtrToStr()

    '        'DebugLogForm.TextBox1.Text = DebugLogForm.TextBox1.Text & vbCrLf & strHead
    '        DebugLogForm.TextBox1.AppendText(vbCrLf & strHead)

    '        If strHead Is Nothing Then
    '            Exit While
    '        End If

    '        If strHead.ToLower.Contains("x-nd-auth") = True Then
    '            Xda = strHead
    '            Xda = Replace(Xda, "Referer: ", "")
    '            Dim ast() As String
    '            ast = Split(Xda, "X-ND-AUTH%22:%22")
    '            Dim a As String
    '            a = ast(ast.Length - 1)
    '            a = Replace(a, "%5C%22", """")
    '            a = Replace(a, "%22%7D", "")
    '            a = Replace(a, "%20", " ")
    '            'Link exp.
    '            'https://basic.smartedu.cn/pdfjs/2.13/web/viewer.html?file=https://r3-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/1c73b348-e8b6-47d6-84b0-6dbacbe28268.pkg/%E4%B9%89%E5%8A%A1%E6%95%99%E8%82%B2%E6%95%99%E7%A7%91%E4%B9%A6%20%E8%AF%AD%E6%96%87%20%E4%B8%80%E5%B9%B4%E7%BA%A7%20%E4%B8%8A%E5%86%8C_1726625277718.pdf&headers=%7B%22X-ND-AUTH%22:%22MAC%20id=%5C%22XXXXXXXXXX%5C%22,nonce=%5C%22XXXXXX%5C%22,mac=%5C%22XXXXXX%5C%22%22%7D
    '            Xda = a
    '            'Webb.StopLoading() '使用该命令会崩溃
    '            Exit While
    '        End If
    '        'headList.Add(strHead)

    '        If rawHead.[next] = IntPtr.Zero Then
    '            Exit While
    '        End If

    '        rawHead = CType(rawHead.[next].UTF8PtrToStruct(GetType(wkeSlist)), wkeSlist)
    '    End While

    'End Sub

    Private Sub OnLoadUrlFail(ByVal sender As Object, ByVal e As LoadUrlFailEventArgs)
        Dim strUrl As String = e.URL
    End Sub

    Sub FailSub(ByVal errcode As Integer)
        If errcode = 1 Then
            MainForm.MessageBoxError("登录失败。无法找到有效的登录状态信息。请尝试重新登陆。", "飞翔教学资源助手 - 错误", False)
        ElseIf errcode = 2 Then
            MainForm.MessageBoxError("登录失败。无法生成X-Nd-Auth请求头。请尝试重新登陆。", "飞翔教学资源助手 - 错误", False)
        End If
        Label1.Visible = False
        Me.Close()
    End Sub
End Class