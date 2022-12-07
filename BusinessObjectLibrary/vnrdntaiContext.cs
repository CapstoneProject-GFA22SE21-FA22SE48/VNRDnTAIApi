using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BusinessObjectLibrary
{
    public partial class vnrdntaiContext : DbContext
    {
        public vnrdntaiContext()
        {
        }

        public vnrdntaiContext(DbContextOptions<vnrdntaiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<AssignedColumn> AssignedColumns { get; set; }
        public virtual DbSet<AssignedQuestionCategory> AssignedQuestionCategories { get; set; }
        public virtual DbSet<AssignedSignCategory> AssignedSignCategories { get; set; }
        public virtual DbSet<Column> Columns { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Decree> Decrees { get; set; }
        public virtual DbSet<Gpssign> Gpssigns { get; set; }
        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<KeywordParagraph> KeywordParagraphs { get; set; }
        public virtual DbSet<LawModificationRequest> LawModificationRequests { get; set; }
        public virtual DbSet<Paragraph> Paragraphs { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionCategory> QuestionCategories { get; set; }
        public virtual DbSet<QuestionModificationRequest> QuestionModificationRequests { get; set; }
        public virtual DbSet<Reference> References { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Sign> Signs { get; set; }
        public virtual DbSet<SignCategory> SignCategories { get; set; }
        public virtual DbSet<SignModificationRequest> SignModificationRequests { get; set; }
        public virtual DbSet<SignParagraph> SignParagraphs { get; set; }
        public virtual DbSet<Statue> Statues { get; set; }
        public virtual DbSet<TestCategory> TestCategories { get; set; }
        public virtual DbSet<TestResult> TestResults { get; set; }
        public virtual DbSet<TestResultDetail> TestResultDetails { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserModificationRequest> UserModificationRequests { get; set; }
        public virtual DbSet<VehicleCategory> VehicleCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Answer__Question__10566F31");
            });

            modelBuilder.Entity<AssignedColumn>(entity =>
            {
                entity.HasKey(e => new { e.ColumnId, e.ScribeId })
                    .HasName("PK__Assigned__C05B11FD0975D591");

                entity.ToTable("AssignedColumn");

                entity.HasOne(d => d.Column)
                    .WithMany(p => p.AssignedColumns)
                    .HasForeignKey(d => d.ColumnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AssignedC__Colum__114A936A");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.AssignedColumns)
                    .HasForeignKey(d => d.ScribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AssignedC__Scrib__123EB7A3");
            });

            modelBuilder.Entity<AssignedQuestionCategory>(entity =>
            {
                entity.HasKey(e => new { e.QuestionCategoryId, e.ScribeId })
                    .HasName("PK__Assigned__92041373B83DF0BF");

                entity.ToTable("AssignedQuestionCategory");

                entity.HasOne(d => d.QuestionCategory)
                    .WithMany(p => p.AssignedQuestionCategories)
                    .HasForeignKey(d => d.QuestionCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AssignedQ__Quest__1332DBDC");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.AssignedQuestionCategories)
                    .HasForeignKey(d => d.ScribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AssignedQ__Scrib__14270015");
            });

            modelBuilder.Entity<AssignedSignCategory>(entity =>
            {
                entity.HasKey(e => new { e.SignCategoryId, e.ScribeId })
                    .HasName("PK__Assigned__E7CAD648D375CE3C");

                entity.ToTable("AssignedSignCategory");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.AssignedSignCategories)
                    .HasForeignKey(d => d.ScribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AssignedS__Scrib__160F4887");

                entity.HasOne(d => d.SignCategory)
                    .WithMany(p => p.AssignedSignCategories)
                    .HasForeignKey(d => d.SignCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AssignedS__SignC__151B244E");
            });

            modelBuilder.Entity<Column>(entity =>
            {
                entity.ToTable("Column");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Decree)
                    .WithMany(p => p.Columns)
                    .HasForeignKey(d => d.DecreeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Column__DecreeId__17036CC0");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__UserId__17F790F9");
            });

            modelBuilder.Entity<Decree>(entity =>
            {
                entity.ToTable("Decree");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Gpssign>(entity =>
            {
                entity.ToTable("GPSSign");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Latitude).HasColumnType("decimal(15, 12)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(15, 12)");

                entity.HasOne(d => d.Sign)
                    .WithMany(p => p.Gpssigns)
                    .HasForeignKey(d => d.SignId)
                    .HasConstraintName("FK__GPSSign__SignId__18EBB532");
            });

            modelBuilder.Entity<Keyword>(entity =>
            {
                entity.ToTable("Keyword");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<KeywordParagraph>(entity =>
            {
                entity.HasKey(e => new { e.KeywordId, e.ParagraphId })
                    .HasName("PK__KeywordP__852C78FC21FF554F");

                entity.ToTable("KeywordParagraph");

                entity.HasOne(d => d.Keyword)
                    .WithMany(p => p.KeywordParagraphs)
                    .HasForeignKey(d => d.KeywordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KeywordPa__Keywo__19DFD96B");

                entity.HasOne(d => d.Paragraph)
                    .WithMany(p => p.KeywordParagraphs)
                    .HasForeignKey(d => d.ParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KeywordPa__Parag__1AD3FDA4");
            });

            modelBuilder.Entity<LawModificationRequest>(entity =>
            {
                entity.ToTable("LawModificationRequest");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeniedReason).HasMaxLength(2000);

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.LawModificationRequestAdmins)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LawModifi__Admin__22751F6C");

                entity.HasOne(d => d.ModifiedParagraph)
                    .WithMany(p => p.LawModificationRequestModifiedParagraphs)
                    .HasForeignKey(d => d.ModifiedParagraphId)
                    .HasConstraintName("FK__LawModifi__Modif__208CD6FA");

                entity.HasOne(d => d.ModifiedSection)
                    .WithMany(p => p.LawModificationRequestModifiedSections)
                    .HasForeignKey(d => d.ModifiedSectionId)
                    .HasConstraintName("FK__LawModifi__Modif__1EA48E88");

                entity.HasOne(d => d.ModifiedStatue)
                    .WithMany(p => p.LawModificationRequestModifiedStatues)
                    .HasForeignKey(d => d.ModifiedStatueId)
                    .HasConstraintName("FK__LawModifi__Modif__1CBC4616");

                entity.HasOne(d => d.ModifyingParagraph)
                    .WithMany(p => p.LawModificationRequestModifyingParagraphs)
                    .HasForeignKey(d => d.ModifyingParagraphId)
                    .HasConstraintName("FK__LawModifi__Modif__1F98B2C1");

                entity.HasOne(d => d.ModifyingSection)
                    .WithMany(p => p.LawModificationRequestModifyingSections)
                    .HasForeignKey(d => d.ModifyingSectionId)
                    .HasConstraintName("FK__LawModifi__Modif__1DB06A4F");

                entity.HasOne(d => d.ModifyingStatue)
                    .WithMany(p => p.LawModificationRequestModifyingStatues)
                    .HasForeignKey(d => d.ModifyingStatueId)
                    .HasConstraintName("FK__LawModifi__Modif__1BC821DD");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.LawModificationRequestScribes)
                    .HasForeignKey(d => d.ScribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LawModifi__Scrib__2180FB33");
            });

            modelBuilder.Entity<Paragraph>(entity =>
            {
                entity.ToTable("Paragraph");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AdditionalPenalty).HasMaxLength(2000);

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.Paragraphs)
                    .HasForeignKey(d => d.SectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Paragraph__Secti__236943A5");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.HasOne(d => d.QuestionCategory)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuestionCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Question__Questi__25518C17");

                entity.HasOne(d => d.TestCategory)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.TestCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Question__TestCa__245D67DE");
            });

            modelBuilder.Entity<QuestionCategory>(entity =>
            {
                entity.ToTable("QuestionCategory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.TestCategory)
                    .WithMany(p => p.QuestionCategories)
                    .HasForeignKey(d => d.TestCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionC__TestC__2645B050");
            });

            modelBuilder.Entity<QuestionModificationRequest>(entity =>
            {
                entity.HasKey(e => e.ModifyingQuestionId)
                    .HasName("PK__Question__94B14AC72A4C37A0");

                entity.ToTable("QuestionModificationRequest");

                entity.Property(e => e.ModifyingQuestionId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeniedReason).HasMaxLength(2000);

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.QuestionModificationRequestAdmins)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionM__Admin__2A164134");

                entity.HasOne(d => d.ModifiedQuestion)
                    .WithMany(p => p.QuestionModificationRequestModifiedQuestions)
                    .HasForeignKey(d => d.ModifiedQuestionId)
                    .HasConstraintName("FK__QuestionM__Modif__282DF8C2");

                entity.HasOne(d => d.ModifyingQuestion)
                    .WithOne(p => p.QuestionModificationRequestModifyingQuestion)
                    .HasForeignKey<QuestionModificationRequest>(d => d.ModifyingQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionM__Modif__2739D489");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.QuestionModificationRequestScribes)
                    .HasForeignKey(d => d.ScribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionM__Scrib__29221CFB");
            });

            modelBuilder.Entity<Reference>(entity =>
            {
                entity.HasKey(e => new { e.ParagraphId, e.ReferenceParagraphId })
                    .HasName("PK__Referenc__44419EC08B7A08C2");

                entity.ToTable("Reference");

                entity.HasOne(d => d.Paragraph)
                    .WithMany(p => p.ReferenceParagraphs)
                    .HasForeignKey(d => d.ParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reference__Parag__2B0A656D");

                entity.HasOne(d => d.ReferenceParagraph)
                    .WithMany(p => p.ReferenceReferenceParagraphs)
                    .HasForeignKey(d => d.ReferenceParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reference__Refer__2BFE89A6");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.ToTable("Section");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.MaxPenalty).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.MinPenalty).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Statue)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.StatueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Section__StatueI__2DE6D218");

                entity.HasOne(d => d.VehicleCategory)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.VehicleCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Section__Vehicle__2CF2ADDF");
            });

            modelBuilder.Entity<Sign>(entity =>
            {
                entity.ToTable("Sign");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.SignCategory)
                    .WithMany(p => p.Signs)
                    .HasForeignKey(d => d.SignCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sign__SignCatego__2EDAF651");
            });

            modelBuilder.Entity<SignCategory>(entity =>
            {
                entity.ToTable("SignCategory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<SignModificationRequest>(entity =>
            {
                entity.ToTable("SignModificationRequest");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeniedReason).HasMaxLength(2000);

                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.Property(e => e.ModifiedGpssignId).HasColumnName("ModifiedGPSSignId");

                entity.Property(e => e.ModifyingGpssignId).HasColumnName("ModifyingGPSSignId");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.SignModificationRequestAdmins)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK__SignModif__Admin__3587F3E0");

                entity.HasOne(d => d.ModifiedGpssign)
                    .WithMany(p => p.SignModificationRequestModifiedGpssigns)
                    .HasForeignKey(d => d.ModifiedGpssignId)
                    .HasConstraintName("FK__SignModif__Modif__32AB8735");

                entity.HasOne(d => d.ModifiedSign)
                    .WithMany(p => p.SignModificationRequestModifiedSigns)
                    .HasForeignKey(d => d.ModifiedSignId)
                    .HasConstraintName("FK__SignModif__Modif__30C33EC3");

                entity.HasOne(d => d.ModifyingGpssign)
                    .WithMany(p => p.SignModificationRequestModifyingGpssigns)
                    .HasForeignKey(d => d.ModifyingGpssignId)
                    .HasConstraintName("FK__SignModif__Modif__31B762FC");

                entity.HasOne(d => d.ModifyingSign)
                    .WithMany(p => p.SignModificationRequestModifyingSigns)
                    .HasForeignKey(d => d.ModifyingSignId)
                    .HasConstraintName("FK__SignModif__Modif__2FCF1A8A");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.SignModificationRequestScribes)
                    .HasForeignKey(d => d.ScribeId)
                    .HasConstraintName("FK__SignModif__Scrib__3493CFA7");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SignModificationRequestUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__SignModif__UserI__339FAB6E");
            });

            modelBuilder.Entity<SignParagraph>(entity =>
            {
                entity.ToTable("SignParagraph");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Paragraph)
                    .WithMany(p => p.SignParagraphs)
                    .HasForeignKey(d => d.ParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignParag__Parag__37703C52");

                entity.HasOne(d => d.Sign)
                    .WithMany(p => p.SignParagraphs)
                    .HasForeignKey(d => d.SignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignParag__SignI__367C1819");
            });

            modelBuilder.Entity<Statue>(entity =>
            {
                entity.ToTable("Statue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Column)
                    .WithMany(p => p.Statues)
                    .HasForeignKey(d => d.ColumnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Statue__ColumnId__3864608B");
            });

            modelBuilder.Entity<TestCategory>(entity =>
            {
                entity.ToTable("TestCategory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.ToTable("TestResult");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.TestCategory)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(d => d.TestCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__TestC__3A4CA8FD");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__UserI__395884C4");
            });

            modelBuilder.Entity<TestResultDetail>(entity =>
            {
                entity.ToTable("TestResultDetail");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.TestResultDetails)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("FK__TestResul__Answe__3C34F16F");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TestResultDetails)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__Quest__3B40CD36");

                entity.HasOne(d => d.TestResult)
                    .WithMany(p => p.TestResultDetails)
                    .HasForeignKey(d => d.TestResultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__TestR__3D2915A8");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Avatar).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DisplayName).HasMaxLength(255);

                entity.Property(e => e.Gmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserModificationRequest>(entity =>
            {
                entity.HasKey(e => new { e.ModifyingUserId, e.ModifiedUserId })
                    .HasName("PK__UserModi__C95E8661B97351D0");

                entity.ToTable("UserModificationRequest");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeniedReason).HasMaxLength(2000);

                entity.HasOne(d => d.ArbitratingAdmin)
                    .WithMany(p => p.UserModificationRequestArbitratingAdmins)
                    .HasForeignKey(d => d.ArbitratingAdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserModif__Arbit__40F9A68C");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.UserModificationRequestModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserModif__Modif__3F115E1A");

                entity.HasOne(d => d.ModifyingUser)
                    .WithMany(p => p.UserModificationRequestModifyingUsers)
                    .HasForeignKey(d => d.ModifyingUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserModif__Modif__3E1D39E1");

                entity.HasOne(d => d.PromotingAdmin)
                    .WithMany(p => p.UserModificationRequestPromotingAdmins)
                    .HasForeignKey(d => d.PromotingAdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserModif__Promo__40058253");
            });

            modelBuilder.Entity<VehicleCategory>(entity =>
            {
                entity.ToTable("VehicleCategory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
