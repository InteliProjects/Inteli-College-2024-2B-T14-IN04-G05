using Microsoft.EntityFrameworkCore;
using WebApiBIMU.Models;

namespace WebApiBIMU.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // DbSets
        public DbSet<AlunoMateria> AlunoMaterias { get; set; }
        public DbSet<Aula> Aulas { get; set; }
        public DbSet<DataMateria> DataMaterias { get; set; }
        public DbSet<DiaSemana> DiasSemana { get; set; }
        public DbSet<FreqAula> FreqAulas { get; set; }
        public DbSet<HistoricoAcesso> HistoricoAcessos { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Pessoas> Pessoas { get; set; }
        public DbSet<ResponsavelAluno> ResponsaveisAluno { get; set; }
        public DbSet<TipoPessoa> TiposPessoa { get; set; }
        public DbSet<AreaAcesso> AreasAcesso { get; set; }
        public DbSet<Eventos> Eventos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Replace with your connection string
                var connectionString = "Host=dpg-cstl5om8ii6s73fk3hqg-a.oregon-postgres.render.com;Port=5432;Database=bimu;Username=bimu_user;Password=1FcHIOYJCxMyuxSJwYAdDDNi9zL16n40;";
                //optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HistoricoAcesso>()
                .Property(h => h.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Pessoas>()
             .HasMany(p => p.Materias)
             .WithMany(e => e.Alunos)
             .UsingEntity<AlunoMateria>(
                r => r.HasOne<Materia>(e => e.Materia).WithMany(e => e.AlunoMaterias).HasForeignKey(e => e.Id_Materia),
                l => l.HasOne<Pessoas>(e => e.Aluno).WithMany(e => e.AlunoMaterias).HasForeignKey(e => e.Id_Aluno));

            modelBuilder.Entity<Pessoas>()
            .HasMany(p => p.Responsavel)
            .WithMany(e => e.Alunos)
            .UsingEntity<ResponsavelAluno>(
               r => r.HasOne<Pessoas>(e => e.Responsavel).WithMany(e => e.ResponsavelDoAluno).HasForeignKey(e => e.Id_Responsavel),
               l => l.HasOne<Pessoas>(e => e.Aluno).WithMany(e => e.AlunoDoResponsavel).HasForeignKey(e => e.Id_Aluno));

            // Configuração de relacionamento em Aula
            modelBuilder.Entity<Aula>()
                .HasOne(a => a.Materia)
                .WithMany(m => m.Aula)
                .HasForeignKey(a => a.Id_Materia);

            modelBuilder.Entity<Aula>()
                .HasOne(a => a.Professor)
                .WithMany(p => p.Aula)
                .HasForeignKey(a => a.Id_Professor);

            // Configuração de relacionamento em FreqAula
            modelBuilder.Entity<FreqAula>()
                .HasOne(fa => fa.Aula)
                .WithMany(a => a.FreqAula)
                .HasForeignKey(fa => fa.Id_Aula);

            modelBuilder.Entity<FreqAula>()
                .HasOne(fa => fa.Aluno)
                .WithMany(a => a.FreqAula)
                .HasForeignKey(fa => fa.Id_Aluno);

            // Configuração de relacionamento em DataMateria
            modelBuilder.Entity<DataMateria>()
                .HasOne(dm => dm.Materia)
                .WithMany(m => m.DataMateria)
                .HasForeignKey(dm => dm.Id_Materia);

            modelBuilder.Entity<DataMateria>()
                .HasOne(dm => dm.DiaSemana)
                .WithMany(ds => ds.DataMateria)
                .HasForeignKey(dm => dm.Id_DiaSemana);

            // Configuração de relacionamento em HistoricoAcesso
            modelBuilder.Entity<HistoricoAcesso>()
                .HasOne(ha => ha.Pessoa)
                .WithMany(p => p.HistoricoAcesso)
                .HasForeignKey(ha => ha.Id_Pessoa);

            modelBuilder.Entity<HistoricoAcesso>()
                .HasOne(ha => ha.Area)
                .WithMany(aa => aa.HistoricoAcesso)
                .HasForeignKey(ha => ha.Id_Area);

            // Configuração de relacionamento em Pessoas
            modelBuilder.Entity<Pessoas>()
                .HasOne(p => p.TipoPessoa)
                .WithMany(tp => tp.Pessoas)
                .HasForeignKey(p => p.Id_Tipo_Pessoa)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração de relacionamento em Usuario
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Pessoa)
                .WithMany(p => p.Usuario)
                .HasForeignKey(u => u.Id_Pessoa);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd(); // Configura autoincrement
            });
            // Nome das tabelas no banco de dados
            modelBuilder.Entity<AlunoMateria>().ToTable("AlunoMaterias");
            modelBuilder.Entity<AreaAcesso>().ToTable("AreaAcessos");
            modelBuilder.Entity<Aula>().ToTable("Aulas");
            modelBuilder.Entity<DataMateria>().ToTable("DataMaterias");
            modelBuilder.Entity<DiaSemana>().ToTable("DiaSemanas");
            modelBuilder.Entity<Eventos>().ToTable("Eventos");
            modelBuilder.Entity<FreqAula>().ToTable("FreqAulas");
            modelBuilder.Entity<HistoricoAcesso>().ToTable("HistoricoAcessos");
            modelBuilder.Entity<Materia>().ToTable("Materias");
            modelBuilder.Entity<Pessoas>().ToTable("Pessoas");
            modelBuilder.Entity<ResponsavelAluno>().ToTable("ResponsavelAlunos");
            modelBuilder.Entity<TipoPessoa>().ToTable("TipoPessoas");
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");

            //modelBuilder.Entity<AlunoMateria>()
            //    .HasOne(am => am.Aluno)
            //    .WithMany(p => p.AlunoMateria)
            //    .HasForeignKey(am => am.Id_Aluno);

            //modelBuilder.Entity<AlunoMateria>()
            //    .HasOne(am => am.Materia)
            //    .WithMany(m => m.AlunoMateria)
            //    .HasForeignKey(am => am.Id_Materia);
        }
    }
}