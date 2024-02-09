using System.Collections;
using UnityEngine;
using DG.Tweening;
public class MealController : MonoBehaviour
{
    private GameController GameController;
    private ParticleSystem _healEffect;

    private void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
        _healEffect = GameObject.FindGameObjectWithTag("HealEffect").GetComponent<ParticleSystem>();
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
            _healEffect.Play();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimationController>().SetWallkingAnimation(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimationController>().SetHeallingAnimation(true);
            GameController.ZombieMovementLock = true;
            GameController.ZombieAttackLock = true;
            transform.DOMoveY(transform.position.y - 2, 2);
            StartCoroutine(SetLocksFalseTimer(2));
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
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOtherController>().IncreaseHealth(500);
        _healEffect.Stop();
        GameController.ZombieMovementLock = false;
        GameController.ZombieAttackLock = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimationController>().SetHeallingAnimation(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>().MoveLock = false;
        gameObject.SetActive(false);
    }
    #endregion
}
