# SPDX-License-Identifier: MPL-2.0
param(
    [Parameter(Mandatory=$true)][string]$GameRoot,
    [string]$FrameworkReferences = 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.1',
    [switch]$AllowDirty
)
$ErrorActionPreference = 'Stop'
$repo = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
Push-Location $repo
try {
    $source = (git rev-parse HEAD).Trim()
    if (!$AllowDirty -and (git status --porcelain)) { throw 'Commit source before creating a handoff build.' }
    # Resolve the selected installed SDK instead of downloading a toolchain.
    $sdkLine = dotnet --list-sdks | Select-Object -Last 1
    $sdkVersion = ($sdkLine -split ' ')[0]
    $sdkRoot = $sdkLine -replace '^.*\[([^]]+)\].*$', '$1'
    $compiler = Join-Path $sdkRoot "$sdkVersion\Roslyn\bincore\csc.dll"
    $out = Join-Path $repo $(if ($AllowDirty) { 'work\probe-compile' } else { 'artifacts\runtime-probe' })
    New-Item -ItemType Directory -Force -Path $out | Out-Null
    $generated = Join-Path $out 'BuildIdentity.cs'
    $identity = "// Generated, not tracked.`nnamespace ExtraSermons.Research { internal static class BuildIdentity { internal const string Source = `"$source`"; } }"
    [IO.File]::WriteAllText($generated, $identity)
    $base = @('/nologo','/noconfig','/nostdlib+','/langversion:7.3','/optimize+','/deterministic+',"/pathmap:$repo=/src")
    $refs = @('mscorlib.dll','System.dll','System.Core.dll') | ForEach-Object { '/reference:' + (Join-Path $FrameworkReferences $_) }
    $probe = Join-Path $repo 'research\RuntimeProbe'
    $testExe = Join-Path $out 'ProbeStateTests.exe'
    & dotnet $compiler @base @refs /target:exe "/out:$testExe" (Join-Path $probe 'ProbeState.cs') (Join-Path $probe 'ProbeStateTests.cs')
    if ($LASTEXITCODE -ne 0) { throw 'Policy test compilation failed' }
    & $testExe
    if ($LASTEXITCODE -ne 0) { throw 'Policy assertions failed' }
    $managed = Join-Path $GameRoot 'Graveyard Keeper_Data\Managed'
    $hostRefs = @('Assembly-CSharp.dll','Assembly-CSharp-firstpass.dll','UnityEngine.dll','UnityEngine.CoreModule.dll','UnityEngine.IMGUIModule.dll','UnityEngine.InputLegacyModule.dll') | ForEach-Object { '/reference:' + (Join-Path $managed $_) }
    $loaderRefs = @('BepInEx.dll','0Harmony.dll') | ForEach-Object { '/reference:' + (Join-Path $GameRoot "BepInEx\core\$_") }
    $dll = Join-Path $out 'ExtraSermons.dll'
    & dotnet $compiler @base @refs @hostRefs @loaderRefs /target:library "/out:$dll" (Join-Path $probe 'Identity.cs') (Join-Path $probe 'ProbeState.cs') (Join-Path $probe 'RuntimeProbe.cs') $generated
    if ($LASTEXITCODE -ne 0) { throw 'Probe compilation failed' }
    $contractExe = Join-Path $out 'HostContractTests.exe'
    & dotnet $compiler @base @refs @loaderRefs /target:exe "/out:$contractExe" (Join-Path $probe 'HostContractTests.cs')
    if ($LASTEXITCODE -ne 0) { throw 'Native contract test compilation failed' }
    & $contractExe $GameRoot $out
    if ($LASTEXITCODE -ne 0) { throw 'Native metadata/IL contracts failed' }
    # AssemblyFileVersion normalizes to four components; Identity.Version owns the three-component plugin version.
    $versionText = [IO.File]::ReadAllText((Join-Path $probe 'Identity.cs'))
    $version = [regex]::Match($versionText, 'Version = "([0-9.]+)"').Groups[1].Value
    $numbered = Join-Path $out "ExtraSermons-$version.dll"
    if ((Test-Path -LiteralPath $numbered) -and (Get-FileHash -LiteralPath $numbered).Hash -ne (Get-FileHash -LiteralPath $dll).Hash) {
        if (!$AllowDirty) { throw 'Immutable numbered artifact already exists with different bytes; use a new version.' }
    }
    Copy-Item -LiteralPath $dll -Destination $numbered
    $referenceIdentity = @('Assembly-CSharp.dll','Assembly-CSharp-firstpass.dll','UnityEngine.dll','UnityEngine.CoreModule.dll','UnityEngine.IMGUIModule.dll','UnityEngine.InputLegacyModule.dll') | ForEach-Object { $file = Join-Path $managed $_; [ordered]@{ file=$_; sha256=(Get-FileHash -LiteralPath $file).Hash.ToLowerInvariant() } }
    $referenceIdentity += @('BepInEx.dll','0Harmony.dll') | ForEach-Object { $file = Join-Path $GameRoot "BepInEx\core\$_"; [ordered]@{ file=$_; sha256=(Get-FileHash -LiteralPath $file).Hash.ToLowerInvariant() } }
    $manifest = [ordered]@{ version=$version; source=$source; dirty=[bool]$AllowDirty; sdk=$sdkVersion; framework='net471'; sha256=(Get-FileHash -LiteralPath $dll -Algorithm SHA256).Hash.ToLowerInvariant(); references=$referenceIdentity; file=[IO.Path]::GetFileName($numbered) }
    $manifest | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $out 'identity.json') -Encoding utf8
    $manifest | ConvertTo-Json
} finally { Pop-Location }
