using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Soundscape : MonoBehaviour
{
    private AudioSource audio_source;
    private List<AudioClip> sounds; 
    private int last_sound;
    private float delay; 
    private const float MIN_DELAY = 5.0f;
    private const float MAX_DELAY = 15.0f;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.audio_source = this.GetComponent<AudioSource>();
        this.sounds = new List<AudioClip>();
        string[] files = System.IO.Directory.GetFiles ( "Assets/Resources/Sounds" );
        for ( int i = 0 ; i < files.Length ; i++ )
        {
            if ( Path.GetExtension ( files[i] ) != ".meta" )
            {
                // Why the fuck I could not put full path here !!!
                this.sounds.Add(  Resources.Load<AudioClip>( "Sounds/" + Path.GetFileNameWithoutExtension ( files[i] ) ) );
                //this.sounds.Add(  Resources.Load<AudioClip>( "Assets/Resources/Sounds/" + files[i] ) );
            }
        }
        this.last_sound = 3; // Hard-coded siren sound
        this.audio_source.clip = this.sounds[this.last_sound];
        //this.audio_source.Play();
        this.delay = this.audio_source.clip.length + Random.Range( Soundscape.MIN_DELAY, Soundscape.MAX_DELAY );
    }

    // Update is called once per frame
    void Update()
    {
        this.delay -= Time.deltaTime;
        if ( this.delay < 0 )
        {
            this.audio_source.Stop();
            int n = Random.Range( 0, sounds.Count );
            while ( n == this.last_sound )
            {
                n = Random.Range( 0, sounds.Count );
            }           

            this.last_sound = n;
            this.audio_source.clip = this.sounds[n];
            //this.audio_source.Play();
            this.delay = this.audio_source.clip.length + Random.Range( Soundscape.MIN_DELAY, Soundscape.MAX_DELAY );
        }
    }
}
