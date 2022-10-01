using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class CommentBusinessEntity
    {
        private IUnitOfWork work;
        public CommentBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            return (await work.Comments.GetAllAsync())
                .Where(comment => !comment.IsDeleted);
        }
        public async Task<Comment> GetCommentAsync(Guid id)
        {
            return (await work.Comments.GetAllAsync())
                .Where(comment => !comment.IsDeleted && comment.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<Comment> AddComment(Comment comment)
        {
            comment.Id = Guid.NewGuid();
            comment.IsDeleted = false;
            await work.Comments.AddAsync(comment);
            await work.Save();
            return comment;
        }
        public async Task<Comment> UpdateComment(Comment comment)
        {
            work.Comments.Update(comment);
            await work.Save();
            return comment;
        }
        public async Task RemoveComment(Guid id)
        {
            Comment comment = await work.Comments.GetAsync(id);
            comment.IsDeleted = true;
            work.Comments.Update(comment);
            await work.Save();
        }
    }
}
