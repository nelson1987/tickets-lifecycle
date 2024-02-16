using MongoDB.Driver;

namespace Blt.Core.Features.Tickets
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _ticketsCollection;
        //TODO: Implementar UnitOfWork
        public TicketRepository()
        {
            var mongoClient = new MongoClient("mongodb://root:example@localhost:27017/");
            var database = mongoClient.GetDatabase("sales");
            _ticketsCollection = database.GetCollection<Ticket>(nameof(Ticket));
        }
        public async Task AddTicketAsync(Ticket ticket)
        {
            await _ticketsCollection.InsertOneAsync(ticket);
            //await Task.CompletedTask;
        }

        public async Task<Ticket?> GetEventByDocument(string @event, string document)
        {
            var collection = await _ticketsCollection.FindAsync(x => x.Event == @event && x.Document == document);
            return collection.FirstOrDefault();
            //return Task.FromResult((Ticket)null);
        }
        public async Task<long> DeleteAll()
        {
            var result = await _ticketsCollection.DeleteManyAsync(x => x.Id != Guid.Empty);
            return result.DeletedCount;
            //return Task.FromResult((Ticket)null);
        }
    }
}