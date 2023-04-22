using UnityEngine;
using TMPro;

public class CollisionCanvas : MonoBehaviour
{
    public TextMeshProUGUI collisionText;
    public GameObject objectToDestroy;
    public KeyCode destroyKey = KeyCode.E;

    private bool isCollision = false;

    public HealthController healthBar;
	//
	public float volLowRange = 0.5f;
	public float volHighRange = 1.0f;

	private AudioSource audioSource;
	public AudioClip AudioSoin;
	//

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

    private void Start () {

		audioSource = gameObject.AddComponent<AudioSource>();
	}

    private void Update()
    {
        if (isCollision && Input.GetKeyDown(destroyKey))
        {

			float volume = Random.Range (volLowRange, volHighRange);
			audioSource.PlayOneShot(AudioSoin, volume );

            if(healthBar.health < 100){
                Destroy(objectToDestroy); 
                collisionText.gameObject.SetActive(false);
                healthBar.onTakePV(25); // => Prends PV
            }
            else {
				collisionText.gameObject.SetActive(false);
                Destroy(objectToDestroy, 0.5f);
                
 
            }
        }


    }
}

