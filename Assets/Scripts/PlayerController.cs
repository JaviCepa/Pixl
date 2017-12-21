using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

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
	public float jumpBoost=1;

	private Rigidbody rb;
	private AudioSource aus;
	public float maxAngularSpeed;
	private bool grounded;

	float currentSize = 0.5f;

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
				rb.velocity += new Vector3(jumpBoost, jumpPower, 0);
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

			if (!Input.GetKey(brakeKey) && alive) {
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

	void ChangeRail(int direction)
	{
		float maxUp = 3f;
		float maxDist = 6f;
		var saveSpeed = rb.velocity;
		var saveAngular = rb.angularVelocity;
		int dist = 2;
		float forwardLook = 1.5f;

		Ray ray = new Ray(transform.position + Vector3.forward * dist * direction + Vector3.up * maxUp + Vector3.right * (forwardLook), Vector3.down);
		RaycastHit hit;
		bool success = Physics.Raycast(ray, out hit, maxDist);
		
		Vector3 landingSpot = Vector3.zero;
		if (success)
		{
			landingSpot = hit.point;
		}
		else
		{
			dist = 4;
			ray = new Ray(transform.position + Vector3.forward * dist * direction + Vector3.up * maxUp + Vector3.right * (forwardLook), Vector3.down);
			success = Physics.Raycast(ray, out hit, maxDist);
			if (success)
			{
				landingSpot = hit.point;
			}
			else
			{
				dist = 6;
				ray = new Ray(transform.position + Vector3.forward * dist * direction + Vector3.up * maxUp + Vector3.right * (forwardLook), Vector3.down);
				success = Physics.Raycast(ray, out hit, maxDist);
				if (success)
				{
					landingSpot = hit.point;
				}
			}
		}

		if (!success)
		{
			landingSpot = transform.position + Vector3.forward * 2 * direction;
			dist = 2;
		}

		Debug.DrawLine(transform.position, landingSpot, success ? Color.green : Color.red, 100);

		readyToRailChange = false;
		grounded = false;
		rb.isKinematic = true;
		railStatus = RailStatus.Jumping;
		aus.clip = Jump;
		aus.Play();

		float animationSpeedFactor=0.2f;
		float elevation = 1.5f;
		float apexHeight=Mathf.Max(landingSpot.y + elevation, transform.position.y+elevation);
		float jumpHeight=apexHeight-transform.position.y;
		float jumpUpTime=jumpHeight*animationSpeedFactor;
		float jumpDownTime=(apexHeight - landingSpot.y)*animationSpeedFactor;
		float jumpTime=jumpUpTime+jumpDownTime;

		var sequence = DOTween.Sequence();
		sequence.Append(transform.DOMoveY(apexHeight, jumpUpTime).SetEase(Ease.OutQuad));
		sequence.Append(transform.DOMoveY(landingSpot.y + 0.5f, jumpDownTime).SetEase(Ease.InQuad));
		sequence.AppendCallback(() => rail -= direction * (dist / 2));
		sequence.AppendCallback(() => { rb.isKinematic = false; railStatus = RailStatus.OnTrack; });
		sequence.AppendCallback(() => { rb.velocity = saveSpeed + Vector3.down * fallSpeed; rb.angularVelocity = saveAngular; });
		sequence.Insert(0, transform.DOMoveZ(dist * direction, jumpTime).SetEase(Ease.OutQuad).SetRelative(true));
		sequence.Insert(0, transform.DORotate(Vector3.right * 180f * direction, jumpTime, RotateMode.WorldAxisAdd).SetEase(Ease.OutBack).SetRelative(true));
		sequence.Insert(0, transform.DOMoveX(landingSpot.x, jumpTime).SetEase(Ease.Linear));

	}

	public void Grow()
	{
		currentSize += 0.1f;

		transform.DOScaleX(currentSize, 0.3f).SetEase(Ease.OutBack);
		transform.DOScaleY(currentSize, 0.3f).SetEase(Ease.OutBack);
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
	
}

public enum RailStatus { OnTrack, Jumping }