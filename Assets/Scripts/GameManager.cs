using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject pacman;
    public GameObject leftWarpNode;
    public GameObject rightWarpNode;


    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeStart;
    public GameObject ghostNodeCenter;
    [Header("Ghosts")]
    public GameObject redGhost;
    public GameObject pinkGhost;
    public GameObject blueGhost;
    public GameObject orangeGhost;

    public EnemyController redGhostController;
    public EnemyController pinkGhostController;
    public EnemyController blueGhostController;
    public EnemyController orangeGhostController;

    public int totalPellets;
    public int pelletsleft;
    public int pelletCollectedOnThisLife;

    public bool gameIsRunnig;

    public List<NodeController> nodeControllers = new List<NodeController>();


    public enum GhostMode
    {
        chase,
        scatter
    }
    public GhostMode currentGhostMode;

    [Header("AudioSources")]
    // public AudioSource siren;
    // public AudioSource munch1;
    // public AudioSource munch2;
    // public AudioSource startGameAudio;

    public int currentMunch = 0;
    public TextMeshProUGUI scoreText;
    public int score;
    public bool hadDeadOnThisLevel = false;

    public bool newGame;
    public bool clearedLevel;

    public int lives;
    public int currentLevel;

    public bool isPowerPelletRunning = false;
    public float currentPowerPelletTime = 0;
    public float powerPelletTimer = 8f;
    public float powerPelletDuration = 7.0f;


    public int[] ghostModeTimers = new int[] { 7, 20, 7, 20, 5, 20, 5 };
    public int ghostModeTiemrIndex;
    public float ghostModeTimer = 0;
    public bool completeTimmer;
    public bool runningTimmer;

    public int powerPelletMultiplyer = 1;
    void Awake()
    {

        newGame = true;
        clearedLevel = false;

        redGhostController = redGhost.GetComponent<EnemyController>();
        pinkGhostController = pinkGhost.GetComponent<EnemyController>();
        blueGhostController = blueGhost.GetComponent<EnemyController>();
        orangeGhostController = orangeGhost.GetComponent<EnemyController>();

        ghostNodeStart.GetComponent<NodeController>().isGhostStartingNode = true;
        pacman = GameObject.Find("Player");

        StartCoroutine(Setup());

    }
    public IEnumerator Setup()
    {
        ghostModeTiemrIndex = 0;
        ghostModeTimer = 0;

        completeTimmer = false;
        runningTimmer = true;
        if (clearedLevel)
        {
            yield return new WaitForSeconds(0.1f);
        }
        pelletCollectedOnThisLife = 0;
        currentGhostMode = GhostMode.scatter;
        gameIsRunnig = false;
        float waitTimer = 1f;
        if (clearedLevel || newGame)
        {
            pelletsleft = totalPellets;
            waitTimer = 3f;
            //Pellet Respawn
            for (int i = 0; i < nodeControllers.Count; i++)
            {
                nodeControllers[i].RespawnPellet();
            }
        }
        if (newGame)
        {
            // startGameAudio.Play();
            score = 0;
            scoreText.text = "Score " + score.ToString();
            lives = 3;
            currentLevel = 1;
        }


        pacman.GetComponent<PlayerController>().Setup();
        redGhostController.Setup();
        pinkGhostController.Setup();
        blueGhostController.Setup();
        orangeGhostController.Setup();

        newGame = false;
        clearedLevel = false;
        hadDeadOnThisLevel = false;
        yield return new WaitForSeconds(waitTimer);

        StartGame();
    }

    void StartGame()
    {
        gameIsRunnig = true;

    }
    void StopGame()
    {
        gameIsRunnig = false;
        pacman.GetComponent<PlayerController>().StopAllCoroutines();

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameIsRunnig)
        {
            return;
        }
        if (!completeTimmer && runningTimmer)
        {
            ghostModeTimer += Time.deltaTime;
            if (ghostModeTimer >= ghostModeTimers[ghostModeTiemrIndex])
            {
                ghostModeTimer = 0;
                ghostModeTiemrIndex++;
                if (currentGhostMode == GhostMode.chase)
                {
                    currentGhostMode = GhostMode.scatter;
                }
                else
                {
                    currentGhostMode = GhostMode.chase;
                }
                if (ghostModeTiemrIndex == ghostModeTimers.Length)
                {
                    completeTimmer = true;
                    runningTimmer = false;
                    currentGhostMode = GhostMode.chase;
                }
            }
        }
        if (isPowerPelletRunning)
        {
            currentPowerPelletTime += Time.deltaTime;
            if (currentPowerPelletTime >= powerPelletDuration)
            {
                isPowerPelletRunning = false;
                currentPowerPelletTime = 0;
                powerPelletMultiplyer = 1;
            }
        }

    }
    public void GotPelletFromNodeController(NodeController nodeController)
    {
        if (nodeController.hasPellet)
        {
            nodeControllers.Add(nodeController); // Guarda referencia al nodo con pellet
            totalPellets++;                      // Suma al total
            pelletsleft++;                       // También al contador de los que faltan
        }

    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score " + score.ToString();

    }

    public IEnumerator CollectedDots(NodeController nodeController)
    {
        // if (currentMunch == 0)
        // {
        //     munch1.Play();
        //     currentMunch = 1;
        // }
        // else if (currentMunch == 1)
        // {
        //     munch2.Play();
        //     currentMunch = 0;
        // }
        pelletsleft--;
        pelletCollectedOnThisLife++;
        int requiredBluePellets = 0;
        int requiredOrangePellets = 0;
        if (hadDeadOnThisLevel)
        {
            requiredBluePellets = 12;
            requiredOrangePellets = 32;
        }
        else
        {
            requiredBluePellets = 30;
            requiredOrangePellets = 60;
        }
        if (pelletCollectedOnThisLife >= requiredBluePellets && !blueGhost.GetComponent<EnemyController>().leftHomeBefore)
        {
            blueGhost.GetComponent<EnemyController>().readyToLeaveHome = true;

        }
        if (pelletCollectedOnThisLife >= requiredOrangePellets && !orangeGhost.GetComponent<EnemyController>().leftHomeBefore)
        {
            orangeGhost.GetComponent<EnemyController>().readyToLeaveHome = true;

        }
        AddScore(10);
        if (pelletsleft == 0)
        {
            currentLevel++;
            clearedLevel = true;
            StopGame();
            yield return new WaitForSeconds(1);
            StartCoroutine(Setup());
        }

        if (nodeController.isPowerPellet)
        {
            isPowerPelletRunning = true;
            currentPowerPelletTime = 0;


            redGhostController.SetFrightened(true);
            pinkGhostController.SetFrightened(true);
            blueGhostController.SetFrightened(true);
            orangeGhostController.SetFrightened(true);

        }
    }

    public IEnumerator PauseGame(float timeToPause)
    {
        gameIsRunnig = false;
        yield return new WaitForSeconds(timeToPause);
        gameIsRunnig = true;
    }

    public void GhostEaten()
    {
        StartCoroutine(PauseGame(1));
    }


    public IEnumerator PlayerEaten()
    {
        hadDeadOnThisLevel = true;
        StopGame();
        yield return new WaitForSeconds(1);

        redGhostController.SetVisible(false);
        pinkGhostController.SetVisible(false);
        blueGhostController.SetVisible(false);
        orangeGhostController.SetVisible(false);

        pacman.GetComponent<PlayerController>().Death();
        yield return new WaitForSeconds(3);

        lives--;
        if (lives <= 0)
        {
            newGame = true;
            yield return new WaitForSeconds(3);
        }
        StartCoroutine(Setup());
    }
}
