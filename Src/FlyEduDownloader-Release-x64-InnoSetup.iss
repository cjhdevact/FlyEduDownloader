﻿;****************************************************************
;PROJECT: FlyEduDownloader
;FILE: FlyEduDownloader-Release-x64-InnoSetup.iss
;PURPOSE: Inno Setup Script.
;AUTHOR: CJH
;****************************************************************


;Inno Setup Script File Version 5.4.0


; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{85F3616B-7A4C-4EED-B00B-DF6866141220}
AppName=飞翔教学资源助手
AppVersion=1.0.6.24111
;AppVerName=FlyEduDownloader 1.0.6.24111
AppPublisher=CJH
DefaultDirName={pf}\CJH\FlyEduDownloader\x64
DefaultGroupName=飞翔教学资源助手
AllowNoIcons=yes
LicenseFile=..\License
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputDir=.\FlyEduDownloader\setupbin\release
OutputBaseFilename=FlyEduDownloader_1.0.6.24111_x64_setup
SetupIconFile=.\FlyEduDownloader\res\FlyEduDownloader.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

VersionInfoCopyright=Copyright © 2024 CJH. All Rights Reserved.
VersionInfoVersion=1.0.6.24111
VersionInfoProductName=FlyEduDownloader 1.0.6.24111
VersionInfoProductVersion=1.0.6.24111
VersionInfoCompany=CJH
VersionInfoDescription=FlyEduDownloader Setup

WizardImageFile=WizModernImage.bmp
WizardSmallImageFile=WizModernSmallImage.bmp

;UninstallDisplayIcon=.\FlyEduDownloader\FlyEduDownloader.ico
UninstallDisplayName=飞翔教学资源助手 1.0.6.24111 (x64)

; "ArchitecturesAllowed=x64" specifies that Setup cannot run on
; anything but x64.
ArchitecturesAllowed=x64
; "ArchitecturesInstallIn64BitMode=x64" requests that the install be
; done in "64-bit mode" on x64, meaning it should use the native
; 64-bit Program Files directory and the 64-bit view of the registry.
ArchitecturesInstallIn64BitMode=x64
[Languages]
Name: "chinesesimplified"; MessagesFile: "compiler:Languages\ChineseSimplified.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkablealone

[InstallDelete]
; Removed old files
Type: files; Name: "{app}\FlyEduDownloader.exe"
Type: files; Name: "{app}\Newtonsoft.Json.dll"
; Removed old links
Type: files; Name: "{group}\FlyEduDownloader 1.0.2.24091 (x64).lnk"
Type: files; Name: "{commondesktop}\FlyEduDownloader 1.0.2.24091 (x64).lnk"
Type: files; Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\FlyEduDownloader 1.0.2.24091 (x64).lnk"

[UninstallRun]
Filename: "https://cjhdevact.github.io/otherprojects/FlyEduDownloader/aslink.html"; Flags: shellexec runmaximized; Tasks: ; Languages:

[Files]
Source: ".\FlyEduDownloader\bin\x64\Release\FlyEduDownloader.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\FlyEduDownloader\libs\miniblink\x64\miniblink.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\FlyEduDownloader\bin\x64\Release\Miniblink.NetLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\FlyEduDownloader\bin\x64\Release\O2S.Components.PDFRender4NET.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\License"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\FlyEduDownloader\bin\x64\Release\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\飞翔教学资源助手"; Filename: "{app}\FlyEduDownloader.exe"
Name: "{commondesktop}\飞翔教学资源助手"; Filename: "{app}\FlyEduDownloader.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\FlyEduDownloader.exe"; Parameters: "/unloginmode"; Description: "{cm:LaunchProgram,FlyEduDownloader}"; Flags: nowait postinstall skipifsilent

[Messages]

DialogFontName=宋体
DialogFontSize=9
WelcomeFontName=宋体
WelcomeFontSize=12
TitleFontName=宋体
TitleFontSize=29
CopyrightFontName=宋体
CopyrightFontSize=8

; *** 应用程序标题
SetupAppTitle=安装
SetupWindowTitle=安装 飞翔教学资源助手
UninstallAppTitle=卸载
UninstallAppFullTitle=卸载 飞翔教学资源助手

; *** 启动问题
PrivilegesRequiredOverrideTitle=选择安装模式
PrivilegesRequiredOverrideInstruction=选择安装模式
PrivilegesRequiredOverrideText1=飞翔教学资源助手 可以为所有用户安装（需要管理员权限），或仅为您安装。
PrivilegesRequiredOverrideText2=飞翔教学资源助手 只能为您安装，或为所有用户安装（需要管理员权限）。
PrivilegesRequiredOverrideAllUsers=为所有用户安装(&A)
PrivilegesRequiredOverrideAllUsersRecommended=为所有用户安装(&A)（建议选项）
PrivilegesRequiredOverrideCurrentUser=只为我安装(&M)
PrivilegesRequiredOverrideCurrentUserRecommended=只为我安装(&M)（建议选项）

