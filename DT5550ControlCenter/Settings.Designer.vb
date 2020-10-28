<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Settings
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TriggerLevelOscilloscope = New System.Windows.Forms.TextBox()
        Me.TriggerSourceOscilloscope = New System.Windows.Forms.ComboBox()
        Me.PreTrigger = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TriggerEdge = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Horizontal = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SignalOffset = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.sampling = New System.Windows.Forms.ComboBox()
        Me.TriggerDelay = New System.Windows.Forms.NumericUpDown()
        Me.TriggerDelayLabel = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TriggerSource = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TriggerMode = New System.Windows.Forms.ComboBox()
        Me.Offset = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Impedance = New System.Windows.Forms.ComboBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Apply = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.SignalOffset, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TriggerDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Offset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoScroll = True
        Me.TableLayoutPanel1.AutoScrollMinSize = New System.Drawing.Size(1000, 500)
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel3, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.DataGridView1, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 214.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1036, 751)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel5)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1030, 208)
        Me.Panel1.TabIndex = 1
        '
        'Panel5
        '
        Me.Panel5.AutoScroll = True
        Me.Panel5.Controls.Add(Me.GroupBox2)
        Me.Panel5.Controls.Add(Me.GroupBox1)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1030, 208)
        Me.Panel5.TabIndex = 4
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Panel4)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox2.Location = New System.Drawing.Point(0, 105)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1030, 103)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Oscilloscope Settings"
        '
        'Panel4
        '
        Me.Panel4.AutoScroll = True
        Me.Panel4.Controls.Add(Me.Label10)
        Me.Panel4.Controls.Add(Me.TriggerLevelOscilloscope)
        Me.Panel4.Controls.Add(Me.TriggerSourceOscilloscope)
        Me.Panel4.Controls.Add(Me.PreTrigger)
        Me.Panel4.Controls.Add(Me.Label13)
        Me.Panel4.Controls.Add(Me.TriggerEdge)
        Me.Panel4.Controls.Add(Me.Label12)
        Me.Panel4.Controls.Add(Me.Label11)
        Me.Panel4.Controls.Add(Me.Horizontal)
        Me.Panel4.Controls.Add(Me.Label14)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(3, 16)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1024, 84)
        Me.Panel4.TabIndex = 60
        '
        'Label10
        '
        Me.Label10.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(414, 46)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(76, 13)
        Me.Label10.TabIndex = 51
        Me.Label10.Text = "Pre Trigger (%)"
        '
        'TriggerLevelOscilloscope
        '
        Me.TriggerLevelOscilloscope.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TriggerLevelOscilloscope.BackColor = System.Drawing.Color.White
        Me.TriggerLevelOscilloscope.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TriggerLevelOscilloscope.Location = New System.Drawing.Point(913, 10)
        Me.TriggerLevelOscilloscope.Name = "TriggerLevelOscilloscope"
        Me.TriggerLevelOscilloscope.Size = New System.Drawing.Size(96, 20)
        Me.TriggerLevelOscilloscope.TabIndex = 59
        Me.TriggerLevelOscilloscope.Text = "10000"
        '
        'TriggerSourceOscilloscope
        '
        Me.TriggerSourceOscilloscope.DropDownHeight = 446
        Me.TriggerSourceOscilloscope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TriggerSourceOscilloscope.FormattingEnabled = True
        Me.TriggerSourceOscilloscope.IntegralHeight = False
        Me.TriggerSourceOscilloscope.Location = New System.Drawing.Point(106, 9)
        Me.TriggerSourceOscilloscope.Name = "TriggerSourceOscilloscope"
        Me.TriggerSourceOscilloscope.Size = New System.Drawing.Size(96, 21)
        Me.TriggerSourceOscilloscope.TabIndex = 57
        '
        'PreTrigger
        '
        Me.PreTrigger.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.PreTrigger.BackColor = System.Drawing.Color.White
        Me.PreTrigger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PreTrigger.Location = New System.Drawing.Point(512, 44)
        Me.PreTrigger.Name = "PreTrigger"
        Me.PreTrigger.Size = New System.Drawing.Size(97, 20)
        Me.PreTrigger.TabIndex = 52
        Me.PreTrigger.Text = "20"
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(8, 12)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(77, 14)
        Me.Label13.TabIndex = 58
        Me.Label13.Text = "Trigger Source"
        '
        'TriggerEdge
        '
        Me.TriggerEdge.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TriggerEdge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TriggerEdge.FormattingEnabled = True
        Me.TriggerEdge.Location = New System.Drawing.Point(513, 9)
        Me.TriggerEdge.Name = "TriggerEdge"
        Me.TriggerEdge.Size = New System.Drawing.Size(96, 21)
        Me.TriggerEdge.TabIndex = 53
        '
        'Label12
        '
        Me.Label12.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(414, 12)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(68, 13)
        Me.Label12.TabIndex = 54
        Me.Label12.Text = "Trigger Edge"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(8, 46)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(93, 13)
        Me.Label11.TabIndex = 49
        Me.Label11.Text = "Horizontal (ns/div)"
        '
        'Horizontal
        '
        Me.Horizontal.BackColor = System.Drawing.Color.White
        Me.Horizontal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Horizontal.Location = New System.Drawing.Point(106, 44)
        Me.Horizontal.Name = "Horizontal"
        Me.Horizontal.Size = New System.Drawing.Size(96, 20)
        Me.Horizontal.TabIndex = 50
        Me.Horizontal.Text = "12.5"
        '
        'Label14
        '
        Me.Label14.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label14.Location = New System.Drawing.Point(810, 12)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(70, 13)
        Me.Label14.TabIndex = 56
        Me.Label14.Text = "Trigger Level"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Panel2)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1030, 105)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "General Settings"
        '
        'Panel2
        '
        Me.Panel2.AutoScroll = True
        Me.Panel2.Controls.Add(Me.Button1)
        Me.Panel2.Controls.Add(Me.SignalOffset)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.sampling)
        Me.Panel2.Controls.Add(Me.TriggerDelay)
        Me.Panel2.Controls.Add(Me.TriggerDelayLabel)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.TriggerSource)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.TriggerMode)
        Me.Panel2.Controls.Add(Me.Offset)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Impedance)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 16)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1024, 86)
        Me.Panel2.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(533, 6)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(184, 23)
        Me.Button1.TabIndex = 40
        Me.Button1.Text = "Automatic Offset Calibration"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'SignalOffset
        '
        Me.SignalOffset.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.SignalOffset.BackColor = System.Drawing.Color.White
        Me.SignalOffset.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.SignalOffset.Location = New System.Drawing.Point(915, 9)
        Me.SignalOffset.Maximum = New Decimal(New Integer() {65536, 0, 0, 0})
        Me.SignalOffset.Name = "SignalOffset"
        Me.SignalOffset.Size = New System.Drawing.Size(96, 20)
        Me.SignalOffset.TabIndex = 39
        Me.SignalOffset.Value = New Decimal(New Integer() {155, 0, 0, 131072})
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label1.Location = New System.Drawing.Point(812, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 13)
        Me.Label1.TabIndex = 38
        Me.Label1.Text = "Spectrum Offset (lsb)"
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.Location = New System.Drawing.Point(812, 43)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(89, 13)
        Me.Label9.TabIndex = 37
        Me.Label9.Text = "Sampling Method"
        '
        'sampling
        '
        Me.sampling.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.sampling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.sampling.FormattingEnabled = True
        Me.sampling.Location = New System.Drawing.Point(915, 40)
        Me.sampling.Name = "sampling"
        Me.sampling.Size = New System.Drawing.Size(96, 21)
        Me.sampling.TabIndex = 36
        '
        'TriggerDelay
        '
        Me.TriggerDelay.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TriggerDelay.BackColor = System.Drawing.Color.White
        Me.TriggerDelay.Location = New System.Drawing.Point(385, 45)
        Me.TriggerDelay.Name = "TriggerDelay"
        Me.TriggerDelay.Size = New System.Drawing.Size(96, 20)
        Me.TriggerDelay.TabIndex = 35
        '
        'TriggerDelayLabel
        '
        Me.TriggerDelayLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TriggerDelayLabel.Location = New System.Drawing.Point(289, 47)
        Me.TriggerDelayLabel.Name = "TriggerDelayLabel"
        Me.TriggerDelayLabel.Size = New System.Drawing.Size(90, 13)
        Me.TriggerDelayLabel.TabIndex = 34
        Me.TriggerDelayLabel.Text = "Trigger Delay (ns)"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 47)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(77, 13)
        Me.Label5.TabIndex = 29
        Me.Label5.Text = "Trigger Source"
        '
        'TriggerSource
        '
        Me.TriggerSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TriggerSource.FormattingEnabled = True
        Me.TriggerSource.Location = New System.Drawing.Point(106, 44)
        Me.TriggerSource.Name = "TriggerSource"
        Me.TriggerSource.Size = New System.Drawing.Size(96, 21)
        Me.TriggerSource.TabIndex = 28
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label4.Location = New System.Drawing.Point(530, 48)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 13)
        Me.Label4.TabIndex = 27
        Me.Label4.Text = "Trigger Mode"
        '
        'TriggerMode
        '
        Me.TriggerMode.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TriggerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TriggerMode.FormattingEnabled = True
        Me.TriggerMode.Location = New System.Drawing.Point(621, 45)
        Me.TriggerMode.Name = "TriggerMode"
        Me.TriggerMode.Size = New System.Drawing.Size(96, 21)
        Me.TriggerMode.TabIndex = 26
        '
        'Offset
        '
        Me.Offset.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Offset.BackColor = System.Drawing.Color.White
        Me.Offset.DecimalPlaces = 3
        Me.Offset.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.Offset.Location = New System.Drawing.Point(385, 9)
        Me.Offset.Maximum = New Decimal(New Integer() {9, 0, 0, 65536})
        Me.Offset.Minimum = New Decimal(New Integer() {9, 0, 0, -2147418112})
        Me.Offset.Name = "Offset"
        Me.Offset.Size = New System.Drawing.Size(96, 20)
        Me.Offset.TabIndex = 25
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label3.Location = New System.Drawing.Point(292, 11)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(91, 13)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "AFE Offset (V)"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 13)
        Me.Label2.TabIndex = 23
        Me.Label2.Text = "AFE Impedence"
        '
        'Impedance
        '
        Me.Impedance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Impedance.FormattingEnabled = True
        Me.Impedance.Location = New System.Drawing.Point(107, 8)
        Me.Impedance.Name = "Impedance"
        Me.Impedance.Size = New System.Drawing.Size(96, 21)
        Me.Impedance.TabIndex = 22
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Apply)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(3, 724)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1030, 24)
        Me.Panel3.TabIndex = 2
        '
        'Apply
        '
        Me.Apply.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Apply.Location = New System.Drawing.Point(416, 0)
        Me.Apply.Name = "Apply"
        Me.Apply.Size = New System.Drawing.Size(178, 21)
        Me.Apply.TabIndex = 0
        Me.Apply.Text = "APPLY SETTINGS"
        Me.Apply.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.DataGridView1.Location = New System.Drawing.Point(3, 217)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.DataGridView1.Size = New System.Drawing.Size(1030, 501)
        Me.DataGridView1.TabIndex = 0
        '
        'Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "Settings"
        Me.Size = New System.Drawing.Size(1036, 751)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        CType(Me.SignalOffset, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TriggerDelay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Offset, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Apply As Button
    Friend WithEvents Panel5 As Panel
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Label10 As Label
    Friend WithEvents TriggerLevelOscilloscope As TextBox
    Friend WithEvents TriggerSourceOscilloscope As ComboBox
    Friend WithEvents PreTrigger As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents TriggerEdge As ComboBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Horizontal As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents sampling As ComboBox
    Friend WithEvents TriggerDelay As NumericUpDown
    Friend WithEvents TriggerDelayLabel As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents TriggerSource As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents TriggerMode As ComboBox
    Friend WithEvents Offset As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Impedance As ComboBox
    Friend WithEvents SignalOffset As NumericUpDown
    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
End Class
