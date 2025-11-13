using InmobiliariaWeb.Controllers;
using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Separaciones;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Programa;
using InmobiliariaWeb.Result.Separaciones;
using Microsoft.Data.SqlClient;
using System.Data;
using ClienteViewModel = InmobiliariaWeb.Models.Separaciones.ClienteViewModel;

namespace InmobiliariaWeb.Servicios
{
    public class SeparacionesService:ISeparacionesService
    {
        private readonly SqlConnection _connection;
        public SeparacionesService(SqlConnection connection) 
        { 
            _connection = connection;
        }
        public async Task<SeparacionesImpresionViewModel> ImprimirSeparaciones(int ident_Separacion)
        {
            SeparacionesImpresionViewModel separacionesImpresionViewModel = new SeparacionesImpresionViewModel();
            separacionesImpresionViewModel.Clientes = new List<SeparacionesClientesImpresionViewModel>();
            try 
            {
                using (SqlCommand command = new SqlCommand("usp_Separaciones_Imprimir", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Separaciones", ident_Separacion);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        separacionesImpresionViewModel.NumeroSerie = reader["NumeroSerie"].ToString();
                        separacionesImpresionViewModel.FechaSeparacion = DateTime.Parse(reader["FechaSeparacion"].ToString());
                        separacionesImpresionViewModel.NombrePrograma = reader["NombrePrograma"].ToString();
                        separacionesImpresionViewModel.Manzana = reader["Manzana"].ToString();
                        separacionesImpresionViewModel.Lote = Int32.Parse(reader["Lote"].ToString());
                        separacionesImpresionViewModel.Ubicacion = reader["Ubicacion"].ToString();
                        separacionesImpresionViewModel.TipoLote = Int32.Parse(reader["TipoLote"].ToString());
                        separacionesImpresionViewModel.Izquierda = decimal.Parse(reader["Izquierda"].ToString());
                        separacionesImpresionViewModel.Derecha = decimal.Parse(reader["Derecha"].ToString());
                        separacionesImpresionViewModel.Frente = decimal.Parse(reader["Frente"].ToString());
                        separacionesImpresionViewModel.Fondo = decimal.Parse(reader["Fondo"].ToString());
                        separacionesImpresionViewModel.L1 = decimal.Parse(reader["L1"].ToString());
                        separacionesImpresionViewModel.L2 = decimal.Parse(reader["L2"].ToString());
                        separacionesImpresionViewModel.L3 = decimal.Parse(reader["L3"].ToString());
                        separacionesImpresionViewModel.L4 = decimal.Parse(reader["L4"].ToString());
                        separacionesImpresionViewModel.L5 = decimal.Parse(reader["L5"].ToString());
                        separacionesImpresionViewModel.L6 = decimal.Parse(reader["L6"].ToString());
                        separacionesImpresionViewModel.L7 = decimal.Parse(reader["L7"].ToString());
                        separacionesImpresionViewModel.L8 = decimal.Parse(reader["L8"].ToString());
                        separacionesImpresionViewModel.L9 = decimal.Parse(reader["L9"].ToString());
                        separacionesImpresionViewModel.L10 = decimal.Parse(reader["L10"].ToString());
                        separacionesImpresionViewModel.Area = decimal.Parse(reader["Area"].ToString());
                        separacionesImpresionViewModel.PrecioM2 = decimal.Parse(reader["PrecioM2"].ToString());
                        separacionesImpresionViewModel.PrecioTotal = decimal.Parse(reader["PrecioTotal"].ToString());
                        separacionesImpresionViewModel.TratadoEn = decimal.Parse(reader["TratadoEn"].ToString());
                        separacionesImpresionViewModel.CuotaInicial = decimal.Parse(reader["CuotaInicial"].ToString());
                        separacionesImpresionViewModel.SaldoAPagar = decimal.Parse(reader["SaldoAPagar"].ToString());
                        separacionesImpresionViewModel.CantidadCuotas = Int32.Parse(reader["CantidadCuotas"].ToString());
                        separacionesImpresionViewModel.CuotasIniciales = decimal.Parse(reader["CuotasIniciales"].ToString());
                        separacionesImpresionViewModel.CuotaFinal = decimal.Parse(reader["CuotaFinal"].ToString());
                        separacionesImpresionViewModel.FechaVencimiento = DateTime.Parse(reader["FechaVencimiento"].ToString());
                        separacionesImpresionViewModel.Observacion = reader["Observacion"].ToString();
                        separacionesImpresionViewModel.ImporteSeparacion = decimal.Parse(reader["ImporteSeparacion"].ToString());
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        SeparacionesClientesImpresionViewModel cliente = new SeparacionesClientesImpresionViewModel();
                        cliente.NombreCliente = reader["NombreCliente"].ToString();
                        cliente.TipoDocumento = reader["TipoDocumento"].ToString();
                        cliente.Documento = reader["Documento"].ToString();
                        separacionesImpresionViewModel.Clientes.Add(cliente);
                    }
                    return (separacionesImpresionViewModel);
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
        public async Task<List<SeparacionesList>> BandejaSeparaciones(IndexViewModel indexViewModel)
        { 
            var separaciones = new List<SeparacionesList>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_SEPARACIONES_BANDEJA", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nCorrelativo", indexViewModel.Correlativo);
                    command.Parameters.AddWithValue("@nIdent_Programa", indexViewModel.Ident_Programa);
                    command.Parameters.AddWithValue("@nIdent_Manzana", indexViewModel.Ident_Manzana);
                    command.Parameters.AddWithValue("@sCliente", indexViewModel.Cliente ?? "");
                    command.Parameters.AddWithValue("@nPaginaActual", indexViewModel.PaginaActual);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var separacionesList = new SeparacionesList();
                        separacionesList.Indice = Int32.Parse(reader["INDICE"].ToString());
                        separacionesList.Ident_Separaciones = Int32.Parse(reader["IDENT_SEPARACIONES"].ToString());
                        separacionesList.NumeroSerie = reader["NUMERO_SERIE"].ToString();
                        separacionesList.Cliente = reader["NOMBRE_CLIENTE"].ToString();
                        separacionesList.Nombreprograma = reader["NOMBRE_PROGRAMA"].ToString();
                        separacionesList.Manzana = reader["MANZANA"].ToString();
                        separacionesList.Lote = Int32.Parse(reader["LOTE"].ToString());
                        separacionesList.FechaVencimiento = DateTime.Parse(reader["FECHA_VENCIMIENTO"].ToString());
                        separacionesList.Correlativo = Int32.Parse(reader["CORRELATIVO"].ToString());
                        separaciones.Add(separacionesList);
                    }
                    if (reader.HasRows)
                    {
                        await reader.NextResultAsync();  // Mueve al conjunto de resultados siguiente
                        while (reader.Read())
                        {
                            var totalPaginas = reader["TOTAL_PAGINAS"];
                            // Hacer algo con la cantidad de páginas...
                            indexViewModel.NumeroPaginas = Convert.ToInt32(totalPaginas);
                        }
                    }
                    return separaciones;
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
        public async Task<List<ProgramasCbxList>> ProgramaCbxListar()
        { 
            var programasList = new List<ProgramasCbxList>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Programa_CbxListar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var programas = new ProgramasCbxList();
                        programas.Ident_Programa = Int32.Parse(reader["IDENT_PROGRAMA"].ToString());
                        programas.Nombre_Programa = reader["NOMBRE_PROGRAMA"].ToString();
                        programasList.Add(programas);
                    }
                    
                    return programasList;
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
        public async Task<List<ManzanaCbxList>> ManzanaCbxListar(int ident_Programa)
        {
            var manzanaCbxLists = new List<ManzanaCbxList>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Manzana_CbxListar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Programa", ident_Programa);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var manzanas = new ManzanaCbxList();
                        manzanas.Ident_Manzana = Int32.Parse(reader["IDENT_MANZANA"].ToString());
                        manzanas.Letra = reader["LETRA"].ToString();
                        manzanaCbxLists.Add(manzanas);
                    }
                    return manzanaCbxLists;
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
        public async Task<List<LoteCbxList>> LoteCbxListar(int ident_Manzana)
        {
            var lotesCbxList = new List<LoteCbxList>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Lotes_CbxListar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Manzana", ident_Manzana);
                    await _connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var lote = new LoteCbxList();
                        lote.Ident_Lote = Int32.Parse(reader["IDENT_LOTE"].ToString());
                        lote.Correlativo = Int32.Parse(reader["CORRELATIVO"].ToString());
                        lotesCbxList.Add(lote);
                    }
                    return lotesCbxList;
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
        public async Task<Lote_xIdentLote> LoteDetalle(int Ident_Lote)
        {
            var lote = new Lote_xIdentLote();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Lote_xIdentLote", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Lote", Ident_Lote);
                    await _connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        lote.Ident_010_TipoLote = Int32.Parse(reader["IDENT_010_TIPOLOTE"].ToString());
                        lote.PrecioM2 = decimal.Parse(reader["PRECIOM2"].ToString());
                        lote.Area = decimal.Parse(reader["AREA"].ToString());
                        lote.PrecioTotal = decimal.Parse(reader["PRECIOTOTAL"].ToString());
                        lote.Porcentaje = decimal.Parse(reader["PORCENTAJE"].ToString());
                        lote.Ident_014_Ubicacionlote = Int32.Parse(reader["IDENT_014_UBICACIONLOTE"].ToString());
                        lote.Ubicacion = reader["UBICACION"].ToString();
                        lote.Izquierda = decimal.Parse(reader["IZQUIERDA"].ToString());
                        lote.Derecha = decimal.Parse(reader["DERECHA"].ToString());
                        lote.Frente = decimal.Parse(reader["FRENTE"].ToString());
                        lote.Fondo = decimal.Parse(reader["FONDO"].ToString());
                        lote.Lado1 = decimal.Parse(reader["LADO1"].ToString());
                        lote.Lado2 = decimal.Parse(reader["LADO2"].ToString());
                        lote.Lado3 = decimal.Parse(reader["LADO3"].ToString());
                        lote.Lado4 = decimal.Parse(reader["LADO4"].ToString());
                        lote.Lado5 = decimal.Parse(reader["LADO5"].ToString());
                        lote.Lado6 = decimal.Parse(reader["LADO6"].ToString());
                        lote.Lado7 = decimal.Parse(reader["LADO7"].ToString());
                        lote.Lado8 = decimal.Parse(reader["LADO8"].ToString());
                        lote.Lado9 = decimal.Parse(reader["LADO9"].ToString());
                        lote.Lado10 = decimal.Parse(reader["LADO10"].ToString());
                    }
                    return lote;
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

        public async Task<int> SeparacionesInsert(CrearViewModel crearViewModel, LoginResult loginResult)
        {
            int ident_Separaciones = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Separaciones_Insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Lote", crearViewModel.Ident_Lote);
                    command.Parameters.AddWithValue("@ISFechaSeparacion", crearViewModel.FechaSeparacion);
                    command.Parameters.AddWithValue("@ISTratadoEn", crearViewModel.TratadoEn);
                    command.Parameters.AddWithValue("@ISCuotaInicial", crearViewModel.CuotaInicial);
                    command.Parameters.AddWithValue("@ISSaldoAPagar", crearViewModel.SaldoAPagar);
                    command.Parameters.AddWithValue("@ISCantidadCuotas", crearViewModel.CantidadCuotas);
                    command.Parameters.AddWithValue("@ISCuotasIniciales", crearViewModel.CuotasIniciales);
                    command.Parameters.AddWithValue("@ISCuotaFinal", crearViewModel.CuotaFinal);
                    command.Parameters.AddWithValue("@ISDiasSeparacion", crearViewModel.DiasSeparacion);
                    //command.Parameters.AddWithValue("@ISFechaVencimiento", crearViewModel.FechaVencimiento);
                    command.Parameters.AddWithValue("@ISImporteSeparacion", crearViewModel.ImporteSeparacion);
                    command.Parameters.AddWithValue("@ISTipoCambio", crearViewModel.TipoCambio);
                    command.Parameters.AddWithValue("@ISObservacion", crearViewModel.Observacion??"");
                    command.Parameters.AddWithValue("@ISUsuarioCreacion", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        ident_Separaciones = Int32.Parse(reader["IDENT_SEPARACIONES"].ToString());
                    }
                    return ident_Separaciones;
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
        public async Task<ActualizarViewModel> SeparacionXIdentSeparacion(int ident_Separacion)
        {
            var actualizarViewModel = new ActualizarViewModel();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Separaciones_xIdentSeparacion", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Separaciones", ident_Separacion);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        actualizarViewModel.Ident_Separacion = Int32.Parse(reader["Ident_Separaciones"].ToString());
                        actualizarViewModel.FechaSeparacion = DateTime.Parse(reader["FechaSeparacion"].ToString());
                        actualizarViewModel.Numero_Separacion = reader["Numero_Separacion"].ToString();
                        actualizarViewModel.Nombre_Programa = reader["Nombre_Programa"].ToString();
                        actualizarViewModel.Manzana = reader["Manzana"].ToString();
                        actualizarViewModel.Lote = Int32.Parse(reader["Lote"].ToString());
                        actualizarViewModel.Area = Decimal.Parse(reader["Area"].ToString());
                        actualizarViewModel.Ubicacion = reader["Ubicacion"].ToString();
                        actualizarViewModel.PrecioM2 = Decimal.Parse(reader["PrecioM2"].ToString());
                        actualizarViewModel.PrecioTotal = Decimal.Parse(reader["PrecioTotal"].ToString());
                        actualizarViewModel.Izquierda = Decimal.Parse(reader["Izquierda"].ToString());
                        actualizarViewModel.Derecha = Decimal.Parse(reader["Derecha"].ToString());
                        actualizarViewModel.Frente = Decimal.Parse(reader["Frente"].ToString());
                        actualizarViewModel.Fondo = Decimal.Parse(reader["Fondo"].ToString());
                        actualizarViewModel.Lado1 = Decimal.Parse(reader["Lado1"].ToString());
                        actualizarViewModel.Lado2 = Decimal.Parse(reader["Lado2"].ToString());
                        actualizarViewModel.Lado3 = Decimal.Parse(reader["Lado3"].ToString());
                        actualizarViewModel.Lado4 = Decimal.Parse(reader["Lado4"].ToString());
                        actualizarViewModel.Lado5 = Decimal.Parse(reader["Lado5"].ToString());
                        actualizarViewModel.Lado6 = Decimal.Parse(reader["Lado6"].ToString());
                        actualizarViewModel.Lado7 = Decimal.Parse(reader["Lado7"].ToString());
                        actualizarViewModel.Lado8 = Decimal.Parse(reader["Lado8"].ToString());
                        actualizarViewModel.Lado9 = Decimal.Parse(reader["Lado9"].ToString());
                        actualizarViewModel.Lado10 = Decimal.Parse(reader["Lado10"].ToString());
                        actualizarViewModel.TratadoEn = Decimal.Parse(reader["TratadoEn"].ToString());
                        actualizarViewModel.CuotaInicial = Decimal.Parse(reader["CuotaInicial"].ToString());
                        actualizarViewModel.SaldoAPagar = Decimal.Parse(reader["SaldoAPagar"].ToString());
                        actualizarViewModel.CantidadCuotas = Int32.Parse(reader["CantidadCuotas"].ToString());
                        actualizarViewModel.CuotasIniciales = Decimal.Parse(reader["CuotasIniciales"].ToString());
                        actualizarViewModel.CuotaFinal = Decimal.Parse(reader["CuotaFinal"].ToString());
                        actualizarViewModel.DiasSeparacion = Int32.Parse(reader["DiasSeparacion"].ToString());
                        actualizarViewModel.FechaVencimiento = DateTime.Parse(reader["FechaVencimiento"].ToString());
                        actualizarViewModel.TipoCambio = Decimal.Parse(reader["TipoCambio"].ToString());
                        actualizarViewModel.ImporteSeparacion = Decimal.Parse(reader["ImporteSeparacion"].ToString());
                        actualizarViewModel.Observacion = reader["Observacion"].ToString();
                        actualizarViewModel.MotivoActualizacion = reader["MotivoActualizacion"].ToString();
                        actualizarViewModel.ident_010_TipoLote = Int32.Parse(reader["ident_010_TipoLote"].ToString());
                    }
                    return actualizarViewModel;
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
        public async Task<string> ActualizarSeparacion(ActualizarViewModel actualizarViewModel, LoginResult loginResult)
        {
            var mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("usp_SeparacionesActualizar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Separaciones", actualizarViewModel.Ident_Separacion);
                    command.Parameters.AddWithValue("@ISTratadoEn", actualizarViewModel.TratadoEn);
                    command.Parameters.AddWithValue("@ISCuotaInicial", actualizarViewModel.CuotaInicial);
                    command.Parameters.AddWithValue("@ISSaldoAPagar", actualizarViewModel.SaldoAPagar);
                    command.Parameters.AddWithValue("@ISCantidadCuotas", actualizarViewModel.CantidadCuotas);
                    command.Parameters.AddWithValue("@ISCuotasIniciales", actualizarViewModel.CuotasIniciales);
                    command.Parameters.AddWithValue("@ISCuotaFinal", actualizarViewModel.CuotaFinal);
                    command.Parameters.AddWithValue("@ISDiasSeparacion", actualizarViewModel.DiasSeparacion);
                    command.Parameters.AddWithValue("@ISFechaVencimiento", actualizarViewModel.FechaVencimiento);
                    command.Parameters.AddWithValue("@ISImporteSeparacion", actualizarViewModel.ImporteSeparacion);
                    command.Parameters.AddWithValue("@ISTipoCambio", actualizarViewModel.TipoCambio);
                    command.Parameters.AddWithValue("@ISMotivoActualizacion", actualizarViewModel.MotivoActualizacion);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        mensaje = reader[0].ToString();
                    }
                    return mensaje;
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
        public async Task<List<ClientesList>> ClientexSeparacion(ClienteViewModel clienteViewModel)
        {
            var clienteList = new List<ClientesList>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_SeparacionesPersonas_List", _connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Separaciones", clienteViewModel.Ident_Separaciones);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var clientes = new ClientesList();
                        clientes.Indice = Int32.Parse(reader["Indice"].ToString());
                        clientes.Ident_SeparacionesPersonas = Int32.Parse(reader["Ident_SeparacionesPersonas"].ToString());
                        clientes.Ident_Separaciones = Int32.Parse(reader["Ident_Separaciones"].ToString());
                        clientes.Ident_Persona = Int32.Parse(reader["Ident_Persona"].ToString());
                        clientes.TipoDocumento = reader["TipoDocumento"].ToString();
                        clientes.NumeroDocumento = reader["NumeroDocumento"].ToString();
                        clientes.Nombre_Completo = reader["Nombre_Completo"].ToString();
                        clienteList.Add(clientes);
                    }
                    return clienteList;
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
        public async Task<string> ClienteInsertar(ClienteViewModel clienteViewModel,LoginResult loginResult)
        {
            var Mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("usp_SeparacionesPersonas_Insert", _connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Separaciones", clienteViewModel.Ident_Separaciones);
                    command.Parameters.AddWithValue("@ISIdent_Persona", clienteViewModel.Ident_Persona);
                    command.Parameters.AddWithValue("@ISIdent_Usuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        Mensaje = reader["Mensaje"].ToString();
                    }
                    return Mensaje;
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
        public async Task<string> ClienteEliminar(int Ident_SeparacionesCliente, LoginResult loginResult)
        {
            var Mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("usp_SeparacionesPersonas_Delete", _connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_SeparacionesCliente", Ident_SeparacionesCliente);
                    command.Parameters.AddWithValue("@ISIdent_Usuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        Mensaje = reader["Mensaje"].ToString();
                    }
                    return Mensaje;
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
        public async Task<string> SeparacionesAnular(ActualizarViewModel actualizarViewModel, LoginResult loginResult)
        {
            var Mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Separaciones_Anular", _connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Separacion", actualizarViewModel.Ident_Separacion);
                    command.Parameters.AddWithValue("@ISIdent_Usuario", loginResult.IdentUsuario);
                    command.Parameters.AddWithValue("@ISMotivoAnulacion", actualizarViewModel.MotivoAnulacion);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        Mensaje = reader["Mensaje"].ToString();
                    }
                    return Mensaje;
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
