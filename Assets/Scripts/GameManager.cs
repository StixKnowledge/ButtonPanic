using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, Won, Lost }
    private GameState currentState = GameState.Playing;

    AudioManager audioManager;
    public ButtonHandler buttonHandler;
    public Raycast raycast;

    [Header("UI Elements")]
    public GameObject finishBoxCrossHair;
    [SerializeField] GameObject introduction;
    [SerializeField] GameObject missionText;
    [SerializeField] GameObject steppedOnFalseBox;
    [SerializeField] GameObject notMovingNotifier;
    [SerializeField] GameObject alnicochill;
    [SerializeField] GameObject alnicokabahan;
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Win Events")]
    public GameObject gameWin;
    [SerializeField] GameObject win;
    [SerializeField] GameObject winVideoPlayer;

    [Header("Lose Events")]
    public GameObject gameLose;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject loseVideoPlayer;

    [Header("Timer Settings")]
    [SerializeField] float targetShowTime = 5;
    [SerializeField] float remainingTime;
    public System.Action OnZeroTime;

    float screamTime;
    bool scream = true;
    bool firstClicked = false;

    Vector3 buttonLastSecondPosition;
    float buttonStillTime = 0f;

    private void Awake()
    {
        screamTime = remainingTime / 2;
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
    }

    private void Start()
    {
        buttonHandler.OnGameWin += GameWin;
        buttonHandler.OnFirstClick += OnFirstClick;
        StartCoroutine(Introductioning());
    }

    private void OnFirstClick()
    {
        firstClicked = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            return;
        }



        if (currentState == GameState.Playing && firstClicked)
        {
            Vector3 currentPosition = buttonHandler.transform.position;

            if (firstClicked)
            {
                if(currentPosition == buttonLastSecondPosition)
                //if (Vector3.Distance(currentPosition, buttonLastSecondPosition) < 0.001f)
                {
                    buttonStillTime += Time.deltaTime;

                    if(buttonStillTime >= 2f)
                    {
                        if (!notMovingNotifier.activeSelf)
                            notMovingNotifier.SetActive(true);

                        Time.timeScale = 1.5f;

                    }
                }
                else
                {
                    buttonStillTime = 0f;
                    Time.timeScale = 1f;

                    if(notMovingNotifier.activeSelf)
                        notMovingNotifier.SetActive(false);
                }
                buttonLastSecondPosition = currentPosition;
            }

            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
            else
            {
                remainingTime = 0;
                OnZeroTime?.Invoke();
                if (currentState == GameState.Playing)
                {
                    GameOver();
                }
            }

            if (remainingTime <= targetShowTime)
            {
                finishBoxCrossHair.SetActive(true);
            }

            if (remainingTime <= screamTime && scream)
            {
                alnicochill.SetActive(false);
                alnicokabahan.SetActive(true);
                audioManager.PlaySFX(audioManager.scream);
                scream = false;
                timerText.color = Color.red;
            }

            int minutes = Mathf.FloorToInt(remainingTime / 60F);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (firstClicked)
        {
            introduction.SetActive(true);
            missionText.SetActive(false);
        }
    }

    IEnumerator Introductioning()
    {
        missionText.SetActive(true);
        yield return new WaitForSeconds(5);
    }

    void GameOver()
    {
        if (currentState != GameState.Playing) return;
        currentState = GameState.Lost;

        StartCoroutine(WaitBeforeGameOver());
    }

    IEnumerator WaitBeforeGameOver()
    {
        if(buttonHandler.falseBoxTeaseCount < buttonHandler.falseBoxTease.Length)
            buttonHandler.falseBoxTease[buttonHandler.falseBoxTeaseCount].SetActive(false);

        OnInteractableUIDisable();
        audioManager.StopSound(audioManager.scream);
        audioManager.StopMusic();
        raycast.enabled = false;
        loseVideoPlayer.SetActive(true);
        explosion.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        gameLose.SetActive(true);
    }

    void GameWin()
    {
        if (currentState != GameState.Playing) return;
        currentState = GameState.Won;

        OnInteractableUIDisable();
        loseVideoPlayer.SetActive(false);
        explosion.SetActive(false);
        gameLose.SetActive(false);
        audioManager.StopSound(audioManager.scream);
        audioManager.StopMusic();
        raycast.enabled = false;
        winVideoPlayer.SetActive(true);
        win.SetActive(true);
        gameWin.SetActive(true);
    }

    void OnInteractableUIDisable()
    {
        steppedOnFalseBox.SetActive(false);
        notMovingNotifier.SetActive(false);
    }
}