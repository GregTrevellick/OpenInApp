echo off

CD C:\Program Files\Microsoft Visual Studio\2022\Preview\VSSDK\VisualStudioIntegration\Tools\Bin

VsixPublisher.exe publish -payload "D:\_git\OpenInApp\Src\OpenInApp\bin\Debug\OpenInApp.vsix" -publishManifest "D:\_git\OpenInApp\Src\VsmpPublish.json" -personalAccessToken "vsmp_pat"

REM https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-publishing-a-visual-studio-extension-via-command-line?view=vs-2019