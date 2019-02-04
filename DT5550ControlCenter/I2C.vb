'Module I2C


'    Public Sub I2CDACWrite(chip_address As Integer, internal_address As Integer, value As Integer)



'        MainForm.board1.FPGAWriteReg((0 << 15), &H704)
'        MainForm.board1.FPGAWriteReg((1 << 15), &H704)
'        MainForm.board1.FPGAWriteReg((0 << 15), &H704)



'        MainForm.board1.FPGAWriteReg((1 << 8), &H704)

'        MainForm.board1.FPGAWriteReg((chip_address << 1) + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(5)
'        MainForm.board1.FPGAWriteReg(internal_address + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(5)
'        MainForm.board1.FPGAWriteReg(((value >> 8) And &H3F) + (3 << 6) + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(5)
'        MainForm.board1.FPGAWriteReg(((value >> 0) And &HFF) + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(5)
'        MainForm.board1.FPGAWriteReg((1 << 9), &H704)
'        System.Threading.Thread.Sleep(5)


'    End Sub

'    Public Sub I2CDACCFG(chip_address As Integer)
'        MainForm.board1.FPGAWriteReg((0 << 15), &H704)
'        MainForm.board1.FPGAWriteReg((1 << 15), &H704)
'        MainForm.board1.FPGAWriteReg((0 << 15), &H704)



'        MainForm.board1.FPGAWriteReg((1 << 8), &H704)

'        MainForm.board1.FPGAWriteReg((chip_address << 1) + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(5)
'        MainForm.board1.FPGAWriteReg(&HC + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(5)
'        MainForm.board1.FPGAWriteReg(&H3F + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(5)
'        MainForm.board1.FPGAWriteReg(0 + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(5)
'        MainForm.board1.FPGAWriteReg((1 << 9), &H704)
'        System.Threading.Thread.Sleep(5)


'    End Sub


'    Public Sub I2CTempRead(chip_address As Integer, ByRef value As Double)
'        Dim VALUE1, VALUE2 As UInteger

'        '  I2CEEpromWrite(&H48, 1, (12 - 9) << 5)
'        ' I2CEEpromWrite(&H48, 0, 0)
'        MainForm.board1.FPGAWriteReg((0 << 15), &H704)
'        MainForm.board1.FPGAWriteReg((1 << 15), &H704)
'        MainForm.board1.FPGAWriteReg((0 << 15), &H704)


'        MainForm.board1.FPGAWriteReg((1 << 8), &H704)

'        MainForm.board1.FPGAWriteReg((chip_address << 1) + 0 + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(1)
'        MainForm.board1.FPGAWriteReg(0 + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(1)
'        MainForm.board1.FPGAWriteReg((1 << 8), &H704)
'        MainForm.board1.FPGAWriteReg((chip_address << 1) + 1 + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(1)
'        MainForm.board1.FPGAWriteReg((1 << 12), &H704)
'        MainForm.board1.FPGAWriteReg((1 << 10) + (1 << 12), &H704)
'        System.Threading.Thread.Sleep(1)
'        VALUE1 = MainForm.board1.FPGAReadReg(&H704) And &HFF

'        MainForm.board1.FPGAWriteReg((1 << 10) + (1 << 12), &H704)
'        System.Threading.Thread.Sleep(1)
'        VALUE2 = MainForm.board1.FPGAReadReg(&H704) And &HFF

'        MainForm.board1.FPGAWriteReg((1 << 9), &H704)
'        System.Threading.Thread.Sleep(1)

'        value = ((VALUE1 << 4) + (VALUE2 >> 4)) / 16.0


'    End Sub

'    Public Sub I2CEEpromWrite(chip_address As Integer, internal_address As Integer, value As Integer)
'        MainForm.board1.FPGAWriteReg((0 << 15), &H704)
'        MainForm.board1.FPGAWriteReg((1 << 15), &H704)
'        MainForm.board1.FPGAWriteReg((0 << 15), &H704)


