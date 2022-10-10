using System;
using System.Threading;

namespace Hw3.Tests;

public class SingleInitializationSingleton
{
    public const int DefaultDelay = 3_000;
    
    private static readonly object Locker = new();

    private static volatile bool _isInitialized = false;
    public static SingleInitializationSingleton Instance => _instance.Value;

    private static Lazy<SingleInitializationSingleton> _instance =
        new(() => new SingleInitializationSingleton()); 
    public int Delay { get; }

    private SingleInitializationSingleton(int delay = DefaultDelay)
    {
        Delay = delay;
        // imitation of complex initialization logic
        Thread.Sleep(delay);
    }

    internal static void Reset()
    {
        if (!_isInitialized)
            return;
        lock (Locker)
        {
            if (!_isInitialized)
                return;
            _instance = new Lazy<SingleInitializationSingleton>
                (() => new SingleInitializationSingleton());
            _isInitialized = false;
        }
    }

    public static void Initialize(int delay)
    {
        if (_isInitialized)
            throw new InvalidOperationException("Singleton has been already initialized");
        lock (Locker)
        {
            if (_isInitialized)
                throw new InvalidOperationException("Singleton has been already initialized");
            _isInitialized = true;
            _instance = new Lazy<SingleInitializationSingleton>
                (() => new SingleInitializationSingleton(delay));
        }
    }
}