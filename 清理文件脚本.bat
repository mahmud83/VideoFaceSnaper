@echo off 

SET Pash=%cd%

rem 删除当前目录下所有obj，bin目录
for /f "tokens=*" %%a in ('dir obj /b /ad /s ^|sort') do rd "%%a" /s/q
for /f "tokens=*" %%a in ('dir bin /b /ad /s ^|sort') do rd "%%a" /s/q

del /s /ah /f *.suo
del /s /f *.user
del /s /f *.cache
del /s /f *.scc
del /s /f *.vssscc
del /s /f *.vspscc
del /s /f *.keep
del /s /ah /f vssver2.scc
del /s /ah StyleCop.Cache
del /s /ah .hgignore

del %Pash%\output\*.* /s/q

del %Pash%\Build\Release\*.* /s/q
del %Pash%\Build\Debug\*.* /s/q

echo 清理完成！

pause