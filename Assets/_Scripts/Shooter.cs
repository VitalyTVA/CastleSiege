using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shooter : MonoBehaviour {
	public GameObject shotee;
	public Transform target;
	public float speed;

	void Start () {
		StartCoroutine (ShootRoutine ());
	}
	IEnumerator<WaitForSeconds> ShootRoutine() {
		while (true) {
			yield return new WaitForSeconds (1);
			GameObject shoteeInstance = (GameObject)Instantiate(shotee, transform.position, Quaternion.identity);

			float distance = Vector3.Distance(transform.position, target.position);
			float angle = MathHelper.CalcShootAngleInRad(distance, speed, -Physics.gravity.y);
			var direction = Mathf.Deg2Rad * (Quaternion.LookRotation(transform.position - target.position).eulerAngles.y - 180);
			//Debug.Log((Mathf.Rad2Deg  * direction).ToString());

			var v = new Vector3 (Mathf.Sin(direction) * Mathf.Cos (angle), Mathf.Sin (angle), Mathf.Cos(direction) * Mathf.Cos (angle));

			shoteeInstance.rigidbody.AddForce (Vector4.Normalize (v) * speed, ForceMode.VelocityChange);
		}
	}
}
