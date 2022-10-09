using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            IEnumerable<Column> columns = (await work.Columns.GetAllMultiIncludeAsync(
                include: column => column
                .Include(c => c.Statues)
                .ThenInclude(st => st.Sections)
                .ThenInclude(sc => sc.Paragraphs)
                ))
                .Where(column => !column.IsDeleted);
            return columns;
        }
        public async Task<Column> GetColumnAsync(Guid id)
        {
            return (await work.Columns.GetAllAsync())
                .Where(column => !column.IsDeleted && column.Id.Equals(id))
                .FirstOrDefault();
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
