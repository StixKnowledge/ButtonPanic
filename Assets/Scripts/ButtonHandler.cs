using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [Header("Boundary Settings")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minZ = -3f;
    public float maxZ = 3f;

    AudioManager audioManager;
    [Header("Game Objects")]
    public GameObject objectPlacer;
    public GameObject objectPlayer;

    [Header("Interactable Objects")]
    public GameObject button;
    public GameObject quad;
    public GameObject finishBox;
    public GameObject falseBox;
    public GameObject stepNotifier;
    public GameObject[] falseBoxTease;

    public bool firstClick = false;

    bool steppedOnFinishBox = false;
    bool steppedOnFalseBox = false;


    bool buttonFinishBoxNotifier = false;
    bool buttonFalseBoxNotifier = false;

    public int falseBoxTeaseCount = 0;

    public System.Action OnGameWin;
    public System.Action OnFalseBox;

    //
    public System.Action OnFirstClick;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
    }

    public void OnButtonClicked()
    {
        audioManager.PlaySFX(audioManager.button);
        if (!firstClick)
        {
            OnFirstClick?.Invoke();
            RandomReposition(button);
            RandomReposition(finishBox);
            SpawnRandom(falseBox);
            SpawnRandom(falseBox);
            SpawnRandom(falseBox);


            objectPlacer.SetActive(true);
            objectPlayer.SetActive(true);
            firstClick = true;
            //Debug.Log("First Click");
        }
        else
        {
            if(buttonFinishBoxNotifier)
            {
                StopCoroutine(PlayFalseBoxTease());
                stepNotifier.SetActive(false);
                OnGameWin?.Invoke();
                //Debug.Log("You won!");
            }

            else if(buttonFalseBoxNotifier)
            {
                StartCoroutine(PlayFalseBoxTease());
                stepNotifier.SetActive(false);
                OnFalseBox?.Invoke();
                //animation na lalayo then destroy falsebox
                audioManager.PlaySlide();

                //Debug.Log("Sikee!");

                objectPlacer.SetActive(true);
                objectPlayer.SetActive(true);
                //falseBox.SetActive(true);
                buttonFalseBoxNotifier = false;
            }
            //Debug.Log("Not First Click");
        }
    }

    public IEnumerator PlayFalseBoxTease()
    {
        falseBoxTease[falseBoxTeaseCount].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        falseBoxTease[falseBoxTeaseCount].SetActive(false);
        falseBoxTeaseCount++;
    }

    private void Update()
    {
        PlayerBoundary();
        if(steppedOnFalseBox || steppedOnFinishBox)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            transform.position = new Vector3(button.transform.position.x, .6f, button.transform.position.z);
        }
        if (steppedOnFinishBox)
        {
            //Debug.Log("Stepped on Finishbox!");
            stepNotifier.SetActive(true);

            objectPlacer.SetActive(false);
            objectPlayer.SetActive(false);

            buttonFinishBoxNotifier = true;
            steppedOnFinishBox = false;

        }
        else if(steppedOnFalseBox)
        {
            //Debug.Log("Stepped on Falsebox!");
            stepNotifier.SetActive(true);

            objectPlacer.SetActive(false);
            objectPlayer.SetActive(false);

            buttonFalseBoxNotifier = true;
            steppedOnFalseBox = false;
        }
    }

    void PlayerBoundary()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);
        transform.position = clampedPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minX, transform.position.y, minZ), new Vector3(maxX, transform.position.y, minZ));
        Gizmos.DrawLine(new Vector3(maxX, transform.position.y, minZ), new Vector3(maxX, transform.position.y, maxZ));
        Gizmos.DrawLine(new Vector3(maxX, transform.position.y, maxZ), new Vector3(minX, transform.position.y, maxZ));
        Gizmos.DrawLine(new Vector3(minX, transform.position.y, maxZ), new Vector3(minX, transform.position.y, minZ));
    }


    void RandomReposition(GameObject objectToSpawn)
    {
        //Debug.Log("random deploy " + objectToSpawn.transform.name);
        // Get the bounds of the quad
        Renderer quadRenderer = quad.GetComponent<Renderer>();
        Bounds bounds = quadRenderer.bounds;

        // Generate random position within bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        Vector3 spawnPosition = new Vector3(randomX, objectToSpawn.transform.position.y, randomZ);
        objectToSpawn.transform.position = spawnPosition;

    }

    void SpawnRandom(GameObject objectToSpawn)
    {
        // Get the bounds of the quad
        Renderer quadRenderer = quad.GetComponent<Renderer>();
        Bounds bounds = quadRenderer.bounds;

        // Generate random position within bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        Vector3 spawnPosition = new Vector3(randomX, objectToSpawn.transform.position.y + .3f, randomZ);

        //dito na rin lang ilalagay yung pagtapakan ng winner, hindi pwede dahil magkatulad na sila ng position ng button
        Instantiate(objectToSpawn, spawnPosition, Quaternion.Euler(90, 0, 0));

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FinishBox"))
        {
            StopCoroutine(PlayFalseBoxTease());
            steppedOnFinishBox = true;
        }

        if( other.gameObject.CompareTag("FalseBox"))
        {
            Destroy(other.gameObject);
            steppedOnFalseBox = true;
            //Debug.Log("Player reached the FalseBox!");
        }
    }
}

