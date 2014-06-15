using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {
	public float speed;
	void Start () {
		StartCoroutine (ShootRoutine ());
	}
	IEnumerator ShootRoutine() {
		yield return new WaitForSeconds (1);

		gameObject.transform.eulerAngles = new Vector3 (-30, 0, 0);
		var angles = gameObject.transform.eulerAngles;
		var v = new Vector3 (0, Mathf.Sin (Mathf.PI * -angles.x / 180), Mathf.Cos (Mathf.PI * -angles.x / 180));
		rigidbody.useGravity = true;
		rigidbody.AddForce (Vector4.Normalize (v) * speed, ForceMode.VelocityChange);

		yield break;
	}
}
