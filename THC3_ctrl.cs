using UnityEngine;
using System.Collections;

public class THC3_ctrl : MonoBehaviour {
	
	
	private Animator anim;
	private CharacterController controller;
	private int battle_state = 0;
	public float speed = 6.0f;
	public float runSpeed = 3.0f;
	public float turnSpeed = 60.0f;
	public float gravity = 20.0f;
	private Vector3 moveDirection = Vector3.zero;
	private float w_sp = 0.0f;
	private float r_sp = 0.0f;

	
	// Use this for initialization
	void Start () 
	{						
		anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController> ();

		w_sp = speed; //read walk speed
		r_sp = runSpeed; //read run speed
	}
	
	// Update is called once per frame
	void Update () 
	{		
		if (Input.GetKey ("1"))  // turn to battle state with walking
		{ 		
			anim.SetInteger ("battle", 0);
			battle_state = 0;
		}
		if (Input.GetKey ("2")) // turn to battle state with run
		{ 
			anim.SetInteger ("battle", 1);
			battle_state = 1;
			
		}
						
		if (Input.GetKey ("up")) 
		{	
			if (battle_state == 0) {
				anim.SetInteger ("moving", 1);//walk
				runSpeed = 1;
			}

			if (battle_state == 1) {
				anim.SetInteger ("moving", 2);//run
				runSpeed = r_sp;
			}
		}
		else if (Input.GetKey ("down")) 
		{	
			if (battle_state == 0) {
				anim.SetInteger ("moving", 20);//walk
				runSpeed = 0.5f;
			}
			
			if (battle_state == 1) {
				anim.SetInteger ("moving", 21);//run
				runSpeed = r_sp/2;
			}
		}

		else 
			{
				anim.SetInteger ("moving", 0);
			}

	
		if (Input.GetMouseButtonDown (0)) { // attack1
			anim.SetInteger ("moving", 4);
		}
		if (Input.GetMouseButtonDown (1)) { // attack2
			anim.SetInteger ("moving", 5);
		}
		if (Input.GetMouseButtonDown (2)) { // attack3
			anim.SetInteger ("moving", 6);
		}

		if (Input.GetKeyDown ("i")) //die1
		{ 
			anim.SetInteger ("moving", 98);
		}
		if (Input.GetKeyDown ("o")) //die1
		{ 
			anim.SetInteger ("moving", 99);
		}

		if (Input.GetKeyDown ("u")) //hit
		{   
				int n = Random.Range (0, 2);
				if (n == 0) 
				{
					anim.SetInteger ("moving", 10);
				} 
				else 
				{
				anim.SetInteger ("moving", 11);
				}
		}


		if (Input.GetKeyDown ("x")) { //howl
			anim.SetInteger ("moving",8);
		}


		if (controller.isGrounded) 
		{
			moveDirection=transform.forward * Input.GetAxis ("Vertical") * speed * runSpeed;
			float turn = Input.GetAxis("Horizontal");
			transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);						
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move (moveDirection * Time.deltaTime);
		}
}



