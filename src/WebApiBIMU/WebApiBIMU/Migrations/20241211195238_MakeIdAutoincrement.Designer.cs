﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApiBIMU.Data;

#nullable disable

namespace WebApiBIMU.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241211195238_MakeIdAutoincrement")]
    partial class MakeIdAutoincrement
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebApiBIMU.Models.AlunoMateria", b =>
                {
                    b.Property<int?>("Id_Aluno")
                        .HasColumnType("integer");

                    b.Property<int>("Id_Materia")
                        .HasColumnType("integer");

                    b.HasKey("Id_Aluno", "Id_Materia");

                    b.HasIndex("Id_Materia");

                    b.ToTable("AlunoMaterias", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.AreaAcesso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Area")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AreaAcessos", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.Aula", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Data")
                        .HasColumnType("timestamp with time zone");

                    b.Property<TimeSpan>("Hora")
                        .HasColumnType("interval");

                    b.Property<int>("Id_Materia")
                        .HasColumnType("integer");

                    b.Property<int>("Id_Professor")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id_Materia");

                    b.HasIndex("Id_Professor");

                    b.ToTable("Aulas", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.DataMateria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<TimeSpan>("Horario")
                        .HasColumnType("interval");

                    b.Property<int>("Id_DiaSemana")
                        .HasColumnType("integer");

                    b.Property<int>("Id_Materia")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id_DiaSemana");

                    b.HasIndex("Id_Materia");

                    b.ToTable("DataMaterias", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.DiaSemana", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Domingo")
                        .HasColumnType("boolean");

                    b.Property<bool>("Quarta")
                        .HasColumnType("boolean");

                    b.Property<bool>("Quinta")
                        .HasColumnType("boolean");

                    b.Property<bool>("Sabado")
                        .HasColumnType("boolean");

                    b.Property<bool>("Segunda")
                        .HasColumnType("boolean");

                    b.Property<bool>("Sexta")
                        .HasColumnType("boolean");

                    b.Property<bool>("Terca")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("DiaSemanas", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.Eventos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Data")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeSpan>("Hora")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.ToTable("Eventos", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.FreqAula", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("EstaPresente")
                        .HasColumnType("boolean");

                    b.Property<int>("Id_Aluno")
                        .HasColumnType("integer");

                    b.Property<int>("Id_Aula")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id_Aluno");

                    b.HasIndex("Id_Aula");

                    b.ToTable("FreqAulas", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.HistoricoAcesso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Data")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Entrada_Saida")
                        .HasColumnType("boolean");

                    b.Property<TimeSpan>("Horario")
                        .HasColumnType("interval");

                    b.Property<int>("Id_Area")
                        .HasColumnType("integer");

                    b.Property<int>("Id_Pessoa")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id_Area");

                    b.HasIndex("Id_Pessoa");

                    b.ToTable("HistoricoAcessos", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.Materia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Materias", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.Pessoas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("CEP")
                        .HasColumnType("integer");

                    b.Property<int?>("CPF")
                        .HasColumnType("integer");

                    b.Property<int?>("Celular")
                        .HasColumnType("integer");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Data_Nascimento")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Genero")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Id_Tipo_Pessoa")
                        .HasColumnType("integer");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("RG")
                        .HasColumnType("integer");

                    b.Property<string>("Rua")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id_Tipo_Pessoa");

                    b.ToTable("Pessoas", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.ResponsavelAluno", b =>
                {
                    b.Property<int>("Id_Aluno")
                        .HasColumnType("integer");

                    b.Property<int>("Id_Responsavel")
                        .HasColumnType("integer");

                    b.HasKey("Id_Aluno", "Id_Responsavel");

                    b.HasIndex("Id_Responsavel");

                    b.ToTable("ResponsavelAlunos", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.TipoPessoa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Tipo_Pessoa_Desc")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TipoPessoas", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.Usuario", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<bool>("Ativado")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("Id_Pessoa")
                        .HasColumnType("integer");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("PskHash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PskSalt")
                        .HasColumnType("bytea");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Id_Pessoa");

                    b.ToTable("Usuarios", (string)null);
                });

            modelBuilder.Entity("WebApiBIMU.Models.AlunoMateria", b =>
                {
                    b.HasOne("WebApiBIMU.Models.Pessoas", "Aluno")
                        .WithMany("AlunoMaterias")
                        .HasForeignKey("Id_Aluno")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApiBIMU.Models.Materia", "Materia")
                        .WithMany("AlunoMaterias")
                        .HasForeignKey("Id_Materia")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aluno");

                    b.Navigation("Materia");
                });

            modelBuilder.Entity("WebApiBIMU.Models.Aula", b =>
                {
                    b.HasOne("WebApiBIMU.Models.Materia", "Materia")
                        .WithMany("Aula")
                        .HasForeignKey("Id_Materia")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApiBIMU.Models.Pessoas", "Professor")
                        .WithMany("Aula")
                        .HasForeignKey("Id_Professor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Materia");

                    b.Navigation("Professor");
                });

            modelBuilder.Entity("WebApiBIMU.Models.DataMateria", b =>
                {
                    b.HasOne("WebApiBIMU.Models.DiaSemana", "DiaSemana")
                        .WithMany("DataMateria")
                        .HasForeignKey("Id_DiaSemana")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApiBIMU.Models.Materia", "Materia")
                        .WithMany("DataMateria")
                        .HasForeignKey("Id_Materia")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiaSemana");

                    b.Navigation("Materia");
                });

            modelBuilder.Entity("WebApiBIMU.Models.FreqAula", b =>
                {
                    b.HasOne("WebApiBIMU.Models.Pessoas", "Aluno")
                        .WithMany("FreqAula")
                        .HasForeignKey("Id_Aluno")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApiBIMU.Models.Aula", "Aula")
                        .WithMany("FreqAula")
                        .HasForeignKey("Id_Aula")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aluno");

                    b.Navigation("Aula");
                });

            modelBuilder.Entity("WebApiBIMU.Models.HistoricoAcesso", b =>
                {
                    b.HasOne("WebApiBIMU.Models.AreaAcesso", "Area")
                        .WithMany("HistoricoAcesso")
                        .HasForeignKey("Id_Area")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApiBIMU.Models.Pessoas", "Pessoa")
                        .WithMany("HistoricoAcesso")
                        .HasForeignKey("Id_Pessoa")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Area");

                    b.Navigation("Pessoa");
                });

            modelBuilder.Entity("WebApiBIMU.Models.Pessoas", b =>
                {
                    b.HasOne("WebApiBIMU.Models.TipoPessoa", "TipoPessoa")
                        .WithMany("Pessoas")
                        .HasForeignKey("Id_Tipo_Pessoa")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("TipoPessoa");
                });

            modelBuilder.Entity("WebApiBIMU.Models.ResponsavelAluno", b =>
                {
                    b.HasOne("WebApiBIMU.Models.Pessoas", "Aluno")
                        .WithMany("AlunoDoResponsavel")
                        .HasForeignKey("Id_Aluno")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApiBIMU.Models.Pessoas", "Responsavel")
                        .WithMany("ResponsavelDoAluno")
                        .HasForeignKey("Id_Responsavel")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aluno");

                    b.Navigation("Responsavel");
                });

            modelBuilder.Entity("WebApiBIMU.Models.Usuario", b =>
                {
                    b.HasOne("WebApiBIMU.Models.Pessoas", "Pessoa")
                        .WithMany("Usuario")
                        .HasForeignKey("Id_Pessoa");

                    b.Navigation("Pessoa");
                });

            modelBuilder.Entity("WebApiBIMU.Models.AreaAcesso", b =>
                {
                    b.Navigation("HistoricoAcesso");
                });

            modelBuilder.Entity("WebApiBIMU.Models.Aula", b =>
                {
                    b.Navigation("FreqAula");
                });

            modelBuilder.Entity("WebApiBIMU.Models.DiaSemana", b =>
                {
                    b.Navigation("DataMateria");
                });

            modelBuilder.Entity("WebApiBIMU.Models.Materia", b =>
                {
                    b.Navigation("AlunoMaterias");

                    b.Navigation("Aula");

                    b.Navigation("DataMateria");
                });

            modelBuilder.Entity("WebApiBIMU.Models.Pessoas", b =>
                {
                    b.Navigation("AlunoDoResponsavel");

                    b.Navigation("AlunoMaterias");

                    b.Navigation("Aula");

                    b.Navigation("FreqAula");

                    b.Navigation("HistoricoAcesso");

                    b.Navigation("ResponsavelDoAluno");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("WebApiBIMU.Models.TipoPessoa", b =>
                {
                    b.Navigation("Pessoas");
                });
#pragma warning restore 612, 618
        }
    }
}
