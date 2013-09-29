using UnityEngine;
using System.Collections;
using System; 
using System.Collections.Generic; 

public class QuirkyPlaneMover : MonoBehaviour {
	public VirtualJoystick virtualJoystick; 
	public int speed = 10; 
	
	//known quirks
	public bool knownToBeDelayedResponse; 
	public bool knownToBeDropper; 
	public bool knownToBeJerker; 
	
	//suprise quirks 
	public bool surpriseDelayedResponse; 
	public bool surpriseDropper;
	public bool surpriseJerker; 
		
	//jerky properties
	float jerkyFactor = 5f; 
	int minSecondsBetweenJerks = 3;
	int jerkDuration = 2; 
	bool isCurrentlyJerking = false; 
	double secondsSinceLastJerk; 
	
	//delayedResponse properties
	int delaySeconds = 2; 
	Queue<Vector3> queuedRotations; 
	Queue<Vector3> queuedTranslations; 
	Queue<DateTime> queuedTimestamps; 
	
	//dropper properties
	long minSecondsBetweenDrops = 5; 
	int dropHeight = -30;
	double dropDuration = 1; 
	bool isCurrentlyDropping = false; 
	double secondsSinceLastDrop; 
	
	private bool isDropper() {
		return knownToBeDropper || surpriseDropper; 
	}
	
	private bool isDelayedResponse() {
		return knownToBeDelayedResponse || surpriseDelayedResponse; 
	}
	
	private bool isJerker() {
		return knownToBeJerker || surpriseJerker; 
	}
	
	
	// Use this for initialization
	void Start () {
		queuedRotations = new Queue<Vector3>(); 
		queuedTranslations = new Queue<Vector3>(); 
		queuedTimestamps = new Queue<DateTime>(); 
		
		surpriseDelayedResponse = (UnityEngine.Random.Range(0,100) % 2 == 0);
		surpriseJerker = (UnityEngine.Random.Range(0,100) % 2 == 0);
		surpriseDropper = (UnityEngine.Random.Range(0,100) % 2 == 0);
		
		
	}
	
	// Update is called once per frame
	void Update () {
		float speedForThisUpdate = speed; 
		
		if (isJerker() == true) {
			secondsSinceLastJerk += Time.deltaTime; 
			if(isCurrentlyJerking == false) {
				if (secondsSinceLastJerk > minSecondsBetweenJerks) {
					isCurrentlyJerking = true; 
					secondsSinceLastJerk = 0;
				}
			}
			
			if (isCurrentlyJerking == true){
				if (secondsSinceLastJerk < jerkDuration) {
					speedForThisUpdate *= jerkyFactor; 
				} else {
					isCurrentlyJerking = false; 
				}
			}
		}
		
		Vector3 rotation = new Vector3(virtualJoystick.GetAxis("Vertical") * 4, virtualJoystick.GetAxis("Horizontal") * 4, 0);
		Vector3 translation = new Vector3(0, 0, speedForThisUpdate) * Time.deltaTime; 
		
		var now = System.DateTime.UtcNow;  
		
		queuedRotations.Enqueue(rotation);
		queuedTimestamps.Enqueue(now); 
		
		transform.Translate(translation); 
		
		int delayToPerform = 0;
		if (isDelayedResponse() == true) {
			delayToPerform = delaySeconds; 
		}
		
		while(queuedTimestamps.Count > 0 && 
			(knownToBeDelayedResponse == false || (now - queuedTimestamps.Peek()).Seconds > delayToPerform)) {
			transform.Rotate(queuedRotations.Dequeue()); 
			queuedTimestamps.Dequeue(); 
		}
		
		
		if (isDropper() == true) {
			secondsSinceLastDrop += Time.deltaTime; 
			if(isCurrentlyDropping == false) {
				if (secondsSinceLastDrop > minSecondsBetweenDrops) {
					isCurrentlyDropping = true; 
					secondsSinceLastDrop = 0;
				}
			}
			if (isCurrentlyDropping == true){
				if (secondsSinceLastDrop < dropDuration) {
					transform.Translate (new Vector3(0, dropHeight, 0) * Time.deltaTime);
				}else {
					isCurrentlyDropping = false; 
				}
			}
		}
	}
	
}
