using UnityEngine;
using System.Collections;

public class VirtualJoystick : MonoBehaviour {

	public bool isKeyboard;
	private float horizAxis;
	private float vertAxis;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public float GetAxis (String axisName) {
		if (isKeyboard) {
			return Input.GetAxis(axisName);
		}
		
		if (axisName == "horizontal") {
			return horizAxis;
		} else if (axisName = "vertical") {
			return vertAxis;
		} else {
			throw new System.InvalidOperationException("Only supports horizontal and vertical axes.");
		}
	}
	
	public float SetAxis(String axisName, float val) {
		if (isKeyboard) {
			throw new System.InvalidOperationException("Cannot set axis for keyboard input.");
		}
		
		if (axisName == "horizontal") {
			horizAxis = val;
		} else if (axisName = "vertical") {
			vertAxis = val;
		} else {
			throw new System.InvalidOperationException("Only supports horizontal and vertical axes.");
		}
		
	}
}
