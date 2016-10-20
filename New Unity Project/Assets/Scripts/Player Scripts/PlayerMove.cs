using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMove : MonoBehaviour {

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

		Ground();
		m_vPreviousPosition = transform.position;
	}

    void Walk()
    {	
            AddWalkVelocity();
    }

    void Jump()
	{
        if (Input.GetKey("space") && airTime < maxAirTime)
        {
			
            isJumping = true;
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
        float distToGround = this.GetComponent<Collider>().bounds.extents.y + 0.001f;
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
        if(col.gameObject.tag == "LevelChangeCollider")
        {
            ChangeLevel(col.gameObject);
        }
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

    void ChangeLevel(GameObject obj)
    {
		GameManager.StorePersistentData ();
        int goToLevel = obj.GetComponent<LevelChangerData>().toLevel;
        Debug.Log("I hit tit.");
        float fadeTime = this.GetComponent<FadeTransition>().BeginFade(1);
        SceneManager.LoadScene(goToLevel);
    }


}
