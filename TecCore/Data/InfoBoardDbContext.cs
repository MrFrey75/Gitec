using Microsoft.EntityFrameworkCore;
using TecCore.Models;
using TecCore.Models.InfoBoard;
using TecCore.Services;
using TecCore.Utilities;

namespace TecCore.Data;

public class InfoBoardDbContext : DbContext
{
    public InfoBoardDbContext(DbContextOptions<InfoBoardDbContext> options) : base(options) { }

    // Default constructor for EF Core tools
    public InfoBoardDbContext() { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Data Source={ConfigService.DatabaseFile}"); // Ensure SQLite is configured
        }
    }
    
    public DbSet<InfoBoard> InfoBoards { get; set; }
    public DbSet<InfoBoardItem> InfoBoardItems { get; set; }
    public DbSet<InfoBoardItemCarousel> InfoBoardItemCarousels { get; set; }
    public DbSet<InfoBoardItemHtml> InfoBoardItemHtmls { get; set; }
    public DbSet<InfoBoardItemImage> InfoBoardItemImages { get; set; }
    public DbSet<InfoBoardItemMarkdown> InfoBoardItemMarkdowns { get; set; }
    public DbSet<InfoBoardItemRss> InfoBoardItemRssFeeds { get; set; }
    public DbSet<InfoBoardItemText> InfoBoardItemTexts { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
        });

        modelBuilder.Entity<InfoBoardItem>()
            .HasDiscriminator<string>("ItemType")
            .HasValue<InfoBoardItem>("Base")
            .HasValue<InfoBoardItemCarousel>("Carousel")
            .HasValue<InfoBoardItemHtml>("Html")
            .HasValue<InfoBoardItemImage>("Image")
            .HasValue<InfoBoardItemMarkdown>("Markdown")
            .HasValue<InfoBoardItemRss>("Rss")
            .HasValue<InfoBoardItemText>("Text");

        modelBuilder.Entity<InfoBoard>()
            .HasMany(b => b.InfoBoardItems)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }
    
    //Admin Users
    
    public List<AdminUser> GetAdminUsers()
    {
        return AdminUsers.ToList();
    }
    
    public AdminUser? GetAdminUser(Guid Uid)
    {
        return AdminUsers.FirstOrDefault(x => x.Uid == Uid);
    }
    
    public AdminUser? GetAdminUser(string Username)
    {
        return AdminUsers.FirstOrDefault(x => x.Username == Username);
    }
    
    public AdminUser CreateAdminUser(string Username, string Password)
    {
        var adminUser = new AdminUser
        {
            Username = Username,
            PasswordHash = EncryptionHelpers.HashString(Password)
        };
        AdminUsers.Add(adminUser);
        SaveChanges();
        return adminUser;
    }
    
    public void UpdateAdminUser(AdminUser adminUser)
    {
        var user = GetAdminUser(adminUser.Uid);
        if (user == null)
        {
            throw new Exception("Admin user not found");
        }
        if(adminUser.PasswordHash != user.PasswordHash)
        {
            adminUser.PasswordHash = EncryptionHelpers.HashString(adminUser.PasswordHash);
        }
        AdminUsers.Update(adminUser);
        SaveChanges();
    }
    
    public void DeleteAdminUser(AdminUser adminUser)
    {
        AdminUsers.Remove(adminUser);
        SaveChanges();
    }

    public void AuthenticateAdminUser(string username, string passwordClear)
    {
        var adminUser = GetAdminUser(username);
        if (adminUser == null)
        {
            throw new Exception("Invalid username or password");
        }
        if (EncryptionHelpers.VerifyHashedString(adminUser.PasswordHash, passwordClear))
        {
            throw new Exception("Invalid username or password");
        }
        
    }
    
    
    //Info Boards
    public List<InfoBoard> GetInfoBoards()
    {
        return InfoBoards
            .Where(w => w.IsArchived == false)
            .OrderBy(x => x.SortOrder)
            .ToList();
    }
    
    public InfoBoard? GetInfoBoard(Guid Uid)
    {
        return InfoBoards.FirstOrDefault(x => x.Uid == Uid);
    }
    
    public InfoBoard CreateInfoBoard()
    {
        var infoBoard = new InfoBoard();
        InfoBoards.Add(infoBoard);
        SaveChanges();
        return infoBoard;
    }

    public void UpdateInfoBoard(InfoBoard infoBoard)
    {
        InfoBoards.Update(infoBoard);
        SaveChanges();
    }
    
    public void ArchiveInfoBoard(InfoBoard infoBoard)
    {
        infoBoard.IsArchived = true;
        UpdateInfoBoard(infoBoard);
    }
    
    public void ArchiveInfoBoard(Guid Uid)
    {
        var infoBoard = GetInfoBoard(Uid);
        if (infoBoard != null)
        {
            ArchiveInfoBoard(infoBoard);
        }
    }
    
    public void UnArchiveInfoBoard(InfoBoard infoBoard)
    {
        infoBoard.IsArchived = false;
        UpdateInfoBoard(infoBoard);
    }
    
    public void UnArchiveInfoBoard(Guid Uid)
    {
        var infoBoard = GetInfoBoard(Uid);
        if (infoBoard != null)
        {
            UnArchiveInfoBoard(infoBoard);
        }
    }
    
    // Info Board Items
    
    public InfoBoardItem? GetInfoBoardItem(Guid Uid)
    {
        return InfoBoardItems.FirstOrDefault(x => x.Uid == Uid);
    }
    
    public InfoBoardItem? CreateInfoBoardItem(InfoBoardItem item)
    {
        InfoBoardItems.Add(item);
        SaveChanges();
        return item;
    }
    
    public void UpdateInfoBoardItem(InfoBoardItem item)
    {
        InfoBoardItems.Update(item);
        SaveChanges();
    }
    
    public void DeleteInfoBoardItem(InfoBoardItem item)
    {
        item.IsDeleted = true;
        UpdateInfoBoardItem(item);
    }
    
    public void DeleteInfoBoardItem(Guid Uid)
    {
        var item = GetInfoBoardItem(Uid);
        if (item != null)
        {
            DeleteInfoBoardItem(item);
        }
    }
    
}