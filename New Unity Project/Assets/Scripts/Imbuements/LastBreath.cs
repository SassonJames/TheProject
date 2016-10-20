using UnityEngine;
using System.Collections;

public class LastBreath : BaseImbue {

	public Player owner;

	// Use this for initialization
	void Start () {
		owner = manager.owner.GetComponent<Player>();
	}
	
	void OnDestroy () {
		Debug.Log(owner);
		// Have the owner create the spell
		// This makes it cost mana each spell cast
		GameObject spell = owner.CastSpell(args[1]);
		
		// return if spellcast failed
		if (spell == null)
			return;
		
		spell.transform.position = transform.position;
		
		// imbue the new spell
		for (int i=2; i<args.Length; i++) {
			owner.Imbue (args[i]);
		}
		
		owner.ReleaseSpell();
	}
}
