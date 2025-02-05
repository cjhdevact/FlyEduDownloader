'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：NoticeForm.vb
'描述：公告栏
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
'Imports Newtonsoft.Json.Linq
Public Class NoticeForm

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    'Private Sub NoticeForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
    '    '获取公告
    '    Dim ntic As String
    '    ntic = MainForm.GetSource("https://cjhdevact.github.io/otherprojects/SmartEduDownloader/noticemsg.json")
    '    Try
    '        Dim NoticeObject As JObject = JObject.Parse(ntic)
    '        Me.Text = CStr(NoticeObject("title"))
    '        Dim InfoText As JArray = NoticeObject("text")
    '        For i = 0 To InfoText.Count - 1
    '            If i = InfoText.Count - 1 Then
    '                TextBox1.Text = TextBox1.Text & InfoText(i).ToString
    '            Else
    '                TextBox1.Text = TextBox1.Text & InfoText(i).ToString & vbCrLf
    '            End If
    '        Next
    '    Catch ex As Exception
    '        TextBox1.Text = "获取公告错误：" & ex.Message
    '    End Try
    'End Sub
End Class