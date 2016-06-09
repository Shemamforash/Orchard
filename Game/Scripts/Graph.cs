using UnityEngine;
using System.Collections.Generic;

public class Graph : MonoBehaviour{
    private List<GameObject> nodes = new List<GameObject>();
	private List<Edge> edges = new List<Edge>();
    public GameObject startingTree;
    public GameObject treePrefab;

    void Start() {
        nodes.Add(startingTree);
    }

    public bool Add(Vector2 position, List<GameObject> nearbyTrees) {
        if (nearbyTrees.Count > 0) {
            GameObject newNode = GameObject.Instantiate(treePrefab);
            newNode.GetComponent<Node>().SetPosition(position);

			List<Edge> newNeighbors = new List<Edge>();
            foreach(GameObject g in nearbyTrees){
				Edge e = new Edge(newNode, g);
				newNeighbors = newNode.GetComponent<Node>().SortNeighbors(e, newNeighbors);
            }

			if (PointManager.IsNodeInternal(newNode.transform.position, nodes)) {
				GameObject.Destroy(newNode);
				return false;
			}
			newNode.GetComponent<Node>().AddNeighbor(newNeighbors);

			edges.AddRange(newNeighbors);
            nodes.Add(newNode);
			newNode.name = "Node " + nodes.Count;

			PointManager.RecalculateExternalPoints(nodes);
        }
		return true;
    }

    public List<GameObject> NearestTrees(Vector3 position) {
        List<GameObject> nearestTrees = new List<GameObject>();
        foreach(GameObject tree in nodes){
            if (Vector2.Distance(position, tree.transform.position) < 5) {
                nearestTrees.Add(tree);
            }
        }
        if (nearestTrees.Count == 1) {
            return nearestTrees;
        } else {
            return ReduceNearestTrees(nearestTrees, position);
        }
    }

    private List<GameObject> ReduceNearestTrees(List<GameObject> nearestTrees, Vector2 position){
        List<GameObject> finalTrees = new List<GameObject>();
        float shortestDistance = 1000;
		for (int i = 0; i < edges.Count; ++i) {
			GameObject a = edges[i].Start();
			GameObject b = edges[i].End();
			float distance = Vector2.Distance(position, a.transform.position) + Vector2.Distance(position, b.transform.position);
			if (distance < shortestDistance) {
				shortestDistance = distance;
				finalTrees.Clear();
				finalTrees.Add(a);
				finalTrees.Add(b);
			}
		}
        return finalTrees;
    }
}
