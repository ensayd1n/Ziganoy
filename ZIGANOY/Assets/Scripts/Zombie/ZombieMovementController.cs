using UnityEngine;
using UnityEngine.Serialization;

public class ZombieMovementController : MonoBehaviour
{
    private GameController GameController;
    [HideInInspector] public ZombieInteractionController ZombieInteractionController;
    [HideInInspector] public ZombieAttackController ZombieAttackController;
    [HideInInspector] public ZombieAnimationController ZombieAnimationController;
    [HideInInspector] public bool MoveLock = false;

    [HideInInspector] public float MoveSpeed = 2;

    private void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
    }
    private void FixedUpdate()
    {
        if (GameController.ZombieMovementLock != true&& MoveLock!=true)
        {
            PositionandRotation();  
        }
    }
    
    #region Move
    private void PositionandRotation()
    {
        if (ZombieInteractionController.Enemy!=null&& ZombieAttackController.AttackingLock==true)
        {
            GameObject obj = ZombieInteractionController.Enemy;
            transform.position = Vector3.MoveTowards(
                transform.position,
                obj.transform.position,
                MoveSpeed * Time.deltaTime);
            
            Vector3 _targetDirection = new Vector3(obj.transform.position.x,0,obj.transform.position.z) - new Vector3(transform.position.x,0,transform.position.z);
            Vector3 _direction=Vector3.RotateTowards(transform.forward, _targetDirection, 10*Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(_direction);   
            ZombieAnimationController.SetWallkingAnimation(true);
        }
    }
    #endregion
    
}
