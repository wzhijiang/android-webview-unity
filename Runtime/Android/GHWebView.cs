using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Android
{
    public class GHWebView : MonoBehaviour
    {
        private static AndroidJavaClass s_JavaClass;
        public static AndroidJavaClass JavaClass
        {
            get
            {
                if (s_JavaClass == null)
                {
                    s_JavaClass = new AndroidJavaClass("io.github.wzhijiang.android.webview.GHWebView");
                }
                return s_JavaClass;
            }
        }

        private AndroidJavaObject m_JavaObject;
        public AndroidJavaObject JavaObject
        {
            get { return m_JavaObject; }
        }

        private int m_Width;
        private int m_Height;

        private WebSettings m_WebSettings;

        public GHWebView(AndroidJavaObject jContext, AndroidJavaObject jParent, int width, int height)
        {
            m_Width = width;
            m_Height = height;

            m_JavaObject = JavaClass.CallStatic<AndroidJavaObject>("create", jContext, jParent, width, height, 0);
        }

        public void LoadUrl(string url)
        {
            m_JavaObject.Call("loadUrl", url);
        }

        public void SetSurface(AndroidJavaObject jSurface)
        {
            m_JavaObject.Call("setSurface", jSurface);
        }

        public bool DispatchTouchEvent(int x, int y, int action)
        {
            return m_JavaObject.Call<bool>("dispatchTouchEvent", x, y, action);
        }

        public int GetWidth()
        {
            return m_Width;
        }

        public int GetHeight()
        {
            return m_Height;
        }

        public WebSettings GetSettings()
        {
            if (m_WebSettings == null)
            {
                AndroidJavaObject jWebSettings = m_JavaObject.Call<AndroidJavaObject>("getSettings");
                m_WebSettings = new WebSettings(jWebSettings);
            }
            return m_WebSettings;
        }
    }
}
