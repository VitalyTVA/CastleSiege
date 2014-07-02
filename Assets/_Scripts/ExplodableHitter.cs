using UnityEngine;
using System.Collections;

public class ExplodableHitter : MonoBehaviour {
	public float explosionRadius = 5f;
	public float explostionForce = 10f;
	public float upwardsForce = 10f;
	public int maxDamage = 100;

	void OnCollisionEnter(Collision collision) {
		Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius);
		foreach (var collider in colliders) {
			if(collider.gameObject.rigidbody != null) {
				collider.gameObject.rigidbody.AddExplosionForce(explostionForce, transform.position, explosionRadius, upwardsForce, ForceMode.Impulse);
				var damageRatio =  Mathf.Max(0, explosionRadius - Vector3.Distance(transform.position, collider.gameObject.transform.position)) / explosionRadius;
				//print(damageRatio);
				Hitter.HitGameObject(collider.gameObject, (int)(maxDamage * damageRatio));
			}
		}
		GameObject.Destroy (gameObject);
	}
}
