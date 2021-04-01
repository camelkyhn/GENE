namespace Gene.Middleware.Bases
{
    public interface IIdentified<TKey>
    {
        TKey Id { get; set; }
    }
}
