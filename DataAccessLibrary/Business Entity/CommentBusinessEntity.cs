using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<double> GetAverageRating()
        {
            double average = 0.0;
            List<Comment> comments = (await GetCommentsAsync()).ToList();
            if (!comments.ToArray().Length.Equals(0))
            {
                comments.ForEach(c => average += c.Rating);
                average /= comments.ToArray().Length;
            }

            return average;
        }

        public async Task<IEnumerable<Comment>> GetCommentByMemberId(Guid memberId)
        {
            IEnumerable<Comment> comments = (await work.Comments.GetAllAsync())
                .Where(c => !c.IsDeleted && c.UserId.Equals(memberId));
            return comments.OrderByDescending(c => c.CreatedDate);
        }
        public async Task<IEnumerable<UserCommentDTO>> GetUsersComments()
        {
            var data = (from comments in (await work.Comments.GetAllAsync()).Where(c => !c.IsDeleted)
                        join users in (await work.Users.GetAllAsync())
                         .Where(u => !u.IsDeleted && u.Role == (int)UserRoles.MEMBER)
                        on comments.UserId equals users.Id
                        select new UserCommentDTO
                        {
                            UserId = users.Id,
                            Avatar = users.Avatar,
                            DisplayName = users.DisplayName,
                            Content = comments.Content,
                            Rating = comments.Rating,
                            CreatedDate = comments.CreatedDate
                        }).ToList();
            return data;
        }

        public async Task<Comment> AddUserComment(Guid userId, Comment commentDTO)
        {
            Comment newComment = new Comment();
            newComment.Id = Guid.NewGuid();
            newComment.UserId = userId;
            newComment.Content = commentDTO.Content;
            newComment.Rating = commentDTO.Rating;
            newComment.CreatedDate = DateTime.Now;
            newComment.IsDeleted = false;
            await work.Comments.AddAsync(newComment);
            await work.Save();

            return newComment;
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
