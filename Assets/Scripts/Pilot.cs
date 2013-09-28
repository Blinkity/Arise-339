using UnityEngine;
using System.Collections;



public class Pilot : MonoBehaviour {

	public VirtualJoystick joystick;
	private State curState;
	private float timeInState;
	
	public enum State {LeftCircle, RightCircle}
	
	// Use this for initialization
	void Start () {
		timeInState = 0;
		curState = State.LeftCircle;
	}
	
	// Update is called once per frame
	void Update () {
		switch (curState)
		{
			case State.LeftCircle:
				updateLeftCircle();
				break;
			case State.RightCircle:
				updateRightCircle();
				break;
			default:
				throw new System.InvalidOperationException("Unsupported state.");
		}
	}
		
	void updateLeftCircle() {
		if (timeInState >= 5000) {
			switchState(State.RightCircle);
			return;
		}
		
		//right();
	}
	
	void updateRightCircle() {
		if (timeInState >= 5000) {
			switchState(State.LeftCircle);
			return;
		}
		
		//left();
	}
	
	void enterLeftCircle() {
		left();
	}
	
	void enterRightCircle() {
		right();
	}
			
	void switchState(State newState) {
		timeInState = 0;
		curState = newState;
		
		switch (newState)
		{
			case State.LeftCircle:
				enterLeftCircle();
				break;
			case State.RightCircle:
				enterRightCircle();
				break;
			default:
				throw new System.InvalidOperationException("Unsupported state.");
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
