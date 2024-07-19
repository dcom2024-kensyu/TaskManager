using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Web.Models;

namespace TaskManager.Web.Services
{
    public class StatisticsService
    {
        private readonly ToDoDbContext _context;

        public StatisticsService(ToDoDbContext context)
        {
            _context = context;
        }

        public List<SelectListItem> GetRoleSelectList()
        {
            return _context.UserRoles
                .Select(e =>
                    new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.RoleName,
                    })
                .OrderBy(e => e.Value)
                .ToList();
        }
    }
}