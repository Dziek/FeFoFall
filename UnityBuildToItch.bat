SET NAME=SwissFall
SET ITCHNAME=FeFoFall
SET MYPATH=C:\Users\Dziek\Documents\UnityProjects\QuickProjects

"C:\Program Files\Unity\Editor\Unity.exe" -quit -batchmode -projectPath %MYPATH%\%NAME% -executeMethod EditorScript.PerformBuild
REM butler push %MYPATH%\%NAME%\Builds\PC dziek/%ITCHNAME%:windows
butler push %MYPATH%\%NAME%\Builds\WebGL dziek/%ITCHNAME%:html5
cmd /k 