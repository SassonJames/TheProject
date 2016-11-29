using UnityEngine;
using System.Collections;

public class UnlockSpell : MonoBehaviour {

	public string spellName;

	// Add the new imbue to the unlocked imbues list
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {

			string[] spelllist = Player.unlockedSpells;
			int newLen = spelllist.Length + 1;
			string[] newList = new string[newLen];
			for (int i=0; i<spelllist.Length; i++) {
				newList[i] = spelllist[i];
			}
			newList[newLen-1] = spellName;
			Player.unlockedSpells = newList;
			// Update the list in the display
			GameManager.reloadSpells();

			Destroy(gameObject);
		}
	}
}
