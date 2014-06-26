using UnityEngine;
using System.Collections;

public class BoundsController : MonoBehaviour {
	void OnTriggerExit(Collider other) {
		GameObject.Destroy (other.gameObject);
	}
}
