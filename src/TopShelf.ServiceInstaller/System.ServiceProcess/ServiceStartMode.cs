#if NETSTANDARD2_0
namespace System.ServiceProcess
{
    public enum ServiceStartMode { Boot = 0, System = 1, Automatic = 2, Manual = 3, Disabled = 4 }
}
#endif
