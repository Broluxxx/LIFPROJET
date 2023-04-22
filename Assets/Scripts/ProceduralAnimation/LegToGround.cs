using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**

Code qui contient le raycast, on va l'associer a chaque cube, qui est en fait l'origine du ray
ça évite de passer par un switch {} et de rentrer les coordonnées directement à la main

*/

public class LegToGround : MonoBehaviour
{

    /** On récupère un objet vide appelé "OrigineRaycast" dans Unity, qui est le parent de l'objet actuel
    on fait ça car on avait un bug ou les origines du ray passaient en dessous du sol
    */

    GameObject origineRay;
    [SerializeField] LayerMask layer;

    
    // Start is called before the first frame update
    void Start()
    {
        origineRay = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitSol;
        //Ray rayPatte = new Ray ();
        if(Physics.Raycast(origineRay.transform.position, Vector3.down, out hitSol, Mathf.Infinity, layer.value)) {
            transform.position = hitSol.point;
        }
    }
}
