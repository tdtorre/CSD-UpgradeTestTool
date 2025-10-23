using DTOs;

namespace Models
{
    public interface IIcaModule: IModule<TestMappingDto>
    {
        public List<Instrument> Instruments { get; set; }
        public List<string> Samples { get; set; } 
    }
}