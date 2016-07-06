using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public float speed;

    public bool isJumping;
    public bool grounded;
    public float airTime;

    public bool walkingUp;
    public bool walkingDown;
    public bool walkingLeft;
    public bool walkingRight;
	
	private bool isWalking;
	private Rigidbody rb;

    // Use this for initialization
    void Start () {
        walkingUp = false;
        walkingDown = false;
        walkingLeft = false;
        walkingRight = false;
        isWalking = false;
        isJumping = false;
        airTime = 0f;
		rb = GetComponent<Rigidbody> ();
    }
	
	// Update is called once per frame
	void Update () {
        Walk();
        Jump();
	}

    void Walk()
    {

		transform.position += new Vector3(Input.GetAxis ("Horizontal")*speed, 0f, Input.GetAxis("Vertical")*speed);

		/*
        if (Input.GetKeyDown("w") && walkingUp == false)
        {
            walkingUp = true;
            isWalking = true;
        }
        if (Input.GetKeyDown("a") && walkingLeft == false)
        {
            walkingLeft = true;
            isWalking = true;
        }
        if (Input.GetKeyDown("s") && walkingDown == false)
        {
            walkingDown = true;
            isWalking = true;
        }
        if (Input.GetKeyDown("d") && walkingRight == false)
        {
            walkingRight = true;
            isWalking = true;
        }

        if(isWalking == true)
        {
            if (walkingUp == true)
            {
                if( Input.GetKeyUp("w") != true)
                {
                    this.transform.position += new Vector3(0f, 0f, 0.075f);
                }
                else { walkingUp = false; }
            }
            if (walkingLeft == true)
            {
                if (Input.GetKeyUp("a") != true)
                {
                    this.transform.position += new Vector3(-0.075f, 0f, 0f);
                }
                else { walkingLeft = false; }
            }
            if (walkingDown == true)
            {
                if (Input.GetKeyUp("s") != true)
                {
                    this.transform.position += new Vector3(0f, 0f, -0.075f);
                }
                else { walkingDown = false; }
            }
            if (walkingRight == true)
            {
                if (Input.GetKeyUp("d") != true)
                {
                    this.transform.position += new Vector3(0.075f, 0f, 0f);
                }
                else { walkingRight = false; }
            }
            if(walkingUp != true && walkingLeft != true && walkingRight != true && walkingDown != true)
            {
                isWalking = false;
            }
        }
        */
    }
    void Jump()
    {
        if (Input.GetKeyDown("space") && grounded == true)
        {
            isJumping = true;
            grounded = false;
        }
        if(isJumping == true)
        {
            if (airTime <= 0.15f && Input.GetKeyUp("space") != true)
            {
                this.transform.Translate(Vector3.up * 9 * Time.deltaTime, Space.World);
                airTime += Time.deltaTime;
            }
            else if (airTime <= 0.3 && Input.GetKeyUp("space") != true)
            {
                this.transform.Translate(Vector3.up * 7 * Time.deltaTime, Space.World);
                airTime += Time.deltaTime;
            }
            else if (airTime <= 0.45 && Input.GetKeyUp("space") != true)
            {
                this.transform.Translate(Vector3.up * 4 * Time.deltaTime, Space.World);
                airTime += Time.deltaTime;
            }
            else
            {
                airTime = 0;
                isJumping = false;
            }
        }
    }
    void OnCollisionEnter (Collision col)
    {
        if(col.gameObject.tag == "Floor")
        {
            grounded = true;
        }
    }
	void OnCollisionExit (Collision col) {
		if(col.gameObject.tag == "Floor")
		{
			grounded = false;
		}
	}
}
