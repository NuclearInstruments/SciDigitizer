Public Class communication

    Public Enum tConnectionMode
        USB = 0
        ETHERNET = 1
        ETHERNET2 = 2
        VME = 3
    End Enum

    Public Enum tModel
        V2495 = 0
        DT5550 = 1
        R5560 = 2
    End Enum

    Public Enum tError
        OK = 0
        NOT_CONNECTED = 1
        ALREADY_CONNECTED = 2
        UNSUPPORTED_DEVICE = 3
        ALREADY_DISCONNECTED = 4
        ERROR_GENERIC = 5
        ERROR_INTERFACE = 6
        ERROR_FPGA = 7
        ERROR_TRANSFER_MAX_LENGTH = 8
        NO_DATA_AVAILABLE = 9
        TOO_MANY_DEVICES_CONNECTED = 10
        INVALID_HANDLE = 11
        INVALID_HARDWARE = 12
        DEVICE_NOT_FOUND = 13
        INVALID_PARAMETER = 14
        ERROR_EEPROM = 15
        TIMEOUT = 16
        BUSY = 17
        OPERATION_ABORTED = 18
        ERROR_IO = 19
        INSUFFICIENT_RESOURCES = 20
        WRITE_FAILED = 21
    End Enum

    Public _boardModel As tModel
    Private _isconnected As New List(Of Boolean)
    Public _nBoard As Integer
    Public _n_ch As Integer
    Public _n_oscilloscope As Integer
    Public _n_ch_oscilloscope As Integer
    Private V2495Handle As UInt32
    Private DT5550Handle As New IntPtr
    Private R5560Handle As New List(Of IntPtr)

    Dim mtx As New Threading.Mutex

    Public Sub New()
        Try
            mtx.ReleaseMutex()
        Catch ex As Exception

        End Try
    End Sub

    Public Function IsFileCompatible(model As String) As Boolean
        Dim modelcode As New tModel
        If model = "V2495" Then
            modelcode = tModel.V2495
        ElseIf model = "DT5550" Then
            modelcode = tModel.DT5550
        ElseIf model = "R5560" Then
            modelcode = tModel.R5560
        End If
        If modelcode = _boardModel Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub StartConnection(n As Integer, model As tModel)
        _isconnected.Clear()
        R5560Handle.Clear()

        For i = 0 To n - 1
            _isconnected.Add(False)
            If model = tModel.R5560 Then
                R5560Handle.Add(0)
            End If
        Next
    End Sub
    Public Function Connect(ConnectionMode As tConnectionMode, model As tModel, param0 As String, ind As Integer) As tError

        Select Case model
            'Case tModel.V2495
            '    V2495_Startup()
            '    Select Case ConnectionMode
            '        Case tConnectionMode.USB
            '            mtx.WaitOne()
            '            Dim ierror = V2495_AttachNewDevice(0, param0, 0, 0, V2495Handle)
            '            mtx.ReleaseMutex()
            '            Dim error_t = IErrorV2495ToNETError(ierror)
            '            If error_t = tError.OK Then
            '                _boardModel = model
            '                _isconnected = True
            '            Else
            '                _isconnected = False
            '            End If
            '            Return error_t
            'Case tConnectionMode.ETHERNET

            '            mtx.WaitOne()
            '            Dim ierror = V2495_AttachNewDevice(1, param0, 9764, 6234, V2495Handle)
            '            mtx.ReleaseMutex()
            '            Dim error_t = IErrorV2495ToNETError(ierror)
            '            If error_t = tError.OK Then
            '                _boardModel = model
            '                _isconnected = True
            '            Else
            '                _isconnected = False
            '            End If
            '            Return error_t
            '        Case tConnectionMode.VME

            '    End Select
            Case tModel.DT5550

                USB3_Init()
                Select Case ConnectionMode
                    Case tConnectionMode.USB
                        mtx.WaitOne()
                        Dim ierror = USB3_ConnectDevice(param0, DT5550Handle)
                        mtx.ReleaseMutex()
                        Dim error_t = IErrorDT5550ToNETError(ierror)
                        If error_t = tError.OK Then
                            _boardModel = model
                            _isconnected(ind) = True
                        Else
                            _isconnected(ind) = False
                        End If
                        Return error_t
                    Case tConnectionMode.ETHERNET
                        Return tError.ERROR_GENERIC
                    Case tConnectionMode.ETHERNET2
                        Return tError.ERROR_GENERIC
                    Case tConnectionMode.VME
                        Return tError.ERROR_GENERIC
                End Select
            Case tModel.R5560

                Select Case ConnectionMode
                    Case tConnectionMode.USB
                    Case tConnectionMode.ETHERNET
                        Return tError.ERROR_GENERIC
                    Case tConnectionMode.ETHERNET2

                        R5560_Init()
                        mtx.WaitOne()
                        Dim R5560_Handle As IntPtr
                        Dim ierror = R5560_ConnectDevice(param0, 8888, R5560_Handle)
                        mtx.ReleaseMutex()
                        Dim error_t = IErrorDT5550ToNETError(ierror)
                        If error_t = tError.OK Then
                            R5560Handle(ind) = R5560_Handle
                            _boardModel = model
                            _isconnected(ind) = True
                        Else
                            _isconnected(ind) = False
                        End If
                        Return error_t

                    Case tConnectionMode.VME
                        Return tError.ERROR_GENERIC
                End Select
            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select
        Return tError.ERROR_GENERIC
    End Function

    Public Function Disconnect() As tError
        If _isconnected.Count = 0 Then
            Return tError.ALREADY_DISCONNECTED
        End If
        Select Case _boardModel
            'Case tModel.V2495
            '    mtx.WaitOne()
            '    Dim ierror = V2495_DeleteDevice(V2495Handle)
            '    mtx.ReleaseMutex()
            '    _isconnected = False
            '    Return IErrorV2495ToNETError(ierror)
            Case tModel.DT5550
                If _isconnected(0) = False Then
                    Return tError.ALREADY_DISCONNECTED
                End If
                mtx.WaitOne()
                Dim ierror = USB3_CloseConnection(DT5550Handle)
                If ierror = 0 Then
                    _isconnected(0) = False
                    Return tError.OK
                Else
                    Return tError.ALREADY_DISCONNECTED
                End If
            Case tModel.R5560
                mtx.WaitOne()
                For r = 0 To R5560Handle.Count - 1
                    R5560_CloseConnection(R5560Handle(r))
                    _isconnected(r) = False
                Next

                Return tError.OK
            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select
    End Function

    Public Function ListDevices(ConnectionMode As tConnectionMode, model As tModel, ByRef ListOfDevice As String, ByRef DeviceCount As Integer) As tError

        Select Case model
            Case tModel.V2495

            Case tModel.DT5550

                USB3_Init()
                Select Case ConnectionMode
                    Case tConnectionMode.USB
                        mtx.WaitOne()
                        Dim ierror = USB3_ListDevices(ListOfDevice, DeviceCount)
                        mtx.ReleaseMutex()
                        Dim error_t = IErrorDT5550ToNETError(ierror)
                        Return error_t
                    Case tConnectionMode.ETHERNET


                    Case tConnectionMode.VME
                End Select
            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select
        Return tError.ERROR_GENERIC
    End Function

    Public Function IErrorDT5550ToNETError(ierror As ULong) As tError

        Select Case ierror
            Case 0
                Return tError.OK
            Case 1
                Return tError.INVALID_HANDLE
            Case 2
                Return tError.DEVICE_NOT_FOUND
            Case 3
                Return tError.NOT_CONNECTED
            Case 4
                Return tError.ERROR_IO
            Case 5
                Return tError.INSUFFICIENT_RESOURCES
            Case 6
                Return tError.INVALID_PARAMETER
            Case 7
                Return tError.INVALID_PARAMETER
            Case 8
                Return tError.WRITE_FAILED
            Case 9
                Return tError.WRITE_FAILED
            Case 10
                Return tError.WRITE_FAILED
            Case 11
                Return tError.ERROR_EEPROM
            Case 12
                Return tError.ERROR_EEPROM
            Case 13
                Return tError.ERROR_EEPROM
            Case 14
                Return tError.ERROR_EEPROM
            Case 15
                Return tError.ERROR_EEPROM
            Case 16
                Return tError.INVALID_PARAMETER
            Case 17
                Return tError.INVALID_HARDWARE
            Case 18
                Return tError.ERROR_GENERIC
            Case 19
                Return tError.TIMEOUT
            Case 20
                Return tError.OPERATION_ABORTED
            Case 21
                Return tError.ERROR_GENERIC
            Case 22
                Return tError.INVALID_PARAMETER
            Case 23
                Return tError.INVALID_PARAMETER
            Case 24
                Return tError.ERROR_IO
            Case 25
                Return tError.ERROR_IO
            Case 26
                Return tError.ERROR_GENERIC
            Case 27
                Return tError.BUSY
            Case 28
                Return tError.INSUFFICIENT_RESOURCES
            Case 29
                Return tError.DEVICE_NOT_FOUND
            Case 30
                Return tError.NOT_CONNECTED
            Case 31
                Return tError.DEVICE_NOT_FOUND
            Case 32
                Return tError.ERROR_GENERIC
        End Select
        Return tError.ERROR_GENERIC
    End Function

    Public Sub GetMessage(errorcode As tError)
        If errorcode = 1 Then
            MsgBox("The device is not connected!", vbCritical + vbOKOnly)
        ElseIf errorcode = 2 Then
            MsgBox("The device is already connected!", vbCritical + vbOKOnly)
        ElseIf errorcode = 3 Then
            MsgBox("The device is not supported!", vbCritical + vbOKOnly)
        ElseIf errorcode = 4 Then
            MsgBox("The device is already disconnected!", vbCritical + vbOKOnly)
        ElseIf errorcode = 5 Then
            MsgBox("Error!", vbCritical + vbOKOnly)
        ElseIf errorcode = 6 Then
            MsgBox("Interface error!", vbCritical + vbOKOnly)
        ElseIf errorcode = 7 Then
            MsgBox("FPGA error!", vbCritical + vbOKOnly)
        ElseIf errorcode = 8 Then
            MsgBox("Exceeding the maximum transfer length!", vbCritical + vbOKOnly)
        ElseIf errorcode = 9 Then
            MsgBox("No data available!", vbCritical + vbOKOnly)
        ElseIf errorcode = 10 Then
            MsgBox("Too many devices connected!", vbCritical + vbOKOnly)
        ElseIf errorcode = 11 Then
            MsgBox("The handle is invalid!", vbCritical + vbOKOnly)
        ElseIf errorcode = 12 Then
            MsgBox("The hardware is invalid!", vbCritical + vbOKOnly)
        ElseIf errorcode = 13 Then
            MsgBox("Device not found!", vbCritical + vbOKOnly)
        ElseIf errorcode = 14 Then
            MsgBox("Invalid parameter!", vbCritical + vbOKOnly)
        ElseIf errorcode = 15 Then
            MsgBox("Eeprom error!", vbCritical + vbOKOnly)
        ElseIf errorcode = 16 Then
            MsgBox("Timeout!", vbCritical + vbOKOnly)
        ElseIf errorcode = 17 Then
            MsgBox("The device is busy!", vbCritical + vbOKOnly)
        ElseIf errorcode = 18 Then
            MsgBox("Operation aborted!", vbCritical + vbOKOnly)
        ElseIf errorcode = 19 Then
            MsgBox("I/O error!", vbCritical + vbOKOnly)
        ElseIf errorcode = 20 Then
            MsgBox("The resources are insufficient!", vbCritical + vbOKOnly)
        ElseIf errorcode = 21 Then
            MsgBox("Write failed!", vbCritical + vbOKOnly)
        End If
    End Sub

    Public Function SetRegister(address As UInt32, value As UInt32, Handle_indx As UInt32) As tError

        Select Case _boardModel
            'Case tModel.V2495
            '    mtx.WaitOne()
            '    Dim ierror = V2495_DHA_WriteReg(value, address, V2495Handle, 0)
            '    mtx.ReleaseMutex()
            '    Return IErrorV2495ToNETError(ierror)
            Case tModel.DT5550
                If _isconnected(0) = False Then
                    Return tError.NOT_CONNECTED
                End If
                mtx.WaitOne()
                Dim ierror = USB3_WriteReg(value, address, DT5550Handle)
                mtx.ReleaseMutex()
                If ierror < &HFFFFFFFF& Then
                    Return ierror
                Else
                    Return tError.ERROR_FPGA
                End If
            Case tModel.R5560
                If _isconnected(Handle_indx) = False Then
                    Return tError.NOT_CONNECTED
                End If
                mtx.WaitOne()
                Dim ierror = R5560_WriteReg(value, address, R5560Handle(Handle_indx))
                mtx.ReleaseMutex()
                If ierror < &HFFFFFFFF& Then
                    Return ierror
                Else
                    Return tError.ERROR_FPGA
                End If
            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select
    End Function

    'Public Function SetRegisterR(address As UInt32, value As UInt32, Handle_indx As UInt32) As tError
    '    If _isconnected = False Then
    '        Return tError.NOT_CONNECTED
    '    End If
    '    Select Case _boardModel
    '        'Case tModel.V2495
    '        '    mtx.WaitOne()
    '        '    Dim ierror = V2495_DHA_WriteReg(value, address, V2495Handle, 0)
    '        '    mtx.ReleaseMutex()
    '        '    Return IErrorV2495ToNETError(ierror)
    '        Case tModel.R5560
    '            mtx.WaitOne()
    '            Dim ierror = R5560_WriteReg(value, address, R5560Handle(Handle_indx))
    '            mtx.ReleaseMutex()
    '            If ierror < &HFFFFFFFF& Then
    '                Return ierror
    '            Else
    '                Return tError.ERROR_FPGA
    '            End If
    '        Case Else
    '            Return tError.UNSUPPORTED_DEVICE
    '    End Select
    'End Function

    Public Function GetRegister(address As UInt32, ByRef value As UInt32, Handle_indx As UInt32) As tError

        Select Case _boardModel
            'Case tModel.V2495
            '    mtx.WaitOne()
            '    Dim ierror = V2495_DHA_ReadReg(value, address, V2495Handle, 0)
            '    mtx.ReleaseMutex()
            '    Return IErrorV2495ToNETError(ierror)
            Case tModel.DT5550
                If _isconnected(0) = False Then
                    Return tError.NOT_CONNECTED
                End If
                mtx.WaitOne()
                Dim ierror = USB3_ReadReg(value, address, DT5550Handle)
                mtx.ReleaseMutex()
                If ierror < &HFFFFFFFF& Then
                    Return ierror
                Else
                    Return tError.ERROR_FPGA
                End If
            Case tModel.R5560
                If _isconnected(Handle_indx) = False Then
                    Return tError.NOT_CONNECTED
                End If
                mtx.WaitOne()
                Dim ierror = R5560_ReadReg(value, address, R5560Handle(Handle_indx))
                mtx.ReleaseMutex()
                If ierror < &HFFFFFFFF& Then
                    Return ierror
                Else
                    Return tError.ERROR_FPGA
                End If
            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select
    End Function

    'Public Function GetRegisterR(address As UInt32, ByRef value As UInt32, Handle_indx As UInt32) As tError
    '    If _isconnected = False Then
    '        Return tError.NOT_CONNECTED
    '    End If
    '    Select Case _boardModel
    '        'Case tModel.V2495
    '        '    mtx.WaitOne()
    '        '    Dim ierror = V2495_DHA_ReadReg(value, address, V2495Handle, 0)
    '        '    mtx.ReleaseMutex()
    '        '    Return IErrorV2495ToNETError(ierror)
    '        Case tModel.R5560
    '            mtx.WaitOne()
    '            Dim ierror = R5560_ReadReg(value, address, R5560Handle(Handle_indx))
    '            mtx.ReleaseMutex()
    '            If ierror < &HFFFFFFFF& Then
    '                Return ierror
    '            Else
    '                Return tError.ERROR_FPGA
    '            End If
    '        Case Else
    '            Return tError.UNSUPPORTED_DEVICE
    '    End Select
    'End Function

    Public Function AFE_SetIICBaseAddress(cntrl As UInt32, status As UInt32)
        Return USB3_SetIICControllerBaseAddress(cntrl, status, DT5550Handle)
    End Function

    Public Function AFE_SetHV(onoff As Boolean, voltage As Single)
        Return USB3_SetHV(onoff, voltage, DT5550Handle)
    End Function

    Public Function AFE_SetOffset(top As Boolean, voltage As Single)
        Return USB3_SetOffset(top, voltage, DT5550Handle)
    End Function

    Public Function AFE_SetImpedence(R50 As Boolean)
        Return USB3_SetImpedance(R50, DT5550Handle)
    End Function

    'Public Function WriteMem(address As UInt32, size As UInt32, value() As UInt32) As tError
    '    If _isconnected = False Then
    '        Return tError.NOT_CONNECTED
    '    End If
    '    Select Case _boardModel
    '        Case tModel.V2495

    '        Case tModel.DT5550

    '        Case Else
    '            Return tError.UNSUPPORTED_DEVICE
    '    End Select
    'End Function

    'Public Function ReadMed(address As UInt32, size As UInt32, ByRef value() As UInt32) As tError
    '    If _isconnected = False Then
    '        Return tError.NOT_CONNECTED
    '    End If

    '    Select Case _boardModel
    '        Case tModel.V2495

    '        Case tModel.DT5550

    '        Case Else
    '            Return tError.UNSUPPORTED_DEVICE
    '    End Select
    '    '
    '    ' Return tError.OK
    'End Function

    'Public Function WriteFIFO(address As UInt32, size As UInt32, value() As UInt32) As tError
    '    If _isconnected = False Then
    '        Return tError.NOT_CONNECTED
    '    End If

    '    Select Case _boardModel
    '        Case tModel.V2495

    '        Case tModel.DT5550

    '        Case Else
    '            Return tError.UNSUPPORTED_DEVICE
    '    End Select

    '    ' Return tError.OK
    'End Function

    'Public Function ReadFIFO(address As UInt32, ByRef value() As Int32, ByRef lenght As Integer, ch As Integer) As tError
    '    If _isconnected = False Then
    '        Return tError.NOT_CONNECTED
    '    End If
    '    Select Case _boardModel
    '        'Case tModel.V2495
    '        '    mtx.WaitOne()
    '        '    Dim ierror = V2495_DHA_ReadArray(value, address, lenght, False, V2495Handle, ch)
    '        '    mtx.ReleaseMutex()
    '        '    ' Return tError.OK
    '        '    Return IErrorV2495ToNETError(ierror)
    '        Case tModel.DT5550

    '        Case Else
    '            Return tError.UNSUPPORTED_DEVICE
    '    End Select

    '    ' Return tError.OK
    'End Function

    Public Function ReadData(address As UInt32, ByRef value() As UInt32, lenght As Integer, bus_mode As Integer, timeout As UInt32, ByRef read_data As UInt32, ByRef valid_data As UInt32, ByVal Handle_indx As Integer) As tError

        Select Case _boardModel
           ' Case tModel.V2495
            Case tModel.DT5550
                If _isconnected(0) = False Then
                    Return terror.NOT_CONNECTED
                End If
                mtx.WaitOne()
                Dim ierror = USB3_ReadData(value, lenght, address, bus_mode, timeout, DT5550Handle, read_data, valid_data)
                mtx.ReleaseMutex()
                Dim _terror = IErrorDT5550ToNETError(ierror)
                Return _terror
            Case tModel.R5560
                If _isconnected(Handle_indx) = False Then
                    Return terror.NOT_CONNECTED
                End If
                mtx.WaitOne()
                Dim ierror = R5560_ReadData(value, lenght, address, R5560Handle(Handle_indx), read_data)
                mtx.ReleaseMutex()
                Dim _terror = IErrorDT5550ToNETError(ierror)
                Return _terror

            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select
    End Function

    Public Function ReadDataFifo(address As UInt32, ByRef value() As UInt32, ByRef lenght As Integer, ByVal address_status As Integer, ByVal bus_mode As Integer, timeout_ms As Integer, ByRef read_data As UInt32, ByRef valid_data As UInt32, ByVal Handle_indx As Integer) As tError

        Select Case _boardModel
           ' Case tModel.V2495
            Case tModel.DT5550

            Case tModel.R5560
                If _isconnected(Handle_indx) = False Then
                    Return terror.NOT_CONNECTED
                End If
                mtx.WaitOne()
                Dim ierror = R5560_ReadFifo(value, lenght, address, address_status, bus_mode, timeout_ms, R5560Handle(Handle_indx), read_data)
                mtx.ReleaseMutex()
                Dim _terror = IErrorDT5550ToNETError(ierror)
                Return _terror

            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select
    End Function


End Class
