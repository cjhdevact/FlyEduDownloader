'==========================================
'项目：FlyEduDownloader
'作者：CJH
'文件：RegKeyModule.vb
'描述：注册表处理模块
'License：
'FlyEduDownloader
'Copyright (C) 2024 CJH.

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
Module RegKeyModule
    Public regkindstr As String ' Reg Kind String format

    '写入注册表值
    'Public Sub addhklmreg(ByVal regkey As String, ByVal regname As String, ByRef regword as object, ByVal regtype As Microsoft.Win32.RegistryValueKind)
    Public Sub AddReg(ByVal regkey, ByVal regname, ByVal regword, ByVal regtype, ByVal regmo)
        If regtype = Microsoft.Win32.RegistryValueKind.String Then
            regkindstr = "REG_SZ"
        ElseIf regtype = Microsoft.Win32.RegistryValueKind.Binary Then
            regkindstr = "REG_BINARY"
        ElseIf regtype = Microsoft.Win32.RegistryValueKind.DWord Then
            regkindstr = "REG_DWORD"
        ElseIf regtype = Microsoft.Win32.RegistryValueKind.QWord Then
            regkindstr = "REG_QWORD"
        ElseIf regtype = Microsoft.Win32.RegistryValueKind.MultiString Then
            regkindstr = "REG_MULTI_SZ"
        ElseIf regtype = Microsoft.Win32.RegistryValueKind.ExpandString Then
            regkindstr = "REG_EXPAND_SZ"
        ElseIf regtype = Microsoft.Win32.RegistryValueKind.None Then
            regkindstr = "REG_NONE"
            'ElseIf regtype = Microsoft.Win32.RegistryValueKind.Unknown Then
        Else
            regkindstr = "REG_UNKNOWN"
        End If

        Dim regm As Object
        Dim regms As String
        '默认的Regm（如果出现错误）
        regm = Registry.CurrentUser
        If regmo.ToString.ToUpper = "HKLM" Then
            regm = Registry.LocalMachine
            regms = "HKEY_LOCAL_MACHINEL\"
        ElseIf regmo.ToString.ToUpper = "HKEY_LOCAL_MACHINE" Then
            regm = Registry.LocalMachine
            regms = "HKEY_LOCAL_MACHINEL\"
        ElseIf regmo.ToString.ToUpper = "HKEY_CLASSES_ROOT" Then
            regm = Registry.ClassesRoot
            regms = "HKEY_CLASSES_ROOT\"
        ElseIf regmo.ToString.ToUpper = "HKCR" Then
            regm = Registry.ClassesRoot
            regms = "HKEY_CLASSES_ROOT\"
        ElseIf regmo.ToString.ToUpper = "HKEY_CURRENT_USER" Then
            regm = Registry.CurrentUser
            regms = "HKEY_CURRENT_USER\"
        ElseIf regmo.ToString.ToUpper = "HKCU" Then
            regm = Registry.CurrentUser
            regms = "HKEY_CURRENT_USER\"
        ElseIf regmo.ToString.ToUpper = "HKEY_USERS" Then
            regm = Registry.Users
            regms = "HKEY_USERS\"
        ElseIf regmo.ToString.ToUpper = "HKU" Then
            regm = Registry.Users
            regms = "HKEY_USERS\"
        ElseIf regmo.ToString.ToUpper = "HKEY_CURRENT_CONFIG" Then
            regm = Registry.CurrentConfig
            regms = "HKEY_CURRENT_CONFIG\"
        ElseIf regmo.ToString.ToUpper = "HKCC" Then
            regm = Registry.CurrentConfig
            regms = "HKEY_CURRENT_CONFIG\"
        End If

        Try
            Dim key2 As RegistryKey = regm.OpenSubKey(regkey, True)
            If (Not key2 Is Nothing) Then
                If (If((key2.GetValue("") Is Nothing), Nothing, key2.GetValue("").ToString) <> regname) Then
                    key2.SetValue(regname, regword, regtype)
                End If
                'If (key2.GetValue("PerceivedType") Is Nothing) Then
                '    key2.SetValue("PerceivedType", str3)
                'End If
                'If (key2.GetValue("Content Type") Is Nothing) Then
                '    key2.SetValue("Content Type", str6)
                'End If
                key2.Close()
            Else
                ' key2.Close()
                key2 = regm.CreateSubKey(regkey, RegistryKeyPermissionCheck.ReadWriteSubTree)
                key2.SetValue(regname, regword, regtype)
                'key2.SetValue("PerceivedType", str3)
                'key2.SetValue("Content Type", str6)
                key2.Close()
            End If
            'Microsoft.Win32.Registry.LocalMachine.CreateSubKey(regkey)
            'Microsoft.Win32.Registry.SetValue(regkey, regname, regword, regtype)
        Catch ex As Exception
        End Try
    End Sub

    '删除注册表值
    Public Sub DelReg(ByVal regkey, ByVal regname, ByVal regmo)
        Dim regm As Object
        Dim regms As String
        '默认的Regm（如果出现错误）
        regm = Registry.CurrentUser
        If regmo.ToString.ToUpper = "HKLM" Then
            regm = Registry.LocalMachine
            regms = "HKEY_LOCAL_MACHINEL\"
        ElseIf regmo.ToString.ToUpper = "HKEY_LOCAL_MACHINE" Then
            regm = Registry.LocalMachine
            regms = "HKEY_LOCAL_MACHINEL\"
        ElseIf regmo.ToString.ToUpper = "HKEY_CLASSES_ROOT" Then
            regm = Registry.ClassesRoot
            regms = "HKEY_CLASSES_ROOT\"
        ElseIf regmo.ToString.ToUpper = "HKCR" Then
            regm = Registry.ClassesRoot
            regms = "HKEY_CLASSES_ROOT\"
        ElseIf regmo.ToString.ToUpper = "HKEY_CURRENT_USER" Then
            regm = Registry.CurrentUser
            regms = "HKEY_CURRENT_USER\"
        ElseIf regmo.ToString.ToUpper = "HKCU" Then
            regm = Registry.CurrentUser
            regms = "HKEY_CURRENT_USER\"
        ElseIf regmo.ToString.ToUpper = "HKEY_USERS" Then
            regm = Registry.Users
            regms = "HKEY_USERS\"
        ElseIf regmo.ToString.ToUpper = "HKU" Then
            regm = Registry.Users
            regms = "HKEY_USERS\"
        ElseIf regmo.ToString.ToUpper = "HKEY_CURRENT_CONFIG" Then
            regm = Registry.CurrentConfig
            regms = "HKEY_CURRENT_CONFIG\"
        ElseIf regmo.ToString.ToUpper = "HKCC" Then
            regm = Registry.CurrentConfig
            regms = "HKEY_CURRENT_CONFIG\"
        End If

        Try
            Dim key2 As RegistryKey = regm.OpenSubKey(regkey, True)
            If (Not key2 Is Nothing) Then
                If (If((key2.GetValue("") Is Nothing), Nothing, key2.GetValue("").ToString) <> regname) Then
                    key2.DeleteValue(regname, True)
                End If
                key2.Close()
            Else
                '必须打开操作才能关闭？
                key2.DeleteValue(regname, True)
                key2.Close()
            End If
        Catch ex As Exception
        End Try
    End Sub


    '写入注册表项
    Public Sub AddKey(ByVal regkey, ByVal regmo)
        Dim regm As Object
        Dim regms As String
        '默认的Regm（如果出现错误）
        regm = Registry.CurrentUser
        If regmo.ToString.ToUpper = "HKLM" Then
            regm = Registry.LocalMachine
            regms = "HKEY_LOCAL_MACHINEL\"
        ElseIf regmo.ToString.ToUpper = "HKEY_LOCAL_MACHINE" Then
            regm = Registry.LocalMachine
            regms = "HKEY_LOCAL_MACHINEL\"
        ElseIf regmo.ToString.ToUpper = "HKEY_CLASSES_ROOT" Then
            regm = Registry.ClassesRoot
            regms = "HKEY_CLASSES_ROOT\"
        ElseIf regmo.ToString.ToUpper = "HKCR" Then
            regm = Registry.ClassesRoot
            regms = "HKEY_CLASSES_ROOT\"
        ElseIf regmo.ToString.ToUpper = "HKEY_CURRENT_USER" Then
            regm = Registry.CurrentUser
            regms = "HKEY_CURRENT_USER\"
        ElseIf regmo.ToString.ToUpper = "HKCU" Then
            regm = Registry.CurrentUser
            regms = "HKEY_CURRENT_USER\"
        ElseIf regmo.ToString.ToUpper = "HKEY_USERS" Then
            regm = Registry.Users
            regms = "HKEY_USERS\"
        ElseIf regmo.ToString.ToUpper = "HKU" Then
            regm = Registry.Users
            regms = "HKEY_USERS\"
        ElseIf regmo.ToString.ToUpper = "HKEY_CURRENT_CONFIG" Then
            regm = Registry.CurrentConfig
            regms = "HKEY_CURRENT_CONFIG\"
        ElseIf regmo.ToString.ToUpper = "HKCC" Then
            regm = Registry.CurrentConfig
            regms = "HKEY_CURRENT_CONFIG\"
        End If
        Try
            regm.CreateSubKey(regkey)
        Catch ex As Exception
        End Try
    End Sub

    '删除注册表项
    Public Sub DelKey(ByVal regkey, ByVal usetree, ByVal regmo)
        Dim regm As Object
        Dim regms As String
        '默认的Regm（如果出现错误）
        regm = Registry.CurrentUser
        If regmo.ToString.ToUpper = "HKLM" Then
            regm = Registry.LocalMachine
            regms = "HKEY_LOCAL_MACHINEL\"
        ElseIf regmo.ToString.ToUpper = "HKEY_LOCAL_MACHINE" Then
            regm = Registry.LocalMachine
            regms = "HKEY_LOCAL_MACHINEL\"
        ElseIf regmo.ToString.ToUpper = "HKEY_CLASSES_ROOT" Then
            regm = Registry.ClassesRoot
            regms = "HKEY_CLASSES_ROOT\"
        ElseIf regmo.ToString.ToUpper = "HKCR" Then
            regm = Registry.ClassesRoot
            regms = "HKEY_CLASSES_ROOT\"
        ElseIf regmo.ToString.ToUpper = "HKEY_CURRENT_USER" Then
            regm = Registry.CurrentUser
            regms = "HKEY_CURRENT_USER\"
        ElseIf regmo.ToString.ToUpper = "HKCU" Then
            regm = Registry.CurrentUser
            regms = "HKEY_CURRENT_USER\"
        ElseIf regmo.ToString.ToUpper = "HKEY_USERS" Then
            regm = Registry.Users
            regms = "HKEY_USERS\"
        ElseIf regmo.ToString.ToUpper = "HKU" Then
            regm = Registry.Users
            regms = "HKEY_USERS\"
        ElseIf regmo.ToString.ToUpper = "HKEY_CURRENT_CONFIG" Then
            regm = Registry.CurrentConfig
            regms = "HKEY_CURRENT_CONFIG\"
        ElseIf regmo.ToString.ToUpper = "HKCC" Then
            regm = Registry.CurrentConfig
            regms = "HKEY_CURRENT_CONFIG\"
        End If
        Try
            If usetree = True Then
                regm.DeleteSubKeyTree(regkey, True)
            Else
                regm.DeleteSubKey(regkey, True)
            End If
        Catch ex As Exception
        End Try
    End Sub
End Module

