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
    public class UserBusinessEntity
    {
        private IUnitOfWork work;
        public UserBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return (await work.Users.GetAllAsync())
                .Where(user => !user.IsDeleted);
        }

        public async Task<IEnumerable<User>> GetMembersAsync()
        {
            var user = (await work.Users.GetAllAsync())
                .Where(user => !user.IsDeleted && user.Role == (int)UserRoles.MEMBER);
            return user.OrderBy(u => u.Status).ThenBy(u => u.Username);
        }

        public async Task<IEnumerable<User>> GetMembersByUserNameAsync(string keywordUserName)
        {
            var user = (await work.Users.GetAllAsync())
                .Where(
                    user => !user.IsDeleted &&
                    user.Role == (int)UserRoles.MEMBER &&
                    user.Username.ToLower().Contains(keywordUserName.ToLower())
                 );
            return user.OrderBy(u => u.Status).ThenBy(u => u.Username);
        }

        public async Task<IEnumerable<User>> GetMembersByCreatedDateRangeAsync(
            DateTime startDate,
            DateTime endDate)
        {
            var user = (await work.Users.GetAllAsync())
                .Where(
                    user => !user.IsDeleted &&
                    user.Role == (int)UserRoles.MEMBER &&
                    user.CreatedDate.CompareTo(startDate) >= 0 &&
                    user.CreatedDate.CompareTo(endDate) <= 0
                 );
            return user.OrderBy(u => u.Status).ThenBy(u => u.Username);
        }

        public async Task<IEnumerable<User>> GetScribesAsync()
        {
            var user = (await work.Users.GetAllAsync())
                .Where(user => !user.IsDeleted && user.Role == (int)UserRoles.SCRIBE);
            return user.OrderBy(u => u.Status).ThenBy(u => u.Username);
        }

        public async Task<IEnumerable<User>> GetScribesByUserNameAsync(string keywordUserName)
        {
            var user = (await work.Users.GetAllAsync())
                .Where(
                    user => !user.IsDeleted &&
                    user.Role == (int)UserRoles.SCRIBE &&
                    user.Username.ToLower().Contains(keywordUserName.ToLower())
                 );
            return user.OrderBy(u => u.Status).ThenBy(u => u.Username);
        }

        public async Task<IEnumerable<User>> GetScribesByCreatedDateRangeAsync(
            DateTime startDate,
            DateTime endDate)
        {
            var user = (await work.Users.GetAllAsync())
                .Where(
                    user => !user.IsDeleted &&
                    user.Role == (int)UserRoles.SCRIBE &&
                    user.CreatedDate.CompareTo(startDate) >= 0 &&
                    user.CreatedDate.CompareTo(endDate) <= 0
                 );
            return user.OrderBy(u => u.Status).ThenBy(u => u.Username);
        }

        public async Task<IEnumerable<User>> GetAdminsAsync()
        {
            var user = (await work.Users.GetAllAsync())
                .Where(user => !user.IsDeleted && user.Role == (int)UserRoles.ADMIN);
            return user.OrderBy(u => u.Status).ThenBy(u => u.Username);
        }

        public async Task<IEnumerable<User>> GetAdminsByUserNameAsync(string keywordUserName)
        {
            var user = (await work.Users.GetAllAsync())
                .Where(
                    user => !user.IsDeleted &&
                    user.Role == (int)UserRoles.ADMIN &&
                    user.Username.ToLower().Contains(keywordUserName.ToLower())
                 );
            return user.OrderBy(u => u.Status).ThenBy(u => u.Username);
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return (await work.Users.GetAllAsync())
                .Where(user => !user.IsDeleted && user.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<User> AddUser(User user)
        {
            user.Id = Guid.NewGuid();
            user.IsDeleted = false;
            await work.Users.AddAsync(user);
            await work.Save();
            return user;
        }
        public async Task<User> UpdateUser(User user)
        {
            work.Users.Update(user);
            await work.Save();
            return user;
        }

        //Deactivate Member: status (deactivated) + comment (isDeleted = true)
        public async Task<User> DeactivateMember(User member)
        {
            User deactivatingMember = await work.Users.GetAsync(member.Id);
            deactivatingMember.Status = (int)Status.Deactivated;
            IEnumerable<Comment> comments = (await work.Comments.GetAllAsync())
                .Where(c => !c.IsDeleted && c.UserId == member.Id);
            foreach (Comment comment in comments)
            {
                comment.IsDeleted = true;
            }
            await work.Save();
            return deactivatingMember;

        }

        public async Task<User> ReEnableMember(User member)
        {
            User reEnablingMember = await work.Users.GetAsync(member.Id);
            reEnablingMember.Status = (int)Status.Active;
            await work.Save();
            return reEnablingMember;

        }
        public async Task RemoveUser(Guid id)
        {
            User user = await work.Users.GetAsync(id);
            user.IsDeleted = true;
            work.Users.Update(user);
            await work.Save();
        }

        public async Task<User> LoginWeb(string username, string password)
        {
            return (await work.Users.GetAllAsync())
                .Where((user) => user.Username == username
                    && user.Password == password && user.Role != (int)UserRoles.MEMBER)
                .FirstOrDefault();
        }

        public async Task<User> LoginMobile(string username, string password)
        {
            return (await work.Users.GetAllAsync())
                .Where((user) => user.Username == username
                    && user.Password == password)
                .FirstOrDefault();
        }

        public async Task<User> RegisterMember(string username, string password, string email)
        {
            User u = (await work.Users.GetAllAsync())
                .FirstOrDefault((user) => !user.IsDeleted && (user.Username == username
                    || user.Gmail == email));

            User user = new User();
            if (u != null)
            {
                user.Id = Guid.NewGuid();
                user.CreatedDate = DateTime.Now;
                user.Username = username;
                user.Password = password;
                if (!string.IsNullOrEmpty(email))
                {
                    user.Gmail = email;
                }
                user.Role = (int)UserRoles.MEMBER;
                user.Status = 5;
                user.IsDeleted = false;
                await work.Users.AddAsync(user);
                await work.Save();
            }

            return user;
        }

        public async Task<User> LoginWithEmail(string email)
        {
            return (await work.Users.GetAllAsync())
                .Where((user) => user.Gmail == email)
                .FirstOrDefault();
        }

        public async Task<MemberByYearReportDTO> GetMemberByYearReport()
        {
            MemberByYearReportDTO memberByYear = new MemberByYearReportDTO();
            List<string> listNumberMemberByYear = new List<string>();
            IEnumerable<User> users = await work.Users.GetAllAsync();
            int minYear = (from u in users select u.CreatedDate).Min().Year;
            for (int i = minYear; i <= DateTime.Now.Year; i++)
            {
                listNumberMemberByYear.Add(i.ToString() + "-" + users
                .Where(u => !u.IsDeleted && u.Role == (int)UserRoles.MEMBER
                        && u.CreatedDate.Year == i).Count().ToString());
            }
            memberByYear.MembersByYear = listNumberMemberByYear;
            return memberByYear;
        }

        public async Task<NewMemberReportDTO> GetNewMemberReport(int month, int year)
        {
            NewMemberReportDTO newMember = new NewMemberReportDTO();
            List<string> listNumberNewMemberByDay = new List<string>();
            IEnumerable<User> users = await work.Users.GetAllAsync();
            int dayCount = DateTime.DaysInMonth(year, month);
            for (int i = 1; i <= dayCount; i++)
            {
                listNumberNewMemberByDay.Add(i.ToString() + "-" + users
                    .Where(u => !u.IsDeleted && u.Role == (int)UserRoles.MEMBER
                            && u.CreatedDate.Day == i && u.CreatedDate.Month == month
                            && u.CreatedDate.Year == year).Count().ToString());
            }
            newMember.NewMembersByDay = listNumberNewMemberByDay;
            return newMember;
        }

    }
}
