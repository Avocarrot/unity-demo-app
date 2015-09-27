using UnityEngine;
using System.Collections;

public class BgScript : MonoBehaviour {

	const float BG_ASPECT_RATIO = 1920f / 1200f;

	Vector3 initialPosition;
	float width = 0;
	float movement = 0.05f;

	void Start() {
		gameObject.transform.localScale = new Vector3(BG_ASPECT_RATIO, 1f, 1f);
		width = gameObject.renderer.bounds.extents.x * 2f;
		if (gameObject.transform.position.x > 0) {
			gameObject.transform.position = new Vector3(width, 0f, 0f);
		} else {
			gameObject.transform.position = new Vector3 (0f, 0f, 0f);
		}
	}

	void FixedUpdate () {
		gameObject.transform.position = new Vector3 (transform.position.x - movement, transform.position.y, transform.position.z);
		if (gameObject.transform.position.x < -1f * width) {
			gameObject.transform.position = new Vector3 (width - movement, transform.position.y, transform.position.z);
		}
	}

	public void setSpeed(float movement) {
		this.movement = movement;
	}

}
