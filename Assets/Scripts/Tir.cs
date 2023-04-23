using UnityEngine;
using TMPro;

public class Tir : MonoBehaviour
{
    public TextMeshProUGUI collisionText;
    public GameObject ballePrefab;
    public Transform balleSpawnTransform;
    public float vitesseBalle = 10f;
    public GameObject smokeParticleSystem; // Le système de particules de fumée
    private bool estActiver;
    private bool isCollision = false;
    public GameObject objetAActiver;
    public GameObject objectToDestroy;

    public TextMeshProUGUI countText;

	private float volLowRange = 0.5f; // volume min
	private float volHighRange = 1.0f; // volume max

    public Munition munition;

    private int count = 0;

    private AudioSource audioSource;
    public AudioClip tirSound; // Le son de tir

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        estActiver = false;
        collisionText.gameObject.SetActive(false);
        smokeParticleSystem.SetActive(false); // Désactiver le système de particules de fumée
        
    }

    void Update()
    {
        if (estActiver)
        {
            collisionText.gameObject.SetActive(false);
            // Afficher l'objet s'il n'est pas déjà affiché
            if (!objetAActiver.activeSelf)
            {
                objetAActiver.SetActive(true);
            }
        }
        else
        {
            // Cacher l'objet s'il n'est pas déjà caché
            if (objetAActiver.activeSelf)
            {
                objetAActiver.SetActive(false);
            }
        }

        
        if ( estActiver && Input.GetButtonDown("Fire1"))
        {
            if(munition.MunitionIncr() > 0){
            GameObject balle = Instantiate(ballePrefab, balleSpawnTransform.position, balleSpawnTransform.rotation); // On instancie la balle 
            balle.GetComponent<Rigidbody>().velocity = balle.transform.forward * vitesseBalle; // On lui donne une trajectoire avec la vitesse 
            count = munition.MunitionDecr();
            Destroy(balle, 1f); // On la detruite 1 seconde apres pour eviter de surcharger la hierarchie
            // Jouer le son de tir
			float volume = Random.Range (volLowRange, volHighRange);
            audioSource.PlayOneShot(tirSound,volume);
            // Activer le système de particules de fumée pendant 0.5 seconde
            smokeParticleSystem.SetActive(true);
            Invoke("DisableSmokeParticleSystem", 0.5f);
            }
        }


        if (isCollision && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(objectToDestroy); 
            estActiver = true;
            collisionText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCollision = true;
            collisionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCollision = false;
            collisionText.gameObject.SetActive(false);
        }
    }

    // Désactiver le système de particules de fumée
    private void DisableSmokeParticleSystem()
    {
        smokeParticleSystem.SetActive(false);
    }


    
}