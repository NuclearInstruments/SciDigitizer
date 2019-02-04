'Imports CyUSB
'Imports System.IO
'Imports System.Runtime.InteropServices

'Public Class FPGA_FX3
'    Dim fxdev As CyUSBDevice

'    Public usbDevices As USBDeviceList
'    Dim curOUTEndpt As CyUSBEndPoint
'    Dim curINEndpt As CyUSBEndPoint

'    Dim curCyUsbDev As CyUSBDevice
'    Dim curHidDev As CyHidDevice
'    Dim curHidReport As CyHidReport

'    Private dataCaption As String
'    Private scriptfile As String
'    Private playscriptfile As String
'    Private fname As String
'    Private Datastring As String
'    Shared file_bytes As Integer
'    Private bRecording As Boolean
'    Private file_buffer As Byte()

'    Private list As ArrayList
'    Private list1 As ArrayList

'    Private Xaction As TTransaction
'    Private stream As FileStream
'    Private sw As StreamWriter
'    Private script_stream As FileStream

'    Private Reqcode As Byte
'    Private wvalue As UShort
'    Private windex As UShort

'    Private Resetreg As UShort
'    Private Maxaddr As UShort
'    Private Sync_Form_Resize As Integer = 0
'    Private Max_Ctlxfer_size As Long
'    Dim __intbuffer As Byte() = New Byte(500000) {}
'    Public Function InitCy()
'        CyConst.SetClassGuid("{CDBF8987-75F1-468e-8217-97197F88F773}")
'        scriptfile = ""
'        playscriptfile = ""
'        Resetreg = &HE600
'        Maxaddr = &H4000
'        Max_Ctlxfer_size = &H1000
'        bRecording = False
'        Xaction = New TTransaction()
'        list = New ArrayList()
'        list1 = New ArrayList()

'        curOUTEndpt = Nothing
'        curINEndpt = Nothing
'        curCyUsbDev = Nothing
'        curHidDev = Nothing
'        curHidReport = Nothing
'        Dim DeviceMask As Byte = 1

'        Dim bufHandle1 As GCHandle = GCHandle.Alloc(__intbuffer, GCHandleType.Pinned)

'        usbDevices = New USBDeviceList(DeviceMask)
'        Return usbDevices
'    End Function


'    Public Function ResetCy()
'        curCyUsbDev.Reset()
'        System.Threading.Thread.Sleep(100)
'    End Function

'    Public Function SelectDeviceBySN(sn As String)
'        For Each dev As USBDevice In usbDevices
'            If dev.SerialNumber = sn Then
'                curCyUsbDev = dev
'                Return dev
'            End If
'        Next

'        Return Nothing
'    End Function


'    Public Sub SelectDeviceByIndex(index As Integer)

'        Try
'            curCyUsbDev = usbDevices(index)
'        Catch ex As Exception
'            MsgBox("Error. Mamba not found")
'            End
'        End Try



'    End Sub



'    Public Function MACAReadCounters(ByRef trigid As UInteger, ByRef packid As Double)
'        Dim buffer(10) As UInteger
'        If FPGAReadBurstUint(buffer, 3, &HF0000A0&) = False Then
'            Return False
'        End If
'        trigid = buffer(0)
'        packid = buffer(1)

'        Return True



'    End Function





'    Public Function MACAReadTrigger(ByRef trigA As UInteger, ByRef trigB As UInteger, ByRef masktrigA As UInteger, ByRef masktrigB As UInteger, ByRef globalTrigInfo As UInteger)
'        Dim buffer(10) As UInteger
'        If FPGAReadBurstUint(buffer, 3, &H23&) = False Then
'            Return False
'        End If
'        trigA = buffer(0) And &HFFFF
'        trigB = (buffer(0) >> 16) And &HFFFF

'        masktrigA = buffer(1) And &HFFFF
'        masktrigB = (buffer(1) >> 16) And &HFFFF

'        globalTrigInfo = buffer(2)
'        Return True



