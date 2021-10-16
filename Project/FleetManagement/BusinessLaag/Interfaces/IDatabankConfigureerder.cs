﻿using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessLaag.Interfaces
{
    public interface IDatabankConfigureerder
    {
        string ProductieConnectieString { get; }
        bool ConnectieSuccesvol { get; }
        bool DatabaseBestaat { get; }
        bool AlleTabellenBestaan { get; }
        bool SequentieDoorlopen { get; }

        IList<string> geefTabellen();
    }
}