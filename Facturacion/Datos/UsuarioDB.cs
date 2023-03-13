using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace Datos
{
    public class UsuarioDB
    {
        string cadena = "server=localhost; user=root; database=factura2; password=123456;";

        //METODO AUTENTICAR
        public Usuario Autenticar(Login login)
        {
            Usuario user = null;
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM usuario WHERE CodigoUsuario = @CodigoUsuario AND Contrasena = @Contrasena;");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;

                        comando.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = login.CodigoUsuario;
                        comando.Parameters.Add("@Contrasena", MySqlDbType.VarChar, 50).Value = login.Contraseña;


                        MySqlDataReader dr = comando.ExecuteReader();


                        if (dr.Read())
                        {
                            user = new Usuario();

                            user.CodigoUsuario = dr["CodigoUsuario"].ToString();
                            user.Nombre = dr["Nombre"].ToString();
                            user.Contraseña = dr["Contrasena"].ToString();
                            user.Correo = dr["Correo"].ToString();
                            user.Rol = dr["Rol"].ToString();
                            user.FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]);
                            user.EstaActivo = Convert.ToBoolean(dr["EstaActivo"]);


                            if (dr["Foto"].GetType() != typeof(DBNull))
                            {
                                user.Foto = (byte[])dr["Foto"];
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
            return user;
        }


        //METODO PARA INSERTAR USUARIO
        public bool Insertar(Usuario user)
        {

            bool inserto = false;

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" INSERT INTO usuario VALUES ");
                sql.Append(" (@CodigoUsuario, @Nombre, @Contrasena, @Correo, @Rol, @Foto, @FechaCreacion, @EstaActivo); ");


                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;

                        comando.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = user.CodigoUsuario;
                        comando.Parameters.Add("@Nombre", MySqlDbType.VarChar, 50).Value = user.Nombre;
                        comando.Parameters.Add("@Contrasena", MySqlDbType.VarChar, 80).Value = user.Contraseña;
                        comando.Parameters.Add("@Correo", MySqlDbType.VarChar, 45).Value = user.Correo;
                        comando.Parameters.Add("@Rol", MySqlDbType.VarChar, 20).Value = user.Rol;
                        comando.Parameters.Add("@Foto", MySqlDbType.LongBlob).Value = user.Foto;
                        comando.Parameters.Add("@FechaCreacion", MySqlDbType.DateTime).Value = user.FechaCreacion;
                        comando.Parameters.Add("@EstaActivo", MySqlDbType.Bit).Value = user.EstaActivo;
                        comando.ExecuteNonQuery();

                        inserto = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return inserto;
        }


        //METODO PARA EDITAR UN USUARIO
        public bool Editar(Usuario user)
        {
            bool edito = false;


            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" UPDATE usuario SET ");
                sql.Append(" Nombre = @Nombre, Contrasena = @Contrasena, Correo = @Correo, Rol = @Rol, Foto = @Foto, EstaActivo = @EstaActivo ");

                sql.Append(" WHERE CodigoUsuario = @CodigoUsuario; ");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = user.CodigoUsuario;
                        comando.Parameters.Add("@Nombre", MySqlDbType.VarChar, 50).Value = user.Nombre;
                        comando.Parameters.Add("@Contrasena", MySqlDbType.VarChar, 80).Value = user.Contraseña;
                        comando.Parameters.Add("@Correo", MySqlDbType.VarChar, 45).Value = user.Correo;
                        comando.Parameters.Add("@Rol", MySqlDbType.VarChar, 20).Value = user.Rol;
                        comando.Parameters.Add("@Foto", MySqlDbType.LongBlob).Value = user.Foto;
                        comando.Parameters.Add("@EstaActivo", MySqlDbType.Bit).Value = user.EstaActivo;
                        comando.ExecuteNonQuery();
                        edito = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return edito;
        }


        //METODO PARA ELIMINAR UN USUARIO o REGISTRO
        public bool Eliminar(string codigoUsuario)
        {
            bool elimino = false;
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" DELETE FROM usuario ");
                sql.Append(" WHERE CodigoUsuario = @CodigoUsuario; ");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = codigoUsuario;
                        comando.ExecuteNonQuery();
                        elimino = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return elimino;
        }


        //METODO PARA TRAER TODO LOS USUARIOS Y PODER VISUALIZARLOS EN EL DATAGRID
        public DataTable DevolverUsuarios()
        {
            DataTable dt = new DataTable();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT * FROM usuario ");
                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        MySqlDataReader dr = comando.ExecuteReader();
                        dt.Load(dr);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return dt;
        }


        //METODO PARA SELECCIONAR O TRAER LA IMAGEN DEL USUARIO
        public byte[] DevolverFoto(string codigoUsuario)
        {
            byte[] foto = new byte[0];
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT Foto FROM usuario WHERE CodigoUsuario = @CodigoUsuario;");
                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;

                        comando.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = codigoUsuario;
                        MySqlDataReader dr = comando.ExecuteReader();

                        if (dr.Read())
                        {
                            foto = (byte[])dr["Foto"];
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return foto;
        }
    }
}
