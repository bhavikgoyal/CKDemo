using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DB_con
{
    #region "enum"
    public enum DBTrans
    {
        Insert,
        Update
    }
    #endregion

    public class ConnectionCls
    {
        #region "variable"
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        SqlTransaction trans;
        #endregion


        public ConnectionCls()
        {
            //
            // TODO: Add constructor logic here
            //
            createConnection();
            setconnection();
        }

        public void closeConnection()
        {
            try
            {
                conn.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void createConnection()
        {
            try
            {
                string str = WebConfigurationManager.ConnectionStrings["sqlconstr"].ConnectionString;
                conn = new SqlConnection(str);
                cmd = new SqlCommand();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void createConnection(Boolean mybool)
        {
            try
            {
                string str = WebConfigurationManager.ConnectionStrings["sqlconstr"].ConnectionString;
                conn = new SqlConnection(str);
                cmd = new SqlCommand();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void setconnection()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void clearParameter()
        {
            try
            {
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void addParameter(string name, object value)
        {
            try
            {
                cmd.Parameters.AddWithValue(name, value);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void addParameter(string name, long value, DB_con.DBTrans trans)
        {
            try
            {
                if (trans == DBTrans.Insert)
                {
                    cmd.Parameters.AddWithValue(name, value);
                    cmd.Parameters[name].Direction = ParameterDirection.Output;
                }
                else if (trans == DBTrans.Update)
                    cmd.Parameters.AddWithValue(name, value);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void addParameter(string name, string value, DB_con.DBTrans trans)
        {
            try
            {
                if (trans == DBTrans.Insert)
                {
                    cmd.Parameters.AddWithValue(name, value);
                    //cmd.Parameters[name].Direction = ParameterDirection.Output;
                }
                else if (trans == DBTrans.Update)
                    cmd.Parameters.AddWithValue(name, value);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public object getValue(string name)
        {
            object parameter = null;
            try
            {
                parameter = cmd.Parameters[name].Value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return parameter;
        }

        public void ExecuteNoneQuery(string commandText, CommandType commandType)
        {
            try
            {
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                setconnection();
                cmd.Connection = conn;

                //cmd.Parameters.Add("@RETVAL", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                //if (cmd.Parameters["@RETVAL"].Value.ToString() != "0" || cmd.Parameters["@RETVAL"].Value.ToString() != string.Empty)
                //{
                //    return Convert.ToUInt64(cmd.Parameters["@RETVAL"].Value.ToString());
                //}
                //else
                //{
                //    return 0;
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            try
            {
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                cmd.Connection = conn;
                setconnection();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// starts Transaction
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    cmd.Transaction = trans;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Commits Transaction
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                if (trans != null)
                {
                    trans.Commit();
                    conn.Close();
                    trans = null;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// RollBacks Transaction
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                if (trans != null)
                {
                    trans.Rollback();
                    conn.Close();
                    trans = null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }
}