# Запуск первого проекта в текущем окне
cd .\Backend\EmitterPersonalAccount.API
dotnet run

# Запуск остальных проектов в фоновом режиме
Start-Process powershell -ArgumentList "-NoExit", "-Command cd ..\DocumentsService; dotnet run"
Start-Process powershell -ArgumentList "-NoExit", "-Command cd ..\EmailSender; dotnet run"
Start-Process powershell -ArgumentList "-NoExit", "-Command cd ..\Registrator.API; dotnet run"
Start-Process powershell -ArgumentList "-NoExit", "-Command cd ..\ExternalOrderReportsService; dotnet run"
