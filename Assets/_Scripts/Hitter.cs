using UnityEngine;
using System.Linq;
using System.Collections;

public class Hitter : MonoBehaviour {
	public int damage = 34;
	void OnCollisionEnter(Collision collision) {
		HitGameObject (collision.gameObject, damage);
		GameObject.Destroy (gameObject);
	}
	public static void HitGameObject(GameObject gameObject, int damage) {
		var hitable = gameObject.GetComponent<HitableObjectBase>();
		if (hitable != null) {
			hitable.Hit(damage);
		}
	}
}
