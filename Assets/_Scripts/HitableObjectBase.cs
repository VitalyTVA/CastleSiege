using UnityEngine;
using System.Collections;

public abstract class HitableObjectBase : MonoBehaviour  {
	public int maxHealth = 100;
	internal int health;
	public float HealthRatio { get { return (float)health / maxHealth; } }
	void Start() {
		health = maxHealth / 2;
	}
	public abstract void Hit ();
}
