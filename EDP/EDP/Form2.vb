Imports MySql.Data.MySqlClient

Public Class Form2
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form4.Show()
        Me.Close()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim identifier As Integer
        If Not Integer.TryParse(TextBox1.Text, identifier) OrElse identifier <= 0 Then
            MessageBox.Show("Enter a positive numeric ID.")
            Return
        End If
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        Try
            Using conn = Database.OpenConnection(),
                  cmd As New MySqlCommand("SELECT FirstName,LastName,Phone FROM customers WHERE CustomerID=@id", conn)
                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = identifier
                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then
                        TextBox2.Text = Convert.ToString(reader(0))
                        TextBox3.Text = Convert.ToString(reader(1))
                        TextBox4.Text = Convert.ToString(reader(2))
                    Else
                        MessageBox.Show("No data found.")
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Search failed: " & ex.Message)
        End Try
    End Sub
End Class
