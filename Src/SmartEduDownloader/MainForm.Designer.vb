<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
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
        Me.FindBooksB = New System.Windows.Forms.ToolStripMenuItem()
        Me.logmm = New System.Windows.Forms.ToolStripMenuItem()
        Me.sxam = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolm = New System.Windows.Forms.ToolStripMenuItem()
        Me.readmm = New System.Windows.Forms.ToolStripMenuItem()
        Me.downmm = New System.Windows.Forms.ToolStripMenuItem()
        Me.dllinm = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtcnvm = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtspm = New System.Windows.Forms.ToolStripMenuItem()
        Me.hlpdocm = New System.Windows.Forms.ToolStripMenuItem()
        Me.ofpm = New System.Windows.Forms.ToolStripMenuItem()
        Me.aboutm = New System.Windows.Forms.ToolStripMenuItem()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.White
        Me.TextBox1.ForeColor = System.Drawing.Color.Black
        Me.TextBox1.Location = New System.Drawing.Point(16, 104)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(660, 29)
        Me.TextBox1.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(687, 97)
        Me.Button1.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(146, 45)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "获取信息"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'BookNameLabel
        '
        Me.BookNameLabel.AutoSize = True
        Me.BookNameLabel.Location = New System.Drawing.Point(12, 182)
        Me.BookNameLabel.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.BookNameLabel.Name = "BookNameLabel"
        Me.BookNameLabel.Size = New System.Drawing.Size(95, 23)
        Me.BookNameLabel.TabIndex = 2
        Me.BookNameLabel.Text = "书籍名称："
        '
        'BookIDLabel
        '
        Me.BookIDLabel.AutoSize = True
        Me.BookIDLabel.Location = New System.Drawing.Point(12, 226)
        Me.BookIDLabel.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.BookIDLabel.Name = "BookIDLabel"
        Me.BookIDLabel.Size = New System.Drawing.Size(79, 23)
        Me.BookIDLabel.TabIndex = 3
        Me.BookIDLabel.Text = "书籍ID："
        '
        'BookTagLabel
        '
        Me.BookTagLabel.AutoSize = True
        Me.BookTagLabel.Location = New System.Drawing.Point(12, 269)
        Me.BookTagLabel.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.BookTagLabel.Name = "BookTagLabel"
        Me.BookTagLabel.Size = New System.Drawing.Size(95, 23)
        Me.BookTagLabel.TabIndex = 4
        Me.BookTagLabel.Text = "书籍标签："
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 436)
        Me.Label1.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(469, 23)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "下载链接：（可能存在多个下载链接，选择一个能用的即可）"
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextBox2.ForeColor = System.Drawing.Color.Black
        Me.TextBox2.Location = New System.Drawing.Point(18, 466)
        Me.TextBox2.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox2.Size = New System.Drawing.Size(815, 135)
        Me.TextBox2.TabIndex = 6
        Me.TextBox2.WordWrap = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 74)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(588, 23)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "国家中小学智慧教育平台教材页面链接：（将教材页面链接粘贴至下方即可）"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FindBooksB, Me.logmm, Me.sxam, Me.toolm, Me.hlpdocm, Me.ofpm, Me.aboutm})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(8, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(854, 28)
        Me.MenuStrip1.TabIndex = 8
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FindBooksB
        '
        Me.FindBooksB.Name = "FindBooksB"
        Me.FindBooksB.Size = New System.Drawing.Size(99, 24)
        Me.FindBooksB.Text = "寻找教材(&F)"
        '
        'logmm
        '
        Me.logmm.Name = "logmm"
        Me.logmm.Size = New System.Drawing.Size(159, 24)
        Me.logmm.Text = "使用登录模式下载(&L)"
        '
        'sxam
        '
        Me.sxam.Name = "sxam"
        Me.sxam.Size = New System.Drawing.Size(130, 24)
        Me.sxam.Text = "设置登录信息(&S)"
        '
        'toolm
        '
        Me.toolm.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.readmm, Me.downmm, Me.dllinm, Me.txtcnvm, Me.txtspm})
        Me.toolm.Name = "toolm"
        Me.toolm.Size = New System.Drawing.Size(100, 24)
        Me.toolm.Text = "实用工具(&T)"
        '
        'readmm
        '
        Me.readmm.Name = "readmm"
        Me.readmm.Size = New System.Drawing.Size(231, 24)
        Me.readmm.Text = "批量解析(&R)"
        '
        'downmm
        '
        Me.downmm.Name = "downmm"
        Me.downmm.Size = New System.Drawing.Size(231, 24)
        Me.downmm.Text = "批量下载(&D)"
        '
        'dllinm
        '
        Me.dllinm.Name = "dllinm"
        Me.dllinm.Size = New System.Drawing.Size(231, 24)
        Me.dllinm.Text = "下载链接(&K)"
        '
        'txtcnvm
        '
        Me.txtcnvm.Name = "txtcnvm"
        Me.txtcnvm.Size = New System.Drawing.Size(231, 24)
        Me.txtcnvm.Text = "多行文本合并为一行(&E)"
        '
        'txtspm
        '
        Me.txtspm.Name = "txtspm"
        Me.txtspm.Size = New System.Drawing.Size(231, 24)
        Me.txtspm.Text = "文本分割(&P)"
        '
        'hlpdocm
        '
        Me.hlpdocm.Name = "hlpdocm"
        Me.hlpdocm.Size = New System.Drawing.Size(103, 24)
        Me.hlpdocm.Text = "使用教程(&H)"
        '
        'ofpm
        '
        Me.ofpm.Name = "ofpm"
        Me.ofpm.Size = New System.Drawing.Size(103, 24)
        Me.ofpm.Text = "软件官网(&O)"
        '
        'aboutm
        '
        Me.aboutm.Name = "aboutm"
        Me.aboutm.Size = New System.Drawing.Size(72, 24)
        Me.aboutm.Text = "关于(&A)"
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(609, 152)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(224, 300)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 9
        Me.PictureBox1.TabStop = False
        '
        'Button2
        '
        Me.Button2.Enabled = False
        Me.Button2.Location = New System.Drawing.Point(424, 641)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(134, 39)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "保存书籍信息"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Enabled = False
        Me.Button3.Location = New System.Drawing.Point(707, 641)
        Me.Button3.Margin = New System.Windows.Forms.Padding(4)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(134, 39)
        Me.Button3.TabIndex = 11
        Me.Button3.Text = "保存电子书"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Enabled = False
        Me.Button4.Location = New System.Drawing.Point(565, 641)
        Me.Button4.Margin = New System.Windows.Forms.Padding(4)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(134, 39)
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
        Me.Label3.Location = New System.Drawing.Point(12, 38)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(605, 23)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "如果登录状态失效或者X-Nd-Auth信息错误，可以在""设置登录信息""菜单修改。"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(120.0!, 120.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(854, 704)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BookTagLabel)
        Me.Controls.Add(Me.BookIDLabel)
        Me.Controls.Add(Me.BookNameLabel)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("微软雅黑", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SmartEduDownloader"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents FindBooksB As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents aboutm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents toolm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents readmm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents downmm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents hlpdocm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ofpm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents logmm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sxam As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dllinm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtcnvm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtspm As System.Windows.Forms.ToolStripMenuItem

End Class
