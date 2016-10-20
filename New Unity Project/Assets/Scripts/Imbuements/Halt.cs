using UnityEngine;
using System.Collections;

public class Halt : BaseImbue {
	
	// Remove all movement scripts from this object
	void Start () {
		manager.RemoveMovementScripts ();
		Destroy (this);
	}
}
