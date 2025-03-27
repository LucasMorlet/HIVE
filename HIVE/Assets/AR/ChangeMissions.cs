using UnityEngine;
using TMPro;
using System.Collections.Generic; 

public class Missions : MonoBehaviour
{
    private TMP_Text textmesh;
    private List<Mission> missions;
    private int current_mission;
    private int current_task;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        missions = Mission.load_missions ( "Assets/AR/missions" );
        this.current_mission = 0;
        this.current_task = 0;
        this.textmesh = this.GetComponent<TMP_Text>();
        this.UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown ( KeyCode.LeftArrow ) )
        {
            if ( this.current_mission >= 1 ) 
            {
                this.current_mission--;
                this.current_task = 0;
                UpdateText ();
            }
        }
        if ( Input.GetKeyDown ( KeyCode.RightArrow ) )
        {
            if ( this.current_mission < missions.Count-1 ) 
            {
                this.current_mission++;
                this.current_task = 0;
                UpdateText ();
            }
        }
        if ( Input.GetKeyDown ( KeyCode.UpArrow ) )
        {
            if ( this.current_task >= 1 ) 
            {
                this.current_task--;
                UpdateText ();
            }
        }
        if ( Input.GetKeyDown ( KeyCode.DownArrow ) )
        {
            if ( this.current_task < this.missions[this.current_mission].tasks_number()-1 ) 
            {
                this.current_task++;
                UpdateText ();
            }
        }

        if ( Input.GetKeyDown ( KeyCode.Space ) )
        {
            this.missions[current_mission].tick(this.current_task);
            UpdateText ();
        }

        if ( Input.GetKeyDown ( KeyCode.Delete ) )
        {
            this.missions[current_mission].untick(this.current_task);
            UpdateText ();
        }
    }

    void UpdateText ()
    {
        this.textmesh.SetText ( this.missions[this.current_mission].ToString() );
        Debug.Log ( "\nMission : " + this.missions[this.current_mission].getTitle() 
            + " - Task : " + this.missions[this.current_mission].getTask(this.current_task) );
    }
}
