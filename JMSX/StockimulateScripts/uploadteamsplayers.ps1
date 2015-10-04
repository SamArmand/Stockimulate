$alphabet=$NULL;For ($a=65;$a –le 90;$a++) {$alphabet+=,[char][byte]$a }

$connectionString = "Data Source=h98ohmld2f.database.windows.net;Initial Catalog=JMSX;Integrated Security=False;User ID=JMSXTech;Password=jmsx!2014;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;"

$teamID = "";

$entities = Import-Csv "teamsplayers.csv"

$connection = New-Object -TypeName System.Data.SqlClient.SqlConnection
$connection.ConnectionString = $connectionString
$command = $connection.CreateCommand()

$query = "DELETE FROM Players"

$command.CommandText = $query
$connection.Open()
$command.ExecuteNonQuery()

$connection.Close()

$connection = New-Object -TypeName System.Data.SqlClient.SqlConnection
$connection.ConnectionString = $connectionString
$command = $connection.CreateCommand()

$query = "DELETE FROM Teams"

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

foreach ($entity in $entities) {

    $query = "";

    $connection = New-Object -TypeName System.Data.SqlClient.SqlConnection
    $connection.ConnectionString = $connectionString
    $command = $connection.CreateCommand()

    if ($entity.Team -eq "1") {

        $teamID = $entity.ID

        $query = "INSERT INTO Teams (ID, Name, Code) VALUES (@ID, @Name, @Code);"
        
        $command.CommandText = $query

        $command.Parameters.AddWithValue("@ID", $entity.ID)
        $command.Parameters.AddWithValue("@Name", $entity.Name)
        $command.Parameters.AddWithValue("@Code", (GET-Temppassword 8 $alphabet))

    }

    else {

        $query = "INSERT INTO Players (ID, Name, TeamID, PositionIndex1, PositionIndex2, PositionIndex3, Funds) VALUES (@ID, @Name, @TeamID, '0', '0', '0', '1000000')"
        $command.CommandText = $query

        $command.Parameters.AddWithValue("@ID", $entity.ID)
        $command.Parameters.AddWithValue("@Name", $entity.Name)
        $command.Parameters.AddWithValue("@TeamID", $teamID)

    }

    $connection.Open()
    $command.ExecuteNonQuery()

    $connection.close()

}