using UnityEngine;
using System.Collections;

public class PlaneMover : MonoBehaviour {
	public VirtualJoystick virtualJoystick; 
	public float speed = 20F; 
	public float horizontalRotationSpeed = 2.0F;
	public float verticalRotationSpeed = 1.5F;
	
	// Use this for initialization
	void Start () {
		 
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(virtualJoystick.GetAxis("Vertical") * horizontalRotationSpeed, virtualJoystick.GetAxis("Horizontal") * verticalRotationSpeed, 0);  
		transform.Translate( new Vector3(0, 0, speed) * Time.deltaTime); 
	}
	
}
