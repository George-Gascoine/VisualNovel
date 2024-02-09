using Godot;

public partial class Robin : Character
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        ReadJSON();
        sprite = GetNode<Sprite2D>("CharacterSprite");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		Act1Load();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Act1Load()
	{
        PlayAnimation("FadeIn");
    }

	public void PlayAnimation(string anim)
	{
		animPlayer.Play(anim);
	}
}
