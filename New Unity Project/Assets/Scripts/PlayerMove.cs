using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public float speed;

    public bool isJumping;
    public bool isWalking;
    public bool grounded;
    public bool insideBuilding;

    public float airTime;
	
	private Rigidbody rb;

    // Use this for initialization
    void Start () {
        isWalking = true;
        isJumping = false;
        insideBuilding = false;
        airTime = 0f;
		rb = GetComponent<Rigidbody> ();
    }
	
	// Update is called once per frame
	void Update () {
        if (isWalking == true)
        {
            Walk();
            Jump();
        }
        Ground();
	}

    void Walk()
    {
<<<<<<< HEAD
		transform.Translate(new Vector3(Input.GetAxis ("Horizontal")*speed, 0f, Input.GetAxis("Vertical")*speed));
		//transform.position += new Vector3(Input.GetAxis ("Horizontal")*speed, 0f, Input.GetAxis("Vertical")*speed);
		//rb.velocity = new Vector3(Input.GetAxis ("Horizontal")*speed, rb.velocity.y, Input.GetAxis("Vertical")*speed);

	}
=======
        if (insideBuilding != true)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal") * speed, 0f, Input.GetAxis("Vertical") * speed);
        }
        else
        {
            transform.position += new Vector3(Input.GetAxis("Vertical") * speed * -1, 0f, Input.GetAxis("Horizontal") * speed);
        }

    }
>>>>>>> 7655a2c8781b2a7a02bcb1ca58d9a0fb0cc1fb1b
    void Jump()
    {
        if (Input.GetKeyDown("space") && grounded == true)
        {
            isJumping = true;
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

    void Ground()
    {
        float distToGround = this.GetComponent<Collider>().bounds.extents.y;
        if (Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.3f))
        {
            grounded = true;
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
}
