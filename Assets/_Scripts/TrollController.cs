using UnityEngine;
using System.Collections;

public class TrollController : MonoBehaviour
{
	public float turnSmoothing = 15f;   // A smoothing value for turning the player.
	public float speedDampTime = 0.1f;  // The damping for the speed parameter
	public float upSpeed = 1f;  // The damping for the speed parameter	
	
	private Animator anim;              // Reference to the animator component.
	private HashIDs hash;               // Reference to the HashIDs.
	
	
	void Awake ()
	{
		// Setting up the references.
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();
	}
	
	
	void FixedUpdate ()
	{
		// Cache the inputs.
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		MovementManagement(h, v);
	}
	
	

	
	void MovementManagement (float horizontal, float vertical)
	{
		// If there is some axis input...
		if(horizontal != 0f || vertical != 0f)
		{
			// ... set the players rotation and set the speed parameter to 5.5f.
			Rotating(horizontal, vertical);
			anim.SetFloat(hash.speedFloat, 1f, speedDampTime, Time.deltaTime);
			Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);

			rigidbody.AddRelativeForce(new Vector3(0, upSpeed, 0));
			//rigidbody.MovePosition(Vector3.Lerp(rigidbody.position, rigidbody.position + targetDirection, speed * Time.deltaTime));
		}
		else {
			// Otherwise set the speed parameter to 0.
			anim.SetFloat(hash.speedFloat, 0);
		}
	}
	
	
	void Rotating (float horizontal, float vertical)
	{
		// Create a new vector of the horizontal and vertical inputs.
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		
		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		
		// Change the players rotation to this new rotation.
		rigidbody.MoveRotation(newRotation);
	}
}