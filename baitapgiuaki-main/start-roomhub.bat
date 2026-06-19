@echo off
setlocal
cd /d "%~dp0"
title RoomHub BCS240032
set PROJECT_FILE=%~dp0Lesson3-CNLTWeb.csproj

echo ============================================================
echo  RoomHub BCS240032 - one-click local starter
echo ============================================================
echo.

where dotnet >nul 2>nul
if errorlevel 1 (
    echo ERROR: dotnet SDK was not found in PATH.
    echo Install .NET 10 SDK, then run this file again.
    pause
    exit /b 1
)

powershell -NoProfile -ExecutionPolicy Bypass -Command "$svc = Get-Service -Name 'MSSQLSERVER' -ErrorAction SilentlyContinue; if ($svc -and $svc.Status -ne 'Running') { try { Start-Service -Name 'MSSQLSERVER' -ErrorAction Stop; Write-Host 'SQL Server MSSQLSERVER started.' } catch { Write-Host 'Could not start SQL Server automatically. If the app cannot connect, start SQL Server manually or run this file as Administrator.' } }"

echo Restoring packages...
dotnet restore "%PROJECT_FILE%"
if errorlevel 1 (
    echo.
    echo ERROR: dotnet restore failed.
    pause
    exit /b 1
)

set ASPNETCORE_ENVIRONMENT=Development
set ASPNETCORE_URLS=http://localhost:5036

echo.
echo Starting RoomHub at http://localhost:5036
echo Database: localhost / MID_BCS240032
echo The database and sample BCS240032 data are created automatically on first run.
echo Press Ctrl+C in this window to stop the server.
echo.

start "" powershell -NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -Command "for ($i = 0; $i -lt 60; $i++) { try { Invoke-WebRequest -Uri 'http://localhost:5036' -UseBasicParsing -TimeoutSec 1 | Out-Null; Start-Process 'http://localhost:5036'; break } catch { Start-Sleep -Seconds 1 } }"

dotnet run --project "%PROJECT_FILE%" --no-launch-profile

echo.
echo Server stopped.
pause
