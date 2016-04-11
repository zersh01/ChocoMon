$packageName = 'chocomon'
 $fileType = 'exe'
 $silentArgs = '/S'

$osBitness = Get-ProcessorBits
if ($osBitness -eq 64) 
 {
 $uninstallString = (Get-ItemProperty 'HKLM:\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\ChocoMon').UninstallString
} else {
 $uninstallString = (Get-ItemProperty 'HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\ChocoMon').UninstallString
}


if ($uninstallString -ne "") {
     Uninstall-ChocolateyPackage $packageName $fileType $silentArgs $uninstallString
}





