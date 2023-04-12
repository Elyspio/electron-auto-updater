namespace AutoUpdater.Abstractions.Interfaces.Assemblers;

public abstract class BaseAssembler<TA, TB> : IAssembler<TA, TB>
{
    public abstract TB Convert(TA obj);

    public abstract TA Convert(TB obj);


    public List<TB> Convert(List<TA> objs)
    {
        return objs.Select(Convert).ToList();
    }

    public List<TA> Convert(List<TB> objs)
    {
        return objs.Select(Convert).ToList();
    }
}