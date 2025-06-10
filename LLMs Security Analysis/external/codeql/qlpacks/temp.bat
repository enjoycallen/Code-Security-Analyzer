@echo off
setlocal enabledelayedexpansion

rem 设置要查找的关键字
set "KEYWORD=@kind tree"

rem 设置根目录
set "ROOT=1.3.9"

rem 遍历所有文件（可加 *.cpp 或 *.txt 过滤）
for /r "%ROOT%" %%f in (*) do (
    findstr /c:"%KEYWORD%" "%%f" >nul 2>nul
    if !errorlevel! == 0 (
        echo Deleting file: %%f
        del /f /q "%%f"
    )
)