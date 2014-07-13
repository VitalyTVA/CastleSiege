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
            yield return new WaitForSeconds(1);
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            var targetCollider = colliders.FirstOrDefault(x => IsTargetWithRigidBody(x));
            if(targetCollider != null) {
                Shoot(transform.position, targetCollider.gameObject, shotee, speed);
            }
        }
    }
    public static void Shoot(Vector3 position, GameObject targetObject, GameObject shootee, float shooteeVelocity) {
        if(targetObject.rigidbody != null) {
            Transform target = targetObject.GetComponent<Target>().target.transform;

            Vector3? velocity = MathHelper.CalcShootVelocity(position, target.position, targetObject.rigidbody.velocity, shooteeVelocity, Physics.gravity.y);
            if(velocity != null) {
                GameObject shoteeInstance = (GameObject)Instantiate(shootee, position, Quaternion.identity);
                shoteeInstance.rigidbody.AddForce(velocity.Value, ForceMode.VelocityChange);
            }
        }
    }
    public static bool IsTargetWithRigidBody(Collider x) {
        return x.gameObject.GetComponent<Target>() != null && x.gameObject.rigidbody != null;
    }
}
