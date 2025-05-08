![QuickLook icon](https://user-images.githubusercontent.com/1687847/29485863-8cd61b7c-84e2-11e7-97d5-eacc2ba10d28.png)

# QuickLook.Plugin.FolderReadme

为包含readme的文件夹覆盖预览为readme

Overwrite the preview of the folder containing readme to readme

还没有上传github

## Try out

1. Go to [Release page](https://github.com/QL-Win/QuickLook.Plugin.FolderReadme/releases) and download the latest version.
2. Make sure that you have QuickLook running in the background. Go to your Download folder, and press <key>Spacebar</key> on the downloaded `.qlplugin` file.
3. Click the “Install” button in the popup window.
4. Restart QuickLook.
6. Select the folder and press <key>Spacebar</key>.

## Development

 1. Clone this project. Do not forget to update submodules.
 2. Build project with `Release` profile.
 3. Run `Scripts\pack-zip.ps1`.
 4. You should find a file named `QuickLook.Plugin.FolderReadme.qlplugin` in the project directory.

## License

MIT License.

## make a package

### powershell :

dotnet build -c Release ; .\pack-zip.ps1