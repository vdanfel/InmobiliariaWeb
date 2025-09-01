using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;

namespace InmobiliariaWeb.Servicios
{
    public class TablasService:ITablasService
    {
        private readonly SqlConnection _connection;

        public TablasService(SqlConnection connection) 
        {
            _connection = connection;
        }
        
        public async Task<List<TipoDocumento>> ListarTipoDocumento()
        {
            int parametro = 1;
            var tipoDocumentoList = new List<TipoDocumento>();
            try 
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoDocumento = new TipoDocumento();
                        tipoDocumento.Ident_001_TipoDocumento= Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoDocumento.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoDocumento.Valor = reader["VALOR"].ToString();
                        tipoDocumento.Abreviatura = reader["ABREVIATURA"].ToString();
                        tipoDocumentoList.Add(tipoDocumento);
                    }
                    return tipoDocumentoList;
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
        public async Task<List<TipoMoneda>> ListarTipoMoneda()
        {
            int parametro = 2;
            var tipoMonedaList = new List<TipoMoneda>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoMoneda = new TipoMoneda();
                        tipoMoneda.Ident_002_TipoMoneda = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoMoneda.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoMoneda.Abreviatura = reader["ABREVIATURA"].ToString();
                        tipoMonedaList.Add(tipoMoneda);
                    }
                    return tipoMonedaList;
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
        public async Task<List<TipoRol>> ListarTipoUsuarios()
        {
            int parametro = 5;
            var tipoRolList = new List<TipoRol>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoRol = new TipoRol();
                        tipoRol.Ident_005_TipoUsuario = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoRol.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoRol.Abreviatura = reader["ABREVIATURA"].ToString();
                        tipoRolList.Add(tipoRol);
                    }
                    return tipoRolList;
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
        public async Task<List<TipoEstadoCivil>> ListarTipoEstadoCivil()
        {
            int parametro = 6;
            var tipoEstadoCivilList = new List<TipoEstadoCivil>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoEstadoCivil = new TipoEstadoCivil();
                        tipoEstadoCivil.Ident_006_TipoEstadoCivil = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoEstadoCivil.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoEstadoCivil.Valor = reader["VALOR"].ToString();
                        tipoEstadoCivilList.Add(tipoEstadoCivil);
                    }
                    return tipoEstadoCivilList;
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
        public async Task<List<Manzanas>> ListarManzanas()
        {
            int parametro = 7;
            var manzanaList = new List<Manzanas>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var manzana = new Manzanas();
                        manzana.ident_Parametro = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        manzana.Descripcion = reader["DESCRIPCION"].ToString();
                        manzana.Valor = Int32.Parse(reader["VALOR"].ToString());
                        manzanaList.Add(manzana);
                    }
                    return manzanaList;
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
        public async Task<List<TipoLote>> listarTipoLote()
        {
            int parametro = 10;
            var tipoLoteList = new List<TipoLote>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoLote = new TipoLote();
                        tipoLote.Ident_010_TipoLote = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoLote.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoLote.Valor = reader["VALOR"].ToString();
                        tipoLote.Abreviatura = reader["ABREVIATURA"].ToString();
                        tipoLoteList.Add(tipoLote);
                    }
                    return tipoLoteList;
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
        public async Task<List<TipoPropietario>> ListarTipoPropietario()
        {
            int parametro = 11;
            var tipoPropietarioList = new List<TipoPropietario>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoPropietario = new TipoPropietario();
                        tipoPropietario.Ident_011_TipoPropietario = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoPropietario.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoPropietario.Valor = reader["VALOR"].ToString();
                        tipoPropietario.Abreviatura = reader["ABREVIATURA"].ToString();
                        tipoPropietarioList.Add(tipoPropietario);
                    }
                    return tipoPropietarioList;
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
        public async Task<List<TipoUbicacionLote>> ListarTipoUbicacionlote()
        {
            int parametro = 14;
            var tipoUbicacionLotesList = new List<TipoUbicacionLote>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoUbicacionLote = new TipoUbicacionLote();
                        tipoUbicacionLote.Ident_014_TipoUbicacionLote = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoUbicacionLote.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoUbicacionLote.Valor = reader["VALOR"].ToString();
                        tipoUbicacionLote.Abreviatura = reader["ABREVIATURA"].ToString();
                        tipoUbicacionLotesList.Add(tipoUbicacionLote);
                    }
                    return tipoUbicacionLotesList;
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
        public async Task<List<Departamento>> ListarDepartamento()
        { 
            var departamentosList = new List<Departamento>();
            try 
            {
                using (SqlCommand command = new SqlCommand("SP_DepartamentosList", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var departamentos = new Departamento();
                        departamentos.CodigoDepartamento= reader["CODIGO"].ToString();
                        departamentos.DescripcionDepartamento= reader["DESCRIPCION"].ToString();
                        departamentosList.Add(departamentos);
                    }
                    return departamentosList;
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
        public async Task<List<Provincia>> ListarProvincia(string codigoDepartamento)
        {
            var provinciaList = new List<Provincia>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_ProvinciaList", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISCodigoDepartamento", codigoDepartamento);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var provincias = new Provincia();
                        provincias.CodigoProvincia = reader["CODIGO"].ToString();
                        provincias.DescripcionProvincia= reader["DESCRIPCION"].ToString();
                        provinciaList.Add(provincias);
                    }
                    return provinciaList;
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
        public async Task<List<Distrito>> ListarDistrito(string codigoDepartamento,string codigoProvincia)
        {
            var distritoList = new List<Distrito>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_DistritoList", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISCodigoDepartamento", codigoDepartamento);
                    command.Parameters.AddWithValue("@ISCodigoProvincia", codigoProvincia);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var distrito = new Distrito();
                        distrito.CodigoDistrito = reader["CODIGO"].ToString();
                        distrito.DescripcionDistrito = reader["DESCRIPCION"].ToString();
                        distrito.IdentUbigeo = Int32.Parse(reader["IDENT_UBIGEO"].ToString());
                        distritoList.Add(distrito);
                    }
                    return distritoList;
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
        public async Task<List<TipoSexo>> ListarSexo()
        {
            int parametro = 16;
            var tipoSexoList = new List<TipoSexo>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoSexo = new TipoSexo();
                        tipoSexo.Ident_016_TipoSexo = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoSexo.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoSexo.Valor = reader["VALOR"].ToString();
                        tipoSexo.Abreviatura = reader["ABREVIATURA"].ToString();
                        tipoSexoList.Add(tipoSexo);
                    }
                    return tipoSexoList;
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
        public async Task<List<TipoContrato>> ListarTipoContrato()
        {
            int parametro = 17;
            var tipoContratos = new List<TipoContrato>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoContrato = new TipoContrato();
                        tipoContrato.Ident_017_TipoContrato= Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoContrato.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoContratos.Add(tipoContrato);
                    }
                    return tipoContratos;
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
        public async Task<List<TipoLado>> ListarLado()
        {
            int parametro = 9;
            var tipoLados = new List<TipoLado>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoLado = new TipoLado();
                        tipoLado.Ident_009_TipoLado = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoLado.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoLados.Add(tipoLado);
                    }
                    return tipoLados;
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
        public async Task<List<Paises>> ListarPaises()
        {
            var PaisesList = new List<Paises>();
            try 
            {
                using (SqlCommand command = new SqlCommand("usp_Pais_Bandeja", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var paises = new Paises();
                        paises.Ident_Pais = Int32.Parse(reader["Ident_Pais"].ToString());
                        paises.Pais = reader["Pais"].ToString();
                        paises.Hombre = reader["Hombre"].ToString();
                        paises.Mujer = reader["Mujer"].ToString();
                        PaisesList.Add(paises);
                    }
                    return PaisesList;
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
        public async Task<List<TipoPago>> ListarTipoPago()
        {
            int parametro = 18;
            var tipoPagoList = new List<TipoPago>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoPago = new TipoPago();
                        tipoPago.Ident_018_TipoPago = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoPago.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoPago.Abreviatura = reader["ABREVIATURA"].ToString();
                        tipoPagoList.Add(tipoPago);
                    }
                    return tipoPagoList;
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
        public async Task<List<Banco>> ListarBancos()
        {
            int parametro = 19;
            var bancoList = new List<Banco>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var banco = new Banco();
                        banco.Ident_019_Banco = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        banco.Descripcion = reader["DESCRIPCION"].ToString();
                        banco.Abreviatura = reader["ABREVIATURA"].ToString();
                        bancoList.Add(banco);
                    }
                    return bancoList;
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
        public async Task<List<TipoCuentaBanco>> ListarTipoCuentaBanco()
        {
            int parametro = 20;
            var tipoCuentaBancoList = new List<TipoCuentaBanco>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoCuentaBanco = new TipoCuentaBanco();
                        tipoCuentaBanco.Ident_020_TipoCuentaBanco = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoCuentaBanco.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoCuentaBancoList.Add(tipoCuentaBanco);
                    }
                    return tipoCuentaBancoList;
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
        public async Task<List<TipoIngreso>> ListarTipoIngreso()
        {
            int parametro = 21;
            var tipoIngresoList = new List<TipoIngreso>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoIngreso = new TipoIngreso();
                        tipoIngreso.nIdent_021_TipoIngreso = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoIngreso.sDescripcion = reader["DESCRIPCION"].ToString();
                        tipoIngresoList.Add(tipoIngreso);
                    }
                    return tipoIngresoList;
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
        public async Task<List<TipoEgreso>> ListarTipoEgreso()
        {
            int parametro = 22;
            var tipoEgresoList = new List<TipoEgreso>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoEgreso = new TipoEgreso();
                        tipoEgreso.nIdent_022_TipoEgreso = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoEgreso.sDescripcion = reader["DESCRIPCION"].ToString();
                        tipoEgresoList.Add(tipoEgreso);
                    }
                    return tipoEgresoList;
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
