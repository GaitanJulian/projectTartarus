using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acordionist : MonoBehaviour
{
    [SerializeField] GameObject onda;
    [SerializeField] Transform salida;
    [SerializeField] float force, radio, timeatack, distance;
    float time;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&time<=0)
        {
            shoot();
            time = timeatack;
        }
        if (time >= 0)
        {
            time -= Time.deltaTime;
        }
    }
    void shoot()
    {
        
        StartCoroutine(wait(0.2f));

    }
    IEnumerator wait(float sec)
    {
        GameObject[] Ondas = new GameObject[3];
        float t = distance;
        for (int i = 0; i < Ondas.Length; i++)
        {

            Ondas[i] = Instantiate(onda, salida.position, salida.rotation);
            Ondas[i].GetComponent<Rigidbody2D>().velocity = new Vector2(force, 0);
            Ondas[i].transform.localScale = new Vector2(radio-i*3,radio-i*3);
            Ondas[i].GetComponent<OndaDestroy>().distance = t;
            t /= 2;
           
            yield return new WaitForSeconds(sec);

        }
        
    }

}
