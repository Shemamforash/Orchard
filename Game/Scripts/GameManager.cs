using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{
	private static int numberOfSeeds = 0;
	public GameObject seedCounter, diseasePrefab;
	private float diseaseCounter = -1, diseaseTarget;

	public void Update() {
		seedCounter.GetComponent<Text>().text = "Number of Seeds: " + numberOfSeeds;
		UpdateDisease();
	}

	private void UpdateDisease() {
		if (diseaseCounter == -1) {
			diseaseCounter = 0;
			diseaseTarget = Random.Range(15, 25);
		} else if (diseaseCounter > diseaseTarget) {
			GameObject newDisease = GameObject.Instantiate(diseasePrefab);
			newDisease.GetComponent<Disease>().FindTarget();
			diseaseCounter = -1;
		} else {
			diseaseCounter += Time.deltaTime;
		}
	}

	public static void AddSeed() {
		++numberOfSeeds;
	}

	public static int SeedCount() {
		return numberOfSeeds;
	}

	public static void RemoveSeed() {
		--numberOfSeeds;
	}
}
