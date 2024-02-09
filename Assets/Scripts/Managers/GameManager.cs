using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
    public string currentAct;
    public Node2D actNode;
    public System.Collections.Generic.Dictionary<string, Character> characters;

    PackedScene fade = ResourceLoader.Load<PackedScene>("res://Assets/Scenes/Managers/TransitionFade.tscn");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //LoadAct("Act1");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }


    public static void TriggerMethod(Action<string> action, string argument)
    {
        action(argument);
    }
    public void LoadAct(string act) 
    {
        Node2D main = (Node2D)GetNode("/root/Main");
        main.Visible = true;
        PackedScene loadThis = ResourceLoader.Load<PackedScene>("res://Assets/Scenes/Acts/" + act + ".tscn");
        Node2D instance = (Node2D)loadThis.Instantiate();
        main.AddChild(instance);
        actNode = instance;
    }

    public void SceneTransition(string sceneName)
    {
        TransitionFade instance = (TransitionFade)fade.Instantiate();
        instance.targetScene = sceneName;
        AddChild(instance);
        instance.Transition();
    }
}
