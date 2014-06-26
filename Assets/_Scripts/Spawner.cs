using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
	public GameObject spawnee;
	public Transform target;
	public float interval = 1;

	void Start () {
		StartCoroutine (SpawnRoutine ());
	}
	IEnumerator<WaitForSeconds> SpawnRoutine() {
		while (true) {
			yield return new WaitForSeconds (interval);
			GameObject spawneeInstance = (GameObject)Instantiate(spawnee, transform.position, Quaternion.identity);
			spawneeInstance.GetComponent<Mover>().target = target;
		}
	}

}
