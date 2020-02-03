Get-EventSubscriber -Force | Unregister-Event -Force
$Folder = $PWD.Path + "\theme"
$Filter = '*.*'
$Watcher = New-Object IO.FileSystemWatcher $Folder, $Filter -Property @{IncludeSubdirectories = $true; NotifyFilter = [IO.NotifyFilters]'FileName, Size'} 
$Watcher.EnableRaisingEvents = $true
$EventListener = Register-ObjectEvent -InputObject $Watcher Changed -Action { 
    function Reset ($fileName) {
        Write-Host "'$fileName' was changed. Rebuilding..."
        Stop-Process -Name "docfx" -Force
        Remove-Item "_site" -Recurse -Force
        Start-Process -FilePath "docfx" -ArgumentList "docfx.json", "--serve" -NoNewWindow
    }
    $name = $Event.SourceEventArgs.Name 
    $changeType = $Event.SourceEventArgs.ChangeType 
    $timeStamp = $Event.TimeGenerated 
    Write-Host "'$name' Changed..."
    Reset($name)
}

