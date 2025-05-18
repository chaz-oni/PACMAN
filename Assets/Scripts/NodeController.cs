using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NodeController : MonoBehaviour
{
    public bool canMoveLeft = false;
    public bool canMoveRigth = false;
    public bool canMoveUp = false;
    public bool canMoveDown = false;
    public GameObject nodeLeft;
    public GameObject nodeRight;
    public GameObject nodeUp;
    public GameObject nodeDown;

    void Start()
    {
        RaycastHit2D[] hitsDown;
        //Raycast hacia abajo
        hitsDown = Physics2D.RaycastAll(transform.position, -Vector2.up);
        for (int i = 0; i < hitsDown.Length; i++)
        {
            float distance = Mathf.Abs(hitsDown[i].point.y - transform.position.y);
            if (distance < 0.4f)
            {
                canMoveDown = true;
                nodeDown = hitsDown[i].collider.gameObject;
            }
        }
        RaycastHit2D[] hitsUp;
        //Raycast hacia arriba
        hitsUp = Physics2D.RaycastAll(transform.position, Vector2.up);
        for (int i = 0; i < hitsUp.Length; i++)
        {
            float distance = Mathf.Abs(hitsUp[i].point.y - transform.position.y);
            if (distance < 0.4f)
            {
                canMoveUp = true;
                nodeUp = hitsUp[i].collider.gameObject;
            }
        }
        RaycastHit2D[] hitsRight;
        //Raycast hacia la derecha
        hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right);
        for (int i = 0; i < hitsRight.Length; i++)
        {
            float distance = Mathf.Abs(hitsRight[i].point.x - transform.position.x);
            if (distance < 0.4f)
            {
                canMoveRigth = true;
                nodeRight = hitsRight[i].collider.gameObject;
            }
        }
        RaycastHit2D[] hitsLeft;
        //Raycast hacia abajo
        hitsLeft = Physics2D.RaycastAll(transform.position, -Vector2.right);
        for (int i = 0; i < hitsLeft.Length; i++)
        {
            float distance = Mathf.Abs(hitsLeft[i].point.x - transform.position.x);
            if (distance < 0.4f)
            {
                canMoveLeft = true;
                nodeLeft = hitsLeft[i].collider.gameObject;
            }
        }

    }

    void Update()
    {

    }
    public GameObject GetNodeFromDirection(string direction)
    {
        if (direction == "left" && canMoveLeft)
        {
            return nodeLeft;
        }
        if (direction == "right" && canMoveRigth)
        {
            return nodeRight;
        }
        if (direction == "up" && canMoveUp)
        {
            return nodeUp;
        }
        if (direction == "down" && canMoveDown)
        {
            return nodeDown;
        }
        else
        {
            return null;
        }
    }
}
