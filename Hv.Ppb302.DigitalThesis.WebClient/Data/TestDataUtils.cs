namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class TestDataUtils
{
    private readonly GeoTagRepository _geoTagRepo;
    private readonly GroupTagRepository _groupTagRepo;
    private readonly MolarMosaicRepository _molarMosaicRepo;
    private readonly MolecularMosaicRepository _molecularMosaicRepo;

    public TestDataUtils(GeoTagRepository geoTagRepo,
        MolarMosaicRepository molarMosaicRepo,
        MolecularMosaicRepository molecularMosaicRepo,
        GroupTagRepository groupTagRepo)
    {
        _geoTagRepo = geoTagRepo;
        _molarMosaicRepo = molarMosaicRepo;
        _molecularMosaicRepo = molecularMosaicRepo;
        _groupTagRepo = groupTagRepo;
    }
}
