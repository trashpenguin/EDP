Module Program
    Sub Main(args As String())
        Dim password = "correct horse battery staple"
        Dim encoded = IO.File.ReadAllText(args(0)).Trim()
        If Not Database.VerifyPassword(password, encoded) Then Throw New Exception("Valid password rejected.")
        If Database.VerifyPassword(password & "x", encoded) Then Throw New Exception("Wrong password accepted.")
        For Each invalid In New String() {Nothing, "", password, "pbkdf2-sha256$600000$bad$bad", "pbkdf2-sha256$1$bad$bad"}
            If Database.VerifyPassword(password, invalid) Then Throw New Exception("Invalid hash accepted.")
        Next
        Environment.SetEnvironmentVariable("EDP_DB_CONNECTION", Nothing)
        Try
            Using connection = Database.OpenConnection()
                Throw New Exception("Missing configuration was accepted.")
            End Using
        Catch ex As InvalidOperationException
            If Not ex.Message.Contains("EDP_DB_CONNECTION") Then Throw
        End Try
        Console.WriteLine("Password verification and configuration checks passed.")
    End Sub
End Module
