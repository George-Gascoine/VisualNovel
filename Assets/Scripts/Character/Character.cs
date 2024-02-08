using Godot;
using System;

public partial class Character : Node2D
{
    public string name;
    public string emotion;
    public Sprite2D sprite;
    public AnimationPlayer animPlayer;
    public string location;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
