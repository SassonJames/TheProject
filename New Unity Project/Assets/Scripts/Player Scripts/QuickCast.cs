using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Current state of crafting an on the fly spell
public enum CraftingState
{
    NOT_CRAFTING = 0,
    CHOOSING_TYPE = 1,
    CHOOSING_ELEMENT = 2,
    CHOOSING_AUGMENTATION = 3,
    CAST_SPELL = 4,
};

// This behavior is for the player to create and cast a spell on the fly
public class QuickCast : MonoBehaviour {

    public CraftingState playerCraftingState;

    private List<string> spellTypes;
    private List<string> spellElements;
    private List<string> spellAugments;
    private string spellString;
    private bool isSelecting;
    private int augmentationCount;
    private UIDisplay playerUIDisplay;
    private PlayerStats playerStats;

	/*
	// Use this for initialization
	void Start () {
        playerUIDisplay = this.GetComponentInChildren<UIDisplay>();
        playerStats = this.GetComponent<PlayerStats>();
        playerCraftingState = CraftingState.NOT_CRAFTING;
        spellString = null;
        isSelecting = false;
        augmentationCount = 0; 
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyUp(KeyCode.LeftShift) == true || augmentationCount >= 3)
        {
            playerUIDisplay.KillDisplay();
            playerCraftingState = CraftingState.CAST_SPELL;
            Debug.Log("Spell Was Cast: " + spellString);
            spellString = null;
            augmentationCount = 0;
            isSelecting = false;
            playerCraftingState = CraftingState.NOT_CRAFTING;
        }

        switch (playerCraftingState)
        {
            case CraftingState.NOT_CRAFTING:
                if(Input.GetKeyDown(KeyCode.LeftShift) == true)
                {
                    playerCraftingState = CraftingState.CHOOSING_TYPE;
                }
                break;
            case CraftingState.CHOOSING_TYPE:
                ChooseType();
                break;
            case CraftingState.CHOOSING_ELEMENT:
                ChooseElement();
                break;
            case CraftingState.CHOOSING_AUGMENTATION:
                ChooseAugmentation();
                break;
            case CraftingState.CAST_SPELL:
                break;
        }
	}

    void ChooseType()
    {
        if(isSelecting != true)
        {
            playerUIDisplay.PresentOptions(playerStats.playerSpellTypes, playerStats.playerSpellTypes.Count);
            isSelecting = true;
        }
        else if(Input.GetKeyDown(KeyCode.Return) == true)
        {
            spellString += (playerUIDisplay.Select() + " ");
            isSelecting = false;
            playerCraftingState = CraftingState.CHOOSING_ELEMENT;
        }
    }

    void ChooseElement()
    {
        if (isSelecting != true)
        {
            playerUIDisplay.PresentOptions(playerStats.playerSpellElements, playerStats.playerSpellElements.Count);
            isSelecting = true;
        }
        else if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            spellString += (playerUIDisplay.Select() + " ");
            isSelecting = false;
            playerCraftingState = CraftingState.CHOOSING_AUGMENTATION;
        }
    }

    void ChooseAugmentation()
    {
        if (isSelecting != true)
        {
            playerUIDisplay.PresentOptions(playerStats.playerSpellAugments, playerStats.playerSpellAugments.Count);
            isSelecting = true;
        }
        else if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            spellString += (playerUIDisplay.Select() + " ");
            isSelecting = false;
            playerCraftingState = CraftingState.CHOOSING_AUGMENTATION;
            augmentationCount++;
        }
    }
    */
}
