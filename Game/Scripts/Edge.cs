using UnityEngine;
using System.Collections.Generic;

public class Edge {
	private GameObject start, end;
	private float distance;

	public Edge(GameObject start, GameObject end) {
		this.start = start;
		this.end = end;
		distance = Vector2.Distance(start.transform.position, end.transform.position);
	}

	public GameObject GetOpposite(GameObject g) {
		if (g.Equals(end)) {
			return start;
		} else if (g.Equals(start)) {
			return end;
		}
		return null;
	}

	public float Distance() {
		return distance;
	}

	public GameObject Start() {
		return start;
	}

	public GameObject End() {
		return end;
	}
}
