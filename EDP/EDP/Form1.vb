Imports MySql.Data.MySqlClient

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox2.UseSystemPasswordChar = True
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        TextBox2.UseSystemPasswordChar = Not CheckBox1.Checked
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim authenticated As Boolean = False
            Using conn = Database.OpenConnection(),
                  cmd As New MySqlCommand("SELECT password FROM users WHERE username=@username", conn)
                cmd.Parameters.Add("@username", MySqlDbType.VarChar).Value = TextBox1.Text.Trim()
                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        If Not reader.IsDBNull(0) AndAlso Database.VerifyPassword(TextBox2.Text, reader.GetString(0)) Then
                            authenticated = True
                            Exit While
                        End If
                    End While
                End Using
            End Using
            TextBox2.Clear()
            If authenticated Then
                Me.Hide()
                Form4.Show()
            Else
                MessageBox.Show("Invalid username or password.")
            End If
        Catch ex As Exception
            MessageBox.Show("Unable to sign in: " & ex.Message)
        End Try
    End Sub
End Class
