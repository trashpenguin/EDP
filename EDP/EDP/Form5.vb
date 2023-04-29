Imports MySql.Data.MySqlClient

Public Class Form5

    Dim conn As New MySqlConnection("DataSource=localhost;database=db;username=root;password=1234;")


    Private Sub load_data2()
        conn.Open()
        Dim cmd1 As New MySqlCommand("select ProductID,ProductName,Price,Description,CategoryID from products", conn)
        Dim da As New MySqlDataAdapter
        da.SelectCommand = cmd1
        Dim dt As New DataTable
        dt.Clear()
        da.Fill(dt)
        DataGridView2.DataSource = dt
        conn.Close()

    End Sub


    Private Sub load_data()
        conn.Open()
        Dim cmd1 As New MySqlCommand("select CustomerID,FirstName,LastName,Email,Phone,Address,City,State,ZipCode,Country from customers", conn)
        Dim da As New MySqlDataAdapter
        da.selectcommand = cmd1
        Dim dt As New DataTable
        dt.Clear()
        da.fill(dt)
        DataGridView1.DataSource = dt
        conn.Close()

    End Sub

    Private Sub form5_load(sender As Object, e As EventArgs) Handles MyBase.Load
        load_data()
        load_data2()

    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide() 'hide form5
        Form4.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim insert_command As New MySqlCommand("INSERT INTO customers(CustomerID,FirstName,LastName,Email,Phone,Address,City,State,ZipCode,Country) VALUES (@id,@fn,@ln,@em,@ph,@adds,@ct,@st,@zp,@cot)", conn)




        insert_command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = TextBox1.Text
        insert_command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = TextBox2.Text
        insert_command.Parameters.Add("@em", MySqlDbType.VarChar).Value = TextBox3.Text
        insert_command.Parameters.Add("@ph", MySqlDbType.VarChar).Value = TextBox4.Text
        insert_command.Parameters.Add("@adds", MySqlDbType.VarChar).Value = TextBox5.Text
        insert_command.Parameters.Add("@ct", MySqlDbType.VarChar).Value = TextBox6.Text
        insert_command.Parameters.Add("@st", MySqlDbType.VarChar).Value = TextBox7.Text
        insert_command.Parameters.Add("@zp", MySqlDbType.VarChar).Value = TextBox8.Text
        insert_command.Parameters.Add("@cot", MySqlDbType.VarChar).Value = TextBox9.Text
        insert_command.Parameters.Add("@id", MySqlDbType.VarChar).Value = TextBox10.Text

        conn.Open()
        insert_command.ExecuteNonQuery()
        conn.Close()
        load_data()



    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Hide() 'hide form5
        Form4.Show()
    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim cmd4 As New MySqlCommand("INSERT INTO products(ProductID,ProductName,Price,Description,CategoryID)VALUES(@pid,@pn,@pr,@des,@cid)", conn)

        cmd4.Parameters.Add("@pid", MySqlDbType.VarChar).Value = TextBox11.Text
        cmd4.Parameters.Add("@pn", MySqlDbType.VarChar).Value = TextBox12.Text
        cmd4.Parameters.Add("@pr", MySqlDbType.VarChar).Value = TextBox13.Text
        cmd4.Parameters.Add("@des", MySqlDbType.VarChar).Value = TextBox14.Text
        cmd4.Parameters.Add("@cid", MySqlDbType.VarChar).Value = TextBox15.Text

        conn.Open()
        cmd4.ExecuteNonQuery()
        conn.Close()
        load_data2()

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class
