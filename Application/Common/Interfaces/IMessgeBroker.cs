namespace Application.Common.Interfaces;

public interface IMessgeBroker<T>
{
    Task Publish(T message);
    Task Publish(IEnumerable<T> messages);
    Task<T> Consume();
}