'        MainForm.board1.FPGAWriteReg((1 << 8), &H704)
'        MainForm.board1.FPGAWriteReg((chip_address << 1) + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(5)
'        MainForm.board1.FPGAWriteReg(internal_address + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(5)
'        MainForm.board1.FPGAWriteReg(value + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(5)
'        MainForm.board1.FPGAWriteReg((1 << 9), &H704)
'        System.Threading.Thread.Sleep(10)
'    End Sub

'    Public Sub I2CEEpromRead(chip_address As Integer, internal_address As Integer, ByRef value As Integer)
'        MainForm.board1.FPGAWriteReg((0 << 15), &H704)
'        MainForm.board1.FPGAWriteReg((1 << 15), &H704)
'        MainForm.board1.FPGAWriteReg((0 << 15), &H704)


'        MainForm.board1.FPGAWriteReg((1 << 8), &H704)

'        MainForm.board1.FPGAWriteReg((chip_address << 1) + 0 + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(1)
'        MainForm.board1.FPGAWriteReg(internal_address + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(1)
'        MainForm.board1.FPGAWriteReg((1 << 8), &H704)
'        MainForm.board1.FPGAWriteReg((chip_address << 1) + 1 + (1 << 11), &H704)
'        System.Threading.Thread.Sleep(1)
'        MainForm.board1.FPGAWriteReg((1 << 10), &H704)
'        System.Threading.Thread.Sleep(1)
'        MainForm.board1.FPGAWriteReg((1 << 9), &H704)
'        System.Threading.Thread.Sleep(1)
'        value = MainForm.board1.FPGAReadReg(&H704) And &HFF
'    End Sub

'    Public Function GetEEPROMString(eeprom_address As Integer, base_address As Integer, maxlen As Integer) As String
'        Dim tmp As String
'        For i = 0 To maxlen - 1
'            Dim valore As Integer = 0
'            I2CEEpromRead(eeprom_address, base_address + i, valore)
'            If valore = 0 Then
'                Exit For
'            End If
'            tmp = tmp & Chr(valore)
'        Next
'        Return tmp
'    End Function


'    Public Function ReadAFEBoardParametersI2C(ByRef SN_AFE As String, ByRef fitBias_min As Double, ByRef fitBias_max As Double) As Boolean
'        Dim tmp
'        Dim bias As Double = 0
'        Dim tmp32 As Integer
'        I2CEEpromRead(&H50, &H10, tmp)
'        tmp32 = (tmp And &HFF) << 8
'        I2CEEpromRead(&H50, &H11, tmp)
'        fitBias_min = (tmp32 + (tmp And &HFF)) / 100

'        I2CEEpromRead(&H50, &H12, tmp)
'        tmp32 = (tmp And &HFF) << 8
'        I2CEEpromRead(&H50, &H13, tmp)
'        fitBias_max = (tmp32 + (tmp And &HFF)) / 100


'        SN_AFE = GetEEPROMString(&H50, 0, 8)

'        If SN_AFE.StartsWith("NI07") Then
'            Return True
'        Else
'            SN_AFE = "none"
'            fitBias_min = 51
'            fitBias_max = 60
'            Return False
'        End If
'    End Function


'    Public Function ReadDETECTORBoardParametersI2C(ByRef DETECTOR_SN As String, ByRef MODELLO_SENSORE As String, ByRef SN_SENSORE As String, ByRef bias As Double) As Boolean
'        Dim tmp
'        DETECTOR_SN = GetEEPROMString(&H52, 0, 8)
'        MODELLO_SENSORE = GetEEPROMString(&H52, 8, 16)
'        SN_SENSORE = GetEEPROMString(&H52, 64, 16) & " - " & GetEEPROMString(&H52, 32, 16)

'        Dim tmp32 As Integer
'        I2CEEpromRead(&H52, 128, tmp)
'        tmp32 = (tmp And &HFF) << 8
'        I2CEEpromRead(&H52, 129, tmp)
'        bias = (tmp32 + (tmp And &HFF)) / 10

'        If DETECTOR_SN.StartsWith("NI08") Then
'            Return True
'        Else
'            bias = 52
'            Return False
'        End If
'    End Function
'End Module
