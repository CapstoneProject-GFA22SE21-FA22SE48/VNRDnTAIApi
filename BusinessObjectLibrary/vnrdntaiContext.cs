using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
        public virtual DbSet<Column> Columns { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Decree> Decrees { get; set; }
        public virtual DbSet<Gpssign> Gpssigns { get; set; }
        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<Paragraph> Paragraphs { get; set; }
        public virtual DbSet<ParagraphModificationRequest> ParagraphModificationRequests { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
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
                    .HasConstraintName("FK__Answer__Question__5EBF139D");
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
                    .HasConstraintName("FK__Column__DecreeId__2E1BDC42");
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
                    .HasConstraintName("FK__Comment__UserId__66603565");
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
                    .HasConstraintName("FK__GPSSign__SignId__571DF1D5");
            });

            modelBuilder.Entity<Keyword>(entity =>
            {
                entity.ToTable("Keyword");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Paragraph)
                    .WithMany(p => p.Keywords)
                    .HasForeignKey(d => d.ParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Keyword__Paragra__45F365D3");
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
                    .HasConstraintName("FK__Paragraph__Secti__398D8EEE");
            });

            modelBuilder.Entity<ParagraphModificationRequest>(entity =>
            {
                entity.HasKey(e => new { e.ModifiedParagraphId, e.ModifyingParagraphId })
                    .HasName("PK__Paragrap__D8B4943AD31F7687");

                entity.ToTable("ParagraphModificationRequest");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.ParagraphModificationRequestAdmins)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Paragraph__Admin__4316F928");

                entity.HasOne(d => d.ModifiedParagraph)
                    .WithMany(p => p.ParagraphModificationRequestModifiedParagraphs)
                    .HasForeignKey(d => d.ModifiedParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Paragraph__Modif__403A8C7D");

                entity.HasOne(d => d.ModifyingParagraph)
                    .WithMany(p => p.ParagraphModificationRequestModifyingParagraphs)
                    .HasForeignKey(d => d.ModifyingParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Paragraph__Modif__412EB0B6");

                entity.HasOne(d => d.Scribe)
                    .WithMany(p => p.ParagraphModificationRequestScribes)
                    .HasForeignKey(d => d.ScribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Paragraph__Scrib__4222D4EF");
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

                entity.HasOne(d => d.TestCategory)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.TestCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Question__TestCa__5BE2A6F2");
            });

            modelBuilder.Entity<QuestionModificationRequest>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("QuestionModificationRequest");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Admin)
                    .WithMany()
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionM__Admin__6383C8BA");

                entity.HasOne(d => d.ModifiedQuestion)
                    .WithMany()
                    .HasForeignKey(d => d.ModifiedQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionM__Modif__60A75C0F");

                entity.HasOne(d => d.ModifyingQuestion)
                    .WithMany()
                    .HasForeignKey(d => d.ModifyingQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionM__Modif__619B8048");

                entity.HasOne(d => d.Scribe)
                    .WithMany()
                    .HasForeignKey(d => d.ScribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionM__Scrib__628FA481");
            });

            modelBuilder.Entity<Reference>(entity =>
            {
                entity.HasKey(e => new { e.ParagraphId, e.ReferenceParagraphId })
                    .HasName("PK__Referenc__44419EC0F62F3D6E");

                entity.ToTable("Reference");

                entity.HasOne(d => d.Paragraph)
                    .WithMany(p => p.ReferenceParagraphs)
                    .HasForeignKey(d => d.ParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reference__Parag__3C69FB99");

                entity.HasOne(d => d.ReferenceParagraph)
                    .WithMany(p => p.ReferenceReferenceParagraphs)
                    .HasForeignKey(d => d.ReferenceParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reference__Refer__3D5E1FD2");
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
                    .HasConstraintName("FK__Section__StatueI__36B12243");
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
                    .HasMaxLength(10);

                entity.HasOne(d => d.SignCategory)
                    .WithMany(p => p.Signs)
                    .HasForeignKey(d => d.SignCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sign__SignCatego__4AB81AF0");
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
                entity.HasNoKey();

                entity.ToTable("SignModificationRequest");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Admin)
                    .WithMany()
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK__SignModif__Admin__5441852A");

                entity.HasOne(d => d.ModifiedSign)
                    .WithMany()
                    .HasForeignKey(d => d.ModifiedSignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignModif__Modif__5070F446");

                entity.HasOne(d => d.ModifyingSign)
                    .WithMany()
                    .HasForeignKey(d => d.ModifyingSignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignModif__Modif__5165187F");

                entity.HasOne(d => d.Scribe)
                    .WithMany()
                    .HasForeignKey(d => d.ScribeId)
                    .HasConstraintName("FK__SignModif__Scrib__534D60F1");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignModif__UserI__52593CB8");
            });

            modelBuilder.Entity<SignParagraph>(entity =>
            {
                entity.ToTable("SignParagraph");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Paragraph)
                    .WithMany(p => p.SignParagraphs)
                    .HasForeignKey(d => d.ParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignParag__Parag__4E88ABD4");

                entity.HasOne(d => d.Sign)
                    .WithMany(p => p.SignParagraphs)
                    .HasForeignKey(d => d.SignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignParag__SignI__4D94879B");
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
                    .HasConstraintName("FK__Statue__ColumnId__33D4B598");

                entity.HasOne(d => d.VehicleCategory)
                    .WithMany(p => p.Statues)
                    .HasForeignKey(d => d.VehicleCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Statue__VehicleC__32E0915F");
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
                    .HasConstraintName("FK__TestResul__TestC__6A30C649");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__UserI__693CA210");
            });

            modelBuilder.Entity<TestResultDetail>(entity =>
            {
                entity.ToTable("TestResultDetail");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.TestResultDetails)
                    .HasForeignKey(d => d.AnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__Answe__6EF57B66");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TestResultDetails)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__Quest__6E01572D");

                entity.HasOne(d => d.TestResult)
                    .WithMany(p => p.TestResultDetails)
                    .HasForeignKey(d => d.TestResultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestResul__TestR__6D0D32F4");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<UserModificationRequest>(entity =>
            {
                entity.HasKey(e => new { e.ModifiedUserId, e.ModifyingUserId })
                    .HasName("PK__UserModi__3C975BE68A055790");

                entity.ToTable("UserModificationRequest");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ArbitratingAdmin)
                    .WithMany(p => p.UserModificationRequestArbitratingAdmins)
                    .HasForeignKey(d => d.ArbitratingAdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserModif__Arbit__29572725");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.UserModificationRequestModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserModif__Modif__267ABA7A");

                entity.HasOne(d => d.ModifyingUser)
                    .WithMany(p => p.UserModificationRequestModifyingUsers)
                    .HasForeignKey(d => d.ModifyingUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserModif__Modif__276EDEB3");

                entity.HasOne(d => d.PromotingAdmin)
                    .WithMany(p => p.UserModificationRequestPromotingAdmins)
                    .HasForeignKey(d => d.PromotingAdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserModif__Promo__286302EC");
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
