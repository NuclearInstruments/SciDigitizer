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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Connection))
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.sW = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.LabelDeviceList = New System.Windows.Forms.Label()
        Me.DeviceList = New System.Windows.Forms.ComboBox()
        Me.LabelIP = New System.Windows.Forms.Label()
        Me.IP = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Firmware_selection = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Connection_selection = New System.Windows.Forms.ComboBox()
        Me.JsonFilePath = New System.Windows.Forms.TextBox()
        Me.Browse = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Connect = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.RemoveButton = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.Connect_R5560 = New System.Windows.Forms.Button()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.DeviceListSciDK = New System.Windows.Forms.ComboBox()
        Me.ConnectSciDK = New System.Windows.Forms.Button()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.Remove_DT5560SE = New System.Windows.Forms.Button()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.Add_DT5560SE = New System.Windows.Forms.Button()
        Me.Connect_DT5560SE = New System.Windows.Forms.Button()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.Remove_R5560SE = New System.Windows.Forms.Button()
        Me.DataGridView3 = New System.Windows.Forms.DataGridView()
        Me.Add_R5560SE = New System.Windows.Forms.Button()
        Me.Connect_R5560SE = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage5.SuspendLayout()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 351)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(237, 13)
        Me.Label3.TabIndex = 26
        Me.Label3.Text = "This software is designed by Nuclear Instruments"
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(401, 351)
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
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Location = New System.Drawing.Point(-2, 115)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(558, 218)
        Me.TabControl1.TabIndex = 29
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Button1)
        Me.TabPage1.Controls.Add(Me.LabelDeviceList)
        Me.TabPage1.Controls.Add(Me.DeviceList)
        Me.TabPage1.Controls.Add(Me.LabelIP)
        Me.TabPage1.Controls.Add(Me.IP)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.Firmware_selection)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.Connection_selection)
        Me.TabPage1.Controls.Add(Me.JsonFilePath)
        Me.TabPage1.Controls.Add(Me.Browse)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.Connect)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(550, 192)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "DT5550"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(465, 79)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 21)
        Me.Button1.TabIndex = 38
        Me.Button1.Text = "Refresh"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'LabelDeviceList
        '
        Me.LabelDeviceList.AutoSize = True
        Me.LabelDeviceList.Location = New System.Drawing.Point(261, 84)
        Me.LabelDeviceList.Name = "LabelDeviceList"
        Me.LabelDeviceList.Size = New System.Drawing.Size(73, 13)
        Me.LabelDeviceList.TabIndex = 37
        Me.LabelDeviceList.Text = "Serial Number"
        '
        'DeviceList
        '
        Me.DeviceList.BackColor = System.Drawing.Color.White
        Me.DeviceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DeviceList.FormattingEnabled = True
        Me.DeviceList.Location = New System.Drawing.Point(338, 79)
        Me.DeviceList.Name = "DeviceList"
        Me.DeviceList.Size = New System.Drawing.Size(121, 21)
        Me.DeviceList.TabIndex = 36
        '
        'LabelIP
        '
        Me.LabelIP.AutoSize = True
        Me.LabelIP.Location = New System.Drawing.Point(317, 109)
        Me.LabelIP.Name = "LabelIP"
        Me.LabelIP.Size = New System.Drawing.Size(17, 13)
        Me.LabelIP.TabIndex = 35
        Me.LabelIP.Text = "IP"
        Me.LabelIP.Visible = False
        '
        'IP
        '
        Me.IP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.IP.Location = New System.Drawing.Point(338, 106)
        Me.IP.Name = "IP"
        Me.IP.Size = New System.Drawing.Size(121, 20)
        Me.IP.TabIndex = 34
        Me.IP.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(5, 18)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = "Firmware Type"
        '
        'Firmware_selection
        '
        Me.Firmware_selection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Firmware_selection.FormattingEnabled = True
        Me.Firmware_selection.Location = New System.Drawing.Point(102, 15)
        Me.Firmware_selection.Name = "Firmware_selection"
        Me.Firmware_selection.Size = New System.Drawing.Size(121, 21)
        Me.Firmware_selection.TabIndex = 32
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 82)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 13)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "Connection Type"
        '
        'Connection_selection
        '
        Me.Connection_selection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Connection_selection.FormattingEnabled = True
        Me.Connection_selection.Location = New System.Drawing.Point(102, 79)
        Me.Connection_selection.Name = "Connection_selection"
        Me.Connection_selection.Size = New System.Drawing.Size(121, 21)
        Me.Connection_selection.TabIndex = 30
        '
        'JsonFilePath
        '
        Me.JsonFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.JsonFilePath.Location = New System.Drawing.Point(102, 48)
        Me.JsonFilePath.Name = "JsonFilePath"
        Me.JsonFilePath.Size = New System.Drawing.Size(357, 20)
        Me.JsonFilePath.TabIndex = 29
        '
        'Browse
        '
        Me.Browse.Location = New System.Drawing.Point(465, 46)
        Me.Browse.Name = "Browse"
        Me.Browse.Size = New System.Drawing.Size(75, 23)
        Me.Browse.TabIndex = 28
        Me.Browse.Text = "Browse..."
        Me.Browse.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 13)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "Select Json File"
        '
        'Connect
        '
        Me.Connect.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Connect.Location = New System.Drawing.Point(222, 145)
        Me.Connect.Name = "Connect"
        Me.Connect.Size = New System.Drawing.Size(91, 32)
        Me.Connect.TabIndex = 26
        Me.Connect.Text = "Connect"
        Me.Connect.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.RemoveButton)
        Me.TabPage2.Controls.Add(Me.DataGridView1)
        Me.TabPage2.Controls.Add(Me.AddButton)
        Me.TabPage2.Controls.Add(Me.Connect_R5560)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(550, 192)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "R5560"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'RemoveButton
        '
        Me.RemoveButton.Image = Global.DT5550ControlCenter.My.Resources.Resources.remove2
        Me.RemoveButton.Location = New System.Drawing.Point(477, 73)
        Me.RemoveButton.Name = "RemoveButton"
        Me.RemoveButton.Size = New System.Drawing.Size(36, 31)
        Me.RemoveButton.TabIndex = 57
        Me.RemoveButton.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(8, 6)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(436, 180)
        Me.DataGridView1.TabIndex = 56
        '
        'AddButton
        '
        Me.AddButton.Image = Global.DT5550ControlCenter.My.Resources.Resources.add_icon
        Me.AddButton.Location = New System.Drawing.Point(477, 18)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(36, 32)
        Me.AddButton.TabIndex = 54
        Me.AddButton.Text = " "
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'Connect_R5560
        '
        Me.Connect_R5560.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Connect_R5560.Location = New System.Drawing.Point(450, 154)
        Me.Connect_R5560.Name = "Connect_R5560"
        Me.Connect_R5560.Size = New System.Drawing.Size(91, 32)
        Me.Connect_R5560.TabIndex = 27
        Me.Connect_R5560.Text = "Connect"
        Me.Connect_R5560.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.Button2)
        Me.TabPage3.Controls.Add(Me.Label5)
        Me.TabPage3.Controls.Add(Me.DeviceListSciDK)
        Me.TabPage3.Controls.Add(Me.ConnectSciDK)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(550, 192)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "SCIDK"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(268, 27)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 21)
        Me.Button2.TabIndex = 44
        Me.Button2.Text = "Refresh"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(21, 31)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 13)
        Me.Label5.TabIndex = 43
        Me.Label5.Text = "Serial Number"
        '
        'DeviceListSciDK
        '
        Me.DeviceListSciDK.BackColor = System.Drawing.Color.White
        Me.DeviceListSciDK.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DeviceListSciDK.FormattingEnabled = True
        Me.DeviceListSciDK.Location = New System.Drawing.Point(110, 27)
        Me.DeviceListSciDK.Name = "DeviceListSciDK"
        Me.DeviceListSciDK.Size = New System.Drawing.Size(121, 21)
        Me.DeviceListSciDK.TabIndex = 42
        '
        'ConnectSciDK
        '
        Me.ConnectSciDK.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ConnectSciDK.Location = New System.Drawing.Point(439, 144)
        Me.ConnectSciDK.Name = "ConnectSciDK"
        Me.ConnectSciDK.Size = New System.Drawing.Size(91, 32)
        Me.ConnectSciDK.TabIndex = 39
        Me.ConnectSciDK.Text = "Connect"
        Me.ConnectSciDK.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.Remove_DT5560SE)
        Me.TabPage4.Controls.Add(Me.DataGridView2)
        Me.TabPage4.Controls.Add(Me.Add_DT5560SE)
        Me.TabPage4.Controls.Add(Me.Connect_DT5560SE)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(550, 192)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "DT5560SE"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'Remove_DT5560SE
        '
        Me.Remove_DT5560SE.Image = Global.DT5550ControlCenter.My.Resources.Resources.remove2
        Me.Remove_DT5560SE.Location = New System.Drawing.Point(478, 73)
        Me.Remove_DT5560SE.Name = "Remove_DT5560SE"
        Me.Remove_DT5560SE.Size = New System.Drawing.Size(36, 31)
        Me.Remove_DT5560SE.TabIndex = 61
        Me.Remove_DT5560SE.UseVisualStyleBackColor = True
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AllowUserToResizeColumns = False
        Me.DataGridView2.AllowUserToResizeRows = False
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView2.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(9, 6)
        Me.DataGridView2.MultiSelect = False
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(436, 180)
        Me.DataGridView2.TabIndex = 60
        '
        'Add_DT5560SE
        '
        Me.Add_DT5560SE.Image = Global.DT5550ControlCenter.My.Resources.Resources.add_icon
        Me.Add_DT5560SE.Location = New System.Drawing.Point(478, 18)
        Me.Add_DT5560SE.Name = "Add_DT5560SE"
        Me.Add_DT5560SE.Size = New System.Drawing.Size(36, 32)
        Me.Add_DT5560SE.TabIndex = 59
        Me.Add_DT5560SE.Text = " "
        Me.Add_DT5560SE.UseVisualStyleBackColor = True
        '
        'Connect_DT5560SE
        '
        Me.Connect_DT5560SE.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Connect_DT5560SE.Location = New System.Drawing.Point(451, 154)
        Me.Connect_DT5560SE.Name = "Connect_DT5560SE"
        Me.Connect_DT5560SE.Size = New System.Drawing.Size(91, 32)
        Me.Connect_DT5560SE.TabIndex = 58
        Me.Connect_DT5560SE.Text = "Connect"
        Me.Connect_DT5560SE.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.Remove_R5560SE)
        Me.TabPage5.Controls.Add(Me.DataGridView3)
        Me.TabPage5.Controls.Add(Me.Add_R5560SE)
        Me.TabPage5.Controls.Add(Me.Connect_R5560SE)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(550, 192)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "R5560SE"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'Remove_R5560SE
        '
        Me.Remove_R5560SE.Image = Global.DT5550ControlCenter.My.Resources.Resources.remove2
        Me.Remove_R5560SE.Location = New System.Drawing.Point(478, 73)
        Me.Remove_R5560SE.Name = "Remove_R5560SE"
        Me.Remove_R5560SE.Size = New System.Drawing.Size(36, 31)
        Me.Remove_R5560SE.TabIndex = 65
        Me.Remove_R5560SE.UseVisualStyleBackColor = True
        '
        'DataGridView3
        '
        Me.DataGridView3.AllowUserToAddRows = False
        Me.DataGridView3.AllowUserToDeleteRows = False
        Me.DataGridView3.AllowUserToResizeColumns = False
        Me.DataGridView3.AllowUserToResizeRows = False
        Me.DataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView3.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView3.Location = New System.Drawing.Point(9, 6)
        Me.DataGridView3.MultiSelect = False
        Me.DataGridView3.Name = "DataGridView3"
        Me.DataGridView3.Size = New System.Drawing.Size(436, 180)
        Me.DataGridView3.TabIndex = 64
        '
        'Add_R5560SE
        '
        Me.Add_R5560SE.Image = Global.DT5550ControlCenter.My.Resources.Resources.add_icon
        Me.Add_R5560SE.Location = New System.Drawing.Point(478, 18)
        Me.Add_R5560SE.Name = "Add_R5560SE"
        Me.Add_R5560SE.Size = New System.Drawing.Size(36, 32)
        Me.Add_R5560SE.TabIndex = 63
        Me.Add_R5560SE.Text = " "
        Me.Add_R5560SE.UseVisualStyleBackColor = True
        '
        'Connect_R5560SE
        '
        Me.Connect_R5560SE.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Connect_R5560SE.Location = New System.Drawing.Point(451, 154)
        Me.Connect_R5560SE.Name = "Connect_R5560SE"
        Me.Connect_R5560SE.Size = New System.Drawing.Size(91, 32)
        Me.Connect_R5560SE.TabIndex = 62
        Me.Connect_R5560SE.Text = "Connect"
        Me.Connect_R5560SE.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(-2, -1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(558, 110)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 24
        Me.PictureBox1.TabStop = False
        '
        'Connection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(555, 381)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.sW)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Connection"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Connection"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage5.ResumeLayout(False)
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label3 As Label
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents sW As Label
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents Button1 As Button
    Friend WithEvents LabelDeviceList As Label
    Friend WithEvents DeviceList As ComboBox
    Friend WithEvents LabelIP As Label
    Friend WithEvents IP As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Firmware_selection As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Connection_selection As ComboBox
    Friend WithEvents JsonFilePath As TextBox
    Friend WithEvents Browse As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Connect As Button
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Connect_R5560 As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents AddButton As Button
    Friend WithEvents RemoveButton As Button
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents Button2 As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents DeviceListSciDK As ComboBox
    Friend WithEvents ConnectSciDK As Button
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents Remove_DT5560SE As Button
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents Add_DT5560SE As Button
    Friend WithEvents Connect_DT5560SE As Button
    Friend WithEvents TabPage5 As TabPage
    Friend WithEvents Remove_R5560SE As Button
    Friend WithEvents DataGridView3 As DataGridView
    Friend WithEvents Add_R5560SE As Button
    Friend WithEvents Connect_R5560SE As Button
End Class
