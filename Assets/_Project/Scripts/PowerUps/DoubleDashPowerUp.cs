
public class DoubleDashPowerUp : PowerUp
{
    public override void Accept(IPowerUpVisitor visitor)
    {
        visitor.Visit(this);
    }
}
