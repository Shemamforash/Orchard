using UnityEngine;
using System.Collections.Generic;

public class Disease : MonoBehaviour {
	private GameObject target;
	private Vector2 origin;
	private float start, duration = 2f;
	private int value;
	public GameObject diseasePrefab;
	private bool lerping = true;
	private DiseaseManager manager;

	public void SetTarget(DiseaseManager manager, GameObject target, int value) {
		this.manager = manager;
		manager.AddVisited(target);
		this.target = target;
		this.value = value;
		Debug.Log(value);
		origin = transform.position;
		start = Time.time;
	}

	public GameObject Target() {
		return target;
	}

	public void Update () {
		if (!lerping && target != null) {
			if (value > 0) {
				SpawnChildren();
			} else {
				Debug.Log("Disease has target and has stopped moving but has no value.");
			}
		} else if (target != null) {
			float timePassed = (Time.time - start);
			float fracJourney = timePassed / duration;
			if (fracJourney + 0.01f >= 1f) {
				lerping = false;
				transform.position = target.transform.position;
			} else {
				transform.position = Vector3.Lerp(origin, target.transform.position, fracJourney);
			}
		} else {
			Debug.Log("Target is null.");
		}
	}

	private void SpawnChildren() {
		List<GameObject> neighbors = new List<GameObject>();
		target.GetComponent<Node>().ChangeLevelOffset(-1);
		foreach (GameObject t in target.GetComponent<Node>().Neighbors()) {
			if (DiseaseFactory.Targetable(t) && !manager.NodeVisited(t)) {
				neighbors.Add(t);
			}
		}
		List<GameObject> newDiseases = new List<GameObject>();
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
					if (value - newValue <= 0) {
						newValue = value;
					}
					newDisease.GetComponent<Disease>().SetTarget(manager, infectee, newValue);
					newDiseases.Add(newDisease);
					value -= newValue;
					--numberToInfect;
				}
			}
		}
		manager.AddChildDiseases(gameObject, newDiseases);
	}
}
