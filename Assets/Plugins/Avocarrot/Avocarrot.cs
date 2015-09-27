using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

#if UNITY_ANDROID

public class Avocarrot {
	
	AndroidJavaObject _plugin;

	GameObject avocarrotGameObj;
	AvocarrotScript avocarrotScript;

	GameObject title, description, ctaBtn, image, icon;

	public Action onAdLoadedEvent;
	public Action onAdErrorEvent;
	public Action onAdImpressionEvent;
	public Action onAdClickedEvent;

	int itemsInUI = 0;
	List<string> renderingItems = new List<string> ();

	public Avocarrot (string apiKey, string placement) {

		avocarrotGameObj = new GameObject ("Avocarrot_"+DateTime.Now.Ticks.ToString());
		avocarrotScript = avocarrotGameObj.AddComponent( typeof ( AvocarrotScript ) ) as AvocarrotScript;
		avocarrotScript.setAvocarrot (this);

		AndroidJavaClass pluginClass = new AndroidJavaClass("com.avocarrot.androidsdk.UnityCustomController");
		_plugin = pluginClass.CallStatic<AndroidJavaObject> ("createInstance", new object[] { avocarrotGameObj.name, apiKey, placement });

	}
	
	public void setSandbox (bool logger) {
		_plugin.Call ("setSandbox", new object[]{ logger });
	}

	public void setLogger (bool logger) {
		_plugin.Call ("setLogger", new object[]{ logger });
	}

	public void setTitle(GameObject title) {
		this.title = title;
		initComponent (title);
	}

	public void setDescription(GameObject description) {
		this.description = description;
		initComponent (description);
	}

	public void setCtaBtn(GameObject ctaBtn) {
		this.ctaBtn = ctaBtn;
		initComponent (ctaBtn);
	}

	public void setImage(GameObject image) {
		this.image = image;
		initComponent (image);
	}
	
	public void setIcon(GameObject icon) {
		this.icon = icon;
		initComponent (icon);
	}

	private void initComponent(GameObject gameObj){
		gameObj.SetActive (false);
		AvocarrotItem avocarrotScript = gameObj.AddComponent( typeof ( AvocarrotItem ) ) as AvocarrotItem;
		avocarrotScript.setAvocarrot (this);
	}

	public void loadAd () {

		_plugin.Call ("loadAd");

		itemsInUI = 0;

		if (icon) itemsInUI++;
		if (image) itemsInUI++;
		if (title) itemsInUI++;
		if (ctaBtn) itemsInUI++;
		if (description) itemsInUI++;

	}

	public void showAd () {

		if (icon) icon.SetActive (true);
		if (image) image.SetActive (true);
		if (title) title.SetActive (true);
		if (description)  description.SetActive (true);
		if (ctaBtn) ctaBtn.SetActive (true);

	}
	
	public void itemStatusChanged(string name, bool enabled) {

		if (enabled) {
			if (!renderingItems.Contains (name)) {
				renderingItems.Add (name);
			}
		} else {
			renderingItems.Remove (name);
		}

		if (itemsInUI == renderingItems.Count) {
			avocarrotScript.StopCoroutine ("checkVisibility");
			avocarrotScript.StartCoroutine (checkVisibility());
		}
	}

	IEnumerator checkVisibility() {

		yield return new WaitForSeconds(1f);

		if (itemsInUI == renderingItems.Count) {
			bool impressionRegistered = _plugin.Call<bool> ("onUnityRegisterImpression");
			if (impressionRegistered) {
				Button button = ctaBtn.GetComponent<Button>();
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(onCtaButtonClicked);

				if (icon) Component.Destroy(icon.GetComponent<AvocarrotItem>());
				if (image) Component.Destroy(image.GetComponent<AvocarrotItem>());
				if (title) Component.Destroy(title.GetComponent<AvocarrotItem>());
				if (description) Component.Destroy(description.GetComponent<AvocarrotItem>());
				if (ctaBtn) {
					Component.Destroy(ctaBtn.GetComponent<AvocarrotItem>());
					Component.Destroy(ctaBtn.GetComponent<OnActivateCTA>());
				}
			}
		}
		
	}

