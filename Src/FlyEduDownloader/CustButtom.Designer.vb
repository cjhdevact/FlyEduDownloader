'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：System.Windows.Forms.Button.Designer.vb
'描述：自定义按钮
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
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class System.Windows.Forms.Button
    Inherits System.Windows.Forms.Button

    'UserControl 重写 Dispose，以清理组件列表。
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
        components = New System.ComponentModel.Container()
        Me.FlatStyle = FlatStyle.Flat
        Me.BackColor = Color.Gainsboro
        Me.FlatAppearance.BorderSize = 1
        'Me.ForeColor = Color.Black
        Me.FlatAppearance.BorderColor = Color.Gainsboro
        Me.FlatAppearance.MouseDownBackColor = Color.DarkGray
        Me.FlatAppearance.MouseOverBackColor = Color.LightGray
    End Sub

End Class
