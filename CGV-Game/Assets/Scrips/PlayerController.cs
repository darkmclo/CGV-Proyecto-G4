using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalMove;
    public float verticalMove;
    public CharacterController player;
    public float playerSpeed;
    private Vector3 playerInput;
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 movePlayer;
    public float gravity = 9.8f;
    public float fallVelocity;
    public float jumpForce;

    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float slideVelocity;
    public float slopeForceDown;

    //variables animaciones
    public Animator playeranimatorcontroller;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        playeranimatorcontroller = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //guardamos los valores de entrada vertical y horizontal
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);
        playeranimatorcontroller.SetFloat("PlayerWalkVelocity", playerInput.magnitude * playerSpeed);

        camDirection(); //llama ala funcion de camDirection
        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer = movePlayer * playerSpeed;

        player.transform.LookAt(player.transform.position + movePlayer);
        SetGravity();
        PlayerSkill();
        player.Move(movePlayer * playerSpeed * Time.deltaTime);
    

    }

    //La direccion a donde mira la camara
    void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    //Gravedad
    void SetGravity()
    {
        if (player.isGrounded)
        {
            //la velocidad de caida es igual ala gravedad en valor negativo * time.deltatime
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else //sino
        {
            //aceleramos la caida cada frame  restandole el valor de la gravedad * time.deltatime.
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
            playeranimatorcontroller.SetFloat("PlayerVelocity", player.velocity.y);
        }


        playeranimatorcontroller.SetBool("IsGrounded", player.isGrounded);
        SlideDown();  // llamamos ala funcion Slidedown() para comprobar si estamos en una pendiente
    }
    
    //Habilidades para el jugador
     public void PlayerSkill()
    {
        //si estamos tocando el suelo y pulsamos el boton de "jump"
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce; //la velocidad de caida pasa a ser el igual a la velocidad de salto
            movePlayer.y = fallVelocity; // pasamos el valor a moveplayer.y
            playeranimatorcontroller.SetTrigger("PlayerJummp");
        }
    }

    //Deslizamiento
    public void SlideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit; 

        if(isOnSlope)
        {
            movePlayer.x += ((1f - hitNormal.y)) * hitNormal.x * slideVelocity;
            movePlayer.z += ((1f - hitNormal.y)) * hitNormal.z * slideVelocity;

            movePlayer.y += slopeForceDown;
        }
    }

    //Si existe "toque" con un angulo inclinado
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    private void OnAnimatorMove()
    {

    }
}
