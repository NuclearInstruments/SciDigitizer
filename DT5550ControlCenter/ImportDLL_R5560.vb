Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Text
Module ImportDLL_R5560

    Const dllpath = ""

    Dim RMutex As System.Threading.Mutex = New System.Threading.Mutex(False, "Applicazione Singleton R5560")

    <DllImport(dllpath & "R5560_SDKLib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function R_Init() As UInteger
    End Function

    <DllImport(dllpath & "R5560_SDKLib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function R5560_ConnectTCP(ByVal IP As String, ByVal port As UInt32,
                               handle As IntPtr) As UInteger
    End Function

    <DllImport(dllpath & "R5560_SDKLib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_CloseConnection(handle As IntPtr) As UInteger
    End Function


    <DllImport(dllpath & "R5560_SDKLib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_WriteReg(ByVal data As UInt32,
                              ByVal address As UInt32,
                              handle As IntPtr) As UInteger
    End Function

    <DllImport(dllpath & "R5560_SDKLib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_ReadReg(ByRef data As UInt32,
                         ByVal address As UInt32,
                         handle As IntPtr) As UInteger
    End Function

    <DllImport(dllpath & "R5560_SDKLib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_WriteData(data As IntPtr,
                           ByVal count As Int32,
                           ByVal address As UInt32,
                           handle As IntPtr,
                           ByRef written_data As UInt32) As UInteger
    End Function

    <DllImport(dllpath & "R5560_SDKLib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_ReadData(data As IntPtr,
                              ByVal count As UInt32,
                              ByVal address As UInt32,
                              handle As IntPtr,
                              ByRef read_data As UInt32) As UInteger
    End Function

    <DllImport(dllpath & "R5560_SDKLib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_ReadFifo(data As IntPtr,
                              ByVal count As UInt32,
                              ByVal address As UInt32,
                              ByVal fifo_status_address As UInt32,
                              ByVal bus_mode As UInteger,
                              ByVal timeout As UInt32,
                              handle As IntPtr,
                              ByRef read_data As UInt32) As UInteger
    End Function


    Public Function R5560_Init() As UInteger
        Return True
    End Function

    Public Function R5560_ConnectDevice(ByVal IP As String, port As UInt32,
                                   ByRef handle As IntPtr) As UInteger
        handle = Marshal.AllocHGlobal(1000)
        Return R5560_ConnectTCP(IP, port, handle)

    End Function

    Public Function R5560_CloseConnection(ByVal handle As IntPtr) As UInteger
        Return NI_CloseConnection(handle)
    End Function

    Public Function R5560_WriteReg(data As UInt32,
                          address As UInt32,
                          handle As IntPtr) As UInteger

        RMutex.WaitOne()
        Dim result = NI_WriteReg(data, address, handle)
        RMutex.ReleaseMutex()
        Return result
    End Function

    Public Function R5560_ReadReg(ByRef data As UInt32,
                             address As UInt32,
                             handle As IntPtr) As UInteger

        '  Dim sizes = Marshal.SizeOf(GetType(UInt32))
        ' Dim ptr As IntPtr = Marshal.AllocHGlobal(sizes * 2)
        RMutex.WaitOne()
        Dim result = NI_ReadReg(data, address, handle)
        RMutex.ReleaseMutex()
        Return result
        ' Marshal.Copy(ptr, data, 0, 1)
        ' Marshal.FreeHGlobal(ptr)

        ' Return status


    End Function


    Public Function R5560_WriteData(ByRef data() As Long,
                               count As Int32,
                               address As UInt32,
                               handle As IntPtr,
                               ByRef written_data As UInt32) As UInteger

        RMutex.WaitOne()
        Dim datab(count * 4) As Byte
        Buffer.BlockCopy(data, 0, datab, 0, count * 4)

        Dim sizes = Marshal.SizeOf(GetType(UInt32))
        Dim ptr As IntPtr = Marshal.AllocHGlobal(sizes * count * 2)
        Marshal.Copy(data, 0, ptr, count)
        Dim status = NI_WriteData(ptr, count, address, handle, written_data)

        Marshal.FreeHGlobal(ptr)
        RMutex.ReleaseMutex()
        Return status

    End Function

    Public Function R5560_ReadData(ByRef data() As UInt32,
                               count As Int32,
                               address As UInt32,
                               handle As IntPtr,
                               ByRef read_data As UInt32) As UInteger
        RMutex.WaitOne()
        Dim datab(count * 4) As Byte
        Dim sizes = Marshal.SizeOf(GetType(UInt32))
        Dim ptr As IntPtr = Marshal.AllocHGlobal(sizes * count * 2)
        Dim status = NI_ReadData(ptr, count, address, handle, read_data)
        Marshal.Copy(ptr, datab, 0, count * 4)
        Marshal.FreeHGlobal(ptr)
        Buffer.BlockCopy(datab, 0, data, 0, count * 4)

        RMutex.ReleaseMutex()
        Return status

    End Function


    Public Function R5560_ReadFifo(ByRef data() As UInt32,
                            count As Int32,
                            address As UInt32,
                            fifo_status_address As UInt32,
                            bus_mode As UInteger,
                            timeout As UInt32,
                            handle As IntPtr,
                            ByRef read_data As UInt32) As UInteger
        RMutex.WaitOne()
        Dim datab(count * 4) As Byte
        Dim sizes = Marshal.SizeOf(GetType(UInt32))
        Dim ptr As IntPtr = Marshal.AllocHGlobal(sizes * count * 2)
        Dim status = NI_ReadFifo(ptr, count, address, fifo_status_address, bus_mode, timeout, handle, read_data)
        Marshal.Copy(ptr, datab, 0, count * 4)
        Marshal.FreeHGlobal(ptr)
        Buffer.BlockCopy(datab, 0, data, 0, count * 4)
        RMutex.ReleaseMutex()
        Return status

    End Function

End Module
