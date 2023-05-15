using System.Collections;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void GetDamaged()
    {
        Debug.Log(gameObject.name + ": Ouch :(");
        animator.SetBool("hit", true);
        Invoke("StopAnimationHit", 0.25f);
    }
    void StopAnimationHit()
    {
        animator.SetBool("hit", false);
    }
}
