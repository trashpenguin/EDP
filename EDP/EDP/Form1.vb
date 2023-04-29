Imports System.Data.SqlTypes
Imports MySql.Data.MySqlClient

Public Class Form1
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            TextBox2.UseSystemPasswordChar = False
        Else
            TextBox2.UseSystemPasswordChar = True

        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim conn As New MySqlConnection("Data Source=localhost;database=db;username=root;password=1234;")
        conn.Open()
        Dim cmd As New MySqlCommand("Select * from users WHERE username=@username AND password=@password", conn)
        cmd.Parameters.AddWithValue("username", TextBox1.Text.Trim)
        cmd.Parameters.AddWithValue("password", TextBox2.Text.Trim)
        Dim reader As MySqlDataReader = cmd.ExecuteReader
        If reader.Read Then
            Me.Hide() 'hide form1
            Form4.Show()

        Else
            MsgBox("Error username and password")
        End If


    End Sub
End Class
