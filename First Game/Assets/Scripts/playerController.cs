using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //script
    [SerializeField] CharacterController controller;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [SerializeField] int jumpsMax;

    // Instantiating using Raycasting
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    //this is what we use to instantiate the cube
    //[SerializeField] GameObject cube;
    bool isShooting;

    int jumpedTimes;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    Vector3 move;

    private void Start()
    {
        
    }

    void Update()
    {
        movement();

        // Instantiating using Raycasting
        if(!isShooting && Input.GetButton("Shoot"))
        {
            StartCoroutine(shoot());
        }
    }

    void movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpedTimes = 0;
        }

        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpsMax)
        {
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // Instantiating using Raycasting
    IEnumerator shoot()
    {
        isShooting = true;

        //raycasting comes to play
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            //Instantiate(cube, hit.point, transform.rotation);
        }
        yield return new WaitForSeconds(shootRate);
        isShooting= false;
    }
}
