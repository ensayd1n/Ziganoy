using System.Collections;
using UnityEngine;

public class PlayerSkillsController : MonoBehaviour
{
   public PlayerOtherController PlayerOtherController;
   
   private GameController _gameController;
   public bool GivenMagicDamageLock = true;
   public ParticleSystem MagicAttackParticleSystem;
   private GameObject Player;

   private GameObject DefaultSkillAmmo;
   public Transform Bird;
   public GameObject ClosestZombie;
 
   private void Start()
   {
      _gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
      DefaultSkillAmmo = _gameController.Objects[4];
      Player = GameObject.FindGameObjectWithTag("Player");
      StartCoroutine(DefaultSkillTimer(PlayerOtherController.PlayerType.DamageDuration));
   }

   #region DefaultSkill
   private Transform SearchCLoseZombie()
   {
      float closestDistance = Mathf.Infinity;
      
      for (int i = 1; i < _gameController.InstantiatedZombies.Length; i++)
      {
         if (_gameController.InstantiatedZombies[i].activeSelf != false)
         {
            float distanceToZombie = Vector3.Distance(transform.position, _gameController.InstantiatedZombies[i].transform.position);
            if (distanceToZombie < closestDistance)
            {
               closestDistance = distanceToZombie;
               ClosestZombie = _gameController.InstantiatedZombies[i];
            }
         }
      }
      if (ClosestZombie != null)
      {
         GameObject obj = Instantiate(DefaultSkillAmmo);
         obj.transform.position = Bird.position;
      }
      return ClosestZombie.transform;
   }
   private IEnumerator DefaultSkillTimer(float time)
   {
      yield return new WaitForSecondsRealtime(time);
      SearchCLoseZombie();
      StartCoroutine(DefaultSkillTimer(time));
   }
   
   #endregion
   #region MagicSkill
   public void UsedMagicSkill(GameObject obj)
   {
      obj.GetComponent<ZombieMovementController>().MoveLock = true;
      StartCoroutine(GivenDamageTimer(1,obj));
      StartCoroutine(GivenDamageTimerV2(1.001F, obj));
   }

   public void MagicSkill()
   {
      GivenMagicDamageLock = false;
      MagicAttackParticleSystem.Play();
      Player.GetComponent<PlayerMovementController>().MoveLock = true;
      Player.GetComponent<PlayerAnimationController>().SetWallkingAnimation(false);
      Player.GetComponent<PlayerAnimationController>().SetMagicAttackAnimation(true);
      StartCoroutine(SetFalseMagicAttackAnimation(2.1F));
      StartCoroutine(SetFalseMagicAttackParticle(3));
   }

   private IEnumerator GivenDamageTimer(float time, GameObject obj)
   {
      yield return new WaitForSecondsRealtime(time);
      obj.GetComponent<ZombieOtherController>().MagicDamageParticleSystem.Play();
      obj.GetComponent<ZombieOtherController>().TakenDamage(obj.GetComponent<ZombieOtherController>().CurrentHealth);
      obj.GetComponent<ZombieOtherController>().CheckHealth();
   }

   private IEnumerator GivenDamageTimerV2(float time,GameObject obj)
   {
      yield return new WaitForSecondsRealtime(time);
      obj.GetComponent<ZombieOtherController>().MagicDamageParticleSystem.Stop(); 
      MagicAttackParticleSystem.Stop();
   }
   private IEnumerator SetFalseMagicAttackAnimation(float time )
   {
      yield return new WaitForSecondsRealtime(time);
      Player.GetComponent<PlayerAnimationController>().SetMagicAttackAnimation(false);
   }
   private IEnumerator SetFalseMagicAttackParticle(float time )
   {
      yield return new WaitForSecondsRealtime(time);
      Player.GetComponent<PlayerMovementController>().MoveLock = false;
      GivenMagicDamageLock = true;
   }
   #endregion

   #region EnemySlowDownSkill

   public void EnemySlowDown(GameObject[] array)
   {
      for (int i = 1; i < array.Length; i++)
      {
         GameObject obj = array[i];
         obj.GetComponent<ZombieMovementController>().MoveSpeed = 0.5F;
         obj.GetComponent<ZombieOtherController>().SlowDownParticleSystem.Play();
         StartCoroutine(StoperEnemySlowDownSkill(5, array));
      }
   }
   private IEnumerator StoperEnemySlowDownSkill(float time,GameObject[] array)
   {
      yield return new WaitForSecondsRealtime(time);
      for (int i = 1; i < array.Length; i++)
      {
         GameObject obj = array[i];
         obj.GetComponent<ZombieMovementController>().MoveSpeed = 2;
         obj.GetComponent<ZombieOtherController>().SlowDownParticleSystem.Stop();
      }
   }

   #endregion
   
}
