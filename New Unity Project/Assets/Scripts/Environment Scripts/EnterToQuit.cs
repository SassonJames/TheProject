using UnityEngine;
using System.Collections;

public class EnterToQuit : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			Debug.Log ("exit");
			Application.Quit ();
		}
	}
}
