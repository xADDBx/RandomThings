# RandomThings
# v0.1.0
* Changed GUI (now uses ModKit since it looks better and is more useful).
* Time Multiplier now No Longer conflicts with fast forwards (Those endless nights...)
* Time Multiplier can be set to any value between 0.00001f and 10. Additionally there's now a reset button to set it back to 1
* Fixed per save settings not working
* (***New Feature***) it is now possible to change the amounts of saves that the game makes before starting to delete old ones.
  * This is by default 10 saves. I think this is per slot; but I'm unsure. They are deleted when a save game is actually loaded.
  * Since it seems like the game validates every save each time the game loads I limited the amount of saves you can keep (50 for now).
* (***New Feature***) I added a save button. This saves both global game stats and creates a new local save.
  * I didn't encounter any problems using this. 
  * Though the game doesn't seem to save time and location, so you'll keep your progress (Inventory, ...) but time is back to beginning of the day and you spawn next to your bed.
  * **Beware** while I didn't encounter problems, the game is in Early Access and certainly doesn't expect Mods. Make sure to bear the risks in mind.
* (***New Feature***) Added a toggle to show the game file stats and the global stats.
# v0.0.1
* This is a supposed to be a Proof of Concept.
* The idea was to use UMM to create a simple Mod. 
* The mod in question basically adds a slider to make the game time pass more slowly (or faster if that's your heart desire).

List of somewhat interesting classes:
QuestTracker - seems to be the Quest Log
Avatar - Player Character (and stats like movespeed gold etc.)
ObiCharacter - Player Movement stuffs?
MasterTimer - Game Time related stuff
GameStatsManager - Tracks save and system wide stats; responsible for saving and loading system wide stats.
SaveGame - Lower Level save class; takes care of I/O operations and seems to have OnSaved and OnLoaded Events
SaveLoadManager - The interesting stuff, responsible for saving and loading every bit of save specific data. Might even include display of saves?

UMM Config:
<!-- 0.25.4 -->
	<GameInfo Name="The Magical Mixture Mill">
		<Folder>The Magical Mixture Mill</Folder>
		<ModsDirectory>Mods</ModsDirectory>
		<ModInfo>Info.json</ModInfo>
		<GameExe>The Magical Mixture Mill.exe</GameExe>
		<EntryPoint>[UnityEngine.UIModule.dll]UnityEngine.Canvas.cctor:Before</EntryPoint>
		<StartingPoint>[Assembly-CSharp.dll]MainMenu.Awake:Before</StartingPoint>
		<UIStartingPoint>[Assembly-CSharp.dll]MainMenu.Start:After</UIStartingPoint>
		<MinimalManagerVersion>0.25.4</MinimalManagerVersion>
	</GameInfo>