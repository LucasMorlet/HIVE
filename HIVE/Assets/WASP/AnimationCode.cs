using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;
using System.IO;
using System;

public class AnimationCode : MonoBehaviour
{
    string fichier = "Assets/WASP/Pose_Tracking/AnimationFile.txt";
    string ligne = "HW!"; 
	StreamReader sr;

    public GameObject[] cloud_points;
    public GameObject[] bones;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log ( "Path " + Directory.GetCurrentDirectory() );
    }   

    // Update is called once per frame
    void Update()
    {
		// Read file is available
        try
		{
			sr = new StreamReader ( fichier );
			ligne = sr.ReadLine();
		}
		catch ( Exception e ) 
		{
			return;
		}
		finally
		{
			sr.Close();
		}
		
		// Place points in the right place
        string[] points = ligne.Split(',');
        for ( int i = 0 ; i < cloud_points.Length ; i++ )
        {
            cloud_points[i].transform.localPosition = new Vector3 
			( 
				float.Parse(points[0 + (i * 3)]) / 100,
				float.Parse(points[1 + (i * 3)]) / 100,
				float.Parse(points[2 + (i * 3)]) / 100
			);
        }
 
        // Torse
        this.EditBone ( 11, 12, 0 );
        this.EditBone ( 11, 23, 1 );
        this.EditBone ( 24, 12, 3 );
        this.EditBone ( 23, 24, 2 );

        // Bras gauche
        this.EditBone ( 11, 13, 10 );
        this.EditBone ( 13, 15, 11 );
        //this.EditBone ( 15, 19, 12 );
        //this.EditBone ( 17, 19, 13 );
        //this.EditBone ( 15, 17, 14 );
        //this.EditBone ( 15, 21, 15 );
        
        // Bras droit
        this.EditBone ( 12, 14, 4 );
        this.EditBone ( 14, 16, 5 );
        //this.EditBone ( 16, 20, 6 );
        //this.EditBone ( 18, 20, 7 ); 
        //this.EditBone ( 16, 18, 8 );
        //this.EditBone ( 16, 22, 9 );

        // Jambe gauche
        this.EditBone ( 23, 25, 21 );
        this.EditBone ( 25, 27, 22 );
        //this.EditBone ( 27, 29, 23 );
        //this.EditBone ( 27, 31, 25 );
        this.EditBone ( 29, 31, 24 );
		
        // Jambe droite
        this.EditBone ( 24, 26, 16 );
        this.EditBone ( 26, 28, 17 );
        //this.EditBone ( 28, 30, 18 );
        //this.EditBone ( 28, 32, 20 );
        this.EditBone ( 32, 30, 19 );
		
		// Because there is a concurrent file
		Thread.Sleep(30);
    }


    void EditBone ( int indice_a, int indice_b, int indice_os )
    {
        GameObject current_bone = bones[indice_os];
        GameObject current_parent = current_bone.transform.parent.gameObject;
        GameObject goA = this.cloud_points[indice_a];
        GameObject goB = this.cloud_points[indice_b];
        Vector3 vec = goB.transform.position - goA.transform.position;

        // Go to world basis
        current_bone.transform.SetParent ( null );

        // Find a way to scale and rotate without skewing the capsule
        /* Scale (world) 
        float length = vec.magnitude;
        float radius = Math.Min ( goA.transform.lossyScale.x, goB.transform.lossyScale.x ); // / current_parent.transform.localScale.x;
        current_bone.transform.localScale = new Vector3 ( radius, length, radius );
        // Fin scale */

        // Scale (local)
        /*
        Vector3 vec = goB.transform.localPosition - goA.transform.localPosition;
        float length = vec.magnitude;
        float radius = Math.Min ( goA.transform.localScale.x, goB.transform.localScale.x ); // / current_parent.transform.localScale.x;
        current_bone.transform.localScale = DivideVectors ( new Vector3 ( radius, length, radius ), current_parent.transform.localScale );
        //current_bone.transform.localScale = new Vector3 ( radius, length, radius );
        */

        // Rotation (world)
        current_bone.transform.rotation = Quaternion.LookRotation( vec, Vector3.up ) * Quaternion.Euler ( 0, 90, 90 );

        // Position (world)
        current_bone.transform.position = ( goA.transform.position + goB.transform.position ) / 2;

        /* Why the collider is not skewed ????
        Transform colliderTransform = current_bone.GetComponentInChildren<Collider>().transform;
        current_bone.transform.localScale = colliderTransform.localScale;
        current_bone.transform.localRotation = colliderTransform.localRotation;
        */

        // Material
        //current_bone.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = goA.GetComponent<Renderer>().material;

        // Return to the hierarchical skeleton
        current_bone.transform.SetParent ( current_parent.transform, true );
    }

/*
    void AddBone ( int indice_a, int indice_b )
    {
        GameObject goA = this.cloud_points[indice_a];
        GameObject goB = this.cloud_points[indice_b];
        Vector3 a = this.AI_points[indice_a];
        Vector3 b = this.AI_points[indice_b];

        Vector3 position = ( a + b ) / 2;
        Vector3 vec = a - b;
        float longueur = vec.magnitude;
        float theta = (float)Math.Atan ( (vec.z / vec.x) ) * 180f / 3.14159286f;
        float phi = (float)Math.Atan ( vec.y / vec.x ) * 180f / 3.14159286f;    
        float rayon = Math.Min ( goA.transform.localScale.x, goB.transform.localScale.x );

        GameObject nouvel_os = Instantiate ( bone );
        bones.Add(nouvel_os);
        nouvel_os.transform.parent = this.cloud_points.transform;
        nouvel_os.transform.localScale = new Vector3 ( longueur, rayon, rayon );
        nouvel_os.transform.localPosition = new Vector3 ( position.x, position.y, position.z );
        nouvel_os.transform.localRotation = Quaternion.Euler( 0, -theta, phi );
        nouvel_os.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = goA.GetComponent<Renderer>().material;
    }
    */

    Vector3 DivideVectors ( Vector3 a, Vector3 b )
    {
        return new Vector3 ( a.x/b.x, a.y/b.y, a.z/b.z );
    }

    public GameObject getBone ( int n )
    {
        return this.bones[n];
    }

    public GameObject getPoint ( int n )
    {
        return this.cloud_points[n];
    }
}