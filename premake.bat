@echo off

call "%VS140COMNTOOLS%vsvars32.bat"

pushd %~dp0dependencies\premake
if not exist bin\release\premake5.exe nmake -f Bootstrap.mak windows
popd

"%~dp0dependencies\premake\bin\release\premake5.exe" vs2017
pause