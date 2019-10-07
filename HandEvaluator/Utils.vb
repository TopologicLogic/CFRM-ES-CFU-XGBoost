Module Utils
    'tell the bitBlt function to do a exact copy from the source to dest.
    Public Const SRCCOPY = &HCC0020


    Private Const LVM_GETITEMCOUNT As UInteger = &H1004
    Private Const LVM_GETITEMTEXT As UInteger = &H102D


    Public Const WM_ACTIVATE = &H6
    Public Const WM_SETFOCUS = &H7
    Public Const WM_CLOSE = &H10
    Public Const WM_GETTEXTLENGTH = &HE
    Public Const WM_GETTEXT = &HD
    Public Const WM_SETTEXT = &HC
    Public Const BM_CLICK As Integer = &HF5
    Public Const BM_SETSTATE As Integer = &HF3
    Public Const BM_GETIMAGE As Integer = &HF6
    Public Const HC_ACTION As Integer = 0
    Public Const WH_MOUSE_LL As Integer = 14
    Public Const WM_MOUSEMOVE As Integer = &H200
    Public Const WM_LBUTTONDOWN As Integer = &H201
    Public Const WM_LBUTTONUP As Integer = &H202
    Public Const WM_LBUTTONDBLCLK As Integer = &H203
    Public Const WM_RBUTTONDOWN As Integer = &H204
    Public Const WM_RBUTTONUP As Integer = &H205
    Public Const WM_RBUTTONDBLCLK As Integer = &H206
    Public Const WM_MBUTTONDOWN As Integer = &H207
    Public Const WM_MBUTTONUP As Integer = &H208
    Public Const WM_MBUTTONDBLCLK As Integer = &H209
    Public Const WM_MOUSEWHEEL As Integer = &H20A
    Public Const WM_SETCURSOR As Integer = &H20
    Public Const GW_CHILD As Integer = 5
    Public Const GW_HWNDNEXT As Integer = 2

    Public Const MOUSEEVENTF_LEFTDOWN = &H2
    Public Const MOUSEEVENTF_LEFTUP = &H4
    Public Const MOUSEEVENTF_RIGHTDOWN = &H8
    Public Const MOUSEEVENTF_RIGHTUP = &H10

    Public Const SW_HIDE = 0
    Public Const SW_RESTORE = 9
    Public Const SW_MAXIMIZE = 3
    Public Const SW_MINIMIZE = 6
    Public Const SW_SHOW = 5
    Public Const SW_SHOWMAXIMIZED = 3
    Public Const SW_SHOWMINIMIZED = 2
    Public Const SW_SHOWMINNOACTIVE = 7

    Public Const HWND_TOPMOST = -1
    Public Const HWND_NOTOPMOST = -2
    Public Const SWP_NOMOVE = &H2
    Public Const SWP_NOSIZE = &H1
    Public Const SWP_NOACTIVATE = &H10
    Public Const SWP_SHOWWINDOW = &H40

    Public Const VK_NUMLOCK = &H90
    Public Const VK_SCROLL = &H91
    Public Const VK_CAPITAL = &H14
    Public Const KEYEVENTF_EXTENDEDKEY = &H1
    Public Const KEYEVENTF_KEYUP = &H2

    Public Const SMTO_ABORTIFHUNG = &H2

    Public Declare Sub keybd_event Lib "user32" _
   (ByVal bVk As Byte, _
    ByVal bScan As Byte, _
    ByVal dwflags As Long, ByVal dwExtraInfo As Long)

    Public Declare Function GetPixel Lib "gdi32" (ByVal hdc As Integer, ByVal x As Integer, ByVal y As Integer) As Integer

    Declare Function GetMenu Lib "user32" Alias "GetMenu" (ByVal hwnd As IntPtr) As IntPtr

    Public Declare Function SetWindowPos Lib "user32" (ByVal hwnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal ByValcx As Integer, ByVal cy As Integer, ByVal wFlags As UInteger) As Long

    Public Declare Function IsWindowVisible Lib "user32" (ByVal hwnd As IntPtr) As Boolean

    Public Declare Ansi Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Public Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" (ByVal hWnd1 As Long, ByVal hWnd2 As Long, ByVal lpsz1 As String, ByVal lpsz2 As String) As Long

    Public Declare Function GetDesktopWindow Lib "user32" () As IntPtr
    Public Declare Function GetParent Lib "user32" (ByVal hwnd As IntPtr) As IntPtr
    Public Declare Auto Function GetClassName Lib "user32" (ByVal hWnd As IntPtr, ByVal lpClassName As System.Text.StringBuilder, ByVal nMaxCount As Integer) As Integer
    Public Declare Function GetForegroundWindow Lib "user32" () As IntPtr
    Public Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As IntPtr) As Long
    Public Declare Function GetActiveWindow Lib "user32" () As IntPtr
    Public Declare Auto Function GetWindow Lib "user32" (ByVal hwnd As IntPtr, ByVal wCmd As Integer) As IntPtr
    Public Declare Function ShowWindow Lib "user32" Alias "ShowWindow" (ByVal hwnd As IntPtr, ByVal nCmdShow As Long) As Long

    Public Declare Function IsIconic Lib "user32" Alias "IsIconic" (ByVal hwnd As IntPtr) As IntPtr

    Public Declare Function GetWindowText Lib "user32" Alias "GetWindowTextA" (ByVal hwnd As IntPtr, ByVal lpString As System.Text.StringBuilder, ByVal cch As Long) As Long

    Public Declare Function GetWindowTextLength Lib "user32" Alias "GetWindowTextLengthA" (ByVal hwnd As IntPtr) As Integer

    'blit the image from one device context to another device context
    Public Declare Function BitBlt Lib "gdi32" Alias "BitBlt" (ByVal hDestDC As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hSrcDC As Integer, ByVal xSrc As Integer, ByVal ySrc As Integer, ByVal dwRop As Integer) As Integer

    'get the windows device context
    Public Declare Function GetDC Lib "user32" Alias "GetDC" (ByVal hwnd As IntPtr) As Integer

    'release the device context back to windows
    Public Declare Function ReleaseDC Lib "user32" Alias "ReleaseDC" (ByVal hwnd As IntPtr, ByVal hdc As Integer) As Integer

    'get the windows handle from the cursor position
    Public Declare Function WindowFromPoint Lib "user32" Alias "WindowFromPoint" (ByVal xPoint As Integer, ByVal yPoint As Integer) As IntPtr

    Public Declare Auto Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As IntPtr, ByVal wMsg As Long, ByVal wParam As Long, ByVal lParam As String) As Long

    Public Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByRef lParam As IntPtr) As IntPtr

    Public Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByRef lParam As IntPtr) As Integer

    Public Declare Auto Function SendMessage Lib "user32.dll" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wparam As Integer, ByVal lparam As System.Text.StringBuilder) As IntPtr

    Public Declare Auto Function SendMessage Lib "user32.dll" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wparam As Integer, ByVal bm As Bitmap) As IntPtr

    Public Declare Auto Function PostMessage Lib "user32" Alias "PostMessageA" (ByVal Hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Long) As Integer
    Public Declare Auto Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal Hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Long) As Integer

    'Public Declare Auto Function SendMessageTimeout Lib "user32" Alias "SendMessageTimeoutA" (ByVal hwnd As Integer, ByVal MSG As Integer, ByVal wParam As Integer, ByVal lParam As Integer, ByVal fuFlags As Integer, ByVal uTimeout As Integer, ByRef lpdwResult As Integer) As Integer
    'Public Declare Auto Function SendMessageTimeout Lib "user32" Alias "SendMessageTimeoutA" (ByVal hwnd As IntPtr, ByVal MSG As Integer, ByVal wParam As Integer, ByVal lParam As System.Text.StringBuilder, ByVal fuFlags As UInteger, ByVal uTimeout As UInteger, ByRef lpdwResult As UInteger) As IntPtr
    'Public Declare Auto Function SendMessageTimeout Lib "user32" Alias "SendMessageTimeoutA" (ByVal hwnd As IntPtr, ByVal MSG As Integer, ByVal lParam As System.Text.StringBuilder, ByVal wParam As Integer, ByVal fuFlags As UInteger, ByVal uTimeout As UInteger, ByRef lpdwResult As UInteger) As IntPtr
    Public Declare Ansi Function SendMessageTimeout Lib "user32" Alias "SendMessageTimeoutA" (ByVal hWnd As IntPtr, ByVal msg As Int32, ByVal wParam As Int32, ByVal lParam As Int32, ByVal fuFlags As Int32, ByVal uTimeout As Int32, ByRef lpdwResult As Int32) As Int32
    Public Declare Ansi Function SendMessageTimeout Lib "user32" Alias "SendMessageTimeoutA" (ByVal hWnd As IntPtr, ByVal msg As Int32, ByVal wParam As Int32, ByVal lParam As Int32, ByVal fuFlags As Int32, ByVal uTimeout As Int32, ByRef lpdwResult As UIntPtr) As Int32

    Declare Function GetProp Lib "user32.dll" Alias "GetPropA" (ByVal hwnd As Int32, ByVal lpString As String) As Int32
    Declare Function SetProp Lib "user32.dll" Alias "SetPropA" (ByVal hwnd As Int32, ByVal lpString As String, ByVal hData As Int32) As Int32
    Public Declare Function RemoveProp Lib "user32.dll" Alias "RemovePropA" (ByVal hwnd As Int32, ByVal lpString As String) As Int32
    Public Declare Function EnumProps Lib "user32" Alias "EnumPropsA" (ByVal hwnd As Integer, ByVal lpEnumFunc As Integer) As Long
    Public Declare Function EnumPropsEx Lib "user32.dll" Alias "EnumPropsExA" (ByVal hWnd As Int32, ByVal EnumPropCallback As EnumPropCallback, ByVal lParam As Int32) As Int32

    Public Declare Function lstrlen Lib "kernel32.dll" Alias "lstrlenA" (ByVal lpString As String) As Int32
    Public Declare Function lstrcpy Lib "kernel32.dll" Alias "lstrcpyA" (ByVal lpString1 As String, ByVal lpString2 As String) As Int32

    Public Delegate Function EnumPropCallback(ByVal hWnd As Integer, ByVal lpszString As Integer, ByVal hData As Integer, ByVal dwData As Integer) As Boolean

    Public Delegate Function MyDelegateCallBack(ByVal hwnd As IntPtr, ByVal lParam As Integer) As Boolean
    Public Declare Function EnumWindows Lib "user32" (ByVal x As MyDelegateCallBack, ByVal y As Integer) As Integer

    'Do mouse button action
    Public Declare Sub mouse_event Lib "user32.dll" (ByVal dwFlags As Long, ByVal dx As Long, ByVal dy As Long, ByVal cButtons As Long, ByVal dwExtraInfo As Long)

    Public Declare Function RealChildWindowFromPoint Lib "user32" _
    (ByVal hWndParent As IntPtr, _
    ByVal xPoint As Long, _
    ByVal yPoint As Long) As IntPtr

    Public Declare Function ScreenToClient Lib "user32" _
    (ByVal hWnd As IntPtr, _
    ByVal lpPoint As Point) As IntPtr

    Public Declare Function GetWindowRect Lib "user32.dll" (ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean

    Public Declare Function MoveWindow Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal bRepaint As Boolean) As Boolean

    Public Declare Function IsWindow Lib "user32" (ByVal hwnd As IntPtr) As Boolean

    Public Const ASFW_ANY As Int32 = -1
    Public Const LSFW_LOCK As Int32 = 1
    Public Const LSFW_UNLOCK As Int32 = 2

    Private Declare Function AllowSetForegroundWindow Lib "user32" Alias "AllowSetForegroundWindow" (ByVal dwProcessId As Int32) As Boolean
    Private Declare Function LockSetForegroundWindow Lib "user32" Alias "LockSetForegroundWindow" (ByVal uLockCode As Int32) As Boolean
    Private Declare Function SetFocus Lib "user32" Alias "SetFocus" (ByVal hWnd As IntPtr) As Int32

    Public Declare Function GetWindowPlacement Lib "user32" (ByVal hwnd As IntPtr, ByRef lpwndpl As WINDOWPLACEMENT) As Integer

    Public Declare Ansi Function RegisterWindowMessage Lib "user32" Alias "RegisterWindowMessageA" (ByVal lpString As String) As Int32

    <System.Runtime.InteropServices.DllImport("oleacc.dll")> _
    Private Function ObjectFromLresult( _
    ByVal lResult As IntPtr, _
    <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStruct)> ByVal refiid As Guid, _
    ByVal wParam As IntPtr) As _
    <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Interface)> Object
    End Function

    <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet:=System.Runtime.InteropServices.CharSet.Ansi)> _
    Public Structure DEVMODE
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=32)> _
        Public dmDeviceName As String
        Public dmSpecVersion As Short
        Public dmDriverVersion As Short
        Public dmSize As Short
        Public dmDriverExtra As Short
        Public dmFields As Integer
        Public dmPositionX As Integer
        Public dmPositionY As Integer
        Public dmDisplayOrientation As Integer
        Public dmDisplayFixedOutput As Integer
        Public dmColor As Short
        Public dmDuplex As Short
        Public dmYResolution As Short
        Public dmTTOption As Short
        Public dmCollate As Short
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=32)> _
        Public dmFormName As String
        Public dmLogPixels As Short
        Public dmBitsPerPel As Short
        Public dmPelsWidth As Integer
        Public dmPelsHeight As Integer
        Public dmDisplayFlags As Integer
        Public dmDisplayFrequency As Integer
        Public dmICMMethod As Integer
        Public dmICMIntent As Integer
        Public dmMediaType As Integer
        Public dmDitherType As Integer
        Public dmReserved1 As Integer
        Public dmReserved2 As Integer
        Public dmPanningWidth As Integer
        Public dmPanningHeight As Integer
    End Structure

    Public Enum DisplayChangeResultCode
        DISP_CHANGE_SUCCESSFUL = 0
        DISP_CHANGE_RESTART = 1
        DISP_CHANGE_FAILED = -1
        DISP_CHANGE_BADMODE = -2
        DISP_CHANGE_NOTUPDATED = -3
        DISP_CHANGE_BADFLAGS = -4
        DISP_CHANGE_BADPARAM = -5
        DISP_CHANGE_BADDUALVIEW = -6
    End Enum

    ' PInvoke declaration for EnumDisplaySettings Win32 API
    <System.Runtime.InteropServices.DllImport("user32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Ansi)> _
    Public Function EnumDisplaySettings( _
    ByVal lpszDeviceName As String, _
    ByVal iModeNum As Integer, _
    ByRef lpDevMode As DEVMODE) As Integer
    End Function

    ' PInvoke declaration for ChangeDisplaySettings Win32 API
    <System.Runtime.InteropServices.DllImport("user32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Ansi)> _
    Public Function ChangeDisplaySettings( _
    ByRef lpDevMode As DEVMODE, _
    ByVal dwFlags As Integer) As Integer
    End Function

    'Private Declare Function ObjectFromLresult Lib "oleacc" (ByVal lResult As UIntPtr, ByRef riid As System.Guid, ByVal wParam As Integer, ByRef ppvObject As IHTMLDocument2) As UInteger

    'Private WM_HTML_GETOBJECT As Integer = RegisterWindowMessage("WM_HTML_GETOBJECT")
    'Private IID_IHTMLDocument2 As System.Guid = GetType(IHTMLDocument2).GUID

    '<System.Runtime.InteropServices.DllImport("oleacc.dll", PreserveSig:=False)> _
    'Private Function ObjectFromLresult( _
    'ByVal lResult As Int32, _
    '<System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)> ByRef riid As System.Guid, _
    'ByVal wParam As Int32, _
    'ByRef ppvObject As mshtml.IHTMLDocument2) As Int32

    'End Function


    '    <System.Runtime.InteropServices.DllImport("kernel32.dll", ExactSpelling:=True, SetLastError:=True, CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
    'Public Function DeviceIoControl(ByVal hDevice As IntPtr, _
    ' ByVal dwIoControlCode As Int32, ByVal lpInBuffer As IntPtr, _
    ' ByVal nInBufferSize As Int32, ByVal lpOutBuffer As IntPtr, _
    ' ByVal nOutBufferSize As Int32, ByRef lpBytesReturned As Int32, _
    ' ByVal lpOverlapped As System.Threading.NativeOverlapped) As Boolean
    '    End Function

    '    <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)> _
    'Private Structure TOKEN_PRIVILEGES
    '        Public PrivilegeCount As UInt32
    '        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=ANYSIZE_ARRAY)> _
    '        Public Privileges() As LUID_AND_ATTRIBUTES
    '    End Structure

    ''[DllImport("oleacc.dll", PreserveSig = false)]
    ''   [MarshalAs(UnmanagedType.IDispatch)]
    ''    public static extern object ObjectFromLresult(
    ''                                                  IntPtr msgcallResult,
    ''                                                  [MarshalAs(UnmanagedType.LPStruct)] Guid refGuid,
    ''                                                 IntPtr resultRef);


    Public Structure POINTAPI
        Public x As Integer
        Public y As Integer
    End Structure

    Public Structure WINDOWPLACEMENT
        Public Length As Integer
        Public flags As Integer
        Public showCmd As Integer
        Public ptMinPosition As POINTAPI
        Public ptMaxPosition As POINTAPI
        Public rcNormalPosition As RECT
    End Structure

    Public Structure RECT
        Public left As Integer
        Public top As Integer
        Public right As Integer
        Public bottom As Integer
    End Structure

    Public Structure double_index
        Dim triangulation As Double
        Dim history_index As Long
    End Structure

    Public Structure data_sort
        Public history As String
        Public pocket As String

        Public pocketMask As ULong
        Public flop As ULong
        Public turn As ULong
        Public river As ULong

        Public flopOdds As Double
        Public turnOdds As Double
        Public riverOdds As Double
        Public flopStrength As Double
        Public turnStrength As Double
        Public riverStrength As Double
    End Structure

    '<Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Auto)> _
    'Public Structure data_sort
    '    Dim flopOdds As Double
    '    Dim turnOdds As Double
    '    Dim riverOdds As Double
    '    Dim flopStrength As Double
    '    Dim turnStrength As Double
    '    Dim riverStrength As Double

    '    <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=32)> _
    '    Dim history As String
    'End Structure



    'Public Declare Function ObjectFromLresult Lib "oleacc" (ByVal a As Integer, ByRef b As UUID, ByVal c As Integer, ByRef d As mshtml.IHTMLDocument2) As Integer

    ''Public Declare Function RegisterWindowMessage Lib "user32" Alias "RegisterWindowMessageA" (ByVal lpString As String) As Integer

    'Public Structure UUID
    '    Public data1 As Integer
    '    Public data2 As Int16
    '    Public data3 As Short
    '    <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8, ArraySubType:=System.Runtime.InteropServices.UnmanagedType.AsAny)> _
    '    Public data4() As Byte
    'End Structure

    'Public Declare Function SendMessageTimeout Lib "user32" Alias "SendMessageTimeoutA" (ByVal hwnd As Integer, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer, ByVal fuFlags As Integer, ByVal uTimeout As Integer, ByRef lpdwResult As Integer) As Integer

    'Public Function IEDOMFromhWnd(ByVal hwnd As IntPtr) As mshtml.IHTMLDocument2
    '    Dim IID_IHTMLDocument As UUID

    '    Dim lRes As Integer
    '    Dim lMsg As Integer
    '    Dim hr As Integer
    '    Dim ret As mshtml.IHTMLDocument2
    '    If Not hwnd.Equals(0) Then
    '        'Dim sz As Integer = Marshal.SizeOf(IID_IHTMLDocument)
    '        lMsg = RegisterWindowMessage("WM_HTML_GETOBJECT")
    '        Call SendMessageTimeout(hwnd, lMsg, 0, 0, SMTO_ABORTIFHUNG, 1000, lRes)
    '        If lRes <> 0 Then
    '            With IID_IHTMLDocument
    '                .data1 = &H626FC520
    '                .data2 = -23522
    '                .data3 = &H11CF
    '                .data4 = New Byte(7) {}
    '                .data4(0) = &HA7
    '                .data4(1) = &H31
    '                .data4(2) = &H0
    '                .data4(3) = &HA0
    '                .data4(4) = &HC9
    '                .data4(5) = &H8
    '                .data4(6) = &H26
    '                .data4(7) = &H37
    '            End With
    '            hr = ObjectFromLresult(lRes, IID_IHTMLDocument, 0, ret)
    '            If hr = 0 Then Return ret
    '            debug("HR: " & hr)
    '        Else
    '            debug("lRes: " & lRes)
    '        End If
    '    End If
    '    Return Nothing
    'End Function

    'Private Function IEDOMFromhWnd(ByVal hWnd As IntPtr) As mshtml.IHTMLDocument2
    '    On Error Resume Next

    '    Dim lres As UIntPtr

    '    If Not hWnd.Equals(0) Then
    '        If WM_HTML_GETOBJECT <> 0 Then
    '            SendMessageTimeout(hWnd, WM_HTML_GETOBJECT, 0, 0, SMTO_ABORTIFHUNG, 1000, lres)
    '            If lres.ToUInt32 <> 0 Then
    '                ' Get the object from lRes
    '                Dim doc As IHTMLDocument2
    '                Dim hr As Integer = ObjectFromLresult(lres, IID_IHTMLDocument2, 0, doc)
    '                If doc IsNot Nothing Then Return doc

    '                debug("HR: " & hr)

    '            Else
    '                debug("lRes: " & lres.ToUInt32)
    '            End If
    '        Else
    '            debug("lMsg: " & WM_HTML_GETOBJECT)
    '        End If
    '    Else
    '        debug("hWnd: " & hWnd.ToInt32)
    '    End If

    '    Return Nothing
    'End Function

    'Public Function getInnerText(ByVal hwnd As IntPtr, ByRef init_count As Integer, ByVal final_index As Integer) As String
    '    On Error Resume Next

    '    Dim classNameSB As New System.Text.StringBuilder(55)
    '    GetClassName(hwnd, classNameSB, classNameSB.Capacity)
    '    Dim cns As String = classNameSB.ToString

    '    If cns = "Internet Explorer_Server" Then
    '        Dim html As IHTMLDocument2 = IEDOMFromhWnd(hwnd)
    '        If html IsNot Nothing Then
    '            If init_count = final_index Then
    '                Dim ib As IHTMLElement = html.body
    '                Return ib.innerText()
    '            End If
    '        End If
    '        init_count += 1
    '    Else
    '        Dim cw() As IntPtr = GetWindows(hwnd)
    '        If cw IsNot Nothing AndAlso cw.Length > 0 Then
    '            For i As Integer = 0 To cw.Length - 1
    '                Dim result As String = getInnerText(cw(i), init_count, final_index)
    '                If result IsNot Nothing Then Return result
    '            Next
    '        End If
    '    End If

    '    Return Nothing
    'End Function


    Public Function ForceForeground(ByVal hWnd As IntPtr) As Boolean
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE)
        SetForegroundWindow(hWnd)
    End Function

    Public Function FileIsLocked(ByVal fileFullPathName As String) As Boolean
        Dim isLocked As Boolean = False
        Dim fileObj As System.IO.FileStream = Nothing

        Try
            fileObj = New System.IO.FileStream( _
                                    fileFullPathName, _
                                    System.IO.FileMode.Open, _
                                    System.IO.FileAccess.ReadWrite, _
                                    System.IO.FileShare.None)
        Catch
            isLocked = True
        Finally
            If fileObj IsNot Nothing Then _
                fileObj.Close()
        End Try

        Return isLocked
    End Function

    Public Function ReadAllLinesWithSharing(ByVal fn As String) As String()

        Try

            Dim fs As New System.IO.FileStream(fn, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
            Dim sr As New System.IO.StreamReader(fs)

            Dim lines() As String = sr.ReadToEnd.Split(vbCrLf)

            sr.Close() : sr.Dispose()
            fs.Close() : fs.Dispose()

            Return lines

        Catch ex As Exception

        End Try

        Return Nothing

    End Function

    Public Function ReadAllTextWithSharing(ByVal fn As String) As String

        Try

            Dim fs As New System.IO.FileStream(fn, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
            Dim sr As New System.IO.StreamReader(fs)

            Dim text As String = sr.ReadToEnd

            sr.Close() : sr.Dispose()
            fs.Close() : fs.Dispose()

            Return text

        Catch ex As Exception

        End Try

        Return Nothing

    End Function

    ' Saves an image as a jpeg image, with the given quality
    ' Gets:
    '   path   - Path to which the image would be saved.
    '   quality - An integer from 0 to 100, with 100 being the
    '           highest quality
    Public Sub SaveJpeg(ByVal path As String, ByVal img As Image, ByVal quality As Long)
        If ((quality < 0) OrElse (quality > 100)) Then
            Throw New ArgumentOutOfRangeException("quality must be between 0 and 100.")
        End If

        ' Encoder parameter for image quality
        Dim qualityParam As New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality)

        ' Jpeg image codec
        Dim jpegCodec As System.Drawing.Imaging.ImageCodecInfo = GetEncoderInfo("image/jpeg")

        Dim encoderParams As New System.Drawing.Imaging.EncoderParameters(1)
        encoderParams.Param(0) = qualityParam

        img.Save(path, jpegCodec, encoderParams)
    End Sub



    ' Returns the image codec with the given mime type
    Public Function GetEncoderInfo(ByVal mimeType As String) As System.Drawing.Imaging.ImageCodecInfo

        ' Get image codecs for all image formats
        Dim codecs As System.Drawing.Imaging.ImageCodecInfo() = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()

        ' Find the correct image codec
        For i As Integer = 0 To codecs.Length - 1
            If (codecs(i).MimeType = mimeType) Then Return codecs(i)
        Next i

        Return Nothing

    End Function

    'Public Sub SimulateMouseClick(ByVal ButtonHandle As IntPtr, ByVal x As Integer, ByVal y As Integer)
    '    'Public Declare Auto Function SendMessageTimeout Lib "user32" Alias "SendMessageTimeoutA" (ByVal hwnd As Integer, ByVal MSG As Integer, ByVal wParam As Integer, ByVal lParam As Integer, ByVal fuFlags As Integer, ByVal uTimeout As Integer, ByRef lpdwResult As Integer) As Integer
    '    Dim dat As Integer = ((y << 16) Or x)
    '    Dim res As Integer
    '    Call PostMessage(ButtonHandle, WM_LBUTTONDOWN, 0, dat)
    '    Call SendMessageTimeout(ButtonHandle, WM_LBUTTONDOWN, 0, dat, SMTO_ABORTIFHUNG, 200, res)
    '    Call SendMessageTimeout(ButtonHandle, BM_SETSTATE, 1, dat, SMTO_ABORTIFHUNG, 200, res)
    '    Call PostMessage(ButtonHandle, WM_LBUTTONUP, 0, dat)
    '    Call SendMessageTimeout(ButtonHandle, WM_LBUTTONUP, 0, dat, SMTO_ABORTIFHUNG, 200, res)
    '    Call SendMessageTimeout(ButtonHandle, BM_SETSTATE, 1, dat, SMTO_ABORTIFHUNG, 200, res)
    'End Sub

    Public Function getFKlast(ByVal key As String, ByVal data As String) As String
        Dim loc1 As Integer = data.LastIndexOf(key)
        If loc1 > -1 Then _
            getFKlast = data.Substring(loc1 + key.Length, data.IndexOf(";", loc1 + key.Length) - loc1 - key.Length) _
        Else _
            getFKlast = ""
    End Function

    Public Function GetWindowFromPoint(ByVal X As Long, ByVal Y As Long) As IntPtr

        Dim p As Point, ch As IntPtr, h As IntPtr

        ch = WindowFromPoint(X, Y)

        Do

            h = ch
            p.X = X : p.Y = Y
            ScreenToClient(h, p)
            ch = RealChildWindowFromPoint(h, p.X, p.Y)

        Loop Until ch.ToInt32 = h.ToInt32 Or ch.ToInt32 = 0

        GetWindowFromPoint = h

    End Function

    '****************************************************
    '* Do a left mouse click on current position
    '****************************************************
    Sub Do_LMouseClick()
        Call mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0)
        Call mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0)
    End Sub

    Public Function GetWindows(ByVal ParentWindowHandle As IntPtr) As IntPtr()

        Dim ptrChild As IntPtr
        Dim ptrRet() As IntPtr = Nothing
        Dim iCounter As Integer

        'get first child handle...
        ptrChild = GetWindow(ParentWindowHandle, GW_CHILD)

        'loop through and collect all child window handles...
        Do Until ptrChild.Equals(IntPtr.Zero)
            'process child...
            ReDim Preserve ptrRet(iCounter)
            ptrRet(iCounter) = ptrChild
            'get next child...
            ptrChild = GetWindow(ptrChild, GW_HWNDNEXT)
            iCounter += 1
        Loop

        'return...
        Return ptrRet

    End Function

    Public Sub SimulateButtonClick(ByVal ButtonHandle As IntPtr)

        'send the left mouse button "down" message to the button...
        For i As Integer = 0 To 3
            Call SendMessage(ButtonHandle, BM_CLICK, i, IntPtr.Zero)
        Next

        'send the button state message to the button, telling it to handle its events...
        Call SendMessage(ButtonHandle, BM_SETSTATE, 1, IntPtr.Zero)

    End Sub

    'Public Sub SimulateMouseClick(ByVal ButtonHandle As IntPtr, ByVal x As Integer, ByVal y As Integer)
    '    Dim dat As Integer = ((y << 16) Or x)
    '    Call PostMessage(ButtonHandle, WM_LBUTTONDOWN, 0, dat)
    '    Call SendMessage(ButtonHandle, WM_LBUTTONDOWN, 0, dat)
    '    Call SendMessage(ButtonHandle, BM_SETSTATE, 1, dat)
    '    Call PostMessage(ButtonHandle, WM_LBUTTONUP, 0, dat)
    '    Call SendMessage(ButtonHandle, WM_LBUTTONUP, 0, dat)
    '    Call SendMessage(ButtonHandle, BM_SETSTATE, 1, dat)
    'End Sub

    'Public Sub SimulateMouseDoubleClick(ByVal ButtonHandle As IntPtr, ByVal x As Integer, ByVal y As Integer)
    '    Dim dat As Integer = ((y << 16) Or x)
    '    Call PostMessage(ButtonHandle, WM_LBUTTONDBLCLK, 0, dat)
    '    Call SendMessage(ButtonHandle, WM_LBUTTONDBLCLK, 0, dat)
    '    Call SendMessage(ButtonHandle, BM_SETSTATE, 1, dat)
    '    'Call PostMessage(ButtonHandle, WM_LBUTTONUP, 0, dat)
    '    'Call SendMessage(ButtonHandle, WM_LBUTTONUP, 0, dat)
    '    'Call SendMessage(ButtonHandle, BM_SETSTATE, 1, dat)
    'End Sub



    'Public Function getButtonImage(ByVal ButtonHandle As IntPtr) As Bitmap
    '    Dim ptr As IntPtr
    '    Call SendMessage(ButtonHandle, BM_GETIMAGE, 0, ptr)
    '    getButtonImage = getBitmap(ptr, 100, 100)
    'End Function

    Public Function GetWindowText(ByVal WindowHandle As IntPtr) As String

        Dim ptrRet As IntPtr
        Dim ptrLength As IntPtr

        'get length for buffer...
        ptrLength = SendMessage(WindowHandle, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero)

        'create buffer for return value...
        Dim sbText As New System.Text.StringBuilder(ptrLength.ToInt32 + 1)

        'get window text...
        ptrRet = SendMessage(WindowHandle, WM_GETTEXT, ptrLength.ToInt32 + 1, sbText)

        'get return value...
        Return sbText.ToString

    End Function




    Public Sub Quicksort(ByRef list1() As Double, ByRef list2() As String, ByVal min As Integer, ByVal max As Integer)
        Dim random_number As New Random
        Dim med_value1 As Double
        Dim med_value2 As String
        Dim hi As Integer
        Dim lo As Integer
        Dim i As Integer

        ' If min >= max, the list contains 0 or 1 items so
        ' it is sorted.
        If min >= max Then Exit Sub

        ' Pick the dividing value.
        i = random_number.Next(min, max + 1)
        med_value1 = list1(i)
        med_value2 = list2(i)

        ' Swap it to the front.
        list1(i) = list1(min)
        list2(i) = list2(min)

        lo = min
        hi = max
        Do
            ' Look down from hi for a value < med_value.
            Do While list1(hi) >= med_value1
                hi = hi - 1
                If hi <= lo Then Exit Do
            Loop
            If hi <= lo Then
                list1(lo) = med_value1
                list2(lo) = med_value2
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(lo) = list1(hi)
            list2(lo) = list2(hi)

            ' Look up from lo for a value >= med_value.
            lo = lo + 1
            Do While list1(lo) < med_value1
                lo = lo + 1
                If lo >= hi Then Exit Do
            Loop
            If lo >= hi Then
                lo = hi
                list1(hi) = med_value1
                list2(hi) = med_value2
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(hi) = list1(lo)
            list2(hi) = list2(lo)
        Loop

        ' Sort the two sublists.
        Quicksort(list1, list2, min, lo - 1)
        Quicksort(list1, list2, lo + 1, max)
    End Sub

    Public Sub QuicksortDoubleIndex(ByRef list1() As double_index, ByVal min As Long, ByVal max As Long)
        Dim random_number As New Random
        Dim med_value1 As double_index
        Dim hi, lo, i As Long

        ' If min >= max, the list contains 0 or 1 items so
        ' it is sorted.
        If min >= max Then Exit Sub

        ' Pick the dividing value.
        i = random_number.Next(min, max + 1)

        med_value1 = list1(i)

        ' Swap it to the front.
        list1(i) = list1(min)

        lo = min
        hi = max
        Do
            ' Look down from hi for a value < med_value.
            Do While list1(hi).triangulation >= med_value1.triangulation
                hi = hi - 1
                If hi <= lo Then Exit Do
            Loop
            If hi <= lo Then
                list1(lo) = med_value1
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(lo) = list1(hi)

            ' Look up from lo for a value >= med_value.
            lo = lo + 1
            Do While list1(lo).triangulation < med_value1.triangulation
                lo = lo + 1
                If lo >= hi Then Exit Do
            Loop
            If lo >= hi Then
                lo = hi
                list1(hi) = med_value1
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(hi) = list1(lo)
        Loop

        random_number = Nothing
        med_value1 = Nothing

        ' Sort the two sublists.
        QuicksortDoubleIndex(list1, min, lo - 1)
        QuicksortDoubleIndex(list1, lo + 1, max)
    End Sub

    Public Sub QuicksortByHistory(ByRef list1() As data_sort, ByRef min As Long, ByRef max As Long)
        'Dim random_number As New Random
        Dim hi, lo, i As Long

        ' If min >= max, the list contains 0 or 1 items so
        ' it is sorted.
        If min >= max Then Exit Sub

        ' Pick the dividing value.
        i = min + CInt(Rnd() * ((max + 1) - min))
        'i = random_number.Next(min, max + 1)

        Dim med_value1 As data_sort = list1(i)

        ' Swap it to the front.
        list1(i) = list1(min)

        lo = min
        hi = max
        Do
            ' Look down from hi for a value < med_value.
            Do While list1(hi).history >= med_value1.history
                hi = hi - 1
                If hi <= lo Then Exit Do
            Loop
            If hi <= lo Then
                list1(lo) = med_value1
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(lo) = list1(hi)

            ' Look up from lo for a value >= med_value.
            lo = lo + 1
            Do While list1(lo).history < med_value1.history
                lo = lo + 1
                If lo >= hi Then Exit Do
            Loop
            If lo >= hi Then
                lo = hi
                list1(hi) = med_value1
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(hi) = list1(lo)
        Loop

        ' Sort the two sublists.
        QuicksortByHistory(list1, min, lo - 1)
        QuicksortByHistory(list1, lo + 1, max)
    End Sub

    Public Sub QuicksortByPocket(ByRef list1() As data_sort, ByRef min As Long, ByRef max As Long)
        'Dim random_number As New Random
        Dim hi, lo, i As Long

        ' If min >= max, the list contains 0 or 1 items so
        ' it is sorted.
        If min >= max Then Exit Sub

        ' Pick the dividing value.
        i = min + CInt(Rnd() * ((max + 1) - min))
        'i = random_number.Next(min, max + 1)

        Dim med_value1 As data_sort = list1(i)

        ' Swap it to the front.
        list1(i) = list1(min)

        lo = min
        hi = max
        Do
            ' Look down from hi for a value < med_value.
            Do While list1(hi).pocket >= med_value1.pocket
                hi = hi - 1
                If hi <= lo Then Exit Do
            Loop
            If hi <= lo Then
                list1(lo) = med_value1
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(lo) = list1(hi)

            ' Look up from lo for a value >= med_value.
            lo = lo + 1
            Do While list1(lo).pocket < med_value1.pocket
                lo = lo + 1
                If lo >= hi Then Exit Do
            Loop
            If lo >= hi Then
                lo = hi
                list1(hi) = med_value1
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(hi) = list1(lo)
        Loop

        ' Sort the two sublists.
        QuicksortByPocket(list1, min, lo - 1)
        QuicksortByPocket(list1, lo + 1, max)
    End Sub

    Public Sub QuicksortByFlop(ByRef list1() As data_sort, ByRef min As Long, ByRef max As Long)
        'Dim random_number As New Random
        Dim hi, lo, i As Long

        ' If min >= max, the list contains 0 or 1 items so
        ' it is sorted.
        If min >= max Then Exit Sub

        ' Pick the dividing value.
        i = min + CInt(Rnd() * ((max + 1) - min))
        'i = random_number.Next(min, max + 1)

        Dim med_value1 As data_sort = list1(i)

        ' Swap it to the front.
        list1(i) = list1(min)

        lo = min
        hi = max
        Do
            ' Look down from hi for a value < med_value.
            Do While list1(hi).flop >= med_value1.flop
                hi = hi - 1
                If hi <= lo Then Exit Do
            Loop
            If hi <= lo Then
                list1(lo) = med_value1
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(lo) = list1(hi)

            ' Look up from lo for a value >= med_value.
            lo = lo + 1
            Do While list1(lo).flop < med_value1.flop
                lo = lo + 1
                If lo >= hi Then Exit Do
            Loop
            If lo >= hi Then
                lo = hi
                list1(hi) = med_value1
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(hi) = list1(lo)
        Loop

        ' Sort the two sublists.
        QuicksortByFlop(list1, min, lo - 1)
        QuicksortByFlop(list1, lo + 1, max)
    End Sub

    Public Sub QuicksortByTurn(ByRef list1() As data_sort, ByRef min As Long, ByRef max As Long)
        'Dim random_number As New Random
        Dim hi, lo, i As Long

        ' If min >= max, the list contains 0 or 1 items so
        ' it is sorted.
        If min >= max Then Exit Sub

        ' Pick the dividing value.
        i = min + CInt(Rnd() * ((max + 1) - min))
        'i = random_number.Next(min, max + 1)

        Dim med_value1 As data_sort = list1(i)

        ' Swap it to the front.
        list1(i) = list1(min)

        lo = min
        hi = max
        Do
            ' Look down from hi for a value < med_value.
            Do While list1(hi).turn >= med_value1.turn
                hi = hi - 1
                If hi <= lo Then Exit Do
            Loop
            If hi <= lo Then
                list1(lo) = med_value1
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(lo) = list1(hi)

            ' Look up from lo for a value >= med_value.
            lo = lo + 1
            Do While list1(lo).turn < med_value1.turn
                lo = lo + 1
                If lo >= hi Then Exit Do
            Loop
            If lo >= hi Then
                lo = hi
                list1(hi) = med_value1
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(hi) = list1(lo)
        Loop

        ' Sort the two sublists.
        QuicksortByTurn(list1, min, lo - 1)
        QuicksortByTurn(list1, lo + 1, max)
    End Sub

    Public Sub QuicksortByRiver(ByRef list1() As data_sort, ByRef min As Long, ByRef max As Long)
        'Dim random_number As New Random
        Dim hi, lo, i As Long

        ' If min >= max, the list contains 0 or 1 items so
        ' it is sorted.
        If min >= max Then Exit Sub

        ' Pick the dividing value.
        i = min + CInt(Rnd() * ((max + 1) - min))
        'i = random_number.Next(min, max + 1)

        Dim med_value1 As data_sort = list1(i)

        ' Swap it to the front.
        list1(i) = list1(min)

        lo = min
        hi = max
        Do
            ' Look down from hi for a value < med_value.
            Do While list1(hi).river >= med_value1.river
                hi = hi - 1
                If hi <= lo Then Exit Do
            Loop
            If hi <= lo Then
                list1(lo) = med_value1
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(lo) = list1(hi)

            ' Look up from lo for a value >= med_value.
            lo = lo + 1
            Do While list1(lo).river < med_value1.river
                lo = lo + 1
                If lo >= hi Then Exit Do
            Loop
            If lo >= hi Then
                lo = hi
                list1(hi) = med_value1
                Exit Do
            End If

            ' Swap the lo and hi values.
            list1(hi) = list1(lo)
        Loop

        ' Sort the two sublists.
        QuicksortByRiver(list1, min, lo - 1)
        QuicksortByRiver(list1, lo + 1, max)
    End Sub

    Dim wshShell As Object = CreateObject("WScript.Shell")

    Public Sub sendClickFormCoords(ByVal hwnd As IntPtr, ByVal x As Integer, ByVal y As Integer)
        Dim rect As Utils.RECT

        SetForegroundWindow(hwnd)
        GetWindowRect(hwnd, rect)

        Cursor.Position = New Point(rect.left + x, rect.top + y + 28)
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0)
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0)
    End Sub

    Public Sub sendKeysU(ByVal hwnd As IntPtr, ByVal keys As String)
        Dim wshShell As Object = CreateObject("WScript.Shell")
        SetForegroundWindow(hwnd)
        wshShell.SendKeys(keys)
        wshShell = Nothing
    End Sub

    Public Function getRandomByProbability2D(ByRef rand As Random, ByRef two_dimensional_array(,) As Double, ByVal left_index As Integer) As Integer

        On Error Resume Next

        If two_dimensional_array Is Nothing Then Return 0
        If rand Is Nothing Then rand = New Random()

        Dim size As Integer = two_dimensional_array.Length / (two_dimensional_array.Rank + 1)

        Dim rd As Double = rand.NextDouble

        Dim total As Double = 0

        Dim z As Integer = 0

        For z = 0 To size - 1
            If Not Double.IsNaN(two_dimensional_array(left_index, z)) AndAlso _
            Not Double.IsInfinity(two_dimensional_array(left_index, z)) AndAlso _
            two_dimensional_array(left_index, z) > 0 Then _
                total += two_dimensional_array(left_index, z)
        Next

        If total <= 0 Then Return rand.Next(0, size)

        Dim count As Double = 0

        Dim index As Integer = size - 1

        z = 0

        While z < size
            If Not Double.IsNaN(two_dimensional_array(left_index, z)) AndAlso _
            Not Double.IsInfinity(two_dimensional_array(left_index, z)) AndAlso _
            two_dimensional_array(left_index, z) > 0 Then _
                count += two_dimensional_array(left_index, z) / total
            If count >= rd Then
                index = z
                Exit While
            End If
            z += 1
        End While

        Return index

    End Function


    Public Sub getScreen(ByVal hwnd As IntPtr, ByRef bm As Bitmap)
        'create a reference to the buffered image
        Dim gfx As Graphics = Graphics.FromImage(bm)

        'the windows device context
        Dim dc As Integer, dc2 As IntPtr

        'get the windows device context
        dc = GetDC(hwnd)

        'get the device context for our bitmap object
        dc2 = gfx.GetHdc

        'blit the pic from the window handle to our bitmap object
        BitBlt(dc2.ToInt32, 0, 0, bm.Width, bm.Height, dc, 0, 0, SRCCOPY)

        ReleaseDC(hwnd, dc)
        gfx.ReleaseHdc(dc2)
    End Sub

    'Public Sub getScreen(ByVal hwnd As IntPtr, ByRef bm_bytes() As Byte, ByVal width As Integer, ByVal height As Integer)

    '    'create a reference to the buffered image
    '    Dim gfx As Graphics = Graphics.FromImage(bm)

    '    'the windows device context
    '    Dim dc As Integer, dc2 As IntPtr

    '    'get the windows device context
    '    dc = GetDC(hwnd)

    '    'get the device context for our bitmap object
    '    dc2 = gfx.GetHdc

    '    'blit the pic from the window handle to our bitmap object
    '    BitBlt(dc2.ToInt32, 0, 0, bm.Width, bm.Height, dc, 0, 0, SRCCOPY)

    '    ReleaseDC(hwnd, dc)
    '    gfx.ReleaseHdc(dc2)

    'End Sub

    Public Function colorCloseTo(ByVal c As Color, ByVal r As Integer, ByVal g As Integer, ByVal b As Integer, ByVal offs As Integer) As Boolean
        Return (CInt(c.R) >= r - offs AndAlso CInt(c.R) <= r + offs AndAlso _
                CInt(c.G) >= g - offs AndAlso CInt(c.G) <= g + offs AndAlso _
                CInt(c.B) >= b - offs AndAlso CInt(c.B) <= b + offs)
    End Function

    Public Sub BMPToBytes(ByRef bmp As Bitmap, ByRef bmpBytes() As Byte)

        Dim bData As System.Drawing.Imaging.BitmapData = _
            bmp.LockBits(New Rectangle(New Point(), bmp.Size), _
            Imaging.ImageLockMode.ReadOnly, Imaging.PixelFormat.Format24bppRgb)

        Dim byteCount As Integer = bData.Stride * bmp.Height

        If byteCount <= bmpBytes.Length Then _
            System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, bmpBytes, 0, byteCount)

        bmp.UnlockBits(bData)

    End Sub

    Public Function BytesToBMP(ByRef bmpBytes() As Byte, ByVal width As Integer, ByVal height As Integer) As Bitmap
        Dim bmp As Bitmap = New Bitmap(width, height)

        Dim bData As System.Drawing.Imaging.BitmapData = _
            bmp.LockBits(New Rectangle(New Point(), bmp.Size), _
            Imaging.ImageLockMode.WriteOnly, Imaging.PixelFormat.Format24bppRgb)

        System.Runtime.InteropServices.Marshal.Copy(bmpBytes, 0, bData.Scan0, bmpBytes.Length)

        bmp.UnlockBits(bData)

        Return bmp
    End Function

    Public Function GetPixel(ByVal x As Integer, ByVal y As Integer, ByVal from_width As Integer, ByRef from() As Byte) As Color
        'MsgBox(from_width)
        Static fstart As Integer

        If from_width Mod 2 = 1 Then _
            fstart = (y * from_width * 3) + (x * 3) + y _
        Else _
            fstart = (y * from_width * 3) + (x * 3)

        Return Color.FromArgb(from(fstart + 2), from(fstart + 1), from(fstart))

    End Function

    Public Function GetPixelBinary(ByVal x As Integer, ByVal y As Integer, ByVal from_width As Integer, ByRef from() As Byte) As Boolean
        'MsgBox(from_width)
        Static fstart As Integer

        If from_width Mod 2 = 1 Then _
            fstart = (y * from_width * 3) + (x * 3) + y _
        Else _
            fstart = (y * from_width * 3) + (x * 3)

        Return (from(fstart + 2) Or from(fstart + 1) Or from(fstart))

    End Function

    Public Function GetPixelBinaryWB(ByVal x As Integer, ByVal y As Integer, ByVal from_width As Integer, ByRef from() As Byte) As Boolean
        'MsgBox(from_width)
        Static fstart As Integer

        If from_width Mod 2 = 1 Then _
            fstart = (y * from_width * 3) + (x * 3) + y _
        Else _
            fstart = (y * from_width * 3) + (x * 3)

        Return (from(fstart + 2) = 255 AndAlso from(fstart + 1) = 255 AndAlso from(fstart) = 255)

    End Function

    Public Sub contrastRect(ByVal from_rect As Rectangle, ByVal from_width As Integer, ByRef from() As Byte, ByVal high As Byte, ByVal low As Byte)
        'On Error Resume Next
        Dim x, fstart, y, c, f_c As Integer
        fstart = (from_rect.Y * from_width * 3) + (from_rect.X * 3)
        For y = 0 To from_rect.Height - 1
            f_c = fstart
            For x = 0 To from_rect.Width - 1
                For c = 0 To 2
                    If from(f_c) < high Then from(f_c) = low
                    f_c += 1
                Next c
            Next x
            fstart += from_width * 3
        Next y
    End Sub

    Public Function compareRect(ByVal r As System.Drawing.Rectangle, ByRef bm1 As Bitmap, ByRef bm2 As Bitmap) As Double

        Dim c1, c2 As Color
        Dim total_distance As Double = 1

        For x As Integer = 0 To r.Width - 1
            For y As Integer = 0 To r.Height - 1
                c1 = bm1.GetPixel(r.Left + x, r.Top + y)
                c2 = bm2.GetPixel(x, y)

                total_distance += _
                (Math.Abs(CInt(c1.R) - CInt(c2.R)) + _
                Math.Abs(CInt(c1.G) - CInt(c2.G)) + _
                Math.Abs(CInt(c1.B) - CInt(c2.B))) / 3

            Next
        Next

        Return total_distance / (r.Width * r.Height)

    End Function

    Public Function compareRect(ByVal from_rect As Rectangle, ByVal too_point As Point, ByVal from_width As Integer, ByRef from() As Byte, ByVal too_width As Integer, ByRef too() As Byte) As Integer
        Dim x, fstart, y, tstart, c, t_c, f_c, t_add_byte, f_add_byte, difference As Integer
        difference = 0

        If from_width Mod 2 = 1 Then
            fstart = (from_rect.Y * from_width * 3) + (from_rect.X * 3) + from_rect.Y
            f_add_byte = 1
        Else
            fstart = (from_rect.Y * from_width * 3) + (from_rect.X * 3)
            f_add_byte = 0
        End If

        If too_width Mod 2 = 1 Then
            tstart = (too_point.Y * too_width * 3) + (too_point.X * 3) + too_point.Y
            t_add_byte = 1
        Else
            tstart = (too_point.Y * too_width * 3) + (too_point.X * 3)
            t_add_byte = 0
        End If

        For y = 0 To from_rect.Height - 1
            f_c = fstart
            t_c = tstart
            For x = 0 To from_rect.Width - 1
                For c = 0 To 2
                    If too(t_c) <> from(f_c) Then difference += 1
                    t_c += 1
                    f_c += 1
                Next c
            Next x
            fstart += from_width * 3 + f_add_byte
            tstart += too_width * 3 + t_add_byte
        Next y

        Return difference
    End Function

    Public Sub copyRect(ByVal from_rect As Rectangle, ByVal too_point As Point, ByVal from_width As Integer, ByRef from() As Byte, ByVal too_width As Integer, ByRef too() As Byte)
        'MsgBox(from_width)
        Dim x, fstart, y, tstart, c, t_c, f_c, t_add_byte, f_add_byte As Integer

        If from_width Mod 2 = 1 Then
            fstart = (from_rect.Y * from_width * 3) + (from_rect.X * 3) + from_rect.Y
            f_add_byte = 1
        Else
            fstart = (from_rect.Y * from_width * 3) + (from_rect.X * 3)
            f_add_byte = 0
        End If

        If too_width Mod 2 = 1 Then
            tstart = (too_point.Y * too_width * 3) + (too_point.X * 3) + too_point.Y
            t_add_byte = 1
        Else
            tstart = (too_point.Y * too_width * 3) + (too_point.X * 3)
            t_add_byte = 0
        End If

        For y = 0 To from_rect.Height - 1
            f_c = fstart
            t_c = tstart
            For x = 0 To from_rect.Width - 1
                For c = 0 To 2
                    too(t_c) = from(f_c)
                    t_c += 1
                    f_c += 1
                Next c
            Next x
            fstart += from_width * 3 + f_add_byte
            tstart += too_width * 3 + t_add_byte
        Next y
    End Sub

    Public Function sumRect(ByVal from_rect As Rectangle, ByVal from_width As Integer, ByRef from() As Byte) As Long
        'MsgBox(from_width)
        Dim x, fstart, y, c, f_c, f_add_byte As Integer
        Dim total As Long = 0

        If from_width Mod 2 = 1 Then
            fstart = (from_rect.Y * from_width * 3) + (from_rect.X * 3) + from_rect.Y
            f_add_byte = 1
        Else
            fstart = (from_rect.Y * from_width * 3) + (from_rect.X * 3)
            f_add_byte = 0
        End If

        For y = 0 To from_rect.Height - 1
            f_c = fstart
            For x = 0 To from_rect.Width - 1
                For c = 0 To 2
                    total += from(f_c)
                    f_c += 1
                Next c
            Next x
            fstart += from_width * 3 + f_add_byte
        Next y

        Return total
    End Function

    Public Function HashRect(ByVal from_rect As Rectangle, ByRef bm As Bitmap) As Long
        On Error Resume Next
        Dim x, y As Integer
        Dim total As Long = 0
        Dim c As Color

        Dim loc As Integer
        For y = from_rect.Y To from_rect.Y + from_rect.Height - 1
            For x = from_rect.X To from_rect.X + from_rect.Width - 1
                c = bm.GetPixel(x, y)
                loc = y - from_rect.Y + x - from_rect.X
                total += loc * (c.R) + loc * (c.G) + loc * (c.B)
            Next x
        Next y

        Return total
    End Function

    Public Function HashRectMD5(ByVal from_rect As Rectangle, ByRef bm As Bitmap) As String
        On Error Resume Next

        Dim x, y As Integer
        Dim c As Color
        Dim a As New ArrayList

        Dim i As Integer = 0

        For y = from_rect.Y To from_rect.Y + from_rect.Height - 1
            For x = from_rect.X To from_rect.X + from_rect.Width - 1
                c = bm.GetPixel(x, y) : a.Add(c.R) : a.Add(c.G) : a.Add(c.B)
            Next x
        Next y

        Dim b As Byte

        Dim data() As Byte = System.Security.Cryptography.MD5.Create().ComputeHash(a.ToArray(b.GetType))

        Dim hash As String = ""
        For x = 0 To data.Length - 1
            hash &= data(x).ToString("x2")
        Next

        a = Nothing
        data = Nothing

        Return hash

    End Function

    Public Function HashRectMD5BGR(ByVal from_rect As Rectangle, ByRef bm() As Byte, ByVal bm_width As Integer) As Long
        On Error Resume Next

        Dim x, y, fstart, fstart_inc As Integer

        If bm_width Mod 2 = 1 Then
            fstart = (from_rect.Y * bm_width * 3) + (from_rect.X * 3) + from_rect.Y
            fstart_inc = bm_width * 3 + 1
        Else
            fstart = (from_rect.Y * bm_width * 3) + (from_rect.X * 3)
            fstart_inc = bm_width * 3
        End If

        Dim md5 As System.Security.Cryptography.MD5CryptoServiceProvider = _
            System.Security.Cryptography.MD5.Create()

        Dim ob(from_rect.Width * 3 - 1) As Byte

        For y = 0 To from_rect.Height - 2
            md5.TransformBlock(bm, fstart, from_rect.Width * 3, ob, 0)
            fstart += fstart_inc
        Next y

        md5.TransformFinalBlock(bm, fstart, from_rect.Width * 3)


        Dim data() As Byte = md5.Hash

        md5.Clear()

        Dim hash As Long

        Dim hsize As Integer = System.Runtime.InteropServices.Marshal.SizeOf(hash)
        Dim hptr As IntPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(hsize)
        System.Runtime.InteropServices.Marshal.Copy(data, 0, hptr, hsize)
        hash = System.Runtime.InteropServices.Marshal.ReadInt64(hptr)
        System.Runtime.InteropServices.Marshal.FreeHGlobal(hptr)

        data = Nothing
        md5 = Nothing

        Return hash

    End Function

    Public Function HashRectMD5(ByVal from_rect As Rectangle, ByRef bm() As Byte, ByVal bm_width As Integer) As String
        On Error Resume Next

        Dim x, y, fstart, fstart_inc, f_c As Integer

        If bm_width Mod 2 = 1 Then
            fstart = (from_rect.Y * bm_width * 3) + (from_rect.X * 3) + from_rect.Y
            fstart_inc = bm_width * 3 + 1
        Else
            fstart = (from_rect.Y * bm_width * 3) + (from_rect.X * 3)
            fstart_inc = bm_width * 3
        End If

        Dim a As New ArrayList

        'Dim ob(from_rect.Width * 3 - 1) As Byte
        'Dim ta(from_rect.Width * 3 - 1) As Byte

        'Dim md5 As System.Security.Cryptography.MD5CryptoServiceProvider = _
        'System.Security.Cryptography.MD5.Create()

        'Dim tai As Integer = 0

        'For y = 0 To from_rect.Height - 1
        '    f_c = fstart
        '    tai = 0
        '    For x = 0 To from_rect.Width - 1
        '        ta(tai) = bm(f_c + 2)
        '        ta(tai + 1) = bm(f_c + 1)
        '        ta(tai + 2) = bm(f_c)
        '        f_c += 3
        '        tai += 3
        '    Next x
        '    md5.TransformBlock(ta, 0, from_rect.Width * 3, ob, 0)
        '    fstart += fstart_inc
        'Next y

        'f_c = fstart
        'tai = 0
        'For x = 0 To from_rect.Width - 1
        '    ta(tai) = bm(f_c + 2)
        '    ta(tai + 1) = bm(f_c + 1)
        '    ta(tai + 2) = bm(f_c)
        '    f_c += 3
        '    tai += 3
        'Next x
        'md5.TransformFinalBlock(ta, 0, from_rect.Width * 3)

        'Dim data() As Byte = md5.Hash

        'md5.Clear()

        For y = 0 To from_rect.Height - 1
            f_c = fstart
            For x = 1 To from_rect.Width
                a.Add(bm(f_c + 2))
                a.Add(bm(f_c + 1))
                a.Add(bm(f_c))
                f_c += 3
            Next
            fstart += fstart_inc
        Next y

        Dim b As Byte

        Dim data() As Byte = System.Security.Cryptography.MD5.Create().ComputeHash(a.ToArray(b.GetType))

        'Dim hash As Long

        'Dim hsize As Integer = System.Runtime.InteropServices.Marshal.SizeOf(hash)
        'Dim hptr As IntPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(hsize)

        'System.Runtime.InteropServices.Marshal.Copy(data, 0, hptr, hsize)
        'hash = System.Runtime.InteropServices.Marshal.ReadInt64(hptr)
        'System.Runtime.InteropServices.Marshal.FreeHGlobal(hptr)


        Dim hash As String = ""
        For x = 0 To data.Length - 1
            hash &= data(x).ToString("x2")
        Next

        a = Nothing
        data = Nothing

        Return hash

    End Function

    Public Function HashRectMD5Long(ByVal from_rect As Rectangle, ByRef bm As Bitmap) As Long
        On Error Resume Next

        Dim x, y As Integer
        Dim c As Color
        Dim a As New ArrayList

        Dim i As Integer = 0

        For y = from_rect.Y To from_rect.Y + from_rect.Height - 1
            For x = from_rect.X To from_rect.X + from_rect.Width - 1
                c = bm.GetPixel(x, y) : a.Add(c.R) : a.Add(c.G) : a.Add(c.B)
            Next x
        Next y

        Dim b As Byte

        Dim data() As Byte = System.Security.Cryptography.MD5.Create().ComputeHash(a.ToArray(b.GetType))

        Dim hash As Long

        Dim hsize As Integer = System.Runtime.InteropServices.Marshal.SizeOf(hash)
        Dim hptr As IntPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(hsize)
        System.Runtime.InteropServices.Marshal.Copy(data, 0, hptr, hsize)
        hash = System.Runtime.InteropServices.Marshal.ReadInt64(hptr)
        System.Runtime.InteropServices.Marshal.FreeHGlobal(hptr)

        data = Nothing

        Return hash

    End Function

    Public Function HashStringMD5(ByVal input As String) As String
        On Error Resume Next

        If input Is Nothing Then Return ""
        If input = "" Then Return ""

        Dim ba(input.Length - 1) As Byte

        For x As Integer = 0 To input.Length - 1
            ba(x) = CByte(Asc(input(x)))
        Next

        Dim data() As Byte = System.Security.Cryptography.MD5.Create().ComputeHash(ba)

        Dim hash As String = ""
        For x As Integer = 0 To data.Length - 1
            hash &= data(x).ToString("x2")
        Next

        data = Nothing

        Return hash

    End Function

    Public Function HashRectMD5BinaryThreshold(ByVal from_rect As Rectangle, ByRef bm As Bitmap, ByVal r As Integer, ByVal g As Integer, ByVal b As Integer) As String
        On Error Resume Next

        Dim x, y As Integer
        Dim c As Color

        Dim ar(from_rect.Width * from_rect.Height) As Byte
        Dim count As Integer = 0

        For y = from_rect.Y To from_rect.Y + from_rect.Height - 1
            For x = from_rect.X To from_rect.X + from_rect.Width - 1
                c = bm.GetPixel(x, y)
                If c.R >= r And c.G >= g And c.B >= b Then ar(count) = 255 Else ar(count) = 0
                count += 1
            Next x
        Next y

        Dim data() As Byte = System.Security.Cryptography.MD5.Create().ComputeHash(ar)

        Dim hash As String = ""
        For x = 0 To data.Length - 1
            hash &= data(x).ToString("x2")
        Next

        ar = Nothing
        data = Nothing

        Return hash

    End Function

    Public Function HashRectMD5BinaryThresholdOld(ByVal from_rect As Rectangle, ByRef bm As Bitmap, ByVal r As Integer, ByVal g As Integer, ByVal b As Integer) As String
        On Error Resume Next

        Dim x, y As Integer
        Dim c As Color
        Dim a As New ArrayList

        For y = from_rect.Y To from_rect.Y + from_rect.Height - 1
            For x = from_rect.X To from_rect.X + from_rect.Width - 1
                c = bm.GetPixel(x, y)
                If c.R >= r And c.G >= g And c.B >= b Then a.Add(CByte(255)) Else a.Add(CByte(0))
            Next x
        Next y

        Dim by As Byte

        Dim data() As Byte = System.Security.Cryptography.MD5.Create().ComputeHash(a.ToArray(by.GetType))

        Dim hash As String = ""
        For x = 0 To data.Length - 1
            hash &= data(x).ToString("x2")
        Next

        a = Nothing
        data = Nothing

        Return hash

    End Function

    Public Function HashRectMD5BinaryExact(ByVal from_rect As Rectangle, ByRef bm As Bitmap, ByVal r As Integer, ByVal g As Integer, ByVal b As Integer) As String
        On Error Resume Next

        Dim x, y As Integer
        Dim c As Color

        Dim ar(from_rect.Width * from_rect.Height) As Byte
        Dim count As Integer = 0

        For y = from_rect.Y To from_rect.Y + from_rect.Height - 1
            For x = from_rect.X To from_rect.X + from_rect.Width - 1
                c = bm.GetPixel(x, y)
                If c.R = r And c.G = g And c.B = b Then ar(count) = 255 Else ar(count) = 0
                count += 1
            Next x
        Next y

        Dim data() As Byte = System.Security.Cryptography.MD5.Create().ComputeHash(ar)

        Dim hash As String = ""
        For x = 0 To data.Length - 1
            hash &= data(x).ToString("x2")
        Next

        ar = Nothing
        data = Nothing

        Return hash

    End Function

    Public Function HashRectMD5Step(ByVal from_rect As Rectangle, ByRef bm As Bitmap, ByVal step_x As Integer, ByVal step_y As Integer) As String
        On Error Resume Next

        Static x, y As Integer
        Static c As Color
        Static b As Byte

        Dim a As New ArrayList

        For y = from_rect.Y To from_rect.Y + from_rect.Height - 1 Step step_y
            For x = from_rect.X To from_rect.X + from_rect.Width - 1 Step step_x
                c = bm.GetPixel(x, y) : a.Add(c.R) : a.Add(c.G) : a.Add(c.B)
            Next x
        Next y


        Dim data() As Byte = System.Security.Cryptography.MD5.Create().ComputeHash(a.ToArray(b.GetType))

        Dim hash As String = ""
        For x = 0 To data.Length - 1
            hash &= data(x).ToString("x2")
        Next

        a = Nothing
        data = Nothing

        Return hash

    End Function



    Private Function CreateDevMode() As DEVMODE
        Dim dm As New DEVMODE
        dm.dmDeviceName = New String(New Char(32) {})
        dm.dmFormName = New String(New Char(32) {})
        dm.dmSize = CShort(System.Runtime.InteropServices.Marshal.SizeOf(dm))
        Return dm
    End Function

    Public Sub ChangeResolution(ByVal width As Integer, ByVal height As Integer, ByVal freq As Integer)

        Const DM_PELSWIDTH As Integer = &H80000
        Const DM_PELSHEIGHT As Integer = &H100000
        Const DM_DISPLAYFREQUENCY As Integer = &H400000
        Const ENUM_CURRENT_SETTINGS As Integer = -1
        Dim DevM As DEVMODE = CreateDevMode()
        Dim enumResult As Integer
        Dim changeResult As DisplayChangeResultCode

        DevM.dmFields = DM_PELSWIDTH Or DM_PELSHEIGHT Or DM_DISPLAYFREQUENCY

        enumResult = EnumDisplaySettings(Nothing, ENUM_CURRENT_SETTINGS, DevM)

        DevM.dmPelsWidth = width
        DevM.dmPelsHeight = height
        DevM.dmDisplayFrequency = freq

        changeResult = CType(ChangeDisplaySettings(DevM, 0), DisplayChangeResultCode)

        If changeResult <> DisplayChangeResultCode.DISP_CHANGE_SUCCESSFUL Then
            Throw New Exception("Failed to change resolution: " & changeResult.ToString)
        End If

    End Sub


End Module