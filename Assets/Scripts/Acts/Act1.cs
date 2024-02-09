using Godot;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public List<Trigger> actTriggers = new();
    public List<Textbox> textboxes = new();
    public Textbox currentTextbox;
    public AnimationPlayer animPlayer;
    public int[] choices;
    public bool trigger;

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
        public int trigger;
    }

    public class Trigger
    {
        public int id;
        public string method;
        public string[] argument;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        gameManager = (GameManager)GetNode("/root/GameManager");
        eventTimer = GetNode<Timer>("EventTimer");
        ReadJSON();
        currentScene = scenes.FirstOrDefault();
        eventTimer.WaitTime = 2;
        eventTimer.Start();
        eventTimer.OneShot = true;
        eventTimer.Timeout += () =>
        {
            StartNewScene(currentScene);
        };
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsActionJustPressed("LeftMouse") && currentTextbox != null)
        {
            if (!currentTextbox.IsProcessing())
            {
                if(!trigger)
                {
                    StartNewScene(FindNewScene());
                }
                else
                {
                    FindTrigger();
                }
            }
            else if (currentTextbox.IsProcessing())
            {
                currentTextbox.CompleteText();
            }
        }
    }

    public void ReadJSON()
    {
        //Scene Data Parse
        JObject scenesObj = JObject.Parse(scenesJSON.Data.ToString());
        List<JToken> jScenes = scenesObj["scene"].Children().ToList();
        scenes = jScenes.Select(scene => scene.ToObject<Scene>()).ToList();
    }

    public void FindTrigger()
    {
        Trigger currentTrigger = actTriggers.Find(trigger => trigger.id == currentScene.trigger);
        object[] arguments = new object[] { currentTrigger.argument };
        Type thisType = GetType();
        MethodInfo theMethod = thisType.GetMethod(currentTrigger.method);
        theMethod.Invoke(this, arguments);
    }

    public Scene FindNewScene()
    {
        int currentID = currentScene.id;
        int newID = currentScene.next;
        Scene newScene = scenes.Find(scene => scene.id == newID);
        trigger = false;
        currentScene = newScene;
        if(newScene.trigger != -1)
        {
            trigger = true;
        }
        return newScene;
    }

    public void StartNewScene(Scene scene)
    {
        if(sceneCharacters.Find(character => character.name == scene.character) == null)
        {
            LoadCharacter(scene.character);
            CreateTextbox(new Vector2(320, 360));
            StartNewDialogue();
        }
        else
        {
            StartNewDialogue();
        }
    }

    public void StartNewDialogue()
    {
        foreach(Textbox textbox in textboxes)
        {
            if (textbox.character != currentScene.character)
            {
                
                textbox.Visible = false;
            }
            else
            {
                textbox.Visible = true;
                currentTextbox = textbox;
                currentTextbox.ClearText();
            }
        }
        currentTextbox.nameLabel.Text = currentScene.character;
        currentTextbox.fullText = FindCurrentDialogue().text;
        currentTextbox.StartNewText();
    }
    public Character.Dialogue FindCurrentDialogue()
    {
        Character currentChar = sceneCharacters.First(chara => chara.name == currentScene.character);
        Character.Dialogue currentDialogue = currentChar.dialogueList.Find(dia => dia.act == currentScene.dialogue[0] && dia.id == currentScene.dialogue[1]);
        GD.Print("Dialogue " + currentDialogue.text);
        return currentDialogue;
    }

    public void LoadCharacter(string name)
    {
        characterNo++;
        PackedScene character = ResourceLoader.Load<PackedScene>("res://Assets/Scenes/Characters/" + name + ".tscn");
        Character instance = (Character)character.Instantiate();
        instance.name = name;
        sceneCharacters.Add(instance);
        AddChild(instance);
        instance.Position = new Vector2(320, 180);     
    }

    public void CreateTextbox(Vector2 loc)
    {
        Textbox box = (Textbox)textbox.Instantiate();
        textboxes.Add(box);
        AddChild(box);
        currentTextbox = box;
        currentTextbox.character = currentScene.character;
        currentTextbox.Position = loc;
    }
}
