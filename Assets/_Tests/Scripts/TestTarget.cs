using UnityEngine;
using System.Collections;

public class TestTarget : MonoBehaviour {
    public Vector3 velocity;
    void Awake() {
        rigidbody.velocity = velocity;
    }
	void OnTriggerEnter(Collider other) {
        transform.parent.GetComponent<TestShooter>().OnHit(gameObject);
    }
}
