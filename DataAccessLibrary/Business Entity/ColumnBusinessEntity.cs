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
        public async Task<IEnumerable<Column>> GetColumnsAsync()
        {
            //IEnumerable<Column> columns = (await work.Columns.GetAllMultiIncludeAsync(
            //    include: column => column
            //    .Include(c => c.Statues)
            //    .ThenInclude(st => st.Sections)
            //    .ThenInclude(sc => sc.Paragraphs)
            //    ))
            //    .Where(column => !column.IsDeleted);
            //return columns;
            IEnumerable<Column> columns = (await work.Columns.GetAllAsync())
                .Where(column => !column.IsDeleted);

            List<Statue> statues = (await work.Statues.GetAllAsync())
                .Where(statue => !statue.IsDeleted
                        && statue.Status == (int)Status.Active).ToList();

            foreach (Column column in columns)
            {
                column.Statues = statues.Where(statue => statue.ColumnId == column.Id).ToList();
            }

            return columns;
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

        public async Task<Column> GetColumnAsync(Guid id)
        {
            Column column = (await work.Columns.GetAllAsync())
                .Where(column => !column.IsDeleted && column.Id.Equals(id))
                .FirstOrDefault();

            List<Statue> statues = (await work.Statues.GetAllAsync())
                .Where(statue => !statue.IsDeleted
                        && statue.Status == (int)Status.Active
                        && statue.ColumnId == column.Id).ToList();
            column.Statues = statues;
            return column;
        }
        public async Task<Column> AddColumn(Column column)
        {
            column.Id = Guid.NewGuid();
            column.IsDeleted = false;
            await work.Columns.AddAsync(column);
            await work.Save();
            return column;
        }
        public async Task<Column> UpdateColumn(Column column)
        {
            work.Columns.Update(column);
            await work.Save();
            return column;
        }
        public async Task RemoveColumn(Guid id)
        {
            Column column = await work.Columns.GetAsync(id);
            column.IsDeleted = true;
            work.Columns.Update(column);
            await work.Save();
        }
    }
}
