using UnityEngine;
using System.Collections.Generic;

public class Graph : MonoBehaviour{
    private List<GameObject> nodes = new List<GameObject>();
    public GameObject startingTree;
    public GameObject treePrefab;

    void Start() {
        nodes.Add(startingTree);
    }

	public List<GameObject> Nodes() {
		return nodes;
	}

	public void Remove(GameObject n) {
		nodes.Remove(n.gameObject);
		List<GameObject> nodesToRemove = new List<GameObject>();
		foreach (GameObject node in nodes) {
			List<GameObject> neighbors = new List<GameObject>();
			neighbors.AddRange(node.GetComponent<Node>().Neighbors());
			foreach (GameObject nodeNeighbor in neighbors) {
				if (nodeNeighbor.Equals(n)) {
					nodesToRemove.Add(nodeNeighbor);
				}
			}
			foreach (GameObject g in nodesToRemove) {
				node.GetComponent<Node>().RemoveNeighbor(g);
			}
			nodesToRemove.Clear();
		}
		GameObject.Destroy(n.gameObject);
		CheckBadConnections();
	}

	public void CheckBadConnections() {
		List<GameObject> nodesToRemove = new List<GameObject>();
		foreach (GameObject node in nodes) {
			if (!ConnectedToHometree(node)) {
				nodesToRemove.Add(node);
			}
		}
		nodesToRemove.ForEach(p => Remove(p));
	}

    public bool Add(Vector2 position, List<GameObject> nearbyTrees) {
        if (nearbyTrees.Count > 0) {												//If there are nearby trees
            GameObject newNode = GameObject.Instantiate(treePrefab);				//Make a new node
			newNode.name = "Node " + nodes.Count;
			nodes.Add(newNode);
            newNode.GetComponent<Node>().SetPosition(this, position);						//Set its position to where we want it
			newNode.GetComponent<Node>().CreateConnectionWithNeighbors(nearbyTrees);
        }
		return true;
    }

    public List<GameObject> NearestTrees(Vector3 position, float maxDistance, float minDistance) {
        List<GameObject> nearestTrees = new List<GameObject>();
        foreach(GameObject tree in nodes){
			float distance = Vector2.Distance(position, tree.transform.position);
			if (distance < maxDistance && distance > minDistance) {
                nearestTrees.Add(tree);
			} else if (distance <= minDistance) {
				nearestTrees.Clear();
				break;
			}
        }
        return nearestTrees;
    }

	private bool ConnectedToHometree(GameObject node) {
		Queue<GameObject> q = new Queue<GameObject>();
		List<GameObject> visited = new List<GameObject>();
		visited.Add(node);
		q.Enqueue(node);
		while (q.Count > 0) {
			GameObject c = q.Dequeue();
			foreach (GameObject g in c.GetComponent<Node>().Neighbors()) {
				if (g.Equals(startingTree)) {
					return true;
				}
				if (!visited.Contains(g)) {
					visited.Add(g);
					q.Enqueue(g);
				}
			}
		}
		return false;
	}
}
