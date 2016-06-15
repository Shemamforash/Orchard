using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour {
	//private Neighbors neighbors = new Neighbors();
    public int locationValue = 0, levelOffset = -1, nodeValue = 0;
	public bool isHometree = false;
	public GameObject connector, seedPrefab;
	private GameObject seed = null;
	private List<GameObject> neighbors = new List<GameObject>();
	public float time = 0;
	private TreeType type;

	public void Start() {
		UpdateType();
	}

    private enum TreeType {
        SAPLING = 0,
        FRUIT = 1,
        PINE = 2,
        OAK = 3,
        REDWOOD = 4,
        EBONY = 5
    }

    private void TemporaryBackgroundUpdate() {
        Color[] colors = new Color[] {Color.green, Color.red, Color.grey, Color.yellow, Color.cyan, Color.black };
        gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().color = colors[(int)type];
    }

	public void Update() {
		if (type == TreeType.EBONY) {
			if (seed == null) {
				time += Time.deltaTime;
			}
			if (time >= 1) {
				MakeSeed();
				time = 0;
			}
		}
		UpdateType();
	}

	private void MakeSeed() {
		seed = GameObject.Instantiate(seedPrefab);
		Vector3 seedPosition = new Vector3(transform.position.x - transform.localScale.x / 10, transform.position.y + 0.75f);
		seed.transform.position = seedPosition;
	}

	public int GetNodeValue() {
		return nodeValue;
	}

    private void UpdateType() {
		if (!isHometree) {
			int sum = locationValue + levelOffset;
			if (sum > 5) {
				sum = 5;
			}
			if (sum < 0) {
				Graph.RemoveNode(this);
			} else {
				nodeValue = sum;
				type = (TreeType)(sum);
				TemporaryBackgroundUpdate();
			}
		}
    }

    public void SetPosition(Vector2 position) {
        type = TreeType.SAPLING;
        transform.position = position;
    }

	public void AddNeighbor(GameObject node) {
		neighbors.Add(node);
		++locationValue;
		UpdateType();
	}

	public void RemoveNeighbor(GameObject node) {
		if (neighbors.Remove(node)) {
			--locationValue;
			UpdateType();
		}
	}

    public void CreateConnectionWithNeighbors(List<GameObject> nodes) {
		foreach (GameObject g in nodes) {
			AddNeighbor(g);
			g.GetComponent<Node>().AddNeighbor(gameObject);
			CreateConnector(g);
		}
    }

	private void CreateConnector(GameObject other) {
		GameObject newConnector = GameObject.Instantiate(connector);
		newConnector.GetComponent<ConnectorBehaviour>().SetEnds(gameObject, other);
		LineRenderer lr = newConnector.GetComponent<LineRenderer>();
		lr.SetVertexCount(2);
		lr.SetPosition(0, other.transform.position);
		lr.SetPosition(1, transform.position);
	}

	public int NumberOfNeighbors() {
		return neighbors.Count;
	}

    public void ChangeLevelOffset(int amount) {
        levelOffset += amount;
        UpdateType();
    }

	public List<GameObject> Neighbors() {
		return neighbors;
	}
}
