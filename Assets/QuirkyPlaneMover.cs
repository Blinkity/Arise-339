using UnityEngine;
using System.Collections;
using System; 
using System.Collections.Generic; 

public class QuirkyPlaneMover : MonoBehaviour {
	public VirtualJoystick virtualJoystick; 
	
	public float speed; //20.0F;
	public float horizontalRotationSpeed;
	public float verticalRotationSpeed;
	public float rollRotationSpeed;
	
	//known quirks
	public bool knownToBeDelayedResponse; 
	public bool knownToBeDropper; 
	public bool knownToBeJerker; 
	
	//suprise quirks 
	public bool surpriseDelayedResponse; 
	public bool surpriseDropper;
	public bool surpriseJerker; 
		
	//jerky properties
	float jerkyFactor = 10f; 
	int minSecondsBetweenJerks = 3;
	int jerkDuration = 1; 
	bool isCurrentlyJerking = false; 
	double secondsSinceLastJerk; 
	
	//delayedResponse properties
	public int delaySeconds = 2; 
	Queue<Vector3> queuedRotations; 
	Queue<Vector3> queuedTranslations; 
	Queue<DateTime> queuedTimestamps; 
	
	//dropper properties
	long minSecondsBetweenDrops = 5; 
	int dropHeight = -15;
	double dropDuration = 1; 
	public bool isCurrentlyDropping = false; 
	double secondsSinceLastDrop; 
	
	public bool isDropper() {
		return knownToBeDropper || surpriseDropper; 
	}
	
	public bool isDelayedResponse() {
		return knownToBeDelayedResponse || surpriseDelayedResponse; 
	}
	
	public bool isJerker() {
		return knownToBeJerker || surpriseJerker; 
	}
	
	
	public static bool weightedCoinFlip(float headsProbability) {
		return (UnityEngine.Random.Range(0f, 1f) < headsProbability); 
	}
	
	// Use this for initialization
	void Start () {
		//speed = 20.0F;
		//horizontalRotationSpeed = 90.0F;
		//verticalRotationSpeed = 60.0F;
		
		queuedRotations = new Queue<Vector3>(); 
		queuedTranslations = new Queue<Vector3>(); 
		queuedTimestamps = new Queue<DateTime>(); 
		
		knownToBeDelayedResponse = weightedCoinFlip(0.25f);
		knownToBeJerker = weightedCoinFlip(0.25f);
		knownToBeDropper = weightedCoinFlip(0.25f);
		
		//with 80% chance
		if (weightedCoinFlip(0.25f)) {
			//pick a surprise quirk
			float x = UnityEngine.Random.Range(0f,1f); 
			if (x < 0.33) {
				surpriseDelayedResponse = true;
			}else if (x < 0.66) {
				surpriseJerker = true;
			}else {
				surpriseDropper = true; 
			}
		}
		
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
		

		Vector3 rotation = new Vector3(virtualJoystick.GetAxis("Vertical") * verticalRotationSpeed, virtualJoystick.GetAxis("Horizontal") * horizontalRotationSpeed, virtualJoystick.GetAxis("Roll") * rollRotationSpeed) * Time.deltaTime;
		Vector3 translation = new Vector3(0, 0, speed) * Time.deltaTime; 
		
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
					//transform.position.Set(transform.position.x, transform.position.y - (dropHeight * Time.deltaTime), transform.position.z);  
					transform.Translate (new Vector3(0, dropHeight, 0) * Time.deltaTime, Space.World); 
					
				}else {
					isCurrentlyDropping = false; 
				}
			}
		}
	}
	
	public bool hasAnySuprisingQuirks() {
		return 
			(knownToBeJerker == false && surpriseJerker == true) ||
				(knownToBeDropper == false && surpriseDropper == true) ||
				(knownToBeDelayedResponse == false && surpriseDelayedResponse == true); 
		
		
	}
	
}
