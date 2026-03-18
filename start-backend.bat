@echo off
echo Starting Laravel Backend Server...
echo.

cd /d "%~dp0karton-backend"

echo Checking if .env file exists...
if not exist .env (
    echo .env file not found! Copying from .env.example...
    copy .env.example .env
    echo .env file created. Please configure it before running again.
    pause
    exit /b
)

echo Starting PHP development server...
echo Server will be available at http://localhost:8000
echo Press Ctrl+C to stop the server
echo.

php artisan serve

pause
