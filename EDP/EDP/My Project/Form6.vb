Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Windows.Forms
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Reflection
Imports System.Runtime.InteropServices

Public Class Form6


    Private connectionString As String = "server=localhost;port=3306;user id=root;password=1234;database=db"
    Private conn As MySqlConnection = Nothing
    Private backupFilePath As String = ""


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Initialize database connection
        conn = New MySqlConnection(connectionString)
        Try
            conn.Open()
        Catch ex As Exception
            MessageBox.Show("Failed to connect to database: " & ex.Message)
        End Try

        ' Load records into DataGridView
        LoadRecords()
    End Sub
    Private Sub LoadRecords()
        ' Query database for records
        Dim command As MySqlCommand = conn.CreateCommand()
        command.CommandText = "SELECT * FROM db.ordersbycustomer"
        Dim adapter As New MySqlDataAdapter(command)
        Dim dataTable As New DataTable()
        adapter.Fill(dataTable)

        ' Display records in DataGridView
        DataGridView1.DataSource = dataTable
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Export records to Excel
        Dim saveFileDialog As New SaveFileDialog()
        saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*"
        saveFileDialog.RestoreDirectory = True
        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            Dim excelFilePath As String = saveFileDialog.FileName

            ' Create Excel application and workbook
            Dim excel As New Microsoft.Office.Interop.Excel.Application()
            Dim workbook As Microsoft.Office.Interop.Excel.Workbook = excel.Workbooks.Add()
            Dim worksheet As Microsoft.Office.Interop.Excel.Worksheet = workbook.Sheets(1)

            ' Copy records from DataGridView to Excel worksheet
            Dim rowsTotal As Integer = DataGridView1.RowCount - 1
            Dim columnsTotal As Integer = DataGridView1.ColumnCount - 1
            For i As Integer = 0 To rowsTotal
                For j As Integer = 0 To columnsTotal
                    worksheet.Cells(i + 1, j + 1) = DataGridView1.Rows(i).Cells(j).Value
                Next
            Next

            ' Save Excel workbook and quit Excel application
            workbook.SaveAs(excelFilePath)
            workbook.Close()
            excel.Quit()
            Marshal.ReleaseComObject(worksheet)
            Marshal.ReleaseComObject(workbook)
            Marshal.ReleaseComObject(excel)

            ' Display success message
            MessageBox.Show("Records exported to Excel successfully.")
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick


    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim form4 As New Form4
        Form2.StartPosition = FormStartPosition.Manual
        form4.DesktopLocation = Me.DesktopLocation
        form4.Show()
        Me.Close()
    End Sub
End Class