::Tips Set the CSIGNCERT as your path.
@echo off
path D:\ProjectsTmp\SignPack;%path%
echo 任意键签名 飞翔教学资源助手（FlyEduDownloader）...
pause > nul
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0FlyEduDownloader\bin\Release\FlyEduDownloader.exe"
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0FlyEduDownloader\bin\Release\Miniblink.NetLib.dll"
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0FlyEduDownloader\bin\Release\x64\FlyEduDownloader.exe"
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0FlyEduDownloader\bin\Release\x64\Miniblink.NetLib.dll"
echo.
echo 完成！
echo 任意键退出...
pause > nul