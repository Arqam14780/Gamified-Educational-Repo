param(
    [Parameter(Mandatory=$true)][string]$PdfPath,
    [Parameter(Mandatory=$true)][string]$OutputDirectory,
    [Parameter(Mandatory=$true)][ValidatePattern('^[a-zA-Z0-9_-]+$')][string]$Prefix
)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Runtime.WindowsRuntime
[Windows.Storage.StorageFile,Windows.Storage,ContentType=WindowsRuntime] | Out-Null
[Windows.Data.Pdf.PdfDocument,Windows.Data.Pdf,ContentType=WindowsRuntime] | Out-Null
[Windows.Data.Pdf.PdfPageRenderOptions,Windows.Data.Pdf,ContentType=WindowsRuntime] | Out-Null
[Windows.Storage.Streams.InMemoryRandomAccessStream,Windows.Storage.Streams,ContentType=WindowsRuntime] | Out-Null
[Windows.Storage.Streams.DataReader,Windows.Storage.Streams,ContentType=WindowsRuntime] | Out-Null
$operationAwait = [System.WindowsRuntimeSystemExtensions].GetMethods() | Where-Object {
    $_.Name -eq 'AsTask' -and $_.IsGenericMethod -and $_.GetParameters().Count -eq 1 -and
    $_.GetParameters()[0].ParameterType.Name -eq 'IAsyncOperation`1'
} | Select-Object -First 1
$actionAwait = [System.WindowsRuntimeSystemExtensions].GetMethods() | Where-Object {
    $_.Name -eq 'AsTask' -and -not $_.IsGenericMethod -and $_.GetParameters().Count -eq 1 -and
    $_.GetParameters()[0].ParameterType.Name -eq 'IAsyncAction'
} | Select-Object -First 1
function Await-Operation($operation, [Type]$resultType) {
    $task = $operationAwait.MakeGenericMethod($resultType).Invoke($null,@($operation))
    $task.Wait()
    return $task.Result
}
$source = (Resolve-Path -LiteralPath $PdfPath).Path
$file = Await-Operation ([Windows.Storage.StorageFile]::GetFileFromPathAsync($source)) ([Windows.Storage.StorageFile])
$document = Await-Operation ([Windows.Data.Pdf.PdfDocument]::LoadFromFileAsync($file)) ([Windows.Data.Pdf.PdfDocument])
New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
for ($index = 0; $index -lt $document.PageCount; $index++) {
    $page = $document.GetPage($index)
    $stream = New-Object Windows.Storage.Streams.InMemoryRandomAccessStream
    $options = New-Object Windows.Data.Pdf.PdfPageRenderOptions
    $options.DestinationWidth = 1400
    $render = $actionAwait.Invoke($null,@($page.RenderToStreamAsync($stream,$options)))
    $render.Wait()
    $inputStream = $stream.GetInputStreamAt(0)
    $reader = New-Object Windows.Storage.Streams.DataReader($inputStream)
    [void](Await-Operation ($reader.LoadAsync([uint32]$stream.Size)) ([uint32]))
    $bytes = New-Object byte[] ([int]$stream.Size)
    $reader.ReadBytes($bytes)
    $target = Join-Path $OutputDirectory ($Prefix + '-page-' + ($index+1) + '.png')
    [IO.File]::WriteAllBytes([IO.Path]::GetFullPath($target),$bytes)
    Write-Output $target
    $reader.Dispose(); $inputStream.Dispose(); $stream.Dispose(); $page.Dispose()
}
