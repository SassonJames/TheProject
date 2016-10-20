﻿using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	public GameObject player;
	public GameObject leftBoundObject;
	public GameObject rightBoundObject;

	public float tracking = 0.1f;

	private float leftBound;
	private float rightBound;
	private float boundDist;

    private Vector3 camOutsidePos;
    private Vector3 camInsidePos;

    private Quaternion camOutsideRot;
    private Quaternion camInsideRot;

    public bool transitioning;
    private bool insideBuilding;

	void Start() {
		leftBound = leftBoundObject.transform.position.x;
		rightBound = rightBoundObject.transform.position.x;
        camOutsidePos = Vector3.zero;
        camInsidePos = Vector3.zero;
        camInsideRot = new Quaternion();
        camOutsideRot = new Quaternion();
		boundDist = GetComponent<Camera> ().orthographicSize * GetComponent<Camera> ().aspect - 2;
        insideBuilding = false;
	}

	// Update is called once per frame
	void Update () {

        if (transitioning == true)
        {
            if (insideBuilding == true)
            {
                transform.position = Vector3.Lerp(transform.position, camInsidePos, Time.deltaTime*2);
                transform.rotation = Quaternion.Lerp(transform.rotation, camInsideRot, Time.deltaTime*2);
				if (Vector3.Distance(player.transform.position, camInsidePos) < 0.2f)
                {
                    transitioning = false;
                    player.GetComponent<PlayerMove>().isWalking = true;
                }
                if(player.transform.rotation.eulerAngles.y <= 90.0f)
                {
                    player.transform.rotation = Quaternion.Euler(new Vector3(0, 90.0f, 0));
                }
                else
                {
					player.GetComponent<PlayerMove>().RotateCharacter();
                }
                return;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, camOutsidePos, Time.deltaTime * 3);
                transform.rotation = Quaternion.Lerp(transform.rotation, camOutsideRot, Time.deltaTime * 3);
                if (Vector3.Distance(transform.position, camOutsidePos) < 0.01f)
                {
                    transitioning = false;
					player.transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
					player.GetComponent<PlayerMove>().isWalking = true;
                }
				if (player.transform.rotation.eulerAngles.y >= 180.0f)
                {
					player.transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
                }
                else
                {
					player.GetComponent<PlayerMove>().RotateCharacter();
                }
                return;
            }
        }
        if (insideBuilding != true)
        {
			float x = player.transform.position.x;

            if (x - leftBound < boundDist)
            {
                return;
            }
            if (rightBound - x < boundDist)
                return;

			Vector3 newPos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, tracking);
        }
	}

    public void ChangeToBuildingCamera(Vector3 buildingPos, Vector3 buildingRot)
    {
        camOutsidePos = transform.position;
        camOutsideRot = transform.rotation;

        camInsidePos = buildingPos;
        camInsideRot = Quaternion.Euler(buildingRot);

        transitioning = true;
        insideBuilding = true;
    }

    public void ChangeToOutsideCamera()
    {
        transitioning = true;
        insideBuilding = false;
    }
}