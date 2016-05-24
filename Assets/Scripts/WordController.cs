using UnityEngine;
using System.Collections;

public class WordController : MonoBehaviour {

    //Public Variables regarding other objects
    public GameObject bullet;
    public GameObject player;
    public GameController gameController;
    public WordGenerator generator;
    //Public variable regarding type of word
    public bool adjective = false;
    public bool verbOrNoun = false;
    //Public variable regarding paramaters
    public float speed;
    public float nextFire;
    public float fireRate;
    //Public variables regarding status
    private bool awake = false;
    private bool vulenerable = false;
    //Public variables regarding dimensions
    private float worldWidth;
    //private float worldHeight;
    private Vector2 direction;

	// Use this for initialization
	void Start () {
        worldWidth = (float)(Camera.main.orthographicSize);
        //worldHeight = (float)(worldWidth * Screen.height / Screen.width);
    }
	
	// Update is called once per frame
	void Update () {
        if (awake)
        {
            UpdateMovement();
            if (verbOrNoun && !gameController.isGameOver())
            {
                UpdateShooting();
            }
            if (adjective && !gameController.isGameOver())
            {
                UpdateRemoval();
            }
        }
	}

    void UpdateRemoval()
    {
        if (Time.time > nextFire)
        {
            generator.UpdateDestroyed();
            Destroy(gameObject);
        }
    }

    // Updates the word's shooting
    void UpdateShooting()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate + fireRate * Random.value;
            GameObject bulletObject = (GameObject) Instantiate(bullet, transform.position, transform.rotation);
            Rigidbody2D body = bulletObject.GetComponent<Rigidbody2D>();
            body.velocity = player.transform.position - transform.position;
        }
    }

    // Updates word's movements
    void UpdateMovement()
    {
        //Calculates velocity
        float velocity = speed * Time.smoothDeltaTime;
        //Updates player's position
        Vector2 position = this.transform.position;
        
        float verticalPosition = position.y + direction.y * velocity;
        float horizontalPosition = position.x;

        //Gets the dimensions
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        float height = collider.size.y * transform.localScale.y;
        float width = collider.size.x * transform.localScale.x;
        if (verticalPosition <= height / 2)
        {
            //Sets the word to shootable
            verticalPosition = height / 2;
            vulenerable = true;
        } else
        {
            //Updates horizontal position like normal
            horizontalPosition = Mathf.Clamp(position.x + direction.x * velocity, -worldWidth + width, worldWidth - width);
        }
        this.transform.position = new Vector2(horizontalPosition, verticalPosition);
    }

    // Sets word to awoke and initializes direction vector
    void WakeUp()
    {
        awake = true;
        direction = new Vector2(worldWidth * Random.value - worldWidth / 2, player.transform.position.y - transform.position.y);
        nextFire = Time.time + fireRate;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (awake)
        {
            if (other.tag == "Bullet" && vulenerable)
            {
                if (adjective)
                {
                    gameController.AddScore(2);
                } else {
                    gameController.AddScore(1);
                }
                Destroy(other.gameObject);
                generator.UpdateDestroyed();
                Destroy(gameObject);
            }
        }
    }


}
