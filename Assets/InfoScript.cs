using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoScript : MonoBehaviour {

	void Start () {

		GameObject.Find ("LearnMore").GetComponent<Button>().onClick.AddListener(learnMore) ;
		GameObject.Find ("Close").GetComponent<Button>().onClick.AddListener(closeMe) ;

	}

	void OnGUI() {
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey (KeyCode.Escape)) {
				closeMe();
				return;
			}
		}
	}

	void closeMe() {
		Application.LoadLevel("Avocarrot");
	}

	void learnMore () {
		Application.OpenURL("http://www.avocarrot.com/");
	}
}
