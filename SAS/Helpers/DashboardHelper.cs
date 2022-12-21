using SAS.DTO;
using SAS.Interfaces;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;


namespace SAS.Helpers
{
    public class DashboardHelper : IDashboardHelper
    {
        public IQueryable<UserDTO> SortUsers(IQueryable<UserDTO> users, DashboardTableRequestDTO request)
        {
            const string ascend = "ascend";
            const string ascendOrder = "ASC";
            const string descendOrder = "DESC";
            var tp = Type.GetType("SAS.DTO.UserDTO");
            var capitalOrder = request.orderColumnName.Substring(0, 1).ToUpper() + request.orderColumnName.Substring(1);
            var property = tp.GetProperties()
                             .FirstOrDefault(x => x.Name.ToString()
                             .Split(' ').Last() == capitalOrder)
                             .Name;
            var result = request.orderBy == ascend ? users.OrderBy($"{property} {ascendOrder}") : users.OrderBy($"{property} {descendOrder}");
            return result;
        }

        public IQueryable<UserDTO> SearchUsers(IQueryable<UserDTO> users, DashboardTableRequestDTO request)
        {
            var tp = Type.GetType("SAS.DTO.UserDTO");
            var capitalSearchColumnName = request.searchColumn.Substring(0, 1).ToUpper() + request.searchColumn.Substring(1);
            var property = tp.GetProperties()
                             .FirstOrDefault(x => x.Name.ToString()
                             .Split(' ').Last() == capitalSearchColumnName)
                             .Name;
            var result = users.Where($"{property}.Contains(@0)", request.searchString);
            return result;
        }
    }
}
