using UnityEngine;
using System.Collections;

public class SeedBehaviour : MonoBehaviour {
	private float timeAlive;
	
	void Update () {
		timeAlive += Time.deltaTime;
		float xOffset = Mathf.Cos(timeAlive) / 100;
		transform.position = new Vector2(transform.position.x + xOffset, transform.position.y + Time.deltaTime / 3);
		float alphaValue = 1- timeAlive / 10;
		if (alphaValue < 0) {
			GameObject.Destroy(gameObject);
		}
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alphaValue);
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
			if (hit != null && hit.collider != null && hit.collider.gameObject.Equals(gameObject)) {
				GameManager.AddSeed();
				GameObject.Destroy(gameObject);
			}
		}
	}
}
