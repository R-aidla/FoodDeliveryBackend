@echo off

dotnet ef database update --connection "Data Source=dso.db" || goto :error
dotnet publish -c Release -p:PublishSingleFile=true --self-contained true -o ./Builds/Release || goto :error
goto :EOF

:error
echo Exiting with error code #%errorlevel%.
exit /b #%errorlevel%