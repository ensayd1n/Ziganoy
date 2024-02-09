using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
   private DynamicJoystick _dynamicJoystick;
   private Rigidbody _rigidbody;
   private PlayerOtherController _playerOtherController;
   private PlayerAnimationController _playerAnimationController;
   [HideInInspector] public bool MoveLock = false;

   public float MoveSpeed=3;

   private void Awake()
   {
      //MoveSpeed = _playerOtherController.PlayerType.MoveSpeed;
   }

   private void Start()
   {
      _rigidbody = gameObject.GetComponent<Rigidbody>();
      _playerOtherController = gameObject.GetComponent<PlayerOtherController>();
      _playerAnimationController = gameObject.GetComponent<PlayerAnimationController>();
      _dynamicJoystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
   } 
   private void FixedUpdate()
   {
      if (_playerOtherController.CurrentyHealty > 0 && MoveLock!=true)
      {
         Movement();
         Rotation();  
      }
   }
   
   
   #region Move
   private void Movement()
   {
      _rigidbody.velocity = new Vector3(_dynamicJoystick.Horizontal * MoveSpeed, 0,
         _dynamicJoystick.Vertical *  MoveSpeed);

      if (_dynamicJoystick.Horizontal != 0 || _dynamicJoystick.Vertical != 0)
      {
         _playerAnimationController.SetWallkingAnimation(true);
      }
      else
      {
         _playerAnimationController.SetWallkingAnimation(false);
      }
   }
   private void Rotation()
   {
      float _rotationAngle = Mathf.Atan2(-_dynamicJoystick.Horizontal*-1, -_dynamicJoystick.Vertical*-1) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.Euler(0f, _rotationAngle, 0f);
   }
   #endregion
  
}
