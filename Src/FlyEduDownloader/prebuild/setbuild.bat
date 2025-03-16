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