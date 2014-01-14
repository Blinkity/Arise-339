using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum State {LeftCircle, RightCircle, Straight, FigureEight1, FigureEight2, FigureEight3, FigureEight4, FigureEight5, FigureEight6, Dive1, Dive2, Dive3, Climb1, Climb2, Climb3, BarrelRoll, ReturnHome, GoToBase1, GoToBase2, GoToBase3, GoToBase4, GoToBase5, Landed};


public class Pilot : MonoBehaviour {
	public Camera girlCamera; 
	
	public VirtualJoystick joystick;
	public State curState;
	public float distance; 
	public float baseDistanceAsTheBirdFlies; 
	private State stateAfterReturnHome; 
	public double vertDegrees;
	public Vector3 curPosition;
	public Vector3 prevPosition;
	public float verticalSpeed;
	
	private List<State> initialStates;
	private List<State> initialStateDistribution;
	
	public float timeInState;
	public float timeExisting;
	public QuirkyPlaneMover plane;
	public double baseDist;
	public double groundDist;
	
	public Dictionary<State,System.Action> updateFunctions = new Dictionary<State,System.Action>();
	public Dictionary<State,System.Action> enterFunctions = new Dictionary<State,System.Action>();
	
	private delegate void updateStateDelegate();
	
	public Vector3 relativeEulers;
	public Vector3 planeEulers;

	public float timeToBase;
	public float timeToGround;
	public float rotationTimeToBase;
	
	// Use this for initialization
	void Start () {
		timeExisting = 0;
		initializeStateFunctions();
		sanityCheckStateFunctions();
		
		switchState(State.Straight);
	}
	
	// Update is called once per frame
	void Update () {
		timeInState += Time.deltaTime;
		timeExisting += Time.deltaTime;

		updateFunctions[curState]();

		prevPosition = curPosition;
		curPosition = plane.transform.position;
		verticalSpeed = (curPosition.y - prevPosition.y )  / Time.deltaTime;

		distance = Vector3.Distance(plane.transform.position, girlCamera.transform.position); 
		baseDist = Vector3.Distance(plane.transform.position, SharedVariables.landingLocation()); 
		//groundDist = Vector3.Distance(plane.transform.position, new Vector3(, 0, ); 
		Quaternion planeRotation = plane.transform.rotation;
		planeEulers = planeRotation.eulerAngles;
		baseDistanceAsTheBirdFlies = Vector3.Distance (plane.transform.position, 
		                                           new Vector3((SharedVariables.landingLocation()).x, plane.transform.position.y, (SharedVariables.landingLocation()).z)); 

	}
			
	void switchState(State newState) {
		//if too far from origin, and entering an initial state.
		if (curState != State.GoToBase1 && curState != State.GoToBase2 && curState != State.GoToBase3 && curState != State.GoToBase4 && curState != State.GoToBase5
			&& newState!= State.ReturnHome && curState!= State.ReturnHome && distance > SharedVariables.maxDistanceFromHome && initialStates.Contains(newState)) {
			stateAfterReturnHome = newState; 
			//turn toward home 
			switchState(State.ReturnHome); 
			return; 
		}
		
		if (curState != State.GoToBase1 && curState != State.GoToBase2 && curState != State.GoToBase3 && curState != State.GoToBase4 && curState != State.GoToBase5
				&& newState != State.GoToBase1
				&& timeExisting > SharedVariables.brokenTimeToLand
				&& initialStates.Contains(newState)
		  		&& plane.hasAnySuprisingQuirks()
		    	&& baseDistanceAsTheBirdFlies > SharedVariables.minGoToBaseDistance) {
			switchState(State.GoToBase1);
			return;
		}
		
		//Debug.Log("Switch state." + newState);
		timeInState = 0;
		curState = newState;
		enterFunctions[newState]();
	}

	void randomManeuver() {
		State randomState = initialStateDistribution[UnityEngine.Random.Range(0,initialStateDistribution.Count)];
		//keep picking a new random state while a 'disallowed' state is chosen
		while (isStateDisallowed(randomState)) 
		{
			randomState = initialStateDistribution[UnityEngine.Random.Range(0,initialStateDistribution.Count)];			
		}
		
		switchState(randomState);
	}

