using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour {

	// public AudioClip shootSound;
	//private AudioSource source;
	public int count = 0 ;
	//public int Soins = 0 ;
	//public float health = 200.0f ;
	// public bool ramasser = false;
	//public bool  destroy = false;
 	private float volLowRange = 0.5f;
	private float volHighRange = 1.0f;
	public TextMeshProUGUI countText;
	//public TextMeshProUGUI SoinsText;
	//public TextMeshProUGUI HealthText;
	// public GameObject MessageText;
	//public GameObject GameOver;
	//public GameObject GameOverImg;


	void Start () {

		// source = GetComponent<AudioSource> ();
		// MessageText.SetActive(false);
	}
	
	// Update is called once per frame

	void Update () {
        /*
		if (health > 0.0f) {

			health -= Time.deltaTime;

		} else {
		   
			GameOver.SetActive(true);
			GameOverImg.SetActive(true);
		}
        */
		
		countText.text = count.ToString ();

        /*
		SoinsText.text = Soins.ToString ();
		HealthText.text = health.ToString ();
        */

		/*
		if ((Input.GetKeyUp(KeyCode.E))&&(ramasser)) {
			/*
			 Soins ++;
			health += 50.0f;
			float vol = Random.Range (volLowRange, volHighRange);
			source.PlayOneShot (shootSound, vol);
            
			MessageText.SetActive(false);
			ramasser = false;
			destroy = true;

		}
		*/
	}


			// Carte 
	void OnControllerColliderHit( ControllerColliderHit hit ){
		
		if(hit.gameObject.tag == "carte"){
			Debug.Log("ici");
			count ++;
		 float vol = Random.Range (volLowRange, volHighRange);
			// source.PlayOneShot(shootSound,vol);
			Destroy(hit.gameObject);
		}
	
	}


	/*
	public void OnTrigerEnter(Collider other){

		if (other.gameObject.tag == "Soins") {
            Debug.Log("ici");
			MessageText.SetActive (true);
			ramasser = true;
		}
	}
	  
	public void OnTrigerExit(Collider other){

		if ((other.gameObject.tag == "Soins") && (destroy)) {
            Debug.Log("ou la");
			MessageText.SetActive (false);
			ramasser = false;
			Destroy (other.gameObject);
			destroy = false;
		} else {
			MessageText.SetActive (false);
			ramasser = false;
		
		}

      }
	  */
}
