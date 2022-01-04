@echo off

set _programFiles=%ProgramFiles%

set _msbuildPath="%_programFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin"

%_msbuildPath%\msbuild "..\src\CommandLine.sln" /t:Build /p:Configuration=Debug /v:m /m

"..\src\CommandLine\bin\Debug\net48\roslynator" lloc "..\src\Roslynator.sln" ^
 -m %_msbuildPath% ^
 --verbosity d ^
 --file-log "roslynator.log" ^
 --file-log-verbosity diag

pause
