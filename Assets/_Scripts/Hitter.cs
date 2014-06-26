using UnityEngine;
using System.Collections;

public class Hitter : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		var hitable = other.GetComponent<HitableObject> ();
		if (hitable != null) {
			GameObject.Destroy (gameObject);
			hitable.Hit();
		}
	}
}
