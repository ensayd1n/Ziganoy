using System.Collections;
using UnityEngine;

public class ZombieAttackController : MonoBehaviour
{
  [HideInInspector] public GameController GameController;
  public ZombieAnimationController ZombieAnimationController;
  public ZombieOtherController ZombieOtherController;
  [HideInInspector] public GameObject Zombie;
  public Zombie ZombieType;
  [HideInInspector] public bool AttackingLock = true;
  [HideInInspector] public GameObject Enemy;
  
  private void Start()
  {
    GameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
  }
  
  #region Interaction
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("PlayerTakenDamageArea"))
    {
      AttackingLock = false;
      ZombieAnimationController.SetWallkingAnimation(false);
      ZombieAnimationController.SetAttacAnimation(true);
      StartCoroutine(GivenDamageTimer(1.70F));
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.CompareTag("PlayerTakenDamageArea")&& ZombieOtherController.CurrentHealth > 0 )
    {
      AttackingLock = true;
      ZombieAnimationController.SetAttacAnimation(false);
    }
  }
  #endregion
  #region Events
  private IEnumerator GivenDamageTimer(float time)
  {
    yield return new WaitForSecondsRealtime(time);
    if (AttackingLock != true && GameController.ZombieMovementLock != true && ZombieAnimationController.Animator.GetBool("Attack")!=false && Time.timeScale==1)
    {
      GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOtherController>().TakenDamage(ZombieType.Damage*GameController.XPLevel);
      StartCoroutine(GivenDamageTimer(time)); 
    }
  }
  #endregion
}
