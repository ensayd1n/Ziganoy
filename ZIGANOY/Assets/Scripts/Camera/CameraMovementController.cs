using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    private Transform _target;
    private void FixedUpdate()
    {
        Movement();
    }

    #region Movement
    private void Movement()
    {
        _target=GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = Vector3.Lerp(transform.position, new Vector3(_target.position.x,
            _target.position.y+8F,
            _target.position.z-7), 0.4F);
    }
    #endregion
   
}
