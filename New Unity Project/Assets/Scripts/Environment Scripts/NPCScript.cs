using UnityEngine;
using System.Collections;

public class NPCScript : MonoBehaviour {

    public Texture promptTexture;

    private GameObject interactionPrompt;
    private GameObject player;
    private GameObject dialoguePrompt;
    private bool playerNear;
    private bool talking;

	// Use this for initialization
	void Start () {
        interactionPrompt = null;
        dialoguePrompt = null;
        player = GameObject.FindGameObjectWithTag("Player");
        playerNear = false;
        talking = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (playerNear)
        {
            Talk();
        }
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (interactionPrompt == null)
            {
                OfferInteraction();
                playerNear = true;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if(interactionPrompt!= null)
            {
                DisableInteraction();
                playerNear = false;
            }
        }
    }

    void Talk()
    {
        if (Input.GetKeyDown(KeyCode.Z) == true && talking == false)
        {
            player.GetComponent<PlayerMove>().isWalking = false;
            GameObject dialogue = new GameObject();
            TextMesh optionText = dialogue.AddComponent<TextMesh>();
            dialogue.GetComponent<TextMesh>().text = "Thanks for talking to me! Press 'Z' To continue...";
            dialogue.transform.position = (this.gameObject.transform.position + new Vector3(this.gameObject.transform.localScale.x * -2.0f, this.gameObject.transform.localScale.y * 3 / 4, 0));
            dialogue.transform.localScale *= 0.25f;
            dialoguePrompt = dialogue;
            DisableInteraction();
            talking = true;
        }
        else if(Input.GetKeyDown(KeyCode.Z) == true && talking == true)
        {
            Destroy(dialoguePrompt);
            OfferInteraction();
            player.GetComponent<PlayerMove>().isWalking = true;
            talking = false;
        }

    }

    void OfferInteraction()
    {
        GameObject prompt = GameObject.CreatePrimitive(PrimitiveType.Cube);
        prompt.transform.position = (this.gameObject.transform.position + new Vector3(0, this.gameObject.transform.localScale.y * 3 / 4, 0));
        prompt.transform.localScale *= 0.5f;
        prompt.transform.localScale = new Vector3(prompt.transform.localScale.x * 1.25f, prompt.transform.localScale.y * 0.75f, prompt.transform.localScale.z * 0.10f);
        prompt.AddComponent<BasicRotation>();
        prompt.GetComponent<Renderer>().material.mainTexture = promptTexture;
        interactionPrompt = prompt;
    }

    void DisableInteraction()
    {
        Destroy(interactionPrompt);
        interactionPrompt = null;
    }
}
