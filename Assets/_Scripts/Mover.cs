using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	public Transform target;
	public float speed = 1f;
	void Start () {
//		if (target == null)
//			return;
		Animator animator = GetComponent<Animator> ();
		HashIDs hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();
		animator.SetFloat(hash.speedFloat, 1f);

		Vector3 targetDirection = target.transform.position - transform.position;
		targetDirection.y = 0;
		rigidbody.velocity = Vector3.Normalize (targetDirection) * speed;
		
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, 1);
		rigidbody.MoveRotation(newRotation);

	}
//	void FixedUpdate() {
//	}
}
