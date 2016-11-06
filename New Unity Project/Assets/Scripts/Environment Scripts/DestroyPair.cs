using UnityEngine;
using System.Collections;

public class DestroyPair : MonoBehaviour {

	public GameObject destroy;

	void OnTriggerEnter(Collider coll) {
		//Debug.Log (coll.gameObject.name);
		if (coll.gameObject.tag == "DamageSpell" && coll.GetComponent<ScriptManager>().owner != this.gameObject) {
			//GameManager.audio.PlayOneShot(GameManager.negate);
			Destroy (destroy);
			Destroy (coll.gameObject);
			Destroy (this.gameObject);
		}
	}
}
