using Microsoft.EntityFrameworkCore;

namespace WMS.Core.Data;

public class DataInitializer
{
    private readonly WMSDbContext _dbContext;

    public DataInitializer(WMSDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void EnsureMigrated()
    {
        _dbContext.Database.Migrate();
    }
}