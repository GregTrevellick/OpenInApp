echo off

CD D:\Program Files\Microsoft Visual Studio\2022\Community\VSSDK\VisualStudioIntegration\Tools\Bin

VsixPublisher.exe publish -payload "D:\_git\OpenInApp\Src\VS2022\OpenInApp\bin\Debug\OpenInVS2022.vsix" -publishManifest "D:\_git\OpenInApp\Src\VS2022\OpenInApp\VisualStudioMarketPlaceAssets\VsmpPublish.json" -personalAccessToken "vsmp_pat"

REM https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-publishing-a-visual-studio-extension-via-command-line?view=vs-2022