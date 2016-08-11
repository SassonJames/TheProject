using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour {
	
	public float speed = 5.0f;
	
	// Update is called once per frame
	void FixedUpdate () {
		// Check if the object is being held in place
		if (!gameObject.GetComponent<ScriptManager> ().shouldUpdate)
			return;

		// Move the x position of this Object forward
		Vector3 move = new Vector3(speed, 0, 0);
		transform.position += move * speed * Time.deltaTime;
	}
}
