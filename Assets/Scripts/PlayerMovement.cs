using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // variables
    Vector3 movementVector;
    [SerializeField] float Speed = 8;
    float sneakSpeed;
    [SerializeField] float jumpForce = 1;
    
    bool sneakInput = false;

    // need this for unity event
    public bool isMakingSound;
    public bool isMoving = false;

    // refrences 
    [SerializeField] Animator myAnimator;
    [SerializeField] HealthSO health;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

       rb = GetComponent<Rigidbody>();
       health.RestoreHealth();
        sneakSpeed = Speed * 0.90f;

    }

    // Update is called once per frame
    void Update()
    {
        myAnimator.SetFloat("walkSpeed", movementVector.magnitude);
        myAnimator.SetFloat("ground", myAnimator.transform.position.y); 
        isMakingSound = !sneakInput;

    }
    private void FixedUpdate()
    {
        movementVector.Normalize();
        float currentSpeed = sneakInput ? sneakSpeed : Speed;
       rb.AddForce(movementVector * currentSpeed, ForceMode.Acceleration); 
    }

    void OnMovement(InputValue v)
    {
        Debug.Log("OnMovement Called!");
        Vector2 inputVector = v.Get<Vector2>();
        movementVector = new Vector3(inputVector.x, 0, inputVector.y);
        myAnimator.transform.forward = movementVector.normalized;

        if (movementVector.magnitude > 0)
            isMoving = true;
        else
        {
            isMoving = false;
        }
            
    }

    bool OnSneak(InputValue v)
    {
        return sneakInput = v.isPressed;
    }

}
