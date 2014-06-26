using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
	public GameObject spawnee;
	public Transform target;
	public float interval = 5;
	public int rows = 2;
	public int columns = 3;
	public float distance = 1f;

	void Start () {
		StartCoroutine (SpawnRoutine ());
	}
	IEnumerator<WaitForSeconds> SpawnRoutine() {
		while (true) {
			yield return new WaitForSeconds (interval);
			for (int i = 0; i < rows; i++) {
				for (int j = 0; j < columns; j++) {
					var position = transform.position + new Vector3(j * distance, 0, i * distance);
					GameObject spawneeInstance = (GameObject)Instantiate(spawnee, position, Quaternion.identity);
					spawneeInstance.GetComponent<Mover>().target = target;
				}
			}
		}
	}

}
