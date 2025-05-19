using UnityEngine;
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

    public enum GhostMode
    {
        chase,
        scatter
    }
    public GhostMode currentGhostMode;

    // public AudioSource siren;
    // public AudioSource munch1;
    // public AudioSource munch2;
    // public int currentMunch = 0;
    // public Text scoreText;
    // public int score;
    void Awake()
    {
        currentGhostMode = GhostMode.chase;
        ghostNodeStart.GetComponent<NodeController>().isGhostStartingNode = true;
        pacman = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {

    }
    // public void AddScore(int amount)
    // {

    // }
    // public void CollectedDots(NodeController node)
    // {
    //     if (currentMunch == 0)
    //     {
    //         munch1.Play();
    //         currentMunch = 1;
    //     }
    //     else if (currentMunch == 1)
    //     {
    //         munch2.Play();
    //         currentMunch = 0;
    //     }
    //     AddScore(10);
    // }
}
