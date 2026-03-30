@echo off
echo Starting Laravel Backend Server...
echo.

cd /d "%~dp0karton-backend"

where php >nul 2>&1
if errorlevel 1 (
    echo PHP is not installed or is not available in PATH.
    echo Run this in PowerShell as administrator:
    echo Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ^(^(New-Object System.Net.WebClient^).DownloadString('https://php.new/install/windows/8.4'^)^)
    pause
    exit /b 1
)

echo Checking if .env file exists...
if not exist .env (
    echo .env file not found! Copying from .env.example...
    copy .env.example .env
    if errorlevel 1 (
        echo Failed to create .env file. Aborting startup.
        pause
        exit /b 1
    )
    echo .env file created.
)

if not exist vendor (
    echo Vendor directory not found! Installing Composer dependencies...
    composer install
    if errorlevel 1 (
        echo Composer install failed. Aborting startup.
        pause
        exit /b 1
    )
)

findstr /R /C:"^APP_KEY=.+" .env >nul
if errorlevel 1 (
    echo APP_KEY is missing. Generating application key...
    php artisan key:generate --force
    if errorlevel 1 (
        echo Failed to generate APP_KEY. Aborting startup.
        pause
        exit /b 1
    )
)

findstr /R /C:"^DB_CONNECTION=sqlite$" .env >nul
if not errorlevel 1 (
    if not exist database\database.sqlite (
        echo SQLite database file not found. Creating database\database.sqlite...
        type nul > database\database.sqlite
        if errorlevel 1 (
            echo Failed to create SQLite database file. Aborting startup.
            pause
            exit /b 1
        )
    )
)

echo Running database migrations and seeders...
php artisan migrate --seed --force
if errorlevel 1 (
    echo Database migration/seeding failed. Aborting startup.
    pause
    exit /b 1
)

echo Starting PHP development server...
echo Server will be available at http://localhost:8000
echo Press Ctrl+C to stop the server
echo.

php artisan serve

pause
