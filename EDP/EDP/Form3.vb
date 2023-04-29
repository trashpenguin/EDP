Imports MySql.Data.MySqlClient

Public Class Form3

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide() 'hide form3
        Form4.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim conn As New MySqlConnection("Data Source=localhost;database=db;username=root;password=1234;")
        conn.Open()

        Dim search_command As New MySqlCommand("SELECT * FROM `products` WHERE `ProductID` = @id", conn)
        search_command.Parameters.Add("@id", MySqlDbType.Int64).Value = TextBox1.Text

        Dim adapter As New MySqlDataAdapter(search_command)

        Dim table As New DataTable()

        Try

            adapter.Fill(table)


            If table.Rows.Count > 0 Then
                TextBox2.Text = table(0)(1)
                TextBox3.Text = table(0)(2)
                TextBox4.Text = table(0)(3)



            Else
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""
                MessageBox.Show("No Data Found")

            End If



        Catch ex As Exception

            MessageBox.Show("ERROR")


        End Try


    End Sub
End Class