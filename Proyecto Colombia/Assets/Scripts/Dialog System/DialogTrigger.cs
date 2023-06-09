using UnityEngine;
using Fungus;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private Flowchart _flowchart;
    [SerializeField] private GameObject _conversationSprite ;
    private string PLAYERTAG = "Player";
    private string CONVERSATIONBLOCK = "Conversation";

    private void Start()
    {
        _conversationSprite.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYERTAG))
        {
            CharacterController characterController = other.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.OnInteractionKeyPressed += StartDialog;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CharacterController characterController = other.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.OnInteractionKeyPressed -= StartDialog;
        }
    }

    private void StartDialog()
    {
        _conversationSprite.SetActive(false); // Hide the thinking sprite
        _flowchart.ExecuteBlock(CONVERSATIONBLOCK);
    }

    public void ActivateBubble()
    {
        _conversationSprite.SetActive(true);
    }

}