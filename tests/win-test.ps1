$process = Start-Process -filePath dotnet -ArgumentList 'run --project ./FoodDeliveryBackend.csproj --urls http://localhost:5000' -PassThru

# Wait until the app is ready
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
  $process | kill
  exit 1
}

# Actual test
Write-Host "Status Code: $($response.StatusCode)"

if ($response.StatusCode -ne 200) {
  Write-Error "Expected HTTP 200 but got $($response.StatusCode)"
  $process | kill
  exit 1
}

# Clean up
$process | kill