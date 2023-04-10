# RandomThings
# Features
* Add multiplier for game time (not game speed).
* Change the amount of backup saves the game keeps (default 10, maximum 50).
* Toggle to add custom sort buttons in inventories.
* Toggle to automatically sort containers when the game loads.
* Toggle to automatically sort containers when they are opened.
* Choose between 4 possible sort methods (By Name or By Total Resource Count; Ascending or Descending)
* DisclosureToggle to view opened inventories in the mod menu.
* DisclosureToggle to view the Game Stats (save specific and system wide)

# Installation
* Get the latest version of [Unity Mod Manager](https://www.nexusmods.com/site/mods/21) (at least 0.25.5c)
* Install the Manager for the game.
  * See "How to Install" on the above linked page.
* Download the latest release zip and install it with one of the following methods:
  * (Prefered) Start UnityModManager.exe again. Make sure you still have the game selected. Switch to the Mods tab and drag the zip into the "Drop zip files here" field.
  * Open the game folder. Enter the mods directory and unzip the mods archive there.

# Building
* Clone the repository. 
* Open the solution with Visual Studio.
* Make sure the Nuggets are installed as expected. This should automatically happen.
* To ensure portability I use a system variable $(MagicalMixturePath) for references and the build script. If you want to build the project yourself you either:
  * Add the variable yourself. 
    * Go to Properties > Environment Variables (or just search for variables and pick the option that appears)
    * Under User Variables click new, with the variable name being *MagicalMixturePath* 
      and the value being your path to the game directory 
      e.g. *D:\Games\Steam\steamapps\common\The Magical Mixture Mill*.
    * Here is an example on how the entry should look like. ***Do remember to modify the path to the game directory depending on where it is located on your system***.
    ![grafik](https://user-images.githubusercontent.com/62178123/230797697-bb7653bd-14b2-4717-8bbd-718ce068d3b3.png)
  * Replace every reference to the variable in the .csproj file with the path. I don't really recommend doing that.
* Now you should be able to build the project without problems. If you still encounter trouble please contact me on Discord or create a GitHub issue.

# v0.1.3
* Added different ways of sorting. The active one is chosen in the Mod Menu.
* Added Toggles to turn on/off.
  * Custom Sort Buttons in inventories.
  * Auto sort on game load.
  * Auto sort on opeining a container.
* Known issue: Button Highlighting on Hover not working
![grafik](https://user-images.githubusercontent.com/62178123/230792222-c6e1ecaf-27cd-432c-9ea2-3f37b97fdb4a.png)
![grafik](https://user-images.githubusercontent.com/62178123/230792281-a56a4c8e-03d1-4401-9afa-9459bce85758.png)
# v0.1.2
* Added sorting as custom buttons into Game interface.
* Revamped code.
* Game now sorts (chest and inventory) container on load.
# v0.1.1
* Added Interface listing all Inventory Items and adding a sort button in the Mod Menu.
# v0.1.0
* Changed GUI (now uses ModKit since it looks better and is more useful).
* Time Multiplier now No Longer conflicts with fast forwards (Those endless nights...)
* Time Multiplier can be set to any value between 0.00001f and 10. Additionally there's now a reset button to set it back to 1
* Fixed per save settings not working
* (***New Feature***) it is now possible to change the amounts of saves that the game makes before starting to delete old ones.
  * This is by default 10 saves. I think this is per slot; but I'm unsure. They are deleted when a save game is actually loaded.
  * Since it seems like the game validates every save each time the game loads I limited the amount of saves you can keep (50 for now).
* (***New Feature***) I added a save button. This saves both global game stats and creates a new local save.
  * It seems to save stats and inventory at least. You do respawn at the beginning of the day near your bed though
  * **Beware** while I didn't encounter problems, the game is in Early Access and certainly doesn't expect Mods. Make sure to bear the risks in mind.
* (***New Feature***) Added a toggle to show the game file stats and the global stats.
![grafik](https://user-images.githubusercontent.com/62178123/230702634-49d28e94-b584-4eae-a60a-fae4f7cb2650.png)

# v0.0.1
* This is a supposed to be a Proof of Concept.
* The idea was to use UMM to create a simple Mod. 
* The mod in question basically adds a slider to make the game time pass more slowly (or faster if that's your heart desire).

# List of somewhat interesting classes to look at when decompiling:
* QuestTracker - seems to be the Quest Log
* Avatar - Player Character (and stats like movespeed gold etc.)
* ObiCharacter - Player Movement stuffs?
* MasterTimer - Game Time related stuff
* GameStatsManager - Tracks save and system wide stats; responsible for saving and loading system wide stats.
* SaveGame - Lower Level save class; takes care of I/O operations and seems to have OnSaved and OnLoaded Events
* SaveLoadManager - The interesting stuff, responsible for saving and loading every bit of save specific data. Might even include display of saves?