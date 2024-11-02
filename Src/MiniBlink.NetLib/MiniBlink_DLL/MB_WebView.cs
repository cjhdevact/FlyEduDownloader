using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MB
{
    public class WebView : IDisposable
    {
        private IntPtr m_hWnd;
        private IntPtr m_OldProc;
        private wkePaintUpdatedCallback m_wkePaintUpdatedCallback;
        private WndProcCallback m_WndProcCallback;
        private bool m_bMouseEnabled;
        private bool m_bTransparent;

        #region 设置回调事件

        private wkeTitleChangedCallback m_wkeTitleChangedCallback;
        private wkeMouseOverUrlChangedCallback m_wkeMouseOverUrlChangedCallback;
        private wkeURLChangedCallback2 m_wkeURLChangedCallback2;
        private wkeAlertBoxCallback m_wkeAlertBoxCallback;
        private wkeConfirmBoxCallback m_wkeConfirmBoxCallback;
        private wkePromptBoxCallback m_wkePromptBoxCallback;
        private wkeNavigationCallback m_wkeNavigationCallback;
        private wkeCreateViewCallback m_wkeCreateViewCallback;
        private wkeDocumentReady2Callback m_wkeDocumentReadyCallback;
        private wkeLoadingFinishCallback m_wkeLoadingFinishCallback;
        private wkeDownloadCallback m_wkeDownloadCallback;
        private wkeDownload2Callback m_wkeDownloadCallback2;
        private wkeConsoleCallback m_wkeConsoleCallback;
        private wkeLoadUrlBeginCallback m_wkeLoadUrlBeginCallback;
        private wkeLoadUrlEndCallback m_wkeLoadUrlEndCallback;
        private wkeLoadUrlFailCallback m_wkeLoadUrlFailCallback;
        private wkeDidCreateScriptContextCallback m_wkeDidCreateScriptContextCallback;
        private wkeWillReleaseScriptContextCallback m_wkeWillReleaseScriptContextCallback;
        private wkeNetResponseCallback m_wkeNetResponseCallback;
        private wkeWillMediaLoadCallback m_wkeWillMediaLoadCallback;
        private wkeOnOtherLoadCallback m_wkeOnOtherLoadCallback;

        private event EventHandler<TitleChangeEventArgs> m_titleChangeHandler = null;
        private event EventHandler<MouseOverUrlChangedEventArgs> m_mouseOverUrlChangedHandler = null;
        private event EventHandler<UrlChangeEventArgs> m_urlChangeHandler = null;
        private event EventHandler<AlertBoxEventArgs> m_alertBoxHandler = null;
        private event EventHandler<ConfirmBoxEventArgs> m_confirmBoxHandler = null;
        private event EventHandler<PromptBoxEventArgs> m_promptBoxHandler = null;
        private event EventHandler<NavigateEventArgs> m_navigateHandler = null;
        private event EventHandler<CreateViewEventArgs> m_createViewHandler = null;
        private event EventHandler<DocumentReadyEventArgs> m_documentReadyHandler = null;
        private event EventHandler<LoadingFinishEventArgs> m_loadingFinishHandler = null;
        private event EventHandler<DownloadEventArgs> m_downloadHandler = null;
        private event EventHandler<DownloadEventArgs2> m_downloadHandler2 = null;
        private event EventHandler<ConsoleEventArgs> m_consoleHandler = null;
        private event EventHandler<LoadUrlBeginEventArgs> m_loadUrlBeginHandler = null;
        private event EventHandler<LoadUrlEndEventArgs> m_loadUrlEndHandler = null;
        private event EventHandler<LoadUrlFailEventArgs> m_loadUrlFailHandler = null;
        private event EventHandler<DidCreateScriptContextEventArgs> m_didCreateScriptContextHandler = null;
        private event EventHandler<WillReleaseScriptContextEventArgs> m_willReleaseScriptContextHandler = null;
        private event EventHandler<NetResponseEventArgs> m_netResponseHandler = null;
        private event EventHandler<WillMediaLoadEventArgs> m_willMediaLoadHandler = null;
        private event EventHandler<OtherLoadEventArgs> m_OtherLoadHandler = null;

        private void SetCallback()
        {
            m_wkeTitleChangedCallback = new wkeTitleChangedCallback((IntPtr WebView, IntPtr param, IntPtr title) =>
            {
                m_titleChangeHandler?.Invoke(this, new TitleChangeEventArgs(WebView, title));

            });

            m_wkeMouseOverUrlChangedCallback = new wkeMouseOverUrlChangedCallback((IntPtr WebView, IntPtr param, IntPtr url) =>
            {
                m_titleChangeHandler?.Invoke(this, new TitleChangeEventArgs(WebView, url));

            });

            m_wkeURLChangedCallback2 = new wkeURLChangedCallback2((IntPtr WebView, IntPtr param, IntPtr frame, IntPtr url) =>
            {
                m_urlChangeHandler?.Invoke(this, new UrlChangeEventArgs(WebView, url, frame));

            });

            m_wkeAlertBoxCallback = new wkeAlertBoxCallback((IntPtr WebView, IntPtr param, IntPtr msg) =>
            {
                m_alertBoxHandler?.Invoke(this, new AlertBoxEventArgs(WebView, msg));
            });

            m_wkeConfirmBoxCallback = new wkeConfirmBoxCallback((IntPtr WebView, IntPtr param, IntPtr msg) =>
            {
                if (m_confirmBoxHandler != null)
                {
                    ConfirmBoxEventArgs e = new ConfirmBoxEventArgs(WebView, msg);
                    m_confirmBoxHandler(this, e);
                    return Convert.ToByte(e.Result);
                }
                return 0;
            });

            m_wkePromptBoxCallback = new wkePromptBoxCallback((IntPtr webView, IntPtr param, IntPtr msg, IntPtr defaultResult, IntPtr result) =>
            {
                if (m_promptBoxHandler != null)
                {
                    PromptBoxEventArgs e = new PromptBoxEventArgs(webView, msg, defaultResult, result);
                    m_promptBoxHandler(this, e);
                    return Convert.ToByte(e.Result);
                }
                return 0;
            });

            m_wkeNavigationCallback = new wkeNavigationCallback((IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url) =>
            {
                if (m_navigateHandler != null)
                {
                    NavigateEventArgs e = new NavigateEventArgs(webView, navigationType, url);
                    m_navigateHandler(this, e);

                    return (byte)(e.Cancel ? 0 : 1);
                }
                return 1;
            });

            m_wkeCreateViewCallback = new wkeCreateViewCallback((IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url, IntPtr windowFeatures) =>
            {
                if (m_createViewHandler != null)
                {
                    CreateViewEventArgs e = new CreateViewEventArgs(webView, navigationType, url, windowFeatures);
                    m_createViewHandler(this, e);

                    return e.NewWebViewHandle;
                }
                return webView;
            });

            m_wkeDocumentReadyCallback = new wkeDocumentReady2Callback((IntPtr webView, IntPtr param, IntPtr frame) =>
            {
                m_documentReadyHandler?.Invoke(this, new DocumentReadyEventArgs(webView, frame));
            });

            m_wkeLoadingFinishCallback = new wkeLoadingFinishCallback((IntPtr webView, IntPtr param, IntPtr url, wkeLoadingResult result, IntPtr failedReason) =>
            {
                m_loadingFinishHandler?.Invoke(this, new LoadingFinishEventArgs(webView, url, result, failedReason));
            });

            m_wkeDownloadCallback = new wkeDownloadCallback((IntPtr webView, IntPtr param, IntPtr url) =>
            {
                if (m_downloadHandler != null)
                {
                    DownloadEventArgs e = new DownloadEventArgs(webView, url);
                    m_downloadHandler(this, e);
                    return (byte)(e.Cancel ? 1 : 0);
                }
                return 1;
            });

            m_wkeDownloadCallback2 = new wkeDownload2Callback((IntPtr webView, IntPtr param, uint expectedContentLength, IntPtr url, IntPtr mime, IntPtr disposition, IntPtr job, IntPtr dataBind) =>
            {
                if (m_downloadHandler2 != null)
                {
                    DownloadEventArgs2 e = new DownloadEventArgs2(webView, param, expectedContentLength, url, mime, disposition, job, dataBind);
                    m_downloadHandler2(this, e);
                    return (byte)(e.Cancel ? 1 : 0);
                }
                return 1;
            });

            m_wkeConsoleCallback = new wkeConsoleCallback((IntPtr webView, IntPtr param, wkeConsoleLevel level, IntPtr message, IntPtr sourceName, uint sourceLine, IntPtr stackTrace) =>
            {
                m_consoleHandler?.Invoke(this, new ConsoleEventArgs(webView, level, message, sourceName, sourceLine, stackTrace));
            });

            m_wkeLoadUrlBeginCallback = new wkeLoadUrlBeginCallback((IntPtr webView, IntPtr param, IntPtr url, IntPtr job) =>
            {
                if (m_loadUrlBeginHandler != null)
                {
                    LoadUrlBeginEventArgs e = new LoadUrlBeginEventArgs(webView, url, job);
                    m_loadUrlBeginHandler(this, e);
                    return (byte)(e.Cancel ? 1 : 0);
                }
                return 0;
            });

            m_wkeLoadUrlEndCallback = new wkeLoadUrlEndCallback((IntPtr webView, IntPtr param, IntPtr url, IntPtr job, IntPtr buf, int len) =>
            {
                m_loadUrlEndHandler?.Invoke(this, new LoadUrlEndEventArgs(webView, url, job, buf, len));
            });

            m_wkeDidCreateScriptContextCallback = new wkeDidCreateScriptContextCallback((IntPtr webView, IntPtr param, IntPtr frame, IntPtr context, int extensionGroup, int worldId) =>
            {
                m_didCreateScriptContextHandler?.Invoke(this, new DidCreateScriptContextEventArgs(webView, frame, context, extensionGroup, worldId));
            });

            m_wkeWillReleaseScriptContextCallback = new wkeWillReleaseScriptContextCallback((IntPtr webView, IntPtr param, IntPtr frame, IntPtr context, int worldId) =>
            {
                m_willReleaseScriptContextHandler?.Invoke(this, new WillReleaseScriptContextEventArgs(webView, frame, context, worldId));
            });

            m_wkeNetResponseCallback = new wkeNetResponseCallback((IntPtr WebView, IntPtr param, IntPtr url, IntPtr job) => {
                if (m_netResponseHandler != null)
                {
                    NetResponseEventArgs e = new NetResponseEventArgs(WebView, url, job);
                    m_netResponseHandler(this, e);
                    return (byte)(e.Cancel ? 1 : 0);
                }
                return 0;
            });

            m_wkeWillMediaLoadCallback = new wkeWillMediaLoadCallback((IntPtr webView, IntPtr param, IntPtr url, IntPtr info) =>
            {
                m_willMediaLoadHandler?.Invoke(this, new WillMediaLoadEventArgs(webView, url, info));
            });

            m_wkeOnOtherLoadCallback = new wkeOnOtherLoadCallback((IntPtr webView, IntPtr param, wkeOtherLoadType type, IntPtr info) =>
            {
                m_OtherLoadHandler?.Invoke(this, new OtherLoadEventArgs(webView, type, info));
            });
        }

        /// <summary>
        /// 窗口过程事件
        /// </summary>
        public event EventHandler<WindowProcEventArgs> OnWindowProc;

        /// <summary>
        /// 鼠标经过URL改变事件
        /// </summary>
        public event EventHandler<MouseOverUrlChangedEventArgs> OnMouseoverUrlChange
        {
            add
            {
                if (m_mouseOverUrlChangedHandler == null)
                {
                    MBApi.wkeOnMouseOverUrlChanged(Handle, m_wkeMouseOverUrlChangedCallback, IntPtr.Zero);
                }
                m_mouseOverUrlChangedHandler += value;
            }
            remove
            {
                m_mouseOverUrlChangedHandler -= value;
                if (m_mouseOverUrlChangedHandler == null)
                {
                    MBApi.wkeOnMouseOverUrlChanged(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 标题被改变事件
        /// </summary>
        public event EventHandler<TitleChangeEventArgs> OnTitleChange
        {
            add
            {
                if (m_titleChangeHandler == null)
                {
                    MBApi.wkeOnTitleChanged(Handle, m_wkeTitleChangedCallback, IntPtr.Zero);
                }
                m_titleChangeHandler += value;
            }
            remove
            {
                m_titleChangeHandler -= value;
                if (m_titleChangeHandler == null)
                {
                    MBApi.wkeOnTitleChanged(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// URL被改变事件
        /// </summary>
        public event EventHandler<UrlChangeEventArgs> OnURLChange
        {
            add
            {
                if (m_urlChangeHandler == null)
                {
                    MBApi.wkeOnURLChanged2(Handle, m_wkeURLChangedCallback2, IntPtr.Zero);
                }
                m_urlChangeHandler += value;
            }
            remove
            {
                m_urlChangeHandler -= value;
                if (m_urlChangeHandler == null)
                {
                    MBApi.wkeOnURLChanged2(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// alert被调用事件
        /// </summary>
        public event EventHandler<AlertBoxEventArgs> OnAlertBox
        {
            add
            {
                if (m_alertBoxHandler == null)
                {
                    MBApi.wkeOnAlertBox(Handle, m_wkeAlertBoxCallback, IntPtr.Zero);
                }
                m_alertBoxHandler += value;
            }
            remove
            {
                m_alertBoxHandler -= value;
                if (m_alertBoxHandler == null)
                {
                    MBApi.wkeOnAlertBox(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// confirm被调用事件
        /// </summary>
        public event EventHandler<ConfirmBoxEventArgs> OnConfirmBox
        {
            add
            {
                if (m_confirmBoxHandler == null)
                {
                    MBApi.wkeOnConfirmBox(Handle, m_wkeConfirmBoxCallback, IntPtr.Zero);
                }
                m_confirmBoxHandler += value;
            }
            remove
            {
                m_confirmBoxHandler -= value;
                if (m_confirmBoxHandler == null)
                {
                    MBApi.wkeOnConfirmBox(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// prompt被调用事件
        /// </summary>
        public event EventHandler<PromptBoxEventArgs> OnPromptBox
        {
            add
            {
                if (m_promptBoxHandler == null)
                {
                    MBApi.wkeOnPromptBox(Handle, m_wkePromptBoxCallback, IntPtr.Zero);
                }
                m_promptBoxHandler += value;
            }
            remove
            {
                m_promptBoxHandler -= value;
                if (m_promptBoxHandler == null)
                {
                    MBApi.wkeOnPromptBox(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 导航事件
        /// </summary>
        public event EventHandler<NavigateEventArgs> OnNavigate
        {
            add
            {
                if (m_navigateHandler == null)
                {
                    MBApi.wkeOnNavigation(Handle, m_wkeNavigationCallback, IntPtr.Zero);
                }
                m_navigateHandler += value;
            }
            remove
            {
                m_navigateHandler -= value;
                if (m_navigateHandler == null)
                {
                    MBApi.wkeOnNavigation(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 将创建新窗口
        /// </summary>
        public event EventHandler<CreateViewEventArgs> OnCreateView
        {
            add
            {
                if (m_createViewHandler == null)
                {
                    MBApi.wkeOnCreateView(Handle, m_wkeCreateViewCallback, IntPtr.Zero);
                }
                m_createViewHandler += value;
            }
            remove
            {
                m_createViewHandler -= value;
                if (m_createViewHandler == null)
                {
                    MBApi.wkeOnCreateView(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 文档就绪
        /// </summary>
        public event EventHandler<DocumentReadyEventArgs> OnDocumentReady
        {
            add
            {
                if (m_documentReadyHandler == null)
                {
                    MBApi.wkeOnDocumentReady2(Handle, m_wkeDocumentReadyCallback, IntPtr.Zero);
                }
                m_documentReadyHandler += value;
            }
            remove
            {
                m_documentReadyHandler -= value;
                if (m_documentReadyHandler == null)
                {
                    MBApi.wkeOnDocumentReady2(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 载入完成
        /// </summary>
        public event EventHandler<LoadingFinishEventArgs> OnLoadingFinish
        {
            add
            {
                if (m_loadingFinishHandler == null)
                {
                    MBApi.wkeOnLoadingFinish(Handle, m_wkeLoadingFinishCallback, IntPtr.Zero);
                }
                m_loadingFinishHandler += value;
            }
            remove
            {
                m_loadingFinishHandler -= value;
                if (m_loadingFinishHandler == null)
                {
                    MBApi.wkeOnLoadingFinish(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        public event EventHandler<DownloadEventArgs> OnDownload
        {
            add
            {
                if (m_downloadHandler == null)
                {
                    MBApi.wkeOnDownload(Handle, m_wkeDownloadCallback, IntPtr.Zero);
                }
                m_downloadHandler += value;
            }
            remove
            {
                m_downloadHandler -= value;
                if (m_downloadHandler == null)
                {
                    MBApi.wkeOnDownload(Handle, null, IntPtr.Zero);
                }
            }
        }

        public event EventHandler<DownloadEventArgs2> OnDownload2
        {
            add
            {
                if (m_downloadHandler2 == null)
                {
                    MBApi.wkeOnDownload2(Handle, m_wkeDownloadCallback2, IntPtr.Zero);
                }
                m_downloadHandler2 += value;
            }
            remove
            {
                m_downloadHandler2 -= value;
                if (m_downloadHandler2 == null)
                {
                    MBApi.wkeOnDownload2(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 控制台
        /// </summary>
        public event EventHandler<ConsoleEventArgs> OnConsole
        {
            add
            {
                if (m_consoleHandler == null)
                {
                    MBApi.wkeOnConsole(Handle, m_wkeConsoleCallback, IntPtr.Zero);
                }
                m_consoleHandler += value;
            }
            remove
            {
                m_consoleHandler -= value;
                if (m_consoleHandler == null)
                {
                    MBApi.wkeOnConsole(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 开始载入URL
        /// </summary>
        public event EventHandler<LoadUrlBeginEventArgs> OnLoadUrlBegin
        {
            add
            {
                if (m_loadUrlBeginHandler == null)
                {
                    MBApi.wkeOnLoadUrlBegin(Handle, m_wkeLoadUrlBeginCallback, IntPtr.Zero);
                }
                m_loadUrlBeginHandler += value;
            }
            remove
            {
                m_loadUrlBeginHandler -= value;
                if (m_loadUrlBeginHandler == null)
                {
                    MBApi.wkeOnLoadUrlBegin(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 结束载入URL
        /// </summary>
        public event EventHandler<LoadUrlEndEventArgs> OnLoadUrlEnd
        {
            add
            {
                if (m_loadUrlEndHandler == null)
                {
                    MBApi.wkeOnLoadUrlEnd(Handle, m_wkeLoadUrlEndCallback, IntPtr.Zero);
                }
                m_loadUrlEndHandler += value;
            }
            remove
            {
                m_loadUrlEndHandler -= value;
                if (m_loadUrlEndHandler == null)
                {
                    MBApi.wkeOnLoadUrlEnd(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 载入URL失败
        /// </summary>
        public event EventHandler<LoadUrlFailEventArgs> OnLoadUrlFail
        {
            add
            {
                if (m_loadUrlFailHandler == null)
                {
                    MBApi.wkeOnLoadUrlFail(Handle, m_wkeLoadUrlFailCallback, IntPtr.Zero);
                }
                m_loadUrlFailHandler += value;
            }
            remove
            {
                m_loadUrlFailHandler -= value;
                if (m_loadUrlFailHandler == null)
                {
                    MBApi.wkeOnLoadUrlFail(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 脚本上下文已创建
        /// </summary>
        public event EventHandler<DidCreateScriptContextEventArgs> OnDidCreateScriptContext
        {
            add
            {
                if (m_didCreateScriptContextHandler == null)
                {
                    MBApi.wkeOnDidCreateScriptContext(Handle, m_wkeDidCreateScriptContextCallback, IntPtr.Zero);
                }
                m_didCreateScriptContextHandler += value;
            }
            remove
            {
                m_didCreateScriptContextHandler -= value;
                if (m_didCreateScriptContextHandler == null)
                {
                    MBApi.wkeOnDidCreateScriptContext(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 脚本上下文将释放
        /// </summary>
        public event EventHandler<WillReleaseScriptContextEventArgs> OnWillReleaseScriptContext
        {
            add
            {
                if (m_willReleaseScriptContextHandler == null)
                {
                    MBApi.wkeOnWillReleaseScriptContext(Handle, m_wkeWillReleaseScriptContextCallback, IntPtr.Zero);
                }
                m_willReleaseScriptContextHandler += value;
            }
            remove
            {
                m_willReleaseScriptContextHandler -= value;
                if (m_willReleaseScriptContextHandler == null)
                {
                    MBApi.wkeOnWillReleaseScriptContext(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 网络响应事件
        /// </summary>
        public event EventHandler<NetResponseEventArgs> OnNetResponse
        {
            add
            {
                if (m_netResponseHandler == null)
                {
                    MBApi.wkeNetOnResponse(Handle, m_wkeNetResponseCallback, IntPtr.Zero);
                }
                m_netResponseHandler += value;
            }
            remove
            {
                m_netResponseHandler -= value;
                if (m_netResponseHandler == null)
                {
                    MBApi.wkeNetOnResponse(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 媒体将被载入事件
        /// </summary>
        public event EventHandler<WillMediaLoadEventArgs> OnWillMediaLoad
        {
            add
            {
                if (m_willMediaLoadHandler == null)
                {
                    MBApi.wkeOnWillMediaLoad(Handle, m_wkeWillMediaLoadCallback, IntPtr.Zero);
                }
                m_willMediaLoadHandler += value;
            }
            remove
            {
                m_willMediaLoadHandler -= value;
                if (m_willMediaLoadHandler == null)
                {
                    MBApi.wkeOnWillMediaLoad(Handle, null, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// 其他载入事件
        /// </summary>
        public event EventHandler<OtherLoadEventArgs> OnOtherLoad
        {
            add
            {
                if (m_OtherLoadHandler == null)
                {
                    MBApi.wkeOnOtherLoad(Handle, m_wkeOnOtherLoadCallback, IntPtr.Zero);
                }
                m_OtherLoadHandler += value;
            }
            remove
            {
                m_OtherLoadHandler -= value;
                if (m_OtherLoadHandler == null)
                {
                    MBApi.wkeOnOtherLoad(Handle, null, IntPtr.Zero);
                }
            }
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public WebView()
        {
            /*if (!File.Exists($"{Environment.CurrentDirectory}\\node.dll"))
            {
                MessageBox.Show("请在程序同目录下放置node.dll文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }*/

            if (MBApi.wkeIsInitialize() == 0)
            {
                MBApi.wkeInitialize();
            }
            m_wkePaintUpdatedCallback = new wkePaintUpdatedCallback(wkeOnPaintUpdated);
            m_WndProcCallback = new WndProcCallback(OnWndProc);
            SetCallback();
            Handle = MBApi.wkeCreateWebView();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="window">窗口</param>
        /// <param name="isTransparent">true 表示为分层窗口，窗口必须是顶层</param>
        public WebView(IWin32Window window, bool isTransparent = false)
        {
            /*if (!File.Exists($"{Environment.CurrentDirectory}\\node.dll"))
            {
                MessageBox.Show("请在程序同目录下放置node.dll文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }*/

            if (MBApi.wkeIsInitialize() == 0)
            {
                MBApi.wkeInitialize();
            }
            m_wkePaintUpdatedCallback = new wkePaintUpdatedCallback(wkeOnPaintUpdated);
            m_WndProcCallback = new WndProcCallback(OnWndProc);
            SetCallback();
            Bind(window, isTransparent);
        }

        /// <summary>
        /// 销毁 WebView
        /// </summary>
        public void Dispose()
        {
            if (Handle != IntPtr.Zero)
            {
                if (m_OldProc != IntPtr.Zero)
                {
                    MB_Common.SetWindowLong(m_hWnd, (int)WinConst.GWL_WNDPROC, m_OldProc.To32());
                    m_OldProc = IntPtr.Zero;
                }

                MBApi.wkeSetHandle(Handle, IntPtr.Zero);
                MBApi.wkeDestroyWebView(Handle);
                Handle = IntPtr.Zero;
                m_hWnd = IntPtr.Zero;
            }
        }

        protected void wkeOnPaintUpdated(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy)
        {
            IntPtr hWnd = param;

            if ((int)WinConst.WS_EX_LAYERED == ((int)WinConst.WS_EX_LAYERED & MB_Common.GetWindowLong(m_hWnd, (int)WinConst.GWL_EXSTYLE)))
            {
                RECT rectDest = new RECT();
                MB_Common.GetWindowRect(m_hWnd, ref rectDest);
                MB_Common.OffsetRect(ref rectDest, -rectDest.Left, -rectDest.Top);

                SIZE sizeDest = new SIZE(rectDest.Right - rectDest.Left, rectDest.Bottom - rectDest.Top);
                POINT pointSource = new POINT();

                BITMAP bmp = new BITMAP();
                IntPtr hBmp = MB_Common.GetCurrentObject(hdc, (int)WinConst.OBJ_BITMAP);
                MB_Common.GetObject(hBmp, Marshal.SizeOf(typeof(BITMAP)), ref bmp);

                sizeDest.cx = bmp.bmWidth;
                sizeDest.cy = bmp.bmHeight;

                IntPtr hdcScreen = MB_Common.GetDC(hWnd);

                BLENDFUNCTION blend = new BLENDFUNCTION();
                blend.BlendOp = (byte)WinConst.AC_SRC_OVER;
                blend.SourceConstantAlpha = 255;
                blend.AlphaFormat = (byte)WinConst.AC_SRC_ALPHA;

                if (MB_Common.UpdateLayeredWindow(m_hWnd, hdcScreen, IntPtr.Zero, ref sizeDest, hdc, ref pointSource, 0, ref blend, (int)WinConst.ULW_ALPHA) == 0)
                {
                    IntPtr hdcMemory = MB_Common.CreateCompatibleDC(hdcScreen);
                    IntPtr hbmpMemory = MB_Common.CreateCompatibleBitmap(hdcScreen, sizeDest.cx, sizeDest.cy);
                    IntPtr hbmpOld = MB_Common.SelectObject(hdcMemory, hbmpMemory);

                    MB_Common.BitBlt(hdcMemory, 0, 0, sizeDest.cx, sizeDest.cy, hdc, 0, 0, (int)WinConst.SRCCOPY | (int)WinConst.CAPTUREBLT);
                    MB_Common.BitBlt(hdc, 0, 0, sizeDest.cx, sizeDest.cy, hdcMemory, 0, 0, (int)WinConst.SRCCOPY | (int)WinConst.CAPTUREBLT);
                    MB_Common.UpdateLayeredWindow(m_hWnd, hdcScreen, IntPtr.Zero, ref sizeDest, hdcMemory, ref pointSource, 0, ref blend, (int)WinConst.ULW_ALPHA);
                    MB_Common.SelectObject(hdcMemory, hbmpOld);
                    MB_Common.DeleteObject(hbmpMemory);
                    MB_Common.DeleteDC(hdcMemory);
                }

                MB_Common.ReleaseDC(m_hWnd, hdcScreen);
            }
            else
            {
                RECT rc = new RECT(x, y, x + cx, y + cy);
                MB_Common.InvalidateRect(m_hWnd, ref rc, true);
            }
        }

        protected IntPtr OnWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (OnWindowProc != null)
            {
                WindowProcEventArgs e = new WindowProcEventArgs(hWnd, (int)msg, wParam, lParam);
                OnWindowProc(this, e);
                if (e.bHand)
                {
                    return e.Result;
                }
            }

            switch (msg)
            {
                case (uint)WinConst.WM_PAINT:    // 当窗口显示区域的一部分显示内容或者全部变为“无效”，以致于必须“更新画面”时
                    {
                        if ((int)WinConst.WS_EX_LAYERED != ((int)WinConst.WS_EX_LAYERED & MB_Common.GetWindowLong(hWnd, (int)WinConst.GWL_EXSTYLE)))
                        {
                            PAINTSTRUCT ps = new PAINTSTRUCT();
                            IntPtr hdc = MB_Common.BeginPaint(hWnd, ref ps);

                            RECT rcClip = ps.rcPaint;
                            RECT rcClient = new RECT();
                            MB_Common.GetClientRect(hWnd, ref rcClient);
                            RECT rcInvalid = rcClient;

                            if (rcClip.Right != rcClip.Left && rcClip.Bottom != rcClip.Top)
                            {
                                MB_Common.IntersectRect(ref rcInvalid, ref rcClip, ref rcClient);
                            }

                            int srcX = rcInvalid.Left - rcClient.Left;
                            int srcY = rcInvalid.Top - rcClient.Top;
                            int destX = rcInvalid.Left;
                            int destY = rcInvalid.Top;
                            int width = rcInvalid.Right - rcInvalid.Left;
                            int height = rcInvalid.Bottom - rcInvalid.Top;

                            if (width != 0 && height != 0)
                            {
                                MB_Common.BitBlt(hdc, destX, destY, width, height, MBApi.wkeGetViewDC(Handle), srcX, srcY, (int)WinConst.SRCCOPY);
                            }

                            MB_Common.EndPaint(hWnd, ref ps);
                            return IntPtr.Zero;
                        }

                        break;
                    }

                case (uint)WinConst.WM_ERASEBKGND:    // 窗口背景被擦除时
                    {
                        return new IntPtr(1);
                    }

                case (uint)WinConst.WM_SIZE:    // 窗口尺寸改变时
                    {
                        MBApi.wkeResize(Handle, lParam.To32().LOWORD(), lParam.To32().HIWORD());
                        break;
                    }

                case (uint)WinConst.WM_KEYDOWN:    // 非系统键被按下时
                    {
                        uint flags = 0;
                        if ((lParam.To32().HIWORD() & (int)WinConst.KF_REPEAT) != 0)
                        {
                            flags |= (uint)wkeKeyFlags.WKE_REPEAT;
                        }

                        if ((lParam.To32().HIWORD() & (int)WinConst.KF_EXTENDED) != 0)
                        {
                            flags |= (uint)wkeKeyFlags.WKE_EXTENDED;
                        }

                        if (MBApi.wkeFireKeyDownEvent(Handle, wParam.To32(), flags, false) != 0)
                        {
                            return IntPtr.Zero;
                        }

                        break;
                    }

                case (uint)WinConst.WM_KEYUP:    // 非系统键被抬起时
                    {
                        uint flags = 0;
                        if ((lParam.To32().HIWORD() & (int)WinConst.KF_REPEAT) != 0)
                        {
                            flags |= (uint)wkeKeyFlags.WKE_REPEAT;
                        }

                        if ((lParam.To32().HIWORD() & (int)WinConst.KF_EXTENDED) != 0)
                        {
                            flags |= (uint)wkeKeyFlags.WKE_EXTENDED;
                        }

                        if (MBApi.wkeFireKeyUpEvent(Handle, wParam.To32(), flags, false) != 0)
                        {
                            return IntPtr.Zero;
                        }

                        break;
                    }

                case (uint)WinConst.WM_CHAR:    // ASCII码为0-127之间的按键被按下时
                    {
                        uint flags = 0;
                        if ((lParam.To32().HIWORD() & (int)WinConst.KF_REPEAT) != 0)
                        {
                            flags |= (uint)wkeKeyFlags.WKE_REPEAT;
                        }

                        if ((lParam.To32().HIWORD() & (int)WinConst.KF_EXTENDED) != 0)
                        {
                            flags |= (uint)wkeKeyFlags.WKE_EXTENDED;
                        }

                        if (MBApi.wkeFireKeyPressEvent(Handle, wParam.To32(), flags, false) != 0)
                        {
                            return IntPtr.Zero;
                        }

                        break;
                    }

                case (uint)WinConst.WM_LBUTTONDOWN:
                case (uint)WinConst.WM_MBUTTONDOWN:
                case (uint)WinConst.WM_RBUTTONDOWN:
                case (uint)WinConst.WM_LBUTTONDBLCLK:
                case (uint)WinConst.WM_MBUTTONDBLCLK:
                case (uint)WinConst.WM_RBUTTONDBLCLK:
                case (uint)WinConst.WM_LBUTTONUP:
                case (uint)WinConst.WM_MBUTTONUP:
                case (uint)WinConst.WM_RBUTTONUP:
                case (uint)WinConst.WM_MOUSEMOVE:
                    {
                        if (msg == (uint)WinConst.WM_LBUTTONDOWN || msg == (uint)WinConst.WM_MBUTTONDOWN || msg == (uint)WinConst.WM_RBUTTONDOWN)
                        {
                            if (MB_Common.GetFocus() != hWnd)
                            {
                                MB_Common.SetFocus(hWnd);
                            }
                            MB_Common.SetCapture(hWnd);
                        }
                        else if (msg == (uint)WinConst.WM_LBUTTONUP || msg == (uint)WinConst.WM_MBUTTONUP || msg == (uint)WinConst.WM_RBUTTONUP)
                        {
                            MB_Common.ReleaseCapture();
                        }

                        uint flags = 0;
                        if ((wParam.To32() & (int)WinConst.MK_CONTROL) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_CONTROL;
                        }

                        if ((wParam.To32() & (int)WinConst.MK_SHIFT) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_SHIFT;
                        }

                        if ((wParam.To32() & (int)WinConst.MK_LBUTTON) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_LBUTTON;
                        }

                        if ((wParam.To32() & (int)WinConst.MK_MBUTTON) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_MBUTTON;
                        }

                        if ((wParam.To32() & (int)WinConst.MK_RBUTTON) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_RBUTTON;
                        }

                        if (MBApi.wkeFireMouseEvent(Handle, msg, lParam.To32().LOWORD(), lParam.To32().HIWORD(), flags) != 0)
                        {
                            return IntPtr.Zero;
                        }

                        break;
                    }

                case (uint)WinConst.WM_CONTEXTMENU:
                    {
                        POINT pt;
                        pt.x = lParam.To32().LOWORD();
                        pt.y = lParam.To32().HIWORD();

                        if (pt.x != -1 && pt.y != -1)
                        {
                            MB_Common.ScreenToClient(hWnd, ref pt);
                        }

                        uint flags = 0;

                        if ((wParam.To32() & (int)WinConst.MK_CONTROL) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_CONTROL;
                        }

                        if ((wParam.To32() & (int)WinConst.MK_SHIFT) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_SHIFT;
                        }

                        if ((wParam.To32() & (int)WinConst.MK_LBUTTON) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_LBUTTON;
                        }

                        if ((wParam.To32() & (int)WinConst.MK_MBUTTON) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_MBUTTON;
                        }

                        if ((wParam.To32() & (int)WinConst.MK_RBUTTON) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_RBUTTON;
                        }

                        if (MBApi.wkeFireContextMenuEvent(Handle, pt.x, pt.y, flags) != 0)
                        {
                            return IntPtr.Zero;
                        }

                        break;
                    }

                case (uint)WinConst.WM_MOUSEWHEEL:
                    {
                        POINT pt;
                        pt.x = lParam.To32().LOWORD();
                        pt.y = lParam.To32().HIWORD();
                        MB_Common.ScreenToClient(hWnd, ref pt);

                        uint flags = 0;

                        if ((wParam.To32() & (int)WinConst.MK_CONTROL) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_CONTROL;
                        }

                        if ((wParam.To32() & (int)WinConst.MK_SHIFT) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_SHIFT;
                        }

                        if ((wParam.To32() & (int)WinConst.MK_LBUTTON) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_LBUTTON;
                        }

                        if ((wParam.To32() & (int)WinConst.MK_MBUTTON) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_MBUTTON;
                        }

                        if ((wParam.To32() & (int)WinConst.MK_RBUTTON) != 0)
                        {
                            flags |= (uint)wkeMouseFlags.WKE_RBUTTON;
                        }

                        if (MBApi.wkeFireMouseWheelEvent(Handle, pt.x, pt.y, wParam.To32().HIWORD(), flags) != 0)
                        {
                            return IntPtr.Zero;
                        }

                        break;
                    }
                case (uint)WinConst.WM_SETFOCUS:
                    {
                        MBApi.wkeSetFocus(Handle);
                        return IntPtr.Zero;
                    }

                case (uint)WinConst.WM_KILLFOCUS:
                    {
                        MBApi.wkeKillFocus(Handle);
                        return IntPtr.Zero;
                    }

                case (uint)WinConst.WM_SETCURSOR:
                    {
                        if (MBApi.wkeFireWindowsMessage(Handle, hWnd, (uint)WinConst.WM_SETCURSOR, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) != 0)
                        {
                            return IntPtr.Zero;
                        }

                        break;
                    }

                case (uint)WinConst.WM_IME_STARTCOMPOSITION:
                    {
                        wkeRect caret = MBApi.wkeGetCaretRect(Handle);

                        COMPOSITIONFORM COMPOSITIONFORM = new COMPOSITIONFORM();
                        COMPOSITIONFORM.dwStyle = (int)WinConst.CFS_POINT | (int)WinConst.CFS_FORCE_POSITION;
                        COMPOSITIONFORM.ptCurrentPos.x = caret.x;
                        COMPOSITIONFORM.ptCurrentPos.y = caret.y;

                        IntPtr hIMC = MB_Common.ImmGetContext(hWnd);
                        MB_Common.ImmSetCompositionWindow(hIMC, ref COMPOSITIONFORM);
                        MB_Common.ImmReleaseContext(hWnd, hIMC);

                        return IntPtr.Zero;
                    }

                case (uint)WinConst.WM_INPUTLANGCHANGE:
                    {
                        return MB_Common.DefWindowProc(hWnd, msg, wParam, lParam);
                    }
            }

            return MB_Common.CallWindowProc(m_OldProc, hWnd, msg, wParam,  lParam);
        }

        #region 基本方法

        /// <summary>
        /// 初始化miniblink，如果没有调用，则在 new WebView 时会自己初始化
        /// </summary>
        public void wkeInitialize()
        {
            if (MBApi.wkeIsInitialize() == 0)
            {
                MBApi.wkeInitialize();
            }
        }

        /// <summary>
        /// 初始化miniblink，并可以设置一些参数，如果没有调用，则在 new WebView 时会自己初始化
        /// </summary>
        /// <param name="settings"></param>
        public void wkeInitialize(wkeSettings settings)
        {
            if (MBApi.wkeIsInitialize() == 0)
            {
                MBApi.wkeInitializeEx(settings);
            }
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        public uint Version
        {
            get { return MBApi.wkeGetVersion(); }
        }

        /// <summary>
        /// 获取版本信息
        /// </summary>
        public string VersionString
        {
            get
            {
                IntPtr utf8 = MBApi.wkeGetVersionString();
                if (utf8 != IntPtr.Zero)
                {
                    return Marshal.PtrToStringAnsi(utf8);
                }
                return null;
            }
        }

        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="proxy"></param>
        public void SetProxy(wkeProxy proxy)
        {
            MBApi.wkeSetProxy(ref proxy);
        }

        /// <summary>
        /// 判断是否主框架
        /// </summary>
        /// <param name="WebFrame">框架句柄</param>
        /// <returns></returns>
        public bool FrameIsMainFrame(IntPtr WebFrame)
        {
            return MBApi.wkeIsMainFrame(WebFrame) != 0;
        }

        /// <summary>
        /// 判断是否远程框架
        /// </summary>
        /// <param name="WebFrame"></param>
        /// <returns></returns>
        public bool FrameIsRemoteFrame(IntPtr WebFrame)
        {
            return MBApi.wkeIsWebRemoteFrame(WebFrame) != 0;
        }

        /// <summary>
        /// 获取v8Context
        /// </summary>
        /// <param name="WebFrame">框架句柄</param>
        /// <returns></returns>
        public IntPtr FrameGetMainWorldScriptContext(IntPtr WebFrame)
        {
            IntPtr v8ContextPtr = IntPtr.Zero;
            MBApi.wkeWebFrameGetMainWorldScriptContext(WebFrame, ref v8ContextPtr);
            return v8ContextPtr;
        }

        /// <summary>
        /// 获取v8Isolate
        /// </summary>
        /// <returns></returns>
        public IntPtr GetBlinkMainThreadIsolate()
        {
            return MBApi.wkeGetBlinkMainThreadIsolate();
        }

        /// <summary>
        /// 设置mimeType。此方法应该在 OnLoadUrlBegin 事件中使用
        /// </summary>
        /// <param name="job"></param>
        /// <param name="type">MIMEType</param>
        public void NetSetMIMEType(IntPtr job, string MIMEType)
        {
            MBApi.wkeNetSetMIMEType(job, MIMEType);
        }

        /// <summary>
        /// 获取mimeType。此方法应该在 OnNetResponse 事件中使用
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public string NetGetMIMEType(IntPtr job)
        {
            //IntPtr ptr = MBApi.wkeNetGetMIMEType(job, IntPtr.Zero);
            //return ptr.UTF8PtrToStr();

            IntPtr mime = MBApi.wkeCreateStringW(null, 0);
            if (mime == IntPtr.Zero)
            {
                return string.Empty;
            }

            MBApi.wkeNetGetMIMEType(job, mime);
            string mimeType = MBApi.wkeGetStringW(mime).UTF8PtrToStr();
            MBApi.wkeDeleteString(mime);

            return mimeType;
        }

        /// <summary>
        /// 继续执行中断的网络任务，参看wkeNetHoldJobToAsynCommit接口。
        /// </summary>
        /// <param name="job"></param>
        public void NetContinueJob(IntPtr job)
        {
            MBApi.wkeNetContinueJob(job);
        }

        /// <summary>
        /// 通过jobPtr获取当前请求的url。
        /// </summary>
        /// <param name="job"></param>
        public string NetGetUrlByJob(IntPtr job)
        {
            IntPtr ptr = MBApi.wkeNetGetUrlByJob(job);
            return ptr.UTF8PtrToStr();
        }

        /// <summary>
        /// 获取Raw格式的HTTP请求数据，wkeSlist是个保存了网络数据的链表结构，请参看wke.h文件。
        /// </summary>
        /// <param name="job"></param>
        public wkeSlist NetGetRawHttpHead(IntPtr job)
        {
            IntPtr ptr = MBApi.wkeNetGetRawHttpHead(job);
            return (wkeSlist)ptr.UTF8PtrToStruct(typeof(wkeSlist));
        }

        /// <summary>
        /// 获取Raw格式的HTTP响应数据，wkeSlist是个保存了网络数据的链表结构，请参看wke.h文件。
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public wkeSlist NetGetRawResponseHead(IntPtr job)
        {
            IntPtr ptr = MBApi.wkeNetGetRawResponseHead(job);
            return (wkeSlist)ptr.UTF8PtrToStruct(typeof(wkeSlist));
        }

        /// <summary>
        /// 取消本次网络请求，需要在wkeOnLoadUrlBegin里调用。
        /// </summary>
        /// <param name="job"></param>
        public void NetCancelRequest(IntPtr job)
        {
            MBApi.wkeNetCancelRequest(job);
        }

        /// <summary>
        /// 网络访问经常存在异步操作，当wkeOnLoadUrlBegin里拦截到一个请求后如不能马上判断出结果，此时可以调用本接口，处理完成后需调用wkeNetContinueJob来让此请求继续。
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public bool NetHoldJobToAsynCommit(IntPtr job)
        {
            return MBApi.wkeNetHoldJobToAsynCommit(job) == 1 ? true : false;
        }

        /// <summary>
        /// 修改当前请求的url。
        /// </summary>
        /// <param name="job"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool NetChangeRequestUrl(IntPtr job, string url)
        {
            return MBApi.wkeNetChangeRequestUrl(job, url) == 1 ? true : false;
        }

        /// <summary>
        /// 创建一个网络请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="mime"></param>
        /// <returns></returns>
        public IntPtr NetCreateWebUrlRequest(IntPtr url, IntPtr method, IntPtr mime)
        {
            return MBApi.wkeNetCreateWebUrlRequest(url, method, mime);
        }

        /// <summary>
        /// 创建一个网络请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IntPtr NetCreateWebUrlRequest2(IntPtr request)
        {
            return MBApi.wkeNetCreateWebUrlRequest2(request);
        }

        /// <summary>
        /// 复制一个网络请求
        /// </summary>
        /// <param name="job"></param>
        /// <param name="needExtraData"></param>
        /// <returns></returns>
        public IntPtr NetCopyWebUrlRequest(IntPtr job, bool needExtraData)
        {
            return MBApi.wkeNetCopyWebUrlRequest(job, needExtraData);
        }

        /// <summary>
        /// 取消网络请求。
        /// </summary>
        /// <param name="request"></param>
        public void NetDeleteBlinkWebURLRequestPtr(IntPtr request)
        {
            MBApi.wkeNetDeleteBlinkWebURLRequestPtr(request);
        }

        /// <summary>
        /// 在指定网络请求中插入一个请求头。
        /// </summary>
        /// <param name="request"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void NetAddHTTPHeaderFieldToUrlRequest(IntPtr request, IntPtr name, IntPtr value)
        {
            MBApi.wkeNetAddHTTPHeaderFieldToUrlRequest(request, name, value);
        }

        /// <summary>
        /// 开始网络请求。
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="request"></param>
        /// <param name="param"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public int NetStartUrlRequest(IntPtr webView, IntPtr request, IntPtr param, wkeUrlRequestCallbacks callback)
        {
            return MBApi.wkeNetStartUrlRequest(webView, request, param, callback);
        }

        /// <summary>
        /// 获取HTTP响应状态码。
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public int NetGetHttpStatusCode(IntPtr response)
        {
            return MBApi.wkeNetGetHttpStatusCode(response);
        }

        /// <summary>
        /// 获取响应数据大小。
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public long NetGetExpectedContentLength(IntPtr response)
        {
            return MBApi.wkeNetGetExpectedContentLength(response);
        }

        /// <summary>
        /// 获取响应url。
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public string NetGetResponseUrl(IntPtr response)
        {
            IntPtr ptr = MBApi.wkeNetGetResponseUrl(response);
            return ptr.UTF8PtrToStr();
        }

        /// <summary>
        /// 取消网络请求。
        /// </summary>
        /// <param name="requestId"></param>
        public void NetCancelWebUrlRequest(int requestId)
        {
            MBApi.wkeNetCancelWebUrlRequest(requestId);
        }

        /// <summary>
        /// 获取此请求中的post数据，注意：只有当请求是post时才可获取到，get请求直接从url上取就好了。
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public wkePostBodyElements NetGetPostBody(IntPtr job)
        {
            IntPtr ptr = MBApi.wkeNetGetPostBody(job);
            return (wkePostBodyElements)ptr.UTF8PtrToStruct(typeof(wkePostBodyElements));
        }

        /// <summary>
        /// 创建一组post数据内容
        /// </summary>
        /// <param name="job"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public wkePostBodyElements NetCreatePostBodyElements(long length)
        {
            IntPtr ptr = MBApi.wkeNetCreatePostBodyElements(Handle, length);
            return (wkePostBodyElements)ptr.UTF8PtrToStruct(typeof(wkePostBodyElements));
        }

        /// <summary>
        /// 干掉指定的post数据内容组，释放内存。
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public void NetFreePostBodyElements(IntPtr elements)
        {
            MBApi.wkeNetFreePostBodyElements(elements);
        }

        /// <summary>
        /// 创建一个post数据内容。
        /// </summary>
        /// <param name="webView"></param>
        /// <returns></returns>
        public wkePostBodyElement NetCreatePostBodyElement()
        {
            IntPtr ptr = MBApi.wkeNetCreatePostBodyElement(Handle);
            return (wkePostBodyElement)ptr.UTF8PtrToStruct(typeof(wkePostBodyElement));
        }

        /// <summary>
        /// 干掉指定的post数据内容，释放内存。
        /// </summary>
        /// <param name="elements"></param>
        public void NetFreePostBodyElement(IntPtr element)
        {
            MBApi.wkeNetFreePostBodyElement(element);
        }

        /// <summary>
        /// 设置HTTP头字段。此方法应该在 OnLoadUrlBegin 事件中使用
        /// </summary>
        /// <param name="job"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="response"></param>
        public void NetSetHTTPHeaderField(IntPtr job, string key, string value, bool response)
        {
            MBApi.wkeNetSetHTTPHeaderField(job, key, value, response);
        }

        /// <summary>
        /// 获取HTTP请求头
        /// </summary>
        /// <param name="job"></param>
        /// <param name="key"></param>
        public string NetGetHTTPHeaderField(IntPtr job, string key)
        {
            return MBApi.wkeNetGetHTTPHeaderField(job, key).UTF8PtrToStr();
        }

        /// <summary>
        /// 获取HTTP响应头
        /// </summary>
        /// <param name="job"></param>
        /// <param name="key"></param>
        public string NetGetHTTPHeaderFieldFromResponse(IntPtr job, string key)
        {
            return MBApi.wkeNetGetHTTPHeaderFieldFromResponse(job, key).UTF8PtrToStr();
        }

        /// <summary>
        /// 设置网络数据。此方法应该在 OnLoadUrlBegin 事件中使用
        /// </summary>
        /// <param name="job"></param>
        /// <param name="data"></param>
        public void NetSetData(IntPtr job, byte[] data)
        {
            MBApi.wkeNetSetData(job, data, data.Length);
        }

        /// <summary>
        /// 设置网络数据。此方法应该在 OnLoadUrlBegin 事件中使用
        /// </summary>
        /// <param name="job"></param>
        /// <param name="str">string数据</param>
        public void NetSetData(IntPtr job, string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            MBApi.wkeNetSetData(job, data, data.Length);
        }

        /// <summary>
        /// 设置网络数据。此方法应该在 OnLoadUrlBegin 事件中使用
        /// </summary>
        /// <param name="job"></param>
        /// <param name="png">PNG图片数据</param>
        public void NetSetData(IntPtr job, Image png)
        {
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                png.Save(ms, ImageFormat.Png);
                data = ms.GetBuffer();
            }
            MBApi.wkeNetSetData(job, data, data.Length);
        }

        /// <summary>
        /// 设置网络数据。此方法应该在 OnLoadUrlBegin 事件中使用
        /// </summary>
        /// <param name="job"></param>
        /// <param name="img">图片数据</param>
        /// <param name="fmt">图片格式</param>
        public void NetSetData(IntPtr job, Image img, ImageFormat fmt)
        {
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, fmt);
                data = ms.GetBuffer();
            }
            MBApi.wkeNetSetData(job, data, data.Length);
        }

        /// <summary>
        /// 此方法应该在 OnLoadUrlBegin 事件中使用，调用此函数后,网络层收到数据会存储在一buf内,接收数据完成后响应OnLoadUrlEnd事件.#此调用严重影响性能,慎用，
        /// 此函数和WebView.NetSetData的区别是，WebView.NetHookRequest会在接受到真正网络数据后再调用回调，并允许回调修改网络数据。
        /// 而WebView.NetSetData是在网络数据还没发送的时候修改
        /// </summary>
        /// <param name="job"></param>
        public void NetHookRequest(IntPtr job)
        {
            MBApi.wkeNetHookRequest(job);
        }

        /// <summary>
        /// 获取请求方法
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public wkeRequestType GetRequestMethod(IntPtr job)
        {
            return MBApi.wkeNetGetRequestMethod(job);
        }

        /// <summary>
        /// 获取图标，结果再Callback中取
        /// </summary>
        /// <param name="WebView"></param>
        /// <param name="Callback"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int NetGetFavicon(IntPtr WebView, wkeNetResponseCallback Callback, IntPtr param)
        {
            return MBApi.wkeNetGetFavicon(WebView, Callback, param);
        }

        /// <summary>
        /// 指定一个回调函数，访问所有Cookie
        /// </summary>
        /// <param name="visitor">wkeCookieVisitor 委托</param>
        /// <param name="userData">用户数据</param>
        public void VisitAllCookie(wkeCookieVisitor visitor, IntPtr userData)
        {
            MBApi.wkeVisitAllCookie(Handle, userData, visitor);
        }

        /// <summary>
        /// 执行cookie命令，清空等操作
        /// </summary>
        /// <param name="command"></param>
        public void PerformCookieCommand(wkeCookieCommand command)
        {
            MBApi.wkePerformCookieCommand(command);
        }

        /// <summary>
        /// 绑定指定窗口
        /// </summary>
        /// <param name="window">窗口</param>
        /// <param name="isTransparent">是否透明模式，必须是顶层窗口才有效</param>
        /// <returns></returns>
        public bool Bind(IWin32Window window, bool isTransparent = false)
        {
            if (m_hWnd == window.Handle)
            {
                return true;
            }

            if (Handle == IntPtr.Zero)
            {
                Handle = MBApi.wkeCreateWebView();
                if (Handle == IntPtr.Zero)
                {
                    return false;
                }
            }
            m_hWnd = window.Handle;

            MBApi.wkeSetHandle(Handle, m_hWnd);
            MBApi.wkeOnPaintUpdated(Handle, m_wkePaintUpdatedCallback, m_hWnd);

            if (isTransparent)
            {
                MBApi.wkeSetTransparent(Handle, true);
                int exStyle = MB_Common.GetWindowLong(m_hWnd, (int)WinConst.GWL_EXSTYLE);
                MB_Common.SetWindowLong(m_hWnd, (int)WinConst.GWL_EXSTYLE, exStyle | (int)WinConst.WS_EX_LAYERED);
            }
            else
            {
                MBApi.wkeSetTransparent(Handle, false);
            }

            m_OldProc = (IntPtr)MB_Common.GetWindowLong(m_hWnd, (int)WinConst.GWL_WNDPROC);
            IntPtr ptrProcCallback = Marshal.GetFunctionPointerForDelegate(m_WndProcCallback);
            if (m_OldProc != ptrProcCallback)
            {
                if (IntPtr.Size == 8)
                {
                    m_OldProc = (IntPtr)MB_Common.SetWindowLong(m_hWnd, (int)WinConst.GWL_WNDPROC, (long)ptrProcCallback);
                }
                else
                {
                    m_OldProc = (IntPtr)MB_Common.SetWindowLong(m_hWnd, (int)WinConst.GWL_WNDPROC, (int)ptrProcCallback);
                }
            }

            RECT rc = new RECT();
            MB_Common.GetClientRect(m_hWnd, ref rc);
            MBApi.wkeResize(Handle, rc.Right - rc.Left, rc.Bottom - rc.Top);

            return true;
        }

        /// <summary>
        /// 载入URL
        /// </summary>
        /// <param name="URL"></param>
        public void Load(string URL)
        {
            MBApi.wkeLoadW(Handle, URL);
        }

        /// <summary>
        /// 载入URL
        /// </summary>
        /// <param name="URL"></param>
        public void LoadURL(string URL)
        {
            MBApi.wkeLoadURLW(Handle, URL);
        }

        /// <summary>
        /// 载入本地文件
        /// </summary>
        /// <param name="FileName">文件名</param>
        public void LoadFile(string FileName)
        {
            MBApi.wkeLoadFileW(Handle, FileName);
        }

        /// <summary>
        /// 载入内存HTML文本
        /// </summary>
        /// <param name="html">HTML文本</param>
        public void LoadHTML(string Html)
        {
            MBApi.wkeLoadHTMLW(Handle, Html);
        }

        /// <summary>
        /// POST方式载入URL
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="PostData">提交的POST数据</param>
        public void PostURL(string URL, byte[] PostData)
        {
            MBApi.wkePostURLW(Handle, URL, PostData, PostData.Length);
        }

        /// <summary>
        /// 载入内存HTML文本，并指定 BaseURL
        /// </summary>
        /// <param name="Html">HTML文本</param>
        /// <param name="BaseURL"></param>
        public void LoadHtmlWithBaseUrl(string Html, string BaseURL)
        {
            MBApi.wkeLoadHtmlWithBaseUrl(Handle, Encoding.UTF8.GetBytes(Html), Encoding.UTF8.GetBytes(BaseURL));
        }

        /// <summary>
        /// 停止载入
        /// </summary>
        public void StopLoading()
        {
            MBApi.wkeStopLoading(Handle);
        }

        /// <summary>
        /// 重新载入
        /// </summary>
        public void Reload()
        {
            MBApi.wkeReload(Handle);
        }

        /// <summary>
        /// 跳转到指定偏移的浏览历史
        /// </summary>
        /// <param name="offset"></param>
        public void GoToOffset(int offset)
        {
            MBApi.wkeGoToOffset(Handle, offset);
        }

        /// <summary>
        /// 跳转到指定索引的浏览历史
        /// </summary>
        /// <param name="index"></param>
        public void GoToIndex(int index)
        {
            MBApi.wkeGoToIndex(Handle, index);
        }

        /// <summary>
        /// 获取Webview的ID
        /// </summary>
        /// <returns></returns>
        public int GetId()
        {
            return MBApi.wkeGetWebviewId(Handle);
        }

        /// <summary>
        /// 根据Webview的ID判断是否活动Webview
        /// </summary>
        /// <param name="webViewId"></param>
        /// <returns></returns>
        public bool IsWebviewAlive(int webViewId)
        {
            return MBApi.wkeIsWebviewAlive(Handle, webViewId) != 0;
        }

        /// <summary>
        /// 获取文档完成URL
        /// </summary>
        /// <param name="frameId">框架ID</param>
        /// <param name="partialURL"></param>
        /// <returns></returns>
        public string GetDocumentCompleteURL(IntPtr frameId, string partialURL)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(partialURL);
            return MBApi.wkeGetDocumentCompleteURL(Handle, frameId, utf8).UTF8PtrToStr();
        }

        /// <summary>
        /// 获取当前URL
        /// </summary>
        public string GetURL()
        {
            IntPtr pUrl = MBApi.wkeGetURL(Handle);
            if (pUrl != IntPtr.Zero)
                return pUrl.UTF8PtrToStr();
            return string.Empty;
        }

        /// <summary>
        /// 获取指定框架的URL
        /// </summary>
        /// <param name="FrameId">框架ID</param>
        /// <returns></returns>
        public string GetFrameURL(IntPtr FrameId)
        {
            IntPtr pUrl = MBApi.wkeGetFrameUrl(Handle, FrameId);
            if (pUrl != IntPtr.Zero)
                return pUrl.UTF8PtrToStr();
            return string.Empty;
        }

        /// <summary>
        /// 垃圾回收
        /// </summary>
        /// <param name="delayMs">延迟的毫秒数</param>
        public void GC(int delayMs)
        {
            MBApi.wkeGC(Handle, delayMs);
        }

        /// <summary>
        /// 设置当前WebView的代理
        /// </summary>
        /// <param name="proxy"></param>
        public void SetViewProxy(wkeProxy proxy)
        {
            MBApi.wkeSetViewProxy(Handle, ref proxy);
        }

        /// <summary>
        /// 设置title
        /// </summary>
        /// <param name="title"></param>
        public void SetWindowTitle(string title)
        {
            MBApi.wkeSetWindowTitle(Handle, title);
        }

        /// <summary>
        /// 休眠
        /// </summary>
        public void Sleep()
        {
            MBApi.wkeSleep(Handle);
        }

        /// <summary>
        /// 唤醒
        /// </summary>
        public void Wake()
        {
            MBApi.wkeWake(Handle);
        }

        /// <summary>
        /// 设置UserAgent
        /// </summary>
        /// <param name="UserAgent"></param>
        public void SetUserAgent(string UserAgent)
        {
            MBApi.wkeSetUserAgentW(Handle, UserAgent);
        }

        /// <summary>
        /// 获取UserAgent
        /// </summary>
        /// <returns></returns>
        public string GetUserAgent()
        {
            IntPtr pstr = MBApi.wkeGetUserAgent(Handle);
            if (pstr != IntPtr.Zero)
            {
                return pstr.UTF8PtrToStr();
            }
            return string.Empty;
        }

        /// <summary>
        /// 后退
        /// </summary>
        public bool GoBack()
        {
            return MBApi.wkeGoBack(Handle) != 0;
        }

        /// <summary>
        /// 前进
        /// </summary>
        public bool GoForward()
        {
            return MBApi.wkeGoForward(Handle) != 0;
        }

        /// <summary>
        /// 全选
        /// </summary>
        public void EditorSelectAll()
        {
            MBApi.wkeEditorSelectAll(Handle);
        }

        /// <summary>
        /// 取消选择
        /// </summary>
        public void EditorUnSelect()
        {
            MBApi.wkeEditorUnSelect(Handle);
        }

        /// <summary>
        /// 复制
        /// </summary>
        public void EditorCopy()
        {
            MBApi.wkeEditorCopy(Handle);
        }

        /// <summary>
        /// 剪切
        /// </summary>
        public void EditorCut()
        {
            MBApi.wkeEditorCut(Handle);
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        public void EditorPaste()
        {
            MBApi.wkeEditorPaste(Handle);
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void EditorDelete()
        {
            MBApi.wkeEditorDelete(Handle);
        }

        /// <summary>
        /// 撤销
        /// </summary>
        public void EditorUndo()
        {
            MBApi.wkeEditorUndo(Handle);
        }

        /// <summary>
        /// 重做
        /// </summary>
        public void EditorRedo()
        {
            MBApi.wkeEditorRedo(Handle);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <returns></returns>
        public string GetCookie()
        {
            IntPtr pStr = MBApi.wkeGetCookieW(Handle);
            if (pStr != IntPtr.Zero)
            {
                return Marshal.PtrToStringUni(pStr);
            }
            return string.Empty;
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="Cookie">cookie格式必须是:Set-cookie: PRODUCTINFO=webxpress; domain=.fidelity.com; path=/; secure</param>
        public void SetCookie(string Url, string Cookie)
        {
            byte[] url = Encoding.UTF8.GetBytes(Url);
            byte[] cookie = Encoding.UTF8.GetBytes(Cookie);
            MBApi.wkeSetCookie(Handle, url, cookie);
        }

        /// <summary>
        /// 设置Cookie目录
        /// </summary>
        /// <param name="Path"></param>
        public void SetCookieJarPath(string Path)
        {
            MBApi.wkeSetCookieJarPath(Handle, Path);
        }

        /// <summary>
        /// 设置Cookie全路径，包含文件名
        /// </summary>
        /// <param name="FileName"></param>
        public void SetCookieJarFullPath(string FileName)
        {
            MBApi.wkeSetCookieJarFullPath(Handle, FileName);
        }

        /// <summary>
        /// 设置 LocalStorage 目录
        /// </summary>
        /// <param name="Path"></param>
        public void SetLocalStorageFullPath(string Path)
        {
            MBApi.wkeSetLocalStorageFullPath(Handle, Path);
        }

        /// <summary>
        /// 添加插件目录
        /// </summary>
        /// <param name="Path"></param>
        public void AddPluginDirectory(string Path)
        {
            MBApi.wkeAddPluginDirectory(Handle, Path);
        }

        /// <summary>
        /// 获得焦点
        /// </summary>
        public void SetFocus()
        {
            MBApi.wkeSetFocus(Handle);
        }

        /// <summary>
        /// 失去焦点
        /// </summary>
        public void KillFocus()
        {
            MBApi.wkeKillFocus(Handle);
        }

        /// <summary>
        /// 运行JS
        /// </summary>
        /// <param name="JavaScript">脚本,如果需要返回值，则需要 return</param>
        /// <returns></returns>
        public object RunJS(string JavaScript)
        {
            long jsValue = MBApi.wkeRunJSW(Handle, JavaScript);
            return ToNetValue(jsValue);
        }

        /// <summary>
        /// 执行页面中的js函数
        /// </summary>
        /// <param name="strFuncName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object CallJsFunc(string strFuncName, params object[] param)
        {
            IntPtr es = MBApi.wkeGlobalExec(Handle);
            var args = param.Select(arg => ToJsValue(arg)).ToArray();
            long jsValue = MBApi.jsCall(es, MBApi.jsGetGlobal(es, strFuncName), MBApi.jsUndefined(), args, args.Length);

            return ToNetValue(jsValue);
        }

        /// <summary>
        /// 将js中的函数绑定到c#的函数，用于实现js调用c#
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fn"></param>
        /// <param name="argCount"></param>
        public void BindFunction(string name, wkeJsNativeFunction fn, uint argCount = 0)
        {
            MBApi.wkeJsBindFunction(name, fn, IntPtr.Zero, argCount);
        }

        /// <summary>
        /// 在指定框架运行JS
        /// </summary>
        /// <param name="FrameId">框架ID</param>
        /// <param name="JavaScript">脚本</param>
        /// <param name="IsInClosure">是否闭包</param>
        /// <returns></returns>
        public object RunJsByFrame(IntPtr FrameId, string JavaScript, bool IsInClosure = false)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(JavaScript);
            return MBApi.wkeRunJsByFrame(Handle, FrameId, utf8, IsInClosure);
        }

        /// <summary>
        /// 全局执行
        /// </summary>
        /// <returns>返回全局 jsExecState</returns>
        public IntPtr GlobalExec()
        {
            return MBApi.wkeGlobalExec(Handle);
        }

        /// <summary>
        /// 获取指定框架jsExecState
        /// </summary>
        /// <param name="FrameId">框架ID</param>
        /// <returns>返回 jsExecState</returns>
        public IntPtr GetGlobalExecByFrame(IntPtr FrameId)
        {
            return MBApi.wkeGetGlobalExecByFrame(Handle, FrameId);
        }

        /// <summary>
        /// 在c#中封装一个js函数，用于下面的类型转换
        /// </summary>
        public class JsFunc
        {
            private string m_strName = null;
            private WebView m_webView;

            public JsFunc(WebView webView, long jsValue)
            {
                m_webView = webView;
                IntPtr es = MBApi.wkeGlobalExec(m_webView.Handle);
                m_strName = "func" + Guid.NewGuid().ToString().Replace("-", "");

                MBApi.jsSetGlobal(es, m_strName, jsValue);
            }

            public object Invoke(params object[] param)
            {
                IntPtr es = MBApi.wkeGlobalExec(m_webView.Handle);
                long value = MBApi.jsGetGlobal(es, m_strName);

                long[] jsps = param.Select(i => m_webView.ToJsValue(i)).ToArray();
                object result = m_webView.ToNetValue(MBApi.jsCall(es, value, MBApi.jsUndefined(), jsps, jsps.Length));

                MBApi.jsSetGlobal(es, m_strName, MBApi.jsUndefined());

                return result;
            }
        }


        /// <summary>
        /// 将js值转成c#值，参考凹大神的 https://gitee.com/aochulai/NetMiniblink
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bFunExecute">指定如果是函数是否执行该函数，如果执行，则返回函数执行的结果而不是函数本身</param>
        /// <returns></returns>
        public object ToNetValue(long value, bool bFunExecute = false)
        {
            jsType type = MBApi.jsTypeOf(value);
            IntPtr es = MBApi.wkeGlobalExec(Handle);

            switch (type)
            {
                case jsType.NULL:
                case jsType.UNDEFINED:
                    {
                        return null;
                    }

                case jsType.NUMBER:
                    {
                        return MBApi.jsToDouble(es, value);
                    }

                case jsType.BOOLEAN:
                    {
                        return MBApi.jsToBoolean(es, value);
                    }

                case jsType.STRING:
                    {
                        return MBApi.jsToTempStringW(es, value).UnicodePtrToStr();
                    }

                case jsType.FUNCTION:
                    {
                        JsFunc function = new JsFunc(this, value);
                        if (bFunExecute)
                        {
                            return function.Invoke();    // 如果有参数一般是从另一个jsType传过来，具体情况具体分析
                        }

                        return function;
                    }

                case jsType.ARRAY:
                    {
                        int len = MBApi.jsGetLength(es, value);
                        object[] array = new object[len];
                        for (int i = 0; i < array.Length; i++)
                        {
                            array[i] = ToNetValue(MBApi.jsGetAt(es, value, i));
                        }

                        return array;
                    }

                case jsType.OBJECT:
                    {
                        IntPtr ptr = MBApi.jsGetKeys(es, value);
                        jsKeys keys = (jsKeys)Marshal.PtrToStructure(ptr, typeof(jsKeys));
                        string[] strKeyArr = keys.keys.PtrToStringArray(keys.length);

                        ExpandoObject exp = new ExpandoObject();
                        IDictionary<string, object> map = (IDictionary<string, object>)exp;
                        foreach (string key in strKeyArr)
                        {
                            map.Add(key, ToNetValue(MBApi.jsGet(es, value, key)));
                        }

                        return map;
                    }

                default:
                    {
                        throw new NotSupportedException();
                    }
            }
        }

        // 定义一个通用的函数形式
        public delegate object CommonFuncType(object[] param);

        /// <summary>
        /// 将c#值转成js值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="bFunExecute">指定如果是函数是否执行该函数，如果执行则返回函数执行的结果，如果不执行则返回的是函数的参数，
        /// 注意不是函数本身，这个和ToNetValue不一样，这是由于jsData结构定义导致的</param>
        /// <returns></returns>
        public long ToJsValue(object obj, bool bFunExecute = false)
        {
            IntPtr es = MBApi.wkeGlobalExec(Handle);

            if (obj == null)
            {
                return MBApi.jsUndefined();
            }
            else if (obj is int)
            {
                return MBApi.jsInt((int)obj);
            }
            else if (obj is bool)
            {
                return MBApi.jsBoolean((bool)obj);
            }
            else if (obj is double)
            {
                return MBApi.jsDouble((double)obj);
            }
            else if (obj is long)
            {
                return ToJsValue(obj.ToString());
            }
            else if (obj is float)
            {
                return MBApi.jsFloat((float)obj);
            }
            else if (obj is DateTime)
            {
                return MBApi.jsDouble(((DateTime)obj).ToLong());
            }
            else if (obj is string)
            {
                return MBApi.jsStringW(es, obj.ToString());
            }
            else if (obj is decimal)
            {
                var dec = (decimal)obj;
                if (dec.ToString().Contains("."))
                {
                    return ToJsValue(Convert.ToDouble(dec.ToString()));
                }
                else
                {
                    return ToJsValue(Convert.ToInt32(dec.ToString()));
                }
            }
            else if (obj is IEnumerable)
            {
                List<object> list = new List<object>();
                foreach (object item in (IEnumerable)obj)
                {
                    list.Add(item);
                }

                long array = MBApi.jsEmptyArray(es);
                MBApi.jsSetLength(es, array, list.Count);

                for (int i = 0; i < list.Count; i++)
                {
                    MBApi.jsSetAt(es, array, i, ToJsValue(list[i]));
                }

                return array;
            }
            else if (obj is Delegate)
            {
                Delegate func = (Delegate)obj;
                IntPtr funcptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(jsData)));

                jsData funcdata = new jsData
                {
                    typeName = "function",

                    finalize = (ptr) =>
                    {
                        Marshal.FreeHGlobal(ptr);
                    },

                    callAsFunction = (fes, fobj, fargs, fcount) =>
                    {
                        if (func is CommonFuncType)
                        {
                            var fps = new List<object>();
                            for (var i = 0; i < fcount; i++)
                            {
                                fps.Add(ToNetValue(MBApi.jsArg(fes, i)));
                            }

                            if (bFunExecute)
                            {
                                return ToJsValue(((CommonFuncType)func)(fps.ToArray()));
                            }

                            return ToJsValue(fps.ToArray());
                        }
                        else    // 如果传过来的不是动态参数，而是具体的参数
                        {
                            object[] fps = new object[func.Method.GetParameters().Length];
                            for (int i = 0; i < fcount && i < fps.Length; i++)
                            {
                                fps[i] = ToNetValue(MBApi.jsArg(fes, i));
                            }

                            if (bFunExecute)
                            {
                                return ToJsValue(func.Method.Invoke(func.Target, fps));
                            }

                            return ToJsValue(fps);
                        }
                    },
                };

                Marshal.StructureToPtr(funcdata, funcptr, false);

                return MBApi.jsFunction(es, funcptr);
            }
            else
            {
                long jsobj = MBApi.jsEmptyObject(es);

                PropertyInfo[] PropertyArr = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (var ps in PropertyArr)
                {
                    object oValue = ps.GetValue(obj, null);
                    if (oValue != null)
                    {
                        MBApi.jsSet(es, jsobj, ps.Name, ToJsValue(oValue));
                    }
                }

                return jsobj;
            }
        }

        /// <summary>
        /// 启用或禁用编辑模式
        /// </summary>
        /// <param name="editable"></param>
        public void SetEditable(bool editable)
        {
            MBApi.wkeSetEditable(Handle, editable);
        }

        /// <summary>
        /// 启用或禁用跨域检查
        /// </summary>
        /// <param name="enable"></param>
        public void SetCspCheckEnable(bool enable)
        {
            MBApi.wkeSetCspCheckEnable(Handle, enable);
        }

        /// <summary>
        /// 设置网络接口
        /// </summary>
        /// <param name="NetInterface"></param>
        public void SetNetInterface(string NetInterface)
        {
            MBApi.wkeSetViewNetInterface(Handle, NetInterface);
        }

        /// <summary>
        /// 截图
        /// </summary>
        /// <returns></returns>
        public Bitmap PrintToBitmap()
        {
            if (Handle == IntPtr.Zero)
            {
                return null;
            }
            
            MBApi.wkeRunJSW(Handle, @"document.body.style.overflow='hidden'");
            int w = MBApi.wkeGetContentWidth(Handle);
            int h = MBApi.wkeGetContentHeight(Handle);

            int oldwidth = MBApi.wkeGetWidth(Handle);
            int oldheight = MBApi.wkeGetHeight(Handle);

            MBApi.wkeResize(Handle, w, h);

            Bitmap bmp = new Bitmap(w, h);
            Rectangle rc = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(rc, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            MBApi.wkePaint(Handle, data.Scan0, 0);
            bmp.UnlockBits(data);

            MBApi.wkeResize(Handle, oldwidth, oldheight);
            MBApi.wkeRunJSW(Handle, @"document.body.style.overflow='visible'");

            return bmp;
        }

        /// <summary>
        /// 设置调试配置
        /// </summary>
        /// <param name="debugString">"showDevTools" 是开启devtools功能，参数为：front_end/inspector.html(utf8编码)</param>
        /// <param name="param"></param>
        public void SetDebugConfig(string debugString, string param)
        {
            MBApi.wkeSetDebugConfig(Handle, debugString, Encoding.UTF8.GetBytes(param));
        }

        /// <summary>
        /// 获取调试配置
        /// </summary>
        /// <param name="debugString"></param>
        /// <returns></returns>
        public string GetDebugConfig(string debugString)
        {
            return MBApi.wkeGetDebugConfig(Handle, debugString).UTF8PtrToStr();
        }

        /// <summary>
        /// 设置上下文菜单项目是否显示
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isShow"></param>
        public void SetContextMenuItemShow(wkeMenuItemId item, bool isShow)
        {
            MBApi.wkeSetContextMenuItemShow(Handle, item, isShow);
        }

        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="language"></param>
        public void SetLanguage(string language)
        {
            MBApi.wkeSetLanguage(Handle, language);
        }

        /// <summary>
        /// 设置设备参数
        /// </summary>
        /// <param name="device">如：“navigator.platform”</param>
        /// <param name="paramStr"></param>
        /// <param name="paramInt"></param>
        /// <param name="paramFloat"></param>
        public void SetDeviceParameter(string device, string paramStr, int paramInt = 0, float paramFloat = 0)
        {
            MBApi.wkeSetDeviceParameter(Handle, device, paramStr, paramInt, paramFloat);
        }

        /// <summary>
        /// 显示DevTools窗口
        /// </summary>
        /// <param name="Path">路径</param>
        /// <param name="Callback"></param>
        public void ShowDevtools(string Path, wkeOnShowDevtoolsCallback Callback, IntPtr Param)
        {
            MBApi.wkeShowDevtools(Handle, Path, Callback, Param);
        }

        /// <summary>
        /// 删除将发送请求的信息
        /// </summary>
        /// <param name="WillSendRequestInfoPtr"></param>
        public void DeleteWillSendRequestInfo(IntPtr WillSendRequestInfoPtr)
        {
            MBApi.wkeDeleteWillSendRequestInfo(Handle, WillSendRequestInfoPtr);
        }

        /// <summary>
        /// 在指定框架插入CSS
        /// </summary>
        /// <param name="FrameId">框架ID</param>
        /// <param name="cssText"></param>
        public void InsertCSSByFrame(IntPtr FrameId, string cssText)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(cssText);
            MBApi.wkeInsertCSSByFrame(Handle, FrameId, utf8);
        }

        /// <summary>
        /// 序列化到MHTML
        /// </summary>
        /// <returns></returns>
        public string SerializeToMHTML()
        {
            return MBApi.wkeUtilSerializeToMHTML(Handle).UTF8PtrToStr();
        }

        /// <summary>
        /// 获取网页HTML
        /// </summary>
        /// <returns></returns>
        public string GetSource()
        {
            return MBApi.wkeGetSource(Handle).UTF8PtrToStr();
        }

        /// <summary>
        /// 设置视图配置，可设置尺寸和背景色
        /// </summary>
        /// <param name="settings"></param>
        public void SetViewSettings(wkeViewSettings settings)
        {
            MBApi.wkeSetViewSettings(Handle, settings);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取 WebView 句柄
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// 获取宿主窗口句柄
        /// </summary>
        public IntPtr HostHandle
        {
            get { return MBApi.wkeGetHostHWND(Handle); }
        }

        /// <summary>
        /// 获取是否处于唤醒状态
        /// </summary>
        public bool IsWake
        {
            get { return MBApi.wkeIsAwake(Handle) != 0; }
        }

        /// <summary>
        /// 获取是否正在载入
        /// </summary>
        public bool IsLoading
        {
            get { return MBApi.wkeIsLoading(Handle) != 0; }
        }

        /// <summary>
        /// 获取是否载入失败
        /// </summary>
        public bool IsLoadingFailed
        {
            get { return MBApi.wkeIsLoadingFailed(Handle) != 0; }
        }

        /// <summary>
        /// 获取是否载入完成
        /// </summary>
        public bool IsLoadingCompleted
        {
            get { return MBApi.wkeIsLoadingCompleted(Handle) != 0; }
        }

        /// <summary>
        /// 获取文档是否就绪
        /// </summary>
        public bool IsDocumentReady
        {
            get { return MBApi.wkeIsDocumentReady(Handle) != 0; }
        }

        /// <summary>
        /// 获取标题
        /// </summary>
        public string Title
        {
            get
            {
                IntPtr pTitle = MBApi.wkeGetTitleW(Handle);
                if (pTitle != IntPtr.Zero)
                {
                    return Marshal.PtrToStringUni(pTitle);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取宽度
        /// </summary>
        public int Width
        {
            get { return MBApi.wkeGetWidth(Handle); }
        }

        /// <summary>
        /// 获取高度
        /// </summary>
        public int Height
        {
            get { return MBApi.wkeGetHeight(Handle); }
        }

        /// <summary>
        /// 获取内容宽度
        /// </summary>
        public int ContentWidth
        {
            get { return MBApi.wkeGetContentWidth(Handle); }
        }

        /// <summary>
        /// 获取内容高度
        /// </summary>
        public int ContentHeight
        {
            get { return MBApi.wkeGetContentHeight(Handle); }
        }

        /// <summary>
        /// 是否透明
        /// </summary>
        public bool Transparent
        {
            get { return m_bTransparent; }
            set 
            {
                m_bTransparent = value;
                MBApi.wkeSetTransparent(Handle, value); 
            }
        }

        /// <summary>
        /// 获取是否能后退操作
        /// </summary>
        public bool CanGoBack
        {
            get { return MBApi.wkeCanGoBack(Handle) != 0; }
        }

        /// <summary>
        /// 获取是否能前进操作
        /// </summary>
        public bool CanGoForward
        {
            get { return MBApi.wkeCanGoForward(Handle) != 0; }
        }

        /// <summary>
        /// 获取或设置Cookie引擎是否启用
        /// </summary>
        public bool CookieEnabled
        {
            get { return MBApi.wkeIsCookieEnabled(Handle) != 0; }
            set { MBApi.wkeSetCookieEnabled(Handle, value); }
        }

        /// <summary>
        /// 获取或设置媒体音量
        /// </summary>
        public float MediaVolume
        {
            get { return MBApi.wkeGetMediaVolume(Handle); }
            set { MBApi.wkeSetMediaVolume(Handle, value); }
        }

        /// <summary>
        /// 获取或设置缩放因子
        /// </summary>
        public float ZoomFactor
        {
            get { return MBApi.wkeGetZoomFactor(Handle); }
            set { MBApi.wkeSetZoomFactor(Handle, value); }
        }

        /// <summary>
        /// 获取主框架句柄
        /// </summary>
        public IntPtr MainFrame
        {
            get { return MBApi.wkeWebFrameGetMainFrame(Handle); }
        }

        /// <summary>
        /// 获取或设置光标类型
        /// </summary>
        public wkeCursorStyle CursorInfoType
        {
            get { return MBApi.wkeGetCursorInfoType(Handle); }
            set { MBApi.wkeSetCursorInfoType(Handle, value); }
        }

        /// <summary>
        /// 设置是否启用内存缓存
        /// </summary>
        /// <param name="Enable"></param>
        public bool MemoryCacheEnable
        {
            set { MBApi.wkeSetMemoryCacheEnable(Handle, value); }
        }

        /// <summary>
        /// 设置是否导航到新窗口
        /// </summary>
        /// <param name="enable">如果为false 则不会弹出新窗口</param>
        public bool NavigationToNewWindowEnable
        {
            set { MBApi.wkeSetNavigationToNewWindowEnable(Handle, value); }
        }

        /// <summary>
        /// 设置是否启用触摸
        /// </summary>
        public bool TouchEnable
        {
            set { MBApi.wkeSetTouchEnabled(Handle, value); }
        }

        /// <summary>
        /// 设置是否启用 NPAPI 插件
        /// </summary>
        public bool NpapiPluginsEnabled
        {
            set { MBApi.wkeSetNpapiPluginsEnabled(Handle, value); }
        }

        /// <summary>
        /// 设置是否启用无头模式，可以关闭渲染
        /// </summary>
        public bool HeadlessEnabled
        {
            set { MBApi.wkeSetHeadlessEnabled(Handle, value); }
        }

        /// <summary>
        /// 设置是否启用拖拽，可关闭拖拽文件加载网页
        /// </summary>
        public bool DragEnable
        {
            set { MBApi.wkeSetDragEnable(Handle, value); }
        }

        /// <summary>
        /// 设置或获取是否禁用鼠标
        /// </summary>
        public bool MouseEnabled
        {
            get { return m_bMouseEnabled; }
            set { MBApi.wkeSetMouseEnabled(Handle, value); }
        }

        /// <summary>
        /// 可关闭拖拽到其他进程
        /// </summary>
        public bool DragDropEnable
        {
            set { MBApi.wkeSetDragDropEnable(Handle, value); }
        }

        /// <summary>
        /// 设置资源回收间隔
        /// </summary>
        public int ResourceGc
        {
            set { MBApi.wkeSetResourceGc(Handle, value); }
        }

        /// <summary>
        /// 获取是否真正处理手势
        /// </summary>
        public bool IsProcessingUserGesture
        {
            get { return MBApi.wkeIsProcessingUserGesture(Handle) != 0; }
        }

        #endregion
    }

    #region 事件参数
    public class MiniblinkEventArgs : EventArgs
    {
        public IntPtr Handle { get; }

        public MiniblinkEventArgs(IntPtr webView)
        {
            Handle = webView;
        }
    }

    /// <summary>
    /// OnMouseOverUrlChanged 事件参数
    /// </summary>
    public class MouseOverUrlChangedEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;

        public MouseOverUrlChangedEventArgs(IntPtr webView, IntPtr url) : base(webView)
        {
            m_url = url;
        }

        public string URL
        {
            get { return MBApi.wkeGetStringW(m_url).UnicodePtrToStr(); }
        }
    }

    /// <summary>
    /// OnTitleChange 事件参数
    /// </summary>
    public class TitleChangeEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_title;

        public TitleChangeEventArgs(IntPtr webView, IntPtr title) : base(webView)
        {
            m_title = title;
        }

        public string Title
        {
            get { return MBApi.wkeGetStringW(m_title).UnicodePtrToStr(); }
        }
    }

    /// <summary>
    /// OnUrlChange 事件参数
    /// </summary>
    public class UrlChangeEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;

        public UrlChangeEventArgs(IntPtr webView, IntPtr url, IntPtr webFrame) : base(webView)
        {
            m_url = url;
            WebFrame = webFrame;
        }

        public string URL
        {
            get { return MBApi.wkeGetStringW(m_url).UnicodePtrToStr(); }
        }

        public IntPtr WebFrame { get; }
    }

    /// <summary>
    /// OnAlertBox事件参数
    /// </summary>
    public class AlertBoxEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_msg;

        public AlertBoxEventArgs(IntPtr webView, IntPtr msg) : base(webView)
        {
            m_msg = msg;
        }

        public string Msg
        {
            get { return MBApi.wkeGetStringW(m_msg).UnicodePtrToStr(); }
        }
    }

    /// <summary>
    /// OnConfirmBox事件参数
    /// </summary>
    public class ConfirmBoxEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_msg;
        public bool Result { get; set; }

        public ConfirmBoxEventArgs(IntPtr webView, IntPtr msg) : base(webView)
        {
            m_msg = msg;
        }

        public string Msg
        {
            get { return MBApi.wkeGetStringW(m_msg).UnicodePtrToStr(); }
        }
    }

    /// <summary>
    /// OnPromptBox事件参数
    /// </summary>
    public class PromptBoxEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_msg;
        private IntPtr m_defaultStr;
        private IntPtr m_resultStr;

        public PromptBoxEventArgs(IntPtr webView, IntPtr msg, IntPtr defaultResult, IntPtr result) : base(webView)
        {
            m_msg = msg;
            m_defaultStr = defaultResult;
            m_resultStr = result;
        }

        public bool Result { get; set; }

        public string Msg
        {
            get { return MBApi.wkeGetStringW(m_msg).UnicodePtrToStr(); }
        }

        public string DefaultResultString
        {
            get { return MBApi.wkeGetStringW(m_defaultStr).UnicodePtrToStr(); }
        }

        public string ResultString
        {
            set { MBApi.wkeSetStringW(m_resultStr, value, value.Length); }
        }
    }

    /// <summary>
    /// OnNavigate事件参数
    /// </summary>
    public class NavigateEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;

        public NavigateEventArgs(IntPtr webView, wkeNavigationType navigationType, IntPtr url) : base(webView)
        {
            NavigationType = navigationType;
            m_url = url;
        }

        public wkeNavigationType NavigationType { get; }
        
        public bool Cancel { get; set; }

        public string URL
        {
            get { return MBApi.wkeGetStringW(m_url).UnicodePtrToStr(); }
        }
    }

    /// <summary>
    /// OnCreateView事件参数
    /// </summary>
    public class CreateViewEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;
        private IntPtr m_windowFeatures;

        public CreateViewEventArgs(IntPtr webView, wkeNavigationType navigationType, IntPtr url, IntPtr windowFeatures) : base(webView)
        {
            NavigationType = navigationType;
            m_url = url;
            m_windowFeatures = windowFeatures;
            NewWebViewHandle = webView;
        }

        public wkeNavigationType NavigationType { get; }

        public IntPtr NewWebViewHandle { get; set; }

        public string URL
        {
            get { return MBApi.wkeGetStringW(m_url).UnicodePtrToStr(); }
        }

        public wkeWindowFeatures WindowFeatures
        {
            get { return (wkeWindowFeatures)m_windowFeatures.UTF8PtrToStruct(typeof(wkeWindowFeatures)); }
        }
    }

    /// <summary>
    /// OnDocumentReady
    /// </summary>
    public class DocumentReadyEventArgs : MiniblinkEventArgs
    {
        public DocumentReadyEventArgs(IntPtr webView, IntPtr frame) : base(webView)
        {
            Frame = frame;
        }

        public IntPtr Frame { get; }
    }

    /// <summary>
    /// OnLoadingFinish事件参数
    /// </summary>
    public class LoadingFinishEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;
        private IntPtr m_failedReason;

        public LoadingFinishEventArgs(IntPtr webView, IntPtr url, wkeLoadingResult result, IntPtr failedReason) : base(webView)
        {
            m_url = url;
            LoadingResult = result;
            m_failedReason = failedReason;
        }

        public wkeLoadingResult LoadingResult { get; }

        public string URL
        {
            get { return MBApi.wkeGetStringW(m_url).UnicodePtrToStr(); }
        }

        public string FailedReason
        {
            get { return MBApi.wkeGetStringW(m_failedReason).UnicodePtrToStr(); }
        }
    }

    /// <summary>
    /// OnDownload事件参数
    /// </summary>
    public class DownloadEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;

        public DownloadEventArgs(IntPtr webView, IntPtr url) : base(webView)
        {
            m_url = url;
        }

        public string SaveFilePath { get; set; }

        public bool Cancel { get; set; }

        public long ContentLength { get; set; }

        public string URL
        {
            get { return m_url.UTF8PtrToStr(); }
        }

        public event EventHandler<DownloadProgressEventArgs> Progress;

        public void OnProgress(DownloadProgressEventArgs e)
        {
            Progress?.Invoke(this, e);
        }

        public event EventHandler<DownloadFinishEventArgs> Finish;

        public void OnFinish(DownloadFinishEventArgs e)
        {
            Finish?.Invoke(this, e);
        }
    }

    /// <summary>
    /// OnDownload2事件参数，貌似有问题，经常报“c#尝试读取或写入受保护的内存。这通常指示其他内存已损坏”
    /// </summary>
    public class DownloadEventArgs2 : MiniblinkEventArgs
    {
        private IntPtr m_url;
        private IntPtr m_mime;
        private IntPtr m_disposition;
        private IntPtr m_dataBind;

        public DownloadEventArgs2(IntPtr webView, IntPtr param, uint expectedContentLength, IntPtr url, IntPtr mime, IntPtr disposition, IntPtr job, IntPtr dataBind) : base(webView)
        {
            m_url = url;
            m_mime = mime;
            m_disposition = disposition;
            m_dataBind = dataBind;

            ContentLength = expectedContentLength;
            Job = job;
        }

        public string SaveFilePath { get; set; }

        public bool Cancel { get; set; }

        public long ContentLength { get; set; }

        public IntPtr Job { get; }

        public string URL
        {
            get { return m_url.UTF8PtrToStr(); }
        }

        public string Mime
        {
            get { return m_mime.UTF8PtrToStr(); }
        }

        public string Disposition
        {
            get { return m_disposition.UTF8PtrToStr(); }
        }

        public string DataBind
        {
            get { return m_dataBind.UTF8PtrToStr(); }
        }

        public event EventHandler<DownloadProgressEventArgs> Progress;

        public void OnProgress(DownloadProgressEventArgs e)
        {
            Progress?.Invoke(this, e);
        }

        public event EventHandler<DownloadFinishEventArgs> Finish;

        public void OnFinish(DownloadFinishEventArgs e)
        {
            Finish?.Invoke(this, e);
        }
    }

    /// <summary>
    /// 下载过程事件参数
    /// </summary>
    public class DownloadProgressEventArgs : EventArgs
    {
        public long Total { get; set; }
        public long Received { get; set; }
        public byte[] Data { get; set; }
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// 下载完成事件参数
    /// </summary>
    public class DownloadFinishEventArgs : EventArgs
    {
        public Exception Error { get; set; }
        public bool IsCompleted { get; set; }
    }

    /// <summary>
    /// OnConsole事件参数
    /// </summary>
    public class ConsoleEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_message;
        private IntPtr m_sourceName;
        private IntPtr m_stackTrace;

        public ConsoleEventArgs(IntPtr webView, wkeConsoleLevel level, IntPtr message, IntPtr sourceName, uint sourceLine, IntPtr stackTrace) : base(webView)
        {
            Level = level;
            m_message = message;
            m_sourceName = sourceName;
            SourceLine = sourceLine;
            m_stackTrace = stackTrace;
        }

        public wkeConsoleLevel Level { get; }

        public uint SourceLine { get; }

        public string Message
        {
            get { return MBApi.wkeGetStringW(m_message).UTF8PtrToStr(); }
        }

        public string SourceName
        {
            get { return MBApi.wkeGetStringW(m_sourceName).UTF8PtrToStr(); }
        }

        public string StackTrace
        {
            get { return MBApi.wkeGetStringW(m_stackTrace).UTF8PtrToStr(); }
        }
    }

    /// <summary>
    /// OnLoadUrlBegin事件参数
    /// </summary>
    public class LoadUrlBeginEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;

        public LoadUrlBeginEventArgs(IntPtr webView, IntPtr url, IntPtr job) : base(webView)
        {
            m_url = url;
            Job = job;
        }

        public string URL
        {
            get { return m_url.UTF8PtrToStr(); }
        }

        public IntPtr Job { get; }
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// OnLoadUrlEnd事件参数
    /// </summary>
    public class LoadUrlEndEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;
        private IntPtr m_buf;

        public LoadUrlEndEventArgs(IntPtr webView, IntPtr url, IntPtr job, IntPtr buf, int len) : base(webView)
        {
            m_url = url;
            Job = job;
            m_buf = buf;
            Len = len;
        }

        public IntPtr Job { get; }

        public int Len { get; }

        public string URL
        {
            get { return m_url.UTF8PtrToStr(); }
        }

        public byte[] Data
        {
            get { return m_buf.StructToBytes(); }
            set { m_buf = Data.ByteToUtf8Ptr(); }
        }
    }

    /// <summary>
    /// OnLoadUrlFail事件参数
    /// </summary>
    public class LoadUrlFailEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;

        public LoadUrlFailEventArgs(IntPtr webView, IntPtr url, IntPtr job) : base(webView)
        {
            m_url = url;
            Job = job;
        }

        public IntPtr Job { get; }

        public string URL
        {
            get { return m_url.UTF8PtrToStr(); }
        }
    }

    /// <summary>
    /// OnDidCreateScriptContext
    /// </summary>
    public class DidCreateScriptContextEventArgs : MiniblinkEventArgs
    {
        public DidCreateScriptContextEventArgs(IntPtr webView, IntPtr frame, IntPtr context, int extensionGroup, int worldId) : base(webView)
        {
            Frame = frame;
            Context = context;
            ExtensionGroup = extensionGroup;
            WorldId = worldId;
        }

        public IntPtr Frame { get; }
        public IntPtr Context { get; }
        public int ExtensionGroup { get; }
        public int WorldId { get; }
    }

    /// <summary>
    /// OnWillReleaseScriptContext 事件参数
    /// </summary>
    public class WillReleaseScriptContextEventArgs : MiniblinkEventArgs
    {
        public WillReleaseScriptContextEventArgs(IntPtr webView, IntPtr frame, IntPtr context, int worldId) : base(webView)
        {
            Frame = frame;
            Context = context;
            WorldId = worldId;
        }

        public IntPtr Frame { get; }
        public IntPtr Context { get; }
        public int WorldId { get; }
    }

    /// <summary>
    /// OnNetResponse 事件参数
    /// </summary>
    public class NetResponseEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;

        public NetResponseEventArgs(IntPtr webView, IntPtr url, IntPtr job) : base(webView)
        {
            m_url = url;
            Job = job;
        }

        public string URL
        {
            get { return m_url.UTF8PtrToStr(); }
        }

        public IntPtr Job { get; }
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// OnWillMediaLoad 事件参数
    /// </summary>
    public class WillMediaLoadEventArgs : MiniblinkEventArgs
    {
        private IntPtr m_url;
        private wkeMediaLoadInfo m_info;

        public WillMediaLoadEventArgs(IntPtr webView, IntPtr url, IntPtr info) : base(webView)
        {
            m_url = url;
            m_info = (wkeMediaLoadInfo)Marshal.PtrToStructure(info, typeof(wkeMediaLoadInfo));
        }

        public string URL
        {
            get { return m_url.UTF8PtrToStr(); }
        }

        public wkeMediaLoadInfo Info
        {
            get { return m_info; }
        }
    }

    /// <summary>
    /// OnOtherLoad 事件参数
    /// </summary>
    public class OtherLoadEventArgs : MiniblinkEventArgs
    {
        private wkeTempCallbackInfo m_info;

        public OtherLoadEventArgs(IntPtr webView, wkeOtherLoadType type, IntPtr info) : base(webView)
        {
            LoadType = type;
            m_info = (wkeTempCallbackInfo)Marshal.PtrToStructure(info, typeof(wkeTempCallbackInfo));
        }

        public wkeOtherLoadType LoadType { get; }

        public int Size
        {
            get { return m_info.size; }
        }

        public IntPtr Frame
        {
            get { return m_info.frame; }
        }

        public wkeWillSendRequestInfo WillSendRequestInfo
        {
            get
            {
                wkeWillSendRequestInfo srInfo = new wkeWillSendRequestInfo();
                if (m_info.willSendRequestInfo != IntPtr.Zero)
                {
                    srInfo.isHolded = Marshal.ReadInt32(m_info.willSendRequestInfo) != 0;

                    IntPtr ptr = Marshal.ReadIntPtr(m_info.willSendRequestInfo, 4);
                    srInfo.url = MBApi.wkeGetStringW(ptr).UTF8PtrToStr();

                    ptr = Marshal.ReadIntPtr(m_info.willSendRequestInfo, 8);
                    srInfo.newUrl = MBApi.wkeGetStringW(ptr).UTF8PtrToStr();

                    srInfo.resourceType = (wkeResourceType)Marshal.ReadInt32(m_info.willSendRequestInfo, 12);
                    srInfo.httpResponseCode = Marshal.ReadInt32(m_info.willSendRequestInfo, 16);
                    ptr = Marshal.ReadIntPtr(m_info.willSendRequestInfo, 20);
                    srInfo.method = MBApi.wkeGetStringW(ptr).UTF8PtrToStr();

                    ptr = Marshal.ReadIntPtr(m_info.willSendRequestInfo, 24);
                    srInfo.referrer = MBApi.wkeGetStringW(ptr).UTF8PtrToStr();

                    srInfo.headers = Marshal.ReadIntPtr(m_info.willSendRequestInfo, 28);
                }

                return srInfo;
            }
        }

        public IntPtr WillSendRequestInfoPtr
        {
            get { return m_info.willSendRequestInfo; }
        }
    }

    /// <summary>
    /// 窗口过程事件参数
    /// </summary>
    public class WindowProcEventArgs : EventArgs
    {
        public WindowProcEventArgs(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            Handle = hWnd;
            Msg = msg;
            this.wParam = wParam;
            this.lParam = lParam;
        }

        public IntPtr Handle { get; }
        public int Msg { get; }
        public IntPtr wParam { get; }
        public IntPtr lParam { get; }
        public IntPtr Result { get; set; }

        /// <summary>
        /// 如果为 true 则会返回 Result ，false 则返回默认值
        /// </summary>
        public bool bHand { get; set; }
    }

    #endregion
}
