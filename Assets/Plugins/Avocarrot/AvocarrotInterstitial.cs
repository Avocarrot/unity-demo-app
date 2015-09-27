using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using UnityEngine.UI;

#if UNITY_ANDROID

public class AvocarrotInterstitial {
	
	AndroidJavaObject _plugin;
	
	public Action onAdLoadedEvent;
	public Action onAdErrorEvent;
	public Action onAdDisplayedEvent;
	public Action onAdClickedEvent;
	public Action onAdDismissedEvent;
	
	public AvocarrotInterstitial (string apiKey, string placement) {
		
		GameObject avocarrotGameObj = new GameObject ("AvocarrotInterstitial_"+DateTime.Now.Ticks.ToString());
		AvocarrotInterstitialScript avocarrotScript = avocarrotGameObj.AddComponent( typeof ( AvocarrotInterstitialScript ) ) as AvocarrotInterstitialScript;
		avocarrotScript.setAvocarrot (this);

		AndroidJavaClass pluginClass = new AndroidJavaClass("com.avocarrot.androidsdk.UnityAvocarrotInterstitial");
		_plugin = pluginClass.CallStatic<AndroidJavaObject> ("instance", new object[] { avocarrotGameObj.name, apiKey, placement });

	}
	
	public void setSandbox (bool sandbox) {
		_plugin.Call ("setSandbox", new object[]{ sandbox });
	}
	
	public void setLogger (bool logger) {
		_plugin.Call ("setLogger", new object[]{ logger });
	}
	
	public void loadAd () {
		_plugin.Call ("loadAd");
	}
	
	public bool showAd () {
		return _plugin.Call<bool> ("showAd");
	}
	
	public void loadAndShowAd () {
		_plugin.Call ("loadAndShowAd");
	}
	
	/* Listener */
	
	public class AvocarrotInterstitialScript : MonoBehaviour {
		
		AvocarrotInterstitial avocarrot;
		
		public void setAvocarrot(AvocarrotInterstitial avocarrot) {
			this.avocarrot = avocarrot;
		}
		
		public void onAdLoaded ()
		{
			if (avocarrot.onAdLoadedEvent != null)
				avocarrot.onAdLoadedEvent ();
		}
		
		public void onAdError ()
		{
			if (avocarrot.onAdErrorEvent != null)
				avocarrot.onAdErrorEvent ();
		}
		
		public void onAdDisplayed ()
		{
			if (avocarrot.onAdDisplayedEvent != null)
				avocarrot.onAdDisplayedEvent ();
		}
		
		public void onAdClicked ()
		{
			if (avocarrot.onAdClickedEvent != null)
				avocarrot.onAdClickedEvent ();
		}
		
		public void onAdDismissed ()
		{
			if (avocarrot.onAdDismissedEvent != null)
				avocarrot.onAdDismissedEvent ();
		}
		
	}
	
}

#endif
