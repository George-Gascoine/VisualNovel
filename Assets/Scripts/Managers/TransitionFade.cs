using Godot;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

public partial class TransitionFade : CanvasLayer
{
    public GameManager gameManager;
    public AnimationPlayer AnimationPlayer { get; set; }
    public string targetScene { get; set; }


    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        Visible = false;
        AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        //AnimationPlayer.Play("FadeOut");
        ColorRect color = (ColorRect)GetChild(0);
        Color currentColor = color.SelfModulate;
        currentColor.A = 0f;
        color.SelfModulate = currentColor;
        GD.Print("fade ready");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void Transition()
    {
        AnimationPlayer.Play("FadeOut");
        GD.Print("Transition");
        Visible = true;
    }
    public void OnAnimationEnd(string anim)
    {
        switch (anim)
        {
            case "FadeOut":
                if (!targetScene.Contains("Act"))
                {
                    GetTree().ChangeSceneToFile("res://Assets/Scenes/Cutscenes/" + targetScene + ".tscn");
                }
                else
                {
                    gameManager.LoadAct(targetScene);
                }
                AnimationPlayer.Play("FadeIn");
                break;
            case "FadeIn":
                Visible = false;
                QueueFree();
                break;
        }
    }
}
