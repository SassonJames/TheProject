using UnityEngine;
using System.Collections;

public class ImbueTutorial : MonoBehaviour {

	public GameObject TxtObj1;
	public GameObject TxtObj2;
	public GameObject TxtObj3;
	public GameObject unlock1;
	public GameObject button;

	int state=0;
	TextMesh mesh1;
	TextMesh mesh2;

	void Start() {
		TxtObj3.SetActive (false);
		mesh1 = TxtObj1.GetComponent<TextMesh> ();
		mesh2 = TxtObj2.GetComponent<TextMesh> ();
	}

	// Update is called once per frame
	void Update () {
		switch (state) {
		case 0:
			if (unlock1 == null) {
				state = 1;
			}
			break;
		case 1:
			mesh1.text = "This is the \nMove command";
			mesh2.text = "Cast a Damage \nspell to view \nyour commands.";
			if (GameManager.MainPlayer.GetComponent<Player> ().activeSpell != null) {
				state = 2;
			}
			break;
		case 2:
			mesh1.text = "Adding commands \nto your spell \nis called Imbuing";
			mesh2.text = "Press " + GameManager.hotkeys [0] + " to \nImbue your spell \nwith Move.";
			if (GameManager.MainPlayer.GetComponent<Player> ().activeSpell == null) {
				state = 3;
			}
			break;
		case 3:
			mesh1.text = "Cast Damage \nto begin imbuing.";
			mesh2.text = "Press " + GameManager.hotkeys [0] + " to imbue\n it with Move.";
			if (button == null) {
				state = 4;
			}
			break;
		case 4:
			mesh1.text = "This Imbuement \ncan only move back \nfor now.";
			mesh2.text = "A great caster \ncan write commands \nthat do anything.";
			TxtObj3.SetActive (true);
			state = 5;
			break;
		default:
			break;
		}
	}
}
