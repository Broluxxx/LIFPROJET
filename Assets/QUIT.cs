using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QUIT : MonoBehaviour
{
    void Start()
{
    Cursor.visible = true;
     
}

	public void Quit () {
	
		Application.Quit ();
	
	}
}
