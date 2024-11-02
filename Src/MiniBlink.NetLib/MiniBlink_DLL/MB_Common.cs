using System;
using System.Text;
using System.Runtime.InteropServices;

namespace MB
{
    public delegate IntPtr WndProcCallback(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int cx;
        public int cy;
        public SIZE(int x, int y)
        {
            cx = x;
            cy = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT
    {
        public int hdc;
        public int fErase;
        public RECT rcPaint;
        public int fRestore;
        public int fIncUpdate;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] rgbReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COMPOSITIONFORM
    {
        public int dwStyle;
        public POINT ptCurrentPos;
        public RECT rcArea;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAP
    {
        public int bmType;
        public int bmWidth;
        public int bmHeight;
        public int bmWidthBytes;
        public short bmPlanes;
        public short bmBitsPixel;
        public int bmBits;
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct BLENDFUNCTION
    {
        [FieldOffset(0)]
        public byte BlendOp;

        [FieldOffset(1)]
        public byte BlendFlags;

        [FieldOffset(2)]
        public byte SourceConstantAlpha;

        [FieldOffset(3)]
        public byte AlphaFormat;
    }

    public enum WinConst : int
    {
        GWL_EXSTYLE = -20,
        GWL_WNDPROC = -4,
        WS_EX_LAYERED = 524288,
        WM_PAINT = 15,
        WM_ERASEBKGND = 20,
        WM_SIZE = 5,
        WM_KEYDOWN = 256,
        WM_KEYUP = 257,
        WM_CHAR = 258,
        WM_LBUTTONDOWN = 513,
        WM_LBUTTONUP = 514,
        WM_MBUTTONDOWN = 519,
        WM_RBUTTONDOWN = 516,
        WM_LBUTTONDBLCLK = 515,
        WM_MBUTTONDBLCLK = 521,
        WM_RBUTTONDBLCLK = 518,
        WM_MBUTTONUP = 520,
        WM_RBUTTONUP = 517,
        WM_MOUSEMOVE = 512,
        WM_CONTEXTMENU = 123,
        WM_MOUSEWHEEL = 522,
        WM_SETFOCUS = 7,
        WM_KILLFOCUS = 8,
        WM_IME_STARTCOMPOSITION = 269,
        WM_NCHITTEST = 132,
        WM_GETMINMAXINFO = 36,
        WM_DESTROY = 2,
        WM_SETCURSOR = 32,
        MK_CONTROL = 8,
        MK_SHIFT = 4,
        MK_LBUTTON = 1,
        MK_MBUTTON = 16,
        MK_RBUTTON = 2,
        KF_REPEAT = 16384,
        KF_EXTENDED = 256,
        SRCCOPY = 13369376,
        CAPTUREBLT = 1073741824,
        CFS_POINT = 2,
        CFS_FORCE_POSITION = 32,
        OBJ_BITMAP = 7,
        AC_SRC_OVER = 0,
        AC_SRC_ALPHA = 1,
        ULW_ALPHA = 2,
        WM_INPUTLANGCHANGE = 81,
        WM_NCDESTROY = 130,
        WM_SYSCOMMAND = 274,
        CS_DROPSHADOW = 0x00020000,
    }


    public static class MB_Common
    {
        [DllImport("user32.dll", EntryPoint = "GetWindowLongW")]
        public static extern int GetWindowLong32(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        public static extern int GetWindowLong64(IntPtr hwnd, int nIndex);

        public static int GetWindowLong(IntPtr hwnd, int nIndex)
        {
            return IntPtr.Size == 8 ? GetWindowLong64(hwnd, nIndex) : GetWindowLong32(hwnd, nIndex);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongW")]
        public static extern int SetWindowLong32(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern int SetWindowLong64(IntPtr hwnd, int nIndex, long dwNewLong);

        public static int SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong)
        {
            return IntPtr.Size == 8 ? SetWindowLong64(hwnd, nIndex, dwNewLong) : SetWindowLong32(hwnd, nIndex, (int)dwNewLong);
        }

        [DllImport("user32.dll", EntryPoint = "CallWindowProcW")]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "DefWindowProcA")]
        public static extern IntPtr DefWindowProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetClientRect")]
        public static extern int GetClientRect(IntPtr hwnd, ref RECT lpRect);

        [DllImport("user32.dll", EntryPoint = "BeginPaint")]
        public static extern IntPtr BeginPaint(IntPtr hwnd, ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll", EntryPoint = "IntersectRect")]
        public static extern int IntersectRect(ref RECT lpDestRect, ref RECT lpSrc1Rect, ref RECT lpSrc2Rect);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern int BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("user32.dll", EntryPoint = "EndPaint")]
        public static extern int EndPaint(IntPtr hwnd, ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll", EntryPoint = "GetFocus")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll", EntryPoint = "SetFocus")]
        public static extern IntPtr SetFocus(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "SetCapture")]
        public static extern int SetCapture(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        public static extern int ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "ScreenToClient")]
        public static extern int ScreenToClient(IntPtr hwnd, ref POINT lpPoint);

        [DllImport("imm32.dll", EntryPoint = "ImmGetContext")]
        public static extern IntPtr ImmGetContext(IntPtr hwnd);

        [DllImport("imm32.dll", EntryPoint = "ImmSetCompositionWindow")]
        public static extern int ImmSetCompositionWindow(IntPtr himc, ref COMPOSITIONFORM lpCompositionForm);

        [DllImport("imm32.dll", EntryPoint = "ImmReleaseContext")]
        public static extern int ImmReleaseContext(IntPtr hwnd, IntPtr himc);

        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        public static extern int GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        [DllImport("user32.dll", EntryPoint = "OffsetRect")]
        public static extern int OffsetRect(ref RECT lpRect, int x, int y);

        [DllImport("gdi32.dll", EntryPoint = "GetCurrentObject")]
        public static extern IntPtr GetCurrentObject(IntPtr hdc, int uObjectType);

        [DllImport("gdi32.dll", EntryPoint = "GetObjectW")]
        public static extern int GetObject(IntPtr hObject, int nCount, ref BITMAP lpObject);

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "UpdateLayeredWindow")]
        public static extern int UpdateLayeredWindow(IntPtr hWnd, IntPtr hdcDst, IntPtr pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pptSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern int DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern int DeleteDC(IntPtr hdc);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("user32.dll", EntryPoint = "InvalidateRect")]
        public static extern int InvalidateRect(IntPtr hwnd, ref RECT lpRect, bool bErase);

