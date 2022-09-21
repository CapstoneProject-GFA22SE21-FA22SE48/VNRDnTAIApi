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
        public virtual DbSet<Decree> Decrees { get; set; }
        public virtual DbSet<Gpssign> Gpssigns { get; set; }
        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<Paragraph> Paragraphs { get; set; }
        public virtual DbSet<ParagraphModificationRequest> ParagraphModificationRequests { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Reference> References { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Sign> Signs { get; set; }
        public virtual DbSet<SignCategory> SignCategories { get; set; }
        public virtual DbSet<SignModificationRequest> SignModificationRequests { get; set; }
        public virtual DbSet<SignParagraph> SignParagraphs { get; set; }
        public virtual DbSet<Statue> Statues { get; set; }
        public virtual DbSet<TestCategory> TestCategories { get; set; }
        public virtual DbSet<User> Users { get; set; }
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
                    .HasConstraintName("FK__Answer__Question__5441852A");
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
                    .HasConstraintName("FK__Column__DecreeId__286302EC");
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
                    .HasConstraintName("FK__GPSSign__SignId__4CA06362");
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
                    .HasConstraintName("FK__Keyword__Paragra__3E52440B");
            });

            modelBuilder.Entity<Paragraph>(entity =>
            {
                entity.ToTable("Paragraph");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.Paragraphs)
                    .HasForeignKey(d => d.SectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Paragraph__Secti__33D4B598");
            });

            modelBuilder.Entity<ParagraphModificationRequest>(entity =>
            {
                entity.HasKey(e => new { e.ParagraphId, e.UserId })
                    .HasName("PK__Paragrap__FFAC511EAD6E68D8");

                entity.ToTable("ParagraphModificationRequest");

                entity.HasOne(d => d.Paragraph)
                    .WithMany(p => p.ParagraphModificationRequests)
                    .HasForeignKey(d => d.ParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Paragraph__Parag__3A81B327");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ParagraphModificationRequests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Paragraph__UserI__3B75D760");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Question1)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasColumnName("Question");

                entity.HasOne(d => d.TestCategory)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.TestCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Question__TestCa__5165187F");
            });

            modelBuilder.Entity<Reference>(entity =>
            {
                entity.HasKey(e => new { e.ParagraphId, e.ReferenceParagraphId })
                    .HasName("PK__Referenc__44419EC004FF4975");

                entity.ToTable("Reference");

                entity.HasOne(d => d.Paragraph)
                    .WithMany(p => p.ReferenceParagraphs)
                    .HasForeignKey(d => d.ParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reference__Parag__36B12243");

                entity.HasOne(d => d.ReferenceParagraph)
                    .WithMany(p => p.ReferenceReferenceParagraphs)
                    .HasForeignKey(d => d.ReferenceParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reference__Refer__37A5467C");
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
                    .HasConstraintName("FK__Section__StatueI__30F848ED");
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
                    .HasConstraintName("FK__Sign__SignCatego__4316F928");
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

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Sign)
                    .WithMany()
                    .HasForeignKey(d => d.SignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignModif__SignI__48CFD27E");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignModif__UserI__49C3F6B7");
            });

            modelBuilder.Entity<SignParagraph>(entity =>
            {
                entity.ToTable("SignParagraph");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Paragraph)
                    .WithMany(p => p.SignParagraphs)
                    .HasForeignKey(d => d.ParagraphId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignParag__Parag__46E78A0C");

                entity.HasOne(d => d.Sign)
                    .WithMany(p => p.SignParagraphs)
                    .HasForeignKey(d => d.SignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SignParag__SignI__45F365D3");
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
                    .HasConstraintName("FK__Statue__ColumnId__2E1BDC42");

                entity.HasOne(d => d.VehicleCategory)
                    .WithMany(p => p.Statues)
                    .HasForeignKey(d => d.VehicleCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Statue__VehicleC__2D27B809");
            });

            modelBuilder.Entity<TestCategory>(entity =>
            {
                entity.ToTable("TestCategory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255);
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
