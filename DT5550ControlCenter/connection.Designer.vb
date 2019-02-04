<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Connection
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Connection_selection = New System.Windows.Forms.ComboBox()
        Me.JsonFilePath = New System.Windows.Forms.TextBox()
        Me.Browse = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Connect = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Firmware_selection = New System.Windows.Forms.ComboBox()
        Me.LabelDeviceList = New System.Windows.Forms.Label()
        Me.DeviceList = New System.Windows.Forms.ComboBox()
        Me.LabelIP = New System.Windows.Forms.Label()
        Me.IP = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.sW = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 215)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 13)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Connection Type"
        '
        'Connection_selection
        '
        Me.Connection_selection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Connection_selection.FormattingEnabled = True
        Me.Connection_selection.Location = New System.Drawing.Point(106, 212)
        Me.Connection_selection.Name = "Connection_selection"
        Me.Connection_selection.Size = New System.Drawing.Size(121, 21)
        Me.Connection_selection.TabIndex = 14
        '
        'JsonFilePath
        '
        Me.JsonFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.JsonFilePath.Location = New System.Drawing.Point(106, 181)
        Me.JsonFilePath.Name = "JsonFilePath"
        Me.JsonFilePath.Size = New System.Drawing.Size(357, 20)
        Me.JsonFilePath.TabIndex = 13
        '
        'Browse
        '
        Me.Browse.Location = New System.Drawing.Point(469, 179)
        Me.Browse.Name = "Browse"
        Me.Browse.Size = New System.Drawing.Size(75, 23)
        Me.Browse.TabIndex = 12
        Me.Browse.Text = "Browse..."
        Me.Browse.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 183)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Select Json File"
        '
        'Connect
        '
        Me.Connect.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Connect.Location = New System.Drawing.Point(230, 275)
        Me.Connect.Name = "Connect"
        Me.Connect.Size = New System.Drawing.Size(91, 32)
        Me.Connect.TabIndex = 10
        Me.Connect.Text = "Connect"
        Me.Connect.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 151)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 19
        Me.Label4.Text = "Firmware Type"
        '
        'Firmware_selection
        '
        Me.Firmware_selection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Firmware_selection.FormattingEnabled = True
        Me.Firmware_selection.Location = New System.Drawing.Point(106, 148)
        Me.Firmware_selection.Name = "Firmware_selection"
        Me.Firmware_selection.Size = New System.Drawing.Size(121, 21)
        Me.Firmware_selection.TabIndex = 18
        '
        'LabelDeviceList
        '
        Me.LabelDeviceList.AutoSize = True
        Me.LabelDeviceList.Location = New System.Drawing.Point(265, 217)
        Me.LabelDeviceList.Name = "LabelDeviceList"
        Me.LabelDeviceList.Size = New System.Drawing.Size(73, 13)
        Me.LabelDeviceList.TabIndex = 23
        Me.LabelDeviceList.Text = "Serial Number"
        '
        'DeviceList
        '
        Me.DeviceList.BackColor = System.Drawing.Color.White
        Me.DeviceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DeviceList.FormattingEnabled = True
        Me.DeviceList.Location = New System.Drawing.Point(342, 212)
        Me.DeviceList.Name = "DeviceList"
        Me.DeviceList.Size = New System.Drawing.Size(121, 21)
        Me.DeviceList.TabIndex = 22
        '
        'LabelIP
        '
        Me.LabelIP.AutoSize = True
        Me.LabelIP.Location = New System.Drawing.Point(321, 242)
        Me.LabelIP.Name = "LabelIP"
        Me.LabelIP.Size = New System.Drawing.Size(17, 13)
        Me.LabelIP.TabIndex = 21
        Me.LabelIP.Text = "IP"
        Me.LabelIP.Visible = False
        '
        'IP
        '
        Me.IP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.IP.Location = New System.Drawing.Point(342, 239)
        Me.IP.Name = "IP"
        Me.IP.Size = New System.Drawing.Size(121, 20)
        Me.IP.TabIndex = 20
        Me.IP.Visible = False
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(469, 212)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 21)
        Me.Button1.TabIndex = 25
        Me.Button1.Text = "Refresh"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(25, 351)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(237, 13)
        Me.Label3.TabIndex = 26
        Me.Label3.Text = "This software is designed by Nuclear Instruments"
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(384, 351)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(146, 13)
        Me.LinkLabel1.TabIndex = 27
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "http://www.scicompiler.cloud"
        '
        'sW
        '
        Me.sW.AutoSize = True
        Me.sW.BackColor = System.Drawing.Color.Black
        Me.sW.ForeColor = System.Drawing.Color.White
        Me.sW.Location = New System.Drawing.Point(10, 91)
        Me.sW.Name = "sW"
        Me.sW.Size = New System.Drawing.Size(112, 13)
        Me.sW.TabIndex = 28
        Me.sW.Text = "Software version: xxxx"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.DT5550ControlCenter.My.Resources.Resources.SPLASH_con1
        Me.PictureBox1.Location = New System.Drawing.Point(-2, -1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(557, 110)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 24
        Me.PictureBox1.TabStop = False
        '
        'Connection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(556, 381)
        Me.Controls.Add(Me.sW)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.LabelDeviceList)
        Me.Controls.Add(Me.DeviceList)
        Me.Controls.Add(Me.LabelIP)
        Me.Controls.Add(Me.IP)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Firmware_selection)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Connection_selection)
        Me.Controls.Add(Me.JsonFilePath)
        Me.Controls.Add(Me.Browse)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Connect)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Connection"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Connection"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As Label
    Friend WithEvents Connection_selection As ComboBox
    Friend WithEvents JsonFilePath As TextBox
    Friend WithEvents Browse As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Connect As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Firmware_selection As ComboBox
    Friend WithEvents LabelDeviceList As Label
    Friend WithEvents DeviceList As ComboBox
    Friend WithEvents LabelIP As Label
    Friend WithEvents IP As TextBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents sW As Label
End Class
