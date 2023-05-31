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
        _conversationSprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYERTAG))
        {
            _conversationSprite.SetActive(true); // Show the thinking sprite
            CharacterController characterController = other.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.OnInteractionKeyPressed += StartDialog;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _conversationSprite.SetActive(false); // Hide the thinking sprite
        }
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

}