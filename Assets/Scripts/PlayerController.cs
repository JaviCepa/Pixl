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
	public float jumpPower=10;

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

			if (Input.GetKeyDown(accelKey) && alive && grounded && railStatus == RailStatus.OnTrack && rb.velocity.y < jumpPower*0.5f)
			{
				rb.velocity += new Vector3(0, jumpPower, 0);
				grounded = false;
				aus.clip = Jump;
				aus.Play();
			}

			if (Input.GetKey(railLeftKey) && readyToRailChange && railStatus == RailStatus.OnTrack && rail > 0 && alive)
			{
				ChangeRail(1);
			};
	
			if (Input.GetKey(railRightKey) && readyToRailChange && railStatus == RailStatus.OnTrack && rail < 4 && alive)
			{
				ChangeRail(-1);
			};
			
			//if (Input.GetKey(brakeKey) && alive) {
			//	rb.AddForce(-airPower, 0, 0);
			//	rb.AddTorque(0, 0, +power);
			//};

			if (alive) {
				rb.AddForce(airPower, 0, 0);
				rb.AddTorque(0, 0, -power*transform.localScale.x*2f);
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

	void ChangeRail(int direction) {
		float maxUp = 1f;
		float maxDist = 10f;
		Ray ray = new Ray(transform.position + Vector3.forward * 2 * direction + Vector3.up * maxUp, Vector3.down);
		RaycastHit hit;
		bool success = Physics.Raycast(ray, out hit, maxDist);
		Debug.DrawLine(ray.origin, success ? hit.point : ray.origin+ ray.direction*maxDist, success ? Color.green : Color.cyan, 100);
		if (success)
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
			float animationSpeedFactor=0.15f;
			float apexHeight=Mathf.Max(hit.point.y + 2f, transform.position.y+2);
			float jumpHeight=apexHeight-transform.position.y;
			float jumpUpTime=jumpHeight*animationSpeedFactor;
			float jumpDownTime=(apexHeight - hit.point.y)*animationSpeedFactor;
			float jumpTime=jumpUpTime+jumpDownTime;
			sequence.Append(transform.DOMoveY(apexHeight, jumpUpTime).SetEase(Ease.OutQuad));
			sequence.Append(transform.DOMoveY(hit.point.y + 0.5f, jumpDownTime).SetEase(Ease.InQuad));

			sequence.AppendCallback(() => rail-=direction);
			sequence.AppendCallback(() => { rb.isKinematic = false; railStatus = RailStatus.OnTrack; });
			sequence.AppendCallback(() => { rb.velocity = saveSpeed + Vector3.down * fallSpeed; rb.angularVelocity = saveAngular; });
			sequence.Insert(0, transform.DOMoveZ(2f * direction, jumpTime).SetEase(Ease.InOutQuad).SetRelative(true));
			sequence.Insert(0, transform.DORotate(Vector3.right * 180f * direction, jumpTime, RotateMode.WorldAxisAdd).SetEase(Ease.OutBack).SetRelative(true));
			sequence.Insert(0, transform.DOMoveX(transform.position.x + jumpTime * saveSpeed.x, jumpTime).SetEase(Ease.Linear));
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