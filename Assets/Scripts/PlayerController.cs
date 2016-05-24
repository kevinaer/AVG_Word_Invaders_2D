using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public GameObject bullet;
    public Transform spawnTransform;
    public GameController gameController;
    public float fireRate;
    public float nextFire = 0;
    public float speed = 1.0f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMovement();
        UpdateShooting();
	}

    //Updates player shooting
    void UpdateShooting()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(bullet, spawnTransform.position, spawnTransform.rotation);
        }
    }

    // Updates player's movements
    void UpdateMovement()
    {
        //Gets information from input
        int horizontalInput = (int)(Input.GetAxisRaw("Horizontal"));
        int verticalInput = (int)(Input.GetAxisRaw("Vertical"));

        //Gets width and height of world
        float worldWidth = (float)(Camera.main.orthographicSize + this.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2);
        float worldHeight = (float)(worldWidth * Screen.height / Screen.width);

        //Calculates velocity
        float velocity = speed * Time.deltaTime;

        //Updates player's position
        Vector2 position = this.transform.position;
        float horizontalPosition = Mathf.Clamp(position.x + horizontalInput * velocity, -worldWidth, worldWidth);
        float verticalPosition = Mathf.Clamp(position.y + verticalInput * velocity, -worldHeight, 0);
        this.transform.position = new Vector2(horizontalPosition, verticalPosition);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyBullet")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            gameController.GameOver();
        }
    }
}
