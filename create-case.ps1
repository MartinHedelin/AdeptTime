$headers = @{
    "apikey" = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZS1kZW1vIiwicm9sZSI6ImFub24iLCJleHAiOjE5ODM4MTI5OTZ9.CRXP1A7WOeoJeXxjNni43kdQwgnWNReilDMblYTn_I0"
    "Content-Type" = "application/json"
}

$body = @{
    case_number = "TEST-2025-POWERSHELL"
    title = "PowerShell Test Case"
    description = "Testing case creation via PowerShell"
    status = "Ny"
    priority = "Medium"
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "http://127.0.0.1:54321/rest/v1/cases" -Method POST -Headers $headers -Body $body
    Write-Host "SUCCESS: Case created!" -ForegroundColor Green
    Write-Host ($response | ConvertTo-Json)
} catch {
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Response: $($_.ErrorDetails.Message)" -ForegroundColor Red
}
