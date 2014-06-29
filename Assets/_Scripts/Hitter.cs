using UnityEngine;
using System.Linq;
using System.Collections;

public class Hitter : MonoBehaviour {
	void OnCollisionEnter(Collision collision) {
		var hitable = collision.gameObject.GetComponent<HitableObjectBase>();
		if (hitable != null) {
			hitable.Hit();
		}
		Destroy();
	}
	protected virtual void Destroy() {
		GameObject.Destroy (gameObject);
	}
//	void OnTriggerEnter(Collider other) {
//		var hitable = other.GetComponent<HitableObjectBase>();
//		if (hitable != null) {
//			GameObject.Destroy (gameObject);
//			hitable.Hit();
//		}
//	}
}
