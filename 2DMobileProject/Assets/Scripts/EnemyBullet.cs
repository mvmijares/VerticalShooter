using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public Vector2 direction;
    public float speed;
    public Transform explosion;

    CameraBoundary cameraBounds;

    float spriteHeight;
    float spriteWidth;
    // Use this for initialization
    void Start () {
        cameraBounds = Camera.main.GetComponent<CameraBoundary>();

        spriteHeight = GetComponent<BoxCollider2D>().size.x;
        spriteWidth = GetComponent<BoxCollider2D>().size.y;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(direction * Time.deltaTime * speed);
        if (transform.position.y > cameraBounds.VerticalBoundary(spriteHeight)) {
            Destroy(this.gameObject);
        }
        if (transform.position.x > cameraBounds.HorizontalBoundary(spriteWidth)) {
            Destroy(this.gameObject);
        }
        if (transform.position.y < -cameraBounds.VerticalBoundary(-spriteHeight)) {
            Destroy(this.gameObject);
        }
        if (transform.position.x < -cameraBounds.HorizontalBoundary(-spriteWidth)) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(0);
            Transform clone = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
