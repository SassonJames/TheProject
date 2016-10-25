using UnityEngine;
using System.Collections;

public class Move : BaseImbue {

	int delay;
	string imbue;
	public float x_speed;
    public float y_speed;
    public float z_speed;
    public float speed;
	
	/*
	 * Get the spell arguments from the ScriptManager.
	 */
	void Start () {
		if (!manager.isCustom) {
			speed   = float.Parse(args [1]);
			// Allow for either 2 or 3 args. 3rd arg is the xyz speed vector
			// Separated by commas
			// X speed default to 1, Z speed default to 0
			if (args.Length < 3) {
				x_speed = 1;
				y_speed = 0;
			} else {
				string[] speeds = args[2].Split(',');
				x_speed = float.Parse(speeds [0].Trim());
                y_speed = float.Parse(speeds[1].Trim());
                z_speed = float.Parse(speeds[2].Trim());
            }

			if (args [args.Length - 1] != "add") {
				manager.RemoveMovementScripts (this);
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Check if the object is being held in place
		if (!manager.shouldUpdate)
			return;

		//Debug.Log("MOVING DIAGONALLY GUISE" + speed);
		// Move the x position of this Object forward
		Vector3 move = new Vector3(x_speed*speed, y_speed*speed, z_speed*speed);
		transform.position += move * Time.deltaTime;
	}
}
