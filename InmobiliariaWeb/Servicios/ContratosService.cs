using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Contratos;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Programa;
using InmobiliariaWeb.Result.Separaciones;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace InmobiliariaWeb.Servicios
{
    public class ContratosService: IContratosService
    {
        private readonly SqlConnection _connection;
        public ContratosService(SqlConnection connection)
        { 
            _connection = connection;
        }
        public async Task<List<ContratosList>> BandejaContratos(IndexViewModel indexViewModel)
        {
            var contratos = new List<ContratosList>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_CONTRATOS_BANDEJA", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISCorrelativo", indexViewModel.Correlativo);
                    command.Parameters.AddWithValue("@ISIdent_Programa", indexViewModel.Ident_Programa);
                    command.Parameters.AddWithValue("@ISIdent_Manzana", indexViewModel.Ident_Manzana);
                    command.Parameters.AddWithValue("@ISCliente", indexViewModel.Cliente ?? "");
                    command.Parameters.AddWithValue("@ISPaginaActual", indexViewModel.PaginaActual);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var contratosList = new ContratosList();
                        contratosList.Indice = Int32.Parse(reader["INDICE"].ToString());
                        contratosList.Ident_Contratos = Int32.Parse(reader["IDENT_CONTRATOS"].ToString());
                        contratosList.NumeroSerie = reader["NUMERO_SERIE"].ToString();
                        contratosList.Cliente = reader["NOMBRE_CLIENTE"].ToString();
                        contratosList.Nombreprograma = reader["NOMBRE_PROGRAMA"].ToString();
                        contratosList.Manzana = reader["MANZANA"].ToString();
                        contratosList.Lote = Int32.Parse(reader["LOTE"].ToString());
                        contratos.Add(contratosList);
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
                    return contratos;
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
                using (SqlCommand command = new SqlCommand("SP_Manzana_CbxListar", _connection))
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
                using (SqlCommand command = new SqlCommand("SP_Lotes_CbxListar_Contratos", _connection))
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
        public async Task<int> CrearContrato(CrearViewModel crearViewModel,LoginResult loginResult)
        {
            int ident_Contrato = 0;
            try 
            {
                using (SqlCommand command = new SqlCommand("sp_Contratos_insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISCorrelativo_Separaciones", string.IsNullOrEmpty(crearViewModel.NumeroSeparacion) ? DBNull.Value : (object)crearViewModel.NumeroSeparacion);
                    command.Parameters.AddWithValue("@ISFechaContrato", crearViewModel.FechaContrato);
                    command.Parameters.AddWithValue("@ISIdent_Lote", crearViewModel.Ident_Lote);
                    command.Parameters.AddWithValue("@ISTratadoEn", crearViewModel.TratadoEn);
                    command.Parameters.AddWithValue("@ISCuotaInicial", crearViewModel.CuotaInicial);
                    command.Parameters.AddWithValue("@ISSaldoAPagar", crearViewModel.SaldoAPagar);
                    command.Parameters.AddWithValue("@ISCantidadCuotas", crearViewModel.CantidadCuotas);
                    command.Parameters.AddWithValue("@ISCuotasIniciales", crearViewModel.CuotasIniciales);
                    command.Parameters.AddWithValue("@ISCuotaFinal", crearViewModel.CuotaFinal);
                    command.Parameters.AddWithValue("@ISFechaCuotaInicial", crearViewModel.FechaCuotaInicial);
                    command.Parameters.AddWithValue("@ISObservacion", string.IsNullOrEmpty(crearViewModel.Observacion) ? DBNull.Value : (object)crearViewModel.Observacion);
                    command.Parameters.AddWithValue("@ISUsuarioCreacion", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        ident_Contrato = Int32.Parse(reader["IDENT_CONTRATOS"].ToString());
                    }

                    return ident_Contrato;
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
        public async Task ActualizarContrato(int Ident_Contrato, ActualizarViewModel actualizarViewModel, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Contratos_Actualizar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Contrato", Ident_Contrato);
                    command.Parameters.AddWithValue("@ISTratadoEn", actualizarViewModel.TratadoEn);
                    command.Parameters.AddWithValue("@ISCuotaInicial", actualizarViewModel.CuotaInicial);
                    command.Parameters.AddWithValue("@ISSaldoAPagar", actualizarViewModel.SaldoAPagar);
                    command.Parameters.AddWithValue("@ISCantidadCuotas", actualizarViewModel.CantidadCuotas);
                    command.Parameters.AddWithValue("@ISCuotasIniciales", actualizarViewModel.CuotasIniciales);
                    command.Parameters.AddWithValue("@ISCuotaFinal", actualizarViewModel.CuotaFinal);
                    command.Parameters.AddWithValue("@ISFechaCuotaInicial", actualizarViewModel.FechaCuotaInicial);
                    command.Parameters.AddWithValue("@ISObservacion", actualizarViewModel.Observacion);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
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
        public async Task<Contrato_xNumeroSeparacion> ObtenerxSeparacion(int numeroSeparacion)
        {
            Contrato_xNumeroSeparacion contrato_xNumeroSeparacion = new Contrato_xNumeroSeparacion();
            try 
            {
                using (SqlCommand command = new SqlCommand("usp_Contrato_x_Separacion", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISCorrelativo", numeroSeparacion);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        contrato_xNumeroSeparacion.Ident_Programa = Int32.Parse(reader["Ident_Programa"].ToString());
                        contrato_xNumeroSeparacion.NombrePrograma = reader["NombrePrograma"].ToString();
                        contrato_xNumeroSeparacion.Ident_Manzana = Int32.Parse(reader["Ident_Manzana"].ToString());
                        contrato_xNumeroSeparacion.Manzana = reader["Manzana"].ToString();
                        contrato_xNumeroSeparacion.Ident_Lote = Int32.Parse(reader["Ident_Lote"].ToString());
                        contrato_xNumeroSeparacion.Lote = Int32.Parse(reader["Lote"].ToString());
                        contrato_xNumeroSeparacion.TratadoEn = Decimal.Parse(reader["TratadoEn"].ToString());
                        contrato_xNumeroSeparacion.CuotaInicial = Decimal.Parse(reader["CuotaInicial"].ToString());
                        contrato_xNumeroSeparacion.CantidadCuotas = Int32.Parse(reader["CantidadCuotas"].ToString());
                    }
                    return contrato_xNumeroSeparacion;
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
        public async Task InsertarClientesxSeparacion(string correlativo,int ident_Contrato, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ContratosPersonasXSeparacion", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISCorrelativo", correlativo);
                    command.Parameters.AddWithValue("@ISIdent_Contratos", ident_Contrato);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();

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
        public async Task<int> CrearKardex(int ident_Contratos, LoginResult loginResult)
        {
            var ident_Kardex = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Kardex_Insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Contratos", ident_Contratos);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        ident_Kardex= Int32.Parse(reader["Ident_Kardex"].ToString());
                    }
                    return ident_Kardex;
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
        public async Task CrearCuotas(int ident_Kardex, int correlativo, DateTime fechaPago, decimal ImporteCuota, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Cuotas_Insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Kardex", ident_Kardex);
                    command.Parameters.AddWithValue("@ISCorrelativo", correlativo);
                    command.Parameters.AddWithValue("@ISFechaPago", fechaPago);
                    command.Parameters.AddWithValue("@ISImporteCuota", ImporteCuota);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
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
        public async Task<List<CuotasLista>> ListarCuotas(int ident_Kardex)
        { 
            List<CuotasLista> cuotasListas = new List<CuotasLista> { };
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Cuotas_Listar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Kardex", ident_Kardex);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var cuotasLista = new CuotasLista();
                        cuotasLista.Ident_Cuotas = Int32.Parse(reader["Ident_Cuotas"].ToString());
                        cuotasLista.Correlativo = Int32.Parse(reader["Correlativo"].ToString());
                        cuotasLista.FechaPago = DateTime.Parse(reader["FechaPago"].ToString());
                        cuotasLista.ImporteCuota = Decimal.Parse(reader["ImporteCuota"].ToString());

                        // Verificar si los campos pueden ser nulos antes de intentar convertirlos
                        cuotasLista.FechaPagoRealizado = reader["FechaPagoRealizado"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(reader["FechaPagoRealizado"].ToString());
                        cuotasLista.ImporteCuotaPagado = Decimal.Parse(reader["ImporteCuotaPagado"].ToString());
                        cuotasLista.DiasMoras = Int32.Parse(reader["DiasMoras"].ToString());
                        cuotasLista.ImporteMoras = Decimal.Parse(reader["ImporteMoras"].ToString());

                        // Verificar si los campos pueden ser nulos antes de intentar convertirlos
                        cuotasLista.FechaMorasPagadas = reader["FechaMorasPagadas"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(reader["FechaMorasPagadas"].ToString());
                        cuotasLista.ImporteMorasPagadas = Decimal.Parse(reader["ImporteMorasPagadas"].ToString());

                        cuotasListas.Add(cuotasLista);
                    }

                    return cuotasListas;
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
        public async Task<ActualizarViewModel> ContratoxIdentContrato(int ident_Contrato)
        {
            ActualizarViewModel actualizarViewModel = new ActualizarViewModel();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Contrato_xIdent_Contrato", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Contrato", ident_Contrato);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        actualizarViewModel.NumeroSerie = reader["NumeroSerie"].ToString();
                        actualizarViewModel.FechaContrato = DateTime.Parse(reader["FechaContrato"].ToString());
                        actualizarViewModel.NumeroSeparcion = reader["NumeroSeparcion"].ToString();
                        actualizarViewModel.NombrePrograma = reader["NombrePrograma"].ToString();
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
                        actualizarViewModel.L1 = Decimal.Parse(reader["L1"].ToString());
                        actualizarViewModel.L2 = Decimal.Parse(reader["L2"].ToString());
                        actualizarViewModel.L3 = Decimal.Parse(reader["L3"].ToString());
                        actualizarViewModel.L4 = Decimal.Parse(reader["L4"].ToString());
                        actualizarViewModel.L5 = Decimal.Parse(reader["L5"].ToString());
                        actualizarViewModel.L6 = Decimal.Parse(reader["L6"].ToString());
                        actualizarViewModel.L7 = Decimal.Parse(reader["L7"].ToString());
                        actualizarViewModel.L8 = Decimal.Parse(reader["L8"].ToString());
                        actualizarViewModel.L9 = Decimal.Parse(reader["L9"].ToString());
                        actualizarViewModel.L10 = Decimal.Parse(reader["L10"].ToString());
                        actualizarViewModel.TratadoEn = Decimal.Parse(reader["TratadoEn"].ToString());
                        actualizarViewModel.CuotaInicial = Decimal.Parse(reader["CuotaInicial"].ToString());
                        actualizarViewModel.SaldoAPagar = Decimal.Parse(reader["SaldoAPagar"].ToString());
                        actualizarViewModel.CantidadCuotas = Int32.Parse(reader["CantidadCuotas"].ToString());
                        actualizarViewModel.CuotasIniciales = Decimal.Parse(reader["CuotasIniciales"].ToString());
                        actualizarViewModel.CuotaFinal = Decimal.Parse(reader["CuotaFinal"].ToString());
                        actualizarViewModel.FechaCuotaInicial = DateTime.Parse(reader["FechaCuotaInicial"].ToString());
                        actualizarViewModel.Observacion = reader["Observacion"].ToString();
                        actualizarViewModel.ident_010_TipoLote = Int32.Parse(reader["Ident_010_TipoLote"].ToString());
                        actualizarViewModel.Ident_017_TipoContrato = Int32.Parse(reader["Ident_017_TipoContrato"].ToString());
                        actualizarViewModel.EstadoImpresion = (bool)reader["EstadoImpresion"];
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
        public async Task<int> IdentKardexXIdentContrato(int Ident_Contrato)
        {
            int ident_Kardex = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Kardex_xIdent_Contrato", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Contratos", Ident_Contrato);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        ident_Kardex = Int32.Parse(reader["Ident_Kardex"].ToString());
                    }
                    return ident_Kardex;
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
        public async Task<List<ClienteList>> ClientesxContrato(ClienteViewModel clienteViewModel)
        {
            var clienteList = new List<ClienteList>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ContratosPersonas_List", _connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Contratos", clienteViewModel.Ident_Contratos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var clientes = new ClienteList();
                        clientes.Indice = Int32.Parse(reader["Indice"].ToString());
                        clientes.Ident_ContratosPersonas = Int32.Parse(reader["Ident_ContratosPersonas"].ToString());
                        clientes.Ident_Contratos = Int32.Parse(reader["Ident_Contratos"].ToString());
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
        public async Task<string> ClienteInsertar(ClienteViewModel clienteViewModel, LoginResult loginResult)
        {
            var Mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ContratosPersonas_Insert", _connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Contratos", clienteViewModel.Ident_Contratos);
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
        public async Task<string> ClienteEliminar(int Ident_ContratosCliente, LoginResult loginResult)
        {
            var Mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ContratosPersonas_Delete", _connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ContratosCliente", Ident_ContratosCliente);
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
        public async Task EstadoImpresion(int Ident_Contratos)
        {
            try 
            {
                using (SqlCommand command = new SqlCommand("usp_Contratos_EstadoImpresion", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Contratos", Ident_Contratos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
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
        public async Task<ImpresionContrato> ImprimirContrato(int Ident_Contratos)
        {
            ImpresionContrato impresionContrato = new ImpresionContrato();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ContratoImpresion", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Contrato", Ident_Contratos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén los resultados
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    //obtener el nombre del formato de impresion
                    while (reader.Read()) 
                    {
                        impresionContrato.NombreFormatoImpresion = reader["NombreFormatoImpresion"].ToString();
                    }

                    reader.NextResult();
                    // Llenar la lista de propietarios
                    impresionContrato.Propietarios = new List<TablaPropietarios>();
                    while (reader.Read())
                    {
                        TablaPropietarios propietario = new TablaPropietarios();
                        propietario.Honorifico_Vendedor = reader["Honorifico_Vendedor"].ToString();
                        propietario.Nombre_Vendedor = reader["Nombre_Vendedor"].ToString();
                        propietario.Nacionalidad_Vendedor = reader["Nacionalidad_Vendedor"].ToString();
                        propietario.Identifica_Vendedor = reader["Identifica_Vendedor"].ToString();
                        propietario.Tipo_Documento_Vendedor = reader["Tipo_Documento_Vendedor"].ToString();
                        propietario.Documento_Vendedor = reader["Documento_Vendedor"].ToString();
                        propietario.Estado_Civil_Vendedor = reader["Estado_Civil_Vendedor"].ToString();
                        propietario.Direccion_Vendedor = reader["Direccion_Vendedor"].ToString();
                        propietario.Distrito_Vendedor = reader["Distrito_Vendedor"].ToString();
                        propietario.Provincia_Vendedor = reader["Provincia_Vendedor"].ToString();
                        propietario.Departamento_Vendedor = reader["Departamento_Vendedor"].ToString();
                        // Asignar el resto de propiedades...
                        impresionContrato.Propietarios.Add(propietario);
                    }

                    reader.NextResult();
                    // Llenar la lista de clientes
                    impresionContrato.Clientes = new List<TablaClientes>();
                    while (reader.Read())
                    {
                        TablaClientes cliente = new TablaClientes();
                        cliente.Honorifico_Comprador = reader["Honorifico_Comprador"].ToString();
                        cliente.Nombre_Comprador = reader["Nombre_Comprador"].ToString();
                        cliente.Denominacion_Comprador = reader["Denominacion_Comprador"].ToString();
                        impresionContrato.Clientes.Add(cliente);
                    }

                    reader.NextResult();
                    // Llenar la lista de cláusulas
                    impresionContrato.Clausulas = new List<TablaParrafos>();
                    while (reader.Read())
                    {
                        TablaParrafos clausula = new TablaParrafos();
                        clausula.Correlativo = int.Parse(reader["Correlativo"].ToString());
                        clausula.Detalle = reader["Detalle"].ToString();
                        impresionContrato.Clausulas.Add(clausula);
                    }

                    // Agregar otras asignaciones si es necesario

                    return impresionContrato;
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
        public async Task<Ventas> FormatoContratoVentas(int Ident_Contratos)
        { 
            Ventas ventas = new Ventas();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ImpresionVenta", _connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        ventas.Titulo = reader["Titulo"].ToString();
                        ventas.ParrafoInicial = reader["ParrafoInicial"].ToString();
                    }
                    return ventas;
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
