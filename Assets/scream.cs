using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class scream : MonoBehaviour
{
    private CollisionCarte nbcarte;

    public AudioClip screams; // Le son de tir

    private AudioSource audioSource;

    private bool isGameOver = false;



    private void OnTriggerEnter(Collider other)
    {
        if (!isGameOver && nbcarte.CarteIncr() == 8)
        {

            isGameOver = true;
            SceneManager.LoadScene("GoodEnding");
            audioSource.PlayOneShot(screams);

        }
    }

        void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        nbcarte = GameObject.FindObjectOfType<CollisionCarte>();


    }

}
