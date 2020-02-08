@echo off
setlocal enabledelayedexpansion

if defined VS140COMNTOOLS (
    call "%VS140COMNTOOLS%vsvars32.bat"
) else (
    for /f "usebackq delims=" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -prerelease -latest -property installationPath`) do (
        if exist "%%i\Common7\Tools\vsdevcmd.bat" (
            call "%%i\Common7\Tools\vsdevcmd.bat"
            )
        )
)

pushd %~dp0dependencies\premake
if not exist bin\release\premake5.exe nmake -f Bootstrap.mak MSDEV=vs2015 windows
popd

"%~dp0dependencies\premake\bin\release\premake5.exe" vs2017
if %errorlevel% geq 1 (
    pause
)
exit /b