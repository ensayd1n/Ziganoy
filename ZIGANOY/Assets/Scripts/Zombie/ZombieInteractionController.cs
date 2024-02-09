using UnityEngine;

public class ZombieInteractionController : MonoBehaviour
{
    public ZombieAnimationController ZombieAnimationController;
    public GameObject Zombie;
    public Zombie ZombieType;
    public bool AttackingLock = true;
    public GameObject Enemy;
  
    #region Interaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AttackingLock = false;
            Enemy = other.gameObject;
            ZombieAnimationController.SetWallkingAnimation(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AttackingLock = true;
            Enemy = null;
            ZombieAnimationController.SetWallkingAnimation(false);
        }
    }
    #endregion
}
