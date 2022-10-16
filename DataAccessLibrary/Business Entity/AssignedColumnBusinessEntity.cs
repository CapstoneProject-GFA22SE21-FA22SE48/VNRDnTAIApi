using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class AssignedColumnBusinessEntity
    {
        private IUnitOfWork work;
        public AssignedColumnBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<AssignedColumn>> GetAssignedColumnsAsync()
        {
            return (await work.AssignedColumns.GetAllAsync())
                .Where(assignedColumn => !assignedColumn.IsDeleted);
        }
        public async Task<IEnumerable<AssignedColumn>> GetAssignedColumnsByScribeIdAsync(Guid scribeId)
        {
            return (await work.AssignedColumns.GetAllAsync(nameof(AssignedColumn.Column)))
                .Where(assignedColumn => !assignedColumn.IsDeleted && assignedColumn.ScribeId.Equals(scribeId))
                .OrderBy(assignedColumn => int.Parse(assignedColumn.Column.Name.Split(" ")[1]));
        }
    }
}
