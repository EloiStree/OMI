@echo off
setlocal enabledelayedexpansion

set "WindowTitle=UDP 2 vJoys Python" 

for /f "tokens=*" %%a in ('tasklist /v /fo csv ^| findstr /i "%WindowTitle%"') do (
    for /f tokens^=2^ delims^=^" %%b in ("%%a") do (
        set "PID=%%b"
        taskkill /F /PID !PID!
        echo Process with window title '!WindowTitle!' (PID !PID!) terminated.
    )
)

endlocal