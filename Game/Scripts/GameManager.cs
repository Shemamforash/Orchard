using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{
	private static int numberOfSeeds = 0;
	public GameObject seedCounter, diseasePrefab, gameTimer, gameOverPanel, gameOverText;
	private float diseaseCounter = -1, diseaseTarget;
	private float startTime, endTime;

	public void Start() {
		startTime = Time.time;
		endTime = Time.time + 60f;
	}

	public void Update() {
		UpdateTime();
		if (Time.time >= endTime || Camera.main.gameObject.GetComponent<Graph>().Nodes().Count <= 0) {
			EndGame(false);
		} else if(Time.time >= endTime){
			EndGame(true);
		}
		seedCounter.GetComponent<Text>().text = "Number of Seeds: " + numberOfSeeds;
		UpdateDisease();
	}

	private void EndGame(bool playerWon) {
		gameOverPanel.SetActive(true);
		if (playerWon) {
			gameOverText.GetComponent<Text>().text = "Congratulations, your orchard thrives!".ToUpper();
		} else {
			gameOverText.GetComponent<Text>().text = "You leave desolation behind you.".ToUpper();
		}
	}

	private void UpdateTime() {
		float currentTime = Time.time - startTime;
		string seconds = Mathf.Floor(currentTime % 60).ToString("00");
		string millis = Mathf.Floor(currentTime * 100 % 100).ToString("00");
		gameTimer.GetComponent<Text>().text = seconds + ":" + millis;
	}

	private void UpdateDisease() {
		if (diseaseCounter == -1) {
			diseaseCounter = 0;
			diseaseTarget = Random.Range(15, 25);
		} else if ((diseaseCounter > diseaseTarget || Input.GetButton("Jump")) && !DiseaseFactory.DiseaseActive()) {
			DiseaseFactory.CreateDisease(diseasePrefab);
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
