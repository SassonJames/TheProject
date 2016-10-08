using UnityEngine;
using System.Collections;

public class BuildingScript : MonoBehaviour {

    private GameObject player;
    public Vector3 CameraPosition;
    public Vector3 CameraRotation;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerMove>().isWalking = false;
            player.GetComponent<PlayerMove>().insideBuilding = true;
            player.GetComponent<PlayerMove>().RotateCharacter();
            player.GetComponent<FollowCam>().ChangeToBuildingCamera(CameraPosition,CameraRotation);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerMove>().isWalking = false;
            player.GetComponent<PlayerMove>().insideBuilding = false;
            player.GetComponent<PlayerMove>().RotateCharacter();
            player.GetComponent<FollowCam>().ChangeToOutsideCamera();
        }
    }
}
