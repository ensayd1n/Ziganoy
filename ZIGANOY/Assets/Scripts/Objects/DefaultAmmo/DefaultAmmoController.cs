using UnityEngine;

public class DefaultAmmoController : MonoBehaviour
{
    private GameController _gameController;

    private void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
        
        Destroy(gameObject,4);
    }

    private void FixedUpdate()
    {
        Movement();
        Rotate();
    }
    #region Move
    private void Movement()
    {
        if (GameObject.FindGameObjectWithTag("MagicSkillArea").GetComponent<PlayerSkillsController>().ClosestZombie !=
            null)
        {
            Transform TargetTransform = GameObject.FindGameObjectWithTag("MagicSkillArea")
                .GetComponent<PlayerSkillsController>().ClosestZombie.transform;
        
            transform.position = Vector3.MoveTowards(
                transform.position,
                TargetTransform.position,
                5 * Time.deltaTime);
        }
    }
    private void Rotate()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 50);
        transform.Rotate(Vector3.left, Time.deltaTime * 50);
    }
    #endregion
    #region Interaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ZombieCharacter"))
        {
            ZombieOtherController zombieOtherController = GameObject.FindGameObjectWithTag("MagicSkillArea").GetComponent<PlayerSkillsController>().ClosestZombie
                .GetComponent<ZombieOtherController>();
            zombieOtherController.TakenDamageParticleSystem.Play();
            zombieOtherController.TakenDamage(
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOtherController>().PlayerType.Damage +
                ((GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOtherController>().PlayerType.Damage /
                  2) * _gameController.XPLevel));
            zombieOtherController.CheckHealth();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Map"))
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
