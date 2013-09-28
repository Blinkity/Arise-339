using UnityEngine;
using System.Collections;

public class Transformer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		 
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Input.GetAxis("Vertical") * 4, Input.GetAxis("Horizontal") * 4, 0);  
		transform.Translate( new Vector3(0, 0, 10) * Time.deltaTime); 
	}
	
}
