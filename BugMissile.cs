using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMissile : MonoBehaviour
{
	public Transform target;
	public Rigidbody2D rigidBody;
	public float angleChangingSpeed = 3f;
	public float movementSpeed = 10f;
	public float stopAt = 0.01f;
	//1f = No variation
	public float randomVariation = 1f;
	public bool AIMFirstPosition = false;
	public bool AIMActive = false;

	private float angle;
	private Vector2 lastPosition;
	private Vector2 targetLastPosition;

	void Start() {
		if (target == null)
			target = FindObjectOfType<CharacterController2D>().transform;
		if (rigidBody == null)
			rigidBody = GetComponent<Rigidbody2D>();
		angleChangingSpeed = Random.Range(angleChangingSpeed * randomVariation * randomVariation, angleChangingSpeed);
		movementSpeed = Random.Range(movementSpeed * randomVariation, movementSpeed);
	}

    void FixedUpdate() {
		if (!AIMFirstPosition) {
			//Normal projectile
			Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
			angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
			transform.rotation = Quaternion.Euler(0f, 0f, angle);
			lastPosition = transform.position;
			if (Vector2.Distance(((Vector2)target.position), rigidBody.position) >= stopAt) {
				rigidBody.velocity = transform.up * movementSpeed;
			}
			else {
				rigidBody.velocity = -1 * transform.up * movementSpeed;
			}
		}
		else if (AIMActive) {
			//Delayed AIM
			Vector2 direction = (Vector2)targetLastPosition - (Vector2)transform.position;
			angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
			transform.rotation = Quaternion.Euler(0f, 0f, angle);
			//lastPosition = transform.position;
			if (Vector2.Distance(((Vector2)targetLastPosition), rigidBody.position) >= stopAt) {
				rigidBody.velocity = transform.up * movementSpeed;
			}
			else {
				//IDEA: Start as ghost and change layer after N seconds
				//NOTE: Try to keep moving forward upon reaching destination
				this.enabled = false;
				this.gameObject.layer = 10;
			}
		}
	}

	public void EnableAIM() {
		AIMActive = true;

		if (targetLastPosition == Vector2.zero) {
			if (target == null)
				target = FindObjectOfType<CharacterController2D>().transform;
			targetLastPosition = target.transform.position;
			targetLastPosition = new Vector2(targetLastPosition.x + Random.Range(-2f, 2f), 
				targetLastPosition.y + Random.Range(-1f, 1f));
		}

		GetComponent<Rigidbody2D>().gravityScale = 0f;
		GetComponent<Rigidbody2D>().angularDrag = 0f;
		GetComponent<Rigidbody2D>().drag = 0f;
	}
}
