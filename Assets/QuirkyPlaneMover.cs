using UnityEngine;
using System.Collections;
using System; 
using System.Collections.Generic; 

public class QuirkyPlaneMover : MonoBehaviour {
	public VirtualJoystick virtualJoystick; 
	public int speed; 
	
	//quirk
	public bool delayedResponse; 
	public bool randomDropper; 
	
	//delayedResponse properties
	int delaySeconds = 1; 
	Queue<Vector3> queuedRotations; 
	Queue<Vector3> queuedTranslations; 
	Queue<DateTime> queuedTimestamps; 
	
	//dropper properties
	long minSecondsBetweenDrops = 5; 
	int dropHeight = -30;
	double dropDuration = 1; 
	bool IS_CURRENTLY_DROPPING = false; 
	double secondsSinceLastDrop; 
	
	// Use this for initialization
	void Start () {
		queuedRotations = new Queue<Vector3>(); 
		queuedTranslations = new Queue<Vector3>(); 
		queuedTimestamps = new Queue<DateTime>(); 
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 rotation = new Vector3(virtualJoystick.GetAxis("Vertical") * 4, virtualJoystick.GetAxis("Horizontal") * 4, 0);
		Vector3 translation = new Vector3(0, 0, speed) * Time.deltaTime; 
		
		var now = System.DateTime.UtcNow;  
		
		queuedRotations.Enqueue(rotation);
		queuedTimestamps.Enqueue(now); 
		
		transform.Translate(translation); 
		
		int delayToPerform = 0;
		if (delayedResponse == true) {
			delayToPerform = delaySeconds; 
		}
		
		while(queuedTimestamps.Count > 0 && 
			(delayedResponse == false || (now - queuedTimestamps.Peek()).Seconds > delayToPerform)) {
			transform.Rotate(queuedRotations.Dequeue()); 
			queuedTimestamps.Dequeue(); 
		}
		
		
		if (randomDropper == true) {
			secondsSinceLastDrop += Time.deltaTime; 
			if(IS_CURRENTLY_DROPPING == false) {
				if (secondsSinceLastDrop > minSecondsBetweenDrops) {
					IS_CURRENTLY_DROPPING = true; 
					secondsSinceLastDrop = 0;
				}
			}
			
			if (IS_CURRENTLY_DROPPING == true){
				if (secondsSinceLastDrop < dropDuration) {
					transform.Translate (new Vector3(0, dropHeight, 0) * Time.deltaTime);
				}else {
					IS_CURRENTLY_DROPPING = false; 
				}
			}
		}
		
		
	}
	
}
