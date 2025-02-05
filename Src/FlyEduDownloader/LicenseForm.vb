'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：LicenseForm.vb
'描述：许可协议及免责声明
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
Imports Microsoft.Win32

Public Class LicenseForm
    Dim a As Integer
    Private Sub LicenseForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.RichTextBox1.Rtf = My.Resources.license
        a = 0
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            Registry.CurrentUser.CreateSubKey("Software\CJH")
            Registry.CurrentUser.CreateSubKey("Software\CJH\FlyEduDownloader")
        Catch ex As Exception
        End Try
        Try
            Dim key2 As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\CJH\FlyEduDownloader", True)
            If (Not key2 Is Nothing) Then
                If (If((key2.GetValue("") Is Nothing), Nothing, key2.GetValue("").ToString) <> "AcceptLicense") Then
                    key2.SetValue("AcceptLicense", 0, RegistryValueKind.DWord)
                End If
                key2.Close()
            Else
                key2 = Registry.CurrentUser.CreateSubKey("Software\CJH\FlyEduDownloader", RegistryKeyPermissionCheck.ReadWriteSubTree)
                key2.SetValue("AcceptLicense", 0, RegistryValueKind.DWord)
                key2.Close()
            End If
        Catch ex As Exception
        End Try
        End
    End Sub
    Private Sub LicenseForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If a = 0 Then
            e.Cancel = True
        End If
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        a = 1
        Try
            Registry.CurrentUser.CreateSubKey("Software\CJH")
            Registry.CurrentUser.CreateSubKey("Software\CJH\FlyEduDownloader")
        Catch ex As Exception
        End Try
        Try
            Dim key2 As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\CJH\FlyEduDownloader", True)
            If (Not key2 Is Nothing) Then
                If (If((key2.GetValue("") Is Nothing), Nothing, key2.GetValue("").ToString) <> "AcceptLicense") Then
                    key2.SetValue("AcceptLicense", 1, RegistryValueKind.DWord)
                End If
                key2.Close()
            Else
                key2 = Registry.CurrentUser.CreateSubKey("Software\CJH\FlyEduDownloader", RegistryKeyPermissionCheck.ReadWriteSubTree)
                key2.SetValue("AcceptLicense", 1, RegistryValueKind.DWord)
                key2.Close()
            End If
        Catch ex As Exception
        End Try
        Me.Close()
    End Sub
End Class