	bool isStateDisallowed(State state) {
		return (plane.knownToBeDropper && (state == State.Dive1 || state == State.BarrelRoll))
			|| (plane.knownToBeDelayedResponse && (state == State.Dive1 || state == State.LeftCircle || state == State.RightCircle))
			|| (plane.knownToBeJerker && (state == State.FigureEight1 || state == State.Dive1));
	}

	void initializeStateFunctions() {
		initialStates = new List<State>();
		initialStates.Add(State.LeftCircle);
		initialStates.Add(State.RightCircle);
		initialStates.Add(State.Straight);
		initialStates.Add(State.FigureEight1);
		initialStates.Add(State.Dive1);
		initialStates.Add(State.Climb1);
		initialStates.Add(State.BarrelRoll);
		
		
		initialStateDistribution = new List<State>();
		initialStateDistribution.Add(State.LeftCircle);
		initialStateDistribution.Add(State.RightCircle);
		initialStateDistribution.Add(State.Straight);
		initialStateDistribution.Add(State.Straight);
		initialStateDistribution.Add(State.Straight);
		initialStateDistribution.Add(State.Straight);
		initialStateDistribution.Add(State.Straight);
		initialStateDistribution.Add(State.Straight);
		initialStateDistribution.Add(State.Straight);
		initialStateDistribution.Add(State.Straight);
		initialStateDistribution.Add(State.FigureEight1);
		initialStateDistribution.Add(State.FigureEight1);
		initialStateDistribution.Add(State.Dive1);
		initialStateDistribution.Add(State.Dive1);
		initialStateDistribution.Add(State.Climb1);
		initialStateDistribution.Add(State.Climb1);
		initialStateDistribution.Add(State.BarrelRoll);
		initialStateDistribution.Add(State.BarrelRoll);
		initialStateDistribution.Add(State.BarrelRoll);
		
		/*
		initialStateDistribution = new List<State>();
		initialStateDistribution.Add(State.Straight);
		initialStateDistribution.Add(State.Dive1);
		initialStateDistribution.Add(State.Climb1);
		*/
		
		//left circle
		enterFunctions.Add(State.LeftCircle, () => {
			resetControls();
			left();
		});
		
		updateFunctions.Add(State.LeftCircle, () => {
			if (timeInState >= 4) {
				randomManeuver();
				//switchState(State.RightCircle);
			}
		});
		
		
		//right circle
		enterFunctions.Add(State.RightCircle, () => {
			resetControls();
			right();
		});
		
		updateFunctions.Add(State.RightCircle, () => {
			if (timeInState >= 4) {
				randomManeuver();
				//switchState(State.Straight);
			}
		});
		
		
		//straight
		enterFunctions.Add(State.Straight, () => {
			resetControls();
		});
		
		updateFunctions.Add(State.Straight, () => {
			if (timeInState >= 2.5) {
				randomManeuver();
				//switchState(State.BarrelRoll);
			}
		});
		
		
		//figure eight

		
		enterFunctions.Add(State.FigureEight1, () => {
			resetControls();
		});
		
		updateFunctions.Add(State.FigureEight1, () => {
			if (timeInState >= 0.25) {
				switchState(State.FigureEight2);
			}
		});
		
		
		enterFunctions.Add(State.FigureEight2, () => {
			right();
		});
		
		updateFunctions.Add(State.FigureEight2, () => {
			if (timeInState >= 30F/plane.horizontalRotationSpeed) {
				switchState(State.FigureEight3);
			}
		});
		
		enterFunctions.Add(State.FigureEight3, () => {
			left();
		});
		
		
		updateFunctions.Add(State.FigureEight3, () => {
			if (timeInState >= 270F/plane.horizontalRotationSpeed) {
				switchState(State.FigureEight4);
			}
		});
		
		enterFunctions.Add(State.FigureEight4, () => {
			resetControls();
		});
		
		updateFunctions.Add(State.FigureEight4, () => {
			if (timeInState >= 0.25) {
				switchState(State.FigureEight5);
			}
		});
		
		enterFunctions.Add(State.FigureEight5, () => {
			right();
		});
		
		updateFunctions.Add(State.FigureEight5, () => {
			if (timeInState >= 270F/plane.horizontalRotationSpeed) {
				switchState(State.FigureEight6);

			}
		});
		
		enterFunctions.Add(State.FigureEight6, () => {
			resetControls();
		});
		
		updateFunctions.Add(State.FigureEight6, () => {
			if (timeInState >= 0.25) {
				randomManeuver();
				//switchState(State.Straight);
			}
		});
		
		
		//Dive
		enterFunctions.Add(State.Dive1, () => {
			resetControls();
			down();
		});
		
		updateFunctions.Add(State.Dive1, () => {
			if (timeInState >= 30F/plane.verticalRotationSpeed) {
				switchState(State.Dive2);
			}
		});
		
		enterFunctions.Add(State.Dive2, () => {
			vertStraight();
		});
		
		updateFunctions.Add(State.Dive2, () => {
			if (timeInState >= 1.5) {
				switchState(State.Dive3);
			}
		});
		
		enterFunctions.Add(State.Dive3, () => {
			up();
		});
		
		updateFunctions.Add(State.Dive3, () => {
			if (timeInState >= 30F/plane.horizontalRotationSpeed) {
				randomManeuver();
				//switchState(State.Straight);
			}
		});
		
		
		//Climb
		enterFunctions.Add(State.Climb1, () => {
			resetControls();
			up();
		});
		
		updateFunctions.Add(State.Climb1, () => {
			if (timeInState >= 30F/plane.verticalRotationSpeed) {
				switchState(State.Climb2);
			}
		});
		
		enterFunctions.Add(State.Climb2, () => {
			vertStraight();
		});
		
		updateFunctions.Add(State.Climb2, () => {
			if (timeInState >= 2) {
				switchState(State.Climb3);
			}
		});
		
		enterFunctions.Add(State.Climb3, () => {
			down();
		});
		
		updateFunctions.Add(State.Climb3, () => {
			if (timeInState >= 30F/plane.horizontalRotationSpeed) {
				randomManeuver();
				//switchState(State.Straight);
			}
		});
		
		
		//barrel roll
		enterFunctions.Add(State.BarrelRoll, () => {
			resetControls();
			rollLeft();
		});
		
		updateFunctions.Add(State.BarrelRoll, () => {
			if (timeInState >= 360F/plane.rollRotationSpeed) {
				randomManeuver();
				//switchState(State.Straight);
			}
		});
		
		
		//return home
		enterFunctions.Add(State.ReturnHome, () => {
			resetControls();
			origKnownToBeDelayedResponse = plane.knownToBeDelayedResponse;
			origSurpriseDelayedResponse = plane.surpriseDelayedResponse;
			
			plane.knownToBeDelayedResponse = false;
			plane.surpriseDelayedResponse = false; 
			
			right();
		});
		
		updateFunctions.Add(State.ReturnHome, () => {
			Vector3 adjustedCameraPos = new Vector3(girlCamera.transform.position.x, 
				plane.transform.position.y, girlCamera.transform.position.z); 
			
			Vector3 relativePos = adjustedCameraPos - plane.transform.position;
			Quaternion relativeRotation = Quaternion.LookRotation(relativePos);
			relativeEulers = relativeRotation.eulerAngles;
			
			double degreesDiff = (relativeEulers.y - planeEulers.y);  
			if (degreesDiff > -20 && degreesDiff < 20) {
				plane.knownToBeDelayedResponse = origKnownToBeDelayedResponse;
				plane.surpriseDelayedResponse = origSurpriseDelayedResponse; 
				switchState (stateAfterReturnHome);
			}
			
		});
		
		
		//Go to Base
		enterFunctions.Add(State.GoToBase1, () => {
			Debug.Log("Entered base1");
			plane.loseQuirks(); //landing is hard enough as it is, so let's remove quirks.
			resetControls();
			origKnownToBeDelayedResponse = plane.knownToBeDelayedResponse;
			origSurpriseDelayedResponse = plane.surpriseDelayedResponse;
			
			plane.knownToBeDelayedResponse = false;
			plane.surpriseDelayedResponse = false; 
			
			left();
		});
		
		updateFunctions.Add(State.GoToBase1, () => {
			Vector3 adjustedBasePos = new Vector3(SharedVariables.landingLocation().x, 
				plane.transform.position.y, SharedVariables.landingLocation().z); 
			
			Vector3 relativePos = adjustedBasePos - plane.transform.position;
			Quaternion relativeRotation = Quaternion.LookRotation(relativePos);
			relativeEulers = relativeRotation.eulerAngles;
			
			double degreesDiff = (relativeEulers.y - planeEulers.y);  
			if (degreesDiff > -5 && degreesDiff < 5) {
				switchState (State.GoToBase2);
			}
		});
		
		enterFunctions.Add(State.GoToBase2, () => {
			resetControls();
		});
		
		updateFunctions.Add(State.GoToBase2, () => {	
			if (timeInState > 0.5) {			
				switchState (State.GoToBase3);
			}
		});
		
		enterFunctions.Add(State.GoToBase3, () => {
			Debug.Log("Entered base3");
			resetControls();			
			down();
			
			double vertRadians = System.Math.Asin(plane.transform.position.y/baseDist);
			vertDegrees = vertRadians * 180.0/System.Math.PI;
		});
		
		updateFunctions.Add(State.GoToBase3, () => {
			if (timeInState > vertDegrees / plane.verticalRotationSpeed) {
				switchState(State.GoToBase4);
			}
		
			//if (timeInState >= 30F/plane.verticalRotationSpeed) {
			//	switchState(State.GoToBase4);
			//}
		});
		
		
		enterFunctions.Add(State.GoToBase4, () => {
			resetControls();			
		});
		
		updateFunctions.Add(State.GoToBase4, () => {
			//check whether to start turning up
			/*
			timeToBase = (float) baseDist / plane.speed;
			rotationTimeToBase = (float) vertDegrees / plane.verticalRotationSpeed;
			//if (plane.transform.position.y < SharedVariables.heightAtWhichToRotateToLandingMode) {
			//If the time to hit the base (assuming no rotation for approximation) is equal to the time to rotate to vertical
			if (timeToBase <= rotationTimeToBase) {
				//start rotating to vertical
				switchState(State.GoToBase5);
			}
			*/

			timeToGround = (float) -curPosition.y / verticalSpeed;
			rotationTimeToBase = (float) vertDegrees / plane.verticalRotationSpeed;
			if (timeToGround <= rotationTimeToBase) {
				switchState (State.GoToBase5);
			}
		});
		
		enterFunctions.Add(State.GoToBase5, () => {
			resetControls();
			up();
		});
		
		updateFunctions.Add(State.GoToBase5, () => {	
			if (timeInState >= vertDegrees / plane.verticalRotationSpeed) {
				resetControls();
				plane.speed = plane.speed * (1 - Time.deltaTime); 
				
				if (plane.speed < 0.02){
					switchState(State.Landed); 
				}
			}
		});
		
		enterFunctions.Add(State.Landed, () => {
			resetControls();
			plane.speed = 0; 
			plane.audio.mute = true; 
		});
		
		updateFunctions.Add(State.Landed, () => {	
			//do nothing 
		});
		

	}
	
