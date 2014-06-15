using UnityEngine;
using System.Collections;

public class AlignAlongSpeed : MonoBehaviour {
	void FixedUpdate() {
		if (rigidbody.velocity != Vector3.zero) {
			rigidbody.MoveRotation(Quaternion.LookRotation(rigidbody.velocity));
		}
	}
}
