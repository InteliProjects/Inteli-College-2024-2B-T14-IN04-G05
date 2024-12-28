using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.ResponsavelAlunoService
{
    public class ResponsavelAlunoService : GenericoService<ResponsavelAluno>, IResponsavelAlunoService
    {
        public ResponsavelAlunoService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(mapper, context, httpContextAccessor)
        {
        }
    }
}
