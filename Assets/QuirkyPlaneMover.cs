using UnityEngine;
using System.Collections;
using System; 
using System.Collections.Generic; 

public class QuirkyPlaneMover : MonoBehaviour {
	public VirtualJoystick virtualJoystick; 

	public void InitializeStats() {
		speed = SharedVariables.speed; //20.0F;
		horizontalRotationSpeed = SharedVariables.horizontalRotationSpeed;
		verticalRotationSpeed = SharedVariables.verticalRotationSpeed;
		rollRotationSpeed = SharedVariables.rollRotationSpeed;
		jerkyFactor = SharedVariables.jerkyFactor; 
		minSecondsBetweenJerks = SharedVariables.minSecondsBetweenJerks;
		jerkDuration = SharedVariables.jerkDuration; 
		minSecondsBetweenDrops = SharedVariables.minSecondsBetweenDrops;
		dropHeight = SharedVariables.dropHeight;
		dropDuration = SharedVariables.dropDuration; 
	}

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


	static int nextPlaneColor = 0;

	//jerky properties
	public float jerkyFactor; 
	public int minSecondsBetweenJerks;
	public float jerkDuration; 
	bool isCurrentlyJerking = false; 
	double secondsSinceLastJerk; 
	
	//delayedResponse properties
	public int delaySeconds = SharedVariables.delaySeconds; 
	Queue<Vector3> queuedRotations; 
	Queue<Vector3> queuedTranslations; 
	Queue<DateTime> queuedTimestamps; 
	
	//dropper properties
	long minSecondsBetweenDrops; 
	int dropHeight;
	double dropDuration; 
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
		InitializeStats();
		SetColor ();
		
		//speed = 20.0F;
		//horizontalRotationSpeed = 90.0F;
		//verticalRotationSpeed = 60.0F;
		
		queuedRotations = new Queue<Vector3>(); 
		queuedTranslations = new Queue<Vector3>(); 
		queuedTimestamps = new Queue<DateTime>(); 
		
		//knownToBeDelayedResponse = weightedCoinFlip(0.25f);
		//this is too hard to see, so we're not using delayed response for now
		knownToBeDelayedResponse = false;
		
		knownToBeJerker = weightedCoinFlip(SharedVariables.knownToBeJerkerProb);
		knownToBeDropper = weightedCoinFlip(SharedVariables.knownToBeDropperProb);
		
		//with 80% chance
		if (weightedCoinFlip(SharedVariables.surpriseQuirkProb)) {
			//pick a surprise quirk
			float x = UnityEngine.Random.Range(0f,1f); 
			if (x < SharedVariables.surpriseDelayedResponseProbGivenProblem) {
				surpriseDelayedResponse = true;
			} else if (x < SharedVariables.surpriseDelayedResponseProbGivenProblem + SharedVariables.surpriseJerkerProbGivenProblem) {
				surpriseJerker = true;
			} else {
				surpriseDropper = true; 
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		float speedForThisUpdate = speed; 
		
		if (isJerker()) {
			secondsSinceLastJerk += Time.deltaTime; 
			if(!isCurrentlyJerking) {
				if (secondsSinceLastJerk > minSecondsBetweenJerks) {
					isCurrentlyJerking = true; 
					secondsSinceLastJerk = 0;
				}
			}
			
			if (isCurrentlyJerking){
				if (secondsSinceLastJerk < jerkDuration) {
					speedForThisUpdate *= jerkyFactor; 
				} else {
					isCurrentlyJerking = false; 
				}
			}
		}
		

		Vector3 rotation = new Vector3(virtualJoystick.GetAxis("Vertical") * verticalRotationSpeed, virtualJoystick.GetAxis("Horizontal") * horizontalRotationSpeed, virtualJoystick.GetAxis("Roll") * rollRotationSpeed) * Time.deltaTime;
		Vector3 translation = new Vector3(0, 0, speedForThisUpdate) * Time.deltaTime; 
		
		var now = System.DateTime.UtcNow;  
		
		queuedRotations.Enqueue(rotation);
		queuedTimestamps.Enqueue(now); 
		
		transform.Translate(translation); 
		if (transform.position.y - transform.lossyScale.y < 0) {
			transform.Translate(new Vector3(0,-(transform.position.y - transform.lossyScale.y),0), Space.World);
		}
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

	void SetColor() {
		(this.GetComponentInChildren<MeshRenderer>()).material.SetColor("_Color", SharedVariables.allColors[nextPlaneColor % SharedVariables.allColors.Count]);  
		nextPlaneColor += 1;
	}
	
	public bool hasAnySuprisingQuirks() {
		return 
			(knownToBeJerker == false && surpriseJerker == true) ||
				(knownToBeDropper == false && surpriseDropper == true) ||
				(knownToBeDelayedResponse == false && surpriseDelayedResponse == true); 	
	}
	
	
	//This is used when the plane starts heading home, so that quirks don't fuck it up.
	public void loseQuirks() {
		knownToBeJerker = false;	
		knownToBeDropper = false;
		knownToBeDelayedResponse = false;
		surpriseJerker = false;	
		surpriseDropper = false;
		surpriseDelayedResponse = false;
	}
	
}
