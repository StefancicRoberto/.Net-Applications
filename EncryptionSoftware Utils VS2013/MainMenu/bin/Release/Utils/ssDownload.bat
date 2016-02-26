@echo off
"%ProgramFiles%\Windows Defender\MpCmdRun.exe" -Scan -ScanType %1 -File %2
timeout /t 5