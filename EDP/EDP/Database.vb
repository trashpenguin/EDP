Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography

Friend Module Database
    Friend Function OpenConnection() As MySqlConnection
        Dim value = Environment.GetEnvironmentVariable("EDP_DB_CONNECTION")
        If String.IsNullOrWhiteSpace(value) Then
            Throw New InvalidOperationException("Set EDP_DB_CONNECTION before starting the application.")
        End If
        Dim connection As New MySqlConnection(value)
        Try
            connection.Open()
            Return connection
        Catch
            connection.Dispose()
            Throw
        End Try
    End Function

    Friend Function VerifyPassword(password As String, encoded As String) As Boolean
        If String.IsNullOrEmpty(encoded) Then Return False
        Dim parts = encoded.Split("$"c)
        Dim iterations As Integer
        If parts.Length <> 4 OrElse parts(0) <> "pbkdf2-sha256" OrElse
            Not Integer.TryParse(parts(1), iterations) OrElse iterations < 600000 OrElse iterations > 2000000 Then Return False
        Try
            Dim salt = Convert.FromBase64String(parts(2))
            Dim expected = Convert.FromBase64String(parts(3))
            If salt.Length <> 16 OrElse expected.Length <> 32 Then Return False
            Using derivation As New Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256)
                Return CryptographicOperations.FixedTimeEquals(derivation.GetBytes(32), expected)
            End Using
        Catch ex As FormatException
            Return False
        End Try
    End Function
End Module
