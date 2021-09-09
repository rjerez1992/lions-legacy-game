using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
	private bool hasFloorInFront;
	private bool hasWallInFront;
	private Transform fallCheck;
	private Transform wallCheck;
	public LayerMask turnLayerMask;
	public LayerMask wallLayerMask;
	private Rigidbody2D rb;

	public bool facingRight = false;
	public float speed = 5f;
	public bool TurnOnEdge = false;
	public bool FlipOnStart = false;
	public bool FlipWhenStuck = true;

	public bool isInvincible = false;
	private bool isHitted = false;

	public UnityEvent OnStuck;
	public UnityEvent OnFlip;

	public bool FollowTarget = false;
	private bool DisableFollow = false;

    void Start() {
		StartCoroutine(CheckFollow());
    }

    void Awake() {
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody2D>();
		if (FlipOnStart)
			Flip();
	}

	void FixedUpdate() {
		hasFloorInFront = Physics2D.OverlapCircle(fallCheck.position, .2f, wallLayerMask);
		hasWallInFront = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);

		//Debug.Log($"Floor? {hasFloorInFront} Wall? {hasWallInFront}");

		if (!isHitted && !IsFalling()) {
			if (hasWallInFront && IsStuck()) {
				OnStuck.Invoke();
				if (FlipWhenStuck) {
					Flip();
				}
			}
			else if (hasFloorInFront && !hasWallInFront) {
				if (facingRight) {
					rb.velocity = new Vector2(speed, rb.velocity.y);
				}
				else {
					rb.velocity = new Vector2(-speed, rb.velocity.y);
				}
			}
			else if (TurnOnEdge && !hasWallInFront) {
				Flip();
			}
		}
	}

	public bool IsFalling() {
		return Mathf.Abs(rb.velocity.y) >= 0.5f;
	}

	public bool IsStuck() {
	    return Mathf.Abs(rb.velocity.x) <= 0;
	}

	public void DisableMovement() {
		this.enabled = false;
		this.rb.velocity = Vector2.zero;
		this.rb.isKinematic = true;
		this.rb.angularVelocity = 0f;
	}

	public void EnableMovement() {
		//NOTE: Be carefull, maybe it was never dynamic
		this.rb.isKinematic = false;
		this.enabled = true;
	}

	public void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		OnFlip.Invoke();
	}

	private IEnumerator CheckFollow() {
		GameObject target = FindObjectOfType<CharacterController2D>().gameObject;
		while (!DisableFollow) {
			yield return new WaitForSeconds(2f);
			if (FollowTarget && target != null && !IsTargetInFront(target)) {
				Flip();
			}
		}
	}

	private void StopFollow() {
		this.DisableFollow = true;
	}

	private bool IsTargetInFront(GameObject target) {
		Vector2 relativePoint = transform.InverseTransformPoint(target.transform.position);
		if (relativePoint.x < 0.0)
			return true;
		else
			return false;
	}
}
