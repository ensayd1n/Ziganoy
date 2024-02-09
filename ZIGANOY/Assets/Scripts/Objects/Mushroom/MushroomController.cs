using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class MushroomController : MonoBehaviour
{
    private int _damage = 200;
    private int _increasehealth = 200;
    
    #region Interaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerTakenDamageArea"))
        {
            gameObject.transform.DOMoveY(transform.position.y - 3, 2);
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            int randomIndex = Random.Range(0, 2);
            switch (randomIndex)
            {
                case 0: obj.GetComponent<PlayerOtherController>().TakenDamage(_damage); 
                        obj.GetComponent<PlayerOtherController>().DebufParticleSystem.Play();
                        obj.GetComponent<PlayerMovementController>().MoveSpeed = 1;
                        StartCoroutine(StoperDebufParticleSytem(2));
                    break;
                case 1: obj.GetComponent<PlayerOtherController>().IncreaseHealth(_increasehealth);
                    break;
            }
            StartCoroutine(SetActiveMushroom(2.1F));
        }
    }
    #endregion

    #region Events

    private IEnumerator StoperDebufParticleSytem(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        obj.GetComponent<PlayerOtherController>().DebufParticleSystem.Stop();
        obj.GetComponent<PlayerMovementController>().MoveSpeed =
            obj.GetComponent<PlayerOtherController>().PlayerType.MoveSpeed;
    }

    private IEnumerator SetActiveMushroom(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        gameObject.SetActive(false);
    }

    #endregion
}
