@echo off
echo Cleaning up frontend files...

:: Remove large files
del /f /q image.rar
del /f /q apple.zip

:: Create optimized images directory
mkdir optimized_images

:: Combine CSS files
type login.css signup.css > auth.css
del /f /q login.css
del /f /q signup.css

:: Create logs directory
mkdir ..\logs

echo Cleanup complete!
pause 