; *** 安装程序公共消息
ExitSetupTitle=退出安装程序
ExitSetupMessage=安装程序还未完成安装。如果您现在退出，程序将不能安装。%n%n您可以以后再运行安装程序完成安装。%n%n现在退出安装程序吗？

; *** 按钮
ButtonBack=上一步(&B)
ButtonNext=下一步(&N)
ButtonInstall=安装(&I)
ButtonOK=确定
ButtonCancel=取消(&C)
ButtonYes=是(&Y)
ButtonYesToAll=全是(&A)
ButtonNo=否(&N)
ButtonNoToAll=全否(&O)
ButtonFinish=完成(&F)
ButtonBrowse=浏览(&B)
ButtonWizardBrowse=浏览(&R)
ButtonNewFolder=新建文件夹(&M)

; *** 公共向导文字
ClickNext=点击“下一步”继续，或点击“取消”退出安装程序。
BeveledLabel=飞翔教学资源助手 安装程序
BrowseDialogTitle=浏览文件夹
BrowseDialogLabel=在下列列表中选择一个文件夹，然后点击“确定”。
NewFolderName=新建文件夹

; *** “欢迎”向导页
WelcomeLabel1=欢迎安装 飞翔教学资源助手！
WelcomeLabel2=现在将安装 飞翔教学资源助手 到您的电脑中。%n%n推荐您在继续安装前关闭所有其它应用程序。%n%n编译日期：2024-11-02。%n%n版本：1.0.6.24111 x64

; *** “许可协议”向导页
WizardLicense=许可协议
LicenseLabel=继续安装前请阅读下列重要信息。
LicenseLabel3=请仔细阅读下列许可协议。您在继续安装前必须同意这些协议条款。
LicenseAccepted=我同意此协议(&A)
LicenseNotAccepted=我不同意此协议(&D)

; *** “选择目标目录”向导页
WizardSelectDir=选择目标位置
SelectDirDesc=您想将 飞翔教学资源助手 安装在哪里？
SelectDirLabel3=安装程序将安装 飞翔教学资源助手 到下列文件夹中。
SelectDirBrowseLabel=点击“下一步”继续。如果您想选择其它文件夹，点击“浏览”。
DiskSpaceGBLabel=至少需要有 [gb] GB 的可用磁盘空间。
DiskSpaceMBLabel=至少需要有 [mb] MB 的可用磁盘空间。
CannotInstallToNetworkDrive=安装程序无法安装到一个网络驱动器。
CannotInstallToUNCPath=安装程序无法安装到一个UNC路径。
InvalidPath=您必须输入一个带驱动器卷标的完整路径，例如：%n%nC:\APP%n%n或下列形式的UNC路径：%n%n\\server\share
InvalidDrive=您选定的驱动器或 UNC 共享不存在或不能访问。请选选择其它位置。
DiskSpaceWarningTitle=没有足够的磁盘空间
DiskSpaceWarning=安装程序至少需要 %1 KB 的可用空间才能安装，但选定驱动器只有 %2 KB 的可用空间。%n%n您一定要继续吗？
DirNameTooLong=文件夹名称或路径太长。
InvalidDirName=文件夹名称无效。
BadDirName32=文件夹名称不能包含下列任何字符：%n%n%1
DirExistsTitle=文件夹已存在
DirExists=文件夹：%n%n%1%n%n已经存在。您一定要安装到这个文件夹中吗？
DirDoesntExistTitle=文件夹不存在
DirDoesntExist=文件夹 %1 不存在。您想要创建此文件夹吗？

; *** “选择开始菜单文件夹”向导页
WizardSelectProgramGroup=选择开始菜单文件夹
SelectStartMenuFolderDesc=应该在哪里放置程序的开始菜单快捷方式？
SelectStartMenuFolderLabel3=安装程序现在将在下列开始菜单文件夹中创建程序的快捷方式。
SelectStartMenuFolderBrowseLabel=点击“下一步”继续。如果您想选择其它文件夹，点击“浏览”。
MustEnterGroupName=您必须输入一个文件夹名。
GroupNameTooLong=文件夹名或路径太长。
InvalidGroupName=文件夹名是无效的。
BadGroupName=文件夹名不能包含下列任何字符：%n%n%1
NoProgramGroupCheck2=不创建开始菜单文件夹(&D)

; *** “准备安装”向导页
WizardReady=准备安装
ReadyLabel1=安装程序现在准备开始安装 飞翔教学资源助手 到您的电脑中。
ReadyLabel2a=点击“安装”继续此安装程序。如果您想要回顾或修改设置，请点击“上一步”。
ReadyLabel2b=点击“安装”继续此安装程序？
ReadyMemoUserInfo=用户信息：
ReadyMemoDir=目标位置：
ReadyMemoType=安装类型：
ReadyMemoComponents=选定组件：
ReadyMemoGroup=开始菜单文件夹：
ReadyMemoTasks=附加任务：

