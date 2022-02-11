using System.Threading.Tasks;

namespace ApiCore.ExportExcel
{
    public interface IExcelExporter
    {
        Task Export();
    }
}
