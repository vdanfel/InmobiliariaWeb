using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Documentos;
using InmobiliariaWeb.Models.Opciones;
using InmobiliariaWeb.Models.Tablas;
using Microsoft.Data.SqlClient;

namespace InmobiliariaWeb.Servicios
{
    public class RutaService: IRutaService
    {
        private readonly SqlConnection _connection;
        public RutaService(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<int>RutaArchivoCreate(RutaArchivoDTO rutaArchivoDTO)
        {
            int nIdent_RutaArchivo = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_RutaArchivoCreate", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_024_TablaOrigen", rutaArchivoDTO.nIdent_024_TablaOrigen);
                    command.Parameters.AddWithValue("@nIdent_TablaOrigen", rutaArchivoDTO.nIdent_TablaOrigen);
                    command.Parameters.AddWithValue("@sNombreArchivo", rutaArchivoDTO.sNombreArchivo);
                    command.Parameters.AddWithValue("@sNombreGuardado", rutaArchivoDTO.sNombreGuardado);
                    command.Parameters.AddWithValue("@sExtension", rutaArchivoDTO.sExtension);
                    command.Parameters.AddWithValue("@nTamanio", rutaArchivoDTO.nTamanio);
                    command.Parameters.AddWithValue("@sRutaArchivo", rutaArchivoDTO.sRutaArchivo);
                    command.Parameters.AddWithValue("@nIdent_025_TipoArchivo", rutaArchivoDTO.nIdent_025_TipoArchivo);
                    command.Parameters.AddWithValue("@UsuarioCreacion", rutaArchivoDTO.UsuarioCreacion);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        nIdent_RutaArchivo = Int32.Parse(reader["nIdent_RutaArchivo"].ToString());
                    }

                    return nIdent_RutaArchivo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<List<RutaArchivoDTO>> RutaArchivoList(RutaArchivoRequestDTO rutaArchivoRequestDTO)
        {
            var archivos = new List<RutaArchivoDTO>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_RutaArchivoList", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_024_TablaOrigen", rutaArchivoRequestDTO.nIdent_024_TablaOrigen);
                    command.Parameters.AddWithValue("@nIdent_TablaOrigen", rutaArchivoRequestDTO.nIdent_TablaOrigen);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    { 
                        var archivo = new RutaArchivoDTO();
                        archivo.nIdent_RutaArchivo = Int32.Parse(reader["nIdent_RutaArchivo"].ToString());
                        archivo.sNombreArchivo = reader["sNombreArchivo"].ToString();
                        archivo.sNombreGuardado = reader["sNombreGuardado"].ToString();
                        archivo.sExtension = reader["sExtension"].ToString();
                        archivo.nTamanio = Decimal.Parse(reader["nTamanio"].ToString());
                        archivo.sRutaArchivo = reader["sRutaArchivo"].ToString();
                        archivo.sTipoArchivo = reader["sTipoArchivo"].ToString();
                        archivos.Add(archivo);
                    }
                    return archivos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<List<FormatosResponseDTO>> FormatosList(int nIdent_Contratos)
        {
            var formatos = new List<FormatosResponseDTO>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_FormatosImpresionList", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Contratos", nIdent_Contratos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var formato = new FormatosResponseDTO();
                        formato.sTipoDocumento = reader["sTipoDocumento"].ToString();
                        formato.nIdent_Formato = Int32.Parse(reader["nIdent_Formato"].ToString());
                        formato.sNombreFormato = reader["sNombreFormato"].ToString();
                        formato.dFechaCreacion = DateTime.Parse(reader["dFechaCreacion"].ToString());
                        formatos.Add(formato);
                    }
                    return formatos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<string> FormatoDownload(FormatoSelectDTO formatoSelectDTO)
        {
            string formato = null;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_FormatoImpresionGet", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Formato", formatoSelectDTO.nIdent_Formato);
                    command.Parameters.AddWithValue("@sTipoDocumento", formatoSelectDTO.sTipoDocumento);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        formato = reader["sFormato"].ToString();
                    }
                    return formato;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<int> RutaArchivoDelete(RutaArchivoDTO rutaArchivoDTO)
        {
            int res = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_RutaArchivoDelete", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_RutaArchivo", rutaArchivoDTO.nIdent_RutaArchivo);
                    command.Parameters.AddWithValue("@UsuarioModificacion", rutaArchivoDTO.UsuarioModificacion);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        res = Int32.Parse(reader["res"].ToString());
                    }
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<IEnumerable<TipoArchivoOpcionDTO>> TipoArchivoOpcionListar()
        {
            var tiposArchivo = new List<TipoArchivoOpcionDTO>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_ParametrosTipo", 25);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoArchivo = new TipoArchivoOpcionDTO();
                        tipoArchivo.nIdent_025_TipoArchivo = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoArchivo.sDescripcion = reader["DESCRIPCION"].ToString();
                        tipoArchivo.sAbreviatura = reader["ABREVIATURA"].ToString();
                        tiposArchivo.Add(tipoArchivo);
                    }
                    return tiposArchivo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
