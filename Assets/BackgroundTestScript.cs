using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTestScript : MonoBehaviour
{
	private SpriteRenderer sr;
	private bool on;
	
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
		on = sr.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("b"))
		{
			on = !on;
			UpdateBackground();
		}
    }
	
	void UpdateBackground () {
		sr.enabled = on;
	}
}
