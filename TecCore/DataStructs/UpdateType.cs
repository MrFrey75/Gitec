namespace TecCore.DataStructs;

[Flags]
public enum UpdateType
{
    Undefined = 0,
    Initialize = 1,
    Notes = 2,
    StatusChange = 4,
    Action = 8,
    Reference = 16,
    UserImpact = 32,
    DeviceImpact = 64,
    Resolution = 128,
}