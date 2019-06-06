@echo off
set OPTIONS=--configuration Release --output ../Build --include-symbols
set COMMAND=dotnet pack

cd ../sdmap
%COMMAND% %OPTIONS%

cd ../sdmap.ext
%COMMAND% %OPTIONS%

cd ../sdmap.ext.Dapper
%COMMAND% %OPTIONS%

cd ../Build