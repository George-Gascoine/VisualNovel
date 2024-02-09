using Godot;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static Act1;
using System.Linq;

public partial class Character : Node2D
{
    [Export]
    public Json dialogueJSON;
    [Export]
    public Json characterJSON;

    public List<Dialogue> dialogueList = new();

    public string name;
    public string emotion;
    public Sprite2D sprite;
    public AnimationPlayer animPlayer;
    public string location;

    public class Dialogue
    {
        public int act;
        public int id;
        public string text;
        public int options;
        //public Option[] options;
    }

    public class Option
    {
        public string text;
        public int next;
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void ReadJSON()
    {
        //Dialogue Data Parse
        JObject dialogueObj = JObject.Parse(dialogueJSON.Data.ToString());
        List<JToken> jDialogue = dialogueObj["dialogue"].Children().ToList();
        dialogueList = jDialogue.Select(dialogue => dialogue.ToObject<Dialogue>()).ToList();
        GD.Print(dialogueList.Count);
    }
}
