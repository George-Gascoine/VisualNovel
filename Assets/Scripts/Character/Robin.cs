using Godot;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

public partial class Robin : Character
{
	[Export]
	public Json dialogueJSON;

	public List<Dialogue> dialogueList = new();

	public class Dialogue
	{
		public int act;
		public int id;
		public string sprite;
		public string text;
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
		ReadJSON();
		sprite = GetNode<Sprite2D>("CharacterSprite");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        PlayAnimation("FadeIn");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ReadJSON()
	{
        //Dialogue Data Parse
        JObject dialogueObj = JObject.Parse(dialogueJSON.Data.ToString());
        Dialogue dialogue = dialogueObj["dialogue"].ToObject<Dialogue>();
		dialogueList.Add(dialogue);
    }

	public void PlayAnimation(string anim)
	{
		animPlayer.Play(anim);
	}
}
