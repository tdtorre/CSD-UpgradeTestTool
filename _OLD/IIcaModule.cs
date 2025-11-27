using DTOs;

namespace Models
{
    public interface IIcaModule: IModuleOld<TestMappingDto>
    {
        public List<Instrument> Instruments { get; set; }
    }
}