using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class GameManager : MonoBehaviour {

	public GameObject MainPlayer;
	public Button SpellButtonPrefab;
	public RectTransform SpellsPanel;
	public RectTransform ImbuesPanel;
	public AudioClip _negate;
	public AudioClip _damage;

	public bool ______________________________;

	public static string[] Names;
	public static GameObject[] Spells;
	public bool CustomSpellBook;
	public Button[] CustomButtons;
	public Button[] NormalButtons;

	public static AudioClip negate;
	public static AudioClip damage;
	public static AudioSource audio;

	// Use this for initialization
	void Start () {
		negate = _negate;
		damage = _damage;
		Names = SpellMaker.Names;
		Spells = SpellMaker.Spells;

		CustomButtons = new Button[6];
		NormalButtons = new Button[Player.unlockedSpells.Length];

		setupSpells ();
		setupImbues ();
		loadCustomSpells ();
		audio = GetComponent<AudioSource> ();
	}

	void setupSpells() {
		string[] unlockedSpells = Player.unlockedSpells;
		for (int i=0; i<unlockedSpells.Length; i++) {
			string capture = unlockedSpells[i];
			Button spellButton = Instantiate(SpellButtonPrefab);
			spellButton.GetComponentInChildren<Text>().text = capture;
			spellButton.GetComponent<Button>().onClick.AddListener( () => {
				MainPlayer.GetComponent<Player> ().CastSpell (capture);
				audio.Play();
			});
			float height = spellButton.GetComponent<RectTransform>().rect.height;
			spellButton.transform.SetParent(SpellsPanel);
			spellButton.transform.localPosition = new Vector3(0,-height*i,0);
			NormalButtons[i] = spellButton;
		}
	}

	void setupImbues() {
		string[] unlockedImbues = Player.unlockedImbues;
		for (int i=0; i<unlockedImbues.Length; i++) {
			string capture = unlockedImbues[i];
			Button spellButton = Instantiate(SpellButtonPrefab);
			spellButton.GetComponentInChildren<Text>().text = "Imbue: "+capture;
			spellButton.GetComponent<Button>().onClick.AddListener( () => {
				MainPlayer.GetComponent<Player> ().imbue (capture);
				audio.Play();
			});
			float height = spellButton.GetComponent<RectTransform>().rect.height;
			spellButton.transform.SetParent(ImbuesPanel);
			spellButton.transform.localPosition = new Vector3(0,-height*i,0);
		}
	}
	
	/*
	 * Load all the prefabs from the Temporary folder and store them in the Spells array
	 */
	void loadCustomSpells() {
		
		// First, destroy the old buttons
		foreach (Button b in CustomButtons) {
			if (b != null)
				Destroy(b.gameObject);
		}
		
		// Find all the prefabs and store them
		string[] lookFor = new string[] {"Assets/Temporary"};
		string[] guids = AssetDatabase.FindAssets ("", lookFor);
		for (int i=0; i<guids.Length; i++) {
			string path = AssetDatabase.GUIDToAssetPath(guids[i]);
			Spells[i] = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
			Debug.Log(Spells[i]);
			
			// Find the Spell name from the path string
			string substr = path.Split('/')[2];
			Names[i] = substr.Substring(0, substr.Length - 7);
		}
		
		// Create the buttons
		for (int i=0; i<Names.Length; i++) {
			if (Names[i] == null || Names[i] == "")
				continue;
			string capture = Names[i];
			string idxcap = ""+i;
			Button spellButton = Instantiate(SpellButtonPrefab);
			spellButton.GetComponentInChildren<Text>().text = capture;
			spellButton.GetComponent<Button>().onClick.AddListener( () => {
				MainPlayer.GetComponent<Player>().CastSpell (idxcap);
				audio.Play();
			});
			float height = spellButton.GetComponent<RectTransform>().rect.height;
			spellButton.transform.SetParent(SpellsPanel);
			spellButton.transform.localPosition = new Vector3(0,-height*i,0);
			spellButton.gameObject.SetActive(CustomSpellBook);
			CustomButtons[i] = spellButton;
		}

		Button btn = Instantiate(SpellButtonPrefab);
		btn.GetComponentInChildren<Text>().text = "Clear";
		btn.GetComponent<Button>().onClick.AddListener( () => {
			MainPlayer.GetComponent<Player> ().CastSpell ("Clear");
			audio.Play();
		});
		float h = btn.GetComponent<RectTransform>().rect.height;
		btn.transform.SetParent(SpellsPanel);
		btn.transform.localPosition = new Vector3(0,-h*Names.Length,0);
	}
	
	/*
	 * Tobble between Normal/Custom spells
	 */
	public void ToggleSpellBook() {
		CustomSpellBook = ! CustomSpellBook;
		foreach (Button b in NormalButtons) {
			if (b != null)
				b.gameObject.SetActive(!CustomSpellBook);
		}
		foreach (Button b in CustomButtons) {
			if (b != null)
				b.gameObject.SetActive(CustomSpellBook);
		}
	}
}
