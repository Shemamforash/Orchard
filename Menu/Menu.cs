using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour{
    public void LoadGame() {
        Application.LoadLevel("Game");
    }

    public void CloseWindow() {
        Application.Quit();
    }
}
