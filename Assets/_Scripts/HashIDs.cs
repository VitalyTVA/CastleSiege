using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
	// Here we store the hash tags for various strings used in our animators.
	public int idleState;
	public int runState;
	public int speedFloat;
	
	void Awake ()
	{
		idleState = Animator.StringToHash("Base Layer.idle");
		runState = Animator.StringToHash("Base Layer.run");
		speedFloat = Animator.StringToHash("Speed");
	}
}