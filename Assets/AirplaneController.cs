using UnityEngine;
using System.Collections;

public class AirplaneController : MonoBehaviour {
	public float speed;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//float moveHorizontal = Input.GetAxis("Horizontal");
		//float moveVertical = Input.GetAxis("Vertical");
	
		
		if(Input.GetButton("A")) { 
			transform.Rotate(0, 90, 0);  
		}
			
		//Vector3 movement = new Vector3(moveHorizontal,0, moveVertical);
		//rigidbody.AddForce(movement * speed * Time.deltaTime);
	
	}
}
