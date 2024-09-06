'==========================================
'项目：SmartEduDownloader
'作者：CJH
'文件：MySplashScreen.vb
'描述：启动屏幕
'==========================================
Public NotInheritable Class MySplashScreen
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        End
    End Sub
    'API移动窗体
    Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean
    Declare Function ReleaseCapture Lib "user32" Alias "ReleaseCapture" () As Boolean
    Const WM_SYSCOMMAND = &H112
    Const SC_MOVE = &HF010&
    Const HTCAPTION = 2
    Private Sub MySplashScreen_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown
        ReleaseCapture()
        SendMessage(Me.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0)
    End Sub
End Class
