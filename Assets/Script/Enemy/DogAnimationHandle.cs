using UnityEngine;

public class DogAnimationHandle : MonoBehaviour
{
    private MeleeEnemyBehavior enemyScript;
    private Animator anim;
    void Start()
    {
        anim= GetComponent<Animator>();
        enemyScript=GetComponentInParent<MeleeEnemyBehavior>();
    }

    
    void Update()
    {
        anim.SetBool("attacking", enemyScript.IsAttacking());
    }
}
