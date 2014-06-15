using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {
	public float speed;
	void Start () {
		var angles = gameObject.transform.eulerAngles;
		var v = new Vector3 (0, Mathf.Sin (Mathf.PI * angles.x / 180), Mathf.Cos (Mathf.PI * angles.x / 180));
		rigidbody.AddForce (Vector4.Normalize(v) * speed, ForceMode.VelocityChange); 	
	}
	void FixedUpdate() {
		//Debug.Log (rigidbody.velocity);
		if (rigidbody.velocity != Vector3.zero) {
			rigidbody.MoveRotation(Quaternion.LookRotation(rigidbody.velocity));
		}
			//rigidbody.MoveRotation(Quaternion.LookRotation(rigidbody.velocity));
	}
//	public Transform target;
	void Update () 
	{
//		Vector3 relativePos = target.position - transform.position;
//		Quaternion rotation = Quaternion.LookRotation(relativePos);
//		transform.rotation = rotation;

//		if (rigidbody.velocity != Vector3.zero)
//			transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
//			print (transform.rotation);
	}
}
