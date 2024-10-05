'==========================================
'项目：SmartEduDownloader
'作者：CJH
'文件：TagsSet.vb
'描述：选择资源包里要下载的内容
'License：
'SmartEduDownloader
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
    End Sub
End Class