	void bindModelToUi() {
		if (title) {
			Text text = title.GetComponent<Text>();
			string str = _plugin.Call<string> ("getTitle");
			if ((text!=null) && (str!=null)) text.text = str;
		}
		if (description) {
			Text text = description.GetComponent<Text>();
			string str = _plugin.Call<string> ("getDescription");
			if ((text!=null) && (str!=null)) text.text = str;
		}
		if (image) {
			loadImageFromUrl(_plugin.Call<string> ("getImageUrl"), image);
		}
		if (icon) {
			loadImageFromUrl(_plugin.Call<string> ("getIconUrl"), icon);
		}
		if (ctaBtn) {
			string str = _plugin.Call<string> ("getCtaBtn");
			OnActivateCTA activateScript = ctaBtn.AddComponent( typeof ( OnActivateCTA ) ) as OnActivateCTA;
			activateScript.setText(str);
		}
		
	}

	private void loadImageFromUrl (string url, GameObject image) {
		ImageLoader imageLoader;
		imageLoader = avocarrotGameObj.AddComponent( typeof ( ImageLoader ) ) as ImageLoader;
		imageLoader.setUrl (image, url);
	}

	private void onCtaButtonClicked() {
		_plugin.Call ("onUnityClicked");
	}

	public class ImageLoader : MonoBehaviour {

		string url;
		Image image;

		public void setUrl(GameObject gameObj, string url) {
			this.url = url;
			this.image = gameObj.GetComponent<UnityEngine.UI.Image>();
		}

		IEnumerator Start() {
			WWW www = new WWW(url);
			yield return www;
			if (www.texture != null) {
				image.preserveAspect = true;
				Sprite sprite = new Sprite();
				sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);  
				image.sprite = sprite;
			}
			yield return null;
		}

	}

	/* Listener */

	public class AvocarrotScript : MonoBehaviour {

		Avocarrot avocarrot;

		public void setAvocarrot(Avocarrot avocarrot) {
			this.avocarrot = avocarrot;
		}

		public void onAdLoaded ()
		{
			avocarrot.bindModelToUi ();
			if (avocarrot.onAdLoadedEvent != null)
				avocarrot.onAdLoadedEvent ();
		}
		
		public void onAdError ()
		{
			if (avocarrot.onAdErrorEvent != null)
				avocarrot.onAdErrorEvent ();
		}
		
		public void onAdImpression ()
		{
			if (avocarrot.onAdImpressionEvent != null)
				avocarrot.onAdImpressionEvent ();
		}
		
		public void onAdClicked ()
		{
			if (avocarrot.onAdClickedEvent != null)
				avocarrot.onAdClickedEvent ();
		}

	}

	public class AvocarrotItem : MonoBehaviour {

		Avocarrot avocarrot;

		public void setAvocarrot(Avocarrot avocarrot) {
			this.avocarrot = avocarrot;
		}

		public AvocarrotItem() {
			gameObject.AddComponent("MeshRenderer");
		}

		void OnWillRenderObject () {
			if (avocarrot!=null) avocarrot.itemStatusChanged (gameObject.name, IsVisibleFrom(gameObject.renderer, Camera.main));
		}

		void OnBecameInvisible() {
			if (avocarrot!=null) avocarrot.itemStatusChanged (gameObject.name, false);
		}

		void OnBecameVisible() {
			if (avocarrot!=null) avocarrot.itemStatusChanged (gameObject.name, true);
		}

		public bool IsVisibleFrom(Renderer renderer, Camera camera)
		{
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
			return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
		}

	}

	public class OnActivateCTA : MonoBehaviour {

		string ctaText;

		public OnActivateCTA(string ctaText) {
		}

		public void setText(string ctaText) {
			this.ctaText = ctaText;
		}
		
		void OnEnable() {
			Text text = gameObject.GetComponentInChildren<Text> ();
			if ((text != null) && (ctaText != null))
				text.text = ctaText;
		}
		
	}

}

#endif
