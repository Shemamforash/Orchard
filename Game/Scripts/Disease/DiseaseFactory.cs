using UnityEngine;
using System.Collections.Generic;

public static class DiseaseFactory {
	private static List<DiseaseManager> activeDiseases = new List<DiseaseManager>();

	public static bool DiseaseActive() {
		if (activeDiseases.Count > 0) {
			return true;
		}
		return false;
	}

	public static void Add(DiseaseManager d) {
		activeDiseases.Add(d);
	}

	public static void Remove(DiseaseManager d) {
		activeDiseases.Remove(d);
		d.Empty();
		d = null;
	}

	public static bool Targetable(GameObject n) {
		int targetCount = 0;
		foreach (DiseaseManager d in activeDiseases) {
			if (d.NodeVisited(n)) {
				++targetCount;
			}
		}
		if (n == null || targetCount == n.GetComponent<Node>().GetNodeValue() + 1) {
			return false;
		}
		return true;
	}

	public static void CreateDisease(GameObject diseasePrefab) {
		GameObject initialTarget = FindTarget();
		if (initialTarget != null) {
			float squaredOffset = Mathf.Pow(Random.value, 2);
			int initialValue = (int)(squaredOffset * (float)Random.Range(0, 35)) + 5;
			DiseaseManager manager = new DiseaseManager(diseasePrefab, initialTarget, initialValue);
			activeDiseases.Add(manager);
		}
	}


	private static GameObject FindTarget() {
		int width = Screen.width;
		int height = Screen.height;
		int shortestDistance = 10000;
		GameObject target = null;
		foreach (GameObject node in Camera.main.GetComponent<Graph>().Nodes()) {
			if (Targetable(node)) {
				Vector2 screenPosition = Camera.main.WorldToScreenPoint(node.transform.position);
				int minY = (int)Mathf.Min(height - screenPosition.y, screenPosition.y);
				int minX = (int)Mathf.Min(width - screenPosition.x, screenPosition.x);
				int absoluteMin = (int)Mathf.Min(minY, minX);
				if (absoluteMin < shortestDistance) {
					shortestDistance = absoluteMin;
					target = node;
				}
			}
		}
		return target;
	}
}
