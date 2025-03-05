using TecCore.Data;
using TecCore.Models.InfoBoard;

namespace TecCore.Services;

public class InfoBoardService
{
    public InfoBoardService(InfoBoardDbContext dbContext)
    {
        _dbContext = dbContext;
        InfoBoards = _dbContext.GetInfoBoards();
    }

    private InfoBoardDbContext _dbContext { get; set; }
    
    public List<InfoBoard> InfoBoards { get; set; }
    
}