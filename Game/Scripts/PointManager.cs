using UnityEngine;
using System.Collections.Generic;
using System;

public static class PointManager {
	public static void RecalculateExternalPoints(List<GameObject> points) {
		if (points.Count > 2) {
			foreach (GameObject p in points) {
				if (IsNodeInternal(p.transform.position, points)) {
					p.GetComponent<Node>().External = false;
				}
			}
		}
	}

	//public static List<NodeRotation> OrderPointsByRotation(List<GameObject> originalPoints, GameObject origin) {
	//	List<NodeRotation> orderedPoints = new List<NodeRotation>();
	//	foreach(GameObject g in originalPoints){
	//		if(!g.Equals(origin)){
	//			float rot = RotationOfLine(g.transform.position, origin.transform.position);
	//			if (orderedPoints.Count == 0) {
	//				orderedPoints.Add(new NodeRotation(rot, g));
	//				continue;
	//			}
	//			for (int i = 0; i < orderedPoints.Count; ++i) {
	//				if (orderedPoints[i].Rot() > rot) {
	//					orderedPoints.Insert(i, new NodeRotation(rot, g));
	//					break;
	//				}
	//			}
	//		}
	//	}
	//	return orderedPoints;
	//}

	//class NodeRotation {
	//	private GameObject g;
	//	private float rot;

	//	public NodeRotation(float rot, GameObject g) {
	//		this.g = g;
	//		this.rot = rot;
	//	}

	//	public GameObject G() {
	//		return g;
	//	}

	//	public float Rot() {
	//		return rot;
	//	}
	//}

	//public static bool IsNodeInternal(GameObject p, List<GameObject> nodes) {
	//	List<NodeRotation> orderedPoints = OrderPointsByRotation(nodes, p);
	//	double totalAngle = 0f;
	//	for (int i = 0; i < orderedPoints.Count; ++i) {
	//		int next = i + 1;
	//		if (next == orderedPoints.Count) {
	//			next = 0;
	//		}
	//		GameObject A = orderedPoints[i].G();
	//		GameObject B = orderedPoints[next].G();
	//		float originToA = Vector2.Distance(A.transform.position, p.transform.position);
	//		float originToB = Vector2.Distance(B.transform.position, p.transform.position);
	//		float aToB = Vector2.Distance(A.transform.position, B.transform.position);
	//		double numerator = Math.Pow(neighbors[i].Distance(), 2);
	//			numerator += Math.Pow(neighbors[next].Distance(), 2);
	//			numerator -= Math.Pow(aToB.Distance(), 2);

	//			double denominator = 2 * neighbors[i].Distance() * neighbors[next].Distance();

	//			double angle = numerator / denominator;
	//			angle = Math.Acos(angle);
	//			totalAngle += angle;
	//	}


		
			
			
	//		if (aToB != null) {
				
	//		}
	//	}
	//	totalAngle = Mathf.Rad2Deg * totalAngle;
	//	if (totalAngle < 360.00001 && totalAngle > 359.9999) {
	//		return true;
	//	}
	//	return false;
	//}

	public static bool IsNodeInternal(Vector2 p, List<GameObject> points) {
		List<GameObject> finalPath = GetExternalPath(points);
		int j = finalPath.Count - 1;
		bool oddNodes = false;
		for (int i = 0; i < finalPath.Count; ++i) {
			float iy = finalPath[i].transform.position.y;
			float ix = finalPath[i].transform.position.x;
			float jy = finalPath[j].transform.position.y;
			float jx = finalPath[j].transform.position.x;

			if ((iy < p.y && jy >= p.y) || (jy < p.y && iy >= p.y)) {
				if(ix <= p.x || jx <= p.x){
					float val = (ix + (p.y - iy) / (jy - iy) * (jx - ix));
					oddNodes = val < p.x;
				}
				j = i;
			}
		}
		return oddNodes;
	}

	static bool printed = false;

	public static List<GameObject> GetExternalPath(List<GameObject> points) {
		List<GameObject> finalPath = new List<GameObject>();
		GameObject start = null;
		foreach (GameObject g in points) {
			if (g.GetComponent<Node>().External) {
				start = g;
				break;
			}
		}
		if (start != null) {
			GameObject current = start;
			bool foundEdge = true;
			while (foundEdge) {
				foreach (Edge e in current.GetComponent<Node>().Neighbors()) {
					GameObject other = e.GetOpposite(current);
					if (other.GetComponent<Node>().External && !finalPath.Contains(other)) {
						finalPath.Add(current);
						current = other;
						continue;
					}
				}
				foundEdge = false;
			}
		}
		if (!printed) {
			foreach (GameObject g in finalPath) {
				Debug.Log(g);
			}
			if (finalPath.Count > 0) {
				printed = true;
			}
		}
		return finalPath;
	}

	//public static bool IsNodeInternal(GameObject p) {
	//	List<Edge> neighbors = p.GetComponent<Node>().Neighbors();
	//	double totalAngle = 0f;
	//	for (int i = 0; i < neighbors.Count; ++i) {
	//		int next = i + 1;
	//		if (next == neighbors.Count) {
	//			next = 0;
	//		}
	//		GameObject endA = neighbors[i].GetOpposite(p);
	//		GameObject endB = neighbors[next].GetOpposite(p);
	//		Edge aToB = endA.GetComponent<Node>().ShareEdge(endB);
	//		if (aToB != null) {
	//			double numerator = Math.Pow(neighbors[i].Distance(), 2);
	//			numerator += Math.Pow(neighbors[next].Distance(), 2);
	//			numerator -= Math.Pow(aToB.Distance(), 2);

	//			double denominator = 2 * neighbors[i].Distance() * neighbors[next].Distance();

	//			double angle = numerator / denominator;
	//			angle = Math.Acos(angle);
	//			totalAngle += angle;
	//		}
	//	}
	//	totalAngle = Mathf.Rad2Deg * totalAngle;
	//	if (totalAngle < 360.00001 && totalAngle > 359.9999) {
	//		return true;
	//	}
	//	return false;
	//}

	public static float RotationOfLine(Vector2 point, Vector2 origin) {
		float xDifference = (float)Math.Round(point.x - origin.x, 3);
		float yDifference = (float)Math.Round(point.y - origin.y, 3);
		float angle = Mathf.Atan(yDifference / xDifference);
		angle = angle * Mathf.Rad2Deg;
		if (xDifference == 0f && yDifference > 0f) {//If x equals zero and y is positive the angle is 90 degrees (vertical up)
			angle = 90f;
		} else if (xDifference == 0f && yDifference < 0f) {//If x equals zero and y is negative the angle is 270 degrees (vertical down)
			angle = 270f;
		} else if (yDifference == 0f && xDifference < 0f) {//If y equals zero and x is negative the angle is 180 degrees (horizontal back)
			angle = 180f;
		} else if (yDifference == 0f && xDifference > 0f) {//If y equals zero and x is positive then angle is 360 degrees (horizontal forward
			angle = 0f;
		} else if (xDifference < 0f && yDifference > 0f) {//If x is negative and y is positive the angle is in the top left quadrant
			angle = 180f + angle;
		} else if (xDifference < 0f && yDifference < 0f) {//If x is negative and y is negative the angle is in the bottom left quadrant
			angle = 180f + angle;
		} else if (xDifference > 0f && yDifference < 0f) { //If x is positive and y is negative the angle is in the bottom right quadrant
			angle = 360f + angle;
		}
		return angle;
	}
}
