using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	public GameObject cam;
	public GameObject leftBoundObject;
	public GameObject rightBoundObject;

	public float tracking;

	private float leftBound;
	private float rightBound;
	private float boundDist;

	void Start() {
		leftBound = leftBoundObject.transform.position.x;
		rightBound = rightBoundObject.transform.position.x;
		boundDist = cam.GetComponent<Camera> ().orthographicSize * cam.GetComponent<Camera> ().aspect - 2;
	}

	// Update is called once per frame
	void Update () {
		float x = this.transform.position.x;

		if (x - leftBound < boundDist) {
			return;
		} if (rightBound - x < boundDist)
			return;

		Vector3 newPos = new Vector3 (this.transform.position.x, cam.transform.position.y, cam.transform.position.z);
		cam.transform.position = Vector3.Lerp (cam.transform.position, newPos, tracking);
	}
}
