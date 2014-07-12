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
            if(targetCollider != null && targetCollider.gameObject.rigidbody != null) {
				Transform target = targetCollider.gameObject.GetComponent<Target>().target.transform;

                Vector3? velocity = MathHelper.CalcShootVelocity(transform.position, target.position, targetCollider.gameObject.rigidbody.velocity, speed, Physics.gravity.y);
                if(velocity != null) {
                    GameObject shoteeInstance = (GameObject)Instantiate(shotee, transform.position, Quaternion.identity);
                    shoteeInstance.rigidbody.AddForce(velocity.Value, ForceMode.VelocityChange);
                }
            }
		}
	}
}
