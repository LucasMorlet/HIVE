using UnityEngine;
using System;
using System.Collections;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class VideoStream : MonoBehaviour
{
    // Attributes
    private WebCamDevice[] camera_list;
    private WebCamTexture camera_texture;
    private UnityEngine.UI.RawImage rendered_image;
    private Texture2D texture;


    #if UNITY_IOS || UNITY_WEBGL
        private bool CheckPermissionAndRaiseCallbackIfGranted(UserAuthorization authenticationType, Action authenticationGrantedAction)
        {
            if (Application.HasUserAuthorization(authenticationType))
            {
                if (authenticationGrantedAction != null)
                    authenticationGrantedAction();

                return true;
            }
            return false;
        }

        private IEnumerator AskForPermissionIfRequired(UserAuthorization authenticationType, Action authenticationGrantedAction)
        {
            if (!CheckPermissionAndRaiseCallbackIfGranted(authenticationType, authenticationGrantedAction))
            {
                yield return Application.RequestUserAuthorization(authenticationType);
                if (!CheckPermissionAndRaiseCallbackIfGranted(authenticationType, authenticationGrantedAction))
                    Debug.LogWarning($"Permission {authenticationType} Denied");
            }
        }

        private void PermissionHandling ( )
        {
            StartCoroutine(AskForPermissionIfRequired(UserAuthorization.WebCam, () => { InitializeCamera(); })); // Doubt on still working after updating of core script
            return;
        }


    #elif UNITY_ANDROID
        private void PermissionCallbacksPermissionGranted(string permissionName)
        {
            StartCoroutine(DelayedCameraInitialization());
        }

        private IEnumerator DelayedCameraInitialization()
        {
            yield return null;
            InitializeCamera();
        }

        private void PermissionCallbacksPermissionDenied(string permissionName)
        {
            Debug.LogWarning($"Permission {permissionName} Denied");
        }

        private void AskCameraPermission()
        {
            var callbacks = new PermissionCallbacks();
            callbacks.PermissionDenied += PermissionCallbacksPermissionDenied;
            callbacks.PermissionGranted += PermissionCallbacksPermissionGranted;
            Permission.RequestUserPermission(Permission.Camera, callbacks);
        }

        private void PermissionHandling ( )
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                AskCameraPermission();
                return;
            }
        }
    
    #else // GameMode context

        private void PermissionHandling ( )
        {
            return;
        }

    #endif

    void Start()
    {
        this.texture = this.MakeHole ( new Texture2D(3840, 2160) );
        this.rendered_image = this.GetComponent<UnityEngine.UI.RawImage>();
        this.rendered_image.texture = this.texture;
        PermissionHandling(); // Depends on OS context
        DisplayCameraList();
    }

    private void DisplayCameraList ()
    {
        // Display camera list
        camera_list = WebCamTexture.devices;
        if ( camera_list.Length < 0 ) 
        {
            Debug.Log ( "No camera found");
            return;
        }
        else
        {
            String str = "\n";
            for (int i = 0; i < this.camera_list.Length; i++)
            {
                str += "Camera number ";
                str += i;
                str += " : ";
                str += this.camera_list[i].name;
                str += "\n";
            }
            Debug.Log(str);
        }
    }

    void Update()
    {
        this.OnKeyDown();
        //this.EditMask();
    }

    // Keyboard control
    void OnKeyDown ()
    {       
        // Change camera with numpad
        for ( int i = 0 ; i <=9 ; i++ ) 
        {
            if ( Input.GetKeyDown ( KeyCode.Keypad0 + i ) )
            {
                this.InitializeCamera ( i );
            }
        }
    }

    private void InitializeCamera ( int cam_number )
    {
        if ( cam_number < this.camera_list.Length )
        {
            Debug.Log ( "New camera input : " + this.camera_list[cam_number].name );
            this.camera_texture = new WebCamTexture( this.camera_list[cam_number].name );
            this.rendered_image.texture = this.camera_texture;
            this.camera_texture.Play();
        }
        else 
        {
            Debug.LogWarning ( "Camera number " + cam_number + "does not exist, only " + this.camera_list.Length + " camera(s) found" );
        }
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