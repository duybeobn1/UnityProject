using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text pointsText;
    public static int score;

    public void Setup()
    {
        score = ScoreManager.scoreCount;
        gameObject.SetActive(true);
        pointsText.text = Mathf.Round(score) + " Points";
        Debug.Log(score);
        // Rend le curseur visible
        Cursor.visible = true;

        // Libère le curseur pour qu'il puisse se déplacer librement
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartButton()
    {
    	ScoreManager.scoreCount = 0;
        SceneManager.LoadScene("Demo");
    }

    public void ExitButton()
    {
    	ScoreManager.scoreCount = 0;
        SceneManager.LoadScene("Main Menu");
    }
}
