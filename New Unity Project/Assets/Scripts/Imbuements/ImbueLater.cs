using UnityEngine;
using System.Collections;

public class ImbueLater : MonoBehaviour {

	int delay;
	string imbue;
	ScriptManager manager;
	private bool shouldCast = true;

	/*
	 * Get the spell arguments from the ScriptManager.
	 */
	void Start () {
		manager = GetComponent<ScriptManager> ();
		if (!manager.isCustom) {
			imbue = manager.args [1];
			delay = int.Parse (manager.args [2]);
		}
	}

	/*
	 * Set the timer after release
	 */
	void Update() {
		if (shouldCast && manager.shouldUpdate) {
			Invoke ("MakeImbue", delay);
			shouldCast = false;
		}
	}

	/*
	 * Attach the imbument script
	 */
	void MakeImbue() {
		manager.attachScript (imbue);
	}

}
