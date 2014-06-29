using UnityEngine;
using System.Collections;

public class ExplodableHitter : Hitter {
	public float explosionRadius = 5f;
	public float explostionForce = 10f;
	public float upwardsForce = 10f;
	protected override void Destroy () {
		Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius);
		foreach (var collider in colliders) {
			if(collider.gameObject.rigidbody != null)
				collider.gameObject.rigidbody.AddExplosionForce(explostionForce, transform.position, explosionRadius, upwardsForce, ForceMode.Impulse);
		}
		base.Destroy ();
	}
}
