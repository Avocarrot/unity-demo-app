using UnityEngine;
using System.Collections;

public class RandomMove : MonoBehaviour {

	Vector3 startPoint, endPoint, startScale, endScale;
	float startTime, duration;
	BgScript[] bgScripts;
	Animator anim;
	
	void Start() {
		Invoke("UpdateMovement", 0f);
		bgScripts = new BgScript[] {
			GameObject.Find ("bgL").GetComponent<BgScript> (),
			GameObject.Find ("bgR").GetComponent<BgScript> ()
		};

		anim = GetComponent<Animator>();

	}
	
	void UpdateMovement() {
		anim.Play (Animator.StringToHash ("fly"), 0, 0f);
		startPoint = transform.position;
		endPoint = new Vector3 (Random.Range (-2f, 2f), Random.Range (-2f, 2f), startPoint.z);
		startScale = transform.localScale;
		float scale = Random.Range (1.0f, 3.25f);
		endScale = new Vector3 (scale, scale, startScale.z);
		startTime = Time.time;
		duration = Random.Range (1f, 3f);
		Invoke("UpdateMovement", duration);
		animateBg (0.05f);
	}

	void Update() {
		transform.position = Vector3.Lerp(startPoint, endPoint, (Time.time - startTime) / duration);
		transform.localScale = Vector3.Lerp(startScale, endScale, (Time.time - startTime) / duration);

		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Ended) {
				animateOnTouch(touch.position);
			}
		}

		if (Input.GetMouseButton(0))
			animateOnTouch (Input.mousePosition);
	}

	void animateOnTouch(Vector3 position) {
		anim.Play (Animator.StringToHash ("theHero"), 0, 0f);
		startPoint = transform.position;
		endPoint = Camera.main.ScreenToWorldPoint(position);
		endPoint.z = startPoint.z;
		startScale = transform.localScale;
		float scale = Random.Range (3.0f, 4.0f);
		endScale = new Vector3 (scale, scale, startScale.z);
		startTime = Time.time;
		duration = 1f;
		CancelInvoke("UpdateMovement");
		Invoke("UpdateMovement", duration);
		animateBg (0.25f);
	}

	void animateBg(float f) {
		foreach (BgScript s in bgScripts) {
			s.setSpeed (f);
		}
	}


}
