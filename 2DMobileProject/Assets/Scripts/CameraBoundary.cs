using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundary : MonoBehaviour {
    //Camera variables
    float horzExtent;
    float vertExtent;

    private float minX;
    private float minY;
    private float maxX;
    private float maxY;

    float spriteHeight;
    float spriteWidth;

    private void Awake() {
        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }

    //Returns boundary + sprite dimsion value. Use -(neg) values for lower limit
    public float VerticalBoundary(float spriteDimension) {
        return vertExtent + spriteDimension;
    }
   
    public float HorizontalBoundary(float spriteDimension) {
        return horzExtent + spriteDimension;
    }
}
