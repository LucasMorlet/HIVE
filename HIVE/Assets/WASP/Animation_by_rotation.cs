using UnityEngine;
using System;
using System.IO;
using System.Threading;

public class Animation_by_rotation : MonoBehaviour
{
    const string fichier = "Assets/WASP/Pose_Tracking/AnimationFile.txt";
    string line; 
	StreamReader sr;

    private Vector3 main_position;
    private Quaternion main_rotation;
    private Vector3[] input_points;
    private GameObject[] joints;
    
    // Start is called before the first frame update
    void Start()
    {
        this.transform.GetPositionAndRotation ( out this.main_position, out this.main_rotation );
        this.input_points = new Vector3[33];
        this.joints = new GameObject[10];
        this.joints[0] = this.transform.Find("Hips").gameObject;
        this.joints[1] = this.transform.Find("Hips/Left_hip").gameObject;
        this.joints[2] = this.transform.Find("Hips/Left_hip/Left_thigh_21/Left_knee").gameObject;
        this.joints[3] = this.transform.Find("Hips/Left_hip/Left_thigh_21/Left_knee/Left_tibia_22/Left_ankle").gameObject;
    }   

    // Update is called once per frame
    void Update()
    {
        this.line = "";
		this.read_file();
		this.puppet_animation();
        Thread.Sleep(30);
    }

    void read_file ( )
    {
        // Read file is available
        try
		{
			this.sr = new StreamReader ( fichier );
			this.line = this.sr.ReadLine();
		}
		catch ( Exception e ) 
		{
			return;
		}
		finally
		{
			this.sr.Close();
		}

        // Place points in the right place
        string[] points = this.line.Split(',');
        for ( int i = 0 ; i < this.input_points.Length ; i++ )
        {
            input_points[i] = new Vector3 
			( 
				float.Parse(points[(i * 3) + 0]),
				float.Parse(points[(i * 3) + 1]),
				float.Parse(points[(i * 3) + 2])
			);
        }
    }

    void puppet_animation ( )
    {
        this.transform.position = new Vector3 ( 0, 0, 0 );
        this.transform.rotation = Quaternion.Euler ( 0, 0, 0 );
        

        this.transform.SetPositionAndRotation ( this.main_position, this.main_rotation );
    }

    void rotate_joint ( int index_joint, int index_A, int index_B, int index_parent )
    {
        Vector3 world_orientation = this.input_points[index_B] - this.input_points[index_A];
        this.joints[index_joint].transform.localRotation = Quaternion.FromToRotation ( this.joints[index_parent].transform.up, world_orientation );
    }
}
