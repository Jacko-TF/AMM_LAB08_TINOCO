using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMM_LAB08_TINOCO.interfaces
{
    public interface IQrScanningService
    {
        Task<String> ScanAsync();
    }
}
