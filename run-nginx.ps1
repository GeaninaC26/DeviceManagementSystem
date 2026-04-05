# Start nginx in a Docker container with custom config and port 8081 open
# Usage: .\run-nginx.ps1


# Get absolute path to nginx.conf and convert to Docker-friendly format
$nginxConf = Join-Path $PWD 'nginx.conf'
$nginxConfDocker = $nginxConf -replace '\\','/'



# Pull nginx image if not present
Write-Host "Pulling nginx:latest image..."
docker pull nginx:latest
if ($LASTEXITCODE -ne 0) {
	Write-Error "Failed to pull nginx:latest image."
	exit 1
}


# Stop and remove any existing container named dms-nginx
Write-Host "Removing any existing dms-nginx container..."
docker rm -f dms-nginx 2>$null


# Run nginx container with port 8081:80 and mount config
$volumeArg = "$($nginxConfDocker):/etc/nginx/nginx.conf:ro"
Write-Host "Starting nginx container on port 8081 with config from $nginxConfDocker..."
docker run -d --name dms-nginx -p 8081:80 -v $volumeArg nginx:latest
if ($LASTEXITCODE -ne 0) {
	Write-Error "Failed to start nginx container. Check the config path and Docker status."
	exit 1
}

Write-Host "nginx started on http://localhost:8081 with config from $nginxConfDocker"
