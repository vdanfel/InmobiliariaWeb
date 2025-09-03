using DocumentFormat.OpenXml.Wordprocessing;
using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Caja;
using InmobiliariaWeb.Models.Contratos;
using InmobiliariaWeb.Models.Programa;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Caja;
using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Programa;
using InmobiliariaWeb.Result.Separaciones;
using InmobiliariaWeb.Result.Usuario;
using InmobiliariaWeb.Utilities;
using InmobiliariaWeb.ViewModels.Contratos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;

namespace InmobiliariaWeb.Servicios
{
    public class ContratosService: IContratosService
    {
        private readonly SqlConnection _connection;
        private readonly decimal _penalidad;
        public ContratosService(SqlConnection connection,IConfiguration configuration)
        { 
            _connection = connection;
            _penalidad = decimal.Parse(configuration["Penalidad:Penalidad"]);
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
                using (SqlCommand command = new SqlCommand("usp_Manzana_CbxListarContrato", _connection))
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
                using (SqlCommand command = new SqlCommand("usp_Lote_CbxListarContrato", _connection))
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
        public async Task<int> CrearContrato(ViewModels.Contratos.CrearViewModel crearViewModel,LoginResult loginResult)
        {
            int ident_Contrato = 0;
            try 
            {
                using (SqlCommand command = new SqlCommand("sp_Contratos_insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISCorrelativo_Separaciones", string.IsNullOrEmpty(crearViewModel.NumeroSeparacion) ? DBNull.Value : (object)crearViewModel.NumeroSeparacion);
                    command.Parameters.AddWithValue("@ISFechaContrato", crearViewModel.ContratosModels.FechaContrato);
                    command.Parameters.AddWithValue("@ISIdent_Lote", crearViewModel.ContratosModels.Ident_Lote);
                    command.Parameters.AddWithValue("@ISTratadoEn", crearViewModel.ContratosModels.TratadoEn);
                    command.Parameters.AddWithValue("@ISCuotaInicial", crearViewModel.ContratosModels.CuotaInicial);
                    command.Parameters.AddWithValue("@ISSaldoAPagar", crearViewModel.ContratosModels.SaldoAPagar);
                    command.Parameters.AddWithValue("@ISCantidadCuotas", crearViewModel.ContratosModels.CantidadCuotas);
                    command.Parameters.AddWithValue("@ISCuotasIniciales", crearViewModel.ContratosModels.CuotasIniciales);
                    command.Parameters.AddWithValue("@ISCuotaFinal", crearViewModel.ContratosModels.CuotaFinal);
                    command.Parameters.AddWithValue("@ISDiaCuota", crearViewModel.ContratosModels.DiaCuota);
                    command.Parameters.AddWithValue("@ISFechaCuotaInicial", crearViewModel.ContratosModels.FechaCuotaInicial);
                    command.Parameters.AddWithValue("@ISObservacion", string.IsNullOrEmpty(crearViewModel.ContratosModels.Observacion) ? DBNull.Value : (object)crearViewModel.ContratosModels.Observacion);
                    command.Parameters.AddWithValue("@ISFlag_Legalizado", crearViewModel.ContratosModels.Flag_Legalizado);
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
        public async Task ActualizarContrato(int Ident_Contrato, ViewModels.Contratos.ActualizarViewModel actualizarViewModel, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Contratos_Actualizar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Contrato", Ident_Contrato);
                    command.Parameters.AddWithValue("@ISFechaContrato", actualizarViewModel.ContratosModels.FechaContrato);
                    command.Parameters.AddWithValue("@ISTratadoEn", actualizarViewModel.ContratosModels.TratadoEn);
                    command.Parameters.AddWithValue("@ISCuotaInicial", actualizarViewModel.ContratosModels.CuotaInicial);
                    command.Parameters.AddWithValue("@ISSaldoAPagar", actualizarViewModel.ContratosModels.SaldoAPagar);
                    command.Parameters.AddWithValue("@ISCantidadCuotas", actualizarViewModel.ContratosModels.CantidadCuotas);
                    command.Parameters.AddWithValue("@ISCuotasIniciales", actualizarViewModel.ContratosModels.CuotasIniciales);
                    command.Parameters.AddWithValue("@ISCuotaFinal", actualizarViewModel.ContratosModels.CuotaFinal);
                    command.Parameters.AddWithValue("@ISDiaCuota", actualizarViewModel.ContratosModels.DiaCuota);
                    command.Parameters.AddWithValue("@ISFechaCuotaInicial", actualizarViewModel.ContratosModels.FechaCuotaInicial);
                    command.Parameters.AddWithValue("@ISObservacion",
                        string.IsNullOrWhiteSpace(actualizarViewModel.ContratosModels.Observacion) ? (object)DBNull.Value : actualizarViewModel.ContratosModels.Observacion);
                    command.Parameters.AddWithValue("@ISFlag_Legalizado", actualizarViewModel.ContratosModels.Flag_Legalizado);
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
        public async Task<List<CuotasLista>> ListarCuotas(int ident_Kardex,LoginResult loginResult)
        { 
            List<CuotasLista> cuotasListas = new List<CuotasLista> { };
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Cuotas_Listar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Kardex", ident_Kardex);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
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
                        cuotasLista.Ident_015_EstadoPago = Int32.Parse(reader["Ident_015_EstadoPago"].ToString());
                        
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
        public async Task<ViewModels.Contratos.ActualizarViewModel> ContratoxIdentContrato(int ident_Contrato)
        {
            ViewModels.Contratos.ActualizarViewModel actualizarViewModel = new ViewModels.Contratos.ActualizarViewModel();
            ContratosModel contratosModel = new ContratosModel();
            actualizarViewModel.ContratosModels = contratosModel;
            ProgramaModel programaModel = new ProgramaModel();
            actualizarViewModel.ProgramaModels = programaModel;
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
                        actualizarViewModel.ContratosModels.Ident_Contratos = Int32.Parse(reader["Ident_Contratos"].ToString());
                        actualizarViewModel.NumeroSerie = reader["NumeroSerie"].ToString();
                        actualizarViewModel.ContratosModels.FechaContrato = DateTime.Parse(reader["FechaContrato"].ToString());
                        actualizarViewModel.NumeroSeparacion = reader["NumeroSeparcion"].ToString();
                        actualizarViewModel.ProgramaModels.Nombre = reader["NombrePrograma"].ToString();
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
                        actualizarViewModel.ContratosModels.TratadoEn = Decimal.Parse(reader["TratadoEn"].ToString());
                        actualizarViewModel.ContratosModels.CuotaInicial = Decimal.Parse(reader["CuotaInicial"].ToString());
                        actualizarViewModel.ContratosModels.SaldoAPagar = Decimal.Parse(reader["SaldoAPagar"].ToString());
                        actualizarViewModel.ContratosModels.CantidadCuotas = Int32.Parse(reader["CantidadCuotas"].ToString());
                        actualizarViewModel.ContratosModels.CuotasIniciales = Decimal.Parse(reader["CuotasIniciales"].ToString());
                        actualizarViewModel.ContratosModels.CuotaFinal = Decimal.Parse(reader["CuotaFinal"].ToString());
                        actualizarViewModel.ContratosModels.FechaCuotaInicial = DateTime.Parse(reader["FechaCuotaInicial"].ToString());
                        actualizarViewModel.ContratosModels.DiaCuota = Int32.Parse(reader["DiaCuota"].ToString());
                        actualizarViewModel.ContratosModels.Observacion = reader["Observacion"].ToString();
                        actualizarViewModel.ident_010_TipoLote = Int32.Parse(reader["Ident_010_TipoLote"].ToString());
                        actualizarViewModel.ProgramaModels.Ident_017_TipoContrato = Int32.Parse(reader["Ident_017_TipoContrato"].ToString());
                        actualizarViewModel.ContratosModels.EstadoImpresion = (bool)reader["EstadoImpresion"];
                        actualizarViewModel.ContratosModels.KardexCreado = (bool)reader["KardexCreado"];
                        actualizarViewModel.ContratosModels.Flag_Legalizado = (bool)reader["Flag_Legalizado"];

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
        public async Task<List<ClienteList>> ClientesxContrato(int Ident_Contratos)
        {
            var clienteList = new List<ClienteList>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ContratosPersonas_List", _connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Contratos", Ident_Contratos);
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
        public async Task FormatoVentas_Insert(int Ident_Contratos, LoginResult loginResult)
        {
            Ventas ventas = new Ventas();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_FormatoVenta_Insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);

                    await _connection.OpenAsync();

                    await command.ExecuteNonQueryAsync();

                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<VentasContado> FormatoContratoVentasContado(int Ident_Contratos)
        {
            VentasContado ventasContado = new VentasContado();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ImpresionVentaContado", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);
                    await _connection.OpenAsync();

                    // Ejecuta el procedimiento almacenado y obtiene el resultado
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        // Primer resultado: detalles del contrato
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                ventasContado.Titulo = reader["Titulo"].ToString();
                                ventasContado.ParrafoInicial = reader["ParrafoInicial"].ToString();
                                ventasContado.Clausula1 = reader["Clausula1"].ToString();
                                ventasContado.Clausula2 = reader["Clausula2"].ToString();
                                ventasContado.Clausula3I = reader["Clausula3I"].ToString();
                                ventasContado.TextoFrente = reader["TextoFrente"].ToString();
                                ventasContado.TextoDerecha = reader["TextoDerecha"].ToString();
                                ventasContado.TextoIzquierda = reader["TextoIzquierda"].ToString();
                                ventasContado.TextoFondo = reader["TextoFondo"].ToString();
                                ventasContado.Clausula3F = reader["Clausula3F"].ToString();
                                ventasContado.Clausula4 = reader["Clausula4"].ToString();
                                ventasContado.Clausula5 = reader["Clausula5"].ToString();
                                ventasContado.Clausula6 = reader["Clausula6"].ToString();
                                ventasContado.Clausula7 = reader["Clausula7"].ToString();
                                ventasContado.Clausula8 = reader["Clausula8"].ToString();
                                ventasContado.Clausula9 = reader["Clausula9"].ToString();
                                ventasContado.Clausula10 = reader["Clausula10"].ToString();
                                ventasContado.Clausula11 = reader["Clausula11"].ToString();
                                ventasContado.FechaContrato = reader["FechaContrato"].ToString();
                            }
                        }
                    }
                    return ventasContado;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task FormatoTransferencias_Insert(int Ident_Contratos, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_FormatoTransferencia_Insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<KardexViewModel> DatosKardex(int Ident_Kardex)
        {
            KardexViewModel kardex = new KardexViewModel();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_KardexDetalle", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Kardex", Ident_Kardex);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        kardex.Ident_Kardex = Int32.Parse(reader["Ident_Kardex"].ToString());
                        kardex.Ident_Contratos = Int32.Parse(reader["Ident_Contratos"].ToString());
                        kardex.Correlativo = Int32.Parse(reader["Correlativo"].ToString());
                        kardex.ImporteTotal = Decimal.Parse(reader["ImporteTotal"].ToString());
                        kardex.MontoPagado = Decimal.Parse(reader["MontoPagado"].ToString());
                        kardex.SaldoPendiente = Decimal.Parse(reader["SaldoPendiente"].ToString());
                        kardex.CantidadCuotas = Int32.Parse(reader["CantidadCuotas"].ToString());
                        kardex.CuotaActual = Int32.Parse(reader["CuotaActual"].ToString());
                        kardex.ImporteCuotas = Decimal.Parse(reader["ImporteCuotas"].ToString());
                        kardex.ImporteCuotaFinal = Decimal.Parse(reader["ImporteCuotaFinal"].ToString());
                        kardex.TotalMoras = Decimal.Parse(reader["TotalMoras"].ToString());
                        kardex.MontoMorasPagado = Decimal.Parse(reader["MontoMorasPagado"].ToString());
                        kardex.SaldoMorasPendientes = Decimal.Parse(reader["SaldoMorasPendientes"].ToString());
                    }
                    return kardex;
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
        public async Task<int> CrearKardex(ViewModels.Contratos.ActualizarViewModel actualizarViewModel, LoginResult loginResult)
        {
            var ident_Kardex = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Kardex_Insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos",actualizarViewModel.ContratosModels.Ident_Contratos);
                    command.Parameters.AddWithValue("@Penalidad",_penalidad);
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);
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
        public async Task CrearCuotasMasivas(int ident_Kardex, List<Cuotas> cuotas, int usuarioCreacion)
        {
            // Crear el DataTable para las inserciones masivas
            DataTable dataTable = new DataTable();

            // Definir las columnas que se usarán para la inserción
            dataTable.Columns.Add("Ident_Kardex", typeof(int));
            dataTable.Columns.Add("Correlativo", typeof(int));
            dataTable.Columns.Add("FechaPago", typeof(DateTime));
            dataTable.Columns.Add("ImporteCuota", typeof(decimal));
            dataTable.Columns.Add("ImporteCuotaPagado", typeof(decimal));
            dataTable.Columns.Add("DiasMoras", typeof(int)); 
            dataTable.Columns.Add("Ident_015_EstadoPago", typeof(int)); 
            dataTable.Columns.Add("Ident_004_Estado", typeof(int));     
            dataTable.Columns.Add("UsuarioCreacion", typeof(int));      
            dataTable.Columns.Add("FechaCreacion", typeof(DateTime));   
            dataTable.Columns.Add("UsuarioModificacion", typeof(int));  
            dataTable.Columns.Add("FechaModificacion", typeof(DateTime));

            // Llenar el DataTable con los valores
            cuotas.ForEach(cuota =>
            {
                var row = dataTable.NewRow();
                row["Ident_Kardex"] = cuota.Ident_Kardex;
                row["Correlativo"] = cuota.Correlativo;
                row["FechaPago"] = cuota.FechaPago; 
                row["ImporteCuota"] = cuota.ImporteCuota;
                row["ImporteCuotaPagado"] = cuota.ImporteCuotaPagado;
                row["DiasMoras"] = cuota.DiasMoras;
                row["Ident_015_EstadoPago"] = cuota.Ident_015_EstadoPago;
                row["Ident_004_Estado"] = cuota.Ident_004_Estado;
                row["UsuarioCreacion"] = usuarioCreacion;
                row["FechaCreacion"] = cuota.FechaCreacion;
                row["UsuarioModificacion"] = cuota.UsuarioModificacion;
                row["FechaModificacion"] = cuota.FechaModificacion;
                dataTable.Rows.Add(row);
            });

            // Usar SqlBulkCopy para hacer la inserción masiva
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_connection))
            {
                bulkCopy.DestinationTableName = "Cuotas";

                // Mapear las columnas del DataTable con las de la base de datos
                bulkCopy.ColumnMappings.Add("Ident_Kardex", "Ident_Kardex");
                bulkCopy.ColumnMappings.Add("Correlativo", "Correlativo");
                bulkCopy.ColumnMappings.Add("FechaPago", "FechaPago");
                bulkCopy.ColumnMappings.Add("ImporteCuota", "ImporteCuota");
                bulkCopy.ColumnMappings.Add("ImporteCuotaPagado", "ImporteCuotaPagado");
                bulkCopy.ColumnMappings.Add("DiasMoras", "DiasMoras");
                bulkCopy.ColumnMappings.Add("Ident_015_EstadoPago", "Ident_015_EstadoPago");
                bulkCopy.ColumnMappings.Add("Ident_004_Estado", "Ident_004_Estado");
                bulkCopy.ColumnMappings.Add("UsuarioCreacion", "UsuarioCreacion");
                bulkCopy.ColumnMappings.Add("FechaCreacion", "FechaCreacion");
                bulkCopy.ColumnMappings.Add("UsuarioModificacion", "UsuarioModificacion");
                bulkCopy.ColumnMappings.Add("FechaModificacion", "FechaModificacion");

                try
                {
                    await _connection.OpenAsync();
                    await bulkCopy.WriteToServerAsync(dataTable);
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    throw ex;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
        public async Task<Cuotas> CuotasxIdentCuotas(int Ident_Cuotas)
        {
            Cuotas cuotas = new Cuotas();
            try
            {
                using (SqlCommand command = new SqlCommand("USP_Cuotas_xIdent_Cuotas", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Cuotas", Ident_Cuotas);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        cuotas.Correlativo = Int32.Parse(reader["Correlativo"].ToString());
                        cuotas.FechaPago = DateTime.Parse(reader["FechaPago"].ToString());
                        cuotas.ImporteCuota = decimal.Parse(reader["ImporteCuota"].ToString());
                        cuotas.ImporteCuotaPagado = decimal.Parse(reader["ImporteCuotaPagado"].ToString());
                        cuotas.FechaPagoRealizado = reader["FechaPagoRealizado"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(reader["FechaPagoRealizado"].ToString());
                        cuotas.Ident_015_EstadoPago = Int32.Parse(reader["Ident_015_EstadoPago"].ToString());
                        cuotas.Observacion = reader["Observacion"].ToString();
                    }
                    return cuotas;
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
        public async Task<string> CuotasActualizar(Cuotas cuotas, LoginResult loginResult)
        {
            string mensaje = "";
            try 
            {
                using (SqlCommand command = new SqlCommand("USP_cuotas_Actualizar", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Cuotas", cuotas.Ident_Cuotas);
                    command.Parameters.AddWithValue("@FechaPagoRealizado", cuotas.FechaPagoRealizado);
                    command.Parameters.AddWithValue("@ImporteCuotaPagado", cuotas.ImporteCuotaPagado);
                    command.Parameters.AddWithValue("@DiasMoras",
                        cuotas.FechaPagoRealizado.HasValue && cuotas.FechaPagoRealizado > cuotas.FechaPago
                        ? (cuotas.FechaPagoRealizado.Value - cuotas.FechaPago).Days
                        : 0);
                    command.Parameters.AddWithValue("@Observacion", cuotas.Observacion??"");
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);

                    SqlParameter returnValue = new SqlParameter();
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(returnValue);

                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    int resultadoSP = (int)returnValue.Value;

                    if (resultadoSP == 0)
                    {
                        mensaje = "";
                    }
                    else
                    {
                        mensaje = "No se pudo actualizar la cuota. Verifique los datos.";
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Ocurrió un error al intentar actualizar la cuota: {ex.Message}";
            }
            finally
            {
                _connection.Close();
            }
            return mensaje;
        }
        public async Task<string> MorasActualizar(Moras moras, LoginResult loginResult)
        {
            string mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Moras_Actualizar", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Moras", moras.Ident_Moras);
                    command.Parameters.AddWithValue("@DescuentoDirecto", moras.DescuentoDirecto);
                    command.Parameters.AddWithValue("@DescuentoPorcentaje", moras.DescuentoPorcentaje);
                    command.Parameters.AddWithValue("@NuevoMontoMora", moras.NuevoMontoMora);
                    command.Parameters.AddWithValue("@ImporteMorasPagado", moras.ImporteMorasPagado);
                    command.Parameters.AddWithValue("@FechaPagoRealizado", moras.FechaPagoRealizado);
                    command.Parameters.AddWithValue("@Observacion", moras.Observacion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);

                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        mensaje = reader["Mensaje"].ToString();
                    }
                    return mensaje;
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Ocurrió un error al intentar actualizar la cuota: {ex.Message}";
            }
            finally
            {
                _connection.Close();
            }
            return mensaje;
        }
        public async Task<string> RegistrarFormatoImpreso(int Ident_Contratos, string ContratosFormato, LoginResult loginResult)
        {
            string mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ContratosImpresiones_Insert", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);
                    command.Parameters.AddWithValue("@ContratosFormato", ContratosFormato);
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);

                    SqlParameter returnValue = new SqlParameter();
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(returnValue);

                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    int resultadoSP = (int)returnValue.Value;

                    if (resultadoSP == 0)
                    {
                        mensaje = "";
                    }
                    else
                    {
                        mensaje = "No se pudo ingresar los datos, comuniquese con sistemas.";
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Ocurrió un error al intentar ingresar los datos, comuniquese con sistemas: {ex.Message}";
            }
            finally
            {
                _connection.Close();
            }
            return mensaje;
        }
        public async Task<string> ObtenerFormato(int Ident_Contratos)
        {
            string mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("usp_DescargarFormato", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        mensaje = reader["ContratosFormato"].ToString();
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
        public async Task<List<Involucrados>> ObtenerInvolucrados(int Ident_Contratos)
        {
            List<Involucrados> involucrados = new List<Involucrados>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ObtenerInvolucrados", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);
                    await _connection.OpenAsync();

                    // Ejecuta el procedimiento almacenado y obtiene el resultado
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        // Primer resultado: detalles del contrato
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                var involucrado = new Involucrados
                                {
                                    NombreCompleto = reader["NombreCompleto"].ToString(),
                                    NumeroDocumento = reader["NumeroDocumento"].ToString(),
                                    NumeroPartida = reader["NumeroPartida"].ToString(),
                                    TipoPersona = reader["TipoPersona"].ToString(),
                                    Separacion = reader["Separacion"].ToString(),
                                    TipoDocumento = reader["TipoDocumento"].ToString()
                                };
                                involucrados.Add(involucrado);
                            }
                        }
                    }

                    return involucrados;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<string> ObtenerInvolucradosCabecera(int Ident_Contratos)
        {
            var result = "";
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ObtenerInvolucrados_Cabecera", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);
                    await _connection.OpenAsync();

                    // Ejecuta el procedimiento almacenado y obtiene el resultado
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        // Primer resultado: detalles del contrato
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                result = reader["Cabecera"].ToString();
                            }
                        }
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task RecalculoMoras(int Ident_Kardex)
        {
            try 
            {
                using (SqlCommand command = new SqlCommand("usp_Kardex_TotalMoras", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Kardex", Ident_Kardex);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task MorasMasivo(int Ident_Kardex)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Moras_Insert_MasivoInicial", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Kardex", Ident_Kardex);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<int> MoraExiste(int Ident_Cuotas)
        {
            int ident_Moras = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_MorasExiste", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Cuotas", Ident_Cuotas);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        ident_Moras = (int)reader["Ident_Moras"];
                    }
                    return ident_Moras;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<Moras> ObtenerDatosMora(int Ident_Moras)
        {
            Moras moras = new Moras();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Moras_Obtener", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Moras", Ident_Moras);
                    await _connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        // Campos que siempre tienen datos
                        moras.Ident_Moras = (int)reader["Ident_Moras"];
                        moras.Ident_Kardex = (int)reader["Ident_Kardex"];
                        moras.Ident_Cuotas = (int)reader["Ident_Cuotas"];
                        moras.Correlativo = (int)reader["Correlativo"];
                        moras.DiasMoras = (int)reader["DiasMoras"];
                        moras.ImporteMoras = (decimal)reader["ImporteMoras"];
                        moras.FechaPago = (DateTime)reader["FechaPago"];
                        moras.DescuentoDirecto = (decimal)reader["DescuentoDirecto"];
                        moras.DescuentoPorcentaje = (decimal)reader["DescuentoPorcentaje"];
                        moras.NuevoMontoMora = (decimal)reader["NuevoMontoMora"];
                        moras.Ident_015_EstadoPago = (int)reader["Ident_015_EstadoPago"];

                        // Campos que pueden ser nulos
                        moras.FechaPagoRealizado = reader["FechaPagoRealizado"] != DBNull.Value ? (DateTime?)reader["FechaPagoRealizado"] : null;
                        moras.Observacion = reader["Observacion"] != DBNull.Value ? (string)reader["Observacion"] : null;
                        moras.ImporteMorasPagado = reader["ImporteMorasPagado"] != DBNull.Value ? (decimal)reader["ImporteMorasPagado"] : 0;

                    }
                    return moras;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<int> InsertarMoras(int Ident_Cuotas, LoginResult loginResult)
        {
            int Ident_Moras = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Moras_Insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Cuotas", Ident_Cuotas);
                    command.Parameters.AddWithValue("@UsuarioCreacion", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        Ident_Moras = (int)reader["Ident_Moras"];
                    }
                    return Ident_Moras;
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
        public async Task MorasEliminar(int Ident_Cuotas)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Moras_Eliminar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Cuotas", Ident_Cuotas);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<string> AnularContrato(AnularViewModel anularViewModel,LoginResult loginResult)
        {
            try
            {
                string mensaje = "";
                using (SqlCommand command = new SqlCommand("usp_Contratos_Anular", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos", anularViewModel.Actualizar.ContratosModels.Ident_Contratos);
                    command.Parameters.AddWithValue("@MotivoAnulacion", anularViewModel.MotivoAnulacion);
                    command.Parameters.AddWithValue("@DetalleAnulacion", anularViewModel.DetalleAnulacion);
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        mensaje = reader["Mensaje"].ToString();
                    }
                    return mensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public bool FormatoVenta_Existe(int Ident_Contratos)
        {
            const string query = "SELECT COUNT(Ident_FormatoVenta) FROM FormatoVenta WHERE Ident_Contratos = @Ident_Contratos";

            try
            {
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);

                    if (_connection.State != System.Data.ConnectionState.Open)
                    {
                        _connection.Open();
                    }

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                // Re-throwing the exception preserves the stack trace.
                throw new Exception("Error checking if FormatoVenta exists", ex);
            }
            finally
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }

        public async Task<Ventas> FormatoVentas_List(int Ident_Contratos)
        {
            try
            {
                Ventas ventas = new Ventas();
                using (SqlCommand command = new SqlCommand("usp_FormatoVenta_List", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos",Ident_Contratos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        ventas.Ident_FormatoVenta = Int32.Parse(reader["Ident_FormatoVenta"].ToString());
                        ventas.Titulo = reader["Titulo"].ToString();
                        ventas.ParrafoInicial = reader["ParrafoInicial"].ToString();
                        ventas.Clausula1 = reader["Clausula1"].ToString();
                        ventas.Clausula2 = reader["Clausula2"].ToString();
                        ventas.Clausula3 = reader["Clausula3"].ToString();
                        ventas.Clausula4 = reader["Clausula4"].ToString();
                        ventas.Clausula5 = reader["Clausula5"].ToString();
                        ventas.Clausula6 = reader["Clausula6"].ToString();
                        ventas.Clausula7 = reader["Clausula7"].ToString();
                        ventas.Clausula8 = reader["Clausula8"].ToString();
                        ventas.Clausula9 = reader["Clausula9"].ToString();
                        ventas.Clausula10 = reader["Clausula10"].ToString();
                        ventas.Clausula11 = reader["Clausula11"].ToString();
                        ventas.Clausula12 = reader["Clausula12"].ToString();
                        ventas.Clausula13 = reader["Clausula13"].ToString();
                        ventas.Clausula14 = reader["Clausula14"].ToString();
                        ventas.Clausula15 = reader["Clausula15"].ToString();
                        ventas.Clausula16 = reader["Clausula16"].ToString();
                        ventas.Clausula17 = reader["Clausula17"].ToString();
                        ventas.ClausulaAdicional = reader["ClausulaAdicional"].ToString();
                        ventas.ClausulaAllanamiento = reader["ClausulaAllanamiento"].ToString();
                        ventas.ClausulaDesalojo = reader["ClausulaDesalojo"].ToString();
                        ventas.TextoIzquierda = reader["TextoIzquierda"].ToString();
                        ventas.TextoDerecha = reader["TextoDerecha"].ToString();
                        ventas.TextoFrente = reader["TextoFrente"].ToString();
                        ventas.TextoFondo = reader["TextoFondo"].ToString();
                        ventas.FechaContrato = reader["FechaContrato"].ToString();
                    }
                    return ventas;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task FormatoVentas_Update(Ventas ventas, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_FormatoVenta_Update", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_FormatoVenta", ventas.Ident_FormatoVenta);
                    command.Parameters.AddWithValue("@Ident_Contratos", ventas.Ident_Contrato);
                    command.Parameters.AddWithValue("@Titulo", ventas.Titulo);
                    command.Parameters.AddWithValue("@ParrafoInicial", ventas.ParrafoInicial);
                    command.Parameters.AddWithValue("@Clausula1", ventas.Clausula1);
                    command.Parameters.AddWithValue("@Clausula2", ventas.Clausula2);
                    command.Parameters.AddWithValue("@Clausula3", ventas.Clausula3);
                    command.Parameters.AddWithValue("@Clausula4", ventas.Clausula4);
                    command.Parameters.AddWithValue("@Clausula5", ventas.Clausula5);
                    command.Parameters.AddWithValue("@Clausula6", ventas.Clausula6);
                    command.Parameters.AddWithValue("@Clausula7", ventas.Clausula7);
                    command.Parameters.AddWithValue("@Clausula8", ventas.Clausula8);
                    command.Parameters.AddWithValue("@Clausula9", ventas.Clausula9);
                    command.Parameters.AddWithValue("@Clausula10", ventas.Clausula10);
                    command.Parameters.AddWithValue("@Clausula11", ventas.Clausula11);
                    command.Parameters.AddWithValue("@Clausula12", ventas.Clausula12);
                    command.Parameters.AddWithValue("@Clausula13", ventas.Clausula13);
                    command.Parameters.AddWithValue("@Clausula14", ventas.Clausula14);
                    command.Parameters.AddWithValue("@Clausula15", ventas.Clausula15);
                    command.Parameters.AddWithValue("@Clausula16", ventas.Clausula16);
                    command.Parameters.AddWithValue("@Clausula17", ventas.Clausula17);
                    command.Parameters.AddWithValue("@ClausulaAdicional", ventas.ClausulaAdicional);
                    command.Parameters.AddWithValue("@ClausulaAllanamiento", ventas.ClausulaAllanamiento);
                    command.Parameters.AddWithValue("@ClausulaDesalojo", ventas.ClausulaDesalojo);
                    command.Parameters.AddWithValue("@TextoIzquierda", ventas.TextoIzquierda);
                    command.Parameters.AddWithValue("@TextoDerecha", ventas.TextoDerecha);
                    command.Parameters.AddWithValue("@TextoFrente", ventas.TextoFrente);
                    command.Parameters.AddWithValue("@TextoFondo", ventas.TextoFondo);
                    command.Parameters.AddWithValue("@FechaContrato", ventas.FechaContrato);
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task FormatoTransferencias_Update(Transferencias transferencias, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_FormatoTransferencia_Update", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_FormatoTransferencia", transferencias.Ident_FormatoTransferencia);
                    command.Parameters.AddWithValue("@Ident_Contratos", transferencias.Ident_Contrato);
                    command.Parameters.AddWithValue("@Titulo", transferencias.Titulo);
                    command.Parameters.AddWithValue("@ParrafoInicial", transferencias.ParrafoInicial);
                    command.Parameters.AddWithValue("@Clausula1", transferencias.Clausula1);
                    command.Parameters.AddWithValue("@Clausula2", transferencias.Clausula2);
                    command.Parameters.AddWithValue("@Clausula3", transferencias.Clausula3);
                    command.Parameters.AddWithValue("@Clausula4", transferencias.Clausula4);
                    command.Parameters.AddWithValue("@Clausula5", transferencias.Clausula5);
                    command.Parameters.AddWithValue("@Clausula6", transferencias.Clausula6);
                    command.Parameters.AddWithValue("@Clausula7", transferencias.Clausula7);
                    command.Parameters.AddWithValue("@Clausula8", transferencias.Clausula8);
                    command.Parameters.AddWithValue("@Clausula9", transferencias.Clausula9);
                    command.Parameters.AddWithValue("@Clausula10", transferencias.Clausula10);
                    command.Parameters.AddWithValue("@Clausula11", transferencias.Clausula11);
                    command.Parameters.AddWithValue("@Clausula12", transferencias.Clausula12);
                    command.Parameters.AddWithValue("@Clausula13", transferencias.Clausula13);
                    command.Parameters.AddWithValue("@ClausulaAllanamiento", transferencias.ClausulaAllanamiento);
                    command.Parameters.AddWithValue("@ClausulaDesalojo", transferencias.ClausulaDesalojo);
                    command.Parameters.AddWithValue("@TextoIzquierda", transferencias.TextoIzquierda);
                    command.Parameters.AddWithValue("@TextoDerecha", transferencias.TextoDerecha);
                    command.Parameters.AddWithValue("@TextoFrente", transferencias.TextoFrente);
                    command.Parameters.AddWithValue("@TextoFondo", transferencias.TextoFondo);
                    command.Parameters.AddWithValue("@FechaContrato", transferencias.FechaContrato);
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<int> DefinirFormato(int Ident_Contratos)
        {
            try
            {
                int formato = 0;
                using (SqlCommand command = new SqlCommand("usp_DefinirWord", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        formato = Int32.Parse(reader[0].ToString());
                    }
                    return formato;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<Transferencias> FormatoTransferencias_List(int Ident_Contratos)
        {
            try
            {
                Transferencias transferencias = new Transferencias();
                using (SqlCommand command = new SqlCommand("usp_FormatoTransferencias_List", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        transferencias.Ident_FormatoTransferencia = Int32.Parse(reader["Ident_FormatoTransferencia"].ToString());
                        transferencias.Titulo = reader["Titulo"].ToString();
                        transferencias.ParrafoInicial = reader["ParrafoInicial"].ToString();
                        transferencias.Clausula1 = reader["Clausula1"].ToString();
                        transferencias.Clausula2 = reader["Clausula2"].ToString();
                        transferencias.Clausula3 = reader["Clausula3"].ToString();
                        transferencias.Clausula4 = reader["Clausula4"].ToString();
                        transferencias.Clausula5 = reader["Clausula5"].ToString();
                        transferencias.Clausula6 = reader["Clausula6"].ToString();
                        transferencias.Clausula7 = reader["Clausula7"].ToString();
                        transferencias.Clausula8 = reader["Clausula8"].ToString();
                        transferencias.Clausula9 = reader["Clausula9"].ToString();
                        transferencias.Clausula10 = reader["Clausula10"].ToString();
                        transferencias.Clausula11 = reader["Clausula11"].ToString();
                        transferencias.Clausula12 = reader["Clausula12"].ToString();
                        transferencias.Clausula13 = reader["Clausula13"].ToString();
                        transferencias.ClausulaAllanamiento = reader["ClausulaAllanamiento"].ToString();
                        transferencias.ClausulaDesalojo = reader["ClausulaDesalojo"].ToString();
                        transferencias.TextoIzquierda = reader["TextoIzquierda"].ToString();
                        transferencias.TextoDerecha = reader["TextoDerecha"].ToString();
                        transferencias.TextoFrente = reader["TextoFrente"].ToString();
                        transferencias.TextoFondo = reader["TextoFondo"].ToString();
                        transferencias.FechaContrato = reader["FechaContrato"].ToString();
                    }
                    return transferencias;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public bool FormatoTransferencia_Existe(int Ident_Contratos)
        {
            const string query = "SELECT COUNT(Ident_FormatoTransferencia) FROM FormatoTransferencia WHERE Ident_Contratos = @Ident_Contratos";

            try
            {
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);

                    if (_connection.State != System.Data.ConnectionState.Open)
                    {
                        _connection.Open();
                    }

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                // Re-throwing the exception preserves the stack trace.
                throw new Exception("Error checking if FormatoVenta exists", ex);
            }
            finally
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }
        public async Task<IngresosModel> IngresosCabecera(int Ident_Contratos)
        {
            IngresosModel ingresosModel = new IngresosModel();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Lote_IdentsList", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Contratos", Ident_Contratos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        ingresosModel.Ident_Programa = Int32.Parse(reader["Ident_Programa"].ToString());
                        ingresosModel.Ident_Manzana = Int32.Parse(reader["Ident_Manzana"].ToString());
                        ingresosModel.Ident_Lote = Int32.Parse(reader["Ident_Lote"].ToString());
                    }
                    return ingresosModel;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<decimal> MorasMasivo_Total(int Ident_Kardex)
        {
            decimal totalMoras = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_MorasMasivo_Total", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Kardex", Ident_Kardex);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        totalMoras = decimal.Parse(reader["ImporteMorasTotal"].ToString());
                    }
                    return totalMoras;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<ReciboBE> ImprimirRecibo(int nIdent_Ingreso, int nIdent_Persona)
        {
            ReciboBE reciboBE = new ReciboBE();
            try 
            {
                using (SqlCommand command = new SqlCommand("usp_ImprimirRecibo", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Ingreso", nIdent_Ingreso);
                    command.Parameters.AddWithValue("@nIdent_Persona", nIdent_Persona);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        reciboBE.NombrePrograma = reader["NombrePrograma"].ToString();
                        reciboBE.NumeroRecibo = reader["NumeroRecibo"].ToString();
                        reciboBE.FechaPago = DateTime.Parse(reader["FechaPago"].ToString());
                        reciboBE.NombreCompleto = reader["NombreCompleto"].ToString();
                        reciboBE.Documento = reader["Documento"].ToString();
                        reciboBE.Observacion = reader["Observacion"].ToString();
                        reciboBE.ImporteTotal = decimal.Parse(reader["ImporteTotal"].ToString());
                        reciboBE.NombreUsuario = reader["NombreUsuario"].ToString();
                        reciboBE.TipoCambio = decimal.Parse(reader["TipoCambio"].ToString());
                        reciboBE.Moneda = reader["Moneda"].ToString();
                    }
                    return reciboBE;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task MoraMasivoPago(MorasMasivoPagoDTO morasMasivoPagoDTO, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_PagoMorasMasivo", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Kardex", morasMasivoPagoDTO.Ident_Kardex);
                    command.Parameters.AddWithValue("@DescuentoPorcentaje", morasMasivoPagoDTO.DescuentoPorcentaje);
                    command.Parameters.AddWithValue("@DescuentoDirectoTotal", morasMasivoPagoDTO.DescuentoDirectoTotal);
                    command.Parameters.AddWithValue("@UsuarioModificacion", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<List<ClienteCbxList>> ClienteCbxListar(int Ident_Lote)
        {
            var clienteCbxList = new List<ClienteCbxList>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ClientesXLoteList", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Lote", Ident_Lote);
                    await _connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var cliente = new ClienteCbxList();
                        cliente.nIdent_Persona = Int32.Parse(reader["nIdent_Persona"].ToString());
                        cliente.sNombreCompleto = reader["sNombreCompleto"].ToString();
                        clienteCbxList.Add(cliente);
                    }
                    return clienteCbxList;
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
