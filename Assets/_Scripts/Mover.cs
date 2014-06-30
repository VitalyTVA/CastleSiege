using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	public Transform target;
	public float speed = 1f;
	public float acceleration = 2f;
//	public float tolerance = .05f;
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
		Vector3 actualSpeed = rigidbody.velocity;
		actualSpeed.y = 0;
		Vector3 targetProjectionSpeed = Vector3.Project (actualSpeed, targetSpeed);
		Vector3 moveAwaySpeed = actualSpeed - targetProjectionSpeed;
		Vector3 targetSpeedChange = (targetSpeed - targetProjectionSpeed);
		//Vector3 moveAwaySpeedChange = -moveAwaySpeed;

		if(targetProjectionSpeed.magnitude < targetSpeed.magnitude || Vector3.Dot(targetProjectionSpeed, targetSpeed) < 0)
		   rigidbody.AddForce(Vector3.Normalize (targetDirection) * acceleration, ForceMode.Acceleration);
		if(targetProjectionSpeed.magnitude > targetSpeed.magnitude && Vector3.Dot(targetProjectionSpeed, targetSpeed) > 0)
			rigidbody.AddForce(Vector3.Normalize (-targetDirection) * acceleration, ForceMode.Acceleration);

		if(moveAwaySpeed.magnitude > 0)
			rigidbody.AddForce(Vector3.Normalize (-moveAwaySpeed) * acceleration, ForceMode.Acceleration);
		//rigidbody.AddForce((targetSpeedChange + moveAwaySpeedChange) * acceleration, ForceMode.Acceleration);
		
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, 1);
		rigidbody.MoveRotation(newRotation);
	}
}
