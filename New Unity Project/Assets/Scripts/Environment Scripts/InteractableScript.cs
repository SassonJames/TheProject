using UnityEngine;
using System.Collections;

public class InteractableScript : MonoBehaviour {

    public bool triggered;

	// Use this for initialization
	void Start () {
        triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        triggered = true;
    }

}
