using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoveController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject currentNode;
    public float speed = 4f;
    public string direction = "";
    public string lastMovingDirection = "";

    private bool canWarp = true;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        NodeController currentNodeController = currentNode.GetComponent<NodeController>();
        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);
        bool reverseDirection = false;
        if (
            (direction == "left" && lastMovingDirection == "right")
            || (direction == "right" && lastMovingDirection == "left")
            || (direction == "up" && lastMovingDirection == "down")
            || (direction == "down" && lastMovingDirection == "up")
        )
        {
            reverseDirection = true;
        }
        //Asegurarse de estar en el centro del siguiente nodo
        if ((transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y) || reverseDirection)
        {
            //Si se llega al nodo left warp se cambia la posición al right warp y se ajusta la dirección
            if (currentNodeController.isWarpLeftNode && canWarp)
            {
                currentNode = gameManager.rightWarpNode;
                direction = "left";
                lastMovingDirection = "left";
                transform.position = currentNode.transform.position;
                canWarp = false;
            }
            //Si se llega al nodo right warp se cambia la posición al left warp y se ajusta la dirección

            else if (currentNodeController.isWarpRightNode && canWarp)
            {
                currentNode = gameManager.leftWarpNode;
                direction = "right";
                lastMovingDirection = "right";
                transform.position = currentNode.transform.position;
                canWarp = false;
            }
            //Encuentra el siguiente nodo
            else
            {
                GameObject newNode = currentNodeController.GetNodeFromDirection(direction);
                if (newNode != null)
                {
                    currentNode = newNode;
                    lastMovingDirection = direction;
                }
                //No se puede mover en la dirección deseada, entonces seguir en la ultima dirección
                else
                {
                    direction = lastMovingDirection;
                    newNode = currentNodeController.GetNodeFromDirection(direction);
                    if (newNode != null)
                    {
                        currentNode = newNode;
                    }
                }

            }



        }
        else
        {
            canWarp = true;
        }
    }
    public void SetDirection(string newDirection)
    {
        direction = newDirection;
    }

}
