using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class Player : MonoBehaviour {

	public GameObject damagePrefab;
	public GameObject negatePrefab;
	public GameObject selector;
	public GameObject spellsUIPanel;
	public GameObject imbuesUIPanel;
    public GameObject[] _spellsUIItems;
    public GameObject[] _imbuesUIItems;
	public Text manaField;
	public Text hpField;
	public Text errorField;
	public Vector3 spellPosition;
	public string[] _unlockedSpells;
	public string[] _unlockedImbues;
    public string[] hotkeys;

	public bool ____________________________________;
	public static string[] unlockedSpells;
	public static string[] unlockedImbues;
	public int health = 100;
	public int mana   = 100;
	public bool stunned = false;
	public GameObject activeSpell;
	public string  activeImbue;
	public Stack spells;
	public Stack imbues;

	private bool casting = false;
	private CastUIItem[] spellsUIItems;
	private CastUIItem[] imbuesUIItems;

    public void Awake() {
		unlockedImbues = _unlockedImbues;
		unlockedSpells = _unlockedSpells;
	}

	public void Start() {
		imbues = new Stack ();
		spells = new Stack ();

		spellsUIItems = new CastUIItem[_spellsUIItems.Length];
		imbuesUIItems = new CastUIItem[_imbuesUIItems.Length];

		for (int i = 0; i < _spellsUIItems.Length; i++) {
			spellsUIItems [i] = _spellsUIItems [i].GetComponent<CastUIItem> ();
		}
		for (int i = 0; i < _imbuesUIItems.Length; i++) {
			imbuesUIItems [i] = _imbuesUIItems [i].GetComponent<CastUIItem> ();
		}

		HideSpellUI ();
		initCastUI ();
	}

	public void Update() {
        //if (activeSpell != nul
        //if (activeSpell != null && !activeSpell.GetComponent<ScriptManager>().shouldUpdate)
        //	activeSpell.gameObject.transform.position = transform.position + spellPosition;

        updateHPBar ();

        // Handle state changes
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        } if (Input.GetKeyDown(KeyCode.LeftShift)) {
            Debug.Log("Casting...");
            casting = true;
            ShowSpells();
        } if (Input.GetKeyUp(KeyCode.LeftShift)) {
            Debug.Log("Done.");
            casting = false;
            HideSpellUI();
            ReleaseSpell(); // Releasing shift releases the spell
        }

        // Handle input while casting

        // If no spell is active, use hotkeys create base spell
        if (casting && activeSpell == null) {
            for (int i=0; i<hotkeys.Length; i++) {
                if (i > unlockedSpells.Length) break;
				if (Input.GetKeyDown(hotkeys[i])) {
					if (CastSpell(spellsUIItems[i].getSelection()) != null) 
						ShowImbues();
					break;
				}
            }
        // Else, attach imbues to active spell
        } else if (casting) {
            for (int i = 0; i < hotkeys.Length; i++) {
                if (i > unlockedImbues.Length) break;
				if (Input.GetKeyDown(hotkeys[i])) Imbue(imbuesUIItems[i].getSelection());
            }
        }
		if (activeSpell != null)
			activeSpell.gameObject.transform.position = Vector3.Lerp(activeSpell.gameObject.transform.position, transform.position + spellPosition, 0.1f);
    }

	/**
	 * Create a new Spell in front of the player
	 */
	public GameObject CastSpell(string type) {

		if (stunned || !casting)
			return null;

		if (activeSpell != null)
			spells.Push (activeSpell);

		int manaCost = 0;

        switch (type) {
		case "Light Damage":
			manaCost = 10;
			// Return if too expensive
			if (manaCost > mana) {
				return null;
			}
			activeSpell = Instantiate(damagePrefab) as GameObject;
			activeSpell.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			activeSpell.GetComponent<Damage>().damage = 15;
			activeSpell.GetComponent<ScriptManager>().size = 0.5f;
			break;
		case "Damage":
			manaCost = 25;
			// Return if too expensive
			if (manaCost > mana) {
				return null;
			}
			activeSpell = Instantiate(damagePrefab) as GameObject;
			activeSpell.GetComponent<Damage>().damage = 25;
			break;
		case "Heavy Damage":
			manaCost = 75;
			// Return if too expensive
			if (manaCost > mana) {
				return null;
			}
			activeSpell = Instantiate(damagePrefab) as GameObject;
			activeSpell.transform.localScale = new Vector3(2, 2, 2);
			activeSpell.GetComponent<Damage>().damage = 50;
			activeSpell.GetComponent<ScriptManager>().size = 2f;
			break;
		case "Negation":
			manaCost = 25;
			// Return if too expensive
			if (manaCost > mana) {
				return null;
			}
			activeSpell = Instantiate(negatePrefab) as GameObject;
			activeSpell.GetComponent<ScriptManager>().size = 2f;
			break;
		case "Clear":
			reset();
			return null;
		default:
            // CUSTOM SPELLS ////////////////////////////////////////////////////////////////

            int idx = int.Parse(type);
			if (SpellMaker.Spells[idx] == null)
				return null;

			//activeSpell = Instantiate(SpellMaker.Spells[idx]) as GameObject;
			//activeSpell.gameObject.transform.position = new Vector3(0, 1, 0);

			manaCost = SpellMaker.Spells[idx].GetComponent<ScriptManager>().mana;
			// Return if too expensive
			if (manaCost > mana) {
				return null;
			}
			activeSpell = Instantiate(SpellMaker.Spells[idx]) as GameObject;
			mana -= manaCost;
			activeSpell.gameObject.transform.position = transform.position + spellPosition;
			activeSpell.GetComponent<ScriptManager> ().owner = this.gameObject;
            UpdateManabar();
			UpdateSelector();
			ReleaseSpell();
			return null;
		}
		mana -= manaCost;
		activeSpell.gameObject.transform.position = transform.position + spellPosition;
		activeSpell.GetComponent<ScriptManager>().mana = manaCost;
        activeSpell.GetComponent<ScriptManager>().owner = this.gameObject;
        activeSpell.GetComponent<ScriptManager>().target = this.gameObject;
        
        UpdateManabar ();
		UpdateSelector();
		return activeSpell;
	}

	/**
	 * Allow the current spell to move
	 */
	public void ReleaseSpell() {
		if (activeSpell == null)
			return;

		activeSpell.GetComponent<ScriptManager> ().shouldUpdate = true;
		if (spells.Count > 0) {
			activeSpell = (GameObject)spells.Pop ();
		} else {
			activeSpell = null;
		}

		UpdateSelector ();
	}

	/**
	 * Attach a script to the active spell.
	 * The input string is broken up into tokens and passed to the imbuement script as arguments.
	 * 
	 * Test:aaa:bbb:(ccc:ddd:(eee:fff:(ggg)):hhh):iii:((jjj:kkk):lll):(mmm):nnn
	 * 
	 */
	public void Imbue(string imbuement) {
		if (activeSpell == null)
			return;
		imbuement = imbuement.Replace (" ", "");
		//string[] tokens = imbuement.Split (':');
		string[] tokens = ParseImbuement(imbuement);
		foreach (string s in tokens)
			Debug.Log (s);
		imbuement = tokens [0];
		BaseImbue scriptComponent = activeSpell.GetComponent<ScriptManager>().attachScript (imbuement) as BaseImbue;

		// Add the imbuement to the stack
		if (scriptComponent != null) {
			scriptComponent.args = tokens;
			imbues.Push(activeImbue);
			activeImbue = imbuement;
			//Debug.Log (imbues.Peek());
		}
	}

	/*
	 * Breaks down an imbuement string into argument tokens for the imbuement script
	 * A token is defined as:
	 * 
	 * 		A string enclosed in the outer-most layer of parentheses
	 * 		A string separated by a colon
	 * 
	 * in that order.
	 * 
	 * "Test:aaa:bbb:(ccc:ddd:(eee:fff:(ggg)):hhh):iii:((jjj:kkk):lll):(mmm):nnn"
	 * yeilds
	 * ["Test", "aaa", "bbb", "ccc:ddd:(eee:fff:(ggg)):hhh", "iii", "(jjj:kkk):lll", "mmm", "nnn"]
	 */
	public static string[] ParseImbuement(string imbuement) {
		// First, split the string into tokens based on parentheses
		ArrayList p1 = new ArrayList ();
		ArrayList p2 = new ArrayList ();
		ArrayList parse = new ArrayList ();

		int pcount = 0;
		p1.Add (-1);
		p2.Add (-1);
		// Search for the outer-most parenthesis pairs
		for (int i = 0; i < imbuement.Length; i++) {
			if (imbuement [i] == '(') {
				//Debug.Log ("("+pcount);
				if (pcount == 0)
					p1.Add (i);
				pcount++;
			}
			if (imbuement [i] == ')') {
				pcount--;
				//Debug.Log (")"+pcount);
				if (pcount == 0)
					p2.Add (i);
			}
			if (pcount < 0) // Throw exceptions for poorly formed strings
				throw new UnityException ("Bad parentheses");
		}
		// Throw exceptions for poorly formed strings
		if (pcount > 0 || p1.Count != p2.Count)
			throw new UnityException ("Bad parentheses");

		// Start putting the tokens in list
		for (int i = 1; i < p1.Count; i++) {
			// +1 to remove ')' character
			int idx = (int)p2 [i - 1] + 1;
			int length = (int)p1 [i] - (int)p2 [i - 1] - 1;
			string substr = imbuement.Substring (idx, length);
			if (substr != "")
				parse.Add (substr);
			
			idx = (int)p1 [i];
			length = (int)p2 [i] - (int)p1 [i];
			substr = imbuement.Substring (idx, length);
			if (substr != "")
				parse.Add (substr);
		}
		// If there is still text after the last closing paren, add it as a token
		string last_token = imbuement.Substring ((int)p2 [p2.Count - 1] + 1, imbuement.Length - (int)p2 [p2.Count - 1] - 1);
		if (last_token != "")
			parse.Add (last_token);
		
		//foreach (string s in parse)
		//	Debug.Log (s);

		// Start splitting the non-paren tokens by ":"
		ArrayList tokens = new ArrayList ();
		for (int i = 0; i < parse.Count; i++) {
			if (((string)parse[i])[0] != '(') {
				string[] subarr = ((string)parse [i]).Split (':');
				foreach (string s in subarr)
					if (s != "")
						tokens.Add (s);
			} else
				tokens.Add (((string)parse [i]).Substring(1)); // Remove leading '('
		}

		// Convert to array. Default ToArray() method keeps returning an empty array (???)
		string[] ret = new string[tokens.Count];
		for (int i = 0; i < ret.Length; i++)
			ret [i] = (string)tokens [i];

		// return tokens.ToArray() as string[];
		return ret;
	}
	
	/*
	 * Handle collisions with spells. Currently just take dama	ge.
	 */ 
	public void OnTriggerEnter(Collider coll) {
		//Debug.Log (coll.gameObject.name);
		if (coll.gameObject.tag == "DamageSpell" && coll.GetComponent<ScriptManager>().owner != this.gameObject) {
			health -= coll.GetComponent<Damage>().damage;
			Destroy (coll.gameObject);
			GameManager.audio.PlayOneShot(GameManager.damage);
			if (health <= 0) {
				Destroy (this.gameObject);
			}
		}
	}

	/*
	 * Destroy all spells that have owner == this.gameobject
	 */
	private void reset() {
		GameObject[] damageSpells = GameObject.FindGameObjectsWithTag ("DamageSpell");
		GameObject[] negationSpells = GameObject.FindGameObjectsWithTag ("NegationSpell");
		foreach (GameObject o in damageSpells) {
			if (o.GetComponent<ScriptManager>().owner == this.gameObject) {
				Destroy (o);
			}
		}
		foreach (GameObject o in negationSpells) {
			if (o.GetComponent<ScriptManager>().owner == this.gameObject) {
				Destroy (o);
			}
		}
		imbues = new Stack ();
		spells = new Stack ();
		activeSpell = null;

		UpdateSelector ();
	}

	public void UpdateActiveSpell(GameObject test) {
		if (activeSpell==test) {
			ReleaseSpell();
		}
	}

    /**
     * Move the selection indicator to the correct position
     */
	public void UpdateSelector() {
		if (activeSpell == null) {
			selector.transform.parent = null;
			selector.transform.position = new Vector3 (-50, -50, -50);
		} else {
			selector.transform.position = activeSpell.transform.position;
			float size = activeSpell.GetComponent<ScriptManager>().size;
			selector.transform.localScale = new Vector3(size, size, 1f);
			selector.transform.SetParent (activeSpell.transform, true);
		}
	}

    /*************************************
     * UNFINISHED UI CODE HERE
     */

	/*
	 * Initialize the UI that appears as the player is casting a spell.
	 * Populate it with hotkeys, spell types, and imbuements.
	 */
	public void initCastUI() {
		// Set hotkeys, init option arrays
		for (int i = 0; i < hotkeys.Length; i++) {
			spellsUIItems [i].setHotkey (hotkeys [i]);
			spellsUIItems [i].options = new string[(unlockedSpells.Length/3)+1];

			imbuesUIItems [i].setHotkey (hotkeys [i]);
			imbuesUIItems [i].options = new string[(unlockedImbues.Length/3)+1];
		}

		// Populate option arrays. Option arrays are possible spell types/imbues
		for (int i = 0; i < unlockedSpells.Length; i++) {
			spellsUIItems [i % 3].options [i / 3] = unlockedSpells [i];
		}

		for (int i = 0; i < unlockedImbues.Length; i++) {
			imbuesUIItems [i % 3].options [i / 3] = unlockedImbues [i];
		}

		// Set initial selection
		for (int i = 0; i < hotkeys.Length; i++) {
			spellsUIItems [i].setSelection (spellsUIItems [i].options [0]);
			//spellsUIItems [i].setPostSelection (spellsUIItems [i].options [1]);

			imbuesUIItems [i].setSelection (imbuesUIItems [i].options [0]);
			//imbuesUIItems [i].setPostSelection (imbuesUIItems [i].options [1]);
		}

	}
    
    // The following 3 functions will show/hide relevant information regarding the 
    // spell casting process
    public void ShowSpells() {
		spellsUIPanel.SetActive (true);
		imbuesUIPanel.SetActive (false);
    }
	public void ShowImbues() {
		spellsUIPanel.SetActive (false);
		imbuesUIPanel.SetActive (true);
    }
	public void HideSpellUI() {
		spellsUIPanel.SetActive (false);
		imbuesUIPanel.SetActive (false);
    }

    // The following 3 functions will eventually control the UI Mana/HP display
    // For now, they will just log the numbers to the console window
	public void UpdateManabar() {
        //manaField.text = "Mana: " + mana;
        Debug.Log("Mana: " + mana);
        //TODO
    }
    public void updateHPBar() {
        //hpField.text = "HP: " + health;
        //Debug.Log("HP: " + health);
        //TODO
    }
    void OnDestroy() {
        //hpField.text = "HP: 0";
        //TODO
    }
}
  