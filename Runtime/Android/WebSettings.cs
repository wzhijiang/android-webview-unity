using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Android
{
    public class WebSettings
    {
        private AndroidJavaObject m_JavaObject;
        public AndroidJavaObject JavaObject
        {
            get { return m_JavaObject; }
        }

        public WebSettings(AndroidJavaObject javaObject)
        {
            m_JavaObject = javaObject;
        }

        public void SetUserAgentString(string ua)
        {
            m_JavaObject.Call("setUserAgentString", ua);
        }

        public string GetUserAgentString()
        {
            return m_JavaObject.Call<string>("getUserAgentString");
        }
    }
}