# Knight RPG

Han Saim Jeong

## Description

A simple 2D RPG game made in Unity. 

This project focuses on implementing a utility based decision making algorithm for enemy combat.
Utility is calculated for the following actions available to the enemy:

Physical Attack
Magic Attack
Heal Action
Defend Action
Ultimate Attack Action
Run Action

The algorithm focuses on making decisions based on the utility calculated for each action based on the current situation.
This situation updates for each Enemy Action turn and includes:

Player Health
Player Defense
Enemy Health

However, in order to add some randomality to the decision making, maximum utility alone does not dictate choice. 
Utility is used as a preference for actions where higher utility denotes a higher preference for the enemy to decide on that action.

### How to Run the File

The project is a Unity project and all assets are made available.
All code scripts were made using C#.
The project can be run through the Unity platform.

#### Assets Used

16 x 16 Dungeon Tileset - 0x72
* https://0x72.itch.io/16x16-dungeon-tileset

16 x 16 Dungeon Tileset II - 0x72
* https://0x72.itch.io/dungeontileset-ii

Superpowers Asset Packs - Pixel-Boy
* https://github.com/sparklinlabs/superpowers-asset-packs
* http://superpowers-html5.com/

Brackeys Turn-Based Combat in Unity
* https://github.com/Brackeys/Turn-based-combat

