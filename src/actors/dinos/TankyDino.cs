using System.Collections.Generic;

public class TankyDino : BaseDino
{
    DinoProjectile iceProjectile;

    public TankyDino()
    {
        dinoType = Enums.Dinos.Tanky;
        CalculateUpgrades();

        specialGene = Enums.Genes.Ice;

        dinoUnlockCost = new List<int>() { 25, 50 };
    }

    public override void _Ready()
    {
        base._Ready();

        iceProjectile = (DinoProjectile)FindNode("IceProjectile");
        iceProjectile.Hide();
    }

    // TODO: maybe make a new type of dino with projectiles. #108, https://app.gitkraken.com/glo/view/card/1a7e35a6f0b44f9ba78b9edc247c726c
    public void ShootProjectile()
    {
        iceProjectile.Show();
        iceProjectile.disabled = false;
    }

}