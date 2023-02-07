
New-Item -Path "." -Name "./INNOSETUP_BUILD/build" -ItemType "directory" -Force
Invoke-WebRequest http://installers.lanni/windows_sw/vc_redist/VC_redist.x86.exe -OutFile ./INNOSETUP_BUILD/build/VC_redist.x86.exe	 
Invoke-WebRequest http://installers.lanni/windows_sw/vc_redist/VC_redist_2012.x86.exe -OutFile ./INNOSETUP_BUILD/build/VC_redist_2012.x86.exe	 
Invoke-WebRequest http://installers.lanni/windows_sw/dotnet/ndp48-x86-x64-allos-enu.exe	-OutFile ./INNOSETUP_BUILD/build/ndp48-x86-x64-allos-enu.exe 


Invoke-WebRequest http://installers.lanni/windows_sw/ftdi_driver/d2xx_setup.exe -OutFile ./INNOSETUP_BUILD/build/d2xx_setup.exe
Invoke-WebRequest http://installers.lanni/windows_sw/ftdi_driver/d3xx_setup.exe -OutFile ./INNOSETUP_BUILD/build/d3xx_setup.exe



$param1=$args[0]

iscc /Qp /DMyAppVersion=$param1 scidigitizer.iss
