using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum State {LeftCircle, RightCircle, Straight};
	
public class Pilot2 : MonoBehaviour {

	public VirtualJoystick joystick;
	private State curState;
	private float timeInState;
	
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
		timeInState = 0;
		curState = newState;
		enterFunctions[newState]();
	}
	
	
	void initializeStateFunctions() {
		enterFunctions.Add(State.LeftCircle, () => {
			left();
		});
		
		updateFunctions.Add(State.LeftCircle, () => {
			if (timeInState >= 4) {
				switchState(State.RightCircle);
			}
		});
		
		
		enterFunctions.Add(State.RightCircle, () => {
			right();
		});
		
		updateFunctions.Add(State.RightCircle, () => {
			if (timeInState >= 4) {
				switchState(State.Straight);
			}
		});
		
		
		enterFunctions.Add(State.Straight, () => {
			horizStraight();
		});
		
		updateFunctions.Add(State.Straight, () => {
			if (timeInState >= 5) {
				switchState(State.LeftCircle);
			}
		});
	}
	
	
	void sanityCheckStateFunctions() {
		if (updateFunctions.Keys.count != Enum.GetNames(typeof(State)).Length) {
			throw new System.InvalidOperationException("Wrong number of state update functions.");
		}
		
		if (enterFunctions.Keys.count != Enum.GetNames(typeof(State)).Length) {
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
		joystick.SetAxis("Vertical",1);	
	}

	void down() {
		joystick.SetAxis("Vertical",-1);	
	}
	
	void vertStraight() {
		joystick.SetAxis("Vertical",0);	
	}
}
