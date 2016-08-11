using UnityEngine;
using System.Collections;

public class Magnetism : MonoBehaviour {
	
	public float speed = 5.0f;
	public GameObject target;

	private Vector3 stay = new Vector3(0,0,0);

	// Update is called once per frame
	void LateUpdate () {
		// Check if the object is being held in place
		if (!gameObject.GetComponent<ScriptManager> ().shouldUpdate || gameObject.GetComponent<ScriptManager> ().target == null) {
			gameObject.GetComponent<Rigidbody2D> ().velocity = stay;
			return;
		}
		
		// Move the x position of this Object forward
		GameObject target = gameObject.GetComponent<ScriptManager> ().target;
		Vector3 move = Vector3.Normalize (target.transform.position - transform.position) * speed;
		gameObject.GetComponent<Rigidbody2D> ().velocity = move;
	}
}