'    End Function


'    Public Function MACACheckFirmware(ByRef hwrel As Double, ByRef fwrel As Double)

'        curCyUsbDev.Reset()

'        Dim buffer(10) As UInteger
'        If FPGAReadBurstUint(buffer, 3, &HF000000&) = False Then
'            Return False
'        End If

'        If buffer(0) = &H5550600D& Then
'            hwrel = buffer(2)
'            fwrel = (buffer(1) >> 16) + (buffer(1) And &HFFFF) / 100
'            Return True
'        Else
'            Return False
'        End If


'    End Function


'    Public Sub MACAInitializeBoard(demoadc As Boolean)

'        'REGISTRI CDCDE62005'
'        'FPGAclass.FPGAWriteReg(Convert.ToInt64(TextBox2.Text, 16), &HF0000001&) 'REG0'
'        'FPGAclass.FPGAWriteReg(Convert.ToInt64(TextBox3.Text, 16), &HF0000002&) 'REG1'
'        'Do
'        '    FPGAclass.FPGAWriteReg(1, &HF0000003&) 'STROBE'
'        '    FPGAclass.FPGAWriteReg(0, &HF0000003&) 'STROBE'
'        '    System.Threading.Thread.Sleep(1000)
'        'Loop
'        FPGAWriteReg((23 << 8) + 30, 1)


'        'FPGAWriteReg(&H468211, 2)
'        FPGAWriteReg(&H468401, 2)
'        FPGAWriteReg(15, 3)
'        System.Threading.Thread.Sleep(10)

'        FPGAWriteReg(&H420000 + (0 << 5), 2)
'        FPGAWriteReg(15, 3)
'        System.Threading.Thread.Sleep(10)


'        FPGAWriteReg(&H2880FF, 2)
'        FPGAWriteReg(15, 3)
'        System.Threading.Thread.Sleep(10)





'        'FPGAclass.FPGAWriteReg(&H250000, 2)         'RUN STANDARD


'        'FPGAWriteReg(&H25000F, 2)        'RUN DEMO
'        If demoadc = True Then
'            FPGAWriteReg(&H25004F, 2)        'RUN DEMO (rampa)
'            'FPGAWriteReg(&H250012, 2)        'RUN DEMO (rampa)
'        Else
'            FPGAWriteReg(&H250000, 2)
'        End If
'        'FPGAclass.FPGAWriteReg(&HF0200, 2)
'        FPGAWriteReg(15, 3)
'        FPGAWriteReg(&H260010, 2)
'        FPGAWriteReg(15, 3)
'        FPGAWriteReg(&H270020, 2)
'        FPGAWriteReg(15, 3)



'    End Sub


'    Public Function CapturePackets(ByVal npackets As Integer, ByVal nX As Integer, ByVal nY As Integer, ByRef buffer() As UInteger) As Integer

'        Dim transferSIZE = (npackets + 2) * (nX / 2 * nY + 4)
'        ' Dim buffer As UInt32() = New UInt32(transferSIZE) {}
'        Dim sm As Integer = 0
'        Dim qq, s As Integer
'        Dim sn As Integer
'        Dim cnt As Integer
'        Dim start = Now
'        Dim repetition = 1

'        Dim contatoreRetry = 10

'        MACASoftwareInib(False)
'        FPGAWriteReg(npackets, &H13)
'        FPGAWriteReg(0, &H14)

'        FPGAWriteReg(1, &HC)
'        '        System.Threading.Thread.Sleep(5)
'        FPGAWriteReg(0, &HC)


'        FPGAWriteReg(1, &H14)

'riprova:


'        'For i = 0 To repetition - 1
'        If FPGAReadBurstUint(buffer, transferSIZE, &HF0000000&) = True Then
'            Return transferSIZE
'        Else
'            Return 0
'            '    If contatoreRetry > 0 Then
'            'contatoreRetry -= 1
'            'GoTo riprova
'        End If

