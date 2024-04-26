namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public interface IRepository<T>
{
    T? Get(Guid id);
    List<T>? GetAll();
    void Create(T entity);
    void Update(T entity);
    void Delete(Guid id);
}
