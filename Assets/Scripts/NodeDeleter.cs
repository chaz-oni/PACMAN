using Unity.VisualScripting;
using UnityEngine;

public class NodeDeleter : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Node")
        {
            Debug.Log("hola we");
            Destroy(collision.gameObject);
        }
    }
}
