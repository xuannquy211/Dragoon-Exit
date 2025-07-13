public class EndGamePoint : InteractiveObject
{
    public override void Activate()
    {
        EnvironmentManager.Instance.Restart();
    }
}