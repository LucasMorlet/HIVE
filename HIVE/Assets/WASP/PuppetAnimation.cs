using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PuppetAnimation : MonoBehaviour
{
    public Quaternion chest_correction = Quaternion.Euler ( 0f, 1f, 0f );
	public Quaternion head_correction;
	public Quaternion left_arm_correction;
	public Quaternion left_forearm_correction;
	public Quaternion left_hand_correction;
	public Quaternion right_arm_correction;
	public Quaternion right_forearm_correction;
	public Quaternion right_hand_correction;
	public Quaternion left_leg_correction;
	public Quaternion left_foreleg_correction;
	public Quaternion right_leg_correction;
	public Quaternion right_foreleg_correction;

    public GameObject body_to_copy; 
    public GameObject[] bones;

    // Start is called before the first frame update
    void Start()
    {
		Thread.Sleep(100);
        Debug.Log ( "I will mimic " + this.body_to_copy );
    }

    // Update is called once per frame
    void Update()
    {
        // Chest
        //this.ChestAnimation ( 0, 1, 2, 0, 1, 2, 3 );
		this.ChestAnimationWithPoints ( 0, 1, 2, 3, 11, 12, 23, 24 );
        this.HeadAnimation ( 27, 7, 8, 0 );

        // Right Arm
        this.Mimic ( 4, 4 );
        this.Mimic ( 5, 5 );
		//this.RightHandAnimation ( 16, 18, 20, 22, 6, 9 );

        // Left Arm
        this.Mimic ( 10, 10 );
        this.Mimic ( 11, 11 );
        this.LeftHandAnimation ( 15, 17, 19, 15, 12, 15 );

        // Right Leg
        this.Mimic ( 16, 16 );
        this.Mimic ( 17, 17 );
        this.Mimic ( 19, 19 );
		
        // Left Leg
        this.Mimic ( 21, 21 );
        this.Mimic ( 22, 22 );
        this.Mimic ( 24, 24 );

		// Because everything cannot be right
        this.HotFix();
    }

    void Mimic ( int bone_to_animate, int bone_to_copy )
    {
        this.bones[bone_to_animate].transform.localRotation = this.body_to_copy.GetComponent<AnimationCode>().getBone ( bone_to_copy ).transform.localRotation;
    }

    void ChestAnimation ( int lowerSpine, int mediumSpine, int upperSpine, int upperChest, int leftChest, int lowerChest, int rightChest )
    {
		// utiliser les poinys plutot que les os
        Vector3 neck = this.body_to_copy.GetComponent<AnimationCode>().getBone ( upperChest ).transform.position;
        Vector3 coccyx = this.body_to_copy.GetComponent<AnimationCode>().getBone ( lowerChest ).transform.position;
        Vector3 left = this.body_to_copy.GetComponent<AnimationCode>().getBone ( leftChest ).transform.position;
        Vector3 right = this.body_to_copy.GetComponent<AnimationCode>().getBone ( rightChest ).transform.position;

        Vector3 haut   = neck - coccyx;
        Vector3 droite = left - right;
        Vector3 avant  = Vector3.Cross ( droite, haut );

        Quaternion pitch = Quaternion.FromToRotation( Vector3.right, droite );
        Quaternion yaw = Quaternion.FromToRotation( Vector3.up, haut );
        Quaternion roll = Quaternion.FromToRotation( Vector3.forward, avant );

		// Local ??
        this.bones[lowerSpine].transform.rotation = roll * pitch * yaw * chest_correction; //* Quaternion.Euler ( 270, 180, 0 ); // A corriger
    }
	
	void ChestAnimationWithPoints ( int hips, int lowerSpine, int mediumSpine, int upperSpine, int left_shoulder, int right_shoulder, int left_hip, int right_hip )
    {
		// utiliser les poinys plutot que les os
        Vector3 ls = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( left_shoulder ).transform.position;
        Vector3 rs = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( right_shoulder ).transform.position;
        Vector3 lh = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( left_hip ).transform.position;
        Vector3 rh = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( right_hip ).transform.position;

        Vector3 haut   = ( ls + rs ) / 2 - ( lh + rh ) / 2;
        Vector3 droite = ( rh + rs ) / 2 - ( lh + ls ) / 2;
        Vector3 avant  = Vector3.Cross ( droite, haut );

        Quaternion pitch = Quaternion.FromToRotation( Vector3.right, droite );
        Quaternion yaw = Quaternion.FromToRotation( Vector3.up, haut );
        Quaternion roll = Quaternion.FromToRotation( Vector3.forward, avant );

        this.bones[hips].transform.rotation = roll * pitch * yaw * chest_correction; //* Quaternion.Euler ( 270, 180, 0 ); // A corriger
    }

    void HeadAnimation ( int head, int left_ear, int right_ear, int nose )
    {
        Vector3 left = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( left_ear ).transform.position;
        Vector3 right = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( right_ear ).transform.position;
        Vector3 noze = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( nose ).transform.position;

        Vector3 droite = left - right;
        Vector3 avant  = noze - ( left + right ) / 2;
        Vector3 haut   = Vector3.Cross ( avant, droite );

        Quaternion pitch = Quaternion.FromToRotation( Vector3.right, droite );
        Quaternion yaw = Quaternion.FromToRotation( Vector3.up, haut );
        Quaternion roll = Quaternion.FromToRotation( Vector3.forward, avant );

        this.bones[head].transform.rotation = roll * pitch * yaw * head_correction; // * Quaternion.Euler ( 0, 0, 180 ); // Pas hyper convaincu de la correction
    }

    void LeftHandAnimation ( int left_wrist, int left_pinky, int left_index, int left_thumb, int hand_to_animate, int thumb_to_animate )
    {
        Vector3 wrist = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( left_wrist ).transform.position;
        Vector3 pinky = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( left_pinky ).transform.position;
        Vector3 index = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( left_index ).transform.position;

        Vector3 haut   = ( index + pinky ) / 2 - wrist;
        Vector3 droite = Vector3.Cross ( index - wrist, pinky - wrist );
        Vector3 avant  = Vector3.Cross ( droite, haut );
        
        Quaternion pitch = Quaternion.FromToRotation( Vector3.right, droite );
        Quaternion yaw = Quaternion.FromToRotation( Vector3.up, haut );
        Quaternion roll = Quaternion.FromToRotation( Vector3.forward, avant );

        this.bones[hand_to_animate].transform.rotation = roll * pitch * yaw * left_hand_correction; // Quaternion.Euler ( 270, 180, 0 ); 
    }
	
	void RightHandAnimation ( int right_wrist, int right_pinky, int right_index, int right_thumb, int hand_to_animate, int thumb_to_animate )
    {
        Vector3 wrist = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( right_wrist ).transform.position;
        Vector3 pinky = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( right_pinky ).transform.position;
        Vector3 index = this.body_to_copy.GetComponent<AnimationCode>().getPoint ( right_index ).transform.position;

        Vector3 haut   = ( index + pinky ) / 2 - wrist;
        Vector3 droite = Vector3.Cross ( index - wrist, pinky - wrist );
        Vector3 avant  = Vector3.Cross ( droite, haut );
        
        Quaternion pitch = Quaternion.FromToRotation( Vector3.right, droite );
        Quaternion yaw = Quaternion.FromToRotation( Vector3.up, haut );
        Quaternion roll = Quaternion.FromToRotation( Vector3.forward, avant );

        this.bones[hand_to_animate].transform.rotation = roll * pitch * yaw * right_hand_correction; //Quaternion.Euler ( 90, 180, 0 ); 
    }

    void HotFix ( )
    {
        this.bones[19].transform.Rotate ( 180, 0, 0 );
    }



    /* Sometimes to just write code that is a function in documentation
    Quaternion QuaternionFromBasis ( Vector3 right, Vector3 up, Vector3 forward ) 
    {
        Quaternion q = new Quaternion();
        q.w = Mathf.Sqrt( Mathf.Max( 0, 1 + right.x + up.y + forward.z ) ) / 2;
        q.x = Mathf.Sqrt( Mathf.Max( 0, 1 + right.x - up.y - forward.z  ) ) / 2;
        q.y = Mathf.Sqrt( Mathf.Max( 0, 1 - right.x + up.y - forward.z  ) ) / 2;
        q.z = Mathf.Sqrt( Mathf.Max( 0, 1 - right.x - up.y + forward.z  ) ) / 2;
        q.x *= Mathf.Sign( q.x * ( forward.y - up.z ) );
        q.y *= Mathf.Sign( q.y * ( right.z - forward.x ) );
        q.z *= Mathf.Sign( q.z * ( up.x - right.y ) );
        return q;
    }
    */
}
