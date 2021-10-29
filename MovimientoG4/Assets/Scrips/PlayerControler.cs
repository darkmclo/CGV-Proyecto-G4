using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float horizontalMove;
    public float verticalMove;
    public CharacterController player;
    public float playerSpeed;
    private Vector3 playerInput;
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 CamRight;
    private Vector3 movePlayer;
    public float gavity = 9.8f;
    public float fallVelocity;
    public float junpForce;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        camDirection();
        movePlayer = playerInput.x * CamRight + playerInput.z * camForward;
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
        CamRight = mainCamera.transform.right;

        camForward.y = 0;
        CamRight.y = 0;

        camForward = camForward.normalized;
        CamRight = CamRight.normalized;
    }

    //Gravedad
    void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gavity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else
        {
            fallVelocity -= gavity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
    }
    
    //Habilidades para el jugador
     public void PlayerSkill()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = junpForce;
            movePlayer.y = fallVelocity;
        }
    }
}
