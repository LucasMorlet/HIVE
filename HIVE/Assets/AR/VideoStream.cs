using UnityEngine;
using System;
using System.Collections;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif



public class VideoStream : MonoBehaviour
{
    WebCamDevice[] camera_list;
    WebCamTexture camera_texture;
    UnityEngine.UI.RawImage rendered_image;


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
#endif

    void Start()
    {
        #if UNITY_IOS || UNITY_WEBGL
            StartCoroutine(AskForPermissionIfRequired(UserAuthorization.WebCam, () => { InitializeCamera(); }));
            return;
        #elif UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                AskCameraPermission();
                return;
            }
        #endif
        InitializeCamera();
    }

    private void InitializeCamera()
    {
        // Display camera list
        camera_list = WebCamTexture.devices;
        if ( camera_list.Length < 0 ) 
        {
            Debug.Log ( "No camera found");
            return;
        }
        for (int i = 0; i < camera_list.Length; i++)
        {
            Debug.Log(camera_list[i].name);
        }


        /*
        camera_texture = new WebCamTexture(camera_list[1].name);
        rendered_image = GetComponent<UnityEngine.UI.RawImage>();
        rendered_image.texture = camera_texture;
        camera_texture.Play();*/
    }



    void Update()
    {
        /*
        Event e = Event.current;
        if ( e != null )
        {
            Debug.Log(e);
        } 
        */
        OnKeyDown();
    }

    void OnKeyDown ()
    {
        /*
        KeyCode key = event.keyCode;
        Debug.Log ( key );
        if ( KeyCode.Keypad0 <= key && key <= KeyCode.Keypad9 )
        {
            int camera_number = (int)( key - KeyCode.Keypad0 );
            if ( camera_number < camera_list.Length ) 
            {
                Debug.Log ( "New camera input :" + camera_list[camera_number] );
                camera_texture = new WebCamTexture(camera_list[camera_number].name);
                rendered_image = GetComponent<UnityEngine.UI.RawImage>();
                rendered_image.texture = camera_texture;
                camera_texture.Play();
            }
        }
        */
        
        // Change camera with numpad
        for ( int i = 0 ; i < camera_list.Length ; i++ ) 
        {
            if ( Input.GetKeyDown ( KeyCode.Keypad0 + i ) )
            {
                Debug.Log ( "New camera input :" + camera_list[i].name );
                camera_texture = new WebCamTexture(camera_list[i].name);
                rendered_image = GetComponent<UnityEngine.UI.RawImage>();
                rendered_image.texture = camera_texture;
                camera_texture.Play();
            }
        }

        if ( Input.GetKeyDown ( KeyCode.Space ) )
        {
            Debug.Log("Coucou");
        }
    }
}