using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class scream : MonoBehaviour
{
    private CollisionCarte nbcarte;
    private bool isGameOver = false;



    private void OnTriggerEnter(Collider other)
    {
        if (!isGameOver && nbcarte.CarteIncr() == 8)
        {
            isGameOver = true;
            SceneManager.LoadScene("GoodEnding");
        }
    }

        void Start()
    {

        nbcarte = GameObject.FindObjectOfType<CollisionCarte>();


    }

}
