using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoveController : MonoBehaviour
{
    public GameObject currentNode;
    public float speed = 4f;
    public string direction = "";
    public string lastMovingDirection = "";
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        NodeController currentNodeController = currentNode.GetComponent<NodeController>();
        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);
        //Asegurarse de estar en el centro del siguiente nodo
        if (transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y)
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
    public void SetDirection(string newDirection)
    {
        direction = newDirection;
    }

}
