using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class LoadingPercent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] AudioSource introSource;

    [SerializeField] AudioClip intro;

    public float loadSpeed = 20f; // percent per second

    private float currentProgress = 0f;

    private void Start()
    {
        PlaySFX(intro);
    }
    void Update()
    {
        StartCoroutine(LoopScaleTitleSize());

        if (currentProgress < 100f)
        {
            currentProgress += loadSpeed * Time.deltaTime;
            int displayProgress = Mathf.Clamp(Mathf.FloorToInt(currentProgress), 0, 100);
            loadingText.text = displayProgress + "%";
        }
        else
        {
            introSource.Stop();
            SceneManager.LoadScene("Game");
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        introSource.PlayOneShot(clip);
    }
    IEnumerator LoopScaleTitleSize()
    {
        Vector3 baseSize = new Vector3(.9f, .9f, .9f);       // Neutral scale
        float amplitude = 0.1f;                           // How much to grow/shrink
        float frequency = 2f;                             // Speed of pulsing

        while (true)
        {
            float time = Time.time * frequency;
            float scaleFactor = 1f + Mathf.Sin(time) * amplitude;
            titleText.transform.localScale = baseSize * scaleFactor;
            yield return null;
        }
    }

}
