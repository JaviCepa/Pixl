using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

public class PlayerController : MonoBehaviour {
	
	public RailStatus  railStatus = RailStatus.OnTrack;
	public int rail=2;

	private bool   alive = true;
	private bool   readyToRailChange = false;
	
	public KeyCode railLeftKey;
	public KeyCode railRightKey;
	public KeyCode accelKey;
	public KeyCode brakeKey;

	public AudioClip Coin, Death, Jump, Poc;

	public float power=700;
	public float initialRunSpeed=0;
	public float runAcceleration = 0.01f;
	public float fallSpeed=10;
	public float jumpPower=10;
	public float jumpBoost=1.2f;

	private Rigidbody rb;
	private AudioSource aus;
	public float maxAngularSpeed;
	private bool grounded;

	public GameObject squaredBody;
	public GameObject roundedBody;

	float currentSize = 0.5f;
	[HideInInspector]public bool isAlive = true;
	private float roundedTimer;
	private bool playing;

	float currentSpeed { get { return (Mathf.Max(0, initialRunSpeed + (Time.time-GameManager.gameStartTime) * runAcceleration)) * (isRounded ? 2f : 1f); } }
	bool isRounded { get { return roundedTimer>0; } }

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		aus = GetComponent<AudioSource>();
		rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
		playing = false;
	}

	private void Start()
	{
		SetSquared();
	}

	public void SetSquared() {
		squaredBody.SetActive(true);
		roundedBody.SetActive(false);
	}

	public void SetRounded()
	{
		squaredBody.SetActive(false);
		roundedBody.SetActive(true);
		roundedTimer = 5;
	}

	private void Update()
	{
		if (roundedTimer>0)
		{
			roundedTimer -= Time.deltaTime;
			if (roundedTimer<=0)
			{
				roundedTimer = 0;
				SetSquared();
			}
		}

	}

	void FixedUpdate ()
	{
		if (!playing && (Input.GetKeyDown(accelKey) || Input.GetKeyDown(brakeKey) || Input.GetKeyDown(railLeftKey) || Input.GetKeyDown(railRightKey))) {
			playing = true;
			GameManager.StartGame();
		}

		if (GameManager.isGameRunning)
		{
			rb.useGravity = true;
			rb.maxAngularVelocity = maxAngularSpeed;

			if (Input.GetKeyDown(accelKey) && alive && grounded && railStatus == RailStatus.OnTrack && rb.velocity.y < jumpPower*0.5f)
			{
				rb.velocity += new Vector3(rb.velocity.x*jumpBoost+0.2f, jumpPower, 0);
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

			if (!Input.GetKey(brakeKey) && alive && playing) {
				rb.AddTorque(0, 0, -power*transform.localScale.x*2f* currentSpeed);
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

		float forwardLook = rb.velocity.x * 1f;

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
		sequence.AppendCallback(() => { rb.velocity = Vector3.right*saveSpeed.x + Vector3.down * fallSpeed; rb.angularVelocity = saveAngular; });
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