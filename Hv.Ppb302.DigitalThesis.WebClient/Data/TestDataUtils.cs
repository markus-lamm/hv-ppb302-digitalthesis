using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class TestDataUtils
{
    private readonly GeoTagRepository _geoTagRepo;
    private readonly ConnectorTagRepository _connectorTagRepo;
    private readonly MolarMosaicRepository _molarMosaicRepo;
    private readonly MolecularMosaicRepository _molecularMosaicRepo;

    public TestDataUtils(GeoTagRepository geoTagRepo,
        MolarMosaicRepository molarMosaicRepo,
        MolecularMosaicRepository molecularMosaicRepo,
        ConnectorTagRepository connectorTagRepo)
    {
        _geoTagRepo = geoTagRepo;
        _molarMosaicRepo = molarMosaicRepo;
        _molecularMosaicRepo = molecularMosaicRepo;
        _connectorTagRepo = connectorTagRepo;
    }

    public void CreateConnectorTag()
    {
        var random = new Random();
        var randomNumber = random.Next(1, 1000);
        var connectorTag = new ConnectorTag
        {
            Name = "ConnectorTagTest" + randomNumber,
        };
        _connectorTagRepo.Create(connectorTag);
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

    public void CreateGeoTagWithConnectorTag(string connectorTagId)
    {
        var connectorTag = FindConnectorTagById(connectorTagId);
        if (connectorTag == null) { return; }

        var random = new Random();
        var randomNumber = random.Next(1, 1000);
        var geoTag = new GeoTag
        {
            Title = "GeoTagTest" + randomNumber,
        };

        _geoTagRepo.Create(geoTag);
        _geoTagRepo.AddConnectorTag(geoTag.Id, connectorTag.Id);
    }

    public void CreateMolarMosaicWithConnectorTag(string connectorTagId)
    {
        var connectorTag = FindConnectorTagById(connectorTagId);
        if (connectorTag == null) { return; }

        var random = new Random();
        var randomNumber = random.Next(1, 1000);
        var molarMosaic = new MolarMosaic
        {
            Title = "MolarTest" + randomNumber,
        };

        _molarMosaicRepo.Create(molarMosaic);
        _molarMosaicRepo.AddConnectorTag(molarMosaic.Id, connectorTag.Id);
    }

    public void CreateMolecularMosaicWithConnectorTag(string connectorTagId)
    {
        var connectorTag = FindConnectorTagById(connectorTagId);
        if (connectorTag == null) { return; }

        var random = new Random();
        var randomNumber = random.Next(1, 1000);
        var molecularMosaic = new MolecularMosaic
        {
            Title = "MolecularTest" + randomNumber,
        };

        _molecularMosaicRepo.Create(molecularMosaic);
        _molecularMosaicRepo.AddConnectorTag(molecularMosaic.Id, connectorTag.Id);
    }

    public GeoTag? FindGeoTagById(string id)
    {
        return _geoTagRepo.Get(Guid.Parse(id));
    }

    public ConnectorTag? FindConnectorTagById(string id)
    {
        return _connectorTagRepo.Get(Guid.Parse(id));
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
        _connectorTagRepo.DeleteAllByName(name);
    }
}
