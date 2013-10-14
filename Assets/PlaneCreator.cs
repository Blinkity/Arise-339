using UnityEngine;
using System.Collections;

public class PlaneCreator : MonoBehaviour {
	
	private double planeGenerationTime = 10; 
	public double secondsSinceLastGeneration; 
	
	
	public GameObject exampleOfQuirkyAirplane; 
	// Use this for initialization
	void Start () {
		secondsSinceLastGeneration = planeGenerationTime; 
	
	}
	
	public int a = 0; public int b = 0; 
			
	// Update is called once per frame
	void Update () {
		secondsSinceLastGeneration += Time.deltaTime; 
		
		if (secondsSinceLastGeneration >= planeGenerationTime){
			GameObject airplane = (GameObject) Instantiate(exampleOfQuirkyAirplane);
			airplane.transform.localScale = new Vector3(SharedVariables.planeScale, SharedVariables.planeScale,
				SharedVariables.planeScale); 
			
			
			Vector3 startingPos; 
			
			int startingDistance = 150; 
			//random point on some border edge
			if (QuirkyPlaneMover.weightedCoinFlip(0.5f)){
				a = startingDistance;
			}else {
				a = -1 * startingDistance; 
			}
			b = UnityEngine.Random.Range(-1 * startingDistance, startingDistance); 
			if (QuirkyPlaneMover.weightedCoinFlip(0.5f)){
				startingPos = new Vector3(a, 150, b);
			} else {
				startingPos = new Vector3(b, 150, a);
			}
			
			airplane.SetActive(true); 
			
			airplane.transform.Translate(startingPos - airplane.transform.position); 
			
			secondsSinceLastGeneration = 0; 
		}
	}
}
