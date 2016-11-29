using UnityEngine;
using System.Collections;

public class DetectDamage : MonoBehaviour {

	public bool detected = false;

	void OnTriggerEnter(Collider coll) {
		//Debug.Log (coll.gameObject.name);
		if (coll.gameObject.tag == "DamageSpell") {
			detected = true;
		}
	}

	void OnTriggerExit(Collider coll) {
		//Debug.Log (coll.gameObject.name);
		if (coll.gameObject.tag == "DamageSpell") {
			detected = false;
		}
	}
}
