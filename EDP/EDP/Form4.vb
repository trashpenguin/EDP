Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Windows.Forms
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Reflection
Public Class Form4

    Private connectionString As String = "server=localhost;port=3306;user id=root;password=1234;database=db"
    Private conn As MySqlConnection = Nothing
    Private backupFilePath As String = ""

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize database connection
        conn = New MySqlConnection(connectionString)
        Try
            conn.Open()
        Catch ex As Exception
            MessageBox.Show("Failed to connect to database: " & ex.Message)
        End Try
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim form2 As New Form2
        form2.StartPosition = FormStartPosition.Manual
        form2.DesktopLocation = Me.DesktopLocation
        form2.Show()
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim form3 As New Form3
        Form2.StartPosition = FormStartPosition.Manual
        form3.DesktopLocation = Me.DesktopLocation
        form3.Show()
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim form5 As New Form5
        Form2.StartPosition = FormStartPosition.Manual
        form5.DesktopLocation = Me.DesktopLocation
        form5.Show()
        Me.Close()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Hide() 'hide form4
        Form1.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ' Prompt user to select backup file location
        Dim saveFileDialog As New SaveFileDialog()
        saveFileDialog.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*"
        saveFileDialog.RestoreDirectory = True
        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            backupFilePath = saveFileDialog.FileName

            ' Perform database backup
            Dim command As MySqlCommand = conn.CreateCommand()
            command.CommandText = "mysqldump --user=root --password=1234 --databases db > """ & backupFilePath & """"
            Try
                Dim process As New Process()
                process.StartInfo.FileName = "cmd.exe"
                process.StartInfo.Arguments = "/c " & command.CommandText
                process.StartInfo.UseShellExecute = False
                process.StartInfo.RedirectStandardOutput = True
                process.StartInfo.RedirectStandardError = True
                process.Start()
                process.WaitForExit()
                MessageBox.Show("Database backup completed successfully.")
            Catch ex As Exception
                MessageBox.Show("Failed to perform database backup: " & ex.Message)
            End Try
        End If
    End Sub


    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        ' Prompt user to select CSV file
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
        openFileDialog.RestoreDirectory = True
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            ' Read CSV file into a DataTable
            Dim dataTable As New DataTable()
            Using reader As New StreamReader(openFileDialog.FileName)
                Dim line As String = reader.ReadLine()
                Dim headers As String() = line.Split(",")
                For Each header As String In headers
                    dataTable.Columns.Add(New DataColumn(header))
                Next
                While Not reader.EndOfStream
                    line = reader.ReadLine()
                    Dim values As String() = line.Split(",")
                    Dim row As DataRow = dataTable.NewRow()
                    For i As Integer = 0 To headers.Length - 1
                        row(i) = values(i)
                    Next
                    dataTable.Rows.Add(row)
                End While
            End Using

            ' Display DataTable in DataGridView
            DataGridView2.DataSource = dataTable
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim form6 As New Form6
        Form2.StartPosition = FormStartPosition.Manual
        Form6.DesktopLocation = Me.DesktopLocation
        form6.Show()
        Me.Close()
    End Sub
End Class

