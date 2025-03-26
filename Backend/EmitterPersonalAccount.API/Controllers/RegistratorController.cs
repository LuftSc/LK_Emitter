using EmitterPersonalAccount.Core.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmitterPersonalAccount.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistratorController : ControllerBase
    {
        private readonly IRegistratorRepository registratorRepository;

        public RegistratorController(IRegistratorRepository registratorRepository)
        {
            this.registratorRepository = registratorRepository;
        }
    }
}
