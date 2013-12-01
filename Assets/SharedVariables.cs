using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SharedVariables : MonoBehaviour {
	
	public static Vector3 baseLocation = new Vector3(0,0,300);
	public static float heightAtWhichToLanded = 0f;
	
	public static float planeScale = 6f; 
	public static float heightAtWhichToRotateToLandingMode = planeScale + 3f; 
	
	public static float maxDistanceFromHome = 2000f; 
	
	public static float baseWidth = 100f; 
	public static float baseLength = 100f; 






	//plane speeds
	public static float speed = 100f; //20.0F;
	public static float horizontalRotationSpeed = 90f;
	public static float verticalRotationSpeed = 90f;
	public static float rollRotationSpeed = 120f;
	
	//Values below must sum to 0, should check for this obviously.
	public static float surpriseDelayedResponseProbGivenProblem = 0.0f;
	public static float surpriseDropperProbGivenProblem = 0.5f;
	public static float surpriseJerkerProbGivenProblem = 0.5f;

	//color properties
	public static List<Color> allColors = new List<Color>();
	static SharedVariables() {
		allColors.Add(Color.red);
		allColors.Add(Color.yellow);
		allColors.Add(Color.blue);
		allColors.Add(Color.green);
		allColors.Add(Color.yellow);
		allColors.Add(Color.black);
		allColors.Add(Color.white);
		allColors.Add(Color.magenta);
		allColors.Add(Color.grey);
	}
	
	//jerky properties
	public static float jerkyFactor = 2f; 
	public static int minSecondsBetweenJerks = 5;
	public static float jerkDuration = 0.75f; 
	
	//delayedResponse properties
	public static int delaySeconds = 2; 
	
	//dropper properties
	public static long minSecondsBetweenDrops = 5; 
	public static int dropHeight = -15;
	public static double dropDuration = 1; 


	public static float knownToBeJerkerProb = 0.25f;
	public static float knownToBeDropperProb = 0.25f;

	public static float surpriseQuirkProb = 0.25f;

	public static int planeStartDistance = 1000;
	public static int planeStartingHeight = 1000;


	public static double planeGenerationTime = 20; 

	public static Vector3 landingLocation() {
		return new Vector3(baseLocation.x, baseLocation.y + heightAtWhichToLanded, baseLocation.z); 
		
	}
	
}
