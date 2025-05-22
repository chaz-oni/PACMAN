using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NodeController : MonoBehaviour
{
    [Header("Nodes check")]
    public bool canMoveLeft = false;
    public bool canMoveRigth = false;
    public bool canMoveUp = false;
    public bool canMoveDown = false;

    public GameObject nodeLeft;
    public GameObject nodeRight;
    public GameObject nodeUp;
    public GameObject nodeDown;

    [Header("Warp Nodes")]
    public bool isWarpRightNode = false;
    public bool isWarpLeftNode = false;
    [Header("Collect Pellet")]

    public bool isPelletNode = false;
    public bool hasPellet = false;
    public SpriteRenderer pelletSprite;
    public GameManager gameManager;

    public bool isGhostStartingNode = false;
    public bool isSideNode = false;

    public bool isPowerPellet = false;
    public float powePelletLinkingTimer = 0;
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        foreach (Transform child in transform)
        {
            if (child.name == "Pellet")
            {
                pelletSprite = child.GetComponent<SpriteRenderer>();
                isPelletNode = true;

                // Verifica si el pellet aún está visible
                if (pelletSprite != null && pelletSprite.enabled)
                {
                    hasPellet = true;
                    gameManager.GotPelletFromNodeController(this);
                }
                else
                {
                    hasPellet = false;
                }
                break;
            }
        }

        RaycastHit2D[] hitsDown;
        //Raycast hacia abajo
        hitsDown = Physics2D.RaycastAll(transform.position, -Vector2.up);
        for (int i = 0; i < hitsDown.Length; i++)
        {
            float distance = Mathf.Abs(hitsDown[i].point.y - transform.position.y);
            if (distance < 0.4f && hitsDown[i].collider.tag == "Node")
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
            if (distance < 0.4f && hitsUp[i].collider.tag == "Node")
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
            if (distance < 0.4f && hitsRight[i].collider.tag == "Node")
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
            if (distance < 0.4f && hitsLeft[i].collider.tag == "Node")
            {
                canMoveLeft = true;
                nodeLeft = hitsLeft[i].collider.gameObject;
            }
        }
        if (isGhostStartingNode)
        {
            canMoveDown = true;
            nodeDown = gameManager.ghostNodeCenter;
        }

    }

    void Update()
    {
        if (!gameManager.gameIsRunnig)
        {
            return;
        }
        if (isPowerPellet && hasPellet)
        {
            powePelletLinkingTimer += Time.deltaTime;
            if (powePelletLinkingTimer >= 0.1f)
            {
                powePelletLinkingTimer = 0;
                pelletSprite.enabled = !pelletSprite.enabled;
            }
        }

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
    public void RespawnPellet()
    {
        if (isPelletNode)
        {
            hasPellet = true;
            pelletSprite.enabled = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && hasPellet)
        {
            hasPellet = false;
            pelletSprite.enabled = false;
            StartCoroutine(gameManager.CollectedDots(this));
        }
    }
}
