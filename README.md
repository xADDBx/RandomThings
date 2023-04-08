# RandomThings
This is a supposed to be a Proof of Concept.
The idea was to use UMM to create a simple Mod. 
The mod in question basically adds a slider to make the game time pass more slowly (or faster if that's your heart desire).

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