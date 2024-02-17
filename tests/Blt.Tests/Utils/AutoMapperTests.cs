using Blt.Core.Utils;

namespace Blt.Tests.Utils;
public class AutoMapperTests
{
    [Fact]
    public void ValidateMappingConfigurationTest()
    {
        var mapper = MapperExtensions.Mapper;

        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
