using UnityEngine;
using TMPro;

public class Munition : MonoBehaviour
{
    public TextMeshProUGUI collisionText;
    public GameObject objectToDestroy;
    public KeyCode destroyKey = KeyCode.E;
    public TextMeshProUGUI countText;

    private static int count = 0; // déclarer la variable count comme étant statique comme ca elle incremente chaque instance munition => partager 
    private bool isCollision = false;

	//
	public float volLowRange = 0.5f;
	public float volHighRange = 1.0f;
	
	private AudioSource audioSource;
	public AudioClip AudioMunition;
	//

    public int MunitionIncr(){
        return count;
    }

    public int MunitionDecr(){
        count--;
        return count;
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

    private void Start()
    {
        collisionText.gameObject.SetActive(false);
		audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (isCollision && Input.GetKeyDown(destroyKey))
        {   
			float volume = Random.Range (volLowRange, volHighRange);
			audioSource.PlayOneShot(AudioMunition, volume );
			Destroy(objectToDestroy, 0.5f);
            count=count + 2; 
            collisionText.gameObject.SetActive(false);
        }
        countText.text = count.ToString();
    }
}