namespace TecCore.DataStructs;

[Flags]
public enum TaskState
{
    Submitted = 0,
    Acknowledged = 1,
    InProgress = 2,
    OnHold = 4,
    Blocked = 8,
    Deferred = 16,
    Completed = 32,
    Cancelled = 64
}