using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Note : MonoBehaviour
{
    public TextMeshProUGUI collisionText0;
    public TextMeshProUGUI collisionText1;
    public TextMeshProUGUI collisionText2;
    private CollisionCarte nbcarte;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(nbcarte.CarteIncr() == 0){
                collisionText0.gameObject.SetActive(true);
            }
            if(nbcarte.CarteIncr() == 1){
                collisionText2.gameObject.SetActive(true);
            }
            if(nbcarte.CarteIncr() == 8){
                collisionText1.gameObject.SetActive(true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collisionText0.gameObject.SetActive(false);
            collisionText1.gameObject.SetActive(false);
            collisionText2.gameObject.SetActive(false);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        collisionText0.gameObject.SetActive(false);
        collisionText1.gameObject.SetActive(false);
        collisionText2.gameObject.SetActive(false);

            nbcarte = GameObject.FindObjectOfType<CollisionCarte>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
