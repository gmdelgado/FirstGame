using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("----- Components -----")]
    //script
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(3,8)][SerializeField] float playerSpeed;
    [Range(8,25)][SerializeField] float jumpHeight;
    [Range(10, 50)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int jumpsMax;

    [Header("----- Gun Stats -----")]
    // Instantiating using Raycasting
    [Range(1, 10)][SerializeField] int shootDamage;
    [Range(.1f, 5)][SerializeField] float shootRate;
    [Range(1, 100)][SerializeField] int shootDist;
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
            IDamage damageable = hit.collider.GetComponent<IDamage>();

            if(damageable != null)
            {
                damageable.takeDamage(shootDamage);
            }
        }
        yield return new WaitForSeconds(shootRate);
        isShooting= false;
    }
}
