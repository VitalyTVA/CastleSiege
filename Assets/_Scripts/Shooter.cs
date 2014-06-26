using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Shooter : MonoBehaviour {
	public GameObject shotee;
	//public Transform target;
	public float speed;
	public float radius;

	void Start () {
		StartCoroutine (ShootRoutine ());
	}
	IEnumerator<WaitForSeconds> ShootRoutine() {
		while (true) {
			yield return new WaitForSeconds (1);
			Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
			var targetCollider = colliders.FirstOrDefault(x => x.gameObject.GetComponent<Target>() != null);
			if(targetCollider != null) {
				Transform target = targetCollider.gameObject.GetComponent<Target>().target.transform;

				float distance = Vector3.Distance(transform.position, target.position);
				float? angle = MathHelper.CalcShootAngleInRad(distance, transform.position.y - target.position.y, speed, Physics.gravity.y);
				if(angle != null) {
					GameObject shoteeInstance = (GameObject)Instantiate(shotee, transform.position, Quaternion.identity);

					var direction = Mathf.Deg2Rad * (Quaternion.LookRotation(transform.position - target.position).eulerAngles.y - 180);
					//Debug.Log((Mathf.Rad2Deg  * direction).ToString());

					var v = new Vector3 (Mathf.Sin(direction) * Mathf.Cos (angle.Value), Mathf.Sin (angle.Value), Mathf.Cos(direction) * Mathf.Cos (angle.Value));

					shoteeInstance.rigidbody.AddForce (Vector4.Normalize (v) * speed, ForceMode.VelocityChange);
				}
			}
		}
	}
}
