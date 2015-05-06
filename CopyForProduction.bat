rd /s /q "./Package"
xcopy "./Sundial/bin/Release/*.dll"								"./Package"		/C /D /E /I /F /R /Y /Z
xcopy "./Sundial.DefaultFormatter/bin/Release/*.dll"		"./Package"		/C /D /E /I /F /R /Y /Z
xcopy "./Sundial/bin/Release/*.exe"								"./Package"		/C /D /E /I /F /R /Y /Z
xcopy "./Sundial/bin/Release/*.config"							"./Package"		/C /D /E /I /F /R /Y /Z
cd Package
7z a -tzip sundial.zip *.*
del "*.dll"
del "*.exe"
del "*.config"