using RSG;

namespace Procedures
{
    public interface IProcedure
    {
        IPromise Run();
    }

    public interface IProcedure<T>
    {
        IPromise<T> Run(T context);
    }
}