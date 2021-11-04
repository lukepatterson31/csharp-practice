$StartTime = $(get-date) ;
Invoke-WebRequest -Uri "http://localhost:5000/weatherforecast" -UseBasicParsing ;
Write-Output ("{0}" -f ($(get-date)-$StartTime))