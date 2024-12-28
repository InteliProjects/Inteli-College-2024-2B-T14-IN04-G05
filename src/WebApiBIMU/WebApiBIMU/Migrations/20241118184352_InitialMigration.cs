using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApiBIMU.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreaAcessos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Area = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaAcessos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiaSemanas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Domingo = table.Column<bool>(type: "boolean", nullable: false),
                    Segunda = table.Column<bool>(type: "boolean", nullable: false),
                    Terca = table.Column<bool>(type: "boolean", nullable: false),
                    Quarta = table.Column<bool>(type: "boolean", nullable: false),
                    Quinta = table.Column<bool>(type: "boolean", nullable: false),
                    Sexta = table.Column<bool>(type: "boolean", nullable: false),
                    Sabado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaSemanas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoPessoas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tipo_Pessoa_Desc = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoPessoas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataMaterias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Materia = table.Column<int>(type: "integer", nullable: false),
                    Id_DiaSemana = table.Column<int>(type: "integer", nullable: false),
                    Horario = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataMaterias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataMaterias_DiaSemanas_Id_DiaSemana",
                        column: x => x.Id_DiaSemana,
                        principalTable: "DiaSemanas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataMaterias_Materias_Id_Materia",
                        column: x => x.Id_Materia,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Tipo_Pessoa = table.Column<int>(type: "integer", nullable: true),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Data_Nascimento = table.Column<int>(type: "integer", nullable: true),
                    RG = table.Column<int>(type: "integer", nullable: true),
                    CPF = table.Column<int>(type: "integer", nullable: true),
                    Genero = table.Column<string>(type: "text", nullable: false),
                    Rua = table.Column<string>(type: "text", nullable: false),
                    Bairro = table.Column<string>(type: "text", nullable: false),
                    CEP = table.Column<int>(type: "integer", nullable: true),
                    Cidade = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Celular = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pessoas_TipoPessoas_Id_Tipo_Pessoa",
                        column: x => x.Id_Tipo_Pessoa,
                        principalTable: "TipoPessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlunoMaterias",
                columns: table => new
                {
                    Id_Aluno = table.Column<int>(type: "integer", nullable: false),
                    Id_Materia = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlunoMaterias", x => new { x.Id_Aluno, x.Id_Materia });
                    table.ForeignKey(
                        name: "FK_AlunoMaterias_Materias_Id_Materia",
                        column: x => x.Id_Materia,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlunoMaterias_Pessoas_Id_Aluno",
                        column: x => x.Id_Aluno,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aulas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Materia = table.Column<int>(type: "integer", nullable: false),
                    Id_Professor = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aulas_Materias_Id_Materia",
                        column: x => x.Id_Materia,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aulas_Pessoas_Id_Professor",
                        column: x => x.Id_Professor,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoAcessos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Pessoa = table.Column<int>(type: "integer", nullable: false),
                    Id_Area = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Horario = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Entrada_Saida = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoAcessos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoAcessos_AreaAcessos_Id_Area",
                        column: x => x.Id_Area,
                        principalTable: "AreaAcessos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoricoAcessos_Pessoas_Id_Pessoa",
                        column: x => x.Id_Pessoa,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponsavelAlunos",
                columns: table => new
                {
                    Id_Aluno = table.Column<int>(type: "integer", nullable: false),
                    Id_Responsavel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsavelAlunos", x => new { x.Id_Aluno, x.Id_Responsavel });
                    table.ForeignKey(
                        name: "FK_ResponsavelAlunos_Pessoas_Id_Aluno",
                        column: x => x.Id_Aluno,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResponsavelAlunos_Pessoas_Id_Responsavel",
                        column: x => x.Id_Responsavel,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Pessoa = table.Column<int>(type: "integer", nullable: true),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    PskHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    PskSalt = table.Column<byte[]>(type: "bytea", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Pessoas_Id_Pessoa",
                        column: x => x.Id_Pessoa,
                        principalTable: "Pessoas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FreqAulas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Aula = table.Column<int>(type: "integer", nullable: false),
                    Id_Aluno = table.Column<int>(type: "integer", nullable: false),
                    EstaPresente = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreqAulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FreqAulas_Aulas_Id_Aula",
                        column: x => x.Id_Aula,
                        principalTable: "Aulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FreqAulas_Pessoas_Id_Aluno",
                        column: x => x.Id_Aluno,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlunoMaterias_Id_Materia",
                table: "AlunoMaterias",
                column: "Id_Materia");

            migrationBuilder.CreateIndex(
                name: "IX_Aulas_Id_Materia",
                table: "Aulas",
                column: "Id_Materia");

            migrationBuilder.CreateIndex(
                name: "IX_Aulas_Id_Professor",
                table: "Aulas",
                column: "Id_Professor");

            migrationBuilder.CreateIndex(
                name: "IX_DataMaterias_Id_DiaSemana",
                table: "DataMaterias",
                column: "Id_DiaSemana");

            migrationBuilder.CreateIndex(
                name: "IX_DataMaterias_Id_Materia",
                table: "DataMaterias",
                column: "Id_Materia");

            migrationBuilder.CreateIndex(
                name: "IX_FreqAulas_Id_Aluno",
                table: "FreqAulas",
                column: "Id_Aluno");

            migrationBuilder.CreateIndex(
                name: "IX_FreqAulas_Id_Aula",
                table: "FreqAulas",
                column: "Id_Aula");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoAcessos_Id_Area",
                table: "HistoricoAcessos",
                column: "Id_Area");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoAcessos_Id_Pessoa",
                table: "HistoricoAcessos",
                column: "Id_Pessoa");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_Id_Tipo_Pessoa",
                table: "Pessoas",
                column: "Id_Tipo_Pessoa");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsavelAlunos_Id_Responsavel",
                table: "ResponsavelAlunos",
                column: "Id_Responsavel");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Id_Pessoa",
                table: "Usuarios",
                column: "Id_Pessoa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlunoMaterias");

            migrationBuilder.DropTable(
                name: "DataMaterias");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "FreqAulas");

            migrationBuilder.DropTable(
                name: "HistoricoAcessos");

            migrationBuilder.DropTable(
                name: "ResponsavelAlunos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "DiaSemanas");

            migrationBuilder.DropTable(
                name: "Aulas");

            migrationBuilder.DropTable(
                name: "AreaAcessos");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "Pessoas");

            migrationBuilder.DropTable(
                name: "TipoPessoas");
        }
    }
}
