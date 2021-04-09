cd UpdateUtil
dotnet build
cd bin
dotnet UpdateUtil.dll %1 %2 %3
cd ../..