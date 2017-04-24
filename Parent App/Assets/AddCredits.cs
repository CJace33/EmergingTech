using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Phidgets;
using Phidgets.Events;
using System;
using System.Linq;

public class AddCredits : MonoBehaviour
{

    //What should be added to the card
    public static string tempWrite = "";
    //What is already on the card
    public static string cardText = "";
    public RFID writeRFID = new RFID(); //Declare an RFID object
    //public Text credits;
    public int creditsToAdd = 0;

    public static bool changed = false;

    public GameObject UItext;
    Text noOfCredits;


    // Use this for initialization
    void Start()
    {
        //Setup text in the stored credits text item and make it equal nothing
        UItext = GameObject.Find("StoredCredits");
        noOfCredits = UItext.GetComponent<Text>();
        noOfCredits.text = "";
        try
        {
            //Initialise Phidgets RFID reader and hook the event handlers
            writeRFID.Attach += new AttachEventHandler(writeRFID_Attach);
            writeRFID.Detach += new DetachEventHandler(writeRFID_Detach);
            writeRFID.Error += new ErrorEventHandler(writeRFID_Error);

            writeRFID.Tag += new TagEventHandler(writeRFID_Tag);
            writeRFID.TagLost += new TagEventHandler(writeRFID_TagLost);
            writeRFID.open();

            //Wait for a Phidget RFID to be attached before doing anything with the object
            Debug.Log("Waiting for attachment...");
            writeRFID.waitForAttachment();

            //Turn on the LED and Antenna
            writeRFID.Antenna = true;
            writeRFID.LED = true;
        }
        catch (PhidgetException ex)
        {
            Debug.Log(ex.Description);
        }
    }

    // Update is called once per frame
    void Update()
    {






        //Convert the number of credits that need to be added to a string
        tempWrite = creditsToAdd.ToString();
        //Display any text on the RFID card
        noOfCredits.text = cardText;
        //Compare the two strings to check if they are different
        if (changed)
        {
            try
            {
                int creditsStored;
                //convert the current number of credits to an int
                if (Int32.TryParse(cardText, out creditsStored))
                {
                    creditsToAdd += creditsStored;
                    tempWrite = creditsToAdd.ToString();
                }
                else
                {
                    Debug.Log("Information on card not a number");
                    cardText = "";
                }
                //RFID.RFIDTagProtocol proto = (RFID.RFIDTagProtocol)Enum.Parse(typeof(RFID.RFIDTagProtocol), "PHIDGETS");
                RFID.RFIDTagProtocol proto = RFID.RFIDTagProtocol.PHIDGETS;

                writeRFID.write(tempWrite, proto, false);

                //Null credits to add so they don't get added every frame
                creditsToAdd = 0;
                tempWrite = "";
            }
            catch (PhidgetException ex)
            {
                Debug.Log("Error writing tag: " + ex.Message);
            }
        }
        //else
        //{
        //    Debug.Log("");
        //}
    }

    static void writeRFID_Attach(object sender, AttachEventArgs e)
    {
        Debug.Log("RFIDReader {0} attached!" + e.Device.SerialNumber.ToString());
    }

    //detach event handler...display the serial number of the detached RFID phidget
    static void writeRFID_Detach(object sender, DetachEventArgs e)
    {
        Debug.Log("RFID reader {0} detached!" + e.Device.SerialNumber.ToString());
    }

    //Error event handler...display the error description string
    static void writeRFID_Error(object sender, ErrorEventArgs e)
    {
        Debug.Log(e.Description);
    }

    //Print the tag code of the scanned tag
    static void writeRFID_Tag(object sender, TagEventArgs e)
    {
        cardText = e.Tag;
        //Check if the two tags are different, and if they are indicate that the card needs to be updated
        if (String.Compare(tempWrite, cardText) != 0)
        {
            changed = true;
        }
        Debug.Log("Tag detected: " + e.Tag);
    }

    //print the tag code for the tag that was just lost
    static void writeRFID_TagLost(object sender, TagEventArgs e)
    {
        Debug.Log("Tag lost: " + e.Tag);
    }

    public void addCredits(int addedCredits)
    {
        creditsToAdd += addedCredits;
    }
}