using Microsoft.EntityFrameworkCore;

namespace Observe.OpenTelemetry.AppB.Model;

public class PersonRepo
{
    private readonly PersonDbContext _DbContext;

    public PersonRepo(PersonDbContext dbContext)
    {
        _DbContext = dbContext;
    }
    public async Task<List<Person>> Get()
    {
        Thread.Sleep(2000);
        return await _DbContext.People.ToListAsync();
    }
}