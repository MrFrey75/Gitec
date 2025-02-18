$clientId = "811105c-5db5-4b19-9db8-7e01ac914ac9"
$tenantId = "7eaaa2e0-ab22-47ef-a037-5e216f661c16"
$clientSecret = "9Gt8Q~vDVttzhKKvbAPqSIgzAYDsaq4bF8cG-a_a"

# Convert secure string to plain text for storage (DO NOT use in production without encryption)
$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($clientSecret)
$clientSecretPlain = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)

# Set Environment Variables (Permanent)
[System.Environment]::SetEnvironmentVariable("GRAPH_CLIENT_ID", $clientId, [System.EnvironmentVariableTarget]::Machine)
[System.Environment]::SetEnvironmentVariable("GRAPH_TENANT_ID", $tenantId, [System.EnvironmentVariableTarget]::Machine)
[System.Environment]::SetEnvironmentVariable("GRAPH_CLIENT_SECRET", $clientSecretPlain, [System.EnvironmentVariableTarget]::Machine)

Write-Host "Environment variables set successfully."
Write-Host "Restart your terminal or computer to apply changes."