        [DllImport("kernel32.dll", EntryPoint = "lstrlenA")]
        public static extern int lstrlen(IntPtr lpString);

        [DllImport("kernel32.dll", EntryPoint = "lstrlenA")]
        public static extern int lstrlenA(IntPtr lpString);

        [DllImport("kernel32.dll", EntryPoint = "lstrlenW")]
        public static extern int lstrlenW(IntPtr lpString);

        [DllImport("user32.dll ", EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public static ulong LOWORD(this IntPtr dword)
        {
            return IntPtr.Size == 4 ? (ulong)dword & 65535 : (ulong)dword & 4294967295;
        }

        public static ulong HIWORD(this IntPtr dword)
        {
            return IntPtr.Size == 4 ? (ulong)dword >> 16 : (ulong)dword >> 32;
        }

        public static int LOWORD(this int dword)
        {
            return dword & 65535;
        }

        public static int HIWORD(this int dword)
        {
            return dword >> 16;
        }

        public static int To32(this IntPtr dword)
        {
            return IntPtr.Size == 8 ? (int)(dword.ToInt64() << 32 >> 32) : dword.ToInt32();
        }

        public static string UTF8PtrToStr(this IntPtr utf8)
        {
            if (utf8 == IntPtr.Zero)
            {
                return string.Empty;
            }

            int iLen = lstrlenA(utf8);
            byte[] bytes = new byte[iLen];
            Marshal.Copy(utf8, bytes, 0, iLen);

            return Encoding.UTF8.GetString(bytes);
        }

        public static IntPtr StrToUtf8Ptr(this string str)
        {
            IntPtr ptr = IntPtr.Zero;

            if (str != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                ptr = Marshal.AllocHGlobal(bytes.Length + 1);
                Marshal.Copy(bytes, 0, ptr, bytes.Length);
                Marshal.WriteByte(ptr, bytes.Length, 0);
            }

            return ptr;
        }

        public static string UnicodePtrToStr(this IntPtr unicode)
        {
            /*if (unicode == IntPtr.Zero)
            {
                return string.Empty;
            }

            int iLen = lstrlenW(unicode);
            char[] ch = new char[iLen];
            Marshal.Copy(unicode, ch, 0, iLen);

            return new string(ch);*/

            return Marshal.PtrToStringUni(unicode);
        }

        public static IntPtr StrToUnicodePtr(this string str)
        {
            IntPtr ptr = IntPtr.Zero;

            if (str != null)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(str);
                ptr = Marshal.AllocHGlobal(bytes.Length + 1);
                Marshal.Copy(bytes, 0, ptr, bytes.Length);
                Marshal.WriteByte(ptr, bytes.Length, 0);
            }

            return ptr;
        }

        public static byte[] StructToBytes(this object structObj)
        {
            int iSize = Marshal.SizeOf(structObj);
            byte[] bytes = new byte[iSize];
            IntPtr structPtr = Marshal.AllocHGlobal(iSize);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, iSize);
            Marshal.FreeHGlobal(structPtr);

            return bytes;
        }

