<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WaveformCaptureSelect
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
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

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WaveformCaptureSelect))
        Me.FileName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ChList = New System.Windows.Forms.CheckedListBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TargetValue = New System.Windows.Forms.TextBox()
        Me.TargetMode = New System.Windows.Forms.ComboBox()
        Me.TargetValueUnit = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.DataType = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'FileName
        '
        Me.FileName.BackColor = System.Drawing.Color.White
        Me.FileName.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.FileName.Enabled = False
        Me.FileName.HideSelection = False
        Me.FileName.Location = New System.Drawing.Point(105, 22)
        Me.FileName.Margin = New System.Windows.Forms.Padding(2)
        Me.FileName.Name = "FileName"
        Me.FileName.ReadOnly = True
        Me.FileName.Size = New System.Drawing.Size(417, 20)
        Me.FileName.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(27, 25)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Data File Path"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(533, 20)
        Me.Button1.Margin = New System.Windows.Forms.Padding(2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(50, 22)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "..."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ChList
        '
        Me.ChList.BackColor = System.Drawing.Color.White
        Me.ChList.CheckOnClick = True
        Me.ChList.FormattingEnabled = True
        Me.ChList.Location = New System.Drawing.Point(30, 93)
        Me.ChList.Margin = New System.Windows.Forms.Padding(2)
        Me.ChList.MultiColumn = True
        Me.ChList.Name = "ChList"
        Me.ChList.Size = New System.Drawing.Size(492, 154)
        Me.ChList.TabIndex = 3
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(483, 302)
        Me.Button2.Margin = New System.Windows.Forms.Padding(2)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(100, 33)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "START"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(30, 312)
        Me.Button3.Margin = New System.Windows.Forms.Padding(2)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(73, 23)
        Me.Button3.TabIndex = 5
        Me.Button3.Text = "Cancel"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "CSV File (*.csv)|*.csv"
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(533, 93)
        Me.Button4.Margin = New System.Windows.Forms.Padding(2)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(50, 23)
        Me.Button4.TabIndex = 6
        Me.Button4.Text = "ALL"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(533, 120)
        Me.Button5.Margin = New System.Windows.Forms.Padding(2)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(50, 23)
        Me.Button5.TabIndex = 7
        Me.Button5.Text = "NONE"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(27, 269)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Target Mode"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Enabled = False
        Me.Label3.Location = New System.Drawing.Point(303, 268)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(68, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Target Value"
        '
        'TargetValue
        '
        Me.TargetValue.BackColor = System.Drawing.Color.White
        Me.TargetValue.Enabled = False
        Me.TargetValue.Location = New System.Drawing.Point(377, 265)
        Me.TargetValue.Name = "TargetValue"
        Me.TargetValue.Size = New System.Drawing.Size(100, 20)
        Me.TargetValue.TabIndex = 10
        '
        'TargetMode
        '
        Me.TargetMode.BackColor = System.Drawing.Color.White
        Me.TargetMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TargetMode.FormattingEnabled = True
        Me.TargetMode.Location = New System.Drawing.Point(101, 265)
        Me.TargetMode.Name = "TargetMode"
        Me.TargetMode.Size = New System.Drawing.Size(121, 21)
        Me.TargetMode.TabIndex = 11
        '
        'TargetValueUnit
        '
        Me.TargetValueUnit.BackColor = System.Drawing.Color.White
        Me.TargetValueUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TargetValueUnit.Enabled = False
        Me.TargetValueUnit.FormattingEnabled = True
        Me.TargetValueUnit.Location = New System.Drawing.Point(483, 265)
        Me.TargetValueUnit.Name = "TargetValueUnit"
        Me.TargetValueUnit.Size = New System.Drawing.Size(38, 21)
        Me.TargetValueUnit.TabIndex = 12
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(27, 59)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 13)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Data Type"
        '
        'DataType
        '
        Me.DataType.BackColor = System.Drawing.Color.White
        Me.DataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DataType.FormattingEnabled = True
        Me.DataType.Location = New System.Drawing.Point(105, 56)
        Me.DataType.Name = "DataType"
        Me.DataType.Size = New System.Drawing.Size(121, 21)
        Me.DataType.TabIndex = 14
        '
        'WaveformCaptureSelect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(606, 355)
        Me.Controls.Add(Me.DataType)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TargetValueUnit)
        Me.Controls.Add(Me.TargetMode)
        Me.Controls.Add(Me.TargetValue)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.ChList)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.FileName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MinimizeBox = False
        Me.Name = "WaveformCaptureSelect"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Data Record Configuration"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents FileName As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents ChList As CheckedListBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents TargetValue As TextBox
    Friend WithEvents TargetMode As ComboBox
    Friend WithEvents TargetValueUnit As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents DataType As ComboBox
End Class
