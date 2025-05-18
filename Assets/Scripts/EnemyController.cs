using UnityEngine;

public class EnemyController : MonoBehaviour
{
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
    void Awake()
    {
        moveController = GetComponent<MoveController>();
        if (ghostType == GhostType.red)
        {
            ghostNodeState = GhostNodesStatesEnum.starNode;
            startingNode = ghostNodeStart;

        }
        else if (ghostType == GhostType.pink)
        {
            ghostNodeState = GhostNodesStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodeState = GhostNodesStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodeState = GhostNodesStatesEnum.rightNode;
            startingNode = ghostNodeRight;
        }
        moveController.currentNode = startingNode;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReachCenterNOde(NodeController nodeController)
    {
        if (ghostNodeState == GhostNodesStatesEnum.movingNodes)
        {

        }
        else if (ghostNodeState == GhostNodesStatesEnum.respawing)
        {

        }
        else
        {
            //Si el fantasma está listo para sali del spawn
            if (readyToLeaveHome)
            {
                //Si estaba en el izquierdo se mueve al centro
                if (ghostNodeState == GhostNodesStatesEnum.leftNode)
                {
                    ghostNodeState = GhostNodesStatesEnum.centerNode;
                }
                //Si estaba en el derecho se mueve al centro
                else if (ghostNodeState == GhostNodesStatesEnum.rightNode)
                {
                    ghostNodeState = GhostNodesStatesEnum.centerNode;
                }
                //Si estaba en el centro se mueve al startNode
                else if (ghostNodeState == GhostNodesStatesEnum.centerNode)
                {
                    ghostNodeState = GhostNodesStatesEnum.starNode;
                }
                //Si estaba en el starNode se puede empezar a mover por los demás Nodos
                else if (ghostNodeState == GhostNodesStatesEnum.starNode)
                {
                    ghostNodeState = GhostNodesStatesEnum.movingNodes;
                }
            }
        }

    }
}
