$alphabet=$NULL;For ($a=65;$a –le 90;$a++) {$alphabet+=,[char][byte]$a }

$connectionString = "Data Source=h98ohmld2f.database.windows.net;Initial Catalog=Stockimulate;Integrated Security=False;User ID=JMSXTech;Password=jmsx!2014;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;"

$teamID = ""

$connection = New-Object -TypeName System.Data.SqlClient.SqlConnection
$connection.ConnectionString = $connectionString
$command = $connection.CreateCommand()

$query = "DELETE FROM Trades; DELETE FROM Accounts; DELETE FROM Traders; DELETE FROM Teams;"

$command.CommandText = $query
$connection.Open()
$command.ExecuteNonQuery()

$connection.Close()

Function GET-Temppassword() {

    Param(

    [int]$length,

    [string[]]$sourcedata

    )

    For ($loop=1; $loop –le $length; $loop++) {

                $TempPassword+=($sourcedata | GET-RANDOM)

    }

    return $TempPassword

}

$entities = Import-Csv "teamsplayers.csv"

$currentTeam = ""

$teamID = 0;
$traderID = 0;

foreach ($entity in $entities) {

    if ($entity.Team -ne $currentTeam) {

		$currentTeam = $entity.Team

        $teamID++

		$connection = New-Object -TypeName System.Data.SqlClient.SqlConnection
		$connection.ConnectionString = $connectionString
		$command = $connection.CreateCommand()

        $query = "INSERT INTO Teams (Id, Name, Code) VALUES (@Id, @Name, @Code);"
        
        $command.CommandText = $query

        $command.Parameters.AddWithValue("@Id", $teamID)
        $command.Parameters.AddWithValue("@Name", $currentTeam)
        $command.Parameters.AddWithValue("@Code", (GET-Temppassword 4 $alphabet))

		$connection.Open()
		$command.ExecuteNonQuery()

		$connection.close()

    }

	$traderID++

	$connection = New-Object -TypeName System.Data.SqlClient.SqlConnection
    $connection.ConnectionString = $connectionString
    $command = $connection.CreateCommand()

    $query = "INSERT INTO Traders (Id, Name, TeamId, Funds) VALUES (@Id, @Name, @TeamId, 1000000);"
    $command.CommandText = $query

    $command.Parameters.AddWithValue("@Id", $traderID)
    $command.Parameters.AddWithValue("@Name", $entity.Name)
    $command.Parameters.AddWithValue("@TeamId", $teamID)

	$connection.Open()
	$command.ExecuteNonQuery()

	$connection.close()

}