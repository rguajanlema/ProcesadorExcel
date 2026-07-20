using ProcesadorExcel.Application.Models;

namespace ProcesadorExcel.Application.Interfaces
{
    public interface IExcelValidator
    {
        // Recibe el path o stream del archivo
        ValidationResult ValidateStructure(string filePath, string[] expectedColumns);
    }
}
