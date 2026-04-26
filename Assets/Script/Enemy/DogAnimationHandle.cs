using UnityEngine;

public class DogAnimationHandle : MonoBehaviour
{
    private DogEnemyAI enemyScript;
    private Animator anim;
    void Start()
    {
        anim= GetComponent<Animator>();
        enemyScript=GetComponentInParent<DogEnemyAI>();
    }

    
    void Update()
    {
        anim.SetBool("attacking", enemyScript.IsAttacking());
        anim.SetBool("IsRunning", enemyScript.IsRunning());
    }
}