'        'Return False
'        '
'        '    End If
'        'Next


'        Return True
'    End Function

'    Public Function MACASetAcq(ByVal pre, ByVal length) As Boolean
'        Dim post As UInteger
'        Dim value As UInteger

'        post = pre + length
'        value = post + (pre << 16)
'        FPGAWriteReg(value, &HE)



'    End Function

'    Public Function MACASetTrigMode(ByVal trigmode As Integer, emu_rate As Integer) As Boolean
'        Dim post As UInteger
'        Dim value1 As UInteger = 0
'        Dim value2 As UInteger = 0


'        Select Case (trigmode)
'            Case 0
'                value1 = 7
'            Case 1
'                value1 = 0
'                'Case 2
'                '    value1 = 3
'                'Case 4
'                value2 = &H80000000& + (20000000 / emu_rate)

'            Case 2
'                value1 = 0
'        End Select

'        FPGAWriteReg(value1, &H5)
'        FPGAWriteReg(value2, &H6)



'    End Function

'    Public Function MACASetTrigMask(ByVal ASIC0 As UInteger, ASIC1 As UInteger) As Boolean
'        Return FPGAWriteReg((ASIC1 << 16) + (ASIC0), &H7)

'    End Function


'    Public Function MACAEnableTriggerFlag(ByVal Enable As Boolean) As Boolean
'        If Enable = True Then
'            Return FPGAWriteReg(1, &H8)
'        Else
'            Return FPGAWriteReg(0, &H8)
'        End If
'    End Function


'    Public Sub MACAFlushFifo()
'        FPGAWriteReg(0, &HC)
'        FPGAWriteReg(1, &HC)
'        FPGAWriteReg(0, &HC)
'    End Sub

'    Public Sub MACAResetCounter()
'        FPGAWriteReg(0, &H10)
'        FPGAWriteReg(1, &H10)
'        FPGAWriteReg(0, &H10)
'    End Sub

'    Public Function MACASoftwareInib(ByVal Enable As Boolean) As Boolean
'        If Enable = True Then
'            Return FPGAWriteReg(0, &H11)
'        Else
'            Return FPGAWriteReg(1, &H11)
'        End If
'    End Function

'    Public Function MACARunStop(ByVal Enable As Boolean) As Boolean
'        If Enable = True Then
'            Return FPGAWriteReg(1, &H14)
'        Else
'            Return FPGAWriteReg(0, &H14)
'        End If
'    End Function

'    Public Function MACASetEOCLength(ByVal EOCL As UInteger) As Boolean
'        Return FPGAWriteReg(EOCL, &HF)

'    End Function
'    Public Function MACASetEOCPolarity(ByVal EOCL As UInteger) As Boolean
'        Return FPGAWriteReg(EOCL, &H16)

'    End Function

'    Public Function MACAmaskadc(ByVal adc0 As UInteger, ByVal adc1 As UInteger, ByVal adc2 As UInteger, ByVal adc3 As UInteger) As Boolean
'        Return FPGAWriteReg((adc0 << 3) + (adc1 << 2) + (adc2 << 1) + (adc3 << 0), &H20)

'    End Function

'    Public Function FPGAWriteReg(val As UInt32, ByVal address As UInt32)

'        Dim transferSIZE = 1 ' 1048576 * 4
'        Dim buffer As UInt32() = New UInt32(16) {}

'        buffer(0) = val

'        Return FPGAWriteBurstUInt(buffer, transferSIZE, address)

'    End Function

'    Public Function FPGAReadReg(ByVal address As UInt32)
'        Dim transferSIZE = 1 ' 1048576 * 4
'        Dim buffer As UInt32() = New UInt32(transferSIZE) {}
'        If FPGAReadBurstUint(buffer, 1, address) = False Then
'            Return Nothing
'        Else
'            Return buffer(0)
'        End If
'    End Function


