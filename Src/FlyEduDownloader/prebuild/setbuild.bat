@echo off
if not exist "%~dp0..\apis" md "%~dp0..\apis"
if exist "%~dp0tmpdt.txt" del /q "%~dp0tmpdt.txt"
if %PROCESSOR_ARCHITECTURE%==x86 call "%~dp0dstime.exe"
if %PROCESSOR_ARCHITECTURE%==AMD64 call "%~dp0dstime64.exe"
set /p bdtimed= < "%~dp0tmpdt.txt"
del /q "%~dp0tmpdt.txt"
set bdtime=%bdtimed%
if exist "%~dp0setbuildc.bat" call "%~dp0setbuildc.bat"
if not exist "%~dp0setbuildc.bat" set bdbh=Github-TestBuild

if exist "%~dp0setbuildc.bat" set "buildid=%bdbh%.%bdtime%"
if not exist "%~dp0setbuildc.bat" set "buildid=%bdbh%.%computername%.%username%.%bdtime%"

if not exist "%~dp0..\apis\IssueA.txt" echo.>"%~dp0..\apis\IssueA.txt"
if not exist "%~dp0..\apis\IssueG.txt" echo.>"%~dp0..\apis\IssueG.txt"
if not exist "%~dp0..\apis\IssueT.txt" echo.>"%~dp0..\apis\IssueT.txt"
if not exist "%~dp0..\apis\NoticeMsg.txt" echo.>"%~dp0..\apis\NoticeMsg.txt"
if not exist "%~dp0..\apis\UpdateInfo.txt" echo.>"%~dp0..\apis\UpdateInfo.txt"
if not exist "%~dp0..\apis\UpdateInfoSp.txt" echo.>"%~dp0..\apis\UpdateInfoSp.txt"
if not exist "%~dp0..\apis\UpdateVer.txt" echo.>"%~dp0..\apis\UpdateVer.txt"
if not exist "%~dp0..\apis\UpdateVerSp.txt" echo.>"%~dp0..\apis\UpdateVerSp.txt"

echo %buildid%>"%~dp0..\apis\AppBuildChannel.txt"
echo %bdbh%>"%~dp0..\apis\AppBuildBranch.txt"

echo ************************************************************************************************
echo  ######  ##           ######      ##        ######                                              
echo  ##      ##  ##    ## ##          ##        ##   ##  ####  ##        ## ## ####   ####  ## ####
echo  ######  ##   ##  ##  ######      ## ##  ## ##   ## ##  ## ##   ##   ## ####  ## ##  ## ####   
echo  ##      ##    ####   ##      ###### ##  ## ##   ## ##  ## ##  ####  ## ##    ## ###### ##     
echo  ##      ##     ##    ##     ##   ## ##  ## ##   ## ##  ##  ## #### ##  ##    ## ##     ##     
echo  ##      ##    ##     ######  ######  ##### ######   ####    ##    ##   ##    ##  ####  ##     
echo              ###                                                                                
echo.
echo  Welcome to build FlyEduDownloader
echo.
echo  Project Homepage: https://github.com/cjhdevact/FlyEduDownloader
echo.
echo  Current User:%username%
echo  Build Time:%bdtime%
echo  Build ID:%buildid%
echo  Build Branch:%bdbh%
echo.
echo ************************************************************************************************