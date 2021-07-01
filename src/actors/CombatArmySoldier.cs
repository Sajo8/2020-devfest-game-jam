using Enums;
using Godot;

public class CombatArmySoldier : Area2D
{
    public ArmyGunTypes gunType;

    double rps;
    int magSize;
    float reloadTime;

    int bulletsLeft;

    public AnimationPlayer animPlayer;
    RayCast2D selfRayCast; // always points to its own lane
    RayCast2D generalRayCast; // points wherever the army dude is looking at
    Lane selfLane;

    public override void _Ready()
    {
        BulletSpawner spawner = (BulletSpawner)FindNode("BulletSpawner");
        animPlayer = (AnimationPlayer)FindNode("AnimationPlayer");
        selfRayCast = (RayCast2D)FindNode("SelfRayCast2D");
        generalRayCast = (RayCast2D)FindNode("GeneralRayCast2D");

        switch (gunType)
        {
            case ArmyGunTypes.Pistol:
                rps = 1;
                magSize = 15;
                reloadTime = 2f;
                break;

            case ArmyGunTypes.Rifle:
                rps = 2;
                magSize = 20;
                reloadTime = 3f;
                break;

            case ArmyGunTypes.Shotgun:
                rps = 1.2;
                magSize = 5;
                reloadTime = 2.5f;
                break;
        }

        spawner.gunType = gunType;
        bulletsLeft = magSize;

        animPlayer.AssignedAnimation = "shoot_" + gunType.ToString().ToLower();
        animPlayer.Seek(0f, true);

        selfRayCast.CastTo = new Vector2(GetViewport().Size.x, 0);
        generalRayCast.CastTo = new Vector2(GetViewport().Size.x, 0);
    }

    public override void _Process(float delta)
    {
        // get our own lane
        // when found, stop running the _process loop
        selfRayCast.RotationDegrees = 180;
        if (selfRayCast.IsColliding())
        {
            selfLane = ((Node)selfRayCast.GetCollider()).GetParent<Lane>();
            SetProcess(false);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        // wait till we've made contact with our lane
        if (selfLane == null)
            return;

        var lanesInDanger = CombatInfo.Instance.lanesInDanger;

        if (selfLane.inDanger) // always look at our own lane if it's in danger
        {
            this.RotationDegrees = 180;
        }
        else if (lanesInDanger.Count > 0) // look at other lane if they are in danger AND we are not in danger
        {
            BaseDino closestDino = lanesInDanger[0].dangerDinos[0];
            if (IsInstanceValid(closestDino))
                LookAt(closestDino.GlobalPosition);
        }
        else if (lanesInDanger.Count == 0) // look at our lane if no other lane is in danger
        {
            RotationDegrees = 180;
        }

        // shoot if we see something, else stop
        if (generalRayCast.IsColliding())
        {
            animPlayer.Play();
        }
        else
        {
            animPlayer.Stop();
        }
    }

    async void CheckReload()
    {
        //? how does this work
        //? remove the (int) maybe
        bulletsLeft -= (int)rps;

        if (bulletsLeft <= 0)
        {
            animPlayer.Stop();

            // play reload anim
            await ToSignal(GetTree().CreateTimer(reloadTime), "timeout");
            bulletsLeft = magSize;
            animPlayer.Play("shoot_" + gunType.ToString().ToLower());
        }
    }
}