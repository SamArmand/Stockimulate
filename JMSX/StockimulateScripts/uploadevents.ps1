$connectionString = "Data Source=h98ohmld2f.database.windows.net;Initial Catalog=JMSX;Integrated Security=False;User ID=JMSXTech;Password=jmsx!2014;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;"

$events = Import-Csv "events.csv"

$connection = New-Object -TypeName System.Data.SqlClient.SqlConnection
$connection.ConnectionString = $connectionString
$command = $connection.CreateCommand()

$query = "DELETE FROM Events"

$command.CommandText = $query
$connection.Open()
$command.ExecuteNonQuery()

$connection.close()

foreach ($event in $events) {
    
    if ($event.News -eq "") {
        $news = "null"
    }
    else {
        $news = $event.News
    }

    $connection = New-Object -TypeName System.Data.SqlClient.SqlConnection
    $connection.ConnectionString = $connectionString
    $command = $connection.CreateCommand()

    $query = "INSERT INTO Events (TradingDay, News, EffectIndex1, EffectIndex2) VALUES (@TradingDay, @News, @EffectIndex1, @EffectIndex2);"

    $command.CommandText = $query

    $command.Parameters.AddWithValue("@TradingDay", $event.Day)
    $command.Parameters.AddWithValue("@News", $news)
    $command.Parameters.AddWithValue("@EffectIndex1", $event.IND1)
    $command.Parameters.AddWithValue("@EffectIndex2", $event.IND2)
	$command.Parameters.AddWithValue("@EffectIndex3", $event.IND3)

    $connection.Open()
    $command.ExecuteNonQuery()

    $connection.close()

}