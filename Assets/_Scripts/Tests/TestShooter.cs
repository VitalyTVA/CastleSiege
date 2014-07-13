using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

public class TestShooter : MonoBehaviour {
    public float velocity;
    public GameObject shootee;
    List<GameObject> targets;
	void Start () {
        targets = transform.Cast<Transform>().Select(x => x.gameObject).ToList();
        foreach(GameObject target in targets) {
            Shooter.Shoot(transform.position, target, shootee, velocity);
        }
	}
    public void OnHit(GameObject target) {
        if(!targets.Contains (target))
			throw new InvalidOperationException();
        targets.Remove(target);
        if(!targets.Any())
            IntegrationTest.Pass(gameObject);
    }
}
