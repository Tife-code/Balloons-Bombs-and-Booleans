using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    private float floatForce = 100f;
    private float gravityModifier = 1f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    //creating an audioclip variable for bounce sound
    public AudioClip bounceSound;

    //creating a varaible to hold whether the player is low enough for another upward force
    bool isLowEnough = true;

    // Start is called before the first frame update
    void Start()
    {
        //adding specific value as gravitational force
        Physics.gravity *= gravityModifier;

        //getting the audio component of the player object
        playerAudio = GetComponent<AudioSource>();

        //getting the rigidbody component of the player object
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        //checking if the player is low enough and making sure they stay within game view
        if(transform.position.y >= 15)
        {
            isLowEnough = false;
            transform.position = new Vector3(transform.position.x, 14, transform.position.z);
        }
        else
        {
            isLowEnough = true ;
        }

        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && isLowEnough && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

       
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

        //if player collides with ground, it bounces with a sound
        if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            playerRb.AddForce(Vector3.up * 10,  ForceMode.Impulse);
            playerAudio.PlayOneShot(bounceSound);
        }
    }

}
