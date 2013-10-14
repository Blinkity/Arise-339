using UnityEngine;
using System.Collections;

public class OrigMover : MonoBehaviour {
	
	public float speed = 2F; 
	public float horizontalRotationSpeed = 2.0F;
	public float verticalRotationSpeed = 1.5F;
	
	//public static Vector3 baseLocation = new Vector3(150,0,150);
	
	public MouseLook mouseLook; 
	public GameObject camera; 
	public GameObject squadronBase; 
	
	
	//public bool canMove = false; 
	public enum PersonState {WatchingAirplanes, RunningToBase, AtBase}; 
	public PersonState currentState = PersonState.WatchingAirplanes; 
	
	// Use this for initialization
	void Start () {
		 
	}
	
	public float distance; 
	public float threshold; 
	
	// Update is called once per frame
	void Update () {
		if (currentState == PersonState.WatchingAirplanes) {
			if ( Input.GetKeyDown(KeyCode.Space) == true ) {
				//this works for standing up. kinda sucks for running 
				mouseLook.transform.Translate(0, 5, 0, Space.World);
				mouseLook.maximumY = 360;
				mouseLook.minimumY = -360;
				mouseLook.rotationY = -90;
				
				currentState = PersonState.RunningToBase; 
			}
			
		}else if (currentState == PersonState.RunningToBase) { 
			Vector3 directionToRun = (SharedVariables.baseLocation - transform.position); 
			
			//float 
			distance = Vector3.Distance(squadronBase.transform.position, transform.position);
			
			directionToRun.y = 0; 
			directionToRun.Normalize(); 
			
			camera.transform.Translate(speed * directionToRun * Time.deltaTime,
				Space.World);
			
			threshold = (squadronBase.transform.lossyScale.z / 2) + 20; 
			
			if (distance < threshold){
				currentState = PersonState.AtBase; 
			}
			
		}else if (currentState == PersonState.AtBase) {
			//do nothing
		}
	}
	
}
