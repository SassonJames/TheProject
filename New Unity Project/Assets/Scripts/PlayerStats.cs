using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour {

    public int playerHealth;
    public int playerMana;
    public List<string> playerSpellTypes;
    public List<string> playerSpellElements;
    public List<string> playerSpellAugments;

	// Use this for initialization
	void Start () {
        playerHealth = 10;
        playerMana = 20;
        PopulateSpellTypes();
        PopulateSpellElements();
        PopulateSpellAugments();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void PopulateSpellTypes()
    {
        playerSpellTypes.Add("Damage");
        playerSpellTypes.Add("Heal");
        playerSpellTypes.Add("Stun");
        playerSpellTypes.Add("Scan");
    }

    private void PopulateSpellElements()
    {
        playerSpellElements.Add("Wind");
        playerSpellElements.Add("Water");
        playerSpellElements.Add("Fire");
    }

    private void PopulateSpellAugments()
    {
        playerSpellAugments.Add("Move Forward");
        playerSpellAugments.Add("Orbit");
        playerSpellAugments.Add("Amplify Type");
        playerSpellAugments.Add("Amplify Element");
    }
}
