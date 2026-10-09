Imports MySql.Data.MySqlClient

Public Class Form5
    Private Sub RefreshRecords()
        Using conn = Database.OpenConnection()
            Dim customers As New DataTable()
            Dim products As New DataTable()
            Using adapter As New MySqlDataAdapter("SELECT CustomerID,FirstName,LastName,Email,Phone,Address,City,State,ZipCode,Country FROM customers", conn)
                adapter.Fill(customers)
            End Using
            Using adapter As New MySqlDataAdapter("SELECT ProductID,ProductName,Price,Description,CategoryID FROM products", conn)
                adapter.Fill(products)
            End Using
            DataGridView1.DataSource = customers
            DataGridView2.DataSource = products
        End Using
    End Sub

    Private Sub form5_load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            RefreshRecords()
        Catch ex As Exception
            MessageBox.Show("Unable to load records: " & ex.Message)
        End Try
    End Sub

    Private Sub Back_Click(sender As Object, e As EventArgs) Handles Button2.Click, Button4.Click
        Form4.Show()
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim identifier As Integer
        If Not Integer.TryParse(TextBox10.Text, identifier) OrElse identifier <= 0 Then
            MessageBox.Show("Enter a positive customer ID.")
            Return
        End If
        If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse String.IsNullOrWhiteSpace(TextBox2.Text) Then
            MessageBox.Show("Enter the customer's first and last name.")
            Return
        End If
        Try
            Using conn = Database.OpenConnection(),
                  cmd As New MySqlCommand("INSERT INTO customers(CustomerID,FirstName,LastName,Email,Phone,Address,City,State,ZipCode,Country) VALUES (@id,@fn,@ln,@em,@ph,@adds,@ct,@st,@zp,@cot)", conn)
                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = identifier
                cmd.Parameters.Add("@fn", MySqlDbType.VarChar).Value = TextBox1.Text.Trim()
                cmd.Parameters.Add("@ln", MySqlDbType.VarChar).Value = TextBox2.Text.Trim()
                cmd.Parameters.Add("@em", MySqlDbType.VarChar).Value = TextBox3.Text.Trim()
                cmd.Parameters.Add("@ph", MySqlDbType.VarChar).Value = TextBox5.Text.Trim()
                cmd.Parameters.Add("@adds", MySqlDbType.VarChar).Value = TextBox4.Text.Trim()
                cmd.Parameters.Add("@ct", MySqlDbType.VarChar).Value = TextBox6.Text.Trim()
                cmd.Parameters.Add("@st", MySqlDbType.VarChar).Value = TextBox7.Text.Trim()
                cmd.Parameters.Add("@zp", MySqlDbType.VarChar).Value = TextBox8.Text.Trim()
                cmd.Parameters.Add("@cot", MySqlDbType.VarChar).Value = TextBox9.Text.Trim()
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            MessageBox.Show("Customer was not saved: " & ex.Message)
            Return
        End Try
        RefreshAfterInsert()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim identifier, category As Integer
        Dim price As Decimal
        If Not Integer.TryParse(TextBox11.Text, identifier) OrElse identifier <= 0 OrElse
            Not Integer.TryParse(TextBox15.Text, category) OrElse category <= 0 OrElse
            Not Decimal.TryParse(TextBox13.Text, price) OrElse price < 0 OrElse price > 99999999.99D OrElse
            String.IsNullOrWhiteSpace(TextBox12.Text) Then
            MessageBox.Show("Enter positive product/category IDs, a product name, and a valid price.")
            Return
        End If
        Try
            Using conn = Database.OpenConnection(),
                  cmd As New MySqlCommand("INSERT INTO products(ProductID,ProductName,Price,Description,CategoryID) VALUES (@pid,@pn,@pr,@des,@cid)", conn)
                cmd.Parameters.Add("@pid", MySqlDbType.Int32).Value = identifier
                cmd.Parameters.Add("@pn", MySqlDbType.VarChar).Value = TextBox12.Text.Trim()
                cmd.Parameters.Add("@pr", MySqlDbType.Decimal).Value = price
                cmd.Parameters.Add("@des", MySqlDbType.VarChar).Value = TextBox14.Text.Trim()
                cmd.Parameters.Add("@cid", MySqlDbType.Int32).Value = category
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            MessageBox.Show("Product was not saved: " & ex.Message)
            Return
        End Try
        RefreshAfterInsert()
    End Sub

    Private Sub RefreshAfterInsert()
        Try
            RefreshRecords()
        Catch ex As Exception
            MessageBox.Show("Record saved, but refreshing the list failed: " & ex.Message)
        End Try
    End Sub
End Class
