using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {
    public float speed;
    private Rigidbody2D body;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        body.velocity = new Vector2(0, speed);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
