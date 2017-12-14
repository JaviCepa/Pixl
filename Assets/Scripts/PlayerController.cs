using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	private bool   jumpStatus=false;
	private float  railStatus=0;
	private int    rail=2;
	private bool   alive=true;
	private int    playerScore=0;
	
	public  int    player=1;
	public  GameObject fader;
		
	public AudioClip Coin, Death, Jump, Poc;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	 int fingerCount = 0;
        foreach (Touch touch in Input.touches) {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                fingerCount++;
        }
        
        if (fingerCount > 0) {
        	GetComponent<Rigidbody>().AddForce(    0, +500f, 0); jumpStatus=false; GetComponent<AudioSource>().clip=Jump; GetComponent<AudioSource>().Play();
            print("User has " + fingerCount + " finger(s) touching the screen");
        }

		if (TimeCounter.gameStart) {
			if (Input.GetKeyDown(KeyCode.Space) && alive && jumpStatus && railStatus<=0) { GetComponent<Rigidbody>().AddForce(    0, +500f, 0); jumpStatus=false; GetComponent<AudioSource>().clip=Jump; GetComponent<AudioSource>().Play();}
			
			if (Input.GetKey(KeyCode.LeftArrow) && railStatus==0 && rail>0 && alive) {
				GetComponent<AudioSource>().clip=Jump; GetComponent<AudioSource>().Play();
				GetComponent<Rigidbody>().velocity=new Vector3(GetComponent<Rigidbody>().velocity.x,  10f, +4.3f);
				GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
				GetComponent<Rigidbody>().angularVelocity=new Vector3(8,0,0);
				rail--;
				railStatus=1;
			};
	
			if (Input.GetKey(KeyCode.RightArrow) && railStatus==0 && rail<4 && alive) {
				GetComponent<AudioSource>().clip=Jump; GetComponent<AudioSource>().Play();
				GetComponent<Rigidbody>().velocity=new Vector3(GetComponent<Rigidbody>().velocity.x,  10f, -4.3f);
				GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
				GetComponent<Rigidbody>().angularVelocity=new Vector3(-8,0,0);
				rail++;
				railStatus=1;
			};
			
			if (Input.GetKey(KeyCode.DownArrow) && alive && jumpStatus && railStatus==0 && GetComponent<Rigidbody>().angularVelocity.x==0) { GetComponent<Rigidbody>().AddForce(-5,  -0, 0); GetComponent<Rigidbody>().AddTorque(0,0,+500f); };
			if (Input.GetKey(KeyCode.UpArrow)   && alive && jumpStatus && railStatus==0 && GetComponent<Rigidbody>().angularVelocity.x==0) { GetComponent<Rigidbody>().AddForce(+5,  -0, 0); GetComponent<Rigidbody>().AddTorque(0,0,-500f); };
		}
		
		if (railStatus>=1) {railStatus+=Time.deltaTime;}
		
		if (railStatus>1.45f) {
			transform.rotation=new Quaternion(0,0,transform.rotation.z,transform.rotation.w);
			GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
			//rigidbody.velocity=new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0);
			railStatus=-1;
			if (rail==0) {
				transform.position=new Vector3(transform.position.x,transform.position.y,+4);
			}
			if (rail==1) {
				transform.position=new Vector3(transform.position.x,transform.position.y,+2);
			}
			if (rail==2) {
				transform.position=new Vector3(transform.position.x,transform.position.y,0);
			}
			if (rail==3) {
				transform.position=new Vector3(transform.position.x,transform.position.y,-2);
			}
			if (rail==4) {
				transform.position=new Vector3(transform.position.x,transform.position.y,-4);
			}
		}
		
		if ((transform.position.y<-10 || transform.position.x<-11) && alive) {
			Kill();
		}
	}
	
	void Kill() {
		alive=false;
		GetComponent<AudioSource>().clip=Death; GetComponent<AudioSource>().Play();
		TimeCounter.killCount++;
	}
	
	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			GetComponent<AudioSource>().clip=Poc; GetComponent<AudioSource>().Play();
			if (contact.point.y<transform.position.y) {
				jumpStatus=true;
			}
			if (railStatus==-1) {
				railStatus=0;
			}
		}
	}
	
	
	void OnCollisionStay(Collision collisionInfo) {
		//foreach (ContactPoint contact in collisionInfo.contacts) {
		//	Debug.DrawRay(contact.point, contact.normal * 10, Color.gray);
		//}
	}
	
	void OnTriggerEnter(Collider collider) {
		GetComponent<AudioSource>().clip=Coin; GetComponent<AudioSource>().Play();
		iTween.PunchScale(gameObject, new Vector3(0,0,0.5f), 1.5f);
		Destroy(collider.transform.gameObject);
		playerScore++;
		transform.localScale=new Vector3(0.5f+playerScore*0.1f,0.5f+playerScore*0.1f,transform.localScale.z);
	}
}
