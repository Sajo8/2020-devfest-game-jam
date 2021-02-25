using Godot;

public class Blockade : Area2D
{
    double health = 150;

    int numSprites;
    double healthPerSprite;

    int currentSpriteNum;
    Sprite currentSprite;
    double currentSpriteHealth;

    Node2D sprites;

    public override void _Ready()
    {
        Events.blockadeHit += OnBlockadeHit;
        Events.newRound += OnNewRound;

        sprites = GetNode<Node2D>("Sprites");

        Reset();
    }

    public override void _ExitTree()
    {
        Events.blockadeHit -= OnBlockadeHit;
        Events.newRound -= OnNewRound;
    }

    void Reset()
    {
        health = 150;

        numSprites = sprites.GetChildCount();
        healthPerSprite = health / numSprites;

        currentSpriteNum = 0;
        currentSprite = (Sprite)sprites.GetChildren()[currentSpriteNum];
        currentSpriteHealth = healthPerSprite;
    }

    void OnBlockadeHit(double dmg)
    {
        health -= dmg;
        currentSpriteHealth -= dmg;

        // set transparency to health / max health
        if (currentSpriteHealth > 0)
        {
            currentSprite.Modulate = new Color(1, 1, 1, (float)(currentSpriteHealth / healthPerSprite));
        }
        else // if destroyed, hide it and move to next sprite
        { 
            currentSprite.Visible = false;
            currentSpriteNum++;

            // only try to get sprite if it exists
            if (currentSpriteNum < numSprites) {
                currentSprite = (Sprite)sprites.GetChildren()[currentSpriteNum];
                currentSpriteHealth = healthPerSprite;
            } else {
                Events.publishroundWon();
                return;
            }
        }

    }

    void OnNewRound()
    {
        foreach (Sprite sprite in sprites.GetChildren()) {
            sprite.Modulate = new Color(1, 1, 1, 1);
            sprite.Visible = true;
        }
        Reset();
    }

}