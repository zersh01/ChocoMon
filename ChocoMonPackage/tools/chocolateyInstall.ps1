$packageName = 'chocomon'
$installerType = 'EXE' 
$url = 'https://raw.githubusercontent.com/zersh01/ChocoMon/master/InstallChocoMon/ChocoMon_1.0.1.1_Setup.exe'
$silentArgs = '/S'
$validExitCodes = @(0) 
Install-ChocolateyPackage "$packageName" "$installerType" "$silentArgs" "$url"   -validExitCodes $validExitCodes




