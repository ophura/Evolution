using System;
using System.Collections;
using UnityEngine;

internal class Test : MonoBehaviour
{
    private void Start()
    {
        // using Unity's StartCoroutine!
        StartCoroutine(
            methodName: nameof(Routine),
            value: (1.26F, "this is more elegant!")
        );

        // using an extension method!
        // backed by the "Coroutine StartCoroutine(IEnumerator)" func sig!
        this.StartCoroutine(Routine,
            arg1: 4.6F,
            arg2: "I've been fed to an extension method!"
        );
    }

    private IEnumerator Routine((float susPeriod, string str) args)
    {
        print(string.Format(
            format: "Coroutine suspended!\n{0}: {1}, ayo?",
            // NOTE: don't keep this in your code lol!
            arg0: UnityEditor.ObjectNames.NicifyVariableName(nameof(args.susPeriod)),
            arg1: args.susPeriod
        ));

        yield return new WaitForSeconds(args.susPeriod);

        print($"Coroutine exiting... remember that {args.str}");
    }

    private IEnumerator Routine(float susPeriod, string str)
    {
        print(string.Format(
            format: "Coroutine suspended!\n{0}: {1}, ayo?!1",
            // NOTE: don't keep this in your code lol!
            arg0: UnityEditor.ObjectNames.NicifyVariableName(nameof(susPeriod)),
            arg1: susPeriod
        ));

        yield return new WaitForSeconds(susPeriod);

        print($"Coroutine exiting... remember that {str}");
    }
}

internal static class StartCoroutineExtentionMethods
{
    internal static Coroutine StartCoroutine(
        this MonoBehaviour instance,
        Func<IEnumerator> routine
    ) => instance.StartCoroutine(routine());

    internal static Coroutine StartCoroutine<TArg>(
        this MonoBehaviour instance,
        Func<TArg, IEnumerator> routine,
        TArg arg
    ) => instance.StartCoroutine(routine(arg));

    internal static Coroutine StartCoroutine<TArg1, TArg2>(
        this MonoBehaviour instance,
        Func<TArg1, TArg2, IEnumerator> routine,
        TArg1 arg1, TArg2 arg2
    ) => instance.StartCoroutine(routine(arg1, arg2));

    internal static Coroutine StartCoroutine<TArg1, TArg2, TArg3>(
        this MonoBehaviour instance,
        Func<TArg1, TArg2, TArg3, IEnumerator> routine,
        TArg1 arg1, TArg2 arg2, TArg3 arg3
    ) => instance.StartCoroutine(routine(arg1, arg2, arg3));

    internal static Coroutine StartCoroutine<TArg1, TArg2, TArg3, TArg4>(
        this MonoBehaviour instance,
        Func<TArg1, TArg2, TArg3, TArg4, IEnumerator> routine,
        TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4
    ) => instance.StartCoroutine(routine(arg1, arg2, arg3, arg4));

    internal static Coroutine StartCoroutine<TArg1, TArg2, TArg3, TArg4, TArg5>(
        this MonoBehaviour instance,
        Func<TArg1, TArg2, TArg3, TArg4, TArg5, IEnumerator> routine,
        TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5
    ) => instance.StartCoroutine(routine(arg1, arg2, arg3, arg4, arg5));

    // usually, you wouldn't want to handle more than 3 arguments at the same time,
    // that'd be mentally taxing... but here, I provide upto 5 for you! gl mate!
}
