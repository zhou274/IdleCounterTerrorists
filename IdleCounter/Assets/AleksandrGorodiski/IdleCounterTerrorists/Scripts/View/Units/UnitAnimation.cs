using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    private Animator anim;

    private void OnEnable ()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayWalkAnimation()
    {
        PlayAnimation("Walk", GetRandomDelay());
    }

    public void PlayAttackAnimation()
    {
        PlayAnimation("Attack", 0f);
    }

    public void PlayShopAnimation()
    {
        PlayAnimation("Shop", GetRandomDelay());
    }

    public void PlayIdleAnimation()
    {
        PlayAnimation("Idle", GetRandomDelay());
    }

    public virtual void PlayDeadAnimation()
    {
        PlayAnimation("Dead", 0f);
    }

    public virtual void PlayDanceAnimation()
    {
        PlayAnimation("Dance", GetRandomDelay());
    }

    public void PlayAnimation(string clip, float delay)
    {
        anim.PlayInFixedTime(clip, 0, delay);
    }

    public virtual void PlayAnimationOnHire()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("OnHire")) PlayAnimation("OnHire", 0f);
    }

    float GetRandomDelay()
    {
        return Random.Range(0f, 1f);
    }
}