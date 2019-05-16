using UnityEngine;
using System.Collections;

public class LocomotionMaleScript : MonoBehaviour {
	private Animator anim;
	
	// Use this for initialization
	void Start () {
		anim = this.transform.GetComponent<Animator>();
	}
	
	void OnGUI () {
		GUILayout.Label("CONTROLS");
		GUILayout.Label("Movement: W A S D");
		GUILayout.Label("Turn: Q E");
		GUILayout.Label("Jump: Spacebar");
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		anim.SetFloat("Speed", vertical, 0.15f, Time.deltaTime);
		anim.SetFloat("Direction", horizontal, 0.15f, Time.deltaTime);
		
		//Procedural rotation input, applied while moving. This allows turning without the need for turning animations.
		if (vertical > 0.05f){
			if(horizontal > 0.05f)
				this.transform.Rotate(Vector3.up * (Time.deltaTime + 2), Space.World);
			if(horizontal < -0.05f)
				this.transform.Rotate(Vector3.up * (Time.deltaTime + -2), Space.World);
		}

		else if (vertical < -0.05f){
			if(horizontal > 0.05f)
				this.transform.Rotate(Vector3.up * (Time.deltaTime + -2), Space.World);
			if(horizontal < -0.05f)
				this.transform.Rotate(Vector3.up * (Time.deltaTime + 2), Space.World);
		}

		//Procedural rotation input for stationary turning
		if(Input.GetKey(KeyCode.Q)){
			anim.SetFloat("Turn", -1, 0.1f, Time.deltaTime);
			this.transform.Rotate(Vector3.up * (Time.deltaTime + -2), Space.World);
		}

		else if (Input.GetKey(KeyCode.E)){
			anim.SetFloat("Turn", 1, 0.1f, Time.deltaTime);
			this.transform.Rotate(Vector3.up * (Time.deltaTime + 2), Space.World);
		}

		else { anim.SetFloat("Turn", 0, 0.1f, Time.deltaTime); }
		
		//Pressing the space bar will cause the character to jump
		if (Input.GetButton("Jump")){
			StartCoroutine(TriggerAnimatorBool("Jump"));
		}
		
	}
	
	///Triggers the bool of the provided name in the animator.
	///The bool is only active for a single frame to prevent looping.
	private IEnumerator TriggerAnimatorBool (string name){
		anim.SetBool(name, true);
		yield return null;
		anim.SetBool(name, false);
	}
}
