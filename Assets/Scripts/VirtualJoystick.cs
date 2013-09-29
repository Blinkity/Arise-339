using UnityEngine;
using System.Collections;

public class VirtualJoystick : MonoBehaviour {

	public bool isKeyboard;
	public float horizAxis;
	public float vertAxis;
	public float rollAxis;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public float GetAxis (string axisName) {
		if (isKeyboard) {
			return Input.GetAxis(axisName);
		}
		
		if (axisName == "Horizontal") {
			return horizAxis;
		} else if (axisName == "Vertical") {
			return vertAxis;
		} else if (axisName == "Roll") {
			return rollAxis;
		} else {
			throw new System.InvalidOperationException("Only supports horizontal, vertical, and roll axes.");
		}
	}
	
	public void SetAxis(string axisName, float val) {
		if (isKeyboard) {
			throw new System.InvalidOperationException("Cannot set axis for keyboard input.");
		}
		
		if (axisName == "Horizontal") {
			horizAxis = val;
		} else if (axisName == "Vertical") {
			vertAxis = val;
		} else if (axisName == "Roll") {
			rollAxis = val;
		} else {
			throw new System.InvalidOperationException("Only supports horizontal, vertical, and roll axes.");
		}
		
	}
}
