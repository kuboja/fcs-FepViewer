@echo off
@chcp 65001>nul

rem Složka na sdíleném disku
set qpath=Q:\Builds\FepViewer\
set baseFileName=FepViewer_

rem build složka
set distFolder=..\Dist
set buildFolder=%distFolder%\build\

rem Složení jména z datumu ve formátu dxf2fcs_YYMMDD
for /f "skip=1" %%d in ('wmic os get localdatetime') do if not defined mydate set mydate=%%d
set name=%baseFileName%%mydate:~2,2%%mydate:~4,2%%mydate:~6,2%
rem echo %name%


rem Nalezení volné pøípony, pokud probìhlo v jeden více releasù
FOR %%G IN (a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z) DO (
    if exist %qpath%%name%%%G.zip (
        rem file exists
    ) else (
        rem file doesn't exist
        set name=%name%%%G
        goto forexit
    )
)

:forexit
rem echo %name%

rem Delete old build folder
if exist %buildFolder% rmdir /s /q %buildFolder%

echo Build FepViewer
dotnet publish ..\FepViewer\FepViewer.csproj  -c Release  --no-self-contained  -o %buildFolder%  -r win10-x64


echo Make FepViewer zip
powershell Compress-Archive %buildFolder%. %distFolder%\%name%.zip -Force

if "%~1"=="deploy" (
    echo Copy FepViewer zip to public path: %qpath%%name%.zip
    robocopy %distFolder% %qpath% %name%.zip /njh /njs /is /ndl
)

echo All DONE.