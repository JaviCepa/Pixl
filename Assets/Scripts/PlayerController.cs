using UnityEngine;
using System.Collections;
using DG.Tweening;


public class PlayerController : MonoBehaviour {
	
	public RailStatus  railStatus = RailStatus.OnTrack;
	public int rail=2;

	private bool   alive = true;
	private int    playerScore=0;
	private bool   readyToRailChange = false;

	public  GameObject fader;
	
	public KeyCode railLeftKey;
	public KeyCode railRightKey;
	public KeyCode accelKey;
	public KeyCode brakeKey;

	public AudioClip Coin, Death, Jump, Poc;

	public float power=700;
	public float airPower=50;
	public float fallSpeed=10;

	float jumpTime=0.5f;

	private Rigidbody rb;
	private AudioSource aus;
	public float maxAngularSpeed;
	private bool grounded;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		aus = GetComponent<AudioSource>();
		rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
	}

	void FixedUpdate ()
	{
		if (GameManager.gameStart)
		{
			rb.useGravity = true;
			rb.maxAngularVelocity = maxAngularSpeed;

			if (Input.GetKeyDown(accelKey) && alive && grounded && railStatus == RailStatus.OnTrack)
			{
				rb.AddForce(0, +500f, 0);
				grounded = false;
				aus.clip = Jump;
				aus.Play();
			}

			if (Input.GetKey(railLeftKey) && readyToRailChange && railStatus == RailStatus.OnTrack && rail > 0 && alive)
			{
				readyToRailChange = false;
				grounded = false;
				var saveSpeed = rb.velocity;
				var saveAngular = rb.angularVelocity;
				rb.isKinematic = true;
				railStatus = RailStatus.Jumping;
				aus.clip = Jump;
				aus.Play();
				var sequence = DOTween.Sequence();
				sequence.Append(transform.DOMoveY(transform.position.y + 1f, 0.5f * jumpTime).SetEase(Ease.OutQuad));
				sequence.Append(transform.DOMoveY(transform.position.y, 0.5f * jumpTime).SetEase(Ease.InQuad));
				sequence.AppendCallback(() => rail--);
				sequence.AppendCallback(() => { rb.isKinematic = false; railStatus = RailStatus.OnTrack; });
				sequence.AppendCallback(() => { rb.velocity = saveSpeed + Vector3.down*fallSpeed; rb.angularVelocity = saveAngular; });
				sequence.Insert(0, transform.DOMoveZ(2f, 1f * jumpTime).SetEase(Ease.Linear).SetRelative(true));
				sequence.Insert(0, transform.DOMoveX(transform.position.x + jumpTime * saveSpeed.x, 1f * jumpTime).SetEase(Ease.Linear));
				sequence.Insert(0, transform.DORotate(Vector3.right * 180f, 1f * jumpTime, RotateMode.WorldAxisAdd).SetEase(Ease.InOutSine).SetRelative(true));
			};
	
			if (Input.GetKey(railRightKey) && readyToRailChange && railStatus == RailStatus.OnTrack && rail < 4 && alive)
			{
				readyToRailChange = false;
				grounded = false;
				var saveSpeed = rb.velocity;
				var saveAngular = rb.angularVelocity;
				rb.isKinematic = true;
				railStatus = RailStatus.Jumping;
				aus.clip=Jump;
				aus.Play();
				var sequence = DOTween.Sequence();
				sequence.Append(transform.DOMoveY(transform.position.y + 1f, 0.5f * jumpTime).SetEase(Ease.OutQuad));
				sequence.Append(transform.DOMoveY(transform.position.y, 0.5f * jumpTime).SetEase(Ease.InQuad));
				sequence.AppendCallback(() => rail++);
				sequence.AppendCallback(() => { rb.isKinematic = false; railStatus = RailStatus.OnTrack; });
				sequence.AppendCallback(() => { rb.velocity = saveSpeed + Vector3.down * fallSpeed; rb.angularVelocity = saveAngular; });
				sequence.Insert(0, transform.DOMoveZ(-2f, 1f * jumpTime).SetEase(Ease.Linear).SetRelative(true));
				sequence.Insert(0, transform.DOMoveX(transform.position.x + jumpTime * saveSpeed.x, 1f * jumpTime).SetEase(Ease.Linear));
				sequence.Insert(0, transform.DORotate(Vector3.left * 180f, 1f * jumpTime, RotateMode.WorldAxisAdd).SetEase(Ease.InOutSine).SetRelative(true));
			};
			
			//if (Input.GetKey(brakeKey) && alive) {
			//	rb.AddForce(-airPower, 0, 0);
			//	rb.AddTorque(0, 0, +power);
			//};

			if (alive) {
				rb.AddForce(airPower, 0, 0);
				rb.AddTorque(0, 0, -power);
			};
		}
		
		if (railStatus == RailStatus.OnTrack)
		{
			
			if (rail==0)
			{
				transform.position=new Vector3(transform.position.x,transform.position.y,+4);
			}
			if (rail==1)
			{
				transform.position=new Vector3(transform.position.x,transform.position.y,+2);
			}
			if (rail==2)
			{
				transform.position=new Vector3(transform.position.x,transform.position.y,0);
			}
			if (rail==3)
			{
				transform.position=new Vector3(transform.position.x,transform.position.y,-2);
			}
			if (rail==4)
			{
				transform.position=new Vector3(transform.position.x,transform.position.y,-4);
			}
		}
		
		if ((transform.position.y<-10 || transform.position.x<-11) && alive)
		{
			Kill();
		}
	}
	
	void Kill() {
		alive=false;
		aus.clip=Death;
		aus.Play();
		GameManager.killCount++;
	}
	
	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			aus.clip=Poc;
			aus.Play();
			if (contact.point.y<transform.position.y) {
				grounded=true;
				readyToRailChange = true;
			}
		}
	}
	
	void OnTriggerEnter(Collider collider)
	{
		aus.clip=Coin;
		aus.Play();
		//iTween.PunchScale(gameObject, new Vector3(0,0,0.5f), 1.5f);
		Destroy(collider.transform.gameObject);
		playerScore++;
		transform.localScale=new Vector3(0.5f+playerScore*0.1f,0.5f+playerScore*0.1f,transform.localScale.z);
	}
}

public enum RailStatus { OnTrack, Jumping }