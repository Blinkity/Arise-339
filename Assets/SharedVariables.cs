using UnityEngine;
using System.Collections;

public class SharedVariables : MonoBehaviour {
	
	public static Vector3 baseLocation = new Vector3(0,0,300);
	public static float heightAtWhichToLanded = 0f;
	
	public static float planeScale = 6f; 
	public static float heightAtWhichToRotateToLandingMode = planeScale + 3f; 
	
	public static float maxDistanceFromHome = 200f; 
	
	public static float baseWidth = 100f; 
	public static float baseLength = 100f; 
	
	public static Vector3 landingLocation() {
		return new Vector3(baseLocation.x, baseLocation.y + heightAtWhichToLanded, baseLocation.z); 
		
	}
	
}
