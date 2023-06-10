using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Accordionist : MonoBehaviour
{
    [SerializeField] GameObject onda;
    [SerializeField] float force, radio, timeatack, distance;
    private PlayerInputActions playerControls;
    CharacterController movControll;
    private InputAction Attack;
    Vector2 direccion = Vector2.right;
    float time;
    void Awake()
    {
        playerControls = new PlayerInputActions();
        movControll = gameObject.GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        Attack = playerControls.Player.Attack;
        Attack.Enable();
    }

    private void OnDisable()
    {
        Attack.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        if (Attack.ReadValue<float>()>0 &&time<=0)
        {
            shoot();
            time = Attack.ReadValue<float>();
        }
        if (time > 0)
        {
            time -= Time.deltaTime;
        }

    }
    void shoot()
    {
        //funcion de disparo para la espera entre la generacion de cada onda
        if ( movControll.ReturnDirection() == null || movControll.ReturnDirection() == Vector2.zero) direccion = Vector2.right;
        else direccion = movControll.ReturnDirection();

        StartCoroutine(wait(0.2f));
    }
    IEnumerator wait(float sec)
    {
        //se instancian 3 ondas es decir el mismo prefab se instancia 3 veces cambiando el valor del radio y la distancia
        GameObject[] Ondas = new GameObject[3];
        float t = distance;
        float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        angle += 220;
        for (int i = 0; i < Ondas.Length; i++)
        {
            Vector2 salida = new Vector2(transform.position.x + direccion.x/2,transform.position.y+ direccion.y/2   );
            Ondas[i] = Instantiate(onda, salida, Quaternion.AngleAxis(angle, Vector3.forward));
           
            Ondas[i].GetComponent<Rigidbody2D>().velocity = direccion*force;
            Ondas[i].transform.localScale = new Vector2(radio-i*3,radio-i*3)/3;
            Ondas[i].GetComponent<OndaDestroy>().distance = t;
            t /= 2;

            Ondas[i].GetComponent<OndaDestroy>()._direction = direccion;
           
            yield return new WaitForSeconds(sec);

        }
        
    }

}
