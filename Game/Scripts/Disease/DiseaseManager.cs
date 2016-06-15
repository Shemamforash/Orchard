using UnityEngine;
using System.Collections.Generic;

public class DiseaseManager {
	private List<GameObject> diseases = new List<GameObject>();
	private List<GameObject> visitedNodes = new List<GameObject>();

	public DiseaseManager(GameObject diseasePrefab, GameObject initialTarget, int initialValue) {
		GameObject initialDisease = GameObject.Instantiate(diseasePrefab);
		initialDisease.GetComponent<Disease>().SetTarget(this, initialTarget, initialValue);
		diseases.Add(initialDisease);
	}

	public void Empty() {
		foreach (GameObject d in diseases) {
			GameObject.Destroy(d);
		}
		diseases.Clear();
	}

	public void AddVisited(GameObject node) {
		visitedNodes.Add(node);
	}

	public bool NodeVisited(GameObject node) {
		return visitedNodes.Contains(node);
	}

	public void AddChildDiseases(GameObject parent, List<GameObject> children) {
		if (diseases.Remove(parent)) {
			diseases.AddRange(children);
			GameObject.Destroy(parent);
			Debug.Log(parent);
		} else {
			Debug.Log("Parent disease not found, children not added.");
		}
		if (diseases.Count == 0) {
			DiseaseFactory.Remove(this);
		}
	}
}
