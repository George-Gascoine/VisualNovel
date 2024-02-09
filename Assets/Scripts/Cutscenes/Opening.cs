using Godot;
using System;

public partial class Opening : CanvasLayer
{
    public GameManager gameManager;
    public Camera2D sceneCamera;
	public Timer sceneTimer;
	public RichTextLabel sceneText, pressAnyKey;

    public string fullText { get; set; } = "This is a test! This is a test! This is a test! This is a test! This is a test! This is a test! This is a test!";
    private string currentText { get; set; } = "";
    private int currentIndex { get; set; } = 0;
    private float typeSpeed { get; set; } = 0.1f; // Adjust this value to control the typing speed

    private bool finished = false;
                                                  // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        gameManager = (GameManager)GetNode("/root/GameManager");
        // How to Disconnect signal
        // pickArea.Disconnect("area_entered", new Callable(currentRock, "PickaxeEntered"));
        sceneCamera = GetNode<Camera2D>("SceneCamera");
		sceneTimer = GetNode<Timer>("SceneTimer");
		sceneText = GetNode<RichTextLabel>("SceneText");
        pressAnyKey = GetNode<RichTextLabel>("PressAnyKey");
        //SetProcess(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (currentIndex < fullText.Length)
        {
            currentText += fullText[currentIndex];
            sceneText.Text = currentText;
            currentIndex++;
        }
        else
        {
            if (!pressAnyKey.Visible)
            {
                pressAnyKey.Visible = true;
                finished = true;
            }
            //SetProcess(false); // Stop the _Process method when all text has been printed
        }

        if(finished && Input.IsActionJustPressed("LeftMouse"))
        {
            gameManager.SceneTransition("Act1");
            Timer timer = new()
            {
                OneShot = true,
                WaitTime = 0.8f,
            };
            AddChild(timer);
            timer.Start();
            timer.Timeout += () =>
            {
                QueueFree();
            };
        }

        // Wait for the next character
        System.Threading.Thread.Sleep((int)(typeSpeed * 1000));

        sceneCamera.Position += new Vector2(1f, 0);
    }
}
