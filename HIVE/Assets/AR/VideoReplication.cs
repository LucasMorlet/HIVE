using UnityEngine;
using System;
using System.Collections;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class VideoReplication : MonoBehaviour
{
    public GameObject left_eye;
    // Attributes
    private UnityEngine.UI.RawImage rendered_image;
    private Texture2D texture;

    void Start()
    {
        //this.texture = this.MakeHole ( new Texture2D(3840, 2160) );
        this.rendered_image = this.GetComponent<UnityEngine.UI.RawImage>();
        this.rendered_image.texture = this.left_eye.GetComponent<UnityEngine.UI.RawImage>().texture;
        //this.rendered_image.texture = this.GetComponent<UnityEngine.UI.RawImage>();
    }



    void Update()
    {
        //this.rendered_image.texture = this.left_eye.GetComponent<UnityEngine.UI.RawImage>();
        //this.EditMask();
        this.rendered_image = this.GetComponent<UnityEngine.UI.RawImage>();
        this.rendered_image.texture = this.left_eye.GetComponent<UnityEngine.UI.RawImage>().texture;
    }

    private void EditMask ( )
    {
        // 
    }

    private Texture2D MakeHole ( Texture2D input )
    {
        Texture2D output = new Texture2D ( input.width, input.height );
        int centerX = (int)(input.width/2);
        int centerY = (int)(input.height/2);
        int radius = (int)(0.6*input.height);
        int square_radius = radius * radius;

        for ( int i = 0 ; i < input.width ; i++ )
        {
            for ( int j = 0 ; j < input.height ; j++ )
            {
                Color color = input.GetPixel ( i, j );
                if ( (i-centerY)*(i-centerY) + (j-centerX)*(j-centerX) <= square_radius )
                {
                    color.a = 0.0f;
                }
                output.SetPixel ( i, j, color );
            }
        }

        return output;
    }
}