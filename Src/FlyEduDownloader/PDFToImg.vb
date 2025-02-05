'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：PDFToImg.vb
'描述：PDF转图片
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
Imports O2S.Components.PDFRender4NET
Public Class PDFToImg
    Delegate Sub SetState()
    Delegate Sub SetStateForm(ByVal progressvalue As Integer, ByVal labeltext As String, ByVal buttontext As String)
    Public imgfr As System.Drawing.Imaging.ImageFormat = Nothing
    Public CmbSet As Integer = 0
    Public imgacc As String = ""
    Public CnvImg As New System.Threading.Thread(AddressOf CnvPDF)
    ' Public TrdAbortState As Integer = 0
    'PDF To Image
    Public Enum Definition
        One = 1
        Two = 2
        Three = 3
        Four = 4
        Five = 5
        Six = 6
        Seven = 7
        Eight = 8
        Nine = 9
        Ten = 10
    End Enum
    Public Sub PdfToImage(ByVal pdfPath As String, ByVal imagePath As String, ByVal imageName As String, ByVal imagePathFormat As String, ByVal imageFormat As System.Drawing.Imaging.ImageFormat, ByVal startPageNum As Integer, ByVal endPageNum As Integer, ByVal definition As Definition)
        Dim pdfFile As O2S.Components.PDFRender4NET.PDFFile = Nothing
        Try
            pdfFile = O2S.Components.PDFRender4NET.PDFFile.Open(pdfPath)
            If Not System.IO.Directory.Exists(imagePath) Then
                System.IO.Directory.CreateDirectory(imagePath)
            End If
            If startPageNum <= 0 Then
                startPageNum = 1
            End If
            If endPageNum > pdfFile.PageCount Then
                endPageNum = pdfFile.PageCount
            End If
            If startPageNum > endPageNum Then
                Dim tempPageNum As Integer = startPageNum
                startPageNum = endPageNum
                endPageNum = startPageNum
            End If
            For i As Integer = startPageNum To endPageNum
                If i = endPageNum Then
                    Me.Invoke(New SetStateForm(AddressOf SetStateFormSub), endPageNum, “转换完成”, "确定")
                Else
                    'ProgressBar1.Value = ProgressBar1.Value + 1
                    'ProgressBar1.Value = i
                    Dim ak As Integer = 0
                    If endPageNum > 0 Then
                        ak = Int((i / endPageNum) * 100)
                    End If
                    Me.Invoke(New SetStateForm(AddressOf SetStateFormSub), i, “正在转换" & ak & "%（第” & i & “页，共" & endPageNum & ”页）“, "取消")
                    'Label3.Text = “正在转换（" & ak & "%）”
                End If
                Dim pageImage As System.Drawing.Bitmap = pdfFile.GetPageImage(i - 1, 56 * CInt(definition))
                pageImage.Save("{imagePath}{imageName}-{i}{imagePathFormat}", imageFormat)
                pageImage.Dispose()
            Next
            pdfFile.Dispose()
        Catch ex As Exception
            'MessageBox.Show("PDF转图片失败。" & vbCrLf & ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            MainForm.MessageBoxError("PDF转图片失败。" & vbCrLf & ex.Message, "飞翔教学资源助手 - 错误", False)
            pdfFile.Dispose()
            Exit Sub
        End Try
    End Sub

    Sub SetAccForm()
        If ComboBox2.SelectedIndex = 0 Then
            imgfr = System.Drawing.Imaging.ImageFormat.Png
            imgacc = ".png"
        ElseIf ComboBox2.SelectedIndex = 1 Then
            imgfr = System.Drawing.Imaging.ImageFormat.Jpeg
            imgacc = ".jpg"
        ElseIf ComboBox2.SelectedIndex = 2 Then
            imgfr = System.Drawing.Imaging.ImageFormat.Bmp
            imgacc = ".bmp"
        ElseIf ComboBox2.SelectedIndex = 3 Then
            imgfr = System.Drawing.Imaging.ImageFormat.Icon
            imgacc = ".ico"
        ElseIf ComboBox2.SelectedIndex = 4 Then
            imgfr = System.Drawing.Imaging.ImageFormat.Gif
            imgacc = ".gif"
        ElseIf ComboBox2.SelectedIndex = 5 Then
            imgfr = System.Drawing.Imaging.ImageFormat.Tiff
            imgacc = ".tiff"
        End If
        CmbSet = ComboBox1.SelectedIndex + 1
    End Sub

    Sub SetStateFormSub(ByVal progressvalue As Integer, ByVal labeltext As String, ByVal buttontext As String)
        ProgressBar1.Value = progressvalue
        Label3.Text = labeltext
        Button3.Text = buttontext
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            TextBox1.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'Dim CnvImg As New System.Threading.Thread(AddressOf CnvPDF)
        If Button3.Text = "确定" Then
            Label3.Visible = False
            Label3.Text = “正在转换0%（第0页，共0页）”
            ProgressBar1.Value = 0
            ProgressBar1.Maximum = 100
            ProgressBar1.Visible = False
            Button3.Text = "开始转换"
            Me.Close()
        ElseIf Button3.Text = "取消" Then
            If MessageBox.Show("是否要取消PDF转图片？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
                Label3.Visible = False
                Label3.Text = “正在转换（0%）”
                ProgressBar1.Value = 0
                ProgressBar1.Maximum = 100
                ProgressBar1.Visible = False
                Button3.Text = "开始转换"
                CnvImg.Abort()
                Me.Close()
            End If
        Else
            If TextBox1.Text = "" Then
                MainForm.MessageBoxError("请选择一个文件。", "飞翔教学资源助手 - 错误", False)
                'MessageBox.Show("请选择一个文件。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            If TextBox2.Text = "" Then
                MainForm.MessageBoxError("请选择保存目录。", "飞翔教学资源助手 - 错误", False)
                'MessageBox.Show("请选择保存目录。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            If Not System.IO.File.Exists(TextBox1.Text) Then
                MainForm.MessageBoxError("未找到文件。", "飞翔教学资源助手 - 错误", False)
                'MessageBox.Show("未找到文件。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            If Not System.IO.Directory.Exists(TextBox2.Text) Then
                Try
                    System.IO.Directory.CreateDirectory(TextBox2.Text)
                Catch ex As Exception
                    MainForm.MessageBoxError("创建保存目录失败，请重新选择一个保存目录。" & vbCrLf & ex.Message, "飞翔教学资源助手 - 错误", False)
                    'MessageBox.Show("创建保存目录失败，请重新选择一个保存目录。" & vbCrLf & ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End Try
            End If
            Dim mypdf As O2S.Components.PDFRender4NET.PDFFile
            Try
                mypdf = O2S.Components.PDFRender4NET.PDFFile.Open(TextBox1.Text)
            Catch ex As Exception
                mypdf = Nothing
                MainForm.MessageBoxError("打开文件错误。" & vbCrLf & ex.Message, "飞翔教学资源助手 - 错误", False)
                ' MessageBox.Show("打开文件错误。" & vbCrLf & ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try
            ProgressBar1.Value = 0
            ProgressBar1.Maximum = mypdf.PageCount
            ProgressBar1.Visible = True
            Label3.Visible = True
            Label3.Text = “正在转换（0%）”
            Button3.Text = "取消"
            ' PdfToImage(TextBox1.Text, TextBox2.Text & "\", a(a.Length - 1), imgacc, imgfr, 1, mypdf.PageCount, ComboBox1.SelectedIndex + 1)
            CnvImg.Start()
        End If
    End Sub

    Sub CnvPDF()
        Dim mypdf As O2S.Components.PDFRender4NET.PDFFile = O2S.Components.PDFRender4NET.PDFFile.Open(TextBox1.Text)
        Me.Invoke(New SetState(AddressOf SetAccForm))
        Dim a As String()
        a = Split(TextBox1.Text, "\")
        a(a.Length - 1) = Replace(a(a.Length - 1), ".pdf", "")
        PdfToImage(TextBox1.Text, TextBox2.Text & "\", a(a.Length - 1), imgacc, imgfr, 1, mypdf.PageCount, CmbSet)
    End Sub

    Private Sub PDFToImg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 4
        ComboBox1.SelectedText = "5（中等）（建议）"

        ComboBox2.SelectedIndex = 0
        ComboBox2.SelectedText = "PNG 格式（建议）"
    End Sub
    Private Sub PDFToImg_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyClass.FormClosing
        If Button3.Text = "取消" Then
            If MessageBox.Show("是否要取消PDF转图片？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
                Label3.Visible = False
                Label3.Text = “正在转换（0%）”
                ProgressBar1.Value = 0
                ProgressBar1.Maximum = 100
                ProgressBar1.Visible = False
                Button3.Text = "开始转换"
                CnvImg.Abort()
                Me.Close()
            End If
        End If
    End Sub
End Class