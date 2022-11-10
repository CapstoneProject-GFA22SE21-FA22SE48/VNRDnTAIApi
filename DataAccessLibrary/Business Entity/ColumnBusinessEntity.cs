using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class ColumnBusinessEntity
    {
        private IUnitOfWork work;
        public ColumnBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        //Scribe get assigned Column
        public async Task<IEnumerable<Column>> GetAssignedColumnsAsync(Guid columnId)
        {
            IEnumerable<Column> assignedColumns =
                (await work.Columns.GetAllAsync())
                .Where(column => !column.IsDeleted && column.Id == columnId);

            List<Statue> statues = (await work.Statues.GetAllAsync())
                .Where(statue => !statue.IsDeleted
                        && statue.Status == (int)Status.Active).ToList();

            foreach (Column column in assignedColumns)
            {
                column.Statues = statues.Where(statue => statue.ColumnId == column.Id).ToList();
            }
            return assignedColumns;
        }
    }
}
