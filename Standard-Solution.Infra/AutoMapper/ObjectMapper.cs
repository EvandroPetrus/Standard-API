using AutoMapper;

// Nothing added for the moment, just add it to your service
// whenever you need to map something else like:
// Car car = ObjectMapper.Mapper.Map<CarDTO, Car>(dto);

namespace Standard_Solution.Infra.AutoMapper
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<Standard_APIProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }
}
