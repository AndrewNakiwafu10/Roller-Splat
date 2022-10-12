using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    private AudioSource cheerAudio;
    //public AudioClip cheerAudio;
    private BallController ballControllerScript;

    private GroundPiece[] allGroundPieces;
    // Start is called before the first frame update
    void Start()
    {
        cheerAudio = GetComponent<AudioSource>();
        ballControllerScript = GameObject.Find("BallPrefab").GetComponent<BallController>();
        ballControllerScript.audioSource.Play();
        ballControllerScript.endLevel.Stop();
        SetUpNewLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetUpNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }else if(singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLeveLFinishedLoading;
    }
    private void OnLeveLFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetUpNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished = true;

        for(int i = 0; i < allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if (isFinished)
        {
            NextLevel();
        }
    }

    private void NextLevel()
    {
        StartCoroutine(CheerPlayer());
  
    }

    IEnumerator CheerPlayer()
    {
        ballControllerScript.audioSource.Stop();
        ballControllerScript.endLevel.Play();
        cheerAudio.Play();
        yield return new WaitForSeconds (2);
        cheerAudio.Stop();
        ballControllerScript.endLevel.Stop();

        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
}
