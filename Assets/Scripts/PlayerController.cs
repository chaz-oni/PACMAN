using UnityEngine;

public class PlayerController : MonoBehaviour
{
    MoveController movementController;
    public GameObject startNode;
    public GameManager gameManager;
    public AudioSource death;

    public Animator animator;
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponentInChildren<Animator>();
        movementController = GetComponent<MoveController>();
        transform.localScale = new Vector3(-1, 1, 1);
        movementController.lastMovingDirection = "left";
        startNode = movementController.currentNode;


    }

    public void Setup()
    {
        movementController.currentNode = startNode;
        movementController.lastMovingDirection = "left";
        transform.localScale = new Vector3(-1, 1, 1);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        animator.speed = 1;
        transform.localPosition = new Vector2(-0.051f, -2.235f);
    }
    void Stop()
    {
        animator.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameManager.gameIsRunnig)
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (movementController.CanMoveInDirection("left"))
            {
                movementController.SetDirection("left");
                transform.localScale = new Vector3(-1, 1, 1);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                animator.Play("Pacman");
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (movementController.CanMoveInDirection("right"))
            {
                movementController.SetDirection("right");
                transform.localScale = new Vector3(1, 1, 1);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                animator.Play("Pacman");
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (movementController.CanMoveInDirection("down"))
            {
                movementController.SetDirection("down");
                transform.localScale = new Vector3(1, 1, 1);
                transform.rotation = Quaternion.Euler(0, 0, -90);
                animator.Play("Pacman");
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            if (movementController.CanMoveInDirection("up"))
            {
                movementController.SetDirection("up");
                transform.localScale = new Vector3(1, 1, 1);
                transform.rotation = Quaternion.Euler(0, 0, 90);
                animator.Play("Pacman");
            }
        }
    }
    public void Death()
    {
        death.Play();
        animator.SetBool("pacman death", true);
        animator.SetBool("Pacman", false);
    }
}
