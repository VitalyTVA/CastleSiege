using UnityEngine;
using System.Linq;
using System.Collections;

public class Hitter : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		var hitable = other.GetComponents<MonoBehaviour>().OfType<IHitableObject>().FirstOrDefault();
		if (hitable != null) {
			GameObject.Destroy (gameObject);
			hitable.Hit();
		}
	}
}
