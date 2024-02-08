using Godot;
using System;

public partial class Textbox : Control
{
    public string FullText { get; set; }
    private string CurrentText { get; set; } = "";
    private int CurrentIndex { get; set; } = 0;
    private float TypeSpeed { get; set; } = 0.1f; // Adjust this value to control the typing speed

	public RichTextLabel textLabel, nameLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		textLabel = GetNode<RichTextLabel>("Background/TextContainer/Text");
		nameLabel = GetNode<RichTextLabel>("Background/NameContainer/Name");
        SetProcess(true); // Start the _Process method
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (CurrentIndex < FullText.Length)
        {
            CurrentText += FullText[CurrentIndex];
            textLabel.Text = CurrentText;
            CurrentIndex++;
        }
        else
        {
            SetProcess(false); // Stop the _Process method when all text has been printed
        }

        // Wait for the next character
        System.Threading.Thread.Sleep((int)(TypeSpeed * 1000));

    }

}
