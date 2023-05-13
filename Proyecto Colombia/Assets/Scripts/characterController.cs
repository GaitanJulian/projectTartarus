using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterController : MonoBehaviour
{
    [SerializeField] private movementStatsScriptableObject movementStats;

    private float timeCounter;
    private float aceleration;

    // Start is called before the first frame update
    void Start()
    {
        timeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        movement.Normalize();
        if (movement != Vector3.zero) 
        {      
            if (timeCounter >= 5)
            {
                timeCounter = 5;
            }
            else
            {
                timeCounter += Time.deltaTime * 5;
            }

            aceleration = acelerationModifier(timeCounter);
            transform.position += movement * aceleration * movementStats.maxSpeed * Time.deltaTime;

        }
        else
        {
            timeCounter = 0;
        }



    }



    private float acelerationModifier(float time)
    {

        return -Mathf.Exp(-time) + 1;
    }


}