'    'Public Function FPGAWriteBurstUInt(ByRef Inbuffer As UInt32(), ByVal size_in_word As UInteger, ByVal start_address As UInt32)
'    '    Dim bXferCompleted As Boolean = False
'    '    curOUTEndpt = curCyUsbDev.BulkOutEndPt

'    '    Dim size_in_byte As UInteger
'    '    Dim remaining As UInteger
'    '    Dim current_transfer_len As UInteger
'    '    Dim current_word As UInteger
'    '    Dim maxTransferSize As UInteger


'    '    If curCyUsbDev.bSuperSpeed = True Then

'    '        maxTransferSize = 1 * 1024576 - 8
'    '    Else

'    '        maxTransferSize = 4 * 1024576 - 8
'    '    End If

'    '    curOUTEndpt.Reset()
'    '    curINEndpt.Reset()


'    '    remaining = size_in_word
'    '    current_word = 0
'    '    ReDim __intbuffer(size_in_word * 4 + 70000)
'    '    While remaining > 0

'    '        If remaining * 4 > maxTransferSize Then
'    '            current_transfer_len = maxTransferSize / 4
'    '        Else
'    '            current_transfer_len = remaining
'    '        End If

'    '        size_in_byte = current_transfer_len * 4
'    '        Dim sizeARR As Byte() = BitConverter.GetBytes(CUInt(Math.Ceiling(size_in_byte / 4)))
'    '        Dim destAddress As Byte() = BitConverter.GetBytes(start_address + current_word)
'    '        __intbuffer(3) = &HAB
'    '        __intbuffer(2) = &HBA
'    '        __intbuffer(1) = &HFF
'    '        __intbuffer(0) = &HF1
'    '        __intbuffer(7) = destAddress(3)
'    '        __intbuffer(6) = destAddress(2)
'    '        __intbuffer(5) = destAddress(1)
'    '        __intbuffer(4) = destAddress(0)
'    '        __intbuffer(11) = sizeARR(3)
'    '        __intbuffer(10) = sizeARR(2)
'    '        __intbuffer(9) = sizeARR(1)
'    '        __intbuffer(8) = sizeARR(0)


'    '        Buffer.BlockCopy(Inbuffer, current_word * 4, __intbuffer, 12, size_in_byte)

'    '        curOUTEndpt.TimeOut = 20000
'    '        bXferCompleted = curOUTEndpt.XferData(__intbuffer, size_in_byte + 12, False)
'    '        If bXferCompleted = False Then
'    '            Return False
'    '        End If

'    '        current_word = current_word + current_transfer_len
'    '        remaining = remaining - current_transfer_len
'    '    End While


'    '    Return True
'    'End Function

'    'Public Function FPGAReadBurstUint(ByRef Inbuffer As UInt32(), ByVal size_in_word As UInt32, ByVal start_address As UInt32)
'    '    Dim bXferCompleted As Boolean = False
'    '    Dim size_input_buffer As UInteger
'    '    Dim roundto As Integer
'    '    Dim maxTransferSize As UInteger

'    '    Dim size_in_byte As UInteger
'    '    Dim remaining As UInteger
'    '    Dim current_transfer_len As UInteger
'    '    Dim current_word As UInteger
'    '    Dim ctr As Integer
'    '    Dim tryt As UInteger
'    '    Dim iofs As UInteger = 4
'    '    curOUTEndpt = curCyUsbDev.BulkOutEndPt
'    '    curINEndpt = curCyUsbDev.BulkInEndPt




'    '    If curCyUsbDev.bSuperSpeed = True Then
'    '        roundto = 16384
'    '        maxTransferSize = 1024576 * 160
'    '    Else
'    '        start_address = start_address - 1
'    '        roundto = 512
'    '        maxTransferSize = 1024576 * 160
'    '    End If


'    '    curOUTEndpt.Reset()
'    '    curINEndpt.Reset()

