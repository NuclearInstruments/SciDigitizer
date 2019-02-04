<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class pSpectra
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.CheckedListBox1 = New System.Windows.Forms.CheckedListBox()
        Me.Pesgo1 = New Gigasoft.ProEssentials.Pesgo()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.BackColor = System.Drawing.Color.White
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.CheckedListBox1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Pesgo1, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(622, 426)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'CheckedListBox1
        '
        Me.CheckedListBox1.BackColor = System.Drawing.Color.White
        Me.CheckedListBox1.CheckOnClick = True
        Me.CheckedListBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CheckedListBox1.FormattingEnabled = True
        Me.CheckedListBox1.Location = New System.Drawing.Point(2, 3)
        Me.CheckedListBox1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.CheckedListBox1.Name = "CheckedListBox1"
        Me.CheckedListBox1.Size = New System.Drawing.Size(96, 420)
        Me.CheckedListBox1.TabIndex = 0
        '
        'Pesgo1
        '
        Me.Pesgo1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Pesgo1.Location = New System.Drawing.Point(102, 2)
        Me.Pesgo1.Margin = New System.Windows.Forms.Padding(2)
        Me.Pesgo1.Name = "Pesgo1"
        Me.Pesgo1.Size = New System.Drawing.Size(518, 422)
        Me.Pesgo1.TabIndex = 1
        Me.Pesgo1.Text = "Pesgo1"
        '
        'Timer1
        '
        Me.Timer1.Interval = 500
        '
        'pSpectra
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.Name = "pSpectra"
        Me.Size = New System.Drawing.Size(622, 426)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents CheckedListBox1 As CheckedListBox
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Pesgo1 As Gigasoft.ProEssentials.Pesgo
End Class
