using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DemoScript : MonoBehaviour {

	GameObject showCustomAdBtn, customAdContainer;
	Avocarrot avocarrot;

	GameObject adImage, adCTA, adTitle, adDescription;

	void Start () {

		(GameObject.Find ("LoadInterstitial").GetComponent<Button>()).onClick.AddListener(load_interstitial) ;
		(GameObject.Find ("Info").GetComponent<Button> ()).onClick.AddListener (load_info);

		/* inactivate ShowCustom btn until the Custom Ad is loaded */
		showCustomAdBtn = GameObject.Find ("ShowCustom");
		(showCustomAdBtn.GetComponent<Button> ()).onClick.AddListener (show_custom_Ad);
		showCustomAdBtn.SetActive (false);

		GameObject closeCustomAdBtn = GameObject.Find ("CloseCustomAd");
		(closeCustomAdBtn.GetComponent<Button> ()).onClick.AddListener (close_custom_Ad);

		/* Get references of GameObject that will display Native Ad */
		adImage = GameObject.Find ("AdImage");
		adCTA = GameObject.Find ("AdCTAButton");
		adTitle = GameObject.Find ("AdTitle");
		adDescription = GameObject.Find ("AdDescription");

		avocarrot = new Avocarrot("8d4f9962ea5997eb921a5f1130d7fc84f593a756", "d8775ad6710f9a0cf08a7739e8f0cc3ef78bde6f");
		avocarrot.setImage (adImage );
		avocarrot.setCtaBtn (adCTA);
		avocarrot.setTitle (adTitle);
		avocarrot.setDescription (adDescription);
		avocarrot.setSandbox (true);
		avocarrot.setLogger (true);
		avocarrot.onAdLoadedEvent += onCustomAdLoaded;
		avocarrot.onAdClickedEvent += onCustomAdClicked;
		avocarrot.onAdImpressionEvent += onCustomAdImpression;
		avocarrot.onAdErrorEvent += onCustomAdError;
		avocarrot.loadAd ();

		customAdContainer = GameObject.Find ("CustomAds");
		customAdContainer.SetActive (false);

	}

	/* Interstitial Ad */

	void load_interstitial() {

		AvocarrotInterstitial avocarrotInterstitial = new AvocarrotInterstitial ("8d4f9962ea5997eb921a5f1130d7fc84f593a756", "1d2fb9f85a1a507850a3ba379b54d23e0089d628");
		avocarrotInterstitial.setSandbox (true);
		avocarrotInterstitial.setLogger (true);

		avocarrotInterstitial.onAdLoadedEvent += onInterstitialAdLoaded;
		avocarrotInterstitial.onAdDisplayedEvent += onInterstitialAdDisplayed;
		avocarrotInterstitial.onAdClickedEvent += onInterstitialAdClicked;
		avocarrotInterstitial.onAdDismissedEvent += onInterstitialAdDismissed;
		avocarrotInterstitial.onAdErrorEvent += onInterstitialAdError;
		
		avocarrotInterstitial.loadAndShowAd ();

	}

	void load_info() {
		Application.LoadLevel ("InfoScene");
	}

	void onInterstitialAdLoaded() {
		Debug.Log ("Interstitial Loaded");
	}
	void onInterstitialAdDisplayed() {
		Debug.Log ("Interstitial Displayed");
	}
	void onInterstitialAdClicked() {
		Debug.Log ("Interstitial Clicked");
	}
	void onInterstitialAdDismissed() {
		Debug.Log ("Interstitial Dismissed");
	}
	void onInterstitialAdError() {
		Debug.Log ("Interstitial Error");
	}

	/* Custom Ad */

	void onCustomAdLoaded() {
		Debug.Log ("CustomAd Loaded");
		showCustomAdBtn.SetActive (true);
	}
	void onCustomAdClicked() {
		Debug.Log ("CustomAd Clicked");
	}
	void onCustomAdError() {
		Debug.Log ("CustomAd Error");
	}
	void onCustomAdImpression() {
		Debug.Log ("CustomAd Impression");
	}

	void show_custom_Ad() {
		avocarrot.showAd ();
		showCustomAdBtn.SetActive (false);
		customAdContainer.SetActive (true);
	}
	void close_custom_Ad() {
		showCustomAdBtn.SetActive (true);
		customAdContainer.SetActive (false);
	}
}
