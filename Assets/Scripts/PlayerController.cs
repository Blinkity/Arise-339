using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public int count; 
	public GUIText countText; 
	public GUIText winText; 
	
	void Start () {
		count = 0; 
		setCountText ();  
		winText.text = ""; 
	}
	
	void setCountText () {
		countText.text = "Count: " + count.ToString();	
	}
	
	//physics code
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
	
		Vector3 movement = new Vector3(moveHorizontal,0, moveVertical);
		rigidbody.AddForce(movement * speed * Time.deltaTime);
	}
	
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Pickup")) {
            other.gameObject.SetActive(false);
			count = count + 1; 
			setCountText (); 
			if (count >= 12) {
				winText.text = "You win!"; 
			}
		}
			
    }
}

