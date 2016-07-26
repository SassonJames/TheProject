using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	public GameObject cam;
	public GameObject leftBoundObject;
	public GameObject rightBoundObject;

	public float tracking;

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
		boundDist = cam.GetComponent<Camera> ().orthographicSize * cam.GetComponent<Camera> ().aspect - 2;
        insideBuilding = false;
	}

	// Update is called once per frame
	void Update () {

        if (transitioning == true)
        {
            if (insideBuilding == true)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, camInsidePos, Time.deltaTime*2);
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camInsideRot, Time.deltaTime*2);
                if (Vector3.Distance(cam.transform.position, camInsidePos) < 0.2f)
                {
                    transitioning = false;
                    this.GetComponent<PlayerMove>().isWalking = true;
                }
                if(transform.rotation.eulerAngles.y <= 90.0f)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 90.0f, 0));
                }
                else
                {
                    this.GetComponent<PlayerMove>().RotateCharacter();
                }
                return;
            }
            else
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, camOutsidePos, Time.deltaTime * 3);
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camOutsideRot, Time.deltaTime * 3);
                if (Vector3.Distance(cam.transform.position, camOutsidePos) < 0.01f)
                {
                    transitioning = false;
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
                    GetComponent<PlayerMove>().isWalking = true;
                }
                if (transform.rotation.eulerAngles.y >= 180.0f)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
                }
                else
                {
                    GetComponent<PlayerMove>().RotateCharacter();
                }
                return;
            }
        }
        if (insideBuilding != true)
        {
            float x = this.transform.position.x;

            if (x - leftBound < boundDist)
            {
                return;
            }
            if (rightBound - x < boundDist)
                return;

            Vector3 newPos = new Vector3(this.transform.position.x, cam.transform.position.y, cam.transform.position.z);
            cam.transform.position = Vector3.Lerp(cam.transform.position, newPos, tracking);
        }
	}

    public void ChangeToBuildingCamera(Vector3 buildingPos, Vector3 buildingRot)
    {
        camOutsidePos = cam.transform.position;
        camOutsideRot = cam.transform.rotation;

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
