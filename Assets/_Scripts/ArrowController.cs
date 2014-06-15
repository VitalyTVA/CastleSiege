﻿using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {
	public float speed;
	void Start () {
		var angles = gameObject.transform.eulerAngles;
		var v = new Vector3 (0, Mathf.Sin (Mathf.PI * -angles.x / 180), Mathf.Cos (Mathf.PI * -angles.x / 180));
		rigidbody.AddForce (Vector4.Normalize(v) * speed, ForceMode.VelocityChange); 	
	}
}
