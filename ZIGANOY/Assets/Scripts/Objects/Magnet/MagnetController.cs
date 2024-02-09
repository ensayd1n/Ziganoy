using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MagnetController : MonoBehaviour
{
    private GameController _gameController;
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
            _gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
            for (int i = 1; i < _gameController.InstantiatedXP.Length; i++)
            {
                _gameController.InstantiatedXP[i].GetComponent<XPController>().MoveLock = false;
            }
            gameObject.transform.DOMoveY(gameObject.transform.position.y - 2, 2F);
            StartCoroutine(SetActiveFalseTimer(3F));
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
    private IEnumerator SetActiveFalseTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        gameObject.SetActive(false);
    }
    #endregion
}
