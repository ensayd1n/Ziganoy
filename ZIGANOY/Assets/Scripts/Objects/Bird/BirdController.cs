using UnityEngine;

public class BirdController : MonoBehaviour
{
    public PlayerSkillsController PlayerSkillsController;
    private void FixedUpdate()
    {
        Rotate();
    }

    #region Rotate
    private void Rotate()
    {
        if (PlayerSkillsController.ClosestZombie != null)
        {
            Transform _target = PlayerSkillsController.ClosestZombie.transform;
            Vector3 _targetDirection = new Vector3(_target.position.x,0,_target.position.z) - new Vector3(transform.position.x,0,transform.position.z);
            Vector3 _direction=Vector3.RotateTowards(transform.forward, _targetDirection, 10*Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(_direction);    
        }
    }
    #endregion
}
