using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phidgets;
using Phidgets.Events;

public class Lives : MonoBehaviour
{

    public static bool CheckPaused = false;
    public RFID Spellbook = new RFID(); //Declare an RFID object called Spellbook
    public static Lives Instance;
    public int LifeStorage = 1;
    static int addLives = 0;
    //When the object is initialised, only create one
    void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {
        try
        {


            //Initialise Phidgets RFID reader and hook the event handlers
            Spellbook.Attach += new AttachEventHandler(Spellbook_Attach);
            Spellbook.Detach += new DetachEventHandler(Spellbook_Detach);
            Spellbook.Error += new ErrorEventHandler(Spellbook_Error);

            Spellbook.Tag += new TagEventHandler(Spellbook_Tag);
            Spellbook.TagLost += new TagEventHandler(Spellbook_TagLost);
            Spellbook.open();

            //Wait for a Phidget RFID to be attached before doing anything with the object
            Debug.Log("Waiting for attachment...");
            Spellbook.waitForAttachment();

            //Turn on the LED and Antenna
            Spellbook.Antenna = true;
            Spellbook.LED = true;

            //Debug.Log("Press e to end.");
            //if (Input.GetButtonDown("e"))
            //{
            //    Spellbook.LED = false;
            //    Spellbook.close();
            //    Spellbook = null;
            //    Debug.Log("RFID device deactivated");


            //}

        }
        catch (PhidgetException ex)
        {
            Debug.Log(ex.Description);
        }

    }



    // Update is called once per frame
    void Update()
    {
        Spellbook.TagLost += new TagEventHandler(Spellbook_TagLost);
        if (CheckPaused == true)
        {
            Time.timeScale = 0;
            Debug.Log("Paused");
        }
        else
        {
            Time.timeScale = 1;
        }
        if (addLives > 0)
        {
            LifeStorage += addLives;
            addLives = 0;
        }
    }

    static void Spellbook_Attach(object sender, AttachEventArgs e)
    {
        Debug.Log("RFIDReader {0} attached!" + e.Device.SerialNumber.ToString());
    }

    //detach event handler...display the serial number of the detached RFID phidget
    static void Spellbook_Detach(object sender, DetachEventArgs e)
    {
        Debug.Log("RFID reader {0} detached!" + e.Device.SerialNumber.ToString());
    }

    //Error event handler...display the error description string
    static void Spellbook_Error(object sender, ErrorEventArgs e)
    {
        Debug.Log(e.Description);
    }

    //Print the tag code of the scanned tag
    static void Spellbook_Tag(object sender, TagEventArgs e)
    {
        Debug.Log("Tag {0} scanned" + e.Tag);
        if (isJump(e))
        {
            CheckPaused = !CheckPaused;
        }
        if (isOneLife(e))
        {
            Lives.addLives++;
        }
    }

    //print the tag code for the tag that was just lost
    static void Spellbook_TagLost(object sender, TagEventArgs e)
    {
        Debug.Log("Tag {0} lost" + e.Tag);
        if (isOneLife(e))
        {
            //          CheckPaused = false;
        }
    }


    private static bool isOneLife(TagEventArgs e)
    {
        return e.Tag.ToLower() == "onelife";
    }

    private static bool isJump(TagEventArgs e)
    {
        return e.Tag.ToLower() == "jump";
    }
}
