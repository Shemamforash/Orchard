using UnityEngine;
using System.Collections.Generic;

public static class DiseaseManager {
	private static List<Disease> activeDiseases = new List<Disease>();

	public static void Add(Disease d) {
		activeDiseases.Add(d);
	}

	public static void Remove(Disease d) {
		activeDiseases.Remove(d);
		GameObject.Destroy(d.gameObject);
	}

	public static bool Targetable(Node n) {
		int targetCount = 0;
		foreach (Disease d in activeDiseases) {
			if (d.Target().GetComponent<Node>().Equals(n)) {
				++targetCount;
			}
		}
		if (targetCount == n.GetNodeValue() + 1) {
			return false;
		}
		return true;
	}

	public static void CreateDisease(GameObject diseasePrefab) {
		GameObject newDisease = GameObject.Instantiate(diseasePrefab);
		newDisease.GetComponent<Disease>().FindTarget();
	}
}
