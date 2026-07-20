using ExcelDataReader;
using ProcesadorExcel.Application.Interfaces;
using ProcesadorExcel.Application.Models;

namespace ProcesadorExcel.Application.Services
{
    
    public class ExcelValidator : IExcelValidator
    {
        public ValidationResult ValidateStructure(string filePath, string[] expectedColumns)
        {
            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Leemos solo la primera fila (la cabecera)
                    if (!reader.Read())
                        return new ValidationResult(false, "El archivo está vacío.");

                    var columnsFound = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        columnsFound.Add(reader.GetString(i)?.Trim());
                    }

                    // Validamos que todas las esperadas existan
                    foreach (var expected in expectedColumns)
                    {
                        if (!columnsFound.Contains(expected, StringComparer.OrdinalIgnoreCase))
                            return new ValidationResult(false, $"Falta la columna requerida: {expected}");
                    }

                    return new ValidationResult(true);
                }
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, $"Error al leer el archivo: {ex.Message}");
            }
        }
    }
}
