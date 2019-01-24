using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public CharacterController CC;

    float Bounds = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var characterX = CC.transform.position.x;
        if (transform.position.x + Bounds < characterX)
        {
            transform.position = new Vector3(characterX - Bounds, transform.position.y, transform.position.z);
        }
        else if (transform.position.x - Bounds > characterX)
        {
            transform.position = new Vector3(characterX + Bounds, transform.position.y, transform.position.z);
        }
    }
}