'    '    curINEndpt.TimeOut = 5000
'    '    'hack
'    '    'size_in_word = size_in_word + 1
'    '    Dim original_send As Integer
'    '    '     If size_in_word * 4 > maxTransferSize Then
'    '    remaining = size_in_word
'    '    current_word = 0

'    '    ReDim __intbuffer(size_in_word * 4 + 70000)
'    '    While remaining > 0
'    '        If remaining * 4 > maxTransferSize Then
'    '            current_transfer_len = maxTransferSize / 4
'    '        Else
'    '            current_transfer_len = remaining
'    '        End If



'    '        size_in_byte = current_transfer_len * 4
'    '        size_input_buffer = Math.Ceiling((size_in_byte + 8) / roundto) * roundto
'    '        Dim sizeARR As Byte() = BitConverter.GetBytes(CUInt(Math.Ceiling(size_in_byte / 4))) '+ 2)
'    '        Dim packet As Byte() = New Byte(12) {}
'    '        Dim destAddress As Byte() = BitConverter.GetBytes(start_address + current_word)


'    '        packet(3) = &HAB
'    '        packet(2) = &HBA
'    '        packet(1) = &HFF
'    '        packet(0) = &HF0
'    '        packet(7) = destAddress(3)
'    '        packet(6) = destAddress(2)
'    '        packet(5) = destAddress(1)
'    '        packet(4) = destAddress(0)
'    '        packet(11) = sizeARR(3)
'    '        packet(10) = sizeARR(2)
'    '        packet(9) = sizeARR(1)
'    '        packet(8) = sizeARR(0)


'    '        curOUTEndpt.TimeOut = 2000
'    '        bXferCompleted = curOUTEndpt.XferData(packet, 12, False)

'    '        If bXferCompleted = True Then
'    '            curINEndpt = curCyUsbDev.BulkInEndPt
'    '            curINEndpt.TimeOut = 10000

'    '            original_send = size_input_buffer
'    '            ctr = size_in_byte
'    '            tryt = 3


'    '            curINEndpt.Reset()


'    '            '  While ((ctr > 0) And (tryt > 0))
'    '            'size_input_buffer = ctr
'    '            Do
'    '                bXferCompleted = curINEndpt.XferData(__intbuffer, size_input_buffer, True)
'    '                If bXferCompleted = False Then
'    '                    curINEndpt.Reset()
'    '                End If
'    '                ctr = ctr - size_input_buffer
'    '            Loop Until size_input_buffer = 0 And bXferCompleted = True 'ctr <= 0 


'    '            ' If size_input_buffer = 0 Then
'    '            'tryt = tryt - 1
'    '            'End If
'    '            'ctr = ctr - size_input_buffer

'    '            ' End While



'    '            If bXferCompleted = True Then
'    '                'Buffer.BlockCopy(__intbuffer, 0, Inbuffer, current_word * 4, size_in_byte)
'    '                Buffer.BlockCopy(__intbuffer, iofs, Inbuffer, current_word * 4, size_in_byte)
'    '                iofs = 4
'    '                current_word = current_word + current_transfer_len
'    '                remaining = remaining - current_transfer_len
'    '            Else
'    '                MsgBox("errore " & curINEndpt.LastError)
'    '                Return False
'    '            End If

'    '        Else
'    '            Return False
'    '        End If
'    '    End While

'    '    Return True
'    '    '      End If


'    'End Function

'    Public Function FPGAWriteBurstUInt(ByRef Inbuffer As UInt32(), ByVal size_in_word As UInteger, ByVal start_address As UInt32)
'        Dim bXferCompleted As Boolean = False
'        curOUTEndpt = curCyUsbDev.BulkOutEndPt

'        Dim size_in_byte As UInteger
'        Dim remaining As UInteger
'        Dim current_transfer_len As UInteger
'        Dim current_word As UInteger
'        Dim maxTransferSize As UInteger


'        If curCyUsbDev.bSuperSpeed = True Then

'            maxTransferSize = 1 * 1024576 - 8
'        Else

'            maxTransferSize = 4 * 1024576 - 8
'        End If


