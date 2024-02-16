using AutoMapper;

namespace Blt.Core.Utils
{
    public static class MapperExtensions
    {
        private static readonly Lazy<IMapper> Lazy = new(() =>
        {
            var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(MapperExtensions).Assembly));
            return config.CreateMapper();
        });

        public static IMapper Mapper => Lazy.Value;

        public static T MapTo<T>(this object source) => Mapper.Map<T>(source);
    }
}