using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BombController : MonoBehaviour
{
    private GameController GameController;
    private bool _moveLock = false;
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
            StartCoroutine(SetActiveBombByTimer((2.1F)));
            GameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
            GameController.ZombieMovementLock = true;
            GameController.ZombieAttackLock = true;
            int destroyedEnemyIndex=0;
            for (int i = 1; i < GameController.InstantiatedZombies.Length; i++)
            {
                if (GameController.InstantiatedZombies[i].activeSelf != false)
                {
                    GameObject obj = GameController.InstantiatedZombies[i];
                    obj.GetComponent<ZombieAnimationController>().SetWallkingAnimation(false);
                    obj.GetComponent<ZombieOtherController>().TakenDamageParticleSystem.Play();
                    StartCoroutine(SetTakenBombCloserTimer(1.1F, obj));
                    destroyedEnemyIndex++;
                }
            }
            for (int i = 0; i < destroyedEnemyIndex/2; i++)
            {
                GameController.InstantiatingZombie();
            }
            transform.DOMoveY(transform.position.y - 2, 2);
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
    private IEnumerator SetTakenBombCloserTimer(float time,GameObject obj)
    {
        yield return new WaitForSecondsRealtime(time);
        obj.GetComponent<ZombieOtherController>().TakenDamageParticleSystem.Stop();
        obj.GetComponent<ZombieMovementController>().MoveLock = true;
        obj.GetComponent<ZombieOtherController>().TakenDamage(obj.GetComponent<ZombieOtherController>().CurrentHealth);
        GameController.ZombieMovementLock = false;
        GameController.ZombieAttackLock = false;
    }

    private IEnumerator SetActiveBombByTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        gameObject.SetActive(false);
    }
    #endregion
}
