using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary.ManageTasks;
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

        public async Task<IEnumerable<TaskDTO>> GetTasksAsync()
        {
           List<TaskDTO> taskList = new List<TaskDTO>();

            List<Column> columns = (await work.Columns.GetAllAsync()).Where(c => !c.IsDeleted).ToList();
            IEnumerable<AssignedColumn> assignedColumns = (await work.AssignedColumns.GetAllAsync()).Where(a => !a.IsDeleted);

            List<QuestionCategory> questionCategories = (await work.QuestionCategories.GetAllAsync()).Where(q => !q.IsDeleted).ToList();
            IEnumerable<AssignedQuestionCategory> assignedQuestionCategories = (await work.AssignedQuestionCategories.GetAllAsync())
               .Where(a => !a.IsDeleted);

            List<SignCategory> signCategories = (await work.SignCategories.GetAllAsync()).Where(s => !s.IsDeleted).ToList();
            IEnumerable<AssignedSignCategory> assignedSignCategories = (await work.AssignedSignCategories.GetAllAsync())
              .Where(a => !a.IsDeleted);

            IEnumerable<User> scribes = (await work.Users.GetAllAsync())
                .Where(u => !u.IsDeleted && u.Status == (int)Status.Active && u.Role == (int)UserRoles.SCRIBE);

            AssignedColumn lastAssignedColumn = assignedColumns.Last();
            foreach(Column cl in columns)
            {
                foreach(AssignedColumn ac in assignedColumns)
                {
                    if(ac.ColumnId == cl.Id)
                    {
                        taskList.Add(new TaskDTO
                        {
                            TaskId = ac.ColumnId,
                            TaskName = "Mục > " + cl.Name,
                            ScribeId = ac.ScribeId,
                            ScribeName = scribes.Where(s => s.Id == ac.ScribeId).FirstOrDefault().Username,
                            IsAssigned = true
                        });
                        break;
                    }
                    if (ac.Equals(lastAssignedColumn))
                    {
                        taskList.Add(new TaskDTO
                        {
                            TaskId = cl.Id,
                            TaskName = "Mục > " + cl.Name,
                            ScribeId = null,
                            ScribeName = null,
                            IsAssigned = false
                        });
                        break;
                    }
                }
            }

            AssignedSignCategory lastAssignedSignCategory = assignedSignCategories.Last();
            foreach (SignCategory sc in signCategories)
            {
                foreach (AssignedSignCategory ac in assignedSignCategories)
                {
                    if (sc.Id == ac.SignCategoryId)
                    {
                        taskList.Add(new TaskDTO
                        {
                            TaskId = ac.SignCategoryId,
                            TaskName = "Biển báo > " + sc.Name,
                            ScribeId = ac.ScribeId,
                            ScribeName = scribes.Where(s => s.Id == ac.ScribeId).FirstOrDefault().Username,
                            IsAssigned = true
                        });
                        break;
                    }
                    if (ac.Equals(lastAssignedSignCategory))
                    {
                        taskList.Add(new TaskDTO
                        {
                            TaskId = sc.Id,
                            TaskName = "Biển báo > " + sc.Name,
                            ScribeId = null,
                            ScribeName = null,
                            IsAssigned = false
                        });
                        break;
                    }
                }
            }

            AssignedQuestionCategory lastAssignedQuestionCategory = assignedQuestionCategories.Last();
            List<TestCategory> testCategories = (await work.TestCategories.GetAllAsync())
                .Where(t => !t.IsDeleted).ToList();
            foreach (QuestionCategory qc in questionCategories)
            {
                foreach (AssignedQuestionCategory aq in assignedQuestionCategories)
                {
                    if (qc.Id == aq.QuestionCategoryId)
                    {
                        taskList.Add(new TaskDTO
                        {
                            TaskId = aq.QuestionCategoryId,
                            TaskName = "Câu hỏi " + 
                                testCategories.Where(t => t.Id == qc.TestCategoryId)
                                .FirstOrDefault().Name 
                                +  " > " + qc.Name,
                            ScribeId = aq.ScribeId,
                            ScribeName = scribes.Where(s => s.Id == aq.ScribeId).FirstOrDefault().Username,
                            IsAssigned = true
                        });
                        break;
                    }
                    if (aq.Equals(lastAssignedQuestionCategory))
                    {
                        taskList.Add(new TaskDTO
                        {
                            TaskId = qc.Id,
                            TaskName = "Câu hỏi " + 
                                testCategories.Where(t => t.Id == qc.TestCategoryId)
                                .FirstOrDefault().Name
                                + " > " + qc.Name,
                            ScribeId = null,
                            ScribeName = null,
                            IsAssigned = false
                        });
                        break;
                    }
                }
            }

            return taskList.OrderBy(t => t.TaskName);
        }
    }
}
