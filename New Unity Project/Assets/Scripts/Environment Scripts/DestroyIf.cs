using UnityEngine;
using System.Collections;

// Destroy all objects in the array if target is inactive
public class DestroyIf : MonoBehaviour {

	public GameObject target;
	public GameObject[] destroy;
	
	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "DamageSpell" && !target.activeSelf) {
			foreach (GameObject o in destroy) {
				Destroy (o);
			}
			Destroy (this.gameObject);
		}
	}


}
