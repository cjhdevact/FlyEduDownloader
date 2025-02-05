<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.BookNameLabel = New System.Windows.Forms.Label()
        Me.BookIDLabel = New System.Windows.Forms.Label()
        Me.BookTagLabel = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FindBooksRes = New System.Windows.Forms.ToolStripMenuItem()
        Me.Fbooksm = New System.Windows.Forms.ToolStripMenuItem()
        Me.findlessm = New System.Windows.Forms.ToolStripMenuItem()
        Me.logmm = New System.Windows.Forms.ToolStripMenuItem()
        Me.sxam = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolm = New System.Windows.Forms.ToolStripMenuItem()
        Me.readmm = New System.Windows.Forms.ToolStripMenuItem()
        Me.downmm = New System.Windows.Forms.ToolStripMenuItem()
        Me.dllinm = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtcnvm = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtspm = New System.Windows.Forms.ToolStripMenuItem()
        Me.PDFToPicm = New System.Windows.Forms.ToolStripMenuItem()
        Me.settingm = New System.Windows.Forms.ToolStripMenuItem()
        Me.helpm = New System.Windows.Forms.ToolStripMenuItem()
        Me.FindUpdatem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.Hlpdocm = New System.Windows.Forms.ToolStripMenuItem()
        Me.feedbackm = New System.Windows.Forms.ToolStripMenuItem()
        Me.Ofpm = New System.Windows.Forms.ToolStripMenuItem()
        Me.noticem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.aboutm = New System.Windows.Forms.ToolStripMenuItem()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.White
        Me.TextBox1.ForeColor = System.Drawing.Color.Black
        Me.TextBox1.Location = New System.Drawing.Point(13, 57)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(529, 25)
        Me.TextBox1.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(550, 52)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(117, 36)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "获取信息"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'BookNameLabel
        '
        Me.BookNameLabel.AutoSize = True
        Me.BookNameLabel.Location = New System.Drawing.Point(14, 122)
        Me.BookNameLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.BookNameLabel.Name = "BookNameLabel"
        Me.BookNameLabel.Size = New System.Drawing.Size(74, 19)
        Me.BookNameLabel.TabIndex = 2
        Me.BookNameLabel.Text = "书籍名称："
        '
        'BookIDLabel
        '
        Me.BookIDLabel.AutoSize = True
        Me.BookIDLabel.Location = New System.Drawing.Point(14, 157)
        Me.BookIDLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.BookIDLabel.Name = "BookIDLabel"
        Me.BookIDLabel.Size = New System.Drawing.Size(62, 19)
        Me.BookIDLabel.TabIndex = 3
        Me.BookIDLabel.Text = "书籍ID："
        '
        'BookTagLabel
        '
        Me.BookTagLabel.Location = New System.Drawing.Point(14, 191)
        Me.BookTagLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.BookTagLabel.Name = "BookTagLabel"
        Me.BookTagLabel.Size = New System.Drawing.Size(376, 154)
        Me.BookTagLabel.TabIndex = 4
        Me.BookTagLabel.Text = "书籍标签："
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(18, 0)
        Me.Label1.Margin = New System.Windows.Forms.Padding(0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(647, 26)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "下载链接：（可能存在多个下载链接，选择一个能用的即可）"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox2.ForeColor = System.Drawing.Color.Black
        Me.TextBox2.Location = New System.Drawing.Point(18, 26)
        Me.TextBox2.Margin = New System.Windows.Forms.Padding(0)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox2.Size = New System.Drawing.Size(647, 133)
        Me.TextBox2.TabIndex = 6
        Me.TextBox2.WordWrap = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(452, 19)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "请输入国家中小学智慧教育平台教材页面或者课程教学页面的链接或对应ID："
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FindBooksRes, Me.logmm, Me.sxam, Me.toolm, Me.helpm})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(683, 25)
        Me.MenuStrip1.TabIndex = 8
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FindBooksRes
        '
        Me.FindBooksRes.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Fbooksm, Me.findlessm})
        Me.FindBooksRes.Name = "FindBooksRes"
        Me.FindBooksRes.Size = New System.Drawing.Size(82, 21)
        Me.FindBooksRes.Text = "寻找资源(&F)"
        '
        'Fbooksm
        '
        Me.Fbooksm.Name = "Fbooksm"
        Me.Fbooksm.Size = New System.Drawing.Size(163, 22)
        Me.Fbooksm.Text = "寻找教材(&B)"
        '
        'findlessm
        '
        Me.findlessm.Name = "findlessm"
        Me.findlessm.Size = New System.Drawing.Size(163, 22)
        Me.findlessm.Text = "寻找课程资源(&P)"
        '
        'logmm
        '
        Me.logmm.Name = "logmm"
        Me.logmm.Size = New System.Drawing.Size(130, 21)
        Me.logmm.Text = "使用登录模式下载(&L)"
        '
        'sxam
        '
        Me.sxam.Name = "sxam"
        Me.sxam.Size = New System.Drawing.Size(107, 21)
        Me.sxam.Text = "设置登录信息(&S)"
        '
        'toolm
        '
        Me.toolm.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.readmm, Me.downmm, Me.dllinm, Me.txtcnvm, Me.txtspm, Me.PDFToPicm, Me.settingm})
        Me.toolm.Name = "toolm"
        Me.toolm.Size = New System.Drawing.Size(83, 21)
        Me.toolm.Text = "实用工具(&T)"
        '
        'readmm
        '
        Me.readmm.Name = "readmm"
        Me.readmm.Size = New System.Drawing.Size(199, 22)
        Me.readmm.Text = "批量解析教材(&R)"
        '
        'downmm
        '
        Me.downmm.Name = "downmm"
        Me.downmm.Size = New System.Drawing.Size(199, 22)
        Me.downmm.Text = "批量下载教材(&D)"
        '
        'dllinm
        '
        Me.dllinm.Name = "dllinm"
        Me.dllinm.Size = New System.Drawing.Size(199, 22)
        Me.dllinm.Text = "下载链接(&K)"
        '
        'txtcnvm
        '
        Me.txtcnvm.Name = "txtcnvm"
        Me.txtcnvm.Size = New System.Drawing.Size(199, 22)
        Me.txtcnvm.Text = "多行文本合并为一行(&E)"
        '
        'txtspm
        '
        Me.txtspm.Name = "txtspm"
        Me.txtspm.Size = New System.Drawing.Size(199, 22)
        Me.txtspm.Text = "文本分割(&P)"
        '
        'PDFToPicm
        '
        Me.PDFToPicm.Name = "PDFToPicm"
        Me.PDFToPicm.Size = New System.Drawing.Size(199, 22)
        Me.PDFToPicm.Text = "PDF转图片(&C)"
        '
        'settingm
        '
        Me.settingm.Name = "settingm"
        Me.settingm.Size = New System.Drawing.Size(199, 22)
        Me.settingm.Text = "设置(&S)"
        '
        'helpm
        '
        Me.helpm.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FindUpdatem, Me.ToolStripMenuItem1, Me.Hlpdocm, Me.feedbackm, Me.Ofpm, Me.noticem, Me.ToolStripSeparator1, Me.aboutm})
        Me.helpm.Name = "helpm"
        Me.helpm.Size = New System.Drawing.Size(61, 21)
        Me.helpm.Text = "帮助(&H)"
        '
        'FindUpdatem
        '
        Me.FindUpdatem.Name = "FindUpdatem"
        Me.FindUpdatem.Size = New System.Drawing.Size(142, 22)
        Me.FindUpdatem.Text = "检查更新(&U)"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(139, 6)
        '
        'Hlpdocm
        '
        Me.Hlpdocm.Name = "Hlpdocm"
        Me.Hlpdocm.Size = New System.Drawing.Size(142, 22)
        Me.Hlpdocm.Text = "使用教程(&H)"
        '
        'feedbackm
        '
        Me.feedbackm.Name = "feedbackm"
        Me.feedbackm.Size = New System.Drawing.Size(142, 22)
        Me.feedbackm.Text = "反馈问题(&F)"
        '
        'Ofpm
        '
        Me.Ofpm.Name = "Ofpm"
        Me.Ofpm.Size = New System.Drawing.Size(142, 22)
        Me.Ofpm.Text = "软件官网(&O)"
        '
        'noticem
        '
        Me.noticem.Name = "noticem"
        Me.noticem.Size = New System.Drawing.Size(142, 22)
        Me.noticem.Text = "公告(&P)"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(139, 6)
        '
        'aboutm
        '
        Me.aboutm.Name = "aboutm"
        Me.aboutm.Size = New System.Drawing.Size(142, 22)
        Me.aboutm.Text = "关于(&A)"
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(487, 122)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(180, 240)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 9
        Me.PictureBox1.TabStop = False
        '
        'Button2
        '
        Me.Button2.Enabled = False
        Me.Button2.Location = New System.Drawing.Point(424, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(107, 31)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "保存书籍信息"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Enabled = False
        Me.Button3.Location = New System.Drawing.Point(537, 3)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(107, 31)
        Me.Button3.TabIndex = 11
        Me.Button3.Text = "保存电子书"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Enabled = False
        Me.Button4.Location = New System.Drawing.Point(311, 3)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(107, 31)
        Me.Button4.TabIndex = 12
        Me.Button4.Text = "保存封面"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Title = "请选择你要保存的位置..."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 93)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(595, 19)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "如果登录状态失效（下载提示401）或者X-Nd-Auth信息错误，可以在""设置登录信息""菜单重新登录。"
        '
        'Button5
        '
        Me.Button5.Enabled = False
        Me.Button5.Location = New System.Drawing.Point(198, 3)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(107, 31)
        Me.Button5.TabIndex = 14
        Me.Button5.Text = "保存Json信息"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox2, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel1, 1, 2)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 378)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(683, 199)
        Me.TableLayoutPanel1.TabIndex = 16
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.Button3)
        Me.FlowLayoutPanel1.Controls.Add(Me.Button2)
        Me.FlowLayoutPanel1.Controls.Add(Me.Button4)
        Me.FlowLayoutPanel1.Controls.Add(Me.Button5)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(18, 159)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(647, 40)
        Me.FlowLayoutPanel1.TabIndex = 7
        '
        'PictureBox2
        '
        Me.PictureBox2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(459, 33)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 17
        Me.PictureBox2.TabStop = False
        '
        'FolderBrowserDialog1
        '
        Me.FolderBrowserDialog1.Description = "请选择你要保存的目录"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(539, 28)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(132, 23)
        Me.CheckBox1.TabIndex = 18
        Me.CheckBox1.Text = "尝试获取旧版教材"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(683, 577)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BookIDLabel)
        Me.Controls.Add(Me.BookNameLabel)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.BookTagLabel)
        Me.Font = New System.Drawing.Font("微软雅黑", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "飞翔教学资源助手"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents BookNameLabel As System.Windows.Forms.Label
    Friend WithEvents BookIDLabel As System.Windows.Forms.Label
    Friend WithEvents BookTagLabel As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FindBooksRes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents toolm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents readmm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents downmm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents logmm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sxam As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dllinm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtcnvm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtspm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Fbooksm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents findlessm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents PDFToPicm As ToolStripMenuItem
    Friend WithEvents settingm As ToolStripMenuItem
    Friend WithEvents helpm As ToolStripMenuItem
    Friend WithEvents FindUpdatem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents Hlpdocm As ToolStripMenuItem
    Friend WithEvents Ofpm As ToolStripMenuItem
    Friend WithEvents noticem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents aboutm As ToolStripMenuItem
    Friend WithEvents feedbackm As ToolStripMenuItem
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
End Class
