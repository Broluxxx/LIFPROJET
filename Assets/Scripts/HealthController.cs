using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthController : MonoBehaviour
{
    public Image healthBar;
    public float health;
    public float startHealth;

    public void onTakeDamage(int damage){ // Fonction public pour recevoir des dégats
        health = health - damage; // Santer init - degat predefinis
        healthBar.fillAmount = health / startHealth; // Remet a jour 
    }

    public void onTakePV(int damage){ // Fonction public pour recevoir des dégats
        health = health + damage; // Santer init - degat predefinis
        healthBar.fillAmount = health / startHealth; // Remet a jour 
    }



    
}
