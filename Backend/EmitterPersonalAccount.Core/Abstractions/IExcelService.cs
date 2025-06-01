using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IExcelService
    {
        Task<Result<byte[]>> WriteLogsToExcelFile(List<ActionDTO> logs);
    }
}