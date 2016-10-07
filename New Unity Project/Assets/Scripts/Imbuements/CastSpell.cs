using UnityEngine;
using System.Collections;

public class CastSpell : BaseImbue {

	public Player owner;
	public string type;

	void Start() {
		if (manager.isCustom)
			return;

		type = args[1];
		owner = manager.owner.GetComponent<Player>();
	}

	void Update () {

		if (!manager.shouldUpdate)
			return;

		// Have the owner create the spell
		// This makes it cost mana each spell cast
		GameObject spell = owner.CastSpell(type);
		Debug.Log (type);

		if (spell != null) {

			spell.transform.position = transform.position;

			// imbue the new spell
			for (int i=2; i<args.Length; i++) {
				owner.Imbue (args [i]);
			}

			owner.ReleaseSpell ();
		}
		Destroy (this);
	}
}
