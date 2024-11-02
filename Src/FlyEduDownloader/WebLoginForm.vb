'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：WebLoginForm.vb
'描述：网页登录
'License：
'FlyEduDownloader
'Copyright (C) 2024 CJH.

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

Public Class WebLoginForm
    Public Webb As New WebView
    Public Xda As String

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
        'Webb.LoadHTML(My.Resources.String1)
        Webb.LoadURL("https://auth.smartedu.cn/uias/login")
        'Webb.LoadURL("https://www.smartedu.cn/")
        Xda = ""
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Webb.GetURL = "https://www.smartedu.cn/" Then
            Webb.StopLoading()
            Panel1.Visible = False
            Label1.Visible = True
            'Me.Hide()
            AddHandler Webb.OnLoadUrlBegin, AddressOf OnLoadUrlBegin
            'AddHandler Webb.OnLoadUrlEnd, AddressOf OnLoadUrlEnd
            'AddHandler Webb.OnNetResponse, AddressOf OnNetResponse
            AddHandler Webb.OnLoadUrlFail, AddressOf OnLoadUrlFail
            '这里参考了AnyTextbookDownloader项目的抓取登录信息原理
            Webb.LoadURL("https://basic.smartedu.cn/tchMaterial/detail?contentType=assets_document&contentId=1c73b348-e8b6-47d6-84b0-6dbacbe28268&catalogType=tchMaterial&subCatalog=tchMaterial")
        End If
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

    Private Sub OnLoadUrlBegin(ByVal sender As Object, ByVal e As LoadUrlBeginEventArgs)
        On Error Resume Next
        Dim strUrl As String = e.URL
        Dim strHttpHead As String = Webb.NetGetHTTPHeaderField(e.Job, "x-nd-auth")
        Dim RequestMethod As wkeRequestType = Webb.GetRequestMethod(e.Job)
        Dim rawHead As wkeSlist = Webb.NetGetRawHttpHead(e.Job)
        Dim headList As List(Of String) = New List(Of String)()

        While True
            Dim strHead As String = rawHead.str.UTF8PtrToStr()

            If strHead Is Nothing Then
                Exit While
            End If

            If strHead.ToLower.Contains("x-nd-auth") = True Then
                Xda = strHead
                Xda = Replace(Xda, "Referer: ", "")
                Dim ast() As String
                ast = Split(Xda, "X-ND-AUTH%22:%22")
                Dim a As String
                a = ast(ast.Length - 1)
                a = Replace(a, "%5C%22", """")
                a = Replace(a, "%22%7D", "")
                a = Replace(a, "%20", " ")
                'Link exp.
                'https://basic.smartedu.cn/pdfjs/2.13/web/viewer.html?file=https://r3-ndr-private.ykt.cbern.com.cn/edu_product/esp/assets/1c73b348-e8b6-47d6-84b0-6dbacbe28268.pkg/%E4%B9%89%E5%8A%A1%E6%95%99%E8%82%B2%E6%95%99%E7%A7%91%E4%B9%A6%20%E8%AF%AD%E6%96%87%20%E4%B8%80%E5%B9%B4%E7%BA%A7%20%E4%B8%8A%E5%86%8C_1726625277718.pdf&headers=%7B%22X-ND-AUTH%22:%22MAC%20id=%5C%22XXXXXXXXXX%5C%22,nonce=%5C%22XXXXXX%5C%22,mac=%5C%22XXXXXX%5C%22%22%7D
                Xda = a
                'Webb.StopLoading() '使用该命令会崩溃
                Exit While
            End If
            'headList.Add(strHead)

            If rawHead.[next] = IntPtr.Zero Then
                Exit While
            End If

            rawHead = CType(rawHead.[next].UTF8PtrToStruct(GetType(wkeSlist)), wkeSlist)
        End While

        'Dim strPostData As Dictionary(Of String, String) = New Dictionary(Of String, String)()

        'If RequestMethod = wkeRequestType.Post Then
        '    Dim eles As wkePostBodyElements = Webb.NetGetPostBody(e.Job)
        '    Dim eleList As List(Of wkePostBodyElement) = New List(Of wkePostBodyElement)()
        '    Dim ptrEles As IntPtr = eles.element

        '    For i As Integer = 0 To eles.elementSize - 1
        '        Dim ptrTemp As IntPtr = CType(ptrEles.UTF8PtrToStruct(GetType(IntPtr)), IntPtr)
        '        eleList.Add(CType(ptrTemp.UTF8PtrToStruct(GetType(wkePostBodyElement)), wkePostBodyElement))
        '        ptrEles = New IntPtr(ptrEles.ToInt64() + IntPtr.Size)
        '    Next

        '    For Each item In eleList

        '        If item.type = wkeHttBodyElementType.wkeHttBodyElementTypeData Then
        '            Dim memBuf As wkeMemBuf = CType(item.data.UTF8PtrToStruct(GetType(wkeMemBuf)), wkeMemBuf)
        '            Dim data As Byte() = New Byte(memBuf.length - 1) {}
        '            Marshal.Copy(memBuf.data, data, 0, data.Length)
        '            Dim strData As String = Encoding.UTF8.GetString(data)

        '            If strData = String.Empty OrElse strData Is Nothing OrElse strData.StartsWith("--") Then
        '                Continue For
        '            End If

        '            For Each strKV As String In strData.Split("&"c)
        '                Dim kv As String() = strKV.Split("="c)

        '                If kv.Length = 2 Then
        '                    strPostData.Add(kv(0), kv(1))
        '                End If
        '            Next
        '        End If
        '    Next
        'End If

        'Dim newRequest As HttpWebRequest = CType(WebRequest.Create("新的url"), HttpWebRequest)
        'newRequest.Method = "post"
        'newRequest.AllowAutoRedirect = True
        'Dim postData As Byte() = Encoding.UTF8.GetBytes("新的要post的数据")
        'newRequest.ContentLength = postData.Length

        'Using reqStream As Stream = newRequest.GetRequestStream()
        '    reqStream.Write(postData, 0, postData.Length)
        '    reqStream.Close()
        'End Using

        'Dim newResponse As WebResponse = newRequest.GetResponse()

        'If strUrl.Contains("你想干掉的关键字") Then
        '    Webb.NetCancelRequest(e.Job)
        '    e.Cancel = True
        'End If

        'Webb.NetSetData(e.Job, "alert('test')")
        'Webb.NetHookRequest(e.Job)
        'Webb.NetChangeRequestUrl(e.Job, "新的url")
        'Webb.NetSetMIMEType(e.Job, "text/html")

        'If Webb.NetHoldJobToAsynCommit(e.Job) Then
        '    'Task.Factory.StartNew(Sub()
        '    '                          m_AsynJob = e.Job
        '    '                          File.ReadAllText("某个文件")
        '    '                      End Sub).ContinueWith(Sub(arg)
        '    '                                                Invoke(New Action(Sub()
        '    '                                                                      Webb.NetContinueJob(m_AsynJob)
        '    '                                                                  End Sub))
        '    '                                            End Sub)
        'End If
    End Sub

    'Private Sub OnLoadUrlEnd(ByVal sender As Object, ByVal e As LoadUrlEndEventArgs)
    '    Dim strData As String = Encoding.UTF8.GetString(e.Data)

    '    If strData.Contains("你关注的数据内容") Then
    '        e.Data = Encoding.UTF8.GetBytes("修改后的数据")
    '    End If
    'End Sub

    'Private Sub OnNetResponse(ByVal sender As Object, ByVal e As NetResponseEventArgs)
    '    Dim strUrl As String = e.URL
    '    Dim strResponseHead As String = Webb.NetGetHTTPHeaderFieldFromResponse(e.Job, "content-type")
    '    Dim strMimeType As String = Webb.NetGetMIMEType(e.Job)
    '    Dim rawResponseHead As wkeSlist = Webb.NetGetRawResponseHead(e.Job)
    '    Dim headList As List(Of String) = New List(Of String)()

    '    While True
    '        Dim strHead As String = rawResponseHead.str.UTF8PtrToStr()

    '        If strHead Is Nothing Then
    '            Exit While
    '        End If

    '        headList.Add(strHead)

    '        If rawResponseHead.[next] = IntPtr.Zero Then
    '            Exit While
    '        End If

    '        rawResponseHead = CType(rawResponseHead.[next].UTF8PtrToStruct(GetType(wkeSlist)), wkeSlist)
    '    End While
    'End Sub

    Private Sub OnLoadUrlFail(ByVal sender As Object, ByVal e As LoadUrlFailEventArgs)
        Dim strUrl As String = e.URL
    End Sub
End Class