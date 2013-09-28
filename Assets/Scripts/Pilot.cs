using UnityEngine;
using System.Collections;



public class Pilot : MonoBehaviour {

	public VirtualJoystick joystick;
	private State curState;
	private float timeInState;
	
	public enum State {LeftCircle, RightCircle, Straight}
	
	// Use this for initialization
	void Start () {
		Debug.Log("Any logging?");
		switchState(State.Straight);
	}
	
	// Update is called once per frame
	void Update () {
		timeInState += Time.deltaTime;
		
		switch (curState)
		{
			case State.Straight:
				updateStraight();
				break;
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
	
	void enterLeftCircle() {
		left();
	}
	
	void enterRightCircle() {
		right();
	}

		
	void updateLeftCircle() {
		if (timeInState >= 4) {
			switchState(State.RightCircle);
			return;
		}
		
	}
	
	void updateRightCircle() {
		if (timeInState >= 4) {
			switchState(State.Straight);
			return;
		}
		
	}
	
	void updateStraight() {
		if (timeInState >= 5) {
			switchState(State.LeftCircle);
			return;
		}
		
		//left();
	}
	

	
	
	void enterStraight() {
		Debug.Log("Entering straight.");
		horizStraight();
		//nothing
	}
			
	void switchState(State newState) {
		timeInState = 0;
		curState = newState;
		
		switch (newState)
		{
			case State.Straight:
				enterStraight();
				break;
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
