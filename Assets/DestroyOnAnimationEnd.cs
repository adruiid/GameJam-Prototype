using UnityEngine;

public class DestroyOnAnimationEnd : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Destroys the GameObject as soon as it transitions out of Tremor_down
        Destroy(animator.gameObject);
    }
}