; *** “正在准备安装”向导页
WizardPreparing=正在准备安装
PreparingDesc=安装程序正在准备安装 飞翔教学资源助手 到您的电脑中。
PreviousInstallNotCompleted=先前程序的安装/卸载未完成。您需要重新启动您的电脑才能完成安装。%n%n在重新启动电脑后，再运行 飞翔教学资源助手 的安装程序。
CannotContinue=安装程序不能继续。请点击“取消”退出。
ApplicationsFound=下列应用程序正在使用的文件需要更新设置。它是建议您允许安装程序自动关闭这些应用程序。
ApplicationsFound2=下列应用程序正在使用的文件需要更新设置。它是建议您允许安装程序自动关闭这些应用程序。安装完成后，安装程序将尝试重新启动应用程序。
CloseApplications=自动关闭该应用程序(&A)
DontCloseApplications=不要关闭该应用程序(&D)
ErrorCloseApplications=安装程序无法自动关闭所有应用程序。在继续之前，我们建议您关闭所有使用需要更新的安装程序文件。
PrepareToInstallNeedsRestart=安装程序必须重新启动计算机。重新启动计算机后，请再次运行安装程序以完成 [name] 的安装。%n%n是否立即重新启动？

; *** “正在安装”向导页
WizardInstalling=正在安装
InstallingLabel=安装程序正在安装 飞翔教学资源助手 到您的电脑中，请稍等。

; *** “安装完成”向导页
FinishedHeadingLabel=飞翔教学资源助手 安装完成
FinishedLabelNoIcons=安装程序已在您的电脑中安装了 飞翔教学资源助手。
FinishedLabel=安装程序已在您的电脑中安装了 飞翔教学资源助手。此应用程序可以通过选择安装的快捷方式运行。
ClickFinish=点击“完成”退出安装程序。
FinishedRestartLabel=要完成 飞翔教学资源助手 的安装，安装程序必须重新启动您的电脑。您想要立即重新启动吗？
FinishedRestartMessage=要完成 飞翔教学资源助手 的安装，安装程序必须重新启动您的电脑。%n%n您想要立即重新启动吗？
ShowReadmeCheck=是，我想查阅自述文件
YesRadio=是，立即重新启动电脑(&Y)
NoRadio=否，稍后重新启动电脑(&N)
;运行 飞翔教学资源助手
RunEntryExec=运行 飞翔教学资源助手
;阅读更新日志
RunEntryShellExec=阅读更新日志

; *** 卸载消息
UninstallNotFound=文件“%1”不存在。无法卸载。
UninstallOpenError=文件“%1”不能打开。无法卸载。
UninstallUnsupportedVer=此版本的卸载程序无法识别卸载参数文件“%1”的格式。无法卸载
UninstallUnknownEntry=在卸载日志中遇到一个未知的条目 (%1)
ConfirmUninstall=确定卸载飞翔教学资源助手吗？%n%n卸载之后你将无法快速方便下载教学资源，是否要卸载？
UninstallOnlyOnWin64=这个卸载程序只能在 Windows x64 中进行卸载。
OnlyAdminCanUninstall=这个卸载程序需要有管理员权限的用户才能卸载。
UninstallStatusLabel=正在从您的电脑中删除 飞翔教学资源助手，请稍等。
UninstalledAll=飞翔教学资源助手 已从您的电脑中删除。
UninstalledMost=飞翔教学资源助手 卸载完成。%n%n有一些内容无法被删除。您可以手动删除它们。
UninstalledAndNeedsRestart=要完成 飞翔教学资源助手 的卸载，您的电脑必须重新启动。%n%n您想立即重新启动电脑吗？
UninstallDataCorrupted=文件“%1”已损坏，无法卸载

; *** 卸载状态消息
ConfirmDeleteSharedFileTitle=删除共享文件吗？
ConfirmDeleteSharedFile2=系统中包含的下列共享文件已经不再被其它程序使用。您想要卸载程序删除这些共享文件吗？%n%n如果这些文件被删除，但还有程序正在使用这些文件，这些程序可能不能正确执行。如果您不能确定，选择“否”。把这些文件保留在系统中以免引起问题。
SharedFileNameLabel=文件名：
SharedFileLocationLabel=位置：
WizardUninstalling=卸载状态
StatusUninstalling=正在卸载 飞翔教学资源助手...

; *** Shutdown block reasons
ShutdownBlockReasonInstallingApp=正在安装 飞翔教学资源助手。
ShutdownBlockReasonUninstallingApp=正在卸载 飞翔教学资源助手。

