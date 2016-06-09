using UnityEngine;
using System.Collections.Generic;
using System;

public static class PointManager {
	public static bool IsNodeExternal(Vector2 p) {
		List<GameObject> finalPath = GetExternalPath();
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
		return !oddNodes;
	}

	public static List<GameObject> GetExternalPath() {
		List<GameObject> finalPath = new List<GameObject>();
		GameObject start = null;
		foreach (GameObject g in Graph.Nodes()) {
			if (g.GetComponent<Node>().External) {
				start = g;
				break;
			}
		}
		if (start != null) {
			GameObject current = start;
			bool foundEdge = true;
			while (foundEdge) {
				foundEdge = false;
				foreach (Edge e in current.GetComponent<Node>().Neighbors()) {
					GameObject other = e.GetOpposite(current);
					if (other.GetComponent<Node>().External && !finalPath.Contains(other)) {
						finalPath.Add(current);
						current = other;
						foundEdge = true;
						break;
					}
				}
			}
		}
		String path = "Start: ";
		foreach (GameObject g in finalPath) {
			path += g.name + ", to";
		}
		//Debug.Log(path);

		return finalPath;
	}

	//public static float RotationOfLine(Vector2 point, Vector2 origin) {
	//	float xDifference = (float)Math.Round(point.x - origin.x, 3);
	//	float yDifference = (float)Math.Round(point.y - origin.y, 3);
	//	float angle = Mathf.Atan(yDifference / xDifference);
	//	angle = angle * Mathf.Rad2Deg;
	//	if (xDifference == 0f && yDifference > 0f) {//If x equals zero and y is positive the angle is 90 degrees (vertical up)
	//		angle = 90f;
	//	} else if (xDifference == 0f && yDifference < 0f) {//If x equals zero and y is negative the angle is 270 degrees (vertical down)
	//		angle = 270f;
	//	} else if (yDifference == 0f && xDifference < 0f) {//If y equals zero and x is negative the angle is 180 degrees (horizontal back)
	//		angle = 180f;
	//	} else if (yDifference == 0f && xDifference > 0f) {//If y equals zero and x is positive then angle is 360 degrees (horizontal forward
	//		angle = 0f;
	//	} else if (xDifference < 0f && yDifference > 0f) {//If x is negative and y is positive the angle is in the top left quadrant
	//		angle = 180f + angle;
	//	} else if (xDifference < 0f && yDifference < 0f) {//If x is negative and y is negative the angle is in the bottom left quadrant
	//		angle = 180f + angle;
	//	} else if (xDifference > 0f && yDifference < 0f) { //If x is positive and y is negative the angle is in the bottom right quadrant
	//		angle = 360f + angle;
	//	}
	//	return angle;
	//}
}
