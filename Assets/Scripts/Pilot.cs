using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum State {LeftCircle, RightCircle, Straight, FigureEight1, FigureEight2, FigureEight3, FigureEight4, Dive1, Dive2, Dive3, Climb1, Climb2, Climb3, BarrelRoll};
	
public class Pilot : MonoBehaviour {

	public VirtualJoystick joystick;
	private State curState;
	private float timeInState;
	public QuirkyPlaneMover plane;
	
	public Dictionary<State,System.Action> updateFunctions = new Dictionary<State,System.Action>();
	public Dictionary<State,System.Action> enterFunctions = new Dictionary<State,System.Action>();
	
	private delegate void updateStateDelegate();
	
	
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
	}
			
	void switchState(State newState) {
		//Debug.Log("Switch state." + newState);
		timeInState = 0;
		curState = newState;
		enterFunctions[newState]();
	}
	
	
	void initializeStateFunctions() {
		
		
		//left circle
		enterFunctions.Add(State.LeftCircle, () => {
			resetControls();
			left();
		});
		
		updateFunctions.Add(State.LeftCircle, () => {
			if (timeInState >= 4) {
				switchState(State.RightCircle);
			}
		});
		
		
		//right circle
		enterFunctions.Add(State.RightCircle, () => {
			resetControls();
			right();
		});
		
		updateFunctions.Add(State.RightCircle, () => {
			if (timeInState >= 4) {
				switchState(State.Straight);
			}
		});
		
		
		//straight
		enterFunctions.Add(State.Straight, () => {
			resetControls();
		});
		
		updateFunctions.Add(State.Straight, () => {
			if (timeInState >= 2.5) {
				switchState(State.BarrelRoll);
			}
		});
		
		
		//figure eight
		enterFunctions.Add(State.FigureEight1, () => {
			resetControls();
		});
		
		updateFunctions.Add(State.FigureEight1, () => {
			if (timeInState >= 1) {
				switchState(State.FigureEight2);
			}
		});
		
		enterFunctions.Add(State.FigureEight2, () => {
			left();
		});
		
		updateFunctions.Add(State.FigureEight2, () => {
			if (timeInState >= 235F/plane.horizontalRotationSpeed) {
				switchState(State.FigureEight3);
			}
		});
		
		enterFunctions.Add(State.FigureEight3, () => {
			horizStraight();
			vertStraight();
		});
		
		updateFunctions.Add(State.FigureEight3, () => {
			if (timeInState >= 1) {
				switchState(State.FigureEight4);
			}
		});
		
		enterFunctions.Add(State.FigureEight4, () => {
			right();
		});
		
		updateFunctions.Add(State.FigureEight4, () => {
			if (timeInState >= 235F/plane.horizontalRotationSpeed) {
				switchState(State.Straight);
			}
		});
		
		
		//Dive
		enterFunctions.Add(State.Dive1, () => {
			resetControls();
			down();
		});
		
		updateFunctions.Add(State.Dive1, () => {
			if (timeInState >= 45F/plane.verticalRotationSpeed) {
				switchState(State.Dive2);
			}
		});
		
		enterFunctions.Add(State.Dive2, () => {
			vertStraight();
		});
		
		updateFunctions.Add(State.Dive2, () => {
			if (timeInState >= 3) {
				switchState(State.Dive3);
			}
		});
		
		enterFunctions.Add(State.Dive3, () => {
			up();
		});
		
		updateFunctions.Add(State.Dive3, () => {
			if (timeInState >= 45F/plane.horizontalRotationSpeed) {
				switchState(State.Straight);
			}
		});
		
		
		//Climb
		enterFunctions.Add(State.Climb1, () => {
			resetControls();
			up();
		});
		
		updateFunctions.Add(State.Climb1, () => {
			if (timeInState >= 45F/plane.verticalRotationSpeed) {
				switchState(State.Climb2);
			}
		});
		
		enterFunctions.Add(State.Climb2, () => {
			vertStraight();
		});
		
		updateFunctions.Add(State.Climb2, () => {
			if (timeInState >= 3) {
				switchState(State.Climb3);
			}
		});
		
		enterFunctions.Add(State.Climb3, () => {
			down();
		});
		
		updateFunctions.Add(State.Climb3, () => {
			if (timeInState >= 45F/plane.horizontalRotationSpeed) {
				switchState(State.Straight);
			}
		});
		
		
		//barrel roll
		enterFunctions.Add(State.BarrelRoll, () => {
			resetControls();
			rollLeft();
		});
		
		updateFunctions.Add(State.BarrelRoll, () => {
			if (timeInState >= 360F/plane.rollRotationSpeed) {
				switchState(State.Straight);
			}
		});
	}
	
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
