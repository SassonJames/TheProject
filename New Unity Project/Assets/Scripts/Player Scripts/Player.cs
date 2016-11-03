using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class Player : MonoBehaviour {

	// underscore names are to organize the preview in the editor
	public bool prefabs____________________________________;
	public GameObject damagePrefab;
	public GameObject negatePrefab;
	public GameObject selectorPrefab;

	public bool gameobjects_________________________________;
	public GameObject manaField;
	public GameObject hpField;
	public GameObject errorField;

	public bool raw_values____________________________________;
	public Vector3 spellPosition;
	public string[] _unlockedSpells;
	public string[] _unlockedImbues;

	public bool init_on_start__________________________________;
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
	private bool autoscroll = false;
	public GameObject selector;
	private static bool keepStaticFields;

	public void Awake() {
		if (keepStaticFields) {
			return;
		}
		unlockedImbues = _unlockedImbues;
		unlockedSpells = _unlockedSpells;
	}

	public void Start() {
		keepStaticFields = true;
		Camera.main.GetComponent<FollowCam> ().player = this.gameObject;
		GameManager.MainPlayer = this.gameObject;

		imbues = new Stack ();
		spells = new Stack ();

		selector = Instantiate (selectorPrefab) as GameObject;
		selector.transform.position = new Vector3 (-100, -100, -100);
	}

	public void Update() {

        //updateHPBar ();

        // Handle state changes
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        } if (Input.GetKeyDown(KeyCode.LeftShift)) {
            //Debug.Log("Casting...");
            casting = true;
            GameManager.ShowSpells();
        } if (Input.GetKeyUp(KeyCode.LeftShift)) {
            //Debug.Log("Done.");
            casting = false;
			GameManager.HideSpellUI();
            ReleaseSpell(); // Releasing shift releases the spell
		} 
		// For quick scrolling through the spell/imbue menu
		if (! (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow))) {
			autoscroll = false;
		}

        // Handle input while casting

        // If no spell is active, use hotkeys create base spell
		if (casting && activeSpell == null) {
			// Arrow keys scroll through the options
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				GameManager.ScrollSpellsUp ();
				Invoke ("AutoScroll", 0.5f);
			} if (Input.GetKeyDown (KeyCode.UpArrow)) {
				GameManager.ScrollSpellsDown ();
				Invoke ("AutoScroll", 0.5f);
			}
			if (autoscroll && Input.GetKey(KeyCode.DownArrow)) {
				GameManager.ScrollSpellsUp ();
				autoscroll = false;
				Invoke ("AutoScroll", 0.1f);
			} if (autoscroll && Input.GetKey(KeyCode.UpArrow)) {
				GameManager.ScrollSpellsDown ();
				autoscroll = false;
				Invoke ("AutoScroll", 0.1f);
			}
			for (int i=0; i<GameManager.hotkeys.Length; i++) {
                if (i > unlockedSpells.Length) break;
				if (Input.GetKeyDown(GameManager.hotkeys[i])) {
					if (CastSpell(GameManager.spellsUIItems[i].getSelection()) != null)
						GameManager.ShowImbues(); 
					break;
				}
            }
        // Else, attach imbues to active spell
		} else if (casting) {
			// Arrow keys scroll through the options
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				GameManager.ScrollImbuesUp ();
				Invoke ("AutoScroll", 0.5f);
			} if (Input.GetKeyDown(KeyCode.UpArrow)) {
				GameManager.ScrollImbuesDown ();
				Invoke ("AutoScroll", 0.5f);
			}
			if (autoscroll && Input.GetKey(KeyCode.DownArrow)) {
				GameManager.ScrollImbuesUp ();
				autoscroll = false;
				Invoke ("AutoScroll", 0.1f);
			} if (autoscroll && Input.GetKey(KeyCode.UpArrow)) {
				GameManager.ScrollImbuesDown ();
				autoscroll = false;
				Invoke ("AutoScroll", 0.1f);
			}
			for (int i = 0; i < GameManager.hotkeys.Length; i++) {
				if (i > unlockedImbues.Length) break;
				if (Input.GetKeyDown (GameManager.hotkeys [i])) Imbue (GameManager.imbuesUIItems [i].getSelection ());
            }
        }
		// Make the active spell follow the player.
		if (activeSpell != null)
			activeSpell.gameObject.transform.position = Vector3.Lerp(activeSpell.gameObject.transform.position, transform.position + spellPosition, 0.1f);
    }
	public void AutoScroll() {autoscroll = true;}

	/**
	 * Create a new Spell in front of the player
	 */
	public GameObject CastSpell(string type) {

		if (stunned || type == null || type == "")
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
	 * Allow the current spell to update
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
	 */
	public void Imbue(string imbuement) {
		if (activeSpell == null || imbuement == null || imbuement == "")
			return;
		imbuement = imbuement.Replace (" ", "");
		//string[] tokens = imbuement.Split (':');
		string[] tokens = ParseImbuement(imbuement);

		imbuement = tokens [0];
		BaseImbue scriptComponent = activeSpell.GetComponent<ScriptManager>().attachScript (imbuement) as BaseImbue;

		// Add the imbuement to the stack
		if (scriptComponent != null) {
			scriptComponent.args = tokens;
			scriptComponent.manager = activeSpell.GetComponent<ScriptManager> ();
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

		// Start putting the tokens in the list
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
	 * Handle collisions with spells. Currently just take damage.
	 */ 
	public void OnTriggerEnter(Collider coll) {
		//Debug.Log (coll.gameObject.name);
		if (coll.gameObject.tag == "DamageSpell" && coll.GetComponent<ScriptManager>().owner != this.gameObject) {
			health -= coll.GetComponent<Damage>().damage;
			Destroy (coll.gameObject);
			//GameManager.audio.PlayOneShot(GameManager.damage);
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
		Destroy(selector);
    }
}
  