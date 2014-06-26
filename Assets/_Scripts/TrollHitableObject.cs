using UnityEngine;
using System.Collections;

public class TrollHitableObject : HitableObjectBase {
	Animator anim;	
	HashIDs hash;
	bool dead = false;
	bool dying = false;

	void Awake () {
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();
	}
	
	public override void Hit() {
		dying = true;
//		Animator animator = GetComponent<Animator> ();
//		HashIDs hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();
//		animator.SetBool(hash.deadBool, true);

//		animator.SetBool(hash.deadBool, false);
		//animator.SetFloat(hash.speedFloat, 0f);
		//GameObject.Destroy (gameObject, 1f);

	}	
	void Update ()
	{
		if (!dying)
						return;
			// ... and if the player is not yet dead...
		if(!dead)
			PlayerDying();
		else
			PlayerDead();
	}
	void PlayerDying ()
	{
		dead = true;
		anim.SetBool(hash.deadBool, dead);

	}
	
	
	void PlayerDead ()
	{
		if(anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.dieState)
			anim.SetBool(hash.deadBool, false);
		anim.SetFloat(hash.speedFloat, 0f);
		GameObject.Destroy (gameObject, 1f);
	}
}
