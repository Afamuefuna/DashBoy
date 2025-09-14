public class SpeedBoostPowerUp : PowerUp
{
    public override void Accept(IPowerUpVisitor visitor)
    {
        visitor.Visit(this);
    }
}
