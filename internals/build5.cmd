
set version=%1
set key=%2

dotnet nuget locals all --clear

cd %~dp0/../..
magic.library/build.cmd %version% %key%
