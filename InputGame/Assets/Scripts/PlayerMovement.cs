using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class PlayerMovement : MonoBehaviour
{
    Vector2 movementInput;

    public Animator animator;
    public Button jumpButton;
    
    public GameObject CameraTarget;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private Vector2 _input;
    // Start is called before the first frame update
    private void Start()
    {
        _cinemachineTargetYaw = CameraTarget.transform.rotation.eulerAngles.y;
        //_cinemachineTargetPitch = CameraTarget.transform.rotation.eulerAngles.x;
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
        
        Debug.Log(movementInput);
    }
   public void OnJump(InputValue value)
    {
        Debug.Log("Jump");
    }

    public void Move(Vector2 move)
    {
        animator.SetFloat("Speed", move.magnitude);
        Debug.Log(move);
    }
    // Function to rotate the player to the direction of the camera joystick input
    public void Look(Vector2 look)
    {
        if (look.magnitude < 0.1f)
        {
            return;
        }
        float angle = Mathf.Atan2(look.x, look.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
        //when the player stops moving, the player will face the direction of the last input
    }
    public void Jump()
    {
        animator.SetBool("Jump", true);
        StartCoroutine(JumpRoutine());
        Debug.Log("Jump");
    }
    public void IsGrounded()
    {
        animator.SetBool("Jump", false);
        
    }

    public IEnumerator JumpRoutine()
    {
        jumpButton.interactable = false;
        yield return new WaitForSeconds(1.5f);
        IsGrounded();
        jumpButton.interactable = true;
    }

    public void Aim(Vector2 aim)
    {
        _input = aim;
    }
    public void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.sqrMagnitude >= 0.01f)
        {
            Debug.Log("camare" + _input);
            //Don't multiply mouse input by Time.deltaTime;
            _cinemachineTargetYaw += _input.x ;
            _cinemachineTargetPitch += _input.y;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, -30, 70);

        // Cinemachine will follow this target
        CameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + 0, _cinemachineTargetYaw, 0.0f);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }
}
