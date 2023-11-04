using RSG;

namespace Procedures
{
    public interface IAbstractProcedureStep
    {
        
    }

    public interface IProcedureStep : IAbstractProcedureStep
    {
        internal IPromise Run();
    }

    public interface IProcedureStep<T> : IAbstractProcedureStep
    {
        internal IPromise<T> Run(T context);
    }
}