'        remaining = size_in_word
'        current_word = 0
'        ' ReDim __intbuffer(size_in_word * 4 + 70000)
'        While remaining > 0

'            If remaining * 4 > maxTransferSize Then
'                current_transfer_len = maxTransferSize / 4
'            Else
'                current_transfer_len = remaining
'            End If

'            size_in_byte = current_transfer_len * 4
'            Dim sizeARR As Byte() = BitConverter.GetBytes(CUInt(Math.Ceiling(size_in_byte / 4)))
'            Dim destAddress As Byte() = BitConverter.GetBytes(start_address + current_word)
'            __intbuffer(3) = &HAB
'            __intbuffer(2) = &HBA
'            __intbuffer(1) = &HFF
'            __intbuffer(0) = &HF1
'            __intbuffer(7) = destAddress(3)
'            __intbuffer(6) = destAddress(2)
'            __intbuffer(5) = destAddress(1)
'            __intbuffer(4) = destAddress(0)
'            __intbuffer(11) = sizeARR(3)
'            __intbuffer(10) = sizeARR(2)
'            __intbuffer(9) = sizeARR(1)
'            __intbuffer(8) = sizeARR(0)


'            Buffer.BlockCopy(Inbuffer, current_word * 4, __intbuffer, 12, size_in_byte)

'            curOUTEndpt.TimeOut = 2000
'            bXferCompleted = curOUTEndpt.XferData(__intbuffer, size_in_byte + 12, False)
'            If bXferCompleted = False Then
'                Return False
'            End If

'            current_word = current_word + current_transfer_len
'            remaining = remaining - current_transfer_len
'        End While


'        Return True
'    End Function

'    Public Function FPGAReadBurstUint(ByRef Inbuffer As UInt32(), ByVal size_in_word As UInt32, ByVal start_address As UInt32)
'        Try


'            Dim bXferCompleted As Boolean = False
'            Dim size_input_buffer As UInteger
'            Dim roundto As Integer
'            Dim maxTransferSize As UInteger

'            Dim size_in_byte As UInteger
'            Dim remaining As UInteger
'            Dim current_transfer_len As UInteger
'            Dim current_word As UInteger
'            Dim ctr As Integer
'            Dim tryt As UInteger
'            Dim iofs As UInteger = 4
'            curOUTEndpt = curCyUsbDev.BulkOutEndPt
'            curINEndpt = curCyUsbDev.BulkInEndPt




'            If curCyUsbDev.bSuperSpeed = True Then
'                roundto = 16384
'                maxTransferSize = 1024576 * 160
'            Else
'                roundto = 8192
'                maxTransferSize = 1024576 * 160
'            End If

'            curINEndpt.TimeOut = 2000
'            'hack
'            'size_in_word = size_in_word + 1
'            Dim original_send As Integer
'            '     If size_in_word * 4 > maxTransferSize Then
'            remaining = size_in_word
'            current_word = 0

'            'ReDim __intbuffer(size_in_word * 4 + 70000)
'            '            Dim inData

'            While remaining > 0
'                If remaining * 4 > maxTransferSize Then
'                    current_transfer_len = maxTransferSize / 4
'                Else
'                    current_transfer_len = remaining
'                End If



'                size_in_byte = current_transfer_len * 4
'                size_input_buffer = Math.Ceiling((size_in_byte + 8) / roundto) * roundto
'                Dim sizeARR As Byte() = BitConverter.GetBytes(CUInt(Math.Ceiling(size_in_byte / 4)) + 2)
'                Dim packet As Byte() = New Byte(12) {}
'                Dim destAddress As Byte() = BitConverter.GetBytes(start_address + current_word)


'                packet(3) = &HAB
'                packet(2) = &HBA
'                packet(1) = &HFF
'                packet(0) = &HF0
'                packet(7) = destAddress(3)
'                packet(6) = destAddress(2)
'                packet(5) = destAddress(1)
'                packet(4) = destAddress(0)
'                packet(11) = sizeARR(3)
'                packet(10) = sizeARR(2)
'                packet(9) = sizeARR(1)
'                packet(8) = sizeARR(0)


