using AutoMapper;

namespace Blt.Core
{
    //internal class Mapping
    //{
    //}

    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<BuyTicketCommand, Ticket>()
                .ForMember(x => x.Id, y => y.Ignore());
            CreateMap<Ticket, TicketReservedEvent>();
        }
    }

    public static class ComnadExtensions
    {
        public static string ToJson(this object command)
        {
            return System.Text.Json.JsonSerializer.Serialize(command);
        }
    }

    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> Lazy = new(() =>
        {
            var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(ObjectMapper).Assembly));
            return config.CreateMapper();
        });

        public static IMapper Mapper => Lazy.Value;

        public static T MapTo<T>(this object source) => Mapper.Map<T>(source);
    }
}