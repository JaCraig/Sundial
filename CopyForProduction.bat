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
cd ..

echo "Commit, add git release tag, and push to github. Then add sundial.zip to the release... Not sure on that part..."

cd chocolatey
echo "Modify version info of nuspec file"
cpack
FOR %%A IN (*.nupkg) DO choco push %%A
del "*.nupkg"