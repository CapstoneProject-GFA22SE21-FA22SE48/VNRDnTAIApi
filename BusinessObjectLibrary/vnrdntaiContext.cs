using Microsoft.EntityFrameworkCore;

#nullable disable

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
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

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
                    .HasConstraintName("FK__Answer__Question__6FE99F9F");
            });

            modelBuilder.Entity<AssignedColumn>(entity =>
            {
                entity.HasKey(e => new { e.ColumnId, e.ScribeId })
                    .HasName("PK__Assigned__C05B11FDB0B0F08F");

                entity.ToTable("AssignedColumn");

                entity.HasOne(d => d.Column)
                    .WithMany(p => p.AssignedColumns)
                    .HasForeignKey(d => d.ColumnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AssignedC__Colum__2B3F6F97");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.AssignedColumns)
                    .HasForeignKey(d => d.ScribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AssignedC__Scrib__2C3393D0");
            });

            modelBuilder.Entity<AssignedQuestionCategory>(entity =>
            {
                entity.HasKey(e => new { e.QuestionCategoryId, e.ScribeId })
                    .HasName("PK__Assigned__920413739E02E46F");

                entity.ToTable("AssignedQuestionCategory");

                entity.HasOne(d => d.QuestionCategory)
                    .WithMany(p => p.AssignedQuestionCategories)
                    .HasForeignKey(d => d.QuestionCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AssignedQ__Quest__07C12930");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.AssignedQuestionCategories)
                    .HasForeignKey(d => d.ScribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AssignedQ__Scrib__08B54D69");
            });

            modelBuilder.Entity<AssignedSignCategory>(entity =>
            {
                entity.HasKey(e => new { e.SignCategoryId, e.ScribeId })
                    .HasName("PK__Assigned__E7CAD648B705ACFC");

                entity.ToTable("AssignedSignCategory");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.AssignedSignCategories)
                    .HasForeignKey(d => d.ScribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AssignedS__Scrib__04E4BC85");

                entity.HasOne(d => d.SignCategory)
                    .WithMany(p => p.AssignedSignCategories)
                    .HasForeignKey(d => d.SignCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AssignedS__SignC__03F0984C");
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
                    .HasConstraintName("FK__Column__DecreeId__267ABA7A");
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
                    .HasConstraintName("FK__Comment__UserId__787EE5A0");
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

                entity.Property(e => e.Latitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.Longtitude).HasColumnType("decimal(12, 9)");

                entity.HasOne(d => d.Sign)
                    .WithMany(p => p.Gpssigns)
                    .HasForeignKey(d => d.SignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GPSSign__SignId__5812160E");
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
                    .HasName("PK__KeywordP__852C78FCD11E998C");

                entity.ToTable("KeywordParagraph");

                entity.HasOne(d => d.Keyword)
                    .WithMany(p => p.KeywordParagraphs)
                    .HasForeignKey(d => d.KeywordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KeywordPa__Keywo__4F7CD00D");

                entity.HasOne(d => d.Paragraph)
                    .WithMany(p => p.KeywordParagraphs)
                    .HasForeignKey(d => d.ParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KeywordPa__Parag__5070F446");
            });

            modelBuilder.Entity<LawModificationRequest>(entity =>
            {
                entity.ToTable("LawModificationRequest");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.LawModificationRequestAdmins)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LawModifi__Admin__4AB81AF0");

                entity.HasOne(d => d.ModifiedParagraph)
                    .WithMany(p => p.LawModificationRequestModifiedParagraphs)
                    .HasForeignKey(d => d.ModifiedParagraphId)
                    .HasConstraintName("FK__LawModifi__Modif__48CFD27E");

                entity.HasOne(d => d.ModifiedSection)
                    .WithMany(p => p.LawModificationRequestModifiedSections)
                    .HasForeignKey(d => d.ModifiedSectionId)
                    .HasConstraintName("FK__LawModifi__Modif__46E78A0C");

                entity.HasOne(d => d.ModifiedStatue)
                    .WithMany(p => p.LawModificationRequestModifiedStatues)
                    .HasForeignKey(d => d.ModifiedStatueId)
                    .HasConstraintName("FK__LawModifi__Modif__44FF419A");

                entity.HasOne(d => d.ModifyingParagraph)
                    .WithMany(p => p.LawModificationRequestModifyingParagraphs)
                    .HasForeignKey(d => d.ModifyingParagraphId)
                    .HasConstraintName("FK__LawModifi__Modif__47DBAE45");

                entity.HasOne(d => d.ModifyingSection)
                    .WithMany(p => p.LawModificationRequestModifyingSections)
                    .HasForeignKey(d => d.ModifyingSectionId)
                    .HasConstraintName("FK__LawModifi__Modif__45F365D3");

                entity.HasOne(d => d.ModifyingStatue)
                    .WithMany(p => p.LawModificationRequestModifyingStatues)
                    .HasForeignKey(d => d.ModifyingStatueId)
                    .HasConstraintName("FK__LawModifi__Modif__440B1D61");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.LawModificationRequestScribes)
                    .HasForeignKey(d => d.ScribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LawModifi__Scrib__49C3F6B7");
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
                    .HasConstraintName("FK__Paragraph__Secti__3D5E1FD2");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.QuestionCategory)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuestionCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Question__Questi__6D0D32F4");

                entity.HasOne(d => d.TestCategory)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.TestCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Question__TestCa__6C190EBB");
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
                    .HasConstraintName("FK__QuestionC__TestC__693CA210");
            });

            modelBuilder.Entity<QuestionModificationRequest>(entity =>
            {
                entity.HasKey(e => e.ModifyingQuestionId)
                    .HasName("PK__Question__94B14AC77380D411");

                entity.ToTable("QuestionModificationRequest");

                entity.Property(e => e.ModifyingQuestionId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.QuestionModificationRequestAdmins)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionM__Admin__75A278F5");

                entity.HasOne(d => d.ModifiedQuestion)
                    .WithMany(p => p.QuestionModificationRequestModifiedQuestions)
                    .HasForeignKey(d => d.ModifiedQuestionId)
                    .HasConstraintName("FK__QuestionM__Modif__73BA3083");

                entity.HasOne(d => d.ModifyingQuestion)
                    .WithOne(p => p.QuestionModificationRequestModifyingQuestion)
                    .HasForeignKey<QuestionModificationRequest>(d => d.ModifyingQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionM__Modif__72C60C4A");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.QuestionModificationRequestScribes)
                    .HasForeignKey(d => d.ScribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionM__Scrib__74AE54BC");
            });

            modelBuilder.Entity<Reference>(entity =>
            {
                entity.HasKey(e => new { e.ParagraphId, e.ReferenceParagraphId })
                    .HasName("PK__Referenc__44419EC04EFBF4BD");

                entity.ToTable("Reference");

                entity.HasOne(d => d.Paragraph)
                    .WithMany(p => p.ReferenceParagraphs)
                    .HasForeignKey(d => d.ParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reference__Parag__403A8C7D");

                entity.HasOne(d => d.ReferenceParagraph)
                    .WithMany(p => p.ReferenceReferenceParagraphs)
                    .HasForeignKey(d => d.ReferenceParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reference__Refer__412EB0B6");
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
                    .HasConstraintName("FK__Section__StatueI__3A81B327");

                entity.HasOne(d => d.VehicleCategory)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.VehicleCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Section__Vehicle__398D8EEE");
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
                    .HasConstraintName("FK__Sign__SignCatego__5535A963");
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

                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.Property(e => e.ModifiedGpssignId).HasColumnName("ModifiedGPSSignId");

                entity.Property(e => e.ModifyingGpssignId).HasColumnName("ModifyingGPSSignId");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.SignModificationRequestAdmins)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK__SignModif__Admin__6477ECF3");

                entity.HasOne(d => d.ModifiedGpssign)
                    .WithMany(p => p.SignModificationRequestModifiedGpssigns)
                    .HasForeignKey(d => d.ModifiedGpssignId)
                    .HasConstraintName("FK__SignModif__Modif__619B8048");

                entity.HasOne(d => d.ModifiedSign)
                    .WithMany(p => p.SignModificationRequestModifiedSigns)
                    .HasForeignKey(d => d.ModifiedSignId)
                    .HasConstraintName("FK__SignModif__Modif__5FB337D6");

                entity.HasOne(d => d.ModifyingGpssign)
                    .WithMany(p => p.SignModificationRequestModifyingGpssigns)
                    .HasForeignKey(d => d.ModifyingGpssignId)
                    .HasConstraintName("FK__SignModif__Modif__60A75C0F");

                entity.HasOne(d => d.ModifyingSign)
                    .WithMany(p => p.SignModificationRequestModifyingSigns)
                    .HasForeignKey(d => d.ModifyingSignId)
                    .HasConstraintName("FK__SignModif__Modif__5EBF139D");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.SignModificationRequestScribes)
                    .HasForeignKey(d => d.ScribeId)
                    .HasConstraintName("FK__SignModif__Scrib__6383C8BA");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SignModificationRequestUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__SignModif__UserI__628FA481");
            });

            modelBuilder.Entity<SignParagraph>(entity =>
            {
                entity.ToTable("SignParagraph");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Paragraph)
                    .WithMany(p => p.SignParagraphs)
                    .HasForeignKey(d => d.ParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignParag__Parag__5BE2A6F2");

                entity.HasOne(d => d.Sign)
                    .WithMany(p => p.SignParagraphs)
                    .HasForeignKey(d => d.SignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignParag__SignI__5AEE82B9");
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
                    .HasConstraintName("FK__Statue__ColumnId__36B12243");
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
                    .HasConstraintName("FK__TestResul__TestC__7C4F7684");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__UserI__7B5B524B");
            });

            modelBuilder.Entity<TestResultDetail>(entity =>
            {
                entity.ToTable("TestResultDetail");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.TestResultDetails)
                    .HasForeignKey(d => d.AnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__Answe__01142BA1");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TestResultDetails)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__Quest__00200768");

                entity.HasOne(d => d.TestResult)
                    .WithMany(p => p.TestResultDetails)
                    .HasForeignKey(d => d.TestResultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__TestR__7F2BE32F");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Gmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserModificationRequest>(entity =>
            {
                entity.HasKey(e => new { e.ModifyingUserId, e.ModifiedUserId })
                    .HasName("PK__UserModi__C95E8661C6491A6D");

                entity.ToTable("UserModificationRequest");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ArbitratingAdmin)
                    .WithMany(p => p.UserModificationRequestArbitratingAdmins)
                    .HasForeignKey(d => d.ArbitratingAdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserModif__Arbit__31EC6D26");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.UserModificationRequestModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserModif__Modif__300424B4");

                entity.HasOne(d => d.ModifyingUser)
                    .WithMany(p => p.UserModificationRequestModifyingUsers)
                    .HasForeignKey(d => d.ModifyingUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserModif__Modif__2F10007B");

                entity.HasOne(d => d.PromotingAdmin)
                    .WithMany(p => p.UserModificationRequestPromotingAdmins)
                    .HasForeignKey(d => d.PromotingAdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserModif__Promo__30F848ED");
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
