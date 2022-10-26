using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary.SearchSign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class SignCategoryBusinessEntity
    {
        private IUnitOfWork work;
        public SignCategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<SignCategoryDTO>> GetSignCategoriesDTOList()
        {
            var res = new List<SignCategoryDTO>();
            var signCatList = (await work.SignCategories.GetAllAsync(nameof(SignCategory.Signs)))
               .Where(signCategory => !signCategory.IsDeleted);

            foreach (var signCategory in signCatList)
            {
                res.Add(new SignCategoryDTO
                {
                    Id = signCategory.Id,
                    Name = signCategory.Name,
                    SearchSignDTOs = signCategory.Signs.Where(s => !s.IsDeleted && s.Status == (int)Status.Active).Select(s => new SearchSignDTO
                    {
                        Name = s.Name,
                        Description = s.Description,
                        ImageUrl = s.ImageUrl
                    }).ToList()
                });
            }

            return res.OrderBy(r => r.Name);
        }
        public async Task<IEnumerable<SignCategory>> GetSignCategoriesAsync()
        {
            return (await work.SignCategories.GetAllAsync())
                .Where(signCategory => !signCategory.IsDeleted);
        }
        public async Task<SignCategory> GetSignCategoryAsync(Guid id)
        {
            return (await work.SignCategories.GetAllAsync())
                .Where(signCategory => !signCategory.IsDeleted && signCategory.Id.Equals(id))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<SignCategory>> GetScribeAssignedSignCategoriesAsync(Guid scribeId)
        {
            IEnumerable<SignCategory> signCategorys = from assignedSignCategory in
                                          (await work.AssignedSignCategories.GetAllAsync())
                                          .Where(asc => !asc.IsDeleted && asc.ScribeId == scribeId)
                                                      join signCategory in (await work.SignCategories.GetAllAsync())
                                                                                   .Where(sc => !sc.IsDeleted)
                                                      on assignedSignCategory.SignCategoryId equals signCategory.Id

                                                      select signCategory;

            return signCategorys;
        }

        public async Task<SignCategory> AddSignCategory(SignCategory signCategory)
        {
            signCategory.Id = Guid.NewGuid();
            signCategory.IsDeleted = false;
            await work.SignCategories.AddAsync(signCategory);
            await work.Save();
            return signCategory;
        }
        public async Task<SignCategory> UpdateSignCategory(SignCategory signCategory)
        {
            work.SignCategories.Update(signCategory);
            await work.Save();
            return signCategory;
        }
        public async Task RemoveSignCategory(Guid id)
        {
            SignCategory signCategory = await work.SignCategories.GetAsync(id);
            signCategory.IsDeleted = true;
            work.SignCategories.Update(signCategory);
            await work.Save();
        }
    }
}
