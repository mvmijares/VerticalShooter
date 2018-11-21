using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    //Camera variables
    float horzExtent;
    float vertExtent;

    private float minX;
    private float minY;
    private float maxX;
    private float maxY;



    CameraBoundary cameraBounds;
    float spriteHeight;
    float spriteWidth;

    void Start() {
        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;

        cameraBounds = Camera.main.GetComponent<CameraBoundary>();
        //Need a better way to get these components
        spriteHeight = GetComponentInChildren<SpriteRenderer>().size.y;
        spriteWidth = GetComponentInChildren<SpriteRenderer>().size.x;

   
    }
    // Update is called once per frame
    private void Update() {
        float posX = Mathf.Clamp(transform.position.x, -cameraBounds.HorizontalBoundary(-spriteWidth / 2), cameraBounds.HorizontalBoundary(-spriteWidth / 2));
        float posY = Mathf.Clamp(transform.position.y, -cameraBounds.VerticalBoundary(-spriteHeight), cameraBounds.VerticalBoundary(spriteHeight));
        transform.position = new Vector2(posX, posY);
    }
  
}
