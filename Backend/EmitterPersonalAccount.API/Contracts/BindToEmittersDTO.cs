namespace EmitterPersonalAccount.API.Contracts
{
    public record BindToEmittersDTO (
        Guid UserId,
        List<Guid> EmittersIdList
        )
    {
    }
}
