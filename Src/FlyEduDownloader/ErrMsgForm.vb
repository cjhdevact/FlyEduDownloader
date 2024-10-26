'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：ErrMsgForm.vb
'描述：错误信息对话框
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
Public Class ErrMsgForm
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FeedBackForm.FeedBackInfo = Me.TextBox1.Text
        FeedBackForm.ShowDialog()
    End Sub
End Class