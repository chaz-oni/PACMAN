using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

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
    public AudioSource siren;
    public AudioSource munch1;
    public AudioSource munch2;
    public AudioSource startGameAudio;

    public int currentMunch = 0;
    public Text scoreText;
    public int score;
    public bool hadDeadOnThisLevel = false;

    public bool newGame;
    public bool clearedLevel;

    public int lives;
    public int currentLevel;
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
            waitTimer = 4f;
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

    // Update is called once per frame
    void Update()
    {

    }
    public void GotPelletFromNodeController(NodeController nodeController)
    {
        nodeControllers.Add(nodeController);
        totalPellets++;
        pelletsleft++;

    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score " + score.ToString();

    }

    public void CollectedDots(NodeController nodeController)
    {
        if (currentMunch == 0)
        {
            munch1.Play();
            currentMunch = 1;
        }
        else if (currentMunch == 1)
        {
            munch2.Play();
            currentMunch = 0;
        }
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
    }
}
