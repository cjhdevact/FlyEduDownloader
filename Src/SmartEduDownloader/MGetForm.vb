'==========================================
'项目：SmartEduDownloader
'作者：CJH
'文件：MGetForm.vb
'描述：批量解析链接
'==========================================
Imports Newtonsoft.Json.Linq
Imports System.Net

Public Class MGetForm
    Dim DownLinks() As String
    Dim OErr As Integer
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Then
            MessageBox.Show("至少输入一个地址。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        TextBox2.Text = ""
        DownLinks = Split(TextBox1.Text, vbCrLf)
        Dim iu As Integer
        iu = DownLinks.Length
        For i = 0 To iu - 1
            If DownLinks(i) = "" Then
                If OErr = 1 Then
                    TextBox2.Text = TextBox2.Text & vbCrLf
                End If

            ElseIf Not InStr(DownLinks(i), "basic.smartedu.cn") > 0 Then
                If OErr = 1 Then
                    TextBox2.Text = TextBox2.Text & vbCrLf & "错误：不支持下载当前链接。"
                End If
            Else
                If InStr(DownLinks(i), "basic.smartedu.cn") > 0 Then Call smartedudown(DownLinks(i))
            End If
        Next
    End Sub

    Sub smartedudown(ByVal BookLink As String)
        Dim k As Integer
        k = InStr(BookLink, "contentId=")
        If k <= 0 Then
            If OErr = 1 Then
                TextBox2.Text = TextBox2.Text & vbCrLf & "错误：链接解析失败。无法获取BookID。"
            End If

            Exit Sub
        End If
        k = k + 9
        Dim bookid As String = BookLink.Substring(k, 36) '截取从索引k开始，长度为36的子字符串
        If Len(bookid) < 36 Then
            If OErr = 1 Then
                TextBox2.Text = TextBox2.Text & vbCrLf & "错误：链接解析失败。无法获取BookID。"
            End If

            Exit Sub
        End If
        Dim booknameurl As String
        booknameurl = "https://s-file-1.ykt.cbern.com.cn/zxx/ndrv2/resources/tch_material/details/" & bookid & ".json"
        Dim bookinforeq As String

        bookinforeq = MainForm.GetSource(booknameurl)
        If bookinforeq = "" Then
            If OErr = 1 Then
                TextBox2.Text = TextBox2.Text & vbCrLf & "错误：获取电子书信息失败。"
            End If

            Exit Sub
        End If
        Try
            Dim BookInfoObject As JObject = JObject.Parse(bookinforeq)
            Dim BookItemsObject As JArray = BookInfoObject("ti_items")
            Dim DownBookLinkPri As String = CStr((BookItemsObject(1)("ti_storages"))(0))
            Dim DownBookLink As String
            If MainForm.DownloadMode = 1 Then
                DownBookLink = Replace(DownBookLinkPri, "ndr-private.ykt.cbern.com.cn", "ndr.ykt.cbern.com.cn")
            Else
                DownBookLink = DownBookLinkPri
            End If

            TextBox2.Text = TextBox2.Text & vbCrLf & DownBookLink

        Catch ex As Exception
            If OErr = 1 Then
                TextBox2.Text = TextBox2.Text & vbCrLf & "错误：" & ex.Message
            End If
        End Try
    End Sub
    Private Sub MGetForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ServicePointManager.SecurityProtocol = CType(192, SecurityProtocolType) Or CType(768, SecurityProtocolType) Or CType(3072, SecurityProtocolType)
        OErr = 1
        CheckBox1.Checked = False
        DownLinks = Nothing
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            OErr = 0
        Else
            OErr = 1
        End If
    End Sub
End Class