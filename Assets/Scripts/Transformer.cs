﻿using UnityEngine;
using System.Collections;

public class Transformer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		 
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Translate( new Vector3(0, 0, 1)*Time.deltaTime*50); 
		transform.Rotate(Vector3.up*1);
	}
	
}
