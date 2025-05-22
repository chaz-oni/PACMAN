using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator ghost;
    public enum GhostNodesStatesEnum
    {
        respawing,
        leftNode,
        rightNode,
        centerNode,
        starNode,
        movingNodes

    }
    public GhostNodesStatesEnum ghostNodeState;
    public GhostNodesStatesEnum startGhostNodeState;
    public GhostNodesStatesEnum respawnState;
    public enum GhostType
    {
        red,
        blue,
        pink,
        orange
    }

    public GhostType ghostType;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeStart;
    public GameObject ghostNodeCenter;

    public MoveController moveController;
    public GameObject startingNode;
    public bool readyToLeaveHome = false;
    public GameManager gameManager;
    public bool testRespawn = false;
    public bool isFrightened = false;
    public GameObject[] scatterNodes;
    public int scatterNodeIndex;

    public bool leftHomeBefore = false;
    public bool isVisible = true;

    public SpriteRenderer ghostSprite;

    //AWAKE

    void Awake()
    {
        ghostSprite = GetComponent<SpriteRenderer>();
        scatterNodeIndex = 0;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        moveController = GetComponent<MoveController>();
        //Se determina el movimiento específico de cada fantasma
        if (ghostType == GhostType.red)
        {
            startGhostNodeState = GhostNodesStatesEnum.starNode;
            respawnState = GhostNodesStatesEnum.centerNode;
            startingNode = ghostNodeStart;
            readyToLeaveHome = true;
            leftHomeBefore = true;

        }
        else if (ghostType == GhostType.pink)
        {
            startGhostNodeState = GhostNodesStatesEnum.centerNode;
            respawnState = GhostNodesStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
        }
        else if (ghostType == GhostType.blue)
        {
            startGhostNodeState = GhostNodesStatesEnum.leftNode;
            respawnState = GhostNodesStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
        }
        else if (ghostType == GhostType.orange)
        {
            startGhostNodeState = GhostNodesStatesEnum.rightNode;
            respawnState = GhostNodesStatesEnum.rightNode;
            startingNode = ghostNodeRight;
        }

        moveController.currentNode = startingNode;
        transform.position = startingNode.transform.position;

    }

    //SETUP de inicio de juego
    public void Setup()
    {
        ghostNodeState = startGhostNodeState;
        readyToLeaveHome = false;
        moveController.currentNode = startingNode;
        transform.position = startingNode.transform.position;

        scatterNodeIndex = 0;
        isFrightened = false;
        leftHomeBefore = false;

        if (ghostType == GhostType.red)
        {
            readyToLeaveHome = true;
            leftHomeBefore = true;
        }
        else if (ghostType == GhostType.pink)
        {
            readyToLeaveHome = true;
        }
        SetVisible(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (ghostNodeState != GhostNodesStatesEnum.movingNodes || !gameManager.isPowerPelletRunning)
        {
            isFrightened = false;
        }

        //Show Spritess
        if (isVisible)
        {
            if (ghostNodeState != GhostNodesStatesEnum.respawing)
            {
                ghostSprite.enabled = true;
            }
            else
            {
                ghostSprite.enabled = false;
            }
            ghostSprite.enabled = true;
        }
        else
        {
            ghostSprite.enabled = false;

        }
        if (!gameManager.isPowerPelletRunning)
        {
            isFrightened = false;
        }
        if (!gameManager.gameIsRunnig)
        {
            return;
        }

        if (isFrightened)
        {
            ghost.SetBool("vulnerable", true);
        }
        else
        {
            ghost.SetBool("vulnerable", false);
        }

        if (testRespawn == true)
        {
            //readyToLeaveHome = false;
            ghostNodeState = GhostNodesStatesEnum.respawing;
            testRespawn = false;
        }
        if (moveController.currentNode.GetComponent<NodeController>().isSideNode)
        {
            moveController.SetSpeed(1);
        }
        else
        {
            if (isFrightened)
            {
                moveController.SetSpeed(1);
            }
            else if (ghostNodeState == GhostNodesStatesEnum.respawing)
            {
                moveController.SetSpeed(5);
            }
            else
            {
                moveController.SetSpeed(2);
            }

        }

    }

    //Cuando pacaman come los power pellets
    public void SetFrightened(bool newIsFrightened)
    {
        isFrightened = newIsFrightened;
    }

    //Alcanzar el centro de los nodos.
    public void ReachCenterNOde(NodeController nodeController)
    {
        if (ghostNodeState == GhostNodesStatesEnum.movingNodes)
        {
            leftHomeBefore = true;
            //Scatter Mode, se mueve en sus nodos de scatter
            if (gameManager.currentGhostMode == GameManager.GhostMode.scatter)
            {
                DeterminateGhosteScatterModeDirection();


            }
            //Se mueve de forma aleatoria
            else if (isFrightened)
            {
                string direction = GetRandomDirection();
                moveController.SetDirection(direction);

            }
            //Chase Mode, el fantasma persigue a pacman
            else
            {
                if (ghostType == GhostType.red)
                {
                    DetermineRedGhostDirection();
                }
                else if (ghostType == GhostType.pink)
                {
                    DeterminePinkGhostDirection();
                }
                else if (ghostType == GhostType.blue)
                {
                    DetermineBlueGhostDirection();
                }
                else if (ghostType == GhostType.orange)
                {
                    DetermineOrangeGhostDirection();
                }
            }
        }

        else if (ghostNodeState == GhostNodesStatesEnum.respawing)
        {
            string direction = "";
            //Llega al nodo Start y baja
            if (transform.position.x == ghostNodeStart.transform.position.x && transform.position.y == ghostNodeStart.transform.position.y)
            {
                direction = "down";

            }
            //Llega al nodo Center
            else if (transform.position.x == ghostNodeCenter.transform.position.x && transform.position.y == ghostNodeCenter.transform.position.y)
            {
                if (respawnState == GhostNodesStatesEnum.centerNode)
                {
                    ghostNodeState = respawnState;
                }
                //Para los fantasmas azul y naranja que están en los nodos de las orillas
                else if (respawnState == GhostNodesStatesEnum.leftNode)
                {
                    direction = "left";

                }
                else if (respawnState == GhostNodesStatesEnum.rightNode)
                {
                    direction = "right";
                }
            }
            else if (
                (transform.position.x == ghostNodeLeft.transform.position.x && transform.position.y == ghostNodeLeft.transform.position.y)
                || (transform.position.x == ghostNodeRight.transform.position.x && transform.position.y == ghostNodeRight.transform.position.y)
            )
            {
                ghostNodeState = respawnState;
            }
            else
            {
                direction = GetClosestDirection(ghostNodeStart.transform.position);

            }

            moveController.SetDirection(direction);

        }
        else
        {
            //Si el fantasma está listo para salir del spawn
            if (readyToLeaveHome)
            {
                //Si estaba en el izquierdo se mueve al centro
                if (ghostNodeState == GhostNodesStatesEnum.leftNode)
                {
                    ghostNodeState = GhostNodesStatesEnum.centerNode;
                    moveController.SetDirection("right");
                }
                //Si estaba en el derecho se mueve al centro
                else if (ghostNodeState == GhostNodesStatesEnum.rightNode)
                {
                    ghostNodeState = GhostNodesStatesEnum.centerNode;
                    moveController.SetDirection("left");
                }
                //Si estaba en el centro se mueve al startNode
                else if (ghostNodeState == GhostNodesStatesEnum.centerNode)
                {
                    ghostNodeState = GhostNodesStatesEnum.starNode;
                    moveController.SetDirection("up");
                }
                //Si estaba en el starNode se puede empezar a mover por los demás Nodos
                else if (ghostNodeState == GhostNodesStatesEnum.starNode)
                {
                    ghostNodeState = GhostNodesStatesEnum.movingNodes;
                    moveController.SetDirection("left");
                }
            }
        }

    }

    //*****Direción random para cuando huyen*****//

    string GetRandomDirection()
    {
        List<string> possibleDirection = new List<string>();
        NodeController nodeController = moveController.currentNode.GetComponent<NodeController>();
        if (nodeController.canMoveDown && moveController.lastMovingDirection != "up")
        {
            possibleDirection.Add("down");
        }
        if (nodeController.canMoveUp && moveController.lastMovingDirection != "down")
        {
            possibleDirection.Add("up");
        }
        if (nodeController.canMoveLeft && moveController.lastMovingDirection != "right")
        {
            possibleDirection.Add("left");
        }
        if (nodeController.canMoveRigth && moveController.lastMovingDirection != "left")
        {
            possibleDirection.Add("right");
        }
        string direction = "";
        int randomDirectionIndex = Random.Range(0, possibleDirection.Count);
        if (randomDirectionIndex < 0)
        {
            randomDirectionIndex *= -1;
        }
        direction = possibleDirection[randomDirectionIndex];
        return direction;
    }
    void DeterminateGhosteScatterModeDirection()
    {
        if (transform.position.x == scatterNodes[scatterNodeIndex].transform.position.x && transform.position.y == scatterNodes[scatterNodeIndex].transform.position.y)
        {
            scatterNodeIndex++;
            if (scatterNodeIndex == scatterNodes.Length - 1)
            {
                scatterNodeIndex = 0;
            }

        }
        string direction = GetClosestDirection(scatterNodes[scatterNodeIndex].transform.position);
        moveController.SetDirection(direction);

    }

    //***********MOVIMIENTO DE BLINKY*********//
    void DetermineRedGhostDirection()
    {
        string direction = GetClosestDirection(gameManager.pacman.transform.position);
        moveController.SetDirection(direction);

    }
    //***********MOVIMIENTO DE PINKY*********//
    void DeterminePinkGhostDirection()
    {
        string pacmandirection = gameManager.pacman.GetComponent<MoveController>().lastMovingDirection;
        float distanceBetweenNodes = 0.35f;
        Vector2 target = gameManager.pacman.transform.position;
        if (pacmandirection == "left")
        {
            target.x -= (distanceBetweenNodes * 2);

        }
        else if (pacmandirection == "right")
        {
            target.x += (distanceBetweenNodes * 2);
        }
        else if (pacmandirection == "up")
        {
            target.y += (distanceBetweenNodes * 2);
        }
        else if (pacmandirection == "down")
        {
            target.y -= (distanceBetweenNodes * 2);
        }
        string direction = GetClosestDirection(target);
        moveController.SetDirection(direction);
    }
    //***********MOVIMIENTO DE INKY*********//
    void DetermineBlueGhostDirection()
    {
        string pacmandirection = gameManager.pacman.GetComponent<MoveController>().lastMovingDirection;
        float distanceBetweenNodes = 0.35f;
        Vector2 target = gameManager.pacman.transform.position;
        if (pacmandirection == "left")
        {
            target.x -= (distanceBetweenNodes * 2);

        }
        else if (pacmandirection == "right")
        {
            target.x += (distanceBetweenNodes * 2);
        }
        else if (pacmandirection == "up")
        {
            target.y += (distanceBetweenNodes * 2);
        }
        else if (pacmandirection == "down")
        {
            target.y -= (distanceBetweenNodes * 2);
        }
        GameObject redGhost = gameManager.redGhost;
        float xDistance = target.x - redGhost.transform.position.x;
        float yDistance = target.x - redGhost.transform.position.y;

        Vector2 blueTarget = new Vector2(target.x + xDistance, target.y + yDistance);
        string direction = GetClosestDirection(blueTarget);
        moveController.SetDirection(direction);

    }
    //***********MOVIMIENTO DE CLYDE*********//
    void DetermineOrangeGhostDirection()
    {

        float distance = Vector2.Distance(gameManager.pacman.transform.position, transform.position);
        float scatterThreshold = 0.35f * 8;
        if (distance < 0)
        {
            distance *= -1;
        }
        if (distance <= scatterThreshold)
        {
            DetermineRedGhostDirection();
        }
        else
        {
            DeterminateGhosteScatterModeDirection();

        }


    }





    //Función para obtener la dirección más corta para alcanzar otor nodo.
    string GetClosestDirection(Vector2 target)
    {
        float shortestDistance = 0;
        string lastMovingDirection = moveController.lastMovingDirection;
        string newDirection = "";
        NodeController nodeController = moveController.currentNode.GetComponent<NodeController>();
        if (nodeController.canMoveUp && lastMovingDirection != "down")
        {
            GameObject nodeUp = nodeController.nodeUp;
            //Distancia entre el nodo superior y pacman
            float distance = Vector2.Distance(nodeUp.transform.localPosition, target);
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "up";
            }
        }
        if (nodeController.canMoveDown && lastMovingDirection != "up")
        {
            GameObject nodeDown = nodeController.nodeDown;
            //Distancia entre el nodo superior y pacman
            float distance = Vector2.Distance(nodeDown.transform.localPosition, target);
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "down";
            }
        }
        if (nodeController.canMoveLeft && lastMovingDirection != "right")
        {
            GameObject nodeLeft = nodeController.nodeLeft;
            //Distancia entre el nodo superior y pacman
            float distance = Vector2.Distance(nodeLeft.transform.localPosition, target);
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "left";
            }
        }
        if (nodeController.canMoveRigth && lastMovingDirection != "left")
        {
            GameObject nodeRight = nodeController.nodeRight;
            //Distancia entre el nodo superior y pacman
            float distance = Vector2.Distance(nodeRight.transform.localPosition, target);
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "right";
            }
        }
        return newDirection;
    }
    public void SetVisible(bool newVisible)
    {
        isVisible = newVisible;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && ghostNodeState != GhostNodesStatesEnum.respawing)
        {
            if (isFrightened)
            {
                gameManager.GhostEaten();
                ghostNodeState = GhostNodesStatesEnum.respawing;

            }
            else
            {
                StartCoroutine(gameManager.PlayerEaten());

            }
        }
    }
}
