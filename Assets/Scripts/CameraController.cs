using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject player; 
	public GameObject airplane; 
	private Vector3 offset; 
	
	// Use this for initialization
	void Start () {
		//offset = airplane.transform.position - transform.position;  
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//transform.position = airplane.transform.position; 
		//transform.rotation = airplane.transform.rotation; 
	}
}
