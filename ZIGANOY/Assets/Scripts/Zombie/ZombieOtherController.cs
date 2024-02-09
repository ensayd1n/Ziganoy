using System.Collections;
using System.Linq;
using UnityEngine;

public class ZombieOtherController : MonoBehaviour
{
   public ZombieInteractionController ZombieInteractionController;
   private ZombieAnimationController _zombieAnimationController;
   private GameController _gameController;
   public ParticleSystem TakenDamageParticleSystem;
   public ParticleSystem MagicDamageParticleSystem;
   public ParticleSystem SlowDownParticleSystem;

   [HideInInspector] public float CurrentHealth;
   private bool InstatiateLock = false;

   private void Start()
   {
      _zombieAnimationController = GetComponent<ZombieAnimationController>();
      _gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
      CurrentHealth = ZombieInteractionController.ZombieType.Health;
   }
   #region Health
   public void TakenDamage(float damage)
   {
      if (CurrentHealth - damage < 0)
      {
         CurrentHealth = 0;
         CheckHealth();
      }
      else if (CurrentHealth - damage >= 0)
      {
         CurrentHealth -= damage;
         CheckHealth();
      }
   }
   public void FillHealth()
   {
      CurrentHealth=ZombieInteractionController.ZombieType.Health;
   }
    public void CheckHealth()
      {
         if (CurrentHealth <= 0 && gameObject.activeSelf !=false)
         {
            GetComponent<ZombieMovementController>().MoveLock = true;
            _zombieAnimationController.SetWallkingAnimation(false);
            _zombieAnimationController.SetAttacAnimation(false);
            _zombieAnimationController.SetDieAnimation(true);
            StartCoroutine(ZombieSetActiveFalseTimer(3,gameObject));
            _gameController.ScorIncrease();
         }
         else if (CurrentHealth > 0)
         {
            _zombieAnimationController.SetDieAnimation(false);
         }
      }
   #endregion
   
   #region Events
  
   private IEnumerator ZombieSetActiveFalseTimer(float time,GameObject obj)
   {
      yield return new WaitForSecondsRealtime(time);
      obj.SetActive(false);
      gameObject.SetActive(false);
      if (InstatiateLock != true)
      {
         for (int i = 0; i < _gameController.InstantiatedXP.Length; i++)
         {
            GameObject XP = _gameController.InstantiatedXP[i];
            if (_gameController.InstantiatedXP[i]!=null &&XP.activeSelf==false)
            {
               XP.SetActive(true);
               XP.transform.position = new Vector3(transform.position.x, 1F, transform.position.z);
               break;
            }
            if(i==_gameController.InstantiatedXP.Length-1)
            {
               XP =  Instantiate(_gameController.Objects[2], new Vector3(transform.position.x, 1F, transform.position.z),
                  Quaternion.identity);
               _gameController.InstantiatedXP = _gameController.InstantiatedXP.Concat(new GameObject[]{XP}).ToArray();
               break;
            }
         }
      }
      GetComponent<ZombieMovementController>().MoveLock = false;
   }
   #endregion
}
