namespace AutoUpdater.Abstractions.Interfaces.Assemblers;

public interface IAssembler<TA, TB>
{
    TB Convert(TA obj);
    TA Convert(TB obj);

    List<TB> Convert(List<TA> objs);
    List<TA> Convert(List<TB> objs);
}