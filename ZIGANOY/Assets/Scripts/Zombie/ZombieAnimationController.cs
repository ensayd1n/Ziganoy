using UnityEngine;
using UnityEngine.Serialization;

public class ZombieAnimationController : MonoBehaviour
{
    public GameObject Zombie;

    [HideInInspector] public Animator Animator = new Animator();

    private void Awake()
    {
        Animator = Zombie.GetComponent<Animator>();
    }
    
    #region Animations

    public void SetWallkingAnimation(bool isActive)
    {
        Animator.SetBool("Wallking",isActive);
    }
    public void SetAttacAnimation(bool isActive)
    {
        Animator.SetBool("Attack",isActive);
    }
    public void SetDieAnimation(bool isActive)
    {
        Animator.SetBool("Die",isActive);
    }
    #endregion
}
