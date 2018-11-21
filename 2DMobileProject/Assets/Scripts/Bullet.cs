using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Bullet : MonoBehaviour {

    //Camera variables
    float horzExtent;
    float vertExtent;

    private float minX;
    private float minY;
    private float maxX;
    private float maxY;

    float spriteHeight;
    float spriteWidth;
    public float speed;
    
    public Vector3 direction;
    public Text text;

    public GameObject explosion;

    public int damage;

    CameraBoundary cameraBounds;
	// Use this for initialization
	void Start () {
        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;

        spriteHeight = GetComponent<BoxCollider2D>().size.y;
        spriteWidth = GetComponent<BoxCollider2D>().size.x;

        text = GameObject.FindGameObjectWithTag("Debug").GetComponent<Text>();

        cameraBounds = Camera.main.GetComponent<CameraBoundary>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(direction * Time.deltaTime * speed);
        if(transform.position.y > cameraBounds.VerticalBoundary(spriteHeight)) {
            Destroy(this.gameObject);
        }
        if (transform.position.x > cameraBounds.HorizontalBoundary(spriteWidth / 2)) {
            Destroy(this.gameObject);
        }
        if (transform.position.y < -cameraBounds.VerticalBoundary(-spriteHeight)) {
            Destroy(this.gameObject);
        }
        if (transform.position.x < -cameraBounds.HorizontalBoundary(-spriteWidth / 2)) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            enemy.TakeDamage(damage);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
