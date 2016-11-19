using UnityEngine;
using System.Collections;

public class BasicRotation : MonoBehaviour {
	
	// Update is called once per frame
	void LateUpdate () {
        transform.Rotate(Vector3.up * Time.deltaTime * 60, Space.World);
	}
}
