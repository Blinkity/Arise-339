var gameover=0;//Flugzeug an und abschalten. Wird später interessant. Game Over || Turn on and off the airplane code. Becomes interesting at a later point. Game over ...
static var airplaneangley: float=0.0;//Diese Variable geht zum Blob Shadow. || This goes to the blob shadow. 

//Rotation und Position unseres Flugzeugs. || Rotaton and position of our airplane
var rotationx=0;
var rotationy=0;
var rotationz=0;
var positionx: float=0.0;
var positiony: float=0.0;
var positionz: float=0.0;

var speed:float =0.0;// speed Variable gibt die Geschwindigkeit an || speed variable is the speed
var uplift:float =0.0;

function Update () {

//Schattenzeug || Shadowstuff-----------------------------------------------------------------------------------------------------------------------------------------

		//Schatten auf gleiche Drehung wie Flugzeug || shadow to the same angle than the airplane
		airplaneangley= transform.eulerAngles.y; 

//Maincode fliegen || Maincode flying ----------------------------------------------------------------------------------------------------------------------------------

	// Mit varwhatmoves 1 ist der Code aktiv || Code is active when varwhatmoves is 1
	if(gameover==0){

	// Drehungen des Flugzeugs || Rotations of the airplane
		if (speed>595)transform.Rotate(Input.GetAxis("Vertical")*Time.deltaTime*100,0,0); //Hoch Runter, limitiert auf eine Minimalgeschwindigkeit || Up Down, limited to a minimum speed
		transform.Rotate(0,Input.GetAxis("Horizontal")*Time.deltaTime*100,0,Space.World); //Rechts Links || Left Right
		if (groundtrigger.triggered==0) transform.Rotate(0,0,Input.GetAxis("Horizontal")*Time.deltaTime*50*-1); //Seitenneigung. Mal Minus 1 um in die richtige Richtung zu drehen || Tilt multiplied with minus 1 to go into the right direction
		
		//Seitenneigung limitieren damit flugzeug in Kurve keine Rolle schlägt || limit tilt so that airplane doesn`t fly a roll while flying a curve
		if ((Input.GetAxis ("Horizontal")<0)&&(rotationz >45)&&(rotationz <90)) transform.Rotate(0,0,Time.deltaTime*-50);//linksrum || to the left
		if ((Input.GetAxis ("Horizontal")>0)&&(rotationz <315)&&(rotationz >270)) transform.Rotate(0,0,Time.deltaTime*50);//rechtsrum ||to the right

	// Geschwindigkeit || Speed
		transform.Translate(0,0,speed/20*Time.deltaTime);
	
	
	//Variablen auf Position und Rotation des Objekts einstellen || Turn variables to rotation and position of the object
		rotationx=transform.eulerAngles.x; 
		rotationy=transform.eulerAngles.y; 
		rotationz=transform.eulerAngles.z; 
		positionx=transform.position.x;
		positiony=transform.position.y;
		positionz=transform.position.z;

	
	//Zurückdrehen Z Achse. Limitiert auf Horizontal Button ist nicht gedrückt|| Rotate back in z axis , limited by no horizontal button pressed
		if ((rotationz >0) && (rotationz < 90)&&(!Input.GetButton ("Horizontal"))) transform.Rotate(0,0,Time.deltaTime*-50);
		if ((rotationz >0) && (rotationz > 270)&&(!Input.GetButton ("Horizontal"))) transform.Rotate(0,0,Time.deltaTime*50);
		if ((rotationz >180) && (rotationz < 270)&&(!Input.GetButton ("Horizontal"))) transform.Rotate(0,0,Time.deltaTime*-50);
		if ((rotationz <180) && (rotationz > 90)&&(!Input.GetButton ("Horizontal"))) transform.Rotate(0,0,Time.deltaTime*50);
		
		//Zurückdrehen X Achse || Rotate back X axis
		
				if ((rotationx >0) && (rotationx < 180)&&(!Input.GetButton ("Vertical"))) transform.Rotate(Time.deltaTime*-50,0,0);
				if ((rotationx >0) && (rotationx > 180)&&(!Input.GetButton ("Vertical"))) transform.Rotate(Time.deltaTime*50,0,0);
		
		
		//Geschwindigkeit Fahren und Fliegen || Speed driving and flying ------------------------------------------------------------------------------------------
		//Wir brauchen ein minimales Geschwindigkeitslimit in der Luft. Wir limitieren wieder mit der groundtrigger.triggered Variable
		//We need a minimum speed limit in the air. We limit again with the groundtrigger.triggered variable
	
		// Input Gas geben und abbremsen am Boden|| Input Accellerate and deccellerate at ground
		if ((groundtrigger.triggered==1)&&(Input.GetButton("Fire1"))&&(speed<800)) speed+=Time.deltaTime*240;
		if ((groundtrigger.triggered==1)&&(Input.GetButton("Fire2"))&&(speed>0)) speed-=Time.deltaTime*240;
		
				// Input Gas geben und abbremsen in der Luft|| Input Accellerate and deccellerate in the air
		if ((groundtrigger.triggered==0)&&(Input.GetButton("Fire1"))&&(speed<800)) speed+=Time.deltaTime*240;
		if ((groundtrigger.triggered==0)&&(Input.GetButton("Fire2"))&&(speed>600)) speed-=Time.deltaTime*240;
		
		//Auftrieb  -------------------------------------------------------------------------------------------------------------------------------------------------------
		//Wenn in der Luft weder Gasgeben noch Abbremsen gedrückt wird soll unser Flugzeug auf eine neutrale Geschwindigkeit gehen. Mit dieser Geschwindigkeit soll es auch neutral in der Höhe bleiben. Drüber soll es steigen, drunter soll es sinken. Auf diesem Wege lässt sich dann abheben und landen
		//When we don`t accellerate or deccellerate we want to go to a neutral speed in the air. With this speed it has to stay at a neutral height. Above this value the airplane has to climb, with a lower speed it has to  sink. That way we are able to takeoff and land then.
		
			//Neutrale Geschwindigkeit bei 700 || Neutral speed at 700
			//Dieser Code stellt in der Luft die Geschwindigkeit auf 700 zurück wenn nicht gasgegeben oder abgebremst wird. Maximum 800, minimum 600
			//This code resets the speed to 700 when there is no acceleration or deccelleration. Maximum 800, minimum 600
		if((Input.GetButton("Fire1")==false)&&(Input.GetButton("Fire2")==false)&&(speed>595)&&(speed<700)) speed+=Time.deltaTime*240;
		if((Input.GetButton("Fire1")==false)&&(Input.GetButton("Fire2")==false)&&(speed>595)&&(speed>700)) speed-=Time.deltaTime*240;
	
		
		//uplift - Auftrieb
		transform.Translate(0,uplift*Time.deltaTime/10.0,0);
				
		//Uplift kalkulieren. Der Auftrieb || Calculate uplift
		uplift = -700+speed;
//Wir wollen am Boden keinen Abtrieb. Wenn die Uplift am Boden kleiner 0 ist, setzen wir sie auf 0. We don`t want downlift. So when the uplift value lower zero we set it to 0
		if ((groundtrigger.triggered==1)&&(uplift < 0)) uplift=0; 
	}
}
