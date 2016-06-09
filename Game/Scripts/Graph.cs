using UnityEngine;
using System.Collections.Generic;

public class Graph : MonoBehaviour{
    private static List<GameObject> nodes = new List<GameObject>();
	private List<Edge> edges = new List<Edge>();
    public GameObject startingTree;
    public GameObject treePrefab;

    void Start() {
        nodes.Add(startingTree);
    }

	public static List<GameObject> Nodes(){
		return nodes;
	}

    public bool Add(Vector2 position, List<GameObject> nearbyTrees) {
        if (nearbyTrees.Count > 0) {												//If there are nearby trees
            GameObject newNode = GameObject.Instantiate(treePrefab);				//Make a new node
			newNode.name = "Node " + nodes.Count;
            newNode.GetComponent<Node>().SetPosition(position);						//Set its position to where we want it

			List<Edge> newNeighbors = new List<Edge>();								//Make a list of the potential edges it can be a part of
            foreach(GameObject g in nearbyTrees){									//For each nearby tree
				Edge e = new Edge(newNode, g);										//Make a temporary edge
				newNeighbors.Add(e);
            }

			if (!PointManager.IsNodeExternal(newNode.transform.position)) {
				GameObject.Destroy(newNode);
				return false;
			}

			foreach (Edge e in newNeighbors) {
				newNode.GetComponent<Node>().CreateConnectionWithNeighbor(e);
				GameObject wtf = GameObject.Instantiate(treePrefab);
				wtf.name = e.Start() + " " + e.End();
				wtf.transform.parent = newNode.transform;
			}
			edges.AddRange(newNeighbors);
            nodes.Add(newNode);
        }
		UpdateExternalNodes();
		return true;
    }

	private void UpdateExternalNodes() {
		foreach (GameObject g in nodes) {
			g.GetComponent<Node>().External = PointManager.IsNodeExternal(g.transform.position);
		}
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
