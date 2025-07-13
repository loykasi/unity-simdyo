public class OutputTrigger : Port<InputTrigger>
{
    public string Name;

    public InputTrigger Destination;

    public override void Connect(InputTrigger port)
    {
        Destination = port;
    }

    public void Invoke(VisualScripting vs)
    {
        Destination?.Invoke(vs);
    }
}