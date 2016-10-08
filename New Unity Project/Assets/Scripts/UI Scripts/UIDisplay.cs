using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDisplay : MonoBehaviour{

    public GameObject player;
    private List<GameObject> presentedOptions;
    public int selectedOption;
    private bool optionsPresent;
    private string previousText;

	// Use this for initialization
	void Start () {
        presentedOptions = null;
        previousText = null;
        selectedOption = 0;
        optionsPresent = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (optionsPresent)
        {
            for (int j = 0; j < presentedOptions.Count; j++)
            {
                presentedOptions[j].transform.position = (player.transform.position + new Vector3(player.transform.localScale.x, 0.5f * (presentedOptions.Count - j), 0));
            }
            ChangeOption();
        }
    }

    public void PresentOptions(List<string> options, int numOptions)
    {
        presentedOptions = new List<GameObject>();
        for (int i = 0; i < numOptions; i++)
        {
            GameObject option = new GameObject();
            TextMesh optionText = option.AddComponent<TextMesh>();

            optionText.text = options[i];
            option.transform.localScale *= 0.5f;

            presentedOptions.Add(option);
        }
        previousText = options[selectedOption];
        presentedOptions[selectedOption].GetComponent<TextMesh>().text += " <--";
        optionsPresent = true;
    }

    private void ChangeOption()
    {
        if (Input.GetKeyDown("h") == true && optionsPresent)
        {
            presentedOptions[selectedOption].GetComponent<TextMesh>().text = previousText;
            if (selectedOption >= presentedOptions.Count-1)
            {
                selectedOption = 0;
            }
            else { selectedOption++; }
            previousText = presentedOptions[selectedOption].GetComponent<TextMesh>().text;
            presentedOptions[selectedOption].GetComponent<TextMesh>().text += " <--";
        }

        if (Input.GetKeyDown("y") == true && optionsPresent)
        {
            presentedOptions[selectedOption].GetComponent<TextMesh>().text = previousText;
            if (selectedOption <= 0)
            {
                selectedOption = presentedOptions.Count-1;
            }
            else { selectedOption--; }
            previousText = presentedOptions[selectedOption].GetComponent<TextMesh>().text;
            presentedOptions[selectedOption].GetComponent<TextMesh>().text += " <--";
        }
    }

    public string Select()
    {
        string optionChosen = presentedOptions[selectedOption].GetComponent<TextMesh>().text;
        KillDisplay();
        return optionChosen;
    }

    public void KillDisplay()
    {
        if (presentedOptions != null)
        {
            for (int i = presentedOptions.Count - 1; i >= 0; i--)
            {
                Destroy(presentedOptions[i]);
            }
            presentedOptions = null;
            selectedOption = 0;
            previousText = null;
            optionsPresent = false;
        }
    }
}
