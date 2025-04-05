using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class Mission
{
    public string title;
    public List<string> tasks;
    public List<bool> done;

    // Constructeur
    public Mission ( string t )
    {
        this.title = t;
        this.tasks = new List<string>();
        this.done = new List<bool>();
    }

    // Accesseurs
    public string getTitle ()
    {
        return this.title;
    }

    public int tasks_number ( )
    {
        return this.tasks.Count;
    }

    public string getTask ( int n )
    {
        return this.tasks[n];
    }

    private void addTask ( string t )
    {
        this.tasks.Add(t);
        this.done.Add(false);
    }

    public bool is_done ( int n )
    {
        return this.done[n];
    }

    public void tick ( int n )
    {
        this.done[n] = true;
    }

    public void untick ( int n )
    {
        this.done[n] = false;
    }

    public override string ToString ( ) 
    {
        string str = "*****" + this.title + "*****\n";
        for ( int i = 0 ; i < this.tasks_number() ; i++ )
        {
            if ( this.done[i] )
            {
                //str += "\U00002611 "; // Ticked box
                str += "<s>" + this.tasks[i] + "</s> \n";
            }
            else
            {
                //str += "\U00002610 "; // Unticked box
                str += this.tasks[i] + "\n";
            }
        }
        return str;
    }

    public static Mission load_mission ( string txt_file )
    {
        StreamReader stream = new StreamReader( txt_file );
        string titre = stream.ReadLine( );
        Mission m = new Mission( titre );
        while ( !stream.EndOfStream )
        {
            m.addTask ( stream.ReadLine( ) ) ; 
        }
        stream.Close( );  
        return m;
    }

    public static List<Mission> load_missions ( string folder )
    {
        string[] files = System.IO.Directory.GetFiles ( folder );
        List<Mission> missions = new List<Mission>();
        for ( int i = 0 ; i < files.Length ; i++ )
        {
            if ( Path.GetExtension ( files[i] ) != ".meta" )
            {
                missions.Add( Mission.load_mission ( files[i] ) );
            }
        }
        return missions;
    }
}
