Public Class SettingForm
    Private Sub SettingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.AGetNotice = 1 Then
            CheckBox1.Checked = True
        Else
            CheckBox1.Checked = False
        End If
        If MainForm.AGetUpdate = 1 Then
            CheckBox2.Checked = True
        Else
            CheckBox2.Checked = False
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            AddReg("Software\CJH\FlyEduDownloader", "GetNotice", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
            MainForm.AGetNotice = 1
        Else
            AddReg("Software\CJH\FlyEduDownloader", "GetNotice", 0, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
            MainForm.AGetNotice = 0
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            AddReg("Software\CJH\FlyEduDownloader", "AutoGetUpdate", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
            MainForm.AGetUpdate = 1
        Else
            AddReg("Software\CJH\FlyEduDownloader", "AutoGetUpdate", 0, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
            MainForm.AGetUpdate = 0
        End If
    End Sub

    Private Sub CheckBox2_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckStateChanged
        If CheckBox2.CheckState = CheckState.Unchecked Then
            If MessageBox.Show("本工具每次更新都包括许多功能修复和改进，如果你取消更新，你将无法快速获得最新版本信息，且可能因为平台更新而导致无法下载资源。" & vbCrLf & "是否取消自动检查更新？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                CheckBox2.Checked = CheckState.Checked
            End If
        End If
    End Sub
End Class