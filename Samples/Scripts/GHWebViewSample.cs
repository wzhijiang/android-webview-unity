using Igw.Android;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Samples
{
    public class GHWebViewSample : MonoBehaviour
    {
        // See https://developers.whatismybrowser.com/useragents/explore/hardware_type_specific/tablet/
        public const string USER_AGENT_TABLET = 
            "Mozilla/5.0 (iPad; CPU OS 11_0 like Mac OS X) AppleWebKit/604.1.34 (KHTML, like Gecko) Version/11.0 Mobile/15A5341f Safari/604.1";
        public const string USER_AGENT_MOBILE =
            "Mozilla/5.0 (Linux; Android 11) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.82 Mobile Safari/537.36";
        public const string USER_AGENT_DESKTOP =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.101 Safari/537.36";

        [SerializeField]
        private Renderer m_Renderer;
        [SerializeField]
        private TouchDetector m_TouchDetector;
        
        private GHWebView m_WebView;

        private Surface m_Surface;
        private ExternalTexture m_ExternalTexture;

        private Texture2D m_Texture;

        private Handler m_Handler;

        IEnumerator Start()
        {
            m_Handler = new Handler(Looper.GetMainLooper());
            m_TouchDetector.onTouch += OnTouch;

            m_ExternalTexture = new ExternalTexture(null, 1920, 1080);
            yield return m_ExternalTexture.WaitForInitialized();

            InitSurface(1920, 1080);

            yield return m_Handler.PostAsync(() =>
            {
                m_WebView = new GHWebView(UnityPlayerActivity.CurrentActivity, UnityPlayerActivity.UnityPlayer.JavaObject, 1920, 1080);
                m_WebView.SetSurface(m_Surface.JavaObject);

                m_WebView.GetSettings().SetUserAgentString(USER_AGENT_TABLET);

                m_WebView.LoadUrl("https://www.bilibili.com");
            });
        }

        void Update()
        {
            m_ExternalTexture.UpdateTexture();
        }

        void InitSurface(int width, int height)
        {
            m_Surface = new Surface(m_ExternalTexture.GetSurfaceTexture());

            int textureId = m_ExternalTexture.GetTextureId();
            Debug.LogFormat("Texture created: {0}", textureId);

            m_Texture = Texture2D.CreateExternalTexture(width, height, TextureFormat.RGBA32, false, true, (IntPtr)textureId);
            m_Renderer.sharedMaterial.mainTexture = m_Texture;
        }

        void OnTouch(Vector2 point, int action)
        {
            m_Handler.Post(() =>
            {
                m_WebView.DispatchTouchEvent((int)(m_WebView.GetWidth() * point.x), (int)(m_WebView.GetHeight() * point.y), action);
            });
        }
    }
}