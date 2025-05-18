using UnityEngine;

public class PlayerController : MonoBehaviour
{
    MoveController movementController;
    Animator animator;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        movementController = GetComponent<MoveController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementController.SetDirection("left");
            transform.localScale = new Vector3(-1, 1, 1);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.Play("Pacman");
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementController.SetDirection("right");
            transform.localScale = new Vector3(1, 1, 1);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.Play("Pacman");
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementController.SetDirection("down");
            transform.localScale = new Vector3(1, 1, 1);
            transform.rotation = Quaternion.Euler(0, 0, -90);
            animator.Play("Pacman");
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementController.SetDirection("up");
            transform.localScale = new Vector3(1, 1, 1);
            transform.rotation = Quaternion.Euler(0, 0, 90);
            animator.Play("Pacman");
        }
    }
}
