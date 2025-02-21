using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Programa;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Programa;
using InmobiliariaWeb.Result.Usuario;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace InmobiliariaWeb.Servicios
{
    public class UsuarioService: IUsuarioService
    {
        private readonly SqlConnection _connection;
        public UsuarioService(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<List<UsuarioList>> ListarUsuario(string buscar)
        {
            var usuarioList = new List<UsuarioList>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_USUARIO_BANDEJA", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISBuscar", buscar ?? "");
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var usuario = new UsuarioList();
                        usuario.Indice = Int32.Parse(reader["INDICE"].ToString());
                        usuario.Ident_Usuario = Int32.Parse(reader["IDENT_USUARIO"].ToString());
                        usuario.IdentPersona = Int32.Parse(reader["IDENT_PERSONA"].ToString());
                        usuario.Nombre = reader["NOMBRE_COMPLETO"].ToString();
                        usuario.Usuario = reader["USUARIO"].ToString();
                        usuario.Clave = reader["CLAVE"].ToString();
                        usuario.Ident_005_TipoUsuario = Int32.Parse(reader["IDENT_005_TIPOUSUARIO"].ToString());
                        usuario.TipoUsuario = reader["TIPOUSUARIO"].ToString();
                        usuarioList.Add(usuario);
                    }
                    return usuarioList;
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
        public async Task<UsuarioList> ListarUsuario_xIdentUsuario(int IdentUsuario)
        {
            var usuario = new UsuarioList();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_USUARIO_Ident_Usuario", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Usuario", IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        usuario.Ident_Usuario = Int32.Parse(reader["IDENT_USUARIO"].ToString());
                        usuario.IdentPersona = Int32.Parse(reader["IDENT_PERSONA"].ToString());
                        usuario.Nombre = reader["NOMBRE_COMPLETO"].ToString();
                        usuario.Usuario = reader["USUARIO"].ToString();
                        usuario.Documento = reader["DOCUMENTO"].ToString();
                        usuario.Clave = reader["CLAVE"].ToString();
                        usuario.Ident_005_TipoUsuario = Int32.Parse(reader["IDENT_005_TIPOUSUARIO"].ToString());
                        usuario.TipoUsuario = reader["TIPOUSUARIO"].ToString();
                    }
                    return usuario;
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
        public async Task<int> RegistrarUsuario(UsuarioList usuarioList, LoginResult loginResult)
        {
            var claveEncriptada = EncriptarClave(usuarioList.Clave);
            int Ident_Usuario = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("SP_USUARIO_Registrar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Persona", usuarioList.IdentPersona);
                    command.Parameters.AddWithValue("@ISIdent_005_TipoUsuario", usuarioList.Ident_005_TipoUsuario);
                    command.Parameters.AddWithValue("@ISUsuario",usuarioList.Usuario);
                    command.Parameters.AddWithValue("@ISClave",claveEncriptada);
                    command.Parameters.AddWithValue("@ISUsuarioCreacion",loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        Ident_Usuario = Int32.Parse(reader["IDENT_USUARIO"].ToString());
                    }
                }
                return Ident_Usuario;
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
        
        public async Task<string> ActualizarUsuario(UsuarioList usuarioList, LoginResult loginResult, int tipoActualizar)
        {
            var claveEncriptada = EncriptarClave(usuarioList.Clave);
            var claveEncriptada1 = EncriptarClave(usuarioList.Clave1);
            
            string mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("SP_USUARIO_Actualizar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Usuario", usuarioList.Ident_Usuario);
                    command.Parameters.AddWithValue("@ISIdent_Persona", usuarioList.IdentPersona);
                    command.Parameters.AddWithValue("@ISIdent_005_TipoUsuario", usuarioList.Ident_005_TipoUsuario);
                    command.Parameters.AddWithValue("@ISUsuario", usuarioList.Usuario ?? "");
                    command.Parameters.AddWithValue("@ISClave", claveEncriptada ?? "");
                    command.Parameters.AddWithValue("@ISClave1", claveEncriptada1 ?? "");
                    command.Parameters.AddWithValue("@ISUsuarioCreacion", loginResult.IdentUsuario);
                    command.Parameters.AddWithValue("@ISTipoActualizacion", tipoActualizar);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        mensaje = reader["MENSAJE"].ToString();
                    }
                }
                return mensaje;
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
        public async Task<string> AnularUsuario(UsuarioList usuarioList, LoginResult loginResult)
        {
            string mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("SP_USUARIO_Anular", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Usuario", usuarioList.Ident_Usuario);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        mensaje = reader["MENSAJE"].ToString();
                    }
                }
                return mensaje;
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
        private string EncriptarClave(string password)
        {
            if (password == null)
            {
                return password;
            }
            else
            {
                using (var md5 = MD5.Create())
                {
                    byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }

                    // Retorna un string hexadecimal
                    return sBuilder.ToString();
                }
            }

        }
        
    }
}
