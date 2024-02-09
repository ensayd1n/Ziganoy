using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BookController : MonoBehaviour
{
   private GameController _gameController;
   private GameObject _magicSkillButton;
   private ParticleSystem _magicEffect;
   
   private void Start()
   {
      _gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
      _magicEffect = GameObject.FindGameObjectWithTag("MagicEffect").GetComponent<ParticleSystem>();
      _magicSkillButton = _gameController.InterfaceObjects[1];
   }
   private void FixedUpdate()
   {
      Movement();
      Rotate();
   }

   #region Interaction
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("PlayerTakenDamageArea"))
      {
         GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>().MoveLock = true;
         GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimationController>().SetWallkingAnimation(false);
         GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimationController>().SetVictoryAnimation(true);
         _magicEffect.Play();
         _gameController.ZombieMovementLock = true;
         _gameController.ZombieAttackLock = true;
         transform.DOMoveY(transform.position.y - 2, 2);
         StartCoroutine(SetLocksFalseTimer(2F));
      }
   }
   #endregion
   #region Move
   private void Movement()
   {
      if (gameObject.transform.position.y == 0.75F)
      {
         gameObject.transform.DOMoveY(1.75F, 2);
      }
      else if (gameObject.transform.position.y == 1.75F)
      {
         gameObject.transform.DOMoveY(0.75F, 2);
      }
   }

   private void Rotate()
   {
      transform.Rotate(Vector3.up, Time.deltaTime * 30);
   }
   #endregion
   
   #region Events
   private IEnumerator SetLocksFalseTimer(float time)
   {
      yield return new WaitForSecondsRealtime(time);
      _magicEffect.Stop();
      GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimationController>().SetVictoryAnimation(false);
      GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>().MoveLock = false;
      _magicSkillButton.GetComponent<Button>().interactable = true;
      _gameController.ResetMagicSkillReloadTimer();
      _gameController.ZombieMovementLock = false;
      _gameController.ZombieAttackLock = false;
      gameObject.SetActive(false);
   }
   #endregion
}
