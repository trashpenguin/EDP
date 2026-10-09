Option Strict Off
Imports MySql.Data.MySqlClient
Imports System.Runtime.InteropServices

Public Class Form6
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using conn = Database.OpenConnection(),
                  adapter As New MySqlDataAdapter("SELECT * FROM ordersbycustomer", conn)
                Dim table As New DataTable()
                adapter.Fill(table)
                DataGridView1.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("Unable to load records: " & ex.Message)
        End Try
    End Sub

    Private Sub Release(value As Object)
        If value IsNot Nothing AndAlso Marshal.IsComObject(value) Then Marshal.FinalReleaseComObject(value)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim table = TryCast(DataGridView1.DataSource, DataTable)
        If table Is Nothing OrElse table.Columns.Count = 0 Then
            MessageBox.Show("Load records before exporting.")
            Return
        End If
        Using dialog As New SaveFileDialog()
            dialog.Filter = "Excel files (*.xlsx)|*.xlsx"
            If dialog.ShowDialog() <> DialogResult.OK Then Return
            Dim application As Object = Nothing
            Dim workbooks As Object = Nothing
            Dim workbook As Object = Nothing
            Dim sheets As Object = Nothing
            Dim worksheet As Object = Nothing
            Dim cells As Object = Nothing
            Try
                application = CreateObject("Excel.Application")
                application.DisplayAlerts = False
                workbooks = application.Workbooks
                workbook = workbooks.Add()
                sheets = workbook.Worksheets
                worksheet = sheets.Item(1)
                cells = worksheet.Cells
                For column As Integer = 0 To table.Columns.Count - 1
                    Dim cell = cells.Item(1, column + 1)
                    Try
                        cell.NumberFormat = "@"
                        cell.Value2 = table.Columns(column).ColumnName
                    Finally
                        Release(cell)
                    End Try
                Next
                For row As Integer = 0 To table.Rows.Count - 1
                    For column As Integer = 0 To table.Columns.Count - 1
                        Dim cell = cells.Item(row + 2, column + 1)
                        Try
                            Dim value = table.Rows(row)(column)
                            If TypeOf value Is String Then cell.NumberFormat = "@"
                            If Not Convert.IsDBNull(value) Then cell.Value2 = Convert.ToString(value)
                        Finally
                            Release(cell)
                        End Try
                    Next
                Next
                workbook.SaveAs(dialog.FileName, 51)
                MessageBox.Show("Records exported successfully.")
            Catch ex As Exception
                MessageBox.Show("Excel export failed: " & ex.Message)
            Finally
                Try
                    If workbook IsNot Nothing Then workbook.Close(False)
                Catch ex As COMException
                    Diagnostics.Debug.WriteLine(ex)
                Finally
                    Try
                        If application IsNot Nothing Then application.Quit()
                    Catch ex As COMException
                        Diagnostics.Debug.WriteLine(ex)
                    Finally
                        Release(cells)
                        Release(worksheet)
                        Release(sheets)
                        Release(workbook)
                        Release(workbooks)
                        Release(application)
                    End Try
                End Try
            End Try
        End Using
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form4.Show()
        Me.Close()
    End Sub
End Class
