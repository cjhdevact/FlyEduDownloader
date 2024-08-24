Public Class AboutForm

    Private Sub LinkLabel3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/SmartEduDownloader/Help/index.html")
    End Sub

    Private Sub AboutForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sa As String
        If Environment.Is64BitOperatingSystem = True Then
            sa = "x64"
        Else
            sa = "x86"
        End If
        Label3.Text = "版本 " & My.Application.Info.Version.ToString & " (" & MainForm.AppBuildTime & ") " & MainForm.MyArch & " " & MainForm.AppBuildChannel
        Label6.Text = "系统信息 " & My.Computer.Info.OSFullName & " " & sa & " " & GetOSVersion()
    End Sub

    Function GetOSVersion() As String
        Dim strBuild1, strBuild2, strBuild3, strBuild4 As String
        Dim regKey As Microsoft.Win32.RegistryKey
        regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion")
        strBuild1 = regKey.GetValue("CurrentMajorVersionNumber")
        strBuild2 = regKey.GetValue("CurrentMinorVersionNumber")
        strBuild3 = regKey.GetValue("CurrentBuild")
        strBuild4 = regKey.GetValue("UBR")
        regKey.Close()
        Return strBuild1 & "." & strBuild2 & "." & strBuild3 & "." & strBuild4
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        System.Diagnostics.Process.Start("https://github.com/cjhdevact/SmartEduDownloader/issues")
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("https://cjhdevact.github.io/otherprojects/SmartEduDownloader/index.html")
    End Sub

    Private Sub LinkLabel4_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        System.Diagnostics.Process.Start("https://github.com/cjhdevact/SmartEduDownloader")
    End Sub
End Class