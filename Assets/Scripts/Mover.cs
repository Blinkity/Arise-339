using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	public float speed = 2F; 
	public float horizontalRotationSpeed = 2.0F;
	public float verticalRotationSpeed = 1.5F;
	
	public static Vector3 baseLocation = new Vector3(150,0,150);
	
	public MouseLook mouseLook; 
	
	public bool canMove = false; 
	
	// Use this for initialization
	void Start () {
		 
	}
	
	// Update is called once per frame
	void Update () {
		if (canMove == false) {
			if ( Input.GetKeyDown(KeyCode.Space) == true ) {
				canMove = true; 
				mouseLook.transform.Translate(0, 1, 0, Space.World); 
				mouseLook.maximumY = 360;
				mouseLook.minimumY = -360;
				mouseLook.transform.Rotate(0, 0, 1, Space.World); 
				mouseLook.rotationX = 0;
				mouseLook.rotationY = -90; 
				
				//mouseLook.axes = MouseLook.RotationAxes.MouseX;
			}
			
		}
		
		if (canMove) { 
			Vector3 directionToRun = (baseLocation - transform.position); 
			directionToRun.y = 0; 
			directionToRun.Normalize(); 
			
			mouseLook.transform.Translate(Input.GetAxis("Vertical") * speed * directionToRun * Time.deltaTime,
				Space.World);
			
		}
	}
	
}
