using Script;
using UnityEngine;

namespace Carry1st.Platform.Sdk
{
    public class PlatformPayments : MonoBehaviour
    {
        private string PLATFORM = null;
        private string BASE_URL = null;

        private UniWebView _uniWebView;
        private GameObject _webViewGameObject;
        
        // delegates
        public delegate void PageStartedDelegate(UniWebView webView, string url);
        public event PageStartedDelegate OnPageStarted;
    
        public delegate void PageErrorReceivedDelegate(UniWebView webView, int errorCode, string errorMessage);
        public event PageErrorReceivedDelegate OnPageErrorReceived;
        
        public delegate void PageFinishedDelegate(UniWebView webView, int statusCode, string url);
        public event PageFinishedDelegate OnPageFinished;
        
        public delegate void PageProgressChangedDelegate(UniWebView webView, float progress);
        public event PageProgressChangedDelegate OnPageProgressChanged;
        
        public delegate void MessageReceivedDelegate(UniWebView webView, UniWebViewMessage message);
        public event MessageReceivedDelegate OnMessageReceived;
        
        public delegate bool ShouldCloseDelegate(UniWebView webView);
        public event ShouldCloseDelegate OnShouldClose;
        
        public delegate void OnWebContentProcessTerminatedDelegate(UniWebView webView);
        public event OnWebContentProcessTerminatedDelegate OnWebContentProcessTerminated;

        public delegate void MultipleWindowOpenedDelegate(UniWebView webView, string multipleWindowId);
        public event MultipleWindowOpenedDelegate OnMultipleWindowOpened;
        
        public delegate void MultipleWindowClosedDelegate(UniWebView webView, string multipleWindowId);
        public event MultipleWindowClosedDelegate OnMultipleWindowClosed;
        
        void Start()
        {
            PLATFORM = GETPlatform();
            PlayerPrefs.SetString("carry1st_payments_platform", PLATFORM);
           
        }

        public void SetupListeners()
        {
            _uniWebView.OnPageStarted += (view, url) =>
            {
                OnPageStarted?.Invoke(view, url);
            };

            _uniWebView.OnPageFinished += (view, statusCode, url) =>
            {
                
                OnPageFinished?.Invoke(view, statusCode, url);
            };

            _uniWebView.OnMessageReceived += (view, message) =>
            {
                OnMessageReceived?.Invoke(view,message);
            };

            _uniWebView.OnPageErrorReceived += (view, errorCode, errorMessage) =>
            {
                OnPageErrorReceived?.Invoke(view, errorCode, errorMessage);
            };

            _uniWebView.OnPageStarted += (view, url) =>
            {
                OnPageStarted?.Invoke(view, url);
            };

            _uniWebView.OnPageProgressChanged += (view, progress) =>
            {
                OnPageProgressChanged?.Invoke(view,progress);
            };

            _uniWebView.OnMultipleWindowOpened += (view, multipleWindowId) =>
            {
                OnMultipleWindowOpened?.Invoke(view, multipleWindowId);
            };
            _uniWebView.OnMultipleWindowClosed += (view, multipleWindowId) =>
            {
                OnMultipleWindowClosed?.Invoke(view, multipleWindowId);
            };
            _uniWebView.OnWebContentProcessTerminated += (view) =>
            {
                OnWebContentProcessTerminated?.Invoke(view);
            };
        }
       
        public void SetStoreBaseUrl(string url)
        {
            BASE_URL = url;
        }

        private void SetupWebView()
        {
            if (_uniWebView == null)
            {
                _uniWebView = FindObjectOfType<UniWebView>();
                if (_uniWebView == null)
                {
                    _webViewGameObject = GameObject.Find("UniWebView");
                    if (_webViewGameObject == null)
                    {
                        _webViewGameObject = new GameObject("UniWebView");
                        _uniWebView = _webViewGameObject.AddComponent<UniWebView>();
                        _uniWebView.Frame = new Rect(0, 0, Screen.width, Screen.height);
                        
                        _uniWebView.OnShouldClose += (view) =>
                        {
                            OnShouldClose?.Invoke(view);
                            CloseWebView();
                            return true;
                        };

                        _uniWebView.OnOrientationChanged += (view, orientation) =>
                        {
                            _uniWebView.Frame = new Rect(0, 0, Screen.width, Screen.height);

                        };

                    }
                }
            }
        }
        public void CloseWebView()
        {
            Destroy(_uniWebView);
            Destroy(_webViewGameObject);
            _uniWebView = null;
            _webViewGameObject = null;
        }
        
        private void OnDestroy()
        {
            CloseWebView();
        }

        public void OpenStore(string bundleId, string username, string countryCode)
        {
            SetupWebView();
            SetupListeners();
            if (PLATFORM != null && BASE_URL != null && _uniWebView != null)
            {
                Load(bundleId, username, countryCode);
            }
        }

        void Load(string bundleId, string username, string countryCode)
        {
            _uniWebView.Load(BASE_URL+"?platform="+PLATFORM+"&productBundleId="+bundleId+"&username="+username+"&countryCode="+countryCode);
            _uniWebView.Show();
        }

        private void OnEnable()
        {
            PLATFORM = PlayerPrefs.GetString("platform");
        }

        private static string GETPlatform()
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
