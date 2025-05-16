# Start the server as a background process (detached)
$process = Start-Process -FilePath "dotnet" -ArgumentList 'run --project ./FoodDeliveryBackend.csproj --urls http://localhost:5000' -PassThru
Set-Content -Path "server-process.pid" -Value $process.Id

# Wait for server to be up
$maxRetries = 10
$delay = 2
for ($i = 0; $i -lt $maxRetries; $i++) {
  try {
    $response = Invoke-WebRequest -Uri http://localhost:5000/api/companies -UseBasicParsing -TimeoutSec 2
    Write-Host "Server is ready!"
    break
  } catch {
    Write-Host "Waiting for server to start... ($i/$maxRetries)"
    Start-Sleep -Seconds $delay
  }
}

if ($i -eq $maxRetries) {
  Write-Error "Server did not start in time"
  Stop-Process -Id $process.Id
  exit 1
}