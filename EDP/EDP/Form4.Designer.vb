<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form4
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Button1 = New Button()
        Button2 = New Button()
        Button3 = New Button()
        Button4 = New Button()
        Button5 = New Button()
        Button6 = New Button()
        DataGridView2 = New DataGridView()
        Button7 = New Button()
        CType(DataGridView2, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(27, 45)
        Button1.Name = "Button1"
        Button1.Size = New Size(183, 82)
        Button1.TabIndex = 0
        Button1.Text = "Customer details"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(27, 152)
        Button2.Name = "Button2"
        Button2.Size = New Size(183, 82)
        Button2.TabIndex = 1
        Button2.Text = "Product details"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(27, 258)
        Button3.Name = "Button3"
        Button3.Size = New Size(183, 82)
        Button3.TabIndex = 2
        Button3.Text = "insert details"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(27, 366)
        Button4.Name = "Button4"
        Button4.Size = New Size(183, 82)
        Button4.TabIndex = 3
        Button4.Text = "Exit"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(264, 366)
        Button5.Name = "Button5"
        Button5.Size = New Size(183, 82)
        Button5.TabIndex = 4
        Button5.Text = "backup"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' Button6
        ' 
        Button6.Location = New Point(264, 258)
        Button6.Name = "Button6"
        Button6.Size = New Size(183, 82)
        Button6.TabIndex = 5
        Button6.Text = "load"
        Button6.UseVisualStyleBackColor = True
        ' 
        ' DataGridView2
        ' 
        DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView2.Location = New Point(466, 45)
        DataGridView2.Name = "DataGridView2"
        DataGridView2.RowHeadersWidth = 51
        DataGridView2.RowTemplate.Height = 29
        DataGridView2.Size = New Size(405, 403)
        DataGridView2.TabIndex = 12
        ' 
        ' Button7
        ' 
        Button7.Location = New Point(264, 152)
        Button7.Name = "Button7"
        Button7.Size = New Size(183, 82)
        Button7.TabIndex = 13
        Button7.Text = "print"
        Button7.UseVisualStyleBackColor = True
        ' 
        ' Form4
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(883, 482)
        Controls.Add(Button7)
        Controls.Add(DataGridView2)
        Controls.Add(Button6)
        Controls.Add(Button5)
        Controls.Add(Button4)
        Controls.Add(Button3)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Name = "Form4"
        Text = "Form4"
        CType(DataGridView2, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents Button7 As Button
End Class
