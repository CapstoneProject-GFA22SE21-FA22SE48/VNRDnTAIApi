using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using DTOsLibrary.AdminReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNRDnTAILibrary.Utilities;

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
            return user.OrderBy(u => u.Username);
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
            return user.OrderBy(u => u.Username);
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

        public async Task<IEnumerable<AdminDTO>> GetAdminsAsync()
        {
            List<AdminDTO> admins =
                (from admin in (await work.Users.GetAllAsync())
                        .Where(user => !user.IsDeleted && user.Role == (int)UserRoles.ADMIN)
                 select new AdminDTO
                 {
                     Id = admin.Id,
                     Username = admin.Username,
                     PendingRequests = ""
                 }).ToList();

            IEnumerable<LawModificationRequest> lm =
                (await work.LawModificationRequests.GetAllAsync()).Where(lm => !lm.IsDeleted && lm.Status == (int)Status.Pending);
            IEnumerable<SignModificationRequest> sm =
                (await work.SignModificationRequests.GetAllAsync()).Where(sm => !sm.IsDeleted && sm.Status == (int)Status.Pending);
            IEnumerable<QuestionModificationRequest> qm =
                (await work.QuestionModificationRequests.GetAllAsync()).Where(qm => !qm.IsDeleted && qm.Status == (int)Status.Pending);
            IEnumerable<UserModificationRequest> um =
                (await work.UserModificationRequests.GetAllAsync()).Where(um => !um.IsDeleted && um.Status == (int)Status.Pending);
            foreach (AdminDTO admin in admins)
            {
                admin.PendingRequests = (lm.Where(lm => lm.AdminId == admin.Id).Count()
                    + sm.Where(sm => sm.AdminId == admin.Id).Count()
                    + qm.Where(qm => qm.AdminId == admin.Id).Count()
                    + um.Where(um => um.ArbitratingAdminId == admin.Id).Count()) + " yêu cầu đang chờ duyệt";
            }

            return admins.OrderBy(a => a.Username);
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return (await work.Users.GetAllAsync())
                .Where(user => !user.IsDeleted && user.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<User> GetUserAsyncByUsername(string username)
        {
            return (await work.Users.GetAllAsync())
                .Where(user => user.Username != null
                    && (!user.IsDeleted && user.Username.Equals(username)))
                .FirstOrDefault();
        }
        public async Task<User> GetUserAsyncByGmail(string gmail)
        {
            return (await work.Users.GetAllAsync())
                .Where(user => user.Gmail != null
                    && (!user.IsDeleted && user.Gmail.Equals(gmail)))
                .FirstOrDefault();
        }

        public async Task<User> GetUserAsyncByInfo(string username, string gmail)
        {
            return (await work.Users.GetAllAsync())
                .Where(user => 
                    (user.Username != null && !user.IsDeleted && user.Username.Equals(username))
                    || (user.Gmail != null && !user.IsDeleted && user.Gmail.Equals(gmail))
                 ).FirstOrDefault();
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

        //Deactivate Scribe
        public async Task<User> DeactivateScribe(User scribe)
        {
            User deactivatingScribe = await work.Users.GetAsync(scribe.Id);
            deactivatingScribe.Status = (int)Status.Deactivated;
            await work.Save();
            return deactivatingScribe;

        }

        //Re Enable Scribe
        public async Task<User> ReEnableScribe(User scribe)
        {
            User reEnablingScribe = await work.Users.GetAsync(scribe.Id);
            reEnablingScribe.Status = (int)Status.Active;
            await work.Save();
            return reEnablingScribe;

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
                .Where((user) => !user.IsDeleted && user.Username == username
                    && user.Password == password)
                .FirstOrDefault();
        }

        public async Task<User> RegisterMember(string username, string password, string? email)
        {
            User user = new User();
            user.Id = Guid.NewGuid();
            user.CreatedDate = DateTime.Now;
            user.Username = username;
            user.Password = password;
            user.Gmail = email;
            user.Status = 5;
            user.Role = (int)UserRoles.MEMBER;
            user.IsDeleted = false;

            await work.Users.AddAsync(user);
            await work.Save();

            return user;
        }

        public async Task<User> LoginWithGmail(string gmail)
        {
            User user = (await work.Users.GetAllAsync())
                .Where((user) => !user.IsDeleted && user.Gmail != null && user.Gmail == gmail)
                .FirstOrDefault();
            if (user == null)
            {
                //string password = StringUtils.GenerateRandom(12);
                return await RegisterMember(null, null, gmail);
            }
            return user;
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
