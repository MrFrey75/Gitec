namespace TecCore.DataStructs;

[Flags]
public enum TaskCategory
{
    Unknown = 0,
    Hardware = 1,
    Software = 2,
    Network = 4,
    Access = 8,
    NewEnhancement = 16,
    Training = 32,
    Maintenance = 64,
    Other = 128,
}