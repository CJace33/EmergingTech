# EmergingTech

Install the the Phidgets drivers (If you don't already have them), I included the installer in the repository (Phidget-x64_2.1.8.20160222.exe)

#RunAndJump-master:
The game I intended to integrate the shop into.
Open by going to: EmergingTech\RunAndJump-master\Assets\Scenes\Title\Title.unity
The project contains a large amount of code written for the original game, but the main script I wrote is Lives.cs in the main assets folder.

I cannot be sure it still works as I cannot test it and it has been some time since I worked on it.
It is still focused on reading the text on the card, and currently pauses the game while it detects a card with the string "Pause" on it and adds an aditional life if the card has "Onelife" on it.

You can test this by using the RFID-full.exe to write the string "Pause" or "Onelife" to the card, make sure you select the PHIDGETS protocol to write with,and DO NOT select the "lock" check box, as far as I am aware, that will lock the string to the card and make it impossible to use for future projects.


#Parent App:
Contains the ability to encode to a card, not fully finished due to the reader breaking, but it does have the ability to write. See my blog for more details.
In order to test this you can use the RFID-full.exe to encode one of the cards with a number (I'd suggest simply 0) and then press one of the buttons while the card is present to add/remove credits.
