using UnityEngine;
using System.Collections.Generic;

public class MouseFollower : MonoBehaviour {
    private Graph graph;
    public GameObject linePrefab;
    private List<GameObject> nearNodes = new List<GameObject>();

    void Start() {
        graph = Camera.main.GetComponent<Graph>();
    }

	void Update () {
        Vector3 mouseVect = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouse2d = new Vector2(mouseVect.x, mouseVect.y);
        mouseVect.z = 0;
        transform.position = mouseVect;

        if (Input.GetMouseButtonDown(0)) {
			if (!graph.Add(mouse2d, nearNodes)) {
				Debug.Log("bad position");
			}
        }

        DrawNearestNodes(mouse2d);
	}

    private void DrawNearestNodes(Vector2 mouse) {
        List<GameObject> nearestTrees = graph.NearestTrees(mouse);

        foreach (GameObject g in nearNodes) {                                 //For all previous nearby trees
            if (!nearestTrees.Contains(g)) {                                    //If they are not nearby remove them
                g.GetComponent<LineManager>().SetMouseNearby(null);
            }
        }

        nearNodes.Clear();
        foreach (GameObject g in nearestTrees) {
            g.GetComponent<LineManager>().SetMouseNearby(gameObject);
            nearNodes.Add(g);
        }
    }
}
