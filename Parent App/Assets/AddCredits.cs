using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phidgets;
using Phidgets.Events;
using System;
using System.Linq;

public class AddCredits : MonoBehaviour
{

    //What should be added to the card
    public string tempWrite = "";
    //What is already on the card
    public static string currentWrite = "";
    public RFID writeRFID = new RFID(); //Declare an RFID object
    //public Text credits;
    public int creditsToAdd = 0;

    // Use this for initialization
    void Start()
    {
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
        //Compare the two strings to check if they are different
        if (String.Compare(tempWrite, currentWrite) != 0)
        {
            try
            {
                int creditsStored;
                //convert the current number of credits to an int
                if (Int32.TryParse(currentWrite, out creditsStored))
                {
                    creditsToAdd += creditsStored;
                    tempWrite = creditsToAdd.ToString();
                }
                else
                {
                    Debug.Log("Information on card not a number");
                    currentWrite = "";
                }
                //RFID.RFIDTagProtocol proto = (RFID.RFIDTagProtocol)Enum.Parse(typeof(RFID.RFIDTagProtocol), "PHIDGETS");
                RFID.RFIDTagProtocol proto = RFID.RFIDTagProtocol.PHIDGETS;

                writeRFID.write(tempWrite.ToString(), proto, false);
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
        currentWrite = e.Tag;
        Debug.Log(e.Tag);
    }

    private void writeBtn_Click(object sender, EventArgs e)
    {

    }

    //print the tag code for the tag that was just lost
    static void writeRFID_TagLost(object sender, TagEventArgs e)
    {
        Debug.Log("Tag {0} lost " + e.Tag);
    }



    public void addCredits(int addedCredits)
    {
        creditsToAdd = addedCredits;
    }

    

}