'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：TagsSet.vb
'描述：选择资源包里要下载的内容
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
Public Class TagsSet
    Public ec As Integer
    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        ec = 0
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        ec = 1
        Me.Close()
    End Sub

    Private Sub TagsSet_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ec = 0
        If ListBox1.Items.Count <> 0 Then
            For i As Integer = 0 To ListBox1.Items.Count - 1
                ListBox1.SetItemChecked(i, False)
            Next
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If ListBox1.Items.Count <> 0 Then '全部选中
            For u As Integer = 0 To ListBox1.Items.Count - 1
                ListBox1.SetItemChecked(u, True)
            Next
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If ListBox1.Items.Count <> 0 Then '反选
            For u As Integer = 0 To ListBox1.Items.Count - 1
                ListBox1.SetItemChecked(u, Not (ListBox1.GetItemChecked(u)))
            Next
            'Else
            '    MsgBox("反向选中遇到错误：列表数据为空，本次操作无效。", MsgBoxStyle.Critical)
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If ListBox1.Items.Count <> 0 Then
            'Dim allChecked As Boolean = True
            'For i As Integer = 0 To ListBox1.Items.Count - 1
            '    If Not ListBox1.GetItemChecked(i) Then
            '        allChecked = False
            '        Exit For
            '    End If
            'Next
            ' 如果全部都已选中，则取消所有选中；否则选中所有项
            For i As Integer = 0 To ListBox1.Items.Count - 1
                ListBox1.SetItemChecked(i, False)
            Next
        End If
    End Sub
End Class