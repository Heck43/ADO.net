param(
    [string]$ProjectName = "task",
    [string]$ProjectType = "console-npgsql"
)

$pt = $ProjectType.Replace("-npgsql", "")
dotnet new $pt -n $ProjectName -o $ProjectName -f net8.0

if ($ProjectType -like "*npgsql*") {
    dotnet add "$ProjectName/$ProjectName.csproj" package Npgsql
    # Base64 template of Program.cs in UTF-8
    $b64 = "dXNpbmcgTnBnc3FsOw0KDQpzdHJpbmcgY29ubnN0cmluZyA9ICJIb3N0PWxvY2FsaG9zdDtQb3J0PTU0MzI7VXNlcm5hbWU9cG9zdGdyZXM7UGFzc3dvcmQ9ZnVyNDM7RGF0YWJhc2U9eHoxIjsNCg0KDQp1c2luZyB2YXIgY29ubiA9IG5ldyBOcGdzcWxDb25uZWN0aW9uKGNvbm5zdHJpbmcpOw0KdHJ5DQp7DQogICAgY29ubi5PcGVuKCk7DQogICAgQ29uc29sZS5Xcml0ZUxpbmUoItCf0L7QtNC60LvRjtGH0LXQvdC+ISIpOw0KICAgIHN0cmluZyBzcWwgPSBAIg0KICAgIA0KICAgIA0KICAgICI7DQogICAgDQp9DQpjYXRjaCAoTnBnc3FsRXhjZXB0aW9uIGV4KQ0Kew0KICAgIENvbnNvbGUuV3JpdGVMaW5lKCLQntGI0LjQsdC60LA6IHsiICsgZXguTWVzc2FnZSArICJ9Iik7DQp9DQo="
    $bytes = [System.Convert]::FromBase64String($b64)
    [System.IO.File]::WriteAllBytes("$ProjectName/Program.cs", $bytes)
}

Write-Host "=== OK! $ProjectName created with Npgsql template ===" -ForegroundColor Green