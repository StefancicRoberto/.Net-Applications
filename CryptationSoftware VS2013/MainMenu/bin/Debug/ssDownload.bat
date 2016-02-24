@echo off
"%ProgramFiles%\Windows Defender\MpCmdRun.exe" -Scan -ScanType 3 -File %2
timeout /t 5