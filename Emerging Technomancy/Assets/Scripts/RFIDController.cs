
//Using the phidgets libraries
using Phidgets;
using Phidgets.Events;

using UnityEngine;
using System.Collections;

public class RFIDController : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        try
        {
            RFID Spellbook = new RFID; //Declare an RFID object called Spellbook

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

            if (Input.GetButtonDown("e"))
            {
                Spellbook.LED = false;

            }

        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
