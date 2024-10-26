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

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            AddReg("Software\CJH\FlyEduDownloader", "AutoGetUpdate", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
            MainForm.AGetUpdate = 1
        Else
            AddReg("Software\CJH\FlyEduDownloader", "AutoGetUpdate", 0, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
            MainForm.AGetUpdate = 0
        End If
    End Sub
End Class