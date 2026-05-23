using Microsoft.EntityFrameworkCore;
using PetFamily.Domain.Entities;

namespace PetFamily.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tutor> Tutores { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Consulta> Consultas { get; set; }
    public DbSet<Lembrete> Lembretes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.ToTable("TUTORES");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).HasColumnName("ID").UseIdentityColumn();
            entity.Property(t => t.Nome).HasColumnName("NOME").HasMaxLength(150).IsRequired();
            entity.Property(t => t.Email).HasColumnName("EMAIL").HasMaxLength(200).IsRequired();
            entity.Property(t => t.Telefone).HasColumnName("TELEFONE").HasMaxLength(20).IsRequired();

            entity.HasIndex(t => t.Email)
                  .IsUnique()
                  .HasDatabaseName("IX_TUTORES_EMAIL");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.ToTable("PETS");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("ID").UseIdentityColumn();
            entity.Property(p => p.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();
            entity.Property(p => p.Especie).HasColumnName("ESPECIE").HasMaxLength(50).IsRequired();
            entity.Property(p => p.Raca).HasColumnName("RACA").HasMaxLength(100).IsRequired();
            entity.Property(p => p.DataNascimento).HasColumnName("DATA_NASCIMENTO").IsRequired();
            entity.Property(p => p.TutorId).HasColumnName("TUTOR_ID").IsRequired();

            entity.HasOne(p => p.Tutor)
                  .WithMany(t => t.Pets)
                  .HasForeignKey(p => p.TutorId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(p => p.TutorId)
                  .HasDatabaseName("IX_PETS_TUTOR_ID");
            entity.HasIndex(p => p.Especie)
                  .HasDatabaseName("IX_PETS_ESPECIE");
        });

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.ToTable("CONSULTAS");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("ID").UseIdentityColumn();
            entity.Property(c => c.DataConsulta).HasColumnName("DATA_CONSULTA").IsRequired();
            entity.Property(c => c.Observacoes).HasColumnName("OBSERVACOES").HasMaxLength(1000);
            entity.Property(c => c.PetId).HasColumnName("PET_ID").IsRequired();

            entity.HasOne(c => c.Pet)
                  .WithMany(p => p.Consultas)
                  .HasForeignKey(c => c.PetId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(c => c.PetId)
                  .HasDatabaseName("IX_CONSULTAS_PET_ID");
            entity.HasIndex(c => c.DataConsulta)
                  .HasDatabaseName("IX_CONSULTAS_DATA");
        });

        modelBuilder.Entity<Lembrete>(entity =>
        {
            entity.ToTable("LEMBRETES");
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Id).HasColumnName("ID").UseIdentityColumn();
            entity.Property(l => l.Titulo).HasColumnName("TITULO").HasMaxLength(200).IsRequired();
            entity.Property(l => l.Descricao).HasColumnName("DESCRICAO").HasMaxLength(1000);
            entity.Property(l => l.DataLembrete).HasColumnName("DATA_LEMBRETE").IsRequired();
            entity.Property(l => l.PetId).HasColumnName("PET_ID").IsRequired();

            entity.HasOne(l => l.Pet)
                  .WithMany(p => p.Lembretes)
                  .HasForeignKey(l => l.PetId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(l => l.PetId)
                  .HasDatabaseName("IX_LEMBRETES_PET_ID");
            entity.HasIndex(l => l.DataLembrete)
                  .HasDatabaseName("IX_LEMBRETES_DATA");
        });
    }
}
