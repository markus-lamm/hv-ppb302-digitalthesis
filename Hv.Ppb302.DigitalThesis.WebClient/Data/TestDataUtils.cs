using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class TestDataUtils
{
    private readonly GeoTagRepository _geoTagRepo;
    private readonly GroupTagRepository _groupTagRepo;
    private readonly MolarMosaicRepository _molarMosaicRepo;
    private readonly MolecularMosaicRepository _molecularMosaicRepo;
    private readonly KaleidoscopeMosaicRepository _kaleidoscopeMosaicRepository;

    public TestDataUtils(GeoTagRepository geoTagRepo,
        MolarMosaicRepository molarMosaicRepo,
        MolecularMosaicRepository molecularMosaicRepo,
        GroupTagRepository groupTagRepo,
        KaleidoscopeMosaicRepository kaleidoscopeMosaicRepository)
    {
        _geoTagRepo = geoTagRepo;
        _molarMosaicRepo = molarMosaicRepo;
        _molecularMosaicRepo = molecularMosaicRepo;
        _groupTagRepo = groupTagRepo;
        _kaleidoscopeMosaicRepository = kaleidoscopeMosaicRepository;
    }

    public void CreateGroupTag()
    {
        var random = new Random();
        var randomNumber = random.Next(1, 1000);
        var groupTag = new GroupTag
        {
            Name = "GroupTagTest" + randomNumber,
        };
        _groupTagRepo.Create(groupTag);
    }

    public void CreateGeoTag()
    {
        var random = new Random();
        var randomNumber = random.Next(1, 1000);
        var geoTag = new GeoTag
        {
            Title = "GeoTagTest" + randomNumber,
        };
        _geoTagRepo.Create(geoTag);
    }

    public void CreateMolarMosaic()
    {
        var random = new Random();
        var randomNumber = random.Next(1, 1000);
        var molarMosaic = new MolarMosaic
        {
            Title = "MolarTest" + randomNumber,
        };
        _molarMosaicRepo.Create(molarMosaic);
    }

    public void CreateMolecularMosaic()
    {
        var random = new Random();
        var randomNumber = random.Next(1, 1000);
        var molecularMosaic = new MolecularMosaic
        {
            Title = "MolecularTest" + randomNumber,
        };
        _molecularMosaicRepo.Create(molecularMosaic);
    }

    public void CreateGeoTagWithGroupTag(string groupTagId)
    {
        var groupTag = FindGroupTagById(groupTagId);
        if (groupTag == null) { return; }

        var random = new Random();
        var randomNumber = random.Next(1, 1000);
        var geoTag = new GeoTag
        {
            Title = "GeoTagTest" + randomNumber,
        };

        _geoTagRepo.Create(geoTag);
        _geoTagRepo.AddGroupTag(geoTag.Id, groupTag.Id);
    }

    public void CreateMolarMosaicWithGroupTag(string groupTagId)
    {
        var groupTag = FindGroupTagById(groupTagId);
        if (groupTag == null) { return; }

        var random = new Random();
        var randomNumber = random.Next(1, 1000);
        var molarMosaic = new MolarMosaic
        {
            Title = "MolarTest" + randomNumber,
        };

        _molarMosaicRepo.Create(molarMosaic);
        _molarMosaicRepo.AddGroupTag(molarMosaic.Id, groupTag.Id);
    }

    public void CreateMolecularMosaicWithGroupTag(string groupTagId)
    {
        var groupTag = FindGroupTagById(groupTagId);
        if (groupTag == null) { return; }

        var random = new Random();
        var randomNumber = random.Next(1, 1000);
        var molecularMosaic = new MolecularMosaic
        {
            Title = "MolecularTest" + randomNumber,
        };

        _molecularMosaicRepo.Create(molecularMosaic);
        _molecularMosaicRepo.AddGroupTag(molecularMosaic.Id, groupTag.Id);
    }

    public void CreateKaleidoscopeMosaicWithGroupTag(string groupTagId)
    {
        var groupTag = FindGroupTagById(groupTagId);
        if (groupTag == null) { return; }

        var random = new Random();
        var randomNumber = random.Next(1, 1000);
        var KaleidoscopeMosaic = new KaleidoscopeMosaic
        {
            Title = "KaleidoscopeMosaicTest" + randomNumber,
        };

        _kaleidoscopeMosaicRepository.Create(KaleidoscopeMosaic);
        _kaleidoscopeMosaicRepository.AddGroupTag(KaleidoscopeMosaic.Id, groupTag.Id);
    }

    public GeoTag? FindGeoTagById(string id)
    {
        return _geoTagRepo.Get(Guid.Parse(id));
    }

    public GroupTag? FindGroupTagById(string id)
    {
        return _groupTagRepo.Get(Guid.Parse(id));
    }

    public MolarMosaic? FindMolarMosaicById(string id)
    {
        return _molarMosaicRepo.Get(Guid.Parse(id));
    }

    public MolecularMosaic? FindMolecularMosaicById(string id)
    {
        return _molecularMosaicRepo.Get(Guid.Parse(id));
    }

    public void PurgeAllDataByName(string name)
    {
        _geoTagRepo.DeleteAllByTitle(name);
        _molarMosaicRepo.DeleteAllByTitle(name);
        _molecularMosaicRepo.DeleteAllByTitle(name);
        _groupTagRepo.DeleteAllByName(name);
    }
}