        public static object BytesToStuct(this byte[] bytes, Type type)
        {
            object objRet = null;

            int iSize = Marshal.SizeOf(type);
            if (iSize <= bytes.Length)
            {
                IntPtr structPtr = Marshal.AllocHGlobal(iSize);
                Marshal.Copy(bytes, 0, structPtr, iSize);
                objRet = Marshal.PtrToStructure(structPtr, type);
                Marshal.FreeHGlobal(structPtr);
            }

            return objRet;
        }

        public static IntPtr StructToUTF8Ptr(this object structObj)
        {
            int iSize = Marshal.SizeOf(structObj);
            byte[] bytes = new byte[iSize];
            IntPtr structPtr = Marshal.AllocHGlobal(iSize);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, iSize);

            return structPtr;
        }

        public static object UTF8PtrToStruct(this IntPtr structPtr, Type type)
        {
            return Marshal.PtrToStructure(structPtr, type);
        }

        public static byte[] UTF8PtrToByte(this IntPtr utf8)
        {
            if (utf8 == IntPtr.Zero)
            {
                return new byte[0];
            }

            int iLen = lstrlenA(utf8);
            byte[] bytes = new byte[iLen];
            Marshal.Copy(utf8, bytes, 0, iLen);

            return bytes;
        }

        public static IntPtr ByteToUtf8Ptr(this byte[] data)
        {
            IntPtr ptr = IntPtr.Zero;

            if (data != null)
            {
                ptr = Marshal.AllocHGlobal(data.Length + 1);
                Marshal.Copy(data, 0, ptr, data.Length);
                Marshal.WriteByte(ptr, data.Length, 0);
            }

            return ptr;
        }

        public static string[] PtrToStringArray(this IntPtr ptr, int iLength)
        {
            string[] data = new string[iLength];

            for (int i = 0; i < iLength; i++)
            {
                IntPtr str = (IntPtr)Marshal.PtrToStructure(ptr, typeof(IntPtr));
                data[i] = Marshal.PtrToStringAnsi(str);
                ptr = new IntPtr(ptr.ToInt64() + IntPtr.Size);
            }

            return data;
        }

        public static long ToLong(this DateTime time)
        {
            DateTime now = time.ToUniversalTime();
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (now.Ticks - start.Ticks) / 10000;
        }

        public static DateTime ToDT(this long time)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            time = start.Ticks + time * 10000;

            return new DateTime(time, DateTimeKind.Utc).ToLocalTime();
        }
    }
}
