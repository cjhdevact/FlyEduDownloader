'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：DownFormvb.vb
'描述：下载进度显示
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
Public Class DownFormvb
    Public st As Integer
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.ProgressBar1.Value = 100 Then
            st = 1
            Me.Close()
        Else
            If MessageBox.Show("确定要取消下载吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                MainForm.DownloadClient.CancelAsync()
                Try
                    System.IO.File.Delete(MainForm.SaveFileDialog1.FileName)
                Catch ex As Exception
                End Try
                st = 1
                Me.Close()
            End If
        End If

    End Sub

    Private Sub DownFormvb_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyClass.FormClosing
        If st = 1 Then
            e.Cancel = False
        Else
            If Me.ProgressBar1.Value = 100 Then
                e.Cancel = False
            Else
                If MessageBox.Show("确定要取消下载吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    MainForm.DownloadClient.CancelAsync()
                    Try
                        System.IO.File.Delete(MainForm.SaveFileDialog1.FileName)
                    Catch ex As Exception
                    End Try
                    e.Cancel = False
                Else
                    e.Cancel = True
                End If
            End If
        End If
    End Sub

    Private Sub DownFormvb_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        st = 0
    End Sub
End Class