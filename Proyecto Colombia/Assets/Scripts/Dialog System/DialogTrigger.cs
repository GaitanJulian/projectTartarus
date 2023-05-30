using UnityEngine;
using Fungus;

public class DialogTrigger : MonoBehaviour
{
    public Flowchart flowchart;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            flowchart.ExecuteBlock("TestBlock");
        }
    }
}