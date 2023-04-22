using UnityEngine;
using TMPro;

public class CollisionCarte : MonoBehaviour
{
    public TextMeshProUGUI collisionText;
    public TextMeshProUGUI fin;
    public GameObject objectToDestroy;
    public GameObject scremeur_1;
    public GameObject scremeur_2;
    public GameObject scremeur_3;
    public KeyCode destroyKey = KeyCode.E;
    public TextMeshProUGUI countText;
    private bool active_1 = true;
    private bool active_2 = true;
    private bool active_3 = true;
    private AudioSource audioSource;
    private AudioSource audioSource2;
    private AudioSource audioSource3;
	private AudioSource audioSource4;

	public float volLowRange = 0.5f;
	public float volHighRange = 1.0f;

	public AudioClip AudioCarte;
	public bool activeSound = false;

    public AudioClip scream; // Le son de tir
    public AudioClip scream2; // Le son de tir
    public AudioClip scream3;


    private static int count = 0; // déclarer la variable count comme étant statique comme ca elle incremente chaque instance carte => partager 

    public int CarteIncr(){
        return count;
    }

    private bool isCollision = false;

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

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource2 = scremeur_2.AddComponent<AudioSource>();
        audioSource3 = scremeur_3.AddComponent<AudioSource>();
		audioSource4 = scremeur_3.AddComponent<AudioSource> ();

        //fin.gameObject.SetActive(false);
        collisionText.gameObject.SetActive(false);
        scremeur_1.gameObject.SetActive(false);
        scremeur_2.gameObject.SetActive(false);
        scremeur_3.gameObject.SetActive(false);



    }

    private void Update()
	{   
		float volume = Random.Range (volLowRange, volHighRange);

        if (isCollision && Input.GetKeyDown(destroyKey))
        {   

            count += 1; 
			activeSound = true;
            countText.text = count.ToString();
            collisionText.gameObject.SetActive(false);
			activeSound = true;
			Destroy(objectToDestroy, 0.5f);
        }
		if (activeSound) {
		   

			audioSource.PlayOneShot(AudioCarte, volume );
			activeSound = false;
		}
        
        while (count == 1 && active_1 == true){
            scremeur_1.gameObject.SetActive(true);
            audioSource.PlayOneShot(scream, volume);
            Destroy(scremeur_1, 1.5f);
            active_1 = false;
        }
        while (count == 3  && active_2 == true){
            scremeur_2.gameObject.SetActive(true);
            audioSource2.PlayOneShot(scream2,volume);
            Destroy(scremeur_2, 2.0f);
            active_2 = false;
        }
        while (count == 6  && active_3 == true){
            scremeur_3.gameObject.SetActive(true);
            audioSource3.PlayOneShot(scream3, volume);
            Destroy(scremeur_3, 1f);
            active_3 = false;
        }
        /*while (count == 1){
            //fin.gameObject.SetActive(true);
        }*/
    
    }
}