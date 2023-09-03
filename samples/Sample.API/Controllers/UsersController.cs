using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample.API.Core;

namespace Sample.API;

[ApiController]
[Route("/api/[controller]/[action]")]
public class UsersController : ControllerBase
{
    private readonly AdminDbContext _ctx;

    public UsersController(AdminDbContext ctx)
    {
        _ctx = ctx;
    }
    [HttpGet]
    public async Task<IDictionary<string, object>> AllAsync()
    {
        var cnn = _ctx.Database.GetDbConnection();
        var users = await cnn.QueryAsync(@"SELECT *
from uc.uc_users uu ");

        return new Dictionary<string, object>() {
            { "code", 200 },
        { "data", users } };
    }
}
