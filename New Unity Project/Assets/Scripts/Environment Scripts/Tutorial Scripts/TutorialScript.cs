using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour {

    public GameObject[] tutorialTexts;
    public GameObject[] interactables;
    public GameObject[] obstructions;
    private int tutorialStage;
    public int castProgression;
	// Use this for initialization
	void Start () {
        tutorialStage = 0;
        castProgression = 0;
    }
	
	// Update is called once per frame
	void Update () {
        switch (tutorialStage)
        {
            case 0:
                if(interactables[tutorialStage].GetComponent<InteractableScript>().triggered == true)
                {
                    tutorialTexts[tutorialStage].SetActive(false);
                    interactables[0].SetActive(false);
                    tutorialStage++;
                    tutorialTexts[tutorialStage].SetActive(true);
                    UpdateTerrain();
                }
                break;
            case 1:
                if(Input.GetKeyDown(KeyCode.LeftShift) == true)
                {
                    tutorialTexts[tutorialStage].SetActive(false);
                    tutorialStage++;
                    tutorialTexts[tutorialStage].SetActive(true);
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Z) == true)
                {
                     castProgression++;
                }
                if (castProgression >= 2)
                {
                    if (Input.GetKeyUp(KeyCode.LeftShift) == true)
                    {
                        tutorialTexts[tutorialStage].SetActive(false);
                        tutorialStage++;
                        tutorialTexts[tutorialStage].SetActive(true);
                        interactables[1].SetActive(true);
                    }
                }
                else if(Input.GetKeyUp(KeyCode.LeftShift) == true)
                {
                    castProgression = 0;
                    tutorialTexts[tutorialStage].SetActive(false);
                    tutorialStage--;
                    tutorialTexts[tutorialStage].SetActive(true);
                }
                break;
            case 3:
                if(interactables[1].GetComponent<InteractableScript>().triggered == true)
                {
                    tutorialTexts[tutorialStage].SetActive(false);
                    interactables[1].SetActive(false);
                    tutorialStage++;
                    tutorialTexts[tutorialStage].SetActive(true);
                    UpdateTerrain();
                }
                break;
            case 4:
                if (GameObject.FindGameObjectWithTag("Player").transform.position.z >= -8.00)
                {
                    tutorialTexts[tutorialStage].SetActive(false);
                    tutorialStage++;
                    tutorialTexts[tutorialStage].SetActive(true);
                }
                break;
        }
	}

    private void UpdateTerrain()
    {
        switch (tutorialStage)
        {
            case 1:
                obstructions[0].SetActive(false);
                break;
            case 4:
                obstructions[1].SetActive(false);
                break;
        }
    }
}
