using UnityEngine;
using System.Collections.Generic;

public class MouseFollower : MonoBehaviour {
    private Graph graph;
    public GameObject linePrefab;
    private List<GameObject> nearNodes = new List<GameObject>();
	public Sprite valid, invalid;
	private float maxDistance, minDistance;
	private bool validPosition = false;

    void Start() {
        graph = Camera.main.GetComponent<Graph>();
		GetNewMinMaxDistances();
    }

	void Update () {
        Vector3 mouseVect = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouse2d = new Vector2(mouseVect.x, mouseVect.y);
        mouseVect.z = 0;
        transform.position = mouseVect;
		if (GameManager.SeedCount() > 0) {
			DrawNearestNodes(mouse2d);
			if (Input.GetMouseButtonDown(0) && validPosition) {
				if (!graph.Add(mouse2d, nearNodes)) {
					Debug.Log("bad position");
				} else {
					GameManager.RemoveSeed();
					Plant();
					GetNewMinMaxDistances();
				}
			}
		}
	}

	private void GetNewMinMaxDistances() {
		maxDistance = Random.value + 3;
		minDistance = Random.value + 1.5f;
	}

    private void DrawNearestNodes(Vector2 mouse) {
        List<GameObject> nearestTrees = graph.NearestTrees(mouse, maxDistance, minDistance);

        foreach (GameObject g in nearNodes) {                                 //For all previous nearby trees
            if (!nearestTrees.Contains(g)) {                                    //If they are not nearby remove them
                g.GetComponent<LineManager>().SetMouseNearby(null);
            }
        }

        nearNodes.Clear();
		validPosition = false;
        foreach (GameObject g in nearestTrees) {
            g.GetComponent<LineManager>().SetMouseNearby(gameObject);
			float d = Vector2.Distance(g.transform.position, transform.position);
			if (d <= minDistance) {
				validPosition = false;
				break;
			} else if (d < maxDistance) {
				nearNodes.Add(g);
				validPosition = true;
			}
        }
		UpdatePlantArea();
    }


	public void UpdatePlantArea() {
		if (validPosition) {
			transform.FindChild("PlantingArea").GetComponent<SpriteRenderer>().sprite = valid;
		} else {
			transform.FindChild("PlantingArea").GetComponent<SpriteRenderer>().sprite = invalid;
		}
	}

	public void Plant() {
		transform.FindChild("PlantingArea").GetComponent<SpriteRenderer>().sprite = null;
	}
}
