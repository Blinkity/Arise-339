using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum State {LeftCircle, RightCircle, Straight, FigureEight1, FigureEight2, FigureEight3, FigureEight4, FigureEight5, FigureEight6, Dive1, Dive2, Dive3, Climb1, Climb2, Climb3, BarrelRoll, ReturnHome};



public class Pilot : MonoBehaviour {
	public Camera girlCamera; 
	
	public VirtualJoystick joystick;
	public State curState;
	public float distance; 
	private State stateAfterReturnHome; 
	
	private List<State> initialStates;
	private List<State> initialStateDistribution;
	
	public float timeInState;
	public QuirkyPlaneMover plane;
	
	public Dictionary<State,System.Action> updateFunctions = new Dictionary<State,System.Action>();
	public Dictionary<State,System.Action> enterFunctions = new Dictionary<State,System.Action>();
	
	private delegate void updateStateDelegate();
	
	public string debugg; 
	public Vector3 relativeEulers;
	public Vector3 planeEulers;
	
	// Use this for initialization
	void Start () {
		initializeStateFunctions();
		sanityCheckStateFunctions();
		
		switchState(State.Straight);
	}
	
	// Update is called once per frame
	void Update () {
		timeInState += Time.deltaTime;
		updateFunctions[curState]();
		distance = Vector3.Distance(plane.transform.position, girlCamera.transform.position); 
		
		Quaternion planeRotation = plane.transform.rotation;
		planeEulers = planeRotation.eulerAngles;
		
	}
			
	void switchState(State newState) {
		//if too far from origin, and entering an initial state.
		if (newState!= State.ReturnHome && curState!= State.ReturnHome && distance > 150f && initialStates.Contains(newState)) {
			stateAfterReturnHome = newState; 
			//turn toward home 
			switchState(State.ReturnHome); 
			return; 
		}
		
		
		//Debug.Log("Switch state." + newState);
		timeInState = 0;
		curState = newState;
		enterFunctions[newState]();
	}
	
	
	void randomManeuver() {
		//Array values = Enum.GetValues(typeof(State));

		

		State randomState = initialStateDistribution[UnityEngine.Random.Range(0,initialStateDistribution.Count)];
		while (   (plane.knownToBeDropper && (randomState == State.Dive1 || randomState == State.BarrelRoll))
			   || (plane.knownToBeDelayedResponse && (randomState == State.Dive1 || randomState == State.LeftCircle || randomState == State.RightCircle))
			   || (plane.knownToBeJerker && (randomState == State.FigureEight1 || randomState == State.Dive1))) 
		{
			randomState = initialStateDistribution[UnityEngine.Random.Range(0,initialStateDistribution.Count)];			
		}
		
		switchState(randomState);
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
		initialStateDistribution.Add(State.FigureEight1);
		initialStateDistribution.Add(State.FigureEight1);
		initialStateDistribution.Add(State.Dive1);
		initialStateDistribution.Add(State.Dive1);
		initialStateDistribution.Add(State.Climb1);
		initialStateDistribution.Add(State.Climb1);
		initialStateDistribution.Add(State.BarrelRoll);
		initialStateDistribution.Add(State.BarrelRoll);
		
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
