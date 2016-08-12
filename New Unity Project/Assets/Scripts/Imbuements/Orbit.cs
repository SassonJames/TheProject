using UnityEngine;
using System.Collections;

public class Orbit : BaseImbue {
	
	public float speed  = 100.0f; // Deg per sec
	public float frameStep = 0.1f;
	public float radius = 1.5f;
	public GameObject target;
	
	private Vector3 stay = new Vector3(0,0,0);
	private float angle = 0.0f;

    void Start() {
        target = gameObject.GetComponent<ScriptManager>().target;
    }
    
	void LateUpdate () {
		// Check if the object is being held in place
		if (!gameObject.GetComponent<ScriptManager> ().shouldUpdate || gameObject.GetComponent<ScriptManager> ().target == null) {
			gameObject.GetComponent<Rigidbody> ().velocity = stay;
			return;
		}

        float y_pos = target.transform.position.y;

		// MovetTowards the next position
		angle += speed*Time.deltaTime;
		Vector3 target_pos = target.transform.position;
		float x_pos = radius * Mathf.Cos(angle*Mathf.PI/180) + target_pos.x;
		float z_pos = radius * Mathf.Sin(angle*Mathf.PI/180) + target_pos.z;

		Vector3 move = new Vector3(x_pos, y_pos, z_pos);
		float t = frameStep/Vector3.Distance(transform.position, move);
		this.transform.position = Vector3.Lerp (this.transform.position, move, t);
	}
}
