public class NodeConnection
{
    public IPort Source;
    public IPort Destination;

    public NodeConnection(IPort source, IPort destination)
    {
        Source = source;
        Destination = destination;
    }
}