	bool origKnownToBeDelayedResponse;
	bool origSurpriseDelayedResponse; 
	
	void resetControls() {
		vertStraight();
		horizStraight();
		rollStraight();
	}
	
	
	void sanityCheckStateFunctions() {
		if (updateFunctions.Keys.Count != System.Enum.GetNames(typeof(State)).Length) {
			throw new System.InvalidOperationException("Wrong number of state update functions.");
		}
		
		if (enterFunctions.Keys.Count != System.Enum.GetNames(typeof(State)).Length) {
			throw new System.InvalidOperationException("Wrong number of state enter functions.");
		}
	}
	
	
	void left() {
		joystick.SetAxis("Horizontal",-1);	
	}
	
	void right() {
		joystick.SetAxis("Horizontal",1);	
	}
	
	void horizStraight() {
		joystick.SetAxis("Horizontal",0);	
	}
	
	void up() {
		joystick.SetAxis("Vertical",-1);	
	}

	void down() {
		joystick.SetAxis("Vertical",1);	
	}
	
	void vertStraight() {
		joystick.SetAxis("Vertical",0);	
	}
	
	void rollLeft() {
		joystick.SetAxis("Roll",-1);	
	}

	void rollRight() {
		joystick.SetAxis("Roll",1);	
	}
	
	void rollStraight() {
		joystick.SetAxis("Roll",0);	
	}
}
