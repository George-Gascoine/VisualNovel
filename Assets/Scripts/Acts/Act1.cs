using Godot;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

public partial class Act1 : Node2D
{
	public GameManager gameManager;
    [Export]
    public Json scenesJSON;
    public List<Scene> scenes = new();
    public Scene currentScene;
	// Number of characters in the scene to determine zoom size
	public int characterNo;
    // Scene Characters and their locations
    public List<Character> sceneCharacters = new();
    public AnimationPlayer animPlayer;
    public int[] choices;

    public Timer eventTimer;
    
    public PackedScene textbox = ResourceLoader.Load<PackedScene>("res://Assets/Scenes/Managers/Textbox.tscn");

    public class Scene
    {
        public int id;
        public string character;
        public int[] dialogue;
        public string animation;
        public int choices;
        public string camera;
        public int next;
        public string trigger;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        gameManager = (GameManager)GetNode("/root/GameManager");
        eventTimer = GetNode<Timer>("EventTimer");
        eventTimer.WaitTime = 2;
        eventTimer.Start();
        eventTimer.Timeout += () =>
        {
            Textbox box = (Textbox)textbox.Instantiate();
            AddChild(box);
            box.Position = new Vector2(320, 360);
            box.nameLabel.Text = "Krespin";
            box.FullText = "Hello World!";
            eventTimer.Stop();
            //box.textLabel.Text = "Hello World";
        };
        ReadJSON();
        currentScene = scenes.FirstOrDefault();
        StartNewScene(currentScene);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void ReadJSON()
    {
        //Scene Data Parse
        JObject scenesObj = JObject.Parse(scenesJSON.Data.ToString());
        List<JToken> jScenes = scenesObj["scene"].Children().ToList();
        scenes = jScenes.Select(scene => scene.ToObject<Scene>()).ToList();
    }

    public void StartNewScene(Scene scene)
    {
        //if(sceneCharacters.First(character => character.name == scene.character) == null)
        //{
            LoadCharacter(scene.character);
        //}
    }


    public void LoadCharacter(string name)
    {
        GD.Print(name);
        characterNo++;
        PackedScene character = ResourceLoader.Load<PackedScene>("res://Assets/Scenes/Characters/" + name + ".tscn");
        Character instance = (Character)character.Instantiate();
        instance.name = name;
        sceneCharacters.Add(instance);
        CallDeferred("add_child", instance);
        instance.Position = new Vector2(320, 180);     
    }

    public void CreateTextbox(Vector2 loc)
    {
        Textbox box = (Textbox)textbox.Instantiate();
        AddChild(box);
    }
}
