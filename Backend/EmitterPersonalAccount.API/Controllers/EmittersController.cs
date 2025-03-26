using EmitterPersonalAccount.Core.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmitterPersonalAccount.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmittersController : ControllerBase
    {
        private readonly IEmittersRepository emittersRepository;

        public EmittersController(IEmittersRepository emittersRepository)
        {
            this.emittersRepository = emittersRepository;
        }


    }
}
