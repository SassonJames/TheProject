using UnityEngine;
using System.Collections;

public class Repeat_DamageTargetPlayer : MonoBehaviour {

	Enemy enemyscript;

	// Use this for initialization
	void Start () {
		enemyscript = GetComponent<Enemy> ();
		Invoke ("cast", 3f);
	}
	
	void cast() {
		enemyscript.CastSpell ("Damage");
		enemyscript.Imbue ("TargetNearest:Player");
		enemyscript.Imbue ("Magnetism");
		enemyscript.ReleaseSpell ();
		Invoke ("cast", 10f);
	}
}
