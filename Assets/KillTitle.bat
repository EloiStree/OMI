@echo off
setlocal enabledelayedexpansion

REM List of process titles to be killed
set "ProcessTitles =_StartVJoy.bat UDP2vJoy.py _KillProcessByNames.py _KillProcess_X_ByTitle.bat"

REM Iterate through each process title and kill the corresponding processes
for %% i in (% ProcessTitles %) do (
    for / f "tokens =2 delims=," %% a in ('tasklist /v /fo csv ^| findstr /i " %%i"') do (
          set "PID =%%~a"
        taskkill / pid!PID! / f
        echo Process with title " %%i" and PID !PID! has been terminated.
    )
)
endlocal
pause