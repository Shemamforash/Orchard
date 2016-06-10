using UnityEngine;
using System.Collections.Generic;

public class LineManager : MonoBehaviour {
    private LineRenderer lineRenderer;
    private GameObject nearbyMouse;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        if (nearbyMouse == null || GameManager.SeedCount() == 0) {
            lineRenderer.SetVertexCount(0);
        } else {
            lineRenderer.SetVertexCount(2);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, nearbyMouse.transform.position);
        }
    }

	public void SetMouseNearby(GameObject nearbyMouse) {
		this.nearbyMouse = nearbyMouse;
	}
}
