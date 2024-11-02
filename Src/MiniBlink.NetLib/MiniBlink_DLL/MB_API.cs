using System;
using System.Text;
using System.Runtime.InteropServices;

namespace MB
{
    #region 定义委托

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeTitleChangedCallback(IntPtr webView, IntPtr param, IntPtr title);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeMouseOverUrlChangedCallback(IntPtr webView, IntPtr param, IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeURLChangedCallback2(IntPtr webView, IntPtr param, IntPtr frame, IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkePaintUpdatedCallback(IntPtr webView, IntPtr param, IntPtr buffer, int x, int y, int cx, int cy);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkePaintBitUpdatedCallback(IntPtr webView, IntPtr param, IntPtr hdc, ref wkeRect r, int width, int height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeAlertBoxCallback(IntPtr webView, IntPtr param, IntPtr msg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate byte wkeConfirmBoxCallback(IntPtr webView, IntPtr param, IntPtr msg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate byte wkePromptBoxCallback(IntPtr webView, IntPtr param, IntPtr msg, IntPtr defaultResult, IntPtr result);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate byte wkeNavigationCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr wkeCreateViewCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url, IntPtr windowFeatures);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeDocumentReadyCallback(IntPtr webView, IntPtr param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeDocumentReady2Callback(IntPtr webView, IntPtr param, IntPtr frame);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeLoadingFinishCallback(IntPtr webView, IntPtr param, IntPtr url, wkeLoadingResult result, IntPtr failedReason);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate byte wkeDownloadCallback(IntPtr webView, IntPtr param, IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate byte wkeDownload2Callback(IntPtr webView, IntPtr param, uint expectedContentLength, IntPtr url, IntPtr mime, IntPtr disposition, IntPtr job, IntPtr dataBind);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeConsoleCallback(IntPtr webView, IntPtr param, wkeConsoleLevel level, IntPtr message, IntPtr sourceName, uint sourceLine, IntPtr stackTrace);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate byte wkeLoadUrlBeginCallback(IntPtr webView, IntPtr param, IntPtr url, IntPtr job);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeLoadUrlEndCallback(IntPtr webView, IntPtr param, IntPtr url, IntPtr job, IntPtr buf, int len);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeLoadUrlFailCallback(IntPtr webView, IntPtr param, IntPtr url, IntPtr job);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeDidCreateScriptContextCallback(IntPtr webView, IntPtr param, IntPtr frame, IntPtr context, int extensionGroup, int worldId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeWillReleaseScriptContextCallback(IntPtr webView, IntPtr param, IntPtr frame, IntPtr context, int worldId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate byte wkeNetResponseCallback(IntPtr webView, IntPtr param, IntPtr url, IntPtr job);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeWillMediaLoadCallback(IntPtr webView, IntPtr param, IntPtr url, IntPtr info);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeOnOtherLoadCallback(IntPtr webView, IntPtr param, wkeOtherLoadType type, IntPtr info);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate long wkeJsNativeFunction(IntPtr jsExecState, IntPtr param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeOnShowDevtoolsCallback(IntPtr webView, IntPtr param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeOnNetGetFaviconCallback(IntPtr webView, IntPtr param, IntPtr utf8Url, ref wkeMemBuf buf);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeNetJobDataRecvCallback(IntPtr ptr, IntPtr job, IntPtr data, int length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeNetJobDataFinishCallback(IntPtr ptr, IntPtr job, wkeLoadingResult result);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate long jsGetPropertyCallback(IntPtr es, long obj, string propertyName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate byte jsSetPropertyCallback(IntPtr es, long obj, string propertyName, long value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate long jsCallAsFunctionCallback(IntPtr es, long obj, IntPtr args, int argCount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void jsFinalizeCallback(IntPtr data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeOnUrlRequestWillRedirectCallback(IntPtr webView, IntPtr param, IntPtr oldRequest, IntPtr request, IntPtr redirectResponse);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeOnUrlRequestDidReceiveResponseCallback(IntPtr webView, IntPtr param, IntPtr request, IntPtr response);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeOnUrlRequestDidReceiveDataCallback(IntPtr webView, IntPtr param, IntPtr request, IntPtr data, int dataLength);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeOnUrlRequestDidFailCallback(IntPtr webView, IntPtr param, IntPtr request, IntPtr error);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeOnUrlRequestDidFinishLoadingCallback(IntPtr webView, IntPtr param, IntPtr request, long finishTime);

    /// <summary>
    /// 访问Cookie回调
    /// </summary>
    /// <param name="userData">用户数据</param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="domain">域名</param>
    /// <param name="path">路径</param>
    /// <param name="secure">安全，如果非0则仅发送到https请求</param>
    /// <param name="httpOnly">如果非0则仅发送到http请求</param>
    /// <param name="expires">过期时间 The cookie expiration date is only valid if |has_expires| is true.</param>
    /// <returns>返回true 则应用程序自己处理miniblink不处理</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool wkeCookieVisitor(IntPtr userData, [MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]string value, [MarshalAs(UnmanagedType.LPStr)]string domain, [MarshalAs(UnmanagedType.LPStr)]string path, int secure, int httpOnly, ref int expires);

    #endregion

    #region 枚举

    public enum wkeMouseFlags
    {
        WKE_LBUTTON = 0x01,
        WKE_RBUTTON = 0x02,
        WKE_SHIFT = 0x04,
        WKE_CONTROL = 0x08,
        WKE_MBUTTON = 0x10,
    }

    public enum wkeKeyFlags
    {
        WKE_EXTENDED = 0x0100,
        WKE_REPEAT = 0x4000,
    }

    public enum jsType
    {
        NUMBER,
        STRING,
        BOOLEAN,
        OBJECT,
        FUNCTION,
        UNDEFINED,
        ARRAY,
        NULL
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct jsKeys
    {
        public int length;
        public IntPtr keys;
    }

    public enum wkeConsoleLevel
    {
        Debug = 4,
        Log = 1,
        Info = 5,
        Warning = 2,
        Error = 3,
        RevokedError = 6,
    }

    public enum wkeLoadingResult
    {
        Succeeded,
        Failed,
        Canceled
    }

    public enum wkeNavigationType
    {
        LinkClick,
        FormSubmit,
        BackForward,
        ReLoad,
        FormReSubmit,
        Other
    }

    public enum wkeCursorStyle
    {
        Pointer,
        Cross,
        Hand,
        IBeam,
        Wait,
        Help,
        EastResize,
        NorthResize,
        NorthEastResize,
        NorthWestResize,
        SouthResize,
        SouthEastResize,
        SouthWestResize,
        WestResize,
        NorthSouthResize,
        EastWestResize,
        NorthEastSouthWestResize,
        NorthWestSouthEastResize,
        ColumnResize,
        RowResize,
        MiddlePanning,
        EastPanning,
        NorthPanning,
        NorthEastPanning,
        NorthWestPanning,
        SouthPanning,
        SouthEastPanning,
        SouthWestPanning,
        WestPanning,
        Move,
        VerticalText,
        Cell,
        ContextMenu,
        Alias,
        Progress,
        NoDrop,
        Copy,
        None,
        NotAllowed,
        ZoomIn,
        ZoomOut,
        Grab,
        Grabbing,
        Custom
    }

    public enum wkeCookieCommand
    {
        ClearAllCookies,
        ClearSessionCookies,
        FlushCookiesToFile,
        ReloadCookiesFromFile
    }

    public enum wkeProxyType
    {
        NONE,
        HTTP,
        SOCKS4,
        SOCKS4A,
        SOCKS5,
        SOCKS5HOSTNAME
    }

    public enum wkeSettingMask
    {
        PROXY = 1,
        PAINTCALLBACK_IN_OTHER_THREAD = 4,
    }

    public enum wkeOtherLoadType
    {
        WKE_DID_START_LOADING,
        WKE_DID_STOP_LOADING,
        WKE_DID_NAVIGATE,
        WKE_DID_NAVIGATE_IN_PAGE,
        WKE_DID_GET_RESPONSE_DETAILS,
        WKE_DID_GET_REDIRECT_REQUEST
    }

    public enum wkeResourceType
    {
        MAIN_FRAME = 0,       // top level page
        SUB_FRAME = 1,        // frame or iframe
        STYLESHEET = 2,       // a CSS stylesheet
        SCRIPT = 3,           // an external script
        IMAGE = 4,            // an image (jpg/gif/png/etc)
        FONT_RESOURCE = 5,    // a font
        SUB_RESOURCE = 6,     // an "other" subresource.
        OBJECT = 7,           // an object (or embed) tag for a plugin, or a resource that a plugin requested.
        MEDIA = 8,            // a media resource.
        WORKER = 9,           // the main resource of a dedicated worker.
        SHARED_WORKER = 10,   // the main resource of a shared worker.
        PREFETCH = 11,        // an explicitly requested prefetch
        FAVICON = 12,         // a favicon
        XHR = 13,             // a XMLHttpRequest
        PING = 14,            // a ping request for <a ping>
        SERVICE_WORKER = 15,  // the main resource of a service worker.
    }

    public enum wkeMenuItemId
    {
        kWkeMenuSelectedAllId = 1 << 1,
        kWkeMenuSelectedTextId = 1 << 2,
        kWkeMenuUndoId = 1 << 3,
        kWkeMenuCopyImageId = 1 << 4,
        kWkeMenuInspectElementAtId = 1 << 5,
        kWkeMenuCutId = 1 << 6,
        kWkeMenuPasteId = 1 << 7,
        kWkeMenuPrintId = 1 << 8,
        kWkeMenuGoForwardId = 1 << 9,
        kWkeMenuGoBackId = 1 << 10,
        kWkeMenuReloadId = 1 << 11,
    }

    public enum wkeRequestType
    {
        Invalidation,
        Get,
        Post,
        Put,
    }

    public enum wkeHttBodyElementType
    {
        wkeHttBodyElementTypeData,
        wkeHttBodyElementTypeFile
    }

    #endregion

    #region 结构体

    public struct jsData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string typeName;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public jsGetPropertyCallback propertyGet;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public jsSetPropertyCallback propertySet;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public jsFinalizeCallback finalize;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public jsCallAsFunctionCallback callAsFunction;
    }

    public struct wkeNetJobDataBind
    {
        IntPtr param;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public wkeNetJobDataRecvCallback recvCallback;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public wkeNetJobDataFinishCallback finishCallback;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct wkeRect
    {
        public int x;
        public int y;
        public int w;
        public int h;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct wkeProxy
    {
        public wkeProxyType Type;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string HostName;

        public ushort Port;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string UserName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string Password;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct wkeSettings
    {
        public wkeProxy Proxy;
        public wkeSettingMask Mask;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct wkeWindowFeatures
    {
        public int x;
        public int y;
        public int width;
        public int height;

        [MarshalAs(UnmanagedType.I1)]
        public bool menuBarVisible;

        [MarshalAs(UnmanagedType.I1)]
        public bool statusBarVisible;

        [MarshalAs(UnmanagedType.I1)]
        public bool toolBarVisible;

        [MarshalAs(UnmanagedType.I1)]
        public bool locationBarVisible;

        [MarshalAs(UnmanagedType.I1)]
        public bool scrollbarsVisible;

        [MarshalAs(UnmanagedType.I1)]
        public bool resizable;

        [MarshalAs(UnmanagedType.I1)]
        public bool fullscreen;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct wkeMediaLoadInfo
    {
        public int size;
        public int width;
        public int height;
        public double duration;
    }

    public struct wkeWillSendRequestInfo
    {
        public bool isHolded;
        public string url;
        public string newUrl;
        public wkeResourceType resourceType;
        public int httpResponseCode;
        public string method;
        public string referrer;
        public IntPtr headers;
    }

    public struct wkeTempCallbackInfo
    {
        public int size;
        public IntPtr frame;
        public IntPtr willSendRequestInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct wkeMemBuf
    {
        public int size;
        public IntPtr data;
        public int length;
    }

    public struct jsExceptionInfo
    {
        public string Message;
        public string SourceLine;
        public string ScriptResourceName;
        public int LineNumber;
        public int StartPosition;
        public int EndPosition;
        public int StartColumn;
        public int EndColoumn;
        public string CallStackString;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct wkeViewSettings
    {
        public int size;
        public uint bgColor;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct wkeSlist
    {
        public IntPtr str;
        public IntPtr next;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct wkePostBodyElement
    {
        public int size;
        public wkeHttBodyElementType type;
        public IntPtr data;
        public string filePath;
        public long fileStart;
        public long fileLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct wkePostBodyElements
    {
        public int size;
        public IntPtr element;
        public int elementSize;
        public bool isDirty;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct wkeUrlRequestCallbacks
    {
        wkeOnUrlRequestWillRedirectCallback willRedirectCallback;
        wkeOnUrlRequestDidReceiveResponseCallback didReceiveResponseCallback;
        wkeOnUrlRequestDidReceiveDataCallback didReceiveDataCallback;
        wkeOnUrlRequestDidFailCallback didFailCallback;
        wkeOnUrlRequestDidFinishLoadingCallback didFinishLoadingCallback;
    }

    #endregion

    public class MBApi
    {
        private const string ProjectPage = "原项目地址：https://gitee.com/ampereufo/MBforNETDemo";

        private const string m_strDll = "miniblink.dll";    // 编译64位的话，换成"miniblink_x64.dll"，或者把"miniblink_x64.dll"文件改名成"node.dll"也行

        [DllImport(m_strDll, EntryPoint = "wkeIsInitialize", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeIsInitialize();

        [DllImport(m_strDll, EntryPoint = "wkeInitialize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeInitialize();

        [DllImport(m_strDll, EntryPoint = "wkeInitializeEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeInitializeEx(wkeSettings settings);

        [DllImport(m_strDll, EntryPoint = "wkeSetViewSettings", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetViewSettings(IntPtr webView, wkeViewSettings settings);

        [DllImport(m_strDll, EntryPoint = "wkeConfigure", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeConfigure(wkeSettings settings);

        [DllImport(m_strDll, EntryPoint = "wkeSetDebugConfig", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void wkeSetDebugConfig(IntPtr webView, string debugString, [MarshalAs(UnmanagedType.LPArray)]byte[] param);

        [DllImport(m_strDll, EntryPoint = "wkeGetDebugConfig", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr wkeGetDebugConfig(IntPtr webView, string debugString);

        [DllImport(m_strDll, EntryPoint = "wkeGetVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint wkeGetVersion();

        [DllImport(m_strDll, EntryPoint = "wkeGetVersionString", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetVersionString();

        [DllImport(m_strDll, EntryPoint = "wkeGC", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeGC(IntPtr webView, int delayMs);

        [DllImport(m_strDll, EntryPoint = "wkeCreateWebView", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeCreateWebView();

        [DllImport(m_strDll, EntryPoint = "wkeGetWebView", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr wkeGetWebView(string name);

        [DllImport(m_strDll, EntryPoint = "wkeDestroyWebView", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeDestroyWebView(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeSetMemoryCacheEnable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetMemoryCacheEnable(IntPtr webView, [MarshalAs(UnmanagedType.I1)]bool b);

        [DllImport(m_strDll, EntryPoint = "wkeSetTouchEnabled", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetTouchEnabled(IntPtr webView, [MarshalAs(UnmanagedType.I1)]bool b);

        [DllImport(m_strDll, EntryPoint = "wkeSetNavigationToNewWindowEnable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetNavigationToNewWindowEnable(IntPtr webView, [MarshalAs(UnmanagedType.I1)]bool b);

        [DllImport(m_strDll, EntryPoint = "wkeSetCspCheckEnable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetCspCheckEnable(IntPtr webView, [MarshalAs(UnmanagedType.I1)]bool b);

        [DllImport(m_strDll, EntryPoint = "wkeSetNpapiPluginsEnabled", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetNpapiPluginsEnabled(IntPtr webView, [MarshalAs(UnmanagedType.I1)]bool b);

        [DllImport(m_strDll, EntryPoint = "wkeSetHeadlessEnabled", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetHeadlessEnabled(IntPtr webView, [MarshalAs(UnmanagedType.I1)]bool b);

        [DllImport(m_strDll, EntryPoint = "wkeSetMouseEnabled", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetMouseEnabled(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool b);

        [DllImport(m_strDll, EntryPoint = "wkeSetDragEnable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetDragEnable(IntPtr webView, [MarshalAs(UnmanagedType.I1)]bool b);

        [DllImport(m_strDll, EntryPoint = "wkeSetDragDropEnable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetDragDropEnable(IntPtr WebView, [MarshalAs(UnmanagedType.I1)]bool b);

        [DllImport(m_strDll, EntryPoint = "wkeSetContextMenuItemShow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetContextMenuItemShow(IntPtr WebView, wkeMenuItemId item, [MarshalAs(UnmanagedType.I1)]bool b);

        [DllImport(m_strDll, EntryPoint = "wkeSetLanguage", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void wkeSetLanguage(IntPtr WebView, string language);

        [DllImport(m_strDll, EntryPoint = "wkeSetViewNetInterface", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr wkeSetViewNetInterface(IntPtr webView, string netInterface);

        [DllImport(m_strDll, EntryPoint = "wkeSetProxy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetProxy(ref wkeProxy proxy);

        [DllImport(m_strDll, EntryPoint = "wkeSetViewProxy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetViewProxy(IntPtr webView, ref wkeProxy proxy);

        [DllImport(m_strDll, EntryPoint = "wkeSetWindowTitleW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeSetWindowTitle(IntPtr webView, string title);

        [DllImport(m_strDll, EntryPoint = "wkeGetName", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetName(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeSetName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void wkeSetName(IntPtr webView, string name);

        [DllImport(m_strDll, EntryPoint = "wkeSetHandle", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetHandle(IntPtr webView, IntPtr wndHandle);

        [DllImport(m_strDll, EntryPoint = "wkeSetHandleOffset", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetHandleOffset(IntPtr webView, int x, int y);

        [DllImport(m_strDll, EntryPoint = "wkeIsTransparent", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeIsTransparent(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeSetTransparent", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetTransparent(IntPtr webView, [MarshalAs(UnmanagedType.I1)]bool transparent);

        [DllImport(m_strDll, EntryPoint = "wkeSetUserAgentW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeSetUserAgentW(IntPtr webView, string userAgent);

        [DllImport(m_strDll, EntryPoint = "wkeLoadW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeLoadW(IntPtr webView, string url);

        [DllImport(m_strDll, EntryPoint = "wkeLoadURLW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeLoadURLW(IntPtr webView, string url);

        [DllImport(m_strDll, EntryPoint = "wkePostURLW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkePostURLW(IntPtr webView, string url, [MarshalAs(UnmanagedType.LPArray)]byte[] postData, int postLen);

        [DllImport(m_strDll, EntryPoint = "wkeLoadHTMLW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeLoadHTMLW(IntPtr webView, string html);

        [DllImport(m_strDll, EntryPoint = "wkeLoadFileW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeLoadFileW(IntPtr webView, string fileName);

        [DllImport(m_strDll, EntryPoint = "wkeGetURL", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetURL(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeIsLoading", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeIsLoading(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeIsLoadingFailed", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeIsLoadingFailed(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeIsLoadingCompleted", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeIsLoadingCompleted(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeIsDocumentReady", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeIsDocumentReady(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeStopLoading", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeStopLoading(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeReload", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeReload(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeGoToOffset", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeGoToOffset(IntPtr webView, int offset);

        [DllImport(m_strDll, EntryPoint = "wkeGoToIndex", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeGoToIndex(IntPtr webView, int index);

        [DllImport(m_strDll, EntryPoint = "wkeGetWebviewId", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetWebviewId(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeIsWebviewAlive", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeIsWebviewAlive(IntPtr webView, int id);

        [DllImport(m_strDll, EntryPoint = "wkeGetDocumentCompleteURL", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetDocumentCompleteURL(IntPtr webView, IntPtr frameId, [MarshalAs(UnmanagedType.LPArray)]byte[] partialURL);

        [DllImport(m_strDll, EntryPoint = "wkeCreateMemBuf", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeCreateMemBuf(IntPtr webView, [MarshalAs(UnmanagedType.LPArray)]byte[] buff, int length);

        [DllImport(m_strDll, EntryPoint = "wkeFreeMemBuf", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeFreeMemBuf(IntPtr buf);

        [DllImport(m_strDll, EntryPoint = "wkeGetTitleW", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetTitleW(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeResize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeResize(IntPtr webView, int w, int h);

        [DllImport(m_strDll, EntryPoint = "wkeGetWidth", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetWidth(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeGetHeight", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetHeight(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeGetContentWidth", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetContentWidth(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeGetContentHeight", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetContentHeight(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkePaint2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePaint2(IntPtr webView, IntPtr bits, int bufWid, int bufHei, int xDst, int yDst, int w, int h, int xSrc, int ySrc, [MarshalAs(UnmanagedType.I1)]bool bCopyAlpha);

        [DllImport(m_strDll, EntryPoint = "wkePaint", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePaint(IntPtr webView, IntPtr bits, int pitch);

        [DllImport(m_strDll, EntryPoint = "wkeGetViewDC", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetViewDC(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeGetHostHWND", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetHostHWND(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeCanGoBack", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeCanGoBack(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeGoBack", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeGoBack(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeCanGoForward", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeCanGoForward(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeGoForward", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeGoForward(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeEditorSelectAll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeEditorSelectAll(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeEditorUnSelect", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeEditorUnSelect(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeEditorCopy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorCopy(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeEditorCut", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorCut(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeEditorPaste", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorPaste(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeEditorDelete", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorDelete(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeEditorUndo", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorUndo(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeEditorRedo", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorRedo(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeGetCookieW", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetCookieW(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeSetCookie", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetCookie(IntPtr webView, [MarshalAs(UnmanagedType.LPArray)]byte[] url, [MarshalAs(UnmanagedType.LPArray)]byte[] cookie);

        [DllImport(m_strDll, EntryPoint = "wkeVisitAllCookie", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeVisitAllCookie(IntPtr webView, IntPtr usetData, wkeCookieVisitor visitor);

        [DllImport(m_strDll, EntryPoint = "wkePerformCookieCommand", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePerformCookieCommand(wkeCookieCommand command);

        [DllImport(m_strDll, EntryPoint = "wkeSetCookieEnabled", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetCookieEnabled(IntPtr webView, [MarshalAs(UnmanagedType.I1)]bool enable);

        [DllImport(m_strDll, EntryPoint = "wkeIsCookieEnabled", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeIsCookieEnabled(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeSetCookieJarPath", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeSetCookieJarPath(IntPtr webView, string path);

        [DllImport(m_strDll, EntryPoint = "wkeSetCookieJarFullPath", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeSetCookieJarFullPath(IntPtr webView, string path);

        [DllImport(m_strDll, EntryPoint = "wkeSetLocalStorageFullPath", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeSetLocalStorageFullPath(IntPtr webView, string path);

        [DllImport(m_strDll, EntryPoint = "wkeSetMediaVolume", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetMediaVolume(IntPtr webView, float volume);

        [DllImport(m_strDll, EntryPoint = "wkeGetMediaVolume", CallingConvention = CallingConvention.Cdecl)]
        public static extern float wkeGetMediaVolume(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeFireMouseEvent", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeFireMouseEvent(IntPtr webView, uint message, int x, int y, uint flags);

        [DllImport(m_strDll, EntryPoint = "wkeFireContextMenuEvent", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeFireContextMenuEvent(IntPtr webView, int x, int y, uint flags);

        [DllImport(m_strDll, EntryPoint = "wkeFireMouseWheelEvent", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeFireMouseWheelEvent(IntPtr webView, int x, int y, int delta, uint flags);

        [DllImport(m_strDll, EntryPoint = "wkeFireKeyUpEvent", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeFireKeyUpEvent(IntPtr webView, int virtualKeyCode, uint flags, [MarshalAs(UnmanagedType.I1)]bool systemKey);

        [DllImport(m_strDll, EntryPoint = "wkeFireKeyDownEvent", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeFireKeyDownEvent(IntPtr webView, int virtualKeyCode, uint flags, [MarshalAs(UnmanagedType.I1)]bool systemKey);

        [DllImport(m_strDll, EntryPoint = "wkeFireKeyPressEvent", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeFireKeyPressEvent(IntPtr webView, int charCode, uint flags, [MarshalAs(UnmanagedType.I1)]bool systemKey);

        [DllImport(m_strDll, EntryPoint = "wkeFireWindowsMessage", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeFireWindowsMessage(IntPtr webView, IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam, IntPtr result);

        [DllImport(m_strDll, EntryPoint = "wkeSetFocus", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeSetFocus(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeKillFocus", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeKillFocus(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeGetCaretRect", CallingConvention = CallingConvention.Cdecl)]
        public static extern wkeRect wkeGetCaretRect(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeRunJSW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern long wkeRunJSW(IntPtr webView, string script);

        [DllImport(m_strDll, EntryPoint = "wkeGlobalExec", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGlobalExec(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeSleep", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSleep(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeWake", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeWake(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeIsAwake", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeIsAwake(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeSetZoomFactor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetZoomFactor(IntPtr webView, float factor);

        [DllImport(m_strDll, EntryPoint = "wkeGetZoomFactor", CallingConvention = CallingConvention.Cdecl)]
        public static extern float wkeGetZoomFactor(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeSetEditable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetEditable(IntPtr webView, [MarshalAs(UnmanagedType.I1)]bool editable);

        [DllImport(m_strDll, EntryPoint = "wkeGetStringW", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetStringW(IntPtr wkeString);

        [DllImport(m_strDll, EntryPoint = "wkeSetStringW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeSetStringW(IntPtr wkeString, string str, int len);

        [DllImport(m_strDll, EntryPoint = "wkeCreateStringW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern IntPtr wkeCreateStringW(string str, int len);

        [DllImport(m_strDll, EntryPoint = "wkeDeleteString", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeDeleteString(IntPtr wkeString);

        [DllImport(m_strDll, EntryPoint = "wkeGetWebViewForCurrentContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetWebViewForCurrentContext();

        [DllImport(m_strDll, EntryPoint = "wkeSetUserKeyValue", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void wkeSetUserKeyValue(IntPtr webView, string key, IntPtr value);

        [DllImport(m_strDll, EntryPoint = "wkeGetUserKeyValue", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr wkeGetUserKeyValue(IntPtr webView, string key);

        [DllImport(m_strDll, EntryPoint = "wkeGetCursorInfoType", CallingConvention = CallingConvention.Cdecl)]
        public static extern wkeCursorStyle wkeGetCursorInfoType(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeSetCursorInfoType", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetCursorInfoType(IntPtr webView, wkeCursorStyle type);

        [DllImport(m_strDll, EntryPoint = "wkeSetDragFiles", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetDragFiles(IntPtr webView, IntPtr clintPos, IntPtr screenPos, [MarshalAs(UnmanagedType.LPArray)]IntPtr[] files, int filesCount);

        [DllImport(m_strDll, EntryPoint = "wkeOnMouseOverUrlChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnMouseOverUrlChanged(IntPtr webView, wkeMouseOverUrlChangedCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnTitleChanged", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnTitleChanged(IntPtr webView, wkeTitleChangedCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnURLChanged2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnURLChanged2(IntPtr webView, wkeURLChangedCallback2 callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnPaintUpdated", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnPaintUpdated(IntPtr webView, wkePaintUpdatedCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnPaintBitUpdated", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnPaintBitUpdated(IntPtr webView, wkePaintBitUpdatedCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnAlertBox", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnAlertBox(IntPtr webView, wkeAlertBoxCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnConfirmBox", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnConfirmBox(IntPtr webView, wkeConfirmBoxCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnPromptBox", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnPromptBox(IntPtr webView, wkePromptBoxCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnNavigation", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnNavigation(IntPtr webView, wkeNavigationCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnCreateView", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnCreateView(IntPtr webView, wkeCreateViewCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnDocumentReady", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDocumentReady(IntPtr webView, wkeDocumentReadyCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnDocumentReady2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDocumentReady2(IntPtr webView, wkeDocumentReady2Callback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnLoadingFinish", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnLoadingFinish(IntPtr webView, wkeLoadingFinishCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnDownload", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDownload(IntPtr webView, wkeDownloadCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnDownload2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDownload2(IntPtr webView, wkeDownload2Callback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnConsole", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnConsole(IntPtr webView, wkeConsoleCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnDidCreateScriptContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDidCreateScriptContext(IntPtr webView, wkeDidCreateScriptContextCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnWillReleaseScriptContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnWillReleaseScriptContext(IntPtr webView, wkeWillReleaseScriptContextCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnLoadUrlBegin", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnLoadUrlBegin(IntPtr webView, wkeLoadUrlBeginCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnLoadUrlEnd", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnLoadUrlEnd(IntPtr webView, wkeLoadUrlEndCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeOnLoadUrlFail", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnLoadUrlFail(IntPtr webView, wkeLoadUrlFailCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeNetOnResponse", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetOnResponse(IntPtr webView, wkeNetResponseCallback callback, IntPtr callbackParam);

        [DllImport(m_strDll, EntryPoint = "wkeNetSetMIMEType", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void wkeNetSetMIMEType(IntPtr job, string type);

        [DllImport(m_strDll, EntryPoint = "wkeNetSetHTTPHeaderField", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeNetSetHTTPHeaderField(IntPtr job, string key, string value, [MarshalAs(UnmanagedType.I1)]bool response);

        [DllImport(m_strDll, EntryPoint = "wkeNetGetHTTPHeaderField", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr wkeNetGetHTTPHeaderField(IntPtr job, string key);

        [DllImport(m_strDll, EntryPoint = "wkeNetGetHTTPHeaderFieldFromResponse", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr wkeNetGetHTTPHeaderFieldFromResponse(IntPtr job, string key);

        [DllImport(m_strDll, EntryPoint = "wkeNetSetData", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void wkeNetSetData(IntPtr job, [MarshalAs(UnmanagedType.LPArray)]byte[] buf, int len);

        [DllImport(m_strDll, EntryPoint = "wkeNetHookRequest", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetHookRequest(IntPtr job);

        [DllImport(m_strDll, EntryPoint = "wkeNetGetRequestMethod", CallingConvention = CallingConvention.Cdecl)]
        public static extern wkeRequestType wkeNetGetRequestMethod(IntPtr job);

        [DllImport(m_strDll, EntryPoint = "wkeNetGetMIMEType", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetGetMIMEType(IntPtr job, IntPtr mime);

        [DllImport(m_strDll, EntryPoint = "wkeNetContinueJob", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetContinueJob(IntPtr job);

        [DllImport(m_strDll, EntryPoint = "wkeNetGetUrlByJob", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetGetUrlByJob(IntPtr job);

        [DllImport(m_strDll, EntryPoint = "wkeNetGetRawHttpHead", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetGetRawHttpHead(IntPtr job);

        [DllImport(m_strDll, EntryPoint = "wkeNetGetRawResponseHead", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetGetRawResponseHead(IntPtr job);

        [DllImport(m_strDll, EntryPoint = "wkeNetCancelRequest", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetCancelRequest(IntPtr job);

        [DllImport(m_strDll, EntryPoint = "wkeNetHoldJobToAsynCommit", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeNetHoldJobToAsynCommit(IntPtr job);

        [DllImport(m_strDll, EntryPoint = "wkeNetChangeRequestUrl", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte wkeNetChangeRequestUrl(IntPtr job, string url);

        [DllImport(m_strDll, EntryPoint = "wkeNetCreateWebUrlRequest", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetCreateWebUrlRequest(IntPtr url, IntPtr method, IntPtr mime);

        [DllImport(m_strDll, EntryPoint = "wkeNetCreateWebUrlRequest2", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetCreateWebUrlRequest2(IntPtr request);

        [DllImport(m_strDll, EntryPoint = "wkeNetCopyWebUrlRequest", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetCopyWebUrlRequest(IntPtr job, [MarshalAs(UnmanagedType.I1)]bool needExtraData);

        [DllImport(m_strDll, EntryPoint = "wkeNetDeleteBlinkWebURLRequestPtr", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetDeleteBlinkWebURLRequestPtr(IntPtr request);

        [DllImport(m_strDll, EntryPoint = "wkeNetAddHTTPHeaderFieldToUrlRequest", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetAddHTTPHeaderFieldToUrlRequest(IntPtr request, IntPtr name, IntPtr value);

        [DllImport(m_strDll, EntryPoint = "wkeNetStartUrlRequest", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeNetStartUrlRequest(IntPtr webView, IntPtr request, IntPtr param, wkeUrlRequestCallbacks callback);

        [DllImport(m_strDll, EntryPoint = "wkeNetGetHttpStatusCode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeNetGetHttpStatusCode(IntPtr response);

        [DllImport(m_strDll, EntryPoint = "wkeNetGetExpectedContentLength", CallingConvention = CallingConvention.Cdecl)]
        public static extern long wkeNetGetExpectedContentLength(IntPtr response);

        [DllImport(m_strDll, EntryPoint = "wkeNetGetResponseUrl", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetGetResponseUrl(IntPtr response);

        [DllImport(m_strDll, EntryPoint = "wkeNetCancelWebUrlRequest", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetCancelWebUrlRequest(int requestId);

        [DllImport(m_strDll, EntryPoint = "wkeNetGetPostBody", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetGetPostBody(IntPtr job);

        [DllImport(m_strDll, EntryPoint = "wkeNetCreatePostBodyElements", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetCreatePostBodyElements(IntPtr webView, long length);

        [DllImport(m_strDll, EntryPoint = "wkeNetFreePostBodyElements", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetFreePostBodyElements(IntPtr elements);

        [DllImport(m_strDll, EntryPoint = "wkeNetCreatePostBodyElement", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetCreatePostBodyElement(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeNetFreePostBodyElement", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetFreePostBodyElement(IntPtr element);

        [DllImport(m_strDll, EntryPoint = "wkeIsMainFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeIsMainFrame(IntPtr webFrame);

        [DllImport(m_strDll, EntryPoint = "wkeIsWebRemoteFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeIsWebRemoteFrame(IntPtr webFrame);

        [DllImport(m_strDll, EntryPoint = "wkeWebFrameGetMainFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeWebFrameGetMainFrame(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeWebFrameGetMainWorldScriptContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeWebFrameGetMainWorldScriptContext(IntPtr webFrame, ref IntPtr contextOut);

        [DllImport(m_strDll, EntryPoint = "wkeGetBlinkMainThreadIsolate", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetBlinkMainThreadIsolate();

        [DllImport(m_strDll, EntryPoint = "wkeRunJsByFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern long wkeRunJsByFrame(IntPtr webView, IntPtr frameId, [MarshalAs(UnmanagedType.LPArray)]byte[] script, [MarshalAs(UnmanagedType.I1)]bool isInClosure);

        [DllImport(m_strDll, EntryPoint = "wkeGetWindowHandle", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetWindowHandle(IntPtr WebView);

        [DllImport(m_strDll, EntryPoint = "wkeOnWillMediaLoad", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnWillMediaLoad(IntPtr WebView, wkeWillMediaLoadCallback callback, IntPtr param);

        [DllImport(m_strDll, EntryPoint = "wkeDeleteWillSendRequestInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeDeleteWillSendRequestInfo(IntPtr WebView, IntPtr info);

        [DllImport(m_strDll, EntryPoint = "wkeOnOtherLoad", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnOtherLoad(IntPtr WebView, wkeOnOtherLoadCallback callback, IntPtr param);

        [DllImport(m_strDll, EntryPoint = "wkeSetDeviceParameter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void wkeSetDeviceParameter(IntPtr WebView, string device, string paramStr, int paramInt, float paramFloat);

        [DllImport(m_strDll, EntryPoint = "wkeAddPluginDirectory", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeAddPluginDirectory(IntPtr WebView, string path);

        [DllImport(m_strDll, EntryPoint = "wkeGetGlobalExecByFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetGlobalExecByFrame(IntPtr WebView, IntPtr frameId);

        [DllImport(m_strDll, EntryPoint = "wkeShowDevtools", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void wkeShowDevtools(IntPtr WebView, string path, wkeOnShowDevtoolsCallback callback, IntPtr param);

        [DllImport(m_strDll, EntryPoint = "wkeInsertCSSByFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeInsertCSSByFrame(IntPtr WebView, IntPtr frameId, [MarshalAs(UnmanagedType.LPArray)]byte[] utf8css);

        [DllImport(m_strDll, EntryPoint = "wkeSetResourceGc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetResourceGc(IntPtr WebView, int intervalSec);

        [DllImport(m_strDll, EntryPoint = "wkeLoadHtmlWithBaseUrl", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLoadHtmlWithBaseUrl(IntPtr WebView, [MarshalAs(UnmanagedType.LPArray)]byte[] utf8html, [MarshalAs(UnmanagedType.LPArray)]byte[] baseUrl);

        [DllImport(m_strDll, EntryPoint = "wkeGetUserAgent", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetUserAgent(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeGetFrameUrl", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetFrameUrl(IntPtr webView, IntPtr frameId);

        [DllImport(m_strDll, EntryPoint = "wkeNetGetFavicon", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeNetGetFavicon(IntPtr webView, wkeNetResponseCallback callback, IntPtr param);

        [DllImport(m_strDll, EntryPoint = "wkeIsProcessingUserGesture", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte wkeIsProcessingUserGesture(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeUtilSerializeToMHTML", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeUtilSerializeToMHTML(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeGetSource", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetSource(IntPtr webView);

        [DllImport(m_strDll, EntryPoint = "wkeJsBindFunction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void wkeJsBindFunction(string name, wkeJsNativeFunction fn, IntPtr param, uint argCount);

        [DllImport(m_strDll, EntryPoint = "wkeJsBindGetter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void wkeJsBindGetter(string name, wkeJsNativeFunction fn, IntPtr param);

        [DllImport(m_strDll, EntryPoint = "wkeJsBindSetter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void wkeJsBindSetter(string name, wkeJsNativeFunction fn, IntPtr param);

        [DllImport(m_strDll, EntryPoint = "jsArgCount", CallingConvention = CallingConvention.Cdecl)]
        public static extern int jsArgCount(IntPtr es);

        [DllImport(m_strDll, EntryPoint = "jsArgType", CallingConvention = CallingConvention.Cdecl)]
        public static extern jsType jsArgType(IntPtr es, int argIdx);

        [DllImport(m_strDll, EntryPoint = "jsArg", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsArg(IntPtr es, int argIdx);

        [DllImport(m_strDll, EntryPoint = "jsTypeOf", CallingConvention = CallingConvention.Cdecl)]
        public static extern jsType jsTypeOf(long v);

        [DllImport(m_strDll, EntryPoint = "jsIsNumber", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsIsNumber(long v);

        [DllImport(m_strDll, EntryPoint = "jsIsString", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsIsString(long v);

        [DllImport(m_strDll, EntryPoint = "jsIsBoolean", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsIsBoolean(long v);

        [DllImport(m_strDll, EntryPoint = "jsIsObject", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsIsObject(long v);

        [DllImport(m_strDll, EntryPoint = "jsIsFunction", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsIsFunction(long v);

        [DllImport(m_strDll, EntryPoint = "jsIsUndefined", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsIsUndefined(long v);

        [DllImport(m_strDll, EntryPoint = "jsIsNull", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsIsNull(long v);

        [DllImport(m_strDll, EntryPoint = "jsIsArray", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsIsArray(long v);

        [DllImport(m_strDll, EntryPoint = "jsIsTrue", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsIsTrue(long v);

        [DllImport(m_strDll, EntryPoint = "jsIsFalse", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsIsFalse(long v);

        [DllImport(m_strDll, EntryPoint = "jsToInt", CallingConvention = CallingConvention.Cdecl)]
        public static extern int jsToInt(IntPtr es, long v);

        [DllImport(m_strDll, EntryPoint = "jsToFloat", CallingConvention = CallingConvention.Cdecl)]
        public static extern float jsToFloat(IntPtr es, long v);

        [DllImport(m_strDll, EntryPoint = "jsToDouble", CallingConvention = CallingConvention.Cdecl)]
        public static extern double jsToDouble(IntPtr es, long v);

        [DllImport(m_strDll, EntryPoint = "jsToBoolean", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsToBoolean(IntPtr es, long v);

        [DllImport(m_strDll, EntryPoint = "jsToTempStringW", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr jsToTempStringW(IntPtr es, long v);

        [DllImport(m_strDll, EntryPoint = "jsInt", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsInt(int n);

        [DllImport(m_strDll, EntryPoint = "jsFloat", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsFloat(float f);

        [DllImport(m_strDll, EntryPoint = "jsDouble", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsDouble(double d);

        [DllImport(m_strDll, EntryPoint = "jsBoolean", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsBoolean(bool b);

        [DllImport(m_strDll, EntryPoint = "jsUndefined", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsUndefined();

        [DllImport(m_strDll, EntryPoint = "jsNull", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsNull();

        [DllImport(m_strDll, EntryPoint = "jsTrue", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsTrue();

        [DllImport(m_strDll, EntryPoint = "jsFalse", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsFalse();

        [DllImport(m_strDll, EntryPoint = "jsStringW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern long jsStringW(IntPtr es, string str);

        [DllImport(m_strDll, EntryPoint = "jsEmptyObject", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsEmptyObject(IntPtr es);

        [DllImport(m_strDll, EntryPoint = "jsEmptyArray", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsEmptyArray(IntPtr es);

        [DllImport(m_strDll, EntryPoint = "jsArrayBuffer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long jsArrayBuffer(IntPtr es, StringBuilder buffer, int size);

        [DllImport(m_strDll, EntryPoint = "jsObject", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsObject(IntPtr es, IntPtr obj);

        [DllImport(m_strDll, EntryPoint = "jsFunction", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsFunction(IntPtr es, IntPtr obj);

        [DllImport(m_strDll, EntryPoint = "jsGetData", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr jsGetData(IntPtr es, long jsValue);

        [DllImport(m_strDll, EntryPoint = "jsGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long jsGet(IntPtr es, long jsValue, string prop);

        [DllImport(m_strDll, EntryPoint = "jsSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void jsSet(IntPtr es, long jsValue, string prop, long v);

        [DllImport(m_strDll, EntryPoint = "jsGetAt", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsGetAt(IntPtr es, long jsValue, int index);

        [DllImport(m_strDll, EntryPoint = "jsSetAt", CallingConvention = CallingConvention.Cdecl)]
        public static extern void jsSetAt(IntPtr es, long jsValue, int index, long v);

        [DllImport(m_strDll, EntryPoint = "jsGetLength", CallingConvention = CallingConvention.Cdecl)]
        public static extern int jsGetLength(IntPtr es, long jsValue);

        [DllImport(m_strDll, EntryPoint = "jsSetLength", CallingConvention = CallingConvention.Cdecl)]
        public static extern void jsSetLength(IntPtr es, long jsValue, int length);

        [DllImport(m_strDll, EntryPoint = "jsGlobalObject", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsGlobalObject(IntPtr es);

        [DllImport(m_strDll, EntryPoint = "jsGetWebView", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr jsGetWebView(IntPtr es);

        [DllImport(m_strDll, EntryPoint = "jsEvalW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern long jsEvalW(IntPtr es, string str);

        [DllImport(m_strDll, EntryPoint = "jsEvalExW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern long jsEvalExW(IntPtr es, string str, [MarshalAs(UnmanagedType.I1)]bool isInClosure);

        [DllImport(m_strDll, EntryPoint = "jsCall", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsCall(IntPtr es, long func, long thisObject, [MarshalAs(UnmanagedType.LPArray)]Int64[] args, int argCount);

        [DllImport(m_strDll, EntryPoint = "jsCallGlobal", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsCallGlobal(IntPtr es, long func, [MarshalAs(UnmanagedType.LPArray)]Int64[] args, int argCount);

        [DllImport(m_strDll, EntryPoint = "jsGetGlobal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long jsGetGlobal(IntPtr es, string prop);

        [DllImport(m_strDll, EntryPoint = "jsSetGlobal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void jsSetGlobal(IntPtr es, string prop, long jsValue);

        [DllImport(m_strDll, EntryPoint = "jsGC", CallingConvention = CallingConvention.Cdecl)]
        public static extern void jsGC();

        [DllImport(m_strDll, EntryPoint = "jsIsJsValueValid", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsIsJsValueValid(IntPtr es, long jsValue);

        [DllImport(m_strDll, EntryPoint = "jsIsValidExecState", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte jsIsValidExecState(IntPtr es);

        [DllImport(m_strDll, EntryPoint = "jsDeleteObjectProp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void jsDeleteObjectProp(IntPtr es, long jsValue, string prop);

        [DllImport(m_strDll, EntryPoint = "jsGetArrayBuffer", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr jsGetArrayBuffer(IntPtr es, long jsValue);

        [DllImport(m_strDll, EntryPoint = "jsGetLastErrorIfException", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr jsGetLastErrorIfException(IntPtr es);

        [DllImport(m_strDll, EntryPoint = "jsThrowException", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsThrowException(IntPtr es, [MarshalAs(UnmanagedType.LPArray)]byte[] utf8exception);

        [DllImport(m_strDll, EntryPoint = "jsGetKeys", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr jsGetKeys(IntPtr es, long jsValue);
    }
}
