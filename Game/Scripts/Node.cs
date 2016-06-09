using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour {
	//private Neighbors neighbors = new Neighbors();
    public int locationValue = 0, levelOffset = -1;
	public GameObject connector;
	private bool external = true;
	private List<Edge> neighbors = new List<Edge>();

	public bool External {
		get {
			return external;
		}
		set {
			external = value;
		}
	}

	public Edge ShareEdge(GameObject other) {
		foreach (Edge e in neighbors) {
			if (e.GetOpposite(gameObject).Equals(other)) {
				return e;
			}
		}
		return null;
	}

	public List<Edge> Neighbors() {
		return neighbors;
	}

    private enum TreeType {
        SAPLING = 0,
        FRUIT = 1,
        PINE = 2,
        OAK = 3,
        REDWOOD = 4,
        EBONY = 5
    }

    private TreeType type;

    private void TemporaryBackgroundUpdate() {
        Color[] colors = new Color[] {Color.green, Color.red, Color.grey, Color.yellow, Color.cyan, Color.black };
        gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().color = colors[(int)type];
    }

    private void UpdateType() {
        int sum = locationValue + levelOffset;
        if (sum > 5) {
            sum = 5;
        }
        else if (sum < 0) {
            sum = 0;
        }
        type = (TreeType)(sum);
        TemporaryBackgroundUpdate();
    }

    public void SetPosition(Vector2 position) {
        type = TreeType.SAPLING;
        transform.position = position;
    }

    public void AddNeighbor(Edge edge) {
		neighbors = SortNeighbors(edge, neighbors);
        RecalculateLocationValue();
    }

	public void AddNeighbor(List<Edge> edges) {
		foreach (Edge edge in edges) {
			CreateConnector(edge.GetOpposite(gameObject));
			edge.GetOpposite(gameObject).GetComponent<Node>().AddNeighbor(edge);
			AddNeighbor(edge);
		}
	}

	public List<Edge> SortNeighbors(Edge edge, List<Edge> edges) {
		List<Edge> sorted = new List<Edge>();
		sorted.AddRange(edges);
		float angle = PointManager.RotationOfLine(edge.GetOpposite(gameObject).transform.position, gameObject.transform.position);
		if (sorted.Count == 0) {
			sorted.Add(edge);
		} else {
			for (int i = 0; i < edges.Count; ++i) {
				float tempAngle = PointManager.RotationOfLine(edges[i].GetOpposite(gameObject).transform.position, gameObject.transform.position);
				if (angle > tempAngle) {
					sorted.Insert(i, edge);
					break;
				}
			}
		}
		return sorted;
	}

	private void CreateConnector(GameObject other) {
		LineRenderer lr = GameObject.Instantiate(connector).GetComponent<LineRenderer>();
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

    public void RecalculateLocationValue() {        //Where the magic happens TODO
        int n = neighbors.Count;
        locationValue = n;
        UpdateType();
    }
}
