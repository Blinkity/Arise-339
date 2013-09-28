using UnityEngine;
using System.Collections;

public class QuirkyPlaneMover : MonoBehaviour {
	public VirtualJoystick virtualJoystick; 
	public int speed; 
	
	
	
	
	// Use this for initialization
	void Start () {
		 
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(virtualJoystick.GetAxis("Vertical") * 4, virtualJoystick.GetAxis("Horizontal") * 4, 0);  
		transform.Translate( new Vector3(0, 0, speed) * Time.deltaTime); 
		
		
	}
	
}
