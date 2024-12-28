//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApiBIMU.DTOs;
using WebApiBIMU.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using WebApiBIMU.DTOs.Usuarios;

namespace WebApiBIMU.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RSA _privateKey;
        private readonly RSA _publicKey;

        public AuthService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this._mapper = mapper;
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
            this._configuration = configuration;

            // Gera um par de chaves RSA
            using (RSA rsa = RSA.Create())
            {
                _privateKey = rsa;
                var parameters = _privateKey.ExportParameters(false);
                _publicKey = rsa;
                _publicKey.ImportParameters(parameters);
            }
        }

        public async Task<RespostaDeServico<int>> Registrar(Usuario usuario, string password)
        {
            var response = new RespostaDeServico<int>();
            if (await UsuarioExiste(usuario.Nome, usuario.Id_Pessoa ?? 0))
            {
                response.Sucesso = false;
                response.Mensagem = "Usuário já existe.";
                return response;
            }

            _context.Entry(usuario).State = EntityState.Added;

            CriarPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            usuario.PskHash = passwordHash;
            usuario.PskSalt = passwordSalt;
            usuario.Ativado = true;
            usuario.CreatedAt = DateTime.SpecifyKind(DateTime.Parse(DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss")), DateTimeKind.Utc);
            usuario.UpdatedAt = DateTime.SpecifyKind(DateTime.Parse(DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss")), DateTimeKind.Utc);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            response.Dados = usuario.Id ?? 0;
            return response;
        }

        public async Task<RespostaDeServico<string>> Login(string username, string password)
        {
            var resposta = new RespostaDeServico<string>();
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Nome.ToLower().Equals(username.ToLower()));
            if (usuario is null)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = "User not found.";
            }
            else if (!VerificarPasswordHash(password, usuario.PskHash, usuario.PskSalt))
            {
                resposta.Sucesso = true;
                resposta.Mensagem = "Wrong password!";
            }
            else
            {
                resposta.Dados = CriarToken(usuario);
            }
            return resposta;
        }

        public async Task<bool> UsuarioExiste(string usuario, int funcId)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Nome.ToLower() == usuario.ToLower() || u.Id_Pessoa == funcId))
                return true;
            return false;
        }

        private void CriarPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computadoHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computadoHash.SequenceEqual(passwordHash);
            }
        }

        private UnicodeEncoding ByteConverter = new UnicodeEncoding();
        private RSACryptoServiceProvider RSAA = new RSACryptoServiceProvider();
        private byte[] textoPlano;
        private byte[] textoCifrado;
        private byte[] textoDecifrado;


        private string CriarToken(Usuario usuario)
        {
            //textoPlano = ByteConverter.GetBytes(usuario.Id.ToString());
            //textoCifrado = RSACifra(textoPlano, RSAA.ExportParameters(false), false);
            //var teste = ByteConverter.GetString(textoCifrado);

            //textoDecifrado = RSADecifra(textoCifrado, RSAA.ExportParameters(true), false);
            //var result = ByteConverter.GetString(textoDecifrado);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToBase64String(RSACifra(ByteConverter.GetBytes(usuario.Id.ToString()), RSAA.ExportParameters(false), false))!),
                new Claim(ClaimTypes.Name, Convert.ToBase64String(RSACifra(ByteConverter.GetBytes(usuario.Nome), RSAA.ExportParameters(false), false))!),
                new Claim(ClaimTypes.Role, Convert.ToBase64String(RSACifra(ByteConverter.GetBytes("Admin"), RSAA.ExportParameters(false), false))!)
             };

            var appSettings = _configuration.GetSection("AppSettings:Token").Value;
            if (appSettings is null)
                throw new Exception("AppSettings Token is null!!");

            //SymmetricSecurityKey chave = new SymmetricSecurityKey(System.Text.Encoding.UTF8
            //    .GetBytes(appSettings));
            
            SymmetricSecurityKey chave = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings));

            SigningCredentials creds = new SigningCredentials(chave, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescritor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescritor);
            return tokenHandler.WriteToken(token);
        }

        #region AppLogin       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomeusuario"></param>
        /// <param name="password"></param>
        /// <param name="PIN"></param>
        /// <returns></returns>
        //public async Task<RespostaDeServico<DetalhesDoUsuario>> AppLogin(string nomeusuario, string password, string PIN)
        //{
        //    var resposta = new RespostaDeServico<DetalhesDoUsuario>();
        //    try
        //    {
        //        var usuario = await _context.Usuario
        //            .Where(u => u.Id_Empresa.ToString() == PIN && u.Ativo == true)
        //            .FirstOrDefaultAsync(u => u.Usuario.ToLower().Equals(nomeusuario.ToLower()));
        //        if (usuario is null)
        //        {
        //            resposta.Sucesso = false;
        //            resposta.Mensagem = "Usuário não encontrado.";
        //        }
        //        else if (!VerificarPasswordHash(password, usuario.PskHash, usuario.PskSalt))
        //        {
        //            resposta.Sucesso = false;
        //            resposta.Mensagem = "Senha incorreta!";
        //        }
        //        else
        //        {
        //            DetalhesDoUsuario detalhes = new DetalhesDoUsuario
        //            {
        //                Token = CriarTokenApp(usuario),
        //                Id = usuario.Id,
        //                Nome = usuario.Usuario,
        //                IdEmpresa = usuario.Id_Empresa,
        //                Grupo = usuario.Grupo
        //            };
        //            resposta.Dados = detalhes;
        //            resposta.Mensagem = "Login com sucesso!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resposta.Sucesso = false;
        //        resposta.Mensagem = ex.Message;
        //    }
        //    return resposta;
        //}

        //public async Task<RespostaDeServico<List<GetUsuarioDto>>> GetEntregadores()
        //{
        //    var resposta = new RespostaDeServico<List<GetUsuarioDto>>();
        //    try
        //    {
        //        List<Usuario> entregadores = await _context.Usuario
        //            .Where(u => u.Id_Empresa == GetIdEmpresa() && u.Grupo == GrupoUsuario.Entregador &&
        //            u.Ativo == true).ToListAsync();

        //        resposta.Dados = _mapper.Map<List<Usuario>, List<GetUsuarioDto>>(entregadores);
        //    }
        //    catch (Exception ex)
        //    {
        //        resposta.Sucesso = false;
        //        resposta.Mensagem = ex.Message;
        //    }
        //    return resposta;
        //}



        #endregion

        public async Task<RespostaDeServico<GetUsuarioDto>> UpdateUsuario(UpdateUsuarioDto updateUsuario)
        {
            var resposta = new RespostaDeServico<GetUsuarioDto>();
            try
            {
                var usuario =
                    await _context.Usuarios
                        .FirstOrDefaultAsync(e => e.Id == updateUsuario.Id);

                //var usuario =
                //    await _context.Usuario
                //        .FirstOrDefaultAsync(e => e.Id == updateUsuario.Id &&
                //            e.Id_Empresa == e.Id_Empresa);

                if (usuario is null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Usuário não encontrado.";
                }
                else
                {
                    _mapper.Map(updateUsuario, usuario);
                    CriarPasswordHash(updateUsuario.Password!, out byte[] passwordHash, out byte[] passwordSalt);
                    usuario!.PskHash = passwordHash;
                    usuario.PskSalt = passwordSalt;
                    usuario!.UpdatedAt = DateTime.Today;
                    await _context.SaveChangesAsync();
                    resposta.Dados = _mapper.Map<GetUsuarioDto>(usuario);
                }
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = ex.Message;
            }
            return resposta;
        }


        #region Criptografia
        public string EncryptString(string text)
        {
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(text);

            byte[] encryptedBytes = _publicKey.Encrypt(bytesToEncrypt, RSAEncryptionPadding.OaepSHA256);

            string encryptedText = Convert.ToBase64String(encryptedBytes);

            return encryptedText;
        }

        public string DecryptString(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            byte[] decryptedBytes = _privateKey.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);

            string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedText;
        }

        static public byte[] RSACifra(byte[] byteCifrado, RSAParameters RSAInfo, bool isOAEP)
        {
            try
            {
                byte[] DadosCifrados;
                //Cria uma nova instância de RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Importa a informação da chave RSA. 
                    //Feito apenas para incluir a informação da chave pública
                    RSA.ImportParameters(RSAInfo);
                    //Cria o array de bytes e especifica o preenchimento OAEP
                    DadosCifrados = RSA.Encrypt(byteCifrado, isOAEP);
                }
                return DadosCifrados;
            }
            catch (CryptographicException e)
            {
                throw new Exception(e.Message);
            }
        }
        
        static public byte[] RSADecifra(byte[] byteDecifrado, RSAParameters RSAInfo, bool isOAEP)
        {
            try
            {
                byte[] DadosDecifrados;
                //Cria uma nova instância de RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Importa informação da chavev RSA
                    //Isso é preciso para incluir a informação da chave privada
                    RSA.ImportParameters(RSAInfo);
                    //Decifra o array de bytes e especifica o preenchimento OAEP.
                    DadosDecifrados = RSA.Decrypt(byteDecifrado, isOAEP);
                }
                return DadosDecifrados;
            }
            catch (CryptographicException e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion
    }
}