'                curOUTEndpt.TimeOut = 2000
'                bXferCompleted = curOUTEndpt.XferData(packet, 12, False)

'                If bXferCompleted = True Then
'                    curINEndpt = curCyUsbDev.BulkInEndPt
'                    curINEndpt.TimeOut = 20000

'                    original_send = size_input_buffer
'                    ctr = size_in_byte
'                    tryt = 3


'                    curINEndpt.Reset()


'                    '  While ((ctr > 0) And (tryt > 0))
'                    'size_input_buffer = ctr
'                    Do
'                        bXferCompleted = curINEndpt.XferData(__intbuffer, size_input_buffer, True)
'                        If bXferCompleted = False Then
'                            curINEndpt.Reset()
'                        End If
'                        ctr = ctr - size_input_buffer
'                    Loop Until size_input_buffer = 0 And bXferCompleted = True 'ctr <= 0 


'                    ' If size_input_buffer = 0 Then
'                    'tryt = tryt - 1
'                    'End If
'                    'ctr = ctr - size_input_buffer

'                    ' End While



'                    If bXferCompleted = True Then
'                        'Buffer.BlockCopy(__intbuffer, 0, Inbuffer, current_word * 4, size_in_byte)
'                        Buffer.BlockCopy(__intbuffer, iofs, Inbuffer, current_word * 4, size_in_byte)
'                        iofs = 4
'                        current_word = current_word + current_transfer_len
'                        remaining = remaining - current_transfer_len
'                    Else
'                        MsgBox("errore " & curINEndpt.LastError)
'                        Return False
'                    End If

'                Else
'                    Return False
'                End If
'            End While

'            Return True
'            '      End If
'        Catch ex As Exception
'            Return False
'        End Try

'    End Function
'    Public Function Checkbootloader()
'        fxdev = curCyUsbDev
'        If fxdev Is Nothing Then
'            Return -1
'        End If

'        Dim fx As CyFX3Device = TryCast(fxdev, CyFX3Device)


'        ' check for bootloader first, if it is not running then prompt message to user.
'        If fx.IsBootLoaderRunning() Then
'            Return True
'        Else
'            Return False
'        End If
'    End Function
'    Public Function FirmWareProgramToRam(filename As String)
'        fxdev = curCyUsbDev
'        If fxdev Is Nothing Then
'            Return -1
'        End If

'        Dim fx As CyFX3Device = TryCast(fxdev, CyFX3Device)


'        ' check for bootloader first, if it is not running then prompt message to user.
'        If Not fx.IsBootLoaderRunning() Then
'            MessageBox.Show("Please reset your device to download firmware", "Bootloader is not running")
'            Return -2
'        End If




'        If (fxdev IsNot Nothing) AndAlso (File.Exists(filename)) Then
'            Dim enmResult As FX3_FWDWNLOAD_ERROR_CODE = FX3_FWDWNLOAD_ERROR_CODE.SUCCESS




'            If bRecording AndAlso (script_stream IsNot Nothing) Then
'                Dim ConfigNum As Byte = fx.Config
'                Dim IntfcNum As Byte = 0
'                Dim AltIntfc As Byte = fx.AltIntfc
'                fx.SetRecordingFlag(True, ConfigNum, IntfcNum, AltIntfc)
'            End If
'            If fx.IsRecordingFlagSet() Then
'                fx.ScriptFileForDwld(script_stream)
'            End If

'            enmResult = fx.DownloadFw(filename, FX3_FWDWNLOAD_MEDIA_TYPE.RAM)

'            fx.SetRecordingFlag(False, Xaction.ConfigNum, Xaction.IntfcNum, Xaction.AltIntfc)

'            Return 0

'        End If

'        Return -3

'    End Function
'End Class
