using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceWave : MonoBehaviour
{
	public Vector2 direction;
	
	public float speed = 3f;
	public float damage = 1f;
	public GameObject HitEffect;
	public float Duration = 2f;

	//private List<GameObject> _impacted;

	void Start() {
		if (direction != Vector2.right) {
			transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
		}
		Destroy(gameObject, Duration);
		//_impacted = new List<GameObject>();
	}

    void Update() {
		transform.Translate(direction * speed * Time.deltaTime);    
    }

    /*void FixedUpdate() {
		GetComponent<Rigidbody2D>().velocity = direction * speed;
	}*/


    private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Enemy") {
			collision.gameObject.SendMessage("ApplyDamage", damage);
			Destroy(Instantiate(HitEffect, collision.gameObject.transform.position, Quaternion.identity), 2f);
		}
	}
}
