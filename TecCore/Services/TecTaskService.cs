using EnvDTE;
using Microsoft.Graph.Models;
using TecCore.Data;
using TecCore.DataStructs;
using TecCore.Models;

namespace TecCore.Services
{
    public class TecTaskService
    {
        private readonly TecCoreDbContext _dbContext;

        public TecTaskService(TecCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TecTask> GetAllTasks() => _dbContext.TecTasks.Where(x => !x.IsDeleted).ToList();
        public List<TecTask> GetAllTasks(RoomLocation location) => _dbContext.TecTasks.Where(x => !x.IsDeleted && x.Location == location).ToList();
        public List<TecTask> GetAllTasks(Severity severity) => _dbContext.TecTasks.Where(x => !x.IsDeleted && x.Severity == severity).ToList();
        public List<TecTask> GetAllTasks(TaskCategory category) => _dbContext.TecTasks.Where(x => !x.IsDeleted && x.TaskCategory == category).ToList();
        public List<TecTask> GetAllTasks(DateTimeOffset start, DateTimeOffset end) => _dbContext.TecTasks.Where(x => !x.IsDeleted && x.CreatedAt >= start && x.CreatedAt <= end).ToList();
        public List<TecTask> GetAllTasks(TaskState state) => _dbContext.TecTasks.Where(x => !x.IsDeleted && x.TaskState == state).ToList();

        public TecTask GetTask(Guid uid)
        {
            var task = _dbContext.TecTasks.Find(uid);
            if (task == null)
                throw new Exception("Task not found");
            return task;
        }

        public TecTask CreateTask(TecTask task)
        {
            if(!DateTime.TryParse(task.DueDate.ToString(), out var _))
                task.DueDate = DateTimeOffset.Now + TimeSpan.FromDays(7);
            
            task.Updates =
            [
                new TecTaskUpdate
                {
                    TaskState = TaskState.Submitted,
                    UpdateType = UpdateType.StatusChange,
                    Notes = $"Task submitted {DateTimeOffset.Now}"
                }
            ];

            _dbContext.TecTasks.Add(task);
            _dbContext.SaveChanges();
            return task;
        }

        public TecTask UpdateTask(TecTask task)
        {
            _dbContext.TecTasks.Update(task);
            _dbContext.SaveChanges();
            return task;
        }

        public void DeleteTask(Guid uid)
        {
            var task = GetTask(uid);
            task.Updates.Add(new TecTaskUpdate
            {
                UpdateType = UpdateType.System,
                Notes = "Task deleted."
            });
            task.IsDeleted = true;
            _dbContext.SaveChanges();
        }
        
        public void ArchiveTask(Guid uid)
        {
            var task = GetTask(uid);
            task.Updates.Add(new TecTaskUpdate
            {
                UpdateType = UpdateType.System,
                Notes = "Task archived."
            });
            task.IsArchived = true;
            _dbContext.SaveChanges();
        }

        public void UnarchiveTask(Guid uid)
        {
            var task = GetTask(uid);
            task.Updates.Add(new TecTaskUpdate
            {
                UpdateType = UpdateType.System,
                Notes = "Task unarchived."
            });
            task.IsArchived = false;
            _dbContext.SaveChanges();
        }
        
        public void UpdateStatus(Guid uid, TaskState state)
        {
            var task = GetTask(uid);
            task.Updates.Add(new TecTaskUpdate
            {
                TaskState = state,
                Notes = $"Updated from {task.TaskState} to {state}",
                UpdateType = UpdateType.StatusChange
            });
        }

        public void AddAffectedUser(Guid uid, List<GitecUser> users)
        {
            var task = GetTask(uid);
            task.AffectedUsers.AddRange(users);
            task.Updates.Add(new TecTaskUpdate
            {
                Notes = "Added users " + string.Join(", ", users.Select(x => x.DisplayName)),
                UpdateType = UpdateType.UserImpact
            });
            _dbContext.SaveChanges();
        }
        
        public void AddAffectedUser(Guid uid, GitecUser gitecUser)
        {
            AddAffectedUser(uid, [gitecUser]);
        }
        
        public void RemoveAffectedUser(Guid uid, GitecUser gitecUser)
        {
            var task = GetTask(uid);
            task.AffectedUsers.Remove(gitecUser);
            task.Updates.Add(new TecTaskUpdate
            {
                Notes = "Removed user " + gitecUser.DisplayName,
                UpdateType = UpdateType.UserImpact
            });
            _dbContext.SaveChanges();
        }
        
        //--

        public void AddAffectedDevice(Guid uid, List<GitecDevice> devices)
        {
            var task = GetTask(uid);
            task.AffectedDevices.AddRange(devices);
            task.Updates.Add(new TecTaskUpdate
            {
                Notes = "Added devices " + string.Join(", ", devices.Select(x => x.DisplayName)),
                UpdateType = UpdateType.DeviceImpact
            });
            _dbContext.SaveChanges();
        }
        public void AddAffectedDevice(Guid uid, GitecDevice gitecDevice)
        {
            AddAffectedDevice(uid, [gitecDevice]);
        }
        
        public void RemoveAffectedDevice(Guid uid, GitecDevice gitecDevice)
        {
            var task = GetTask(uid);
            task.AffectedDevices.Remove(gitecDevice);
            task.Updates.Add(new TecTaskUpdate
            {
                Notes = "Removed device " + gitecDevice.DisplayName,
                UpdateType = UpdateType.DeviceImpact
            });
            _dbContext.SaveChanges();
        }
        
        


    }
}
