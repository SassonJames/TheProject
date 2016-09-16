using UnityEngine;
using System.Collections;

public class TargetSelf : BaseImbue {

	void Start() {
		manager = gameObject.GetComponent<ScriptManager> ();
		manager.target = manager.owner;
	}
}
