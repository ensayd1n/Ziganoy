using UnityEngine;

public class XPController : MonoBehaviour
{
    private GameController _gameController;
    [HideInInspector] public bool MoveLock = true;

    private void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
    }

    private void FixedUpdate()
    {
        Rotate();
        if (MoveLock != true)
        {
            Movement();
        }
    }
    #region Interaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerTakenDamageArea"))
        {
            _gameController.IncreaseXP(30);
            gameObject.SetActive(false);
            MoveLock = true;
        }
    }
    #endregion
    #region Move
    public void Movement()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            GameObject.FindGameObjectWithTag("PlayerTakenDamageArea").transform.position,
            5 * Time.deltaTime);
    }
    private void Rotate()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 30);
    }
    #endregion
    
}
