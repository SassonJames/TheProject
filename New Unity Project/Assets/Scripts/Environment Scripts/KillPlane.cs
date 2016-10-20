using UnityEngine;
using System.Collections;

public class KillPlane : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject _respawn;
	private Vector3 respawn;

	// Set up respawn point
	void Start() {
		respawn = _respawn.transform.position;
	}

	void OnCollisionEnter(Collision coll) {
		Debug.Log ("killme");
		if (coll.gameObject.tag == "Player") {
			Destroy (coll.gameObject);
			GameObject newPlayer = Instantiate (playerPrefab) as GameObject;
			newPlayer.transform.position = respawn;
		}
	}

}
