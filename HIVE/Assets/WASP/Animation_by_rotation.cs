using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    const string fichier = "Assets/WASP/Pose_Tracking/AnimationFile.txt";
    string line; 
	StreamReader sr;

    private Vector3[] input_points;
    public GameObject[] joints;
    
    // Start is called before the first frame update
    void Start()
    {
        this.joints = new GameObject[10];
        this.joints[0] = this.transform.Find("Bones/Hips/Left_hip").gameObject;
        this.joints[1] = this.transform.Find("Bones/Hips/Left_hip/Left_thigh_21/Left_knee").gameObject;
        this.joints[2] = this.transform.Find("Bones/Hips/Left_hip/Left_thigh_21/Left_knee/Left_tibia_22/Left_ankle").gameObject;
    }   

    // Update is called once per frame
    void Update()
    {
        this.line = "";
		this.read_file();
		this.animation();
    }

    void read_file ( )
    {
        // Read file is available
        try
		{
			sr = new StreamReader ( fichier );
			line = sr.ReadLine();
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
            input_points[i] = new Vector3 
			( 
				float.Parse(points[(i * 3) + 0]),
				float.Parse(points[(i * 3) + 1]),
				float.Parse(points[(i * 3) + 2])
			);
        }
    }

    void animation ( )
    {

    }

    void rotate_joint ( int index_joints, int index_A, int index_B, int index_parent )
    {
        Vector3 global_orientation = this.input_points[this.index_B] - this.input_points[this.index_A];
        this.joints.localRotation = Quaternion.FromToRotation ( this.joints[index_parent].up, global_orientation );
    }
}
