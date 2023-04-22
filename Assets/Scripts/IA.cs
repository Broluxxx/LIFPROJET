using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{
        public GameObject objectToDestroy;

        private int nbCoupEnnemi = 0;

    // Agent de navigation 
    public UnityEngine.AI.NavMeshAgent agent;

    // Cible de l'ennemie
    public Transform Target;

    //Distance entre le joueur et l'ennemi
    private float Distance;

    //Distance de poursuite
    public float chaseRange = 10;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>(); // On assigne agent au component qu'on a ajouter
        
    }

    // Update is called once per frame
    void Update()
    {
        // On cherche le joueur en permanence
        Target = GameObject.Find("PlayerCapsule").transform;
 
        // On calcule la distance entre le joueur et l'ennemi, en fonction de cette distance on effectue diverses actions
        Distance = Vector3.Distance(Target.position, transform.position);

        if (Distance < chaseRange)
            {
                
                agent.destination = Target.position; 

            }

    }

    private void OnTriggerEnter(Collider collision)
    {
    if (collision.CompareTag("Balle"))
    {
        nbCoupEnnemi++;
        if (nbCoupEnnemi >= 3)
        {
            Destroy(objectToDestroy);
            nbCoupEnnemi = 0;
        }
    }
    }

}
