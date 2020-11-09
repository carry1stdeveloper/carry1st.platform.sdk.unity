using Dependencies.UniWebView.Script;
using UnityEngine;

namespace Carry1st.Platform.Sdk 
{
    public class Carry1stPayments : MonoBehaviour
    {
        public string PLATFORM = null;
        private string BASE_URL = null;

        private UniWebView _uniWebView = null;
        // set platform
        // set base url to be passed in
        // set instance of uniwebview

        // Start is called before the first frame update
        void Start()
        {
            PLATFORM = getPlatform();
            PlayerPrefs.SetString("carry1st_payments_platform", PLATFORM);

            // setup webview
            SetupWebView();


            // handle messages
            _uniWebView.OnMessageReceived += (webview, message) => { };


            // handle orientation changes
            _uniWebView.OnOrientationChanged += (webview, deviceOrientation) =>
            {
                _uniWebView.Frame = new Rect(0, 0, Screen.width, Screen.height);
            };
            // handle closing of webview
            _uniWebView.OnShouldClose += (view) =>
            {
                _uniWebView = null;
                return true;
            };
        }

        // set store url when loading.
        public void SetStoreBaseUrl(string url)
        {
            BASE_URL = url;
        }

        private void SetupWebView()
        {
            var webViewGameObject = new GameObject("UniWebView");
            _uniWebView = webViewGameObject.AddComponent<UniWebView>();
            _uniWebView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        }

        public void OpenStore(string bundleId, string username, string countryCode)
        {
            string platform = getPlatform();
            if (platform != null && BASE_URL != null)
            {
            }
        }


        private static string getPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return "AND";
                case RuntimePlatform.IPhonePlayer:
                    return "IOS";
                case RuntimePlatform.WindowsPlayer:
                    return "WEB";
                case RuntimePlatform.OSXPlayer:
                    return "WEB";
                case RuntimePlatform.LinuxPlayer:
                    return "WEB";
                default:
                    return "WEB";
            }
        }
    }
}