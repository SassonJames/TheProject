using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class Enemy : Player {

	new public void Awake() {
	}

	new public void Start() {
		imbues = new Stack ();
		spells = new Stack ();
	}

	new public void Update() {

        //updateHPBar ();
		// Make the active spell follow the player.
		if (activeSpell != null)
			activeSpell.gameObject.transform.position = Vector3.Lerp(activeSpell.gameObject.transform.position, transform.position + spellPosition, 0.1f);
    }
}
  