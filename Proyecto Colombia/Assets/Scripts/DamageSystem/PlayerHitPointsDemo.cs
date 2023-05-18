using Events;
using UnityEngine; //you need to be using this

public class PlayerHitPointsDemo : MonoBehaviour
{
    //This system has its effects in the "PlayerHitPoints" component (script) that should be attached to the player

    //if you want to use the two optional functions listed at the end of this script...
    //...subscribed from this events you should be!
    //PD: as you can see, you have to include the addecuate ENUMs and asign the function that "listens" to them
    private void Awake()
    {
        EventManager.AddListener(ENUM_Player.reachedZero, ListenZero);
        EventManager.AddListener<float>(ENUM_Player.reachedMax, ListenMax);
        //this second one receives a float parameter, you will understand when you finish reading this script!
    }
    //this is a precaution to not have any problems if the object with this script is destroyed:
    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_Player.reachedZero, ListenZero);
        EventManager.RemoveListener<float>(ENUM_Player.reachedMax, ListenMax);
    }

    //this is an example of usage of the "alter (player) hit-points":
    //you use the syntaxis showed here, 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            EventManager.Dispatch(ENUM_Player.alterHitpoints, 3f); //it has to be float!, tell me if we should better use INT to change it
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            EventManager.Dispatch(ENUM_Player.alterHitpoints, -3f); //of course you can use this to add or substract life, why not?
        }
    }

    //this functions are optional. You have to be subscribed to the events to use them:

    //if you wanna know it the player reached zero life from the current script
    private void ListenZero()
    {
        Debug.Log("player reached zero life");
    }

    //if you've got the urge to know if the player tryed to get HP beyond their limits
    private void ListenMax(float magnitude)
    {
        Debug.Log("player reached max life and wasted " + magnitude + " of the potion. Such an arseh*le" );
    }
}
