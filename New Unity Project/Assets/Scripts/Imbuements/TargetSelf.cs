using UnityEngine;
using System.Collections;

public class TargetSelf : BaseImbue {

	void Start() {
		manager.target = manager.owner;
	}
}
