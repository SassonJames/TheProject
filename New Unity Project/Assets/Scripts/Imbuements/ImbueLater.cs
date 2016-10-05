using UnityEngine;
using System.Collections;

public class ImbueLater : BaseImbue {

	public int delay;
	public string imbue;
	private bool shouldCast = true;

	/*
	 * Get the spell arguments from the ScriptManager.
	 */
	void Start () {
		if (!manager.isCustom) {
			delay = int.Parse (args [1]);
			imbue = args [2].Replace (" ", "");
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
		string[] iargs = Player.ParseImbuement (imbue);
		BaseImbue scriptComponent = manager.attachScript (imbue) as BaseImbue;

		// Add the imbuement to the stack
		if (scriptComponent != null) {
			scriptComponent.args = iargs;
			scriptComponent.manager = manager;
		}
	}

}
