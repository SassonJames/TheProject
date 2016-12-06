using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject _persistentPrefab;

	public GameObject _spellsUIPanel;
	public GameObject _imbuesUIPanel;
	public GameObject[] _spellsUIItems;
	public GameObject[] _imbuesUIItems;
	public     string[] _hotkeys;


	public static GameObject persistentPrefab;
	public static GameObject spellsUIPanel;
	public static GameObject imbuesUIPanel;
	public static CastUIItem[] spellsUIItems;
	public static CastUIItem[] imbuesUIItems;
	public static     string[] hotkeys;
	public static         bool keepStaticFields = false;

	private static int spellsUIPage = 0;
	private static int imbuesUIPage = 0;

	// Use this to find the physical player object. Otherwise use the static fields in Player.cs if you need data.
	public static GameObject MainPlayer;


	public void Start() {
		Player.hbarwidth = MainPlayer.GetComponent<PlayerStats> ().hpField.GetComponent<RectTransform> ().rect.width;
		Player.mbarwidth = MainPlayer.GetComponent<PlayerStats> ().manaField.GetComponent<RectTransform> ().rect.width;

		persistentPrefab = _persistentPrefab;
		RetrievePersistentData ();

		spellsUIPanel = _spellsUIPanel;
		imbuesUIPanel = _imbuesUIPanel;
		spellsUIItems = new CastUIItem[_spellsUIItems.Length];
		imbuesUIItems = new CastUIItem[_imbuesUIItems.Length];
		hotkeys       = _hotkeys;

		for (int i = 0; i < _spellsUIItems.Length; i++) {
			spellsUIItems [i] = _spellsUIItems [i].GetComponent<CastUIItem> ();
		}
		for (int i = 0; i < _imbuesUIItems.Length; i++) {
			imbuesUIItems [i] = _imbuesUIItems [i].GetComponent<CastUIItem> ();
		}


		HideSpellUI ();
		initCastUI ();

		Camera.main.transform.position = new Vector3 (-4.28f, 12.21f, -14.85f);
		Camera.main.transform.rotation = Quaternion.Euler (35.4808f, 0, 0);

	}

	// Before a scene transition, store necessary data in an object that will not be destroyed.
	public static void StorePersistentData() {
		GameObject p = Instantiate (persistentPrefab) as GameObject;
		PersistentData pd = p.GetComponent<PersistentData> ();

		pd.unlockedImbues = Player.unlockedImbues;
		pd.unlockedSpells = Player.unlockedSpells;
	}

	public static void RetrievePersistentData() {
		GameObject po = GameObject.Find("Persistent Object(Clone)");

		if (!keepStaticFields && po != null) {
			PersistentData pd = po.GetComponent<PersistentData> ();
			Player.unlockedSpells = pd.unlockedSpells;
			Player.unlockedImbues = pd.unlockedImbues;
		}
		keepStaticFields = false;
		Destroy (po);
	}

	/*************************************
     * UNFINISHED UI CODE HERE
     */

	/*
	* Initialize the UI that appears as the player is casting a spell.
	* Populate it with hotkeys, spell types, and imbuements.
	*/
	public static void initCastUI() {
		// Set hotkeys, init option arrays
		for (int i = 0; i < hotkeys.Length; i++) {
			spellsUIItems [i].setHotkey (hotkeys [i]);
			spellsUIItems [i].options = new string[(Player.unlockedSpells.Length/3)+1];

			imbuesUIItems [i].setHotkey (hotkeys [i]);
			imbuesUIItems [i].options = new string[(Player.unlockedImbues.Length/3)+1];
		}

		// Populate option arrays. Option arrays are possible spell types/imbues
		for (int i = 0; i < Player.unlockedSpells.Length; i++) {
			spellsUIItems [i % 3].options [i / 3] = Player.unlockedSpells [i];
		}

		for (int i = 0; i < Player.unlockedImbues.Length; i++) {
			imbuesUIItems [i % 3].options [i / 3] = Player.unlockedImbues [i];
		}

		// Set initial selection
		for (int i = 0; i < hotkeys.Length; i++) {
			spellsUIItems [i].page = spellsUIPage;
			spellsUIItems [i].refresh ();
			imbuesUIItems [i].page = imbuesUIPage;
			imbuesUIItems [i].refresh ();
		}
	}

	// Re-initialize the UI if any changes to the spell/imbue arrays occur.
	public static void reloadSpells() {
		// Init option arrays
		for (int i = 0; i < hotkeys.Length; i++) {
			spellsUIItems [i].options = new string[(Player.unlockedSpells.Length/3)+1];
		}
		for (int i = 0; i < Player.unlockedSpells.Length; i++) {
			spellsUIItems [i % 3].options [i / 3] = Player.unlockedSpells [i];
		}
		// Refresh strings
		for (int i = 0; i < spellsUIItems.Length; i++) {
			spellsUIItems [i].refresh ();
		}
	}
	public static void reloadImbues() {
		// Init option arrays
		for (int i = 0; i < hotkeys.Length; i++) {
			imbuesUIItems [i].options = new string[(Player.unlockedImbues.Length/3)+1];
		}
		for (int i = 0; i < Player.unlockedImbues.Length; i++) {
			imbuesUIItems [i % 3].options [i / 3] = Player.unlockedImbues [i];
		}

		// Refresh strings
		for (int i = 0; i < imbuesUIItems.Length; i++) {
			imbuesUIItems [i].refresh ();
		}
	}

	// The following 3 functions will show/hide relevant information regarding the 
	// spell casting process
	public static void ShowSpells() {
		spellsUIPanel.SetActive (true);
		imbuesUIPanel.SetActive (false);
	}
	public static void ShowImbues() {
		spellsUIPanel.SetActive (false);
		imbuesUIPanel.SetActive (true);
	}
	public static void HideSpellUI() {
		spellsUIPanel.SetActive (false);
		imbuesUIPanel.SetActive (false);
	}

	// Functions to scroll the UI up and down
	public static void ScrollSpellsUp() {
		if (spellsUIPage >= (Player.unlockedSpells.Length-1) / 3)
			return;

		for (int i = 0; i < spellsUIItems.Length; i++) {
			spellsUIItems [i].scrollUp ();
		}
		spellsUIPage++;
	}
	public static void ScrollSpellsDown() {
		if (spellsUIPage <= 0)
			return;

		for (int i = 0; i < spellsUIItems.Length; i++) {
			spellsUIItems [i].scrollDown ();
		}
		spellsUIPage--;
	}
	public static void ScrollImbuesUp() {
		if (imbuesUIPage >= (Player.unlockedImbues.Length-1) / 3)
			return;

		for (int i = 0; i < imbuesUIItems.Length; i++) {
			imbuesUIItems [i].scrollUp ();
		}
		imbuesUIPage++;
	}
	public static void ScrollImbuesDown() {
		if (imbuesUIPage <= 0)
			return;

		for (int i = 0; i < spellsUIItems.Length; i++) {
			imbuesUIItems [i].scrollDown ();
		}
		imbuesUIPage--;
	}
}
