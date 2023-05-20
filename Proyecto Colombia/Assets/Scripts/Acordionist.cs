using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Acordionist : MonoBehaviour
{
    [SerializeField] GameObject onda;
    [SerializeField] Transform salida;
    [SerializeField] float force, radio, timeatack, distance;
    CharacterController movControll;
    Vector2 direccion;
    float time;
    void Start()
    {
        movControll = gameObject.GetComponent<CharacterController>();
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
        direccion = movControll.ReturnDirection();
        StartCoroutine(wait(0.2f));

    }
    IEnumerator wait(float sec)
    {
        GameObject[] Ondas = new GameObject[3];
        float t = distance;
        float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        angle += 220;
        for (int i = 0; i < Ondas.Length; i++)
        {
            
            Ondas[i] = Instantiate(onda, salida.position, Quaternion.AngleAxis(angle, Vector3.forward));
           
            Ondas[i].GetComponent<Rigidbody2D>().velocity = direccion*force;
            Ondas[i].transform.localScale = new Vector2(radio-i*3,radio-i*3);
            Ondas[i].GetComponent<OndaDestroy>().distance = t;
            t /= 2;
           
            yield return new WaitForSeconds(sec);

        }
        
    }

}
