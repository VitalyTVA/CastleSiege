﻿using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
	// Here we store the hash tags for various strings used in our animators.
	public int idleState;
	public int runState;
	public int dieState;
	public int speedFloat;
//	public int deadBool;
	public int onHitTrigger;
	public int onDieTrigger;

	void Awake ()
	{
		idleState = Animator.StringToHash("Base Layer.Idle");
		runState = Animator.StringToHash("Base Layer.Run");
		dieState = Animator.StringToHash("Base Layer.Die");

		speedFloat = Animator.StringToHash("Speed");
//		deadBool = Animator.StringToHash("Dead");
		onHitTrigger = Animator.StringToHash("OnHit");
		onDieTrigger = Animator.StringToHash("OnDie");}
}