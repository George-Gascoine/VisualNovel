using Godot;
using System;

public partial class Textbox : Control
{
    public string character { get; set; }
    public string fullText { get; set; }
    private string currentText { get; set; } = "";
    private int currentIndex { get; set; } = 0;
    private float typeSpeed { get; set; } = 0.1f; // Adjust this value to control the typing speed

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
        if (currentIndex < fullText.Length)
        {
            currentText += fullText[currentIndex];
            textLabel.Text = currentText;
            currentIndex++;
        }
        else
        {
            SetProcess(false); // Stop the _Process method when all text has been printed
        }

        // Wait for the next character
        System.Threading.Thread.Sleep((int)(typeSpeed * 1000));

    }

    public void CompleteText() 
    { 
        currentIndex = fullText.Length;
        textLabel.Text = fullText;
    }

    public void ClearText()
    {
        currentIndex = 0;
        currentText = "";
        textLabel.Text = currentText;
    }

    public void StartNewText()
    {
        SetProcess(true);
    }
}
