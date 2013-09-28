﻿using UnityEngine;
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
	
	public float GetAxis (string axisName) {
		if (isKeyboard) {
			return Input.GetAxis(axisName);
		}
		
		if (axisName == "Horizontal") {
			return horizAxis;
		} else if (axisName == "Vertical") {
			return vertAxis;
		} else {
			throw new System.InvalidOperationException("Only supports horizontal and vertical axes.");
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
		} else {
			throw new System.InvalidOperationException("Only supports horizontal and vertical axes.");
		}
		
	}
}
