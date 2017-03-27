using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phidgets;
using Phidgets.Events;
using System;

public class AddCredits : MonoBehaviour
{
    //What should be added to the card
    public string tempWrite = "";
    //What is already on the card
    public static string currentWrite = "";
    public RFID writeRFID = new RFID(); //Declare an RFID object
    //public Text credits;


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
        //Compare the two strings to check if they are the same
        if (String.Compare(tempWrite, currentWrite) != 0)
        {
            //If they are then 
            try
            {
                RFID.RFIDTagProtocol proto = (RFID.RFIDTagProtocol)Enum.Parse(typeof(RFID.RFIDTagProtocol), "PHIDGET_RFID_PROTOCOL_PHIDGETS".ToString());
                writeRFID.write(tempWrite, proto, false);
            }
            catch (PhidgetException ex)
            {
                Debug.Log("Error writing tag: " + ex.Message);
            }
        }
        else
        {
            Debug.Log("");
        }
    }

    //print the tag code for the tag that was just lost
    static void writeRFID_TagLost(object sender, TagEventArgs e)
    {
        Debug.Log("Tag {0} lost" + e.Tag);
    }

    public int creditsToAdd = 0;

    public void addCredits(int creditsToAdd)
    {

    }
}