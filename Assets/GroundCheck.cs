using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {
    public CharacterController CC;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        var otherName = other.gameObject.name;
        if (otherName == "Ground" || otherName == "Platform")
        {
            CC.OnLanded(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var otherName = other.gameObject.name;
        if (otherName == "Ground" || otherName == "Platform")
        {
            CC.OnAirborne();
        }
    }
}
