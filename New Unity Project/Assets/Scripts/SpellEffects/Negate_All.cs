using UnityEngine;
using System.Collections;

public class Negate_All : MonoBehaviour {

	void OnTriggerEnter(Collider coll) {
		Destroy (coll.gameObject);
	}
}
