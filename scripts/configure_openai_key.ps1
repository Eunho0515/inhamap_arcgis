[CmdletBinding()]
param()

$ErrorActionPreference = "Stop"
$secureKey = Read-Host "OpenAI API key (input is hidden)" -AsSecureString
$keyPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureKey)

try {
    $plainKey = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($keyPointer)
    $plainKey = $plainKey.Trim()
    if ($plainKey -match '^(?i)Authorization\s*:\s*Bearer\s+(.+)$') {
        $plainKey = $Matches[1].Trim()
    }
    elseif ($plainKey -match '^(?i)Bearer\s+(.+)$') {
        $plainKey = $Matches[1].Trim()
    }
    if ([string]::IsNullOrWhiteSpace($plainKey) -or -not $plainKey.StartsWith("sk-")) {
        throw "The value does not look like an OpenAI API key (expected an sk- prefix)."
    }

    [Environment]::SetEnvironmentVariable("OPENAI_API_KEY", $plainKey, "User")
    $savedKey = [Environment]::GetEnvironmentVariable("OPENAI_API_KEY", "User")
    if ([string]::IsNullOrWhiteSpace($savedKey)) {
        throw "Windows did not persist OPENAI_API_KEY."
    }
    Write-Host "OPENAI_API_KEY was saved to the Windows user environment." -ForegroundColor Green
    Write-Host "Fully close Unity AND Unity Hub, then start them again." -ForegroundColor Yellow
}
finally {
    $plainKey = $null
    $savedKey = $null
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($keyPointer)
}

Read-Host "Press Enter to close"
