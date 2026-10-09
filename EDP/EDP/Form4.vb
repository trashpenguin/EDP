Imports System.IO
Imports System.Diagnostics
Imports System.Globalization
Imports CsvHelper
Imports CsvHelper.Configuration
Imports MySql.Data.MySqlClient

Public Class Form4
    Private Sub OpenChild(child As Form)
        child.StartPosition = FormStartPosition.Manual
        child.DesktopLocation = Me.DesktopLocation
        child.Show()
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenChild(New Form2())
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        OpenChild(New Form3())
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        OpenChild(New Form5())
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Form1.Show()
        Me.Close()
    End Sub

    Private Async Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Using dialog As New SaveFileDialog()
            dialog.Filter = "SQL files (*.sql)|*.sql"
            If dialog.ShowDialog() <> DialogResult.OK Then Return
            Dim temporary = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(dialog.FileName)), Guid.NewGuid().ToString("N") & ".sql")
            Button5.Enabled = False
            Try
                Dim value = Environment.GetEnvironmentVariable("EDP_DB_CONNECTION")
                If String.IsNullOrWhiteSpace(value) Then Throw New InvalidOperationException("Set EDP_DB_CONNECTION before starting the application.")
                Dim settings As New MySqlConnectionStringBuilder(value)
                Dim info As New ProcessStartInfo("mysqldump")
                info.UseShellExecute = False
                info.CreateNoWindow = True
                info.RedirectStandardOutput = True
                info.RedirectStandardError = True
                info.ArgumentList.Add("--host=" & settings.Server)
                info.ArgumentList.Add("--port=" & settings.Port.ToString(CultureInfo.InvariantCulture))
                info.ArgumentList.Add("--user=" & settings.UserID)
                info.ArgumentList.Add("--single-transaction")
                info.ArgumentList.Add("--routines")
                info.ArgumentList.Add("--triggers")
                info.ArgumentList.Add("--result-file=" & temporary)
                info.ArgumentList.Add("--databases")
                info.ArgumentList.Add(settings.Database)
                ' Keep the password out of the command line.
                info.Environment("MYSQL_PWD") = settings.Password
                Using process As New Process()
                    process.StartInfo = info
                    process.Start()
                    Dim output = process.StandardOutput.ReadToEndAsync()
                    Dim errors = process.StandardError.ReadToEndAsync()
                    Await process.WaitForExitAsync()
                    Await output
                    Dim errorText = Await errors
                    If process.ExitCode <> 0 Then Throw New IOException("mysqldump failed: " & errorText)
                End Using
                If Not File.Exists(temporary) OrElse New FileInfo(temporary).Length = 0 Then
                    Throw New IOException("mysqldump produced an empty backup.")
                End If
                File.Move(temporary, dialog.FileName, True)
                MessageBox.Show("Database backup completed successfully.")
            Catch ex As Exception
                MessageBox.Show("Backup failed: " & ex.Message)
            Finally
                Button5.Enabled = True
                Try
                    If File.Exists(temporary) Then File.Delete(temporary)
                Catch ex As IOException
                    MessageBox.Show("Could not remove temporary backup: " & temporary)
                Catch ex As UnauthorizedAccessException
                    MessageBox.Show("Could not remove temporary backup: " & temporary)
                End Try
            End Try
        End Using
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Using dialog As New OpenFileDialog()
            dialog.Filter = "CSV files (*.csv)|*.csv"
            If dialog.ShowDialog() <> DialogResult.OK Then Return
            Try
                Dim table As New DataTable()
                Dim configuration As New CsvConfiguration(CultureInfo.InvariantCulture)
                configuration.DetectColumnCountChanges = True
                Using reader As New StreamReader(dialog.FileName),
                      csv As New CsvReader(reader, configuration)
                    If Not csv.Read() Then Throw New InvalidDataException("The CSV file is empty.")
                    csv.ReadHeader()
                    For Each header In csv.HeaderRecord
                        If String.IsNullOrWhiteSpace(header) OrElse table.Columns.Contains(header) Then
                            Throw New InvalidDataException("CSV headers must be nonempty and unique.")
                        End If
                        table.Columns.Add(header)
                    Next
                    While csv.Read()
                        Dim row = table.NewRow()
                        For i As Integer = 0 To table.Columns.Count - 1
                            row(i) = csv.GetField(i)
                        Next
                        table.Rows.Add(row)
                    End While
                End Using
                DataGridView2.DataSource = table
            Catch ex As Exception
                MessageBox.Show("CSV import failed: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        OpenChild(New Form6())
    End Sub
End Class
