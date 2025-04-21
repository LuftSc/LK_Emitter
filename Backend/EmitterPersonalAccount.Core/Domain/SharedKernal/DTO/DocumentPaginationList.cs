using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.DTO
{
    public record DocumentPaginationList(
        int TotalSize,
        List<DocumentDTO> Documents
        )
    {
    }
}
