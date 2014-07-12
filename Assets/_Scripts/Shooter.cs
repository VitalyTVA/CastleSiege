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

                Vector3? velocity = MathHelper.CalcShootVelocity(transform.position, target.position, speed, Physics.gravity.y);
                if(velocity != null) {
                    GameObject shoteeInstance = (GameObject)Instantiate(shotee, transform.position, Quaternion.identity);
                    shoteeInstance.rigidbody.AddForce(velocity.Value, ForceMode.VelocityChange);
                }

                //Vector3 distanceVector = transform.position - target.position;
                //distanceVector.y = 0;
                //float? angle = MathHelper.CalcShootAngleInRad(distanceVector.magnitude, transform.position.y - target.position.y, speed, Physics.gravity.y);
                //if(angle != null) {
                //    GameObject shoteeInstance = (GameObject)Instantiate(shotee, transform.position, Quaternion.identity);

                //    var direction = Mathf.Deg2Rad * (Quaternion.LookRotation(target.position - transform.position).eulerAngles.y);
                //    //Debug.Log((Mathf.Rad2Deg  * direction).ToString());

                //    var v = new Vector3(Mathf.Sin(direction) * Mathf.Cos(angle.Value), Mathf.Sin(angle.Value), Mathf.Cos(direction) * Mathf.Cos(angle.Value));
                //    Vector3? velocity2 = MathHelper.CalcShootVelocity(transform.position, target.position, speed, Physics.gravity.y);

                //    shoteeInstance.rigidbody.AddForce(Vector3.Normalize(v) * speed, ForceMode.VelocityChange);
                //}
            }
		}
	}
}
