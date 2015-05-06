#NOTE: Please remove any commented lines to tidy up prior to releasing the package, including this one

# stop on all errors
$ErrorActionPreference = 'Stop';


$packageName = 'sundial' # arbitrary name for the package, used in messages
$registryUninstallerKeyName = 'sundial' #ensure this is the value in the registry
$url = 'https://github.com/JaCraig/Sundial/releases/download/v0.1.0/sundial.zip' # download url
$url64 = 'https://github.com/JaCraig/Sundial/releases/download/v0.1.0/sundial.zip' # 64bit URL here or remove - if installer decides, then use $url
$silentArgs = '' # "/s /S /q /Q /quiet /silent /SILENT /VERYSILENT" # try any of these to get the silent installer #msi is always /quiet
$validExitCodes = @(0) #please insert other valid exit codes here, exit codes for ms http://msdn.microsoft.com/en-us/library/aa368542(VS.85).aspx
$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

# main helper functions - these have error handling tucked into them already
# see https://github.com/chocolatey/choco/wiki/HelpersReference
# Install an application, will assert administrative rights
# add additional optional arguments as necessary
Install-ChocolateyZipPackage "$packageName" "$url" "$toolsDir"