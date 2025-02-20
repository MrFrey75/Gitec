using TecCore.Data;
using TecCore.DataStructs;
using TecCore.Models;
using TecCore.Services;
using TecCore.Services.Google;
using UserService = TecCore.Services.Entra.UserService;

namespace TecCore.Services;

public class PeopleMergeService
{
    private readonly Entra.UserService _entraUserService;
    private readonly Google.UserService _googleUserService;
    private readonly TecCoreDbContext _dbContext;
    private readonly OrgUnitService _orgUnitService;
    
    
    public PeopleMergeService(UserService entraUserService, Google.UserService googleUserService, TecCoreDbContext dbContext, OrgUnitService orgUnitService)
    {
        _entraUserService = entraUserService;
        _googleUserService = googleUserService;
        _dbContext = dbContext;
        _orgUnitService = orgUnitService;
    }
}