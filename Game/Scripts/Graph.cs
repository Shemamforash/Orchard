using UnityEngine;
using System.Collections.Generic;

public class Graph : MonoBehaviour{
    private static List<GameObject> nodes = new List<GameObject>();
    public GameObject startingTree;
    public GameObject treePrefab;

    void Start() {
        nodes.Add(startingTree);
    }

	public List<GameObject> Nodes() {
		return nodes;
	}

	public static void RemoveNode(Node n) {
		nodes.Remove(n.gameObject);
		//foreach (GameObject neighbor in n.Neighbors()) {
		//	Debug.Log(neighbor + " " + n);
		//	neighbor.GetComponent<Node>().RemoveNeighbor(n.gameObject);
		//}
		foreach (GameObject node in nodes) {
			List<GameObject> nodesToRemove = new List<GameObject>();
			foreach (GameObject nodeNeighbor in node.GetComponent<Node>().Neighbors()) {
				if (nodeNeighbor.Equals(n)) {
					nodesToRemove.Add(nodeNeighbor);
				}
			}
			foreach (GameObject g in nodesToRemove) {
				node.GetComponent<Node>().RemoveNeighbor(g);
			}
		}
		GameObject.Destroy(n.gameObject);
	}

    public bool Add(Vector2 position, List<GameObject> nearbyTrees) {
        if (nearbyTrees.Count > 0) {												//If there are nearby trees
            GameObject newNode = GameObject.Instantiate(treePrefab);				//Make a new node
			newNode.name = "Node " + nodes.Count;
			nodes.Add(newNode);
            newNode.GetComponent<Node>().SetPosition(position);						//Set its position to where we want it
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
}
