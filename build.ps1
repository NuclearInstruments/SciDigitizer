$filePath = "DT5550ControlCenter\My Project\AssemblyInfo.vb"
$newVersion = $args[0]
$pattern1 = '^\s*<Assembly: AssemblyVersion\("[0-9\.]+"\)>\s*$'
$pattern2 = '^\s*<Assembly: AssemblyFileVersion\("[0-9\.]+"\)>\s*$'

if ($newVersion) {
	$contents = Get-Content $filePath
	$match = Select-String -Path $filePath -Pattern $pattern1
	if ($match) {	
		$index = $contents.IndexOf($match.Line)
		$contents[$index] = ('<Assembly: AssemblyVersion("' + $newVersion + '")>')
		
	}

	$match = Select-String -Path $filePath -Pattern $pattern2	
	if ($match) {
		$index = $contents.IndexOf($match.Line)
		$contents[$index] = ('<Assembly: AssemblyFileVersion("' + $newVersion + '")>')
	}
	$contents | Set-Content $filePath
	
}
nuget restore "OpenHardwareReadoutSoftware.sln"
MSBuild OpenHardwareReadoutSoftware.sln -t:Rebuild -p:Configuration=Debug -p:Platform="Any CPU" 
copy-item  ".\COPY_TO_BIN\*" ".\BIN\" -force -recurse 
