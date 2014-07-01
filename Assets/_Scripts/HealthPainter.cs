using UnityEngine;
using System.Collections;

public class HealthPainter : MonoBehaviour {
	public int healthBarHeight = 2;
	public int healthBarWidth = 50;
	public int adjustment = 0;
	public int barTop = 1;

	HitableObjectBase hitable;
	public Texture ActiveBarTexture;

	public Texture BackgroundBarTexture;
	//GUIStyle style = new GUIStyle ();

	void Awake () {
		hitable = GetComponent<HitableObjectBase>();
	}
	
	void OnGUI () {
		Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + adjustment, transform.position.z);
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
		//screenPosition.y = Screen.height - screenPosition.y;
		//print (screenPosition);
		//creating a ray that will travel forward from the camera's position   
		//var ray = new Ray (Camera.main.transform.position, transform.forward);
		//RaycastHit hit;
		//var forward = transform.TransformDirection(Vector3.forward);
		//var distance = Vector3.Distance(Camera.main.transform.position, transform.position); //gets the distance between the camera, and the intended target we want to raycast to

		//if something obstructs our raycast, that is if our characters are no longer 'visible,' dont draw their health on the screen.
		//if (!Physics.Raycast(ray, out hit, distance))
		//{
		//GUI.backgroundColor = Color.red;
		Rect rect = new Rect (screenPosition.x - healthBarWidth / 2, Screen.height - screenPosition.y - barTop, healthBarWidth, healthBarHeight);
		GUI.DrawTexture(rect, BackgroundBarTexture);
		rect.width = healthBarWidth * hitable.HealthRatio;
		GUI.DrawTexture(rect, ActiveBarTexture); //displays a healthbar

//			GUI.HorizontalScrollbar(new Rect (screenPosition.x - healthBarLeft / 2, Screen.height - screenPosition.y - barTop, 100, 0), 0, hitable.health / 2, 0, hitable.health); //displays a healthbar
			
//			GUI.color = Color.white;
//			GUI.contentColor = Color.white;                
//			GUI.Label(new Rect(screenPosition.x - healthBarLeft / 2, Screen.height - screenPosition.y - barTop + 5, 100, 100), "" + 100 + "/"+ 100); //displays health in text format
		//}	
	}
}
