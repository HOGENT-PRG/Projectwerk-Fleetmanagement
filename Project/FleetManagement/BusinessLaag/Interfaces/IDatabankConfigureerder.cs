using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessLaag.Interfaces
{
    public interface IDatabankConfigureerder
    {
        string ProductieConnectieString { get; }
        bool ConnectieSuccesvol { get; }
        bool DatabaseBestaat { get; }
        bool AlleTabellenBestaan { get; }
        int AantalTabellen { get; }
        bool SequentieDoorlopen { get; }

        IList<string> GeefTabellenLowercase();
    }
}