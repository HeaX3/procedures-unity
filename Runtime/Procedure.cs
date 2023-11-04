using System;
using System.Collections.Generic;
using System.Linq;
using RSG;
using UnityEngine;

namespace Procedures
{
    public class Procedure : IProcedure
    {
        private IProcedureStep[] Steps { get; }

        private bool _running;
        private IProcedureStep _current;

        public Procedure(IEnumerable<IProcedureStep> steps)
        {
            Steps = steps.ToArray();
        }

        public Procedure(params IProcedureStep[] steps)
        {
            Steps = steps;
        }

        public IPromise Run()
        {
            return new Promise((resolve, reject) =>
            {
                if (_running)
                {
                    Debug.LogError("Procedure is already running!");
                    return;
                }

                _running = true;

                var steps = Steps;
                if (steps.Length == 0)
                {
                    resolve();
                    return;
                }

                RunSteps(steps, 0).Then(resolve).Catch(reject);
            });
        }

        private IPromise RunSteps(IReadOnlyList<IProcedureStep> steps, int fromIndex)
        {
            return new Promise((resolve, reject) =>
            {
                if (!_running)
                {
                    reject(new Exception("Procedure interrupted!"));
                    return;
                }

                var current = steps[fromIndex];
                _current = current;
                current.Run().Then(() =>
                {
                    if (fromIndex + 1 >= steps.Count)
                    {
                        resolve();
                        return;
                    }

                    RunSteps(steps, fromIndex + 1).Then(resolve).Catch(reject);
                }).Catch(reject);
            });
        }
    }

    public class Procedure<T> : IProcedure<T>
    {
        private IProcedureStep<T>[] Steps { get; }
        private bool IsCancelled { get; set; }

        private bool _running;
        private IProcedureStep<T> _current;

        public Procedure(IEnumerable<IProcedureStep<T>> steps)
        {
            Steps = steps.ToArray();
        }

        public Procedure(params IProcedureStep<T>[] steps)
        {
            Steps = steps;
        }

        public IPromise<T> Run(T context)
        {
            return new Promise<T>((resolve, reject) =>
            {
                if (_running)
                {
                    Debug.LogError("Procedure is already running!");
                    return;
                }

                _running = true;

                var steps = Steps;
                if (steps.Length == 0)
                {
                    resolve(context);
                    return;
                }

                RunSteps(steps, 0, context).Then(resolve).Catch(reject);
            });
        }

        private IPromise<T> RunSteps(IReadOnlyList<IProcedureStep<T>> steps, int fromIndex, T context)
        {
            return new Promise<T>((resolve, reject) =>
            {
                if (!_running)
                {
                    reject(new Exception("Procedure interrupted!"));
                    return;
                }

                var current = steps[fromIndex];
                _current = current;
                current.Run(context).Then(result =>
                {
                    if (fromIndex + 1 >= steps.Count)
                    {
                        resolve(result);
                        return;
                    }

                    RunSteps(steps, fromIndex + 1, result).Then(resolve).Catch(reject);
                }).Catch(reject);
            });
        }
    }
}