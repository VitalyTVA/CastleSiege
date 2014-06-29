using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	public Transform target;
	public float speed = 1f;
	public float acceleration = .5f;
	void Start () {
//		if (target == null)
//			return;
		Animator animator = GetComponent<Animator> ();
		HashIDs hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();
		animator.SetFloat(hash.speedFloat, 1f);
	}
	void FixedUpdate() {
		Vector3 targetDirection = target.transform.position - transform.position;
		targetDirection.y = 0;
		Vector3 targetSpeed = Vector3.Normalize (targetDirection) * speed;
		//if(rigidbody.velocity.magnitude < speed)
		rigidbody.AddForce((targetSpeed - rigidbody.velocity) * acceleration, ForceMode.Acceleration);
		
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, 1);
		rigidbody.MoveRotation(newRotation);
	}
}
