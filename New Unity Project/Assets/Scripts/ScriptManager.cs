using UnityEngine;
using System;

public class ScriptManager : MonoBehaviour {

	public bool shouldUpdate = false;
	public bool isCustom = false;
	public GameObject owner = null;
	public GameObject target = null;
	public float size = 1;
	public int mana;
	public string[] args;

	/*
	 * Attach a script with the given name to the parent GameObject
	 */
	public Component attachScript(string script) {
		/*
		if (this.GetComponent (script) == null)
			return UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (gameObject, "Assets/Scripts/ScriptManager.cs (12,3)", script);
		else {
			Destroy (this.GetComponent (script));
			return UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (gameObject, "Assets/Scripts/ScriptManager.cs (12,3)", script);
		*/
		return gameObject.AddComponent(Type.GetType (script));
		//return UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (gameObject, "Assets/Scripts/ScriptManager.cs (12,3)", script);
	}

	/*
	 * Return mana to the owner. Update the Active Spell and indicator as needed
	 */
	public void OnDestroy() { 
		if (owner == null)
			return;
		owner.GetComponent<Player>().mana += this.mana;
        owner.GetComponent<Player>().UpdateManabar ();
        owner.GetComponent<Player>().UpdateActiveSpell (gameObject);
		owner.GetComponent<Player>().UpdateSelector ();
	}

	// Destroy all movement scripts. This is done every time a new moevement script is added
	// without specifying that it should be added to the existing movement scripts
	public void RemoveMovementScripts() {
		Component[] scripts = null;
		Debug.Log ("DESTROY");

		// I've decided to make this script do the annoying "50thousand if statement" method
		// Simply because it would be doing that slow search a lot otherwise
		scripts = GetComponents<MoveForward> ();
		foreach (Component c in scripts) {
			Destroy (c);
		}
		scripts = GetComponents<Move> ();
		foreach (Component c in scripts) {
			Destroy (c);
		}
		scripts = GetComponents<Orbit> ();
		foreach (Component c in scripts) {
			Destroy (c);
		}
		scripts = GetComponents<Magnetism> ();
		foreach (Component c in scripts) {
			Destroy (c);
		}
	}

	// Destroy all movement scripts except the given component
	public void RemoveMovementScripts(Component exception) {
		Component[] scripts = null;
		Debug.Log ("DESTROY");

		// I've decided to make this script do the annoying "50thousand if statement" method
		// Simply because it would be doing that slow search a lot otherwise
		scripts = GetComponents<MoveForward> ();
		foreach (Component c in scripts) {
			if (c != exception)
				Destroy (c);
		}
		scripts = GetComponents<Move> ();
		foreach (Component c in scripts) {
			if (c != exception)
				Destroy (c);
		}
		scripts = GetComponents<Orbit> ();
		foreach (Component c in scripts) {
			if (c != exception)
				Destroy (c);
		}
		scripts = GetComponents<Magnetism> ();
		foreach (Component c in scripts) {
			if (c != exception)
				Destroy (c);
		}
	}
}