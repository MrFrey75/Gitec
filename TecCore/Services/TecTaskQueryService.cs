using TecCore.Data;
using TecCore.Models;

namespace TecCore.Services;

public class TecTaskQueryService
{
    private readonly TecCoreDbContext _dbContext;

    public TecTaskQueryService(TecCoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<TecTask> QueryTasks(TecTaskQueryOptions options)
    {
        // Begin with the base query: tasks that are not deleted.
        IQueryable<TecTask> query = _dbContext.TecTasks.Where(x => !x.IsDeleted);

        // Add filters if the corresponding option is provided.
        
        
        // Location
        if (options.Location != null)
            query = query.Where(x => x.Location == options.Location);

        // Severity
        if (options.Severity != null)
            query = query.Where(x => x.Severity == options.Severity);

        // Category
        if (options.TaskCategory != null)
            query = query.Where(x => x.TaskCategory == options.TaskCategory);

        // Date Range
        if (options.StartDate != null)
            query = query.Where(x => x.CreatedAt >= options.StartDate);
        
        if (options.EndDate != null)
            query = query.Where(x => x.CreatedAt <= options.EndDate);

        // State
        if (options.TaskState != null)
            query = query.Where(x => x.TaskState == options.TaskState);
        
        // SearchTerm
        if (!string.IsNullOrEmpty(options.SearchTerm))
            query = query.Where(x => x.TaskName.Contains(options.SearchTerm) || x.TaskDescription.Contains(options.SearchTerm));
        
        // Affected User
        if (options.AffectedUser != null)
            query = query.Where(x => x.AffectedUsers.Contains(options.AffectedUser));
        
        // Affected Device
        if (options.AffectedDevice != null)
            query = query.Where(x => x.AffectedDevices.Contains(options.AffectedDevice));

        // Execute the query and return the list of tasks.
        return query.ToList();
    }
}

/*
*
Usage Example
You can now use this service to build custom queries on the fly:

csharp
Copy
var queryOptions = new TecTaskQueryOptions
{
    Location = someRoomLocation,
    Severity = Severity.High,
    StartDate = DateTimeOffset.UtcNow.AddDays(-7)
    Category = TaskCategory.Software,
    SearchTerm = "network",
    AffectedUser = someUser,
    AffectedDevice = someDevice
};

var queryService = new TecTaskQueryService(dbContext);
var tasks = queryService.QueryTasks(queryOptions);
*/