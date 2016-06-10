using UnityEngine;
using System.Collections;

public class ConnectorBehaviour : MonoBehaviour {
	private GameObject creator, target;

	public void SetEnds(GameObject creator, GameObject target) {
		this.creator = creator;
		this.target = target;
	}

	void Update () {
		if (creator == null || target == null) {
			GameObject.Destroy(gameObject);
		}
	}
}
