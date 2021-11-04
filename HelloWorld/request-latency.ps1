foreach ($i in 1..100)
{
    Invoke-WebRequest http://localhost:5000/weatherforecast 
}
