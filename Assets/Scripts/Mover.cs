using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	
	public float speed = 2F; 
	public float horizontalRotationSpeed = 2.0F;
	public float verticalRotationSpeed = 1.5F;
	
	public static Vector3 baseLocation = new Vector3(150,0,150);
	
	public MouseLook mouseLook; 
	//public FPSWalkerEnhanced fpsWalker; 
	
	public GameObject camera; 
	
	//public bool canMove = false; 
	public enum PersonState {WatchingAirplanes, RunningToBase, AtBase}; 
	public PersonState currentState = PersonState.WatchingAirplanes; 
	
	// Use this for initialization
	void Start () {
		//fpsWalker.enabled = false; 
		
		mouseLook.maximumX = 55;
		mouseLook.minimumX = -55;
		mouseLook.maximumY = 55;
		mouseLook.minimumY = -55; 
		
		//mouseLook.axes = MouseLook.RotationAxes.MouseXAndY; 
		
		 
	}
	
	// Update is called once per frame
	void Update () {
		if (currentState == PersonState.WatchingAirplanes) {
			if ( Input.GetKeyDown(KeyCode.Space) == true ) {
				//this works for standing up. kinda sucks for running 
				mouseLook.transform.Translate(0, 5, 0, Space.World);
				mouseLook.maximumY = 360;
				mouseLook.minimumY = -360;
				mouseLook.rotationY = -90;
				
				//mouseLook.enabled = false; 
				//camera.transform.Rotate(90, 45, 0, Space.World); 
				
				//mouseLook.enabled = true; 
				
				currentState = PersonState.RunningToBase; 
			}
			
		}else if (currentState == PersonState.RunningToBase) { 
			mouseLook.axes = MouseLook.RotationAxes.MouseY;
			//fpsWalker.enabled = true; 
			
			Vector3 directionToRun = (baseLocation - transform.position); 
//			
			float distance = Vector3.Distance(baseLocation, transform.position);
//			
			directionToRun.y = 0; 
			directionToRun.Normalize(); 
//			
			transform.Translate(speed * directionToRun * Time.deltaTime,
				Space.World);
//			
			if (distance < 50){
				currentState = PersonState.AtBase; 
			}
			
		}else if (currentState == PersonState.AtBase) {
			//do nothing
		}
	}
	
}
