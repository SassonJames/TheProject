using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	/*
	 * Hey james
	 * pls put comments
	 * kthx
	 * ily
	 * <3
	 * dump christene for me, u know i got dat good good
	 *                                       ________
	 *    _-----__                         /      ~~  \
	 *  /          \_____________________/  ~~    ~~   \
	 * |__           ~          ~              ~~     /
	 * \           ______~_______________ ~~    ~~   \
	 *  \_     ___/                      \  ~~       /      <---- its me
	 *    -----                           \_____~~__/
	 * 
	 */


	public float speed;
	public float jumpSpeed;
	public float maxAirTime;
	public float gravity;

	public bool ____________________________;

    public bool isJumping;
	public bool isWalking;
	public bool grounded;
	public bool insideBuilding;

    float airTime;
	
	private Rigidbody rb;
	private Vector3 m_vPreviousPosition;

    // Use this for initialization
	void Start () {
		//speed = 0.1f;
        isWalking = true;
        isJumping = false;
        insideBuilding = false;
        airTime = 0f;
        Physics.gravity = new Vector3(0.0f, -gravity, 0.0f);
		rb = GetComponent<Rigidbody> ();
		m_vPreviousPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (isWalking == true)
        {
            Jump();
            Walk();
        }

		m_vPreviousPosition = transform.position;

        Ground();
	}

    void Walk()
    {	
            AddWalkVelocity();
            /*
             * Check for Collision
             */
            //transform.position += GetComponent<Rigidbody>().velocity;
    }

    void Jump()
	{
		/*
        if (grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
        }
        */

        if (Input.GetKey("space") && airTime < maxAirTime)
        {
			
            isJumping = true;
			//GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.15f, GetComponent<Rigidbody>().velocity.z);
			rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            airTime += 1;
        }
        if (Input.GetKeyUp("space"))
        {
            isJumping = false;
            airTime = maxAirTime;
        }
        if(airTime >= maxAirTime)
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
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, GetComponent<Rigidbody>().velocity.y, Input.GetAxis("Vertical") * speed);


		// I stole this from the internet
		RaycastHit hit;
		if(Physics.Linecast(m_vPreviousPosition, transform.position, out hit))
		{
			if (hit.collider)
			{
				transform.position = hit.point;
			}
		}
    }
}
