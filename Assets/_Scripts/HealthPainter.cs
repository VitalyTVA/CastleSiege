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
		Rect rect = new Rect (screenPosition.x - healthBarWidth / 2, Screen.height - screenPosition.y - barTop, healthBarWidth, healthBarHeight);
		GUI.DrawTexture(rect, BackgroundBarTexture);
		rect.width = healthBarWidth * hitable.HealthRatio;
		GUI.DrawTexture(rect, ActiveBarTexture); //displays a healthbar
	}
}
