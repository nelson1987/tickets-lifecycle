using MassTransit.Testing;

namespace Blt.Core.Utils;
public static class MassTransitExtensions
{
    public static async Task<TEvent> ConsumedMessage<TEvent>(this IReceivedMessageList received, FilterDelegate<IReceivedMessage<TEvent>> value) where TEvent : class
    {
        var mensagem = await received.SelectAsync(value).FirstOrDefault();
        return mensagem.Context.Message;
    }
    public static async Task<TEvent> ConsumedMessage<TEvent>(this IReceivedMessageList received) where TEvent : class
    {
        var mensagem = await received.SelectAsync<TEvent>().FirstOrDefault();
        return mensagem.Context.Message;
    }
    public static async Task<TEvent> PublishedMessage<TEvent>(this IPublishedMessageList published, FilterDelegate<IPublishedMessage<TEvent>> value) where TEvent : class
    {
        var mensagem = await published.SelectAsync(value).FirstOrDefault();
        return mensagem.Context.Message;
    }
    public static async Task<TEvent> PublishedMessage<TEvent>(this IPublishedMessageList published) where TEvent : class
    {
        var mensagem = await published.SelectAsync<TEvent>().FirstOrDefault();
        return mensagem.Context.Message;
    }
}
