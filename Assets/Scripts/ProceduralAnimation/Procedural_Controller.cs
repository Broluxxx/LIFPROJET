using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Procedural_Controller : MonoBehaviour
{

    /** 
    Recupere les cibles et les origines de ray de l'araignee
    */
    [SerializeField]
    public GameObject [] targetLegs, rayLegs;

    /**
    Sert pour ajuster correctement les pattes par rapport a l'araignee et sa vitesse,
    pour ca on a besoin de recuperer effectivement l'araignee
    */
    [SerializeField]
    public GameObject spider;

    /**
    La distance max avant de realiser le pas (de changer la position des pattes)
    Valeur a parametrer dans l'inspector Unity
    */
    [SerializeField]
    public float distMaxPas = 0.5f;

    /**
    Hauteur max du pas
    Valeur a parametrer dans l'inspector Unity
    */
    [SerializeField]
    public float hauteurPas = 0.69f;

    /**
    Variable pour que le mouvement des pattes donne moins une impression de "teleportation", d'ou le nom leg_TP
    Valeur a parametrer dans l'inspector Unity
    */
    [SerializeField]
    public float leg_TP = 0f;

    /**
    Utilisee principalement dans la transition d'une position a une autre d'une patte
    Valeur a parametrer dans l'inspector Unity
    */
    [SerializeField]
    public float smoothness = 5f;

    /**
    Variable qui sert a avoir une rotation du corps plus propre et plus jolie a voir
    et pour avoir une sensation de vitesse moins brute
    Valeur a parametrer dans l'inspector Unity
    */
    [SerializeField]
    public float rotate_smoothness = 1f;

    /**
    Utilisee pour donner une meilleure sensation de realisme 
    et eviter que le pattes bougent toutes en meme temps
    => tentative de reduire l'aspect robot
    Valeur a parametrer dans l'inspector Unity
    */
    [SerializeField]
    public int temps_attente = 1;

    /**
    Variable utilisee pour la distance de depassement du pas
    => meilleure impression de vitesse
    Valeur a parametrer dans l'inspector Unity
    */
    [SerializeField]
    public float pas_depasse = 2f;

    /**
    On utilise ces variables pour garder en memoire la position de base et actuelle de chaque patte
    */
    private Vector3[] currentPosLegs, originalPosLegs;

    /**
    La vecteur 3D vitesse permet de donner une sensation de velocite dans le mvt de l'araignee
    */
    private Vector3 vitesse;

    /**
    Pour garder en memoire la derniere position du corps de l'araignee
    */
    private Vector3 spiderLastPos;

    /**
    Pour garder en memoire la derniere normale du corps de l'araignee
    */
    private Vector3 lastSpiderBodyUp;

    /**
    Permet de garder en memore la derniere vitesse connue
    */
    private Vector3 lastVit;

    /**
    Permet de savoir si la patte actuelle est en mvt,
    true si la patte est en mvt, false si patte opposee en mvt
    */
    private bool currentLeg = true;

    /**
    Permet de garder une trace de la patte opposee
    */
    private List<int> leg_opposee = new List<int>();

    /** la 1ere liste permet de savoir quelle patte doit bouger
    car elle est en dehors de sa "zone de confort" 
    => aBouger

    la 2eme permet de traquer quelle liste est effectivement en mvt
    => enMvt
    */
    private List<int> aBouger = new List<int>();
    private List<int> enMvt = new List<int>();




    /**
    Realise le pas d'une araignee, cherche principalement a savoir si pour chaque patte,
    celle-ci est en dehors de sa zone de confort, pour pouvoir après la déplacer le cas echeant
    */
    void bougerPattes() {

        for (int i = 0; i < targetLegs.Length; i++) {

            Vector3 temp_target = targetLegs[i].transform.position;
            Vector3 temp_ray = rayLegs[i].transform.position;

            // Si la distance entre la cible et l'origine du raycast
            if (Vector3.Distance(temp_target, temp_ray) >= distMaxPas) {
            
                /** Verifie si la patte d'index i ne se trouve pas en mouvement
                ou en dehors de la zone de confort. 
                Si c'est le cas, on ajoute la patte d'index i dans aBouger */
                if(!aBouger.Contains(i) && !enMvt.Contains(i)) {
                    
                    aBouger.Add(i);

                }

            }

            /** Verifie si la patte d'index i n'est pas en mvt.
            Si c'est pas le cas, on la bouge a la position initiale
            */
            else if (!enMvt.Contains(i)) {
                targetLegs[i].transform.position = originalPosLegs[i];
            }
        }

        /** On ne fait rien si on a aucune patte a bouger 
        et que toutes les pattes sont en mouvement */
        if (aBouger.Count == 0 || enMvt.Count != 0) {
            return;
        }

        /** On prend le premier index present dans aBouger,
        et on recupere la position originale de la patte a l'index correspondant */
        Vector3 rayCastPos = rayLegs[aBouger[0]].transform.position;

        /**
        On ajoute Mathf.Clamp a la position du rayCast, 
        cela empeche l'araignee d'avoir un bug ou elle a des rotations dans tous les sens
        puis, on recupere la direction du mvt en faisant
        la soustraction entre la position du ray et la cible de la patte dans aBouger,
        on y ajoute la velocite * pas_depasse, juste pour donner
        une impression de vitesse de l'araignee au moment ou elle s'arrete, en depassant un petit peu la destination
        */
        rayCastPos += Mathf.Clamp(vitesse.magnitude * pas_depasse, 0, 2) * (rayLegs[aBouger[0]].transform.position - targetLegs[aBouger[0]].transform.position) + vitesse * pas_depasse;

        /**
        patte_opposee = false, car la patte actuelle fait le mvt => impossible que les deux pattes bougent en meme temps
        */
        StartCoroutine(marcher(aBouger[0], rayCastPos, false));
    }



    /** 
    fonction qui realise la transition d'une patte en dehors de sa zone de confort
    => fait la transition d'une position à une autre de maniere plus jolie
    */
    IEnumerator marcher(int indexPatte, Vector3 prochainePos, bool opposee) {

        /** Si la patte opposee n'est pas en mvt, on bouge la patte opposee en premier
        => donne une impression de realisme et de pas faire bouger les pattes en meme temps
         */
        if (!opposee) {
            bouge_leg_opposee(leg_opposee[indexPatte]);
        }

        /** On enleve la patte de aBouger car on effectue ici le mvt */
        if (aBouger.Contains(indexPatte)) {
            aBouger.Remove(indexPatte);
        }

        /** On ajoute ici la patte qui effectue le pas, elle n'est en mvt */
        if (!enMvt.Contains(indexPatte)) {
            enMvt.Add(indexPatte);
        }

        // posInit recupere la position de base de la patte indexPatte (0,1,2 ou 3)
        Vector3 posInit = originalPosLegs[indexPatte];

        /** interpolation lineaire entre la position initiale (posInit = originalPosLegs[indexPatte])
        et la prochainePos (position du rayCast) => on a un decalage entre les deux */
        for (int i = 1; i <= smoothness; i++) {
            
            /**
            On cree un nouveau Vector3 auquel on va additionner prochainePos, 
            pour faire la transition de la positon de base vers la prochainePos en faisant une courbe sur l'axe y
            => realise effectivement un pas, plus joli a voir
            */
            Vector3 tmp_y = new Vector3(0, Mathf.Sin(i / (float) (smoothness) * Mathf.PI) * hauteurPas, 0);
            targetLegs[indexPatte].transform.position = Vector3.Lerp(posInit, prochainePos + tmp_y, (i / smoothness));
            yield return new WaitForFixedUpdate();

        }

        originalPosLegs[indexPatte] = prochainePos;

        /**
        Permet d'attendre avant de bouger toutes les pattes
        au lieu de tout bouger d'un coup
        => reduit les mvt robotiques de l'araignee
        */
        for (int i = 1; i <= temps_attente; i++) {
            yield return new WaitForFixedUpdate();
        }  
    
        /* On retire la de enMvt, la patte avec l'index correspondant
        car on a deja fait le pas */
        if(enMvt.Contains(indexPatte)) {
            enMvt.Remove(indexPatte);
        }
    } 



    /**
    Realise la meme chose que les 3 dernieres lignes de codes de la fonction bougerPattes(),
    sauf que cette fois on prend leg_opposee = true => on bouge la patte opposee donc
    */
    void bouge_leg_opposee(int indexPatte) {
            
        /** On prend le premier index present dans aBouger,
        et on recupere la position originale de la patte a l'index correspondant */
        Vector3 rayCastPos = rayLegs[indexPatte].transform.position;

        /**
        On ajoute Mathf.Clamp a la position du rayCast, 
        cela empeche l'araignee d'avoir un bug ou elle a des rotations dans tous les sens
        puis, on recupere la direction du mvt en faisant
        la soustraction entre la position du ray et la cible de la patte dans aBouger,
        on y ajoute la velocite * pas_depasse, juste pour donner
        une impression de vitesse de l'araignee au moment ou elle s'arrete, en depassant un petit peu la destination
        */
        rayCastPos = rayCastPos + Mathf.Clamp(vitesse.magnitude * pas_depasse, 0f, 2f) * (rayLegs[indexPatte].transform.position - targetLegs[indexPatte].transform.position) + vitesse * pas_depasse;

        /**
        leg_opposee = false, car la patte actuelle fait le mvt => impossible que les deux pattes bougent en meme temps
        */
        StartCoroutine(marcher(indexPatte, rayCastPos, true));

    }



    /**
    Effectue la rotation du corps de l'araignee (root dans le prefab Unity), appelee dans fixedUpdate(),
    */
    void spider_rotate() {

        /**
        Les deux lignes suivantes nous retournent les vecteur entre chaque patte opposee
        */
        Vector3 diag_v1 = targetLegs[0].transform.position - targetLegs[1].transform.position;
        Vector3 diag_v2 = targetLegs[2].transform.position - targetLegs[3].transform.position;

        /**
        On utilise ensuite ces deux diagonales pour trouver la normale du corps de l'araignee
        */
        Vector3 spider_normal = Vector3.Cross(diag_v1, diag_v2).normalized;
        Vector3 spider_up = Vector3.Lerp(lastSpiderBodyUp, spider_normal, 1f / (float) rotate_smoothness);

        /** La normale de l'araignee est donc spider_up -> correspond a la normale par rapport au terrain incline */
        transform.up = spider_up;

        /**
        Pour effectuer une rotation du rigidBody
        */
        transform.rotation = Quaternion.LookRotation(transform.parent.forward, spider_up);
        
        /** On garde trace de la derniere normale, pour les calculs */
        lastSpiderBodyUp = transform.up;
    }



    // Start is called before the first frame update
    void Start()
    {
        lastSpiderBodyUp = transform.up;
        spiderLastPos = spider.transform.position;

        /** 
        On met toutes les positions currentPosLegs et originalPosLegs a targetLegs.transform.position
        ca donne une position initiale a l'araignee
        */
        currentPosLegs = new Vector3 [targetLegs.Length];
        originalPosLegs = new Vector3 [targetLegs.Length];

        for (int i = 0; i < targetLegs.Length; i++) {

            currentPosLegs[i] = originalPosLegs[i] = targetLegs[i].transform.position;

            /**
            Si la patte actuelle est en mvt, alors la suivante est celle avec l'index i + 1
            Sinon, alors la patte en mvt est celle a l'index i - 1 
            */
            if (currentLeg) {

                leg_opposee.Add(i + 1);
                currentLeg = false;

            } else if (!currentLeg) {

                leg_opposee.Add(i - 1);
                currentLeg = true;

            }
        }
        
        spider_rotate();

    }



    // Update is called once per frame
    void FixedUpdate()
    {

        /**
        A chaque update, on recupere la direction du vecteur 3D vitesse
        */
        vitesse = spider.transform.position - spiderLastPos;
        vitesse += (rotate_smoothness * lastVit) / (rotate_smoothness + 1f);

        bougerPattes();
        spider_rotate();

        /**
        Permet de garder une trace de la position de l'araignee
        et de la derniere vitesse connue
        */
        spiderLastPos = spider.transform.position;
        lastVit = vitesse;

    }

}
