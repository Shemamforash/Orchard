using UnityEngine;
using System.Collections.Generic;

public class Disease : MonoBehaviour {
	private GameObject target;
	private Vector2 origin;
	private float start, duration = 2f;
	private int value;
	public GameObject diseasePrefab;
	private bool lerping = true;
	private static List<GameObject> allTargets = new List<GameObject>();

	private void SetTarget(GameObject target, int value) {
		this.target = target;
		this.value = value;
		origin = transform.position;
		start = Time.time;
	}

	public void Update () {
		if (!lerping) {
			target.GetComponent<Node>().ChangeLevelOffset(-1);
			if (value > 0) {
				SpawnChildren();
				GameObject.Destroy(gameObject);
			}
		} else {
			float timePassed = (Time.time - start);
			float fracJourney = timePassed / duration;
			if (fracJourney + 0.01f >= 1f) {
				lerping = false;
				transform.position = target.transform.position;
			} else {
				transform.position = Vector3.Lerp(origin, target.transform.position, fracJourney);
			}
		}
	}

	private void SpawnChildren() {
		List<GameObject> neighbors = new List<GameObject>();
		foreach (GameObject t in target.GetComponent<Node>().Neighbors()) {
			if (!allTargets.Contains(t)) {
				neighbors.Add(t);
				Disease.allTargets.Add(t);
			}
		}
		if (neighbors.Count > 0) {
			int numberToInfect = Random.Range(1, neighbors.Count);
			if (numberToInfect > value) {
				numberToInfect = value;
			}
			int newValue = (int)Mathf.Ceil((float)value / (float)numberToInfect);
			while (numberToInfect > 0) {
				if (neighbors.Count > 0) {
					int val = Random.Range(0, neighbors.Count - 1);
					GameObject infectee = neighbors[val];
					neighbors.Remove(infectee);
					GameObject newDisease = GameObject.Instantiate(diseasePrefab);
					if (value - newValue < 0) {
						newValue = value;
					}
					newDisease.GetComponent<Disease>().SetTarget(infectee, newValue);
					value -= newValue;
					--numberToInfect;
				}
			}
		}
	}

	public void FindTarget() {
		int width = Screen.width;
		int height = Screen.height;
		int shortestDistance = 10000;
		GameObject target = null;
		foreach (GameObject node in Camera.main.GetComponent<Graph>().Nodes()) {
			Vector2 screenPosition = Camera.main.WorldToScreenPoint(node.transform.position);
			int minY = (int)Mathf.Min(height - screenPosition.y, screenPosition.y);
			int minX = (int)Mathf.Min(width - screenPosition.x, screenPosition.x);
			int absoluteMin = (int)Mathf.Min(minY, minX);
			if (absoluteMin < shortestDistance) {
				shortestDistance = absoluteMin;
				target = node;
			}
		}
		if (target != null) {
			float squaredOffset = Mathf.Pow(Random.value, 2);
			int initialValue = (int)(squaredOffset * (float)Random.Range(5, 40));
			SetTarget(target, initialValue);
		}
	}
}
