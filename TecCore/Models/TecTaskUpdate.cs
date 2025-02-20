using System.ComponentModel.DataAnnotations;
using TecCore.DataStructs;

namespace TecCore.Models;

public class TecTaskUpdate : BaseEntity
{
    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;
    public TaskState TaskState { get; set; } = TaskState.Submitted;
    public UpdateType UpdateType { get; set; } = UpdateType.Undefined;
    public bool IsDeleted { get; set; } = false;
}