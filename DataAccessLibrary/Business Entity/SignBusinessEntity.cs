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
    public class SignBusinessEntity
    {
        private IUnitOfWork work;
        public SignBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Sign>> GetSignsAsync()
        {
            return (await work.Signs.GetAllAsync())
                .Where(sign => !sign.IsDeleted && sign.Status == (int)Status.Active);
        }
        public async Task<Sign> GetSignAsync(Guid id)
        {
            return (await work.Signs.GetAllAsync())
                .Where(sign => !sign.IsDeleted && sign.Id.Equals(id) && sign.Status == (int)Status.Active)
                .FirstOrDefault();
        }

        public async Task<IEnumerable<Sign>> GetSignsBySignCategoryIdAsync(Guid signCategoryId)
        {
            return (await work.Signs.GetAllAsync())
                .Where(sign => !sign.IsDeleted && sign.Status == (int)Status.Active && sign.SignCategoryId == signCategoryId);
        }

        public async Task<IEnumerable<SignDTO>> GetScribeAssignedSignsAsync(Guid scribeId)
        {
            var tmpData1 = from assignedSignCategory in
                                (await work.AssignedSignCategories.GetAllAsync())
                                .Where(asc => !asc.IsDeleted && asc.ScribeId == scribeId)
                           join signCategory in (await work.SignCategories.GetAllAsync())
                                                       .Where(sc => !sc.IsDeleted)
                           on assignedSignCategory.SignCategoryId equals signCategory.Id
                           join sign in (await work.Signs.GetAllAsync())
                                                       .Where(s => !s.IsDeleted && s.Status == (int)Status.Active)
                           on signCategory.Id equals sign.SignCategoryId
                           select new
                           {
                               Id = sign.Id,
                               SignCategoryId = sign.SignCategoryId,
                               Name = sign.Name,
                               Description = sign.Description,
                               ImageUrl = sign.ImageUrl,
                               Status = sign.Status,
                               IsDeleted = sign.IsDeleted,
                               SignCategory = signCategory,
                           };
            foreach (var sign in tmpData1)
            {
                sign.SignCategory.Signs = null;
                sign.SignCategory.AssignedSignCategories = null;
            }

            //Sign with SignParagraph + Paragraph
            var tmpData2 = from tmp1 in tmpData1
                           join signParagraph in (await work.SignParagraphs.GetAllAsync())
                           on tmp1.Id equals signParagraph.SignId

                           join paragraph in (await work.Paragraphs.GetAllAsync())
                           .Where(p => !p.IsDeleted && p.Status == (int)Status.Active)
                           on signParagraph.ParagraphId equals paragraph.Id
                           select new
                           {
                               Id = tmp1.Id,
                               SignCategoryId = tmp1.SignCategoryId,
                               Name = tmp1.Name,
                               Description = tmp1.Description,
                               ImageUrl = tmp1.ImageUrl,
                               Status = tmp1.Status,
                               IsDeleted = tmp1.IsDeleted,
                               SignCategory = tmp1.SignCategory,

                               SignParagraphParagraphId = paragraph.Id,
                               SignParagraphParagraphName = paragraph.Name,
                               SignParagraphParagraphDesc = paragraph.Description,

                               SignParagraphSectionId = paragraph.SectionId
                           };

            //With section
            var tmpData3 = from tmp2 in tmpData2
                           join section in (await work.Sections.GetAllAsync())
                            .Where(sc => !sc.IsDeleted && sc.Status == (int)Status.Active)
                            on tmp2.SignParagraphSectionId equals section.Id
                           select new
                           {
                               Id = tmp2.Id,
                               SignCategoryId = tmp2.SignCategoryId,
                               Name = tmp2.Name,
                               Description = tmp2.Description,
                               ImageUrl = tmp2.ImageUrl,
                               Status = tmp2.Status,
                               IsDeleted = tmp2.IsDeleted,
                               SignCategory = tmp2.SignCategory,

                               SignParagraphParagraphId = tmp2.SignParagraphParagraphId,
                               SignParagraphParagraphName = tmp2.SignParagraphParagraphName,
                               SignParagraphParagraphDesc = tmp2.SignParagraphParagraphDesc,

                               SignParagraphSectionId = tmp2.SignParagraphSectionId,
                               SignParagraphSectionName = section.Name,

                               SignParagraphStatueId = section.StatueId
                           };

            //With statue
            var tmpData4 = from tmp3 in tmpData3
                           join statue in (await work.Statues.GetAllAsync())
                            .Where(st => !st.IsDeleted && st.Status == (int)Status.Active)
                            on tmp3.SignParagraphStatueId equals statue.Id
                           select new
                           {
                               Id = tmp3.Id,
                               SignCategoryId = tmp3.SignCategoryId,
                               Name = tmp3.Name,
                               Description = tmp3.Description,
                               ImageUrl = tmp3.ImageUrl,
                               Status = tmp3.Status,
                               IsDeleted = tmp3.IsDeleted,
                               SignCategory = tmp3.SignCategory,

                               SignParagraphParagraphId = tmp3.SignParagraphParagraphId,
                               SignParagraphParagraphName = tmp3.SignParagraphParagraphName,
                               SignParagraphParagraphDesc = tmp3.SignParagraphParagraphDesc,

                               SignParagraphSectionId = tmp3.SignParagraphSectionId,
                               SignParagraphSectionName = tmp3.SignParagraphSectionName,

                               SignParagraphStatueId = tmp3.SignParagraphStatueId,
                               SignParagraphStatueName = statue.Name
                           };

            List<SignDTO> signDTOs = new List<SignDTO>();
            List<SignParagraphDTO> signParagraphList = null;

            foreach (var sign in tmpData1)
            {
                signParagraphList = new List<SignParagraphDTO>();
                foreach (var data in tmpData4)
                {
                    if (data.Id == sign.Id)
                    {
                        signParagraphList.Add(new SignParagraphDTO
                        {
                            SignParagraphParagraphId = data.SignParagraphParagraphId,
                            SignParagraphParagraphName = data.SignParagraphParagraphName,
                            SignParagraphParagraphDesc = data.SignParagraphParagraphDesc,

                            SignParagraphSectionId = data.SignParagraphSectionId,
                            SignParagraphSectionName = data.SignParagraphSectionName,

                            SignParagraphStatueId = data.SignParagraphStatueId,
                            SignParagraphStatueName = data.SignParagraphStatueName
                        });
                    }
                }
                signDTOs.Add(new SignDTO
                {
                    Id = sign.Id,
                    SignCategoryId = sign.SignCategoryId,
                    Name = sign.Name,
                    Description = sign.Description,
                    ImageUrl = sign.ImageUrl,
                    Status = sign.Status,
                    IsDeleted = sign.IsDeleted,
                    SignCategory = sign.SignCategory,

                    SignParagraphs = signParagraphList.OrderBy(r => int.Parse(r.SignParagraphStatueName.Split(" ")[1]))
                    .ThenBy(r => int.Parse(r.SignParagraphSectionName.Split(" ")[1]))
                    .ThenBy(r => r.SignParagraphParagraphName).ToList()
                });
            }


            return signDTOs.OrderBy(s => s.Name);
        }

        //This new sign is created for ROM of update, delete 
        public async Task<Sign> AddSignForROM(Sign sign)
        {
            sign.SignCategory = null;
            sign.Id = Guid.NewGuid();
            sign.Status = (int)Status.Deactivated;

            //If the sign is for Delete ROM, then keep IsDeleted = true
            if (sign.IsDeleted == true) { }
            else
            {
                sign.IsDeleted = false;
            }

            await work.Signs.AddAsync(sign);
            await work.Save();
            return sign;
        }
        public async Task<Sign> UpdateSign(Sign sign)
        {
            work.Signs.Update(sign);
            await work.Save();
            return sign;
        }
        public async Task RemoveSign(Guid id)
        {
            Sign sign = await work.Signs.GetAsync(id);
            sign.IsDeleted = true;
            work.Signs.Update(sign);
            await work.Save();
        }
    }
}
