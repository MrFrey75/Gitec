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
            task.Updates =
            [
                new TecTaskUpdate
                {
                    TaskState = TaskState.Submitted,
                    UpdateType = UpdateType.StatusChange
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
            task.IsDeleted = true;
            _dbContext.SaveChanges();
        }
        
        public void ArchiveTask(Guid uid)
        {
            var task = GetTask(uid);
            task.IsArchived = true;
            _dbContext.SaveChanges();
        }

        public void UnarchiveTask(Guid uid)
        {
            var task = GetTask(uid);
            task.IsArchived = false;
            _dbContext.SaveChanges();
        }
        
        public void UpdateStatus(Guid uid, TaskState state)
        {
            var task = GetTask(uid);
            task.Updates.Add(new TecTaskUpdate
            {
                TaskState = state,
                UpdateType = UpdateType.StatusChange
            });
        }
        
        //--
        
        public void AddUpdate(Guid uid, TecTaskUpdate update)
        {
            var task = GetTask(uid);
            task.Updates.Add(update);
            _dbContext.SaveChanges();
        }
        
        public void RemoveUpdate(Guid uid, TecTaskUpdate update)
        {
            var task = GetTask(uid);
            task.Updates.Remove(update);
            _dbContext.SaveChanges();
        }
        
        //--

        public void AddAffectedUser(Guid uid, List<User> users)
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
        
        public void AddAffectedUser(Guid uid, User user)
        {
            AddAffectedUser(uid, [user]);
        }
        
        public void RemoveAffectedUser(Guid uid, User user)
        {
            var task = GetTask(uid);
            task.AffectedUsers.Remove(user);
            task.Updates.Add(new TecTaskUpdate
            {
                Notes = "Removed user " + user.DisplayName,
                UpdateType = UpdateType.UserImpact
            });
            _dbContext.SaveChanges();
        }
        
        //--

        public void AddAffectedDevice(Guid uid, List<Device> devices)
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
        public void AddAffectedDevice(Guid uid, Device device)
        {
            AddAffectedDevice(uid, [device]);
        }
        
        public void RemoveAffectedDevice(Guid uid, Device device)
        {
            var task = GetTask(uid);
            task.AffectedDevices.Remove(device);
            task.Updates.Add(new TecTaskUpdate
            {
                Notes = "Removed device " + device.DisplayName,
                UpdateType = UpdateType.DeviceImpact
            });
            _dbContext.SaveChanges();
        }
        
        


    }
}
