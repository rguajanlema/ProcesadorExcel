using ProcesadorExcel.Application.Interfaces;
using ProcesadorExcel.Domain.Entities;
using ExcelDataReader;
using ProcesadorExcel.Application.Exceptions;

namespace ProcesadorExcel.Application.Services
{
    public class ExcelProcessorService
    {
        private readonly IExcelValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public ExcelProcessorService(IExcelValidator validator, IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task ProcessFileAsync(string filePath)
        {
            // 1. Validar
            var validation = _validator.ValidateStructure(filePath, new[] { "Id", "Nombre", "Valor" });
            if (!validation.IsValid)
                throw new ExcelValidationException(validation.ErrorMessage ?? "El archivo no tiene una estructura válida.");

            // 2. Transacción
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read(); // Saltamos la cabecera ya validada

                    var repo = _unitOfWork.GetRepository<Docente>();

                    while (reader.Read())
                    {
                        // Mapeo manual de la fila a tu entidad
                        var entidad = Docente.Crear(
                            reader.GetInt32(0), 
                            reader.GetString(1), 
                            reader.GetDouble(2)
                            );
                        

                        await repo.AddAsync(entidad);
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
