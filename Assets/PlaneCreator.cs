using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaneCreator : MonoBehaviour {
	
	public List<GameObject> planes = new List<GameObject>(); 
	public GUIText gameStateText; 
	public GUIText scoreText; 
	public GUIText winLoseText; 
	
	public Camera girlCamera; 
	private OrigMover origMover; 
	
	public double score = 1000; 
	public double scoreFactor = 1.1; 
	public double secondsSinceStart = 0; 
	
	private double planeGenerationTime = 10; 
	public double secondsSinceLastGeneration; 
	
	
	public int planeStartDistance = 1000;
	public int planeStartingHeight = 400;
	
	public GameObject exampleOfQuirkyAirplane; 
	// Use this for initialization
	void Start () {
		secondsSinceLastGeneration = planeGenerationTime; 
		origMover = (OrigMover) girlCamera.GetComponent(typeof(OrigMover)); 
	}
	
	public int a = 0; public int b = 0; 
	
	private bool winLossDetermined = false; 
	
	// Update is called once per frame
	void Update () {
	
		secondsSinceLastGeneration += Time.deltaTime; 
		
		if (origMover.currentState == OrigMover.PersonState.WatchingAirplanes) {
			secondsSinceStart += Time.deltaTime; 
		}
		
		int numLanded = 0;
		int numFlying = 0; 
		int numFullyLanded = 0; 
		foreach (GameObject plane in planes) {
			Pilot pilot = (Pilot) plane.GetComponent(typeof(Pilot)); 
			if (pilot.curState == State.Landed){
				numFullyLanded++; 
			}
			
			if (pilot.curState == State.Landed || pilot.curState == State.GoToBase5) {
				numLanded++; 
			} else {
				numFlying++; 
			}
		}
		gameStateText.text = "landed: " + numLanded + " -- flying: " + numFlying; 
		
		if (numLanded > 0 && !winLossDetermined) {
			if (origMover.currentState == OrigMover.PersonState.AtBase) {
				winLoseText.text = "Phew! You made it before the plane landed. \n Your score is " + 
					((int) secondsSinceStart) + "."; 
			} else {
				winLoseText.text = "The plane landed, and you weren't there. \n YOU'RE FIRED!"; 
			}
			winLossDetermined = true; 
		}
		
		if (numFullyLanded > 0){
			Time.timeScale = 0.0f; //freeze game
		}
		
		scoreText.text = "Score: " + (int) secondsSinceStart; 
		
		
		
		if (secondsSinceLastGeneration >= planeGenerationTime){
			//create airplane
			GameObject airplane = (GameObject) Instantiate(exampleOfQuirkyAirplane);
			airplane.transform.localScale = new Vector3(SharedVariables.planeScale, SharedVariables.planeScale,
				SharedVariables.planeScale); 
			
			Vector3 startingPos; 
			//random point on some border edge
			if (QuirkyPlaneMover.weightedCoinFlip(0.5f)){
				a = planeStartDistance;
			}else {
				a = -1 * planeStartDistance; 
			}
			b = UnityEngine.Random.Range(-1 * planeStartDistance, planeStartDistance); 
			if (QuirkyPlaneMover.weightedCoinFlip(0.5f)){
				startingPos = new Vector3(a, planeStartingHeight, b);
			} else {
				startingPos = new Vector3(b, planeStartingHeight, a);
			}
			startingPos = new Vector3(startingPos.x + SharedVariables.baseLocation.x,
									  startingPos.y + SharedVariables.baseLocation.y,
									  startingPos.z + SharedVariables.baseLocation.z); 
			 
			airplane.SetActive(true); 
			
			//airplane.renderer.material.color = Color.red;
			airplane.transform.Translate(startingPos - airplane.transform.position); 
			
			
			planes.Add(airplane); 
			secondsSinceLastGeneration = 0; 
		}
	}
}
