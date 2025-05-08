Remove-Item .\QuickLook.Plugin.FolderReadme.qlplugin -ErrorAction SilentlyContinue

$files = Get-ChildItem -Path .\bin\Release\ -Exclude *.pdb,*.xml
Compress-Archive $files .\QuickLook.Plugin.FolderReadme.zip
Move-Item .\QuickLook.Plugin.FolderReadme.zip .\QuickLook.Plugin.FolderReadme.qlplugin