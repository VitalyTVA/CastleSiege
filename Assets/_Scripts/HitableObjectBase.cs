using UnityEngine;
using System.Collections;

public abstract class HitableObjectBase : MonoBehaviour  {
	public int maxHealth = 100;
	internal int health;
	public float HealthRatio { get { return (float)health / maxHealth; } }
	void Start() {
		health = maxHealth;
	}
	public void Hit (int damage) {
		if(damage == 0 || health <= 0)
			return;
		health -= damage;
		if(health > 0)
			OnHit();
		else
			OnDie();
	}
	protected abstract void OnDie ();
	protected abstract void OnHit ();
}
