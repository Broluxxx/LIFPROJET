using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionController : MonoBehaviour
{
    public HealthController healthBar; // On apelle la classe precedement définie

    public void OnCollisionEnter(Collision collision) // On utilise les colision 
    {   
 
        if (collision.gameObject.tag == "Ennemie")    // Si le joueur rentre en collision avec l'entité qui porte le tag "Ennemie"
        {
            
             if (healthBar) // Si vrai 
            {
                healthBar.onTakeDamage(25); // => Perds PV 
             }
        }

        if(healthBar.health <= 0){
            GameOver();
        }

    }

    private void GameOver() {
        SceneManager.LoadScene("GameOver");
    }


}


