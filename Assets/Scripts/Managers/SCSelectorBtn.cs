using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCSelectorBtn : MonoBehaviour {
    //EVENTO (DELEGADO)   --> Restart Game
    public delegate void RestartGame();
    public static event RestartGame OnRestartGame;    //(EVENTO)

    // Método para cargar una nueva escena por nombre
    public void LoadScene(string sceneName) {
        SCManager.instance.LoadScene(sceneName);  // Carga la nueva escena y quita la anterior (lo mismo que LoadSceneMode.Single)
    }
    // Método para cargar una nueva escena por nombre Sin Quitar la actual
    public void LoadSceneAdditive(string sceneName) {
        SCManager.instance.LoadSceneAdditive(sceneName);    // Carga otra escena sin quitar la anterior
    }

    public void MainMenu() {
        SCManager.instance.LoadScene("MainMenu");
        AudioManager.instance.PlayMusic("MainTheme");
        //AudioManager.instance.PlaySFX("Button");
    }
    public void PlayGame() {
        SCManager.instance.LoadScene("Game");
        AudioManager.instance.PlayMusic("GameTheme");
        //AudioManager.instance.PlaySFX("Button");
    }
    public void LoadConfig() {
        SCManager.instance.LoadScene("Settings");
        AudioManager.instance.PlayMusic("MainTheme");
        //AudioManager.instance.PlaySFX("Button");
    }
    public void LoadCredits() {
        SCManager.instance.LoadScene("Credits");
        AudioManager.instance.PlayMusic("MainTheme");
        //AudioManager.instance.PlaySFX("Button");
    }

    public void OpenPause() {
        SCManager.instance.LoadSceneAdditive("Pause");
        Time.timeScale = 0f; // Pausar el tiempo del juego
    }
    public void ClosePause() {
        SCManager.instance.UploadSceneAdditive("Pause");
        Time.timeScale = 1f;
    }

    public void ResetGame() {
        SCManager.instance.LoadScene("Game");
        AudioManager.instance.PlayMusic("GameTheme");
        //AudioManager.instance.PlaySFX("Button");

        // Event Restart Game
        if (OnRestartGame != null)
            OnRestartGame();
    }

    public void ExitGame() {
        //AudioManager.instance.PlaySFX("Button");
        SCManager.instance.ExitGame();
    }
}
