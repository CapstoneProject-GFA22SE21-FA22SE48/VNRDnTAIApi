using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObjectLibrary;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    internal class FeedbackBusinessEntity
    {
        private IUnitOfWork work;
        public FeedbackBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

    }
}
