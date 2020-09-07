@Echo off

:: Require admin mode, from https://stackoverflow.com/a/52517718
set "params=%*"
cd /d "%~dp0" && ( if exist "%temp%\getadmin.vbs" del "%temp%\getadmin.vbs" ) && fsutil dirty query %systemdrive% 1>nul 2>nul || (  echo Set UAC = CreateObject^("Shell.Application"^) : UAC.ShellExecute "cmd.exe", "/k cd ""%~sdp0"" && %~s0 %params%", "", "runas", 1 >> "%temp%\getadmin.vbs" && "%temp%\getadmin.vbs" && exit /B )

SET source="NetCoreNetworking\Samples~"
SET target=Samples

:: Delete folder or broken file
if exist %target%\NUL (rmdir %target%) else (if exist %target% del %target%)

:: regenerate symlink (needs admin rights)
mklink /D %target% %source% 

PAUSE
EXIT