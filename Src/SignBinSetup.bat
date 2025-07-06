::Tips Set the CSIGNCERT as your path.
@echo off
path D:\ProjectsTmp\SignPack;%path%
echo 任意键签名 飞翔教学资源助手安装程序（FlyEduDownloader Setup）...
pause > nul
Set apver=1.0.10.25071
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0FlyEduDownloader\setupbin\release\fed-%apver%-x64-up.exe"
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0FlyEduDownloader\setupbin\release\fed-%apver%-x86-up.exe"
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0FlyEduDownloader\setupbin\release\FlyEduDownloader_%apver%_x64_setup.exe"
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0FlyEduDownloader\setupbin\release\FlyEduDownloader_%apver%_x86_setup.exe"
echo.
echo 完成！
echo 任意键退出...
pause > nul