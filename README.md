# RandomThings
This is a supposed to be a Proof of Concept.
The idea was to use UMM to create a simple Mod. 
The mod in question basically adds a slider to make the game time pass more slowly (or faster if that's your heart desire).

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