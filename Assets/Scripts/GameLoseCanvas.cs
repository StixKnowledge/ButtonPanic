using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoseCanvas : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] float waitTime; // Set the countdown time in seconds

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();

    }
    private void Start()
    {
        //Debug.Log("Playing gameLose SFX");
        audioManager.PlayLose();

        waitTime = audioManager.gameLose.length;
    }

    private void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else if (waitTime < 0)
        {
            waitTime = 0;
            SceneManager.LoadScene("Game");
        }

        int seconds = Mathf.FloorToInt(waitTime % 60);
        timerText.text = $"Play again in: {seconds}";
    }
}
