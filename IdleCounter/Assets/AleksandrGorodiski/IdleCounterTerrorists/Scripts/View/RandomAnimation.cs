using UnityEngine;

public sealed class RandomAnimation : MonoBehaviour
{
    void Start()
    {
        ProcessAnimator();
        ProcessAnimation();
    }

    private void ProcessAnimator()
    {
        Animator animator = GetComponent<Animator>();

        if (null != animator)
        {
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, Random.Range(0f, 1f));
        }
    }

    private void ProcessAnimation()
    {
        Animation animation = GetComponent<Animation>();

        if (null != animation && null != animation.clip)
        {
            animation.Play();
            animation[animation.clip.name].normalizedTime = Random.Range(0f, 1f);
        }
    }
}