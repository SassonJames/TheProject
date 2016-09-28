using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	private float speed;

    public bool isJumping;
    public bool isWalking;
    public bool grounded;
    public bool insideBuilding;

    public float airTime;
	
	private Rigidbody rb;

    // Use this for initialization
    void Start () {
        speed = 0.1f;
        isWalking = true;
        isJumping = false;
        insideBuilding = false;
        airTime = 0f;
        Physics.gravity = new Vector3(0.0f, -1.2f, 0.0f);
		rb = GetComponent<Rigidbody> ();
    }
	
	// Update is called once per frame
	void Update () {
        if (isWalking == true)
        {
            Jump();
            Walk();
        }
        Ground();
	}

    void Walk()
    {	
            AddWalkVelocity();
            /*
             * Check for Collision
             */
            transform.position += GetComponent<Rigidbody>().velocity;
    }

    void Jump()
    {
        if (grounded == true)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f, GetComponent<Rigidbody>().velocity.z);
        }
        if (Input.GetKey("space") && airTime < 15)
        {
            isJumping = true;
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.15f, GetComponent<Rigidbody>().velocity.z);
            airTime += 1;
            isJumping = true;
        }
        if (Input.GetKeyUp("space"))
        {
            isJumping = false;
            airTime = 15;
        }
        if(airTime >= 15)
        {
            isJumping = false;
        }
    }

    void Ground()
    {
        float distToGround = this.GetComponent<Collider>().bounds.extents.y;
        if (Physics.Raycast(transform.position, -Vector3.up, distToGround))
        {
            grounded = true;
            airTime = 0;
        }
        else
        {
            grounded = false;
        }
    }

    public void RotateCharacter()
    {
        this.transform.LookAt(GameObject.FindGameObjectWithTag("Camera").transform.position);
        this.transform.rotation = new Quaternion(0, this.transform.rotation.y, 0, this.transform.rotation.w);
    }

    void OnCollisionEnter (Collision col)
    {
    }
	void OnCollisionExit (Collision col)
    {
	}

    private void AddWalkVelocity()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis("Horizontal") * speed, GetComponent<Rigidbody>().velocity.y, Input.GetAxis("Vertical") * speed);
    }
}
