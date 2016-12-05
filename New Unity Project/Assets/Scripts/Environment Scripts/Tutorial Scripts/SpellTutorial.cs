using UnityEngine;
using System.Collections;

public class SpellTutorial : MonoBehaviour {

	public GameObject unlockCube;
	public GameObject clearCube;
	public GameObject door;
	public GameObject exit;
	public GameObject restart;
	public GameObject rightCollider;
	public GameObject barrier;

	public int state = 0;

	private TextMesh mesh;
	private string prevtxt;
	private int prevstate;

	void Start() {
		mesh = GetComponent<TextMesh> ();
		clearCube.SetActive(false);
		door.SetActive (false);
		exit.SetActive (false);
		restart.SetActive (false);
		GameManager.keepStaticFields = true;
	}

	public void resume() {
		mesh.text = prevtxt;
		state = prevstate;
		GameManager.MainPlayer.GetComponent<Player> ().CastSpell ("Clear");
	}

	// Update is called once per frame
	void Update () {
		if (state != -1 && GameManager.MainPlayer.GetComponent<Player> ().mana == 0 && GameManager.MainPlayer.GetComponent<Player> ().activeSpell == null) {
			prevtxt = mesh.text;
			prevstate = state;
			state = -1;
			mesh.text = "You can only cast so many spells at once!";
			Invoke ("resume", 3);
		}


		if (state == 0 && unlockCube == null) {
			state = 1;
			GetComponent<TextMesh> ().text = "Hold left shift to view your spells.";
		} if (state == 1 && Input.GetKey (KeyCode.LeftShift)) {
			state = 2;
			mesh.text = "You are now in casting mode. Press " + GameManager.hotkeys [0] + " to cast " + Player.unlockedSpells [0];
		} if (state == 2 && !Input.GetKey (KeyCode.LeftShift)) {
			mesh.text = "Don't let go of shift too soon!";
			state = 1;
		} if (state == 2 && GameManager.MainPlayer.GetComponent<Player> ().activeSpell != null) {
			state = 3;
			barrier.SetActive (false);
			mesh.text = "Keep holding shift! You are now casting " + Player.unlockedSpells [0];
			mesh.text += "\nYou will carry this spell with you as long as you hold left shift";
			mesh.text += "\nPlace it down by the right wall by releasing shift near it.";
		} if (state == 3 && GameManager.MainPlayer.GetComponent<Player> ().activeSpell == null && !rightCollider.GetComponent<DetectDamage>().detected) {
			state = 2;
		} if (state == 3 && GameManager.MainPlayer.GetComponent<Player> ().activeSpell == null && rightCollider.GetComponent<DetectDamage>().detected) {
			state = 4;
			mesh.text = "Your spell will sit where it is until it is destroyed.";
			mesh.text += "\nSpells can be made to do much more, but for now they are still.";
			mesh.text += "\nCollect the yellow cube to learn the CLEAR spell.";
			clearCube.SetActive(true);
		} if (state == 4 && clearCube == null) {
			state = 5;
			mesh.text = "You have learned the CLEAR spell.";
			mesh.text += "\nHold left shift to cast it with " + GameManager.hotkeys [1] + ".";
			mesh.text += "\nThis spell will destroy all your active spells";
		} if (state == 5 && Input.GetKey (KeyCode.LeftShift) && Input.GetKey (GameManager.hotkeys [1])) {
			state = 6;
			door.SetActive(true);
			mesh.text = "Now, Destroy the red cube with a damage spell.";
			mesh.text += "\nHold shift, and touch the damage spell to the cube.";
		} if (state == 6 && door == null) {
			state = 7;
			exit.SetActive(true);
			restart.SetActive (true);
			mesh.text = "Touch the sphere on the right to enter the next room.";
			mesh.text += "\nTouch the sphere on the left to start this tutorial over.";
		}
	}
}
