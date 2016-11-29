using UnityEngine;
using System.Collections;

public class DestroyFromDamage : MonoBehaviour {
	
	void OnTriggerEnter(Collider coll) {
		//Debug.Log (coll.gameObject.name);
		if (coll.gameObject.tag == "DamageSpell") {
			//GameManager.audio.PlayOneShot(GameManager.negate);
			Destroy (coll.gameObject);
			Destroy (this.gameObject);
		}
	}
}
