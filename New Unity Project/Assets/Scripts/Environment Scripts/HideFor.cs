using UnityEngine;
using System.Collections;

public class HideFor : MonoBehaviour {

	public float time;

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "DamageSpell") {
			gameObject.SetActive(false);
			Invoke ("Show", time);
		}
	}

	void Show() {
		gameObject.SetActive (true);
	}

}
