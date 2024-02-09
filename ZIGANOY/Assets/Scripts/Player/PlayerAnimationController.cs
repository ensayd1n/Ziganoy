using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator = new Animator();

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    #region PlayerAnimations
    public void SetWallkingAnimation(bool isActive)
    {
        _animator.SetBool("Wallking",isActive);
    }
    public void SetDamageAnimation(bool isActive)
    {
        _animator.SetBool("Damage",isActive);
    }
    public void SetMagicAttackAnimation(bool isActive)
    {
        _animator.SetBool("MagicAttack",isActive);
    }
    public void SetHeallingAnimation(bool isActive)
    {
        _animator.SetBool("Healling",isActive);
    }

    public void SetVictoryAnimation(bool isActive)
    {
        _animator.SetBool("Victory",isActive);
    }

    public void SetDieAnimation(bool isActive)
    {
        _animator.SetBool("Die",isActive);
    }
    #endregion
}
