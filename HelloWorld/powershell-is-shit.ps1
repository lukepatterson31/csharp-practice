$StartTime = $(get-date)
$RequestId = 0
foreach ($i in 1..100)
{
    $RequestId += 1
    Invoke-WebRequest -Uri "http://localhost:5000/weatherforecast" -UseBasicParsing ;
    Write-Output ("{0}" -f ($(get-date)-$StartTime))
}
