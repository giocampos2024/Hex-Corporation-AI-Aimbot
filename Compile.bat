dotnet publish -r win-x64 -c Release --self-contained

REM Define the source and destination paths
set source1="C:\Users\ELOAH\Desktop\sufx gay\source aimbot\AotForms\bin\Release\net7.0-windows\win-x64\publish\cimgui.dll"
set source2="C:\Users\ELOAH\Desktop\sufx gay\source aimbot\AotForms\bin\Release\net7.0-windows\win-x64\publish\Client.dll"
set destination="C:\Windows\Temp"

REM Copy the files, overwrite if they exist
copy /Y %source1% %destination%
copy /Y %source2% %destination%


PAUSE