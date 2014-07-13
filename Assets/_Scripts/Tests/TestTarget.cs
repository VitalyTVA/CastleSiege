using UnityEngine;
using System.Collections;

public class TestTarget : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
        transform.parent.GetComponent<TestShooter>().OnHit(gameObject);
    }
}
