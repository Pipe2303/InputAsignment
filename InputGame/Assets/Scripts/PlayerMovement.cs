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
    
    // Start is called before the first frame update
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
    // Function to rotate the player to the direction of the joystick input
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
}
