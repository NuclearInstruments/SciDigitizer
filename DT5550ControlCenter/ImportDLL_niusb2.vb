
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Text

Module ImportDLL_niusb2
    Const dllpath = ""



    <DllImport(dllpath & "SCIDK_Lib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function SCIDK_ConnectUSB(ByVal SN As String,
                                   handle As IntPtr) As UInteger
    End Function

    <DllImport(dllpath & "SCIDK_Lib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_CloseConnection(handle As IntPtr) As UInteger
    End Function


    <DllImport(dllpath & "SCIDK_Lib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_USBEnumerate(ListOfDevice As StringBuilder,
                                 model As String,
                                 ByRef count As Integer) As UInteger
    End Function

    <DllImport(dllpath & "SCIDK_Lib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_WriteReg(ByVal data As UInt32,
                              ByVal address As UInt32,
                              handle As IntPtr) As UInteger
    End Function

    <DllImport(dllpath & "SCIDK_Lib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_ReadReg(ByRef data As UInt32,
                             ByVal address As UInt32,
                             handle As IntPtr) As UInteger
    End Function

    <DllImport(dllpath & "SCIDK_Lib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_WriteData(data As IntPtr,
                               ByVal count As Int32,
                               ByVal address As UInt32,
                          bus_mode As UInteger,
                          ByVal timeout_ms As UInt32,
                               handle As IntPtr,
                               ByRef written_data As UInt32) As UInteger
    End Function

    <DllImport(dllpath & "SCIDK_Lib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_ReadData(data As IntPtr,
                              ByVal count As UInt32,
                              ByVal address As UInt32,
                         bus_mode As UInteger,
                          ByVal timeout_ms As UInt32,
                              handle As IntPtr,
                              ByRef read_data As UInt32,
                              ByRef valid_data As UInt32) As UInteger
    End Function
    Dim mMutex As System.Threading.Mutex = New System.Threading.Mutex(False, "Applicazione Singleton")

    Public Function USB2_ConnectDevice(ByVal SN As String,
                                       ByRef handle As IntPtr) As UInteger
        handle = Marshal.AllocHGlobal(1000)
        Dim result = SCIDK_ConnectUSB(SN, handle)
        Return result
    End Function

    Public Function USB2_CloseConnection(ByVal handle As IntPtr) As UInteger
        Return NI_CloseConnection(handle)
    End Function

    Public Function USB2_ListDevices(ByRef ListOfDevice As String,
                                     ByRef model As String,
                                     ByRef count As Integer) As UInteger
        Dim sb As New StringBuilder(2048)
        Dim result = NI_USBEnumerate(sb, model, count)
        ListOfDevice = sb.ToString
        Return result
    End Function

    Public Function USB2_WriteReg(data As UInt32,
                                  address As UInt32,
                                  handle As IntPtr) As UInteger
        mMutex.WaitOne()
        Dim result = NI_WriteReg(data, address, handle)
        mMutex.ReleaseMutex()
        Return result
    End Function

    Public Function USB2_ReadReg(ByRef data As UInt32,
                                 address As UInt32,
                                 handle As IntPtr) As UInteger

        '  Dim sizes = Marshal.SizeOf(GetType(UInt32))
        ' Dim ptr As IntPtr = Marshal.AllocHGlobal(sizes * 2)
        mMutex.WaitOne()
        Dim result = NI_ReadReg(data, address, handle)
        mMutex.ReleaseMutex()
        Return result
        ' Marshal.Copy(ptr, data, 0, 1)
        ' Marshal.FreeHGlobal(ptr)

        ' Return status


    End Function


    Public Function USB2_WriteData(ByRef data() As Long,
                               count As Int32,
                               address As UInt32,
                               bus_mode As UInteger,
                               timeout As UInt32,
                               handle As IntPtr,
                               ByRef written_data As UInt32) As UInteger

        mMutex.WaitOne()
        Dim datab(count * 4) As Byte
        Buffer.BlockCopy(data, 0, datab, 0, count * 4)

        Dim sizes = Marshal.SizeOf(GetType(UInt32))
        Dim ptr As IntPtr = Marshal.AllocHGlobal(sizes * count * 2)
        Marshal.Copy(data, 0, ptr, count)
        Dim status = NI_WriteData(ptr, count, address, bus_mode, timeout, handle, written_data)

        Marshal.FreeHGlobal(ptr)
        mMutex.ReleaseMutex()
        Return status

    End Function

    Public Function USB2_ReadData(ByRef data() As UInt32,
                               count As Int32,
                               address As UInt32,
                               bus_mode As UInteger,
                               timeout As UInt32,
                               handle As IntPtr,
                               ByRef read_data As UInt32,
                               ByRef valid_data As UInt32) As UInteger
        mMutex.WaitOne()
        Dim datab(count * 4) As Byte
        Dim sizes = Marshal.SizeOf(GetType(UInt32))
        Dim ptr As IntPtr = Marshal.AllocHGlobal(sizes * count * 2)
        Dim status = NI_ReadData(ptr, count, address, bus_mode, timeout, handle, read_data, valid_data)
        Marshal.Copy(ptr, datab, 0, count * 4)
        Marshal.FreeHGlobal(ptr)
        Buffer.BlockCopy(datab, 0, data, 0, count * 4)
        mMutex.ReleaseMutex()
        Return status

    End Function




End Module