[CustomMessages]

NameAndVersion=%1 %2
AdditionalIcons=快捷方式：
CreateDesktopIcon=创建桌面快捷方式(&D)
CreateQuickLaunchIcon=创建快速运行栏快捷方式(&Q)
ProgramOnTheWeb=%1 网站
UninstallProgram=卸载 飞翔教学资源助手
LaunchProgram=运行 飞翔教学资源助手
AssocFileExtension=将文件扩展名： %2 与 %1 建立关联(&A)
AssocingFileExtension=正在将 %2 文件扩展名与 %1 建立关联...
AutoStartProgramGroupDescription=启动组：
AutoStartProgram=自动启动 %1
AddonHostProgramNotFound=%1无法找到您所选择的文件夹。%n%n您想要继续吗？

[Code]
var
ErrorCode: Integer;
IsRunning: Integer;

//在注册表中插入DisplayIcon项，指定安装卸载页面的程序图标；
function SetUninstallIcon(iconPath:string): Boolean;
var
  InstalledVersion,SubKeyName: String;
begin
if (IsWin64()) then begin
//自己的appID
SubKeyName :=  'Software\Microsoft\Windows\CurrentVersion\Uninstall\{85F3616B-7A4C-4EED-B00B-DF6866141220}_is1';
    RegWriteStringValue(HKLM,SubKeyName,'DisplayIcon',iconPath);
  end;

//卸载显示名
if (IsWin64()) then begin
//自己的appID
SubKeyName :=  'Software\Microsoft\Windows\CurrentVersion\Uninstall\{85F3616B-7A4C-4EED-B00B-DF6866141220}_is1';
    RegWriteStringValue(HKLM,SubKeyName,'DisplayName','飞翔教学资源助手 1.0.6.24111 (x64)');
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then
  RegDeleteKeyIncludingSubkeys(HKEY_CURRENT_USER, 'Software\CJH\FlyEduDownloader')
end;

procedure InitializeWizard();
begin
WizardForm.LICENSEACCEPTEDRADIO.Checked := true;
end;

// 安装时判断程序是否正在运行
function InitializeSetup(): Boolean;
begin
	Result :=true; //安装程序继续
	ShellExec('open', ExpandConstant('{cmd}'), '/c taskkill /f /t /im FlyEduDownloader.exe', '', SW_HIDE, ewNoWait, ErrorCode);
	IsRunning:=FindWindowByWindowName('飞翔教学资源助手');
	while IsRunning<>0 do
		begin
		if Msgbox('安装程序发现 飞翔教学资源助手 正在运行。'#13 #13 '请先关闭 飞翔教学资源助手 按“是”继续安装，或者按“否”退出！', mbCriticalError, MB_YESNO) = idNO then
			begin
			Result :=false; //安装程序退出
			IsRunning :=0;
		end else begin
			Result :=true; //安装程序继续
			IsRunning:=FindWindowByWindowName('飞翔教学资源助手');
		end;
	end;
end;

procedure CurPageChanged(CurPageID: Integer);
begin
  if CurPageID = wpFinished then
  begin
    SetUninstallIcon(ExpandConstant('{app}\FlyEduDownloader.exe'));
  end;
end;

//安装时卸载旧版本
procedure CurStepChanged(CurStep: TSetupStep);
var 
ResultStr: String; 
ResultCode: Integer; 
begin
  if CurStep = ssInstall then
  begin
  if RegQueryStringValue(HKLM, 'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{85F3616B-7A4C-4EED-B00B-DF6866141220}_is1', 'UninstallString', ResultStr) then 
	begin 
		ResultStr := RemoveQuotes(ResultStr); 
		Exec(ResultStr, '/verysilent /norestart /suppressmsgboxes', '', SW_HIDE, ewWaitUntilTerminated, ResultCode); 
	end; 
  end;
end;

// 卸载时判断程序是否正在运行
function InitializeUninstall(): Boolean;
begin
  Result :=true; //卸载程序继续
  ShellExec('open', ExpandConstant('{cmd}'), '/c taskkill /f /t /im FlyEduDownloader.exe', '', SW_HIDE, ewNoWait, ErrorCode);
  IsRunning:=FindWindowByWindowName('飞翔教学资源助手');
  while IsRunning<>0 do
    begin
    if Msgbox('卸载程序发现 飞翔教学资源助手 正在运行。'#13 #13 '请先手动关闭或者按“是”关闭 飞翔教学资源助手。按“是”继续卸载，或者按“否”退出！',mbCriticalError, MB_YESNO) = idNO then
      begin
      Result :=false; //安装程序退出
      IsRunning :=0;
    end else begin
      Result :=true; //卸载程序继续
      IsRunning:=FindWindowByWindowName('飞翔教学资源助手');
    end;
  end;
end;