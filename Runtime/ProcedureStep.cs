using System;
using RSG;
using UnityEngine;

namespace Procedures
{
    public abstract class AbstractProcedureStep : IAbstractProcedureStep
    {
    }

    public abstract class ProcedureStep : AbstractProcedureStep, IProcedureStep
    {
        IPromise IProcedureStep.Run()
        {
            return new Promise((resolve, reject) =>
            {
                Run().Then(() =>
                {
                    try
                    {
                        resolve();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }).Catch(reject);
            });
        }

        protected abstract IPromise Run();
    }

    public abstract class ProcedureStep<T> : AbstractProcedureStep, IProcedureStep<T>
    {
        IPromise<T> IProcedureStep<T>.Run(T context)
        {
            return new Promise<T>((resolve, reject) =>
            {
                Run(context).Then(result =>
                {
                    try
                    {
                        resolve(result);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }).Catch(reject);
            });
        }

        protected abstract IPromise<T> Run(T context);
    }
}