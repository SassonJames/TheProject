using UnityEngine;
using System.Collections;

public class Magnetism : BaseImbue {
	
	public float speed = 5.0f;

	private Vector3 stay = new Vector3(0,0,0);

	void Start() {
		if (args [args.Length - 1] != "add") {
			manager.RemoveMovementScripts (this);
		}
	}

	// Update is called once per frame
	void LateUpdate () {
		// Check if the object is being held in place
		if (!manager.shouldUpdate || manager.target == null) {
			gameObject.GetComponent<Rigidbody> ().velocity = stay;
			return;
		}

		Vector3 move = Vector3.Normalize (manager.target.transform.position - transform.position) * speed;
		Debug.Log (move);
		gameObject.GetComponent<Rigidbody> ().velocity = move;
	}
}
