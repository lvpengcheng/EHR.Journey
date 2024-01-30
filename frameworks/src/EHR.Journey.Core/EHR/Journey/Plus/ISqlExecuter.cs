using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EHR.Journey.Core
{
    public interface ISqlExecuter<D> : ITransientDependency
    {
        void AddRange<TEntity>(IList<TEntity> ntities) where TEntity : class;
        void BulkDelete<TEntity>(IList<TEntity> ntities) where TEntity : class;
        void BulkDeleteAsync<TEntity>(IList<TEntity> ntities) where TEntity : class;
        void BulkInsert<TEntity>(IList<TEntity> ntities) where TEntity : class;
        void BulkInsertAsync<TEntity>(IList<TEntity> ntities) where TEntity : class;
        void BulkMerge<TEntity>(IList<TEntity> ntities) where TEntity : class;
        void BulkMergeAsync<TEntity>(IList<TEntity> ntities) where TEntity : class;
        void BulkUpdate<TEntity>(IList<TEntity> ntities) where TEntity : class;
        void BulkUpdateAsync<TEntity>(IList<TEntity> ntities) where TEntity : class;
        List<T> DataTableToEntity<T>(DataTable dt);
        int Execute(string sql);
        int Execute(string sql, int connectionTimeout = 0);
        int Execute(string sql, params object[] parameters);
        DataSet ExecuteDataSet(string sql, object param = null);
        DataSet ExecuteDataSet(string sql, params object[] parameters);
        Task<DataSet> ExecuteDataSetAsync(List<string> sql, object parameters);
        Task<DataTable> ExecuteDataTableAsync(DbCommand cmd);
        Task<DataTable> ExecuteDataTableAsync(string sql, object parameters);
        int ExecuteNonQuery(string sql, object param = null);
        int ExecuteNonQueryByCommand(string sql, DbParameter[] cmdParms);
        DataTable ExecuteProcedure(string storeProcedureText, object param = null);
        Task<DataSet> ExecuteProcedureSetAsync(string storeProcedureText, object param = null);
        Task<DataTable> ExecuteProcedureAsync(string storeProcedureText, object param = null);
        DataSet ExecuteProcedureDataSet(string storeProcedureText, object param = null);
        int ExecuteProcedureNonQuery(string storeProcedureText, object param = null);

        Task<int> ExecuteNonQueryAsync(string sql, object parameters = null);
        int ExecuteProcedureNonQuery(string cmdText, DbParameter[] cmdParms);
        object ExecuteScalar(CommandType cmdType, string cmdText, DbParameter[] cmdParms);
        DataTable ExecuteSql(string cmdText);
        int ExecuteSqlTextNonQuery(string sql, object param = null);
        DataTable ExecuteTable(string sql, object param = null);
        DataTable ExecuteTable(string sql, params object[] parameters);
        D GetDbContext();
        IQueryable<T> SqlQuery<T>(string sql) where T : BaseEntity<T>;
        IQueryable<T> SqlQuery<T>(string sql, params object[] parameters) where T : BaseEntity<T>;
        List<T> SqlQueryToList<T>(string sql, params object[] parameters) where T : BaseEntity<T>;
        void UpdateRange<TEntity>(IList<TEntity> ntities) where TEntity : class;
        string GetDataBaseName();
    }

    public enum DataBaseType
    {
        /// <summary>
        /// Sql Server
        /// </summary>
        SqlServer = 0,
        /// <summary>
        /// MySql
        /// </summary>
        MySql = 1
    }

    public class SqlExecuter<D> : ISqlExecuter<D>
        where D : AbpDbContext<D>
    {
        private readonly IDbContextProvider<D> _dbContextProvider;
        private DataBaseType _dbType = DataBaseType.MySql;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SqlExecuter(IDbContextProvider<D> dbContextProvider, IUnitOfWorkManager unitOfWorkManager)
        {
            _dbContextProvider = dbContextProvider;
            _unitOfWorkManager = unitOfWorkManager;
            //_dbType = dbType;
        }

        private string ConnectionString
        {
            get { return _dbContextProvider.GetDbContext().Database.GetConnectionString(); }
            //get { return ""; }
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        private IDbConnection GetConnection()
        {
            if (_dbType == DataBaseType.MySql)
            {
                return new MySqlConnection(ConnectionString);
            }
            else
            {
                return new SqlConnection(ConnectionString);
            }
        }
        private IDbCommand GetCommand(string cmdText, IDbConnection connection)
        {
            if (_dbType == DataBaseType.MySql)
            {
                return new MySqlCommand(cmdText, (MySqlConnection)connection);
            }
            else
            {
                return new SqlCommand(cmdText, (SqlConnection)connection);
            }
        }
        private IDataAdapter GetDataAdapter(IDbCommand cmd)
        {
            if (_dbType == DataBaseType.MySql)
            {
                return new MySqlDataAdapter((MySqlCommand)cmd);
            }
            else
            {
                return new SqlDataAdapter((SqlCommand)cmd);
            }
        }



        /// 命令字符串
        /// 要应用于命令字符串的参数
        /// 执行命令后由数据库返回的结果
        public int Execute(string sql, params object[] parameters)
        {
            return _dbContextProvider.GetDbContext().Database.ExecuteSqlRaw(sql, parameters);
        }

        /// 命令字符串
        /// 要应用于命令字符串的参数
        /// 执行命令后由数据库返回的结果
        public int Execute(string sql)
        {
            return _dbContextProvider.GetDbContext().Database.ExecuteSqlRaw(sql);
        }

        /// 命令字符串
        /// 要应用于命令字符串的参数
        /// 执行命令后由数据库返回的结果
        public int Execute(string sql, int connectionTimeout = 0)
        {
            var db = _dbContextProvider.GetDbContext().Database;
            if (connectionTimeout != 0)
                db.SetCommandTimeout(connectionTimeout);
            return db.ExecuteSqlRaw(sql);
        }

        /// <summary>
        /// 根据传入的sql语句获取返回的数据表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteTable(string sql, params object[] parameters)
        {
            var ds = ExecuteDataSet(sql, parameters);
            return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        /// <summary>
        /// 根据传入的sql语句获取返回的数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql, params object[] parameters)
        {
            var ds = new DataSet();

            using (var sqlcon = GetConnection())
            {
                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                var cmd = GetCommand(sql, sqlcon);
                if (parameters != null && parameters.Any())
                {
                    foreach (var p in parameters)
                        cmd.Parameters.Add(p);
                }
                cmd.CommandType = CommandType.Text;
                var da = GetDataAdapter(cmd);
                da.Fill(ds);
                sqlcon.Close();
                sqlcon.Dispose();
            }

            return ds;
        }


        /// <summary>
        /// 流动人口统计
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public DataTable ExecuteSql(string cmdText)
        {
            return ExecuteTable(cmdText, null);
        }

        /// <summary>
        /// 执行存储过程,并返回执行行数,无数据结果返回.
        /// </summary>
        /// <param name="cmdText">SQL语句(存储过程名称)</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns></returns>
        public int ExecuteNonQueryByCommand(string cmdText, DbParameter[] cmdParms)
        {
            using (var conn = GetConnection())
            {
                var cmd = GetCommand(cmdText, conn);
                if (cmdParms != null && cmdParms.Any())
                {
                    foreach (var p in cmdParms)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                cmd.CommandType = CommandType.Text;
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var cnt = cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
                return cnt;
            }
        }

        /// <summary>
        /// 执行存储过程,并返回执行行数,无数据结果返回.
        /// </summary>
        /// <param name="cmdText">SQL语句(存储过程名称)</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns></returns>
        public int ExecuteProcedureNonQuery(string cmdText, DbParameter[] cmdParms)
        {
            using (var conn = GetConnection())
            {
                var cmd = GetCommand(cmdText, conn);
                if (cmdParms != null && cmdParms.Any())
                {
                    foreach (var p in cmdParms)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                cmd.CommandType = CommandType.StoredProcedure;
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var cnt = cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
                return cnt;
            }
        }

        public object ExecuteScalar(CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            using (var conn = GetConnection())
            {
                var cmd = conn.CreateCommand();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        private static void PrepareCommand(IDbCommand cmd, IDbConnection conn, IDbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] commandParameters)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            //attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(cmd, commandParameters);
            }
        }
        private static void AttachParameters(IDbCommand command, DbParameter[] commandParameters)
        {
            foreach (var p in commandParameters)
            {
                //check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                command.Parameters.Add(p);
            }
        }

        public void AddRange<TEntity>(IList<TEntity> ntities) where TEntity : class
        {
            _dbContextProvider.GetDbContext().AddRange(ntities);
        }
        public void UpdateRange<TEntity>(IList<TEntity> ntities) where TEntity : class
        {
            _dbContextProvider.GetDbContext().UpdateRange(ntities);
        }
        public IQueryable<T> SqlQuery<T>(string sql, params object[] parameters) where T : BaseEntity<T>
        {
            return _dbContextProvider.GetDbContext().Set<T>().FromSqlRaw(sql, parameters);
        }

        public List<T> SqlQueryToList<T>(string sql, params object[] parameters) where T : BaseEntity<T>
        {
            var dt = ExecuteTable(sql, parameters);

            return DataTableToEntity<T>(dt);
        }

        public D GetDbContext()
        {
            return _dbContextProvider.GetDbContext();
        }

        public IQueryable<T> SqlQuery<T>(string sql) where T : BaseEntity<T>
        {
            return _dbContextProvider.GetDbContext().Set<T>().FromSqlRaw(sql);
        }

        public List<T> DataTableToEntity<T>(DataTable dt)
        {
            var json = JsonConvert.SerializeObject(dt);
            var list = JsonConvert.DeserializeObject<List<T>>(json);
            return list;
        }

        public static List<TEntity> GetDataTableToEntity<TEntity>(DataTable dt) where TEntity : BaseEntity<TEntity>
        {
            var json = JsonConvert.SerializeObject(dt);
            var list = JsonConvert.DeserializeObject<List<TEntity>>(json);
            return list;
        }

        public void BulkInsert<TEntity>(IList<TEntity> ntities) where TEntity : class
        {
            _dbContextProvider.GetDbContext().BulkInsert<TEntity>(ntities); ;
        }

        public void BulkUpdate<TEntity>(IList<TEntity> ntities) where TEntity : class
        {
            _dbContextProvider.GetDbContext().BulkUpdate<TEntity>(ntities);
        }

        public void BulkDelete<TEntity>(IList<TEntity> ntities) where TEntity : class
        {
            _dbContextProvider.GetDbContext().BulkDelete<TEntity>(ntities);
        }

        public void BulkMerge<TEntity>(IList<TEntity> ntities) where TEntity : class
        {
            _dbContextProvider.GetDbContext().BulkMerge<TEntity>(ntities);
        }

        public void BulkInsertAsync<TEntity>(IList<TEntity> ntities) where TEntity : class
        {
            _dbContextProvider.GetDbContext().BulkInsertAsync<TEntity>(ntities);
        }

        public void BulkUpdateAsync<TEntity>(IList<TEntity> ntities) where TEntity : class
        {
            _dbContextProvider.GetDbContext().BulkUpdateAsync<TEntity>(ntities);
        }

        public void BulkDeleteAsync<TEntity>(IList<TEntity> ntities) where TEntity : class
        {
            _dbContextProvider.GetDbContext().BulkDeleteAsync<TEntity>(ntities);
        }

        public void BulkMergeAsync<TEntity>(IList<TEntity> ntities) where TEntity : class
        {
            _dbContextProvider.GetDbContext().BulkMergeAsync<TEntity>(ntities);
        }

        #region 改造SQL扩展方法 - Sam.Xin

        #region 公用方法

        /// <summary>
        /// 根据传入的sql语句获取返回的数据表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param">对象</param>
        /// <returns></returns>
        public DataTable ExecuteTable(string sql, object param = null)
        {
            var dt = new DataTable();
            if (_dbType == DataBaseType.MySql)
            {
                var p = ConvertObjectToMySqlParameters(ref sql, param);
                using (var sqlcon = new MySqlConnection(ConnectionString))
                {
                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    var cmd = new MySqlCommand(sql, sqlcon);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.Text;
                    var da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }
            else
            {
                var p = ConvertObjectToSqlParameters(ref sql, param);
                using (var sqlcon = new SqlConnection(ConnectionString))
                {
                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    SqlCommand cmd = new SqlCommand(sql, sqlcon);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }


            return dt;
        }

        /// <summary>
        /// 根据传入的sql语句获取返回的数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param">对象</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql, object param = null)
        {
            DataSet ds = new DataSet();
            if (_dbType == DataBaseType.MySql)
            {

                var p = ConvertObjectToMySqlParameters(ref sql, param);
                using (var sqlcon = new MySqlConnection(ConnectionString))
                {
                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    var cmd = new MySqlCommand(sql, sqlcon);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.Text;
                    var da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }
            else
            {
                var p = ConvertObjectToSqlParameters(ref sql, param);
                using (var sqlcon = new SqlConnection(ConnectionString))
                {
                    if (sqlcon.State != ConnectionState.Open)
                    {
                        sqlcon.Open();
                    }
                    SqlCommand cmd = new SqlCommand(sql, sqlcon);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }


            return ds;
        }

        /// <summary>
        /// 命令字符串
        /// 要应用于命令字符串的参数
        /// 执行命令后由数据库返回的结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param">对象</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, object param = null)
        {
            if (_dbType == DataBaseType.MySql)
            {
                var p = ConvertObjectToMySqlParameters(ref sql, param);
                if (p != null && p.Any())
                {
                    return _dbContextProvider.GetDbContext().Database.ExecuteSqlRaw(sql, p);
                }
                else
                {
                    return _dbContextProvider.GetDbContext().Database.ExecuteSqlRaw(sql);
                }
            }
            else
            {
                var p = ConvertObjectToSqlParameters(ref sql, param);
                if (p != null && p.Any())
                {
                    return _dbContextProvider.GetDbContext().Database.ExecuteSqlRaw(sql, p);
                }
                else
                {
                    return _dbContextProvider.GetDbContext().Database.ExecuteSqlRaw(sql);
                }
            }

        }

        /// <summary>
        /// 命令字符串
        /// 要应用于命令字符串的参数
        /// 执行命令后由数据库返回的结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param">对象</param>
        /// <returns></returns>
        public int ExecuteSqlTextNonQuery(string sql, object param = null)
        {
            if (_dbType == DataBaseType.MySql)
            {
                var p = ConvertObjectToMySqlParameters(param);
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    var cmd = new MySqlCommand(sql, conn);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.Text;
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    var cnt = cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    return cnt;
                }
            }
            else
            {
                var p = ConvertObjectToSqlParameters(param);
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.Text;
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    var cnt = cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    return cnt;
                }
            }

        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storeProcedureText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable ExecuteProcedure(string storeProcedureText, object param = null)
        {
            if (_dbType == DataBaseType.MySql)
            {
                var p = ConvertObjectToMySqlParameters(param);
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    var cmd = new MySqlCommand(storeProcedureText, conn);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dap = new MySqlDataAdapter(cmd);
                    var ds = new DataSet();
                    dap.Fill(ds);
                    var dt = ds.Tables[0];
                    conn.Close();
                    conn.Dispose();
                    return dt;
                }
            }
            else
            {
                var p = ConvertObjectToSqlParameters(param);
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(storeProcedureText, conn);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    dap.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    conn.Close();
                    conn.Dispose();
                    return dt;
                }
            }

        }


        /// <summary>
        /// 执行存储过程（DataSet）
        /// </summary>
        /// <param name="storeProcedureText">存储过程</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public async Task<DataSet> ExecuteProcedureSetAsync(string storeProcedureText, object param = null)
        {
            if (_dbType == DataBaseType.MySql)
            {
                var p = ConvertObjectToMySqlParameters(param);
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    var cmd = new MySqlCommand(storeProcedureText, conn);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dap = new MySqlDataAdapter(cmd);
                    var ds = new DataSet();
                    await dap.FillAsync(ds);
                    conn.Close();
                    conn.Dispose();
                    return ds;
                }
            }
            else
            {
                var p = ConvertObjectToSqlParameters(param);
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(storeProcedureText, conn);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    dap.Fill(ds);
                    conn.Close();
                    conn.Dispose();
                    return ds;
                }
            }

        }


        /// <summary>
        /// 执行存储过程（DataTable）
        /// </summary>
        /// <param name="storeProcedureText">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteProcedureAsync(string storeProcedureText, object param = null)
        {
            var ds = await ExecuteProcedureSetAsync(storeProcedureText, param);
            return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storeProcedureText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataSet ExecuteProcedureDataSet(string storeProcedureText, object param = null)
        {
            if (_dbType == DataBaseType.MySql)
            {
                var p = ConvertObjectToSqlParameters(param);
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    var cmd = new MySqlCommand(storeProcedureText, conn);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dap = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    dap.Fill(ds);
                    conn.Close();
                    conn.Dispose();
                    return ds;
                }
            }
            else
            {
                var p = ConvertObjectToSqlParameters(param);
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(storeProcedureText, conn);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    dap.Fill(ds);
                    conn.Close();
                    conn.Dispose();
                    return ds;
                }
            }
        }

        /// <summary>
        /// 执行存储过程(无查询结果)
        /// </summary>
        /// <param name="storeProcedureText"></param>
        /// <param name="param">对象</param>
        /// <returns></returns>
        public int ExecuteProcedureNonQuery(string storeProcedureText, object param = null)
        {
            if (_dbType == DataBaseType.MySql)
            {
                var p = ConvertObjectToMySqlParameters(param);
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    var cmd = new MySqlCommand(storeProcedureText, conn);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    var cnt = cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    return cnt;
                }
            }
            else
            {
                var p = ConvertObjectToSqlParameters(param);
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(storeProcedureText, conn);
                    if (p != null && p.Any())
                    {
                        cmd.Parameters.AddRange(p);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    var cnt = cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    return cnt;
                }
            }

        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 显示SQL参数设置(仅附在进程调试才会显示)
        /// </summary>
        /// <param name="param"></param>
        private static void ShowDebugerSqlParameterDefine(SqlParameter[] param)
        {
            if (Debugger.IsAttached && param != null && param.Any())
            {
                var sb = new StringBuilder();
                var declareFmt = "Declare {0} AS {1};";
                var setFmt = "Set {0} = {1};";
                sb.AppendLine("----------- SQL Declare Info -----------");
                foreach (var p in param)
                {
                    sb.AppendLine(declareFmt.AsFormat(p.ParameterName, p.SqlDbType.ToString()));
                    sb.AppendLine(setFmt.AsFormat(p.ParameterName, p.Value.AsToSqlString()));
                }
                sb.AppendLine("----------------------------------------");
                Debug.Write(sb.ToString());
            }
        }

        /// <summary>
        /// 显示SQL参数设置(仅附在进程调试才会显示)
        /// </summary>
        /// <param name="param"></param>
        private static void ShowDebugerMySqlParameterDefine(MySqlParameter[] param)
        {
            if (Debugger.IsAttached && param != null && param.Any())
            {
                var sb = new StringBuilder();
                var declareFmt = "Declare {0} AS {1};";
                var setFmt = "Set {0} = {1};";
                sb.AppendLine("----------- MySQL Declare Info -----------");
                foreach (var p in param)
                {
                    sb.AppendLine(declareFmt.AsFormat(p.ParameterName, p.DbType.ToString()));
                    sb.AppendLine(setFmt.AsFormat(p.ParameterName, p.Value.AsToSqlString()));
                }
                sb.AppendLine("----------------------------------------");
                Debug.Write(sb.ToString());
            }
        }

        /// <summary>
        /// 转换对象为SQL参数数组
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param">参数对象</param>
        /// <returns></returns>
        private static SqlParameter[] ConvertObjectToSqlParameters(ref string sql, object param)
        {
            if (param != null)
            {
                var properties = param.GetType().GetProperties();
                var paramData = new List<SqlParameter>();
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(param, null);
                    if (IsSearchParameter(value))
                    {
                        var whereData = ConvertSearchParameter(value);
                        if (whereData != null && whereData.Any()) // 如果这个参数指定的是拼接SQL, 且有值,则替换标记位为自动生成的带参数SQL
                        {
                            sql = sql.Replace(!prop.Name.StartsWith("_") ? "_{0}".AsFormat(prop.Name) : prop.Name, " {0} ".AsFormat(whereData.Select(d => d.CreateSQL()).AsJoin(" ")));
                            paramData.AddRange(whereData.Select(d => d.Parameter).ToList());
                        }
                    }
                    else if (IsSortParameter(value))
                    {
                        var sortData = ConvertSortParameter(value);
                        if (sortData != null && sortData.Any()) // 如果这个参数指定的是拼接SQL, 且有值,则替换标记位为自动生成的带参数SQL
                        {
                            sql = sql.Replace(!prop.Name.StartsWith("_") ? "_{0}".AsFormat(prop.Name) : prop.Name, " {0} ".AsFormat(sortData.Select(d => d.CreateSQL()).AsJoin(",")));
                        }
                    }
                    else
                    {
                        if (prop.Name.StartsWith("_")) // 如果这个参数指定的是拼接SQL, 但是没有值, 则替换标记位为空字符串
                        {
                            sql = sql.Replace(!prop.Name.StartsWith("_") ? "_{0}".AsFormat(prop.Name) : prop.Name, "");
                        }
                        else
                        {
                            if (value as string != null)
                            {
                                value = value.ToClearSqlInj(false);
                            }
                            paramData.Add(new SqlParameter(!prop.Name.StartsWith("@") ? "@{0}".AsFormat(prop.Name) : prop.Name, value == null ? DBNull.Value : value));
                        }
                    }


                }
                var list = paramData.ToArray();

                ShowDebugerSqlParameterDefine(list); // 显示SQL参数设置(仅附在进程调试才会显示)
                properties = null;
                paramData = null;
                return list;
            }
            return null;
        }

        /// <summary>
        /// 转换对象为SQL参数数组
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param">参数对象</param>
        /// <returns></returns>
        private static MySqlParameter[] ConvertObjectToMySqlParameters(ref string sql, object param)
        {
            if (param != null)
            {
                var properties = param.GetType().GetProperties();
                var paramData = new List<MySqlParameter>();
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(param, null);
                    if (IsSearchParameter(value))
                    {
                        var whereData = ConvertMySqlSearchParameter(value);
                        if (whereData != null && whereData.Any()) // 如果这个参数指定的是拼接SQL, 且有值,则替换标记位为自动生成的带参数SQL
                        {
                            sql = sql.Replace(!prop.Name.StartsWith("_") ? "_{0}".AsFormat(prop.Name) : prop.Name, " {0} ".AsFormat(whereData.Select(d => d.CreateSQL()).AsJoin(" ")));
                            paramData.AddRange(whereData.Select(d => d.Parameter).ToList());
                        }
                    }
                    else if (IsSortParameter(value))
                    {
                        var sortData = ConvertSortParameter(value);
                        if (sortData != null && sortData.Any()) // 如果这个参数指定的是拼接SQL, 且有值,则替换标记位为自动生成的带参数SQL
                        {
                            sql = sql.Replace(!prop.Name.StartsWith("_") ? "_{0}".AsFormat(prop.Name) : prop.Name, " {0} ".AsFormat(sortData.Select(d => d.CreateSQL()).AsJoin(",")));
                        }
                    }
                    else
                    {
                        if (prop.Name.StartsWith("_")) // 如果这个参数指定的是拼接SQL, 但是没有值, 则替换标记位为空字符串
                        {
                            sql = sql.Replace(!prop.Name.StartsWith("_") ? "_{0}".AsFormat(prop.Name) : prop.Name, "");
                        }
                        else
                        {
                            if (value as string != null)
                            {
                                value = value.ToClearSqlInj(false);
                            }
                            paramData.Add(new MySqlParameter(!prop.Name.StartsWith("@") ? "@{0}".AsFormat(prop.Name) : prop.Name, value == null ? DBNull.Value : value));
                        }
                    }


                }
                var list = paramData.ToArray();

                ShowDebugerMySqlParameterDefine(list); // 显示SQL参数设置(仅附在进程调试才会显示)
                properties = null;
                paramData = null;
                return list;
            }
            return null;
        }

        /// <summary>
        /// 转换对象为SQL参数数组(只转换对象属性为SQL参数,不参与SQL语句的动态变更)
        /// </summary>
        /// <param name="param">参数对象</param>
        /// <returns></returns>
        private static SqlParameter[] ConvertObjectToSqlParameters(object param)
        {
            if (param != null)
            {
                var properties = param.GetType().GetProperties();
                var paramData = new List<SqlParameter>();
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(param, null);
                    if (value as string != null)
                    {
                        value = value.ToClearSqlInj(false);
                    }
                    paramData.Add(new SqlParameter(!prop.Name.StartsWith("@") ? "@{0}".AsFormat(prop.Name) : prop.Name, value == null ? DBNull.Value : value));
                }
                var list = paramData.ToArray();

                ShowDebugerSqlParameterDefine(list); // 显示SQL参数设置(仅附在进程调试才会显示)
                return list;
            }
            return null;
        }


        /// <summary>
        /// 转换对象为SQL参数数组(只转换对象属性为SQL参数,不参与SQL语句的动态变更)
        /// </summary>
        /// <param name="param">参数对象</param>
        /// <returns></returns>
        private static MySqlParameter[] ConvertObjectToMySqlParameters(object param)
        {
            if (param != null)
            {
                var properties = param.GetType().GetProperties();
                var paramData = new List<MySqlParameter>();
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(param, null);
                    if (value as string != null)
                    {
                        value = value.ToClearSqlInj(false);
                    }
                    paramData.Add(new MySqlParameter(prop.Name, value == null ? DBNull.Value : value));
                }
                var list = paramData.ToArray();

                ShowDebugerMySqlParameterDefine(list); // 显示SQL参数设置(仅附在进程调试才会显示)
                return list;
            }
            return null;
        }

        /// <summary>
        /// 获取对象类型中的表名定义扩展属性
        /// </summary>
        /// <typeparam name="T">表对象类型</typeparam>
        /// <param name="t">表对象实例</param>
        /// <returns></returns>
        private static string GetTableObjectName<T>(T t)
        {
            var customAttrs = t.GetType().GetCustomAttributes(typeof(TableAttribute), false);
            if (customAttrs != null && customAttrs.Any())
            {
                var item = customAttrs.FirstOrDefault() as TableAttribute;
                if (item != null)
                {
                    return item.Name;
                }
            }
            return t.GetType().Name;
        }

        /// <summary>
        /// 检查是否是条件拼接字
        /// </summary>
        /// <param name="value">参数对象</param>
        /// <returns></returns>
        private static bool IsSearchParameter(object value)
        {
            if (value == null)
            {
                return false;
            }
            var obj = value;
            if (obj as string != null)
            {
                var checkStrings = new string[] { " and ", " or ", " like ", " not like ", "'", " is null", " is not null", "=", "<>", "<=", ">=", ">", "<" };
                var isFound = false;
                var str = obj.ToString();
                str = !str.StartsWith(" ") ? (" " + str) : str;
                str = !str.EndsWith(" ") ? (str + " ") : str;
                checkStrings.ToList().ForEach(f =>
                {
                    if (isFound)
                    {
                        return;
                    }
                    if (str.IndexOf(f, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        isFound = true;
                    }
                });
                return isFound;
            }
            return false;
        }
        /// <summary>
        /// 检查是否存在排序关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool IsSortParameter(object value)
        {
            if (value == null)
            {
                return false;
            }
            var obj = value;
            if (obj as string != null)
            {
                var checkStrings = new string[] { " asc", " desc" };
                var isFound = false;
                var str = obj.ToString();
                str = !str.StartsWith(" ") ? (" " + str) : str;
                str = !str.EndsWith(" ") ? (str + " ") : str;
                checkStrings.ToList().ForEach(f =>
                {
                    if (isFound)
                    {
                        return;
                    }
                    if (str.IndexOf(f, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        isFound = true;
                    }
                });
                return isFound;
            }
            return false;
        }


        /// <summary>
        /// 生成排序字符串列表
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static List<SortSqlParameterInfo> ConvertSortParameter(object value)
        {
            if (value == null)
            {
                return null;
            }
            var obj = value;
            if (obj as string != null)
            {
                var str = obj.ToString();
                str = !str.StartsWith(" ") ? (" " + str) : str;
                str = !str.EndsWith(" ") ? (str + " ") : str;
                var checkStrings = new string[] { " asc", " desc" };
                var isFound = false;
                checkStrings.ToList().ForEach(f =>
                {
                    if (isFound)
                    {
                        return;
                    }
                    if (str.IndexOf(f, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        isFound = true;
                    }
                });
                if (isFound) // 如果是拼接字符串
                {
                    var sorts = new List<SortSqlParameterInfo>();
                    // 处理拼接字符串
                    foreach (var item in obj.ToString().AsSplit(",")) // 分拆对象
                    {
                        if (string.IsNullOrEmpty(item.Trim()))
                        {
                            continue;
                        }
                        var sortItem = item.ToClearSqlInj(true).AsSplit(" ");
                        if (sortItem.Length == 2)
                        {
                            if (sortItem[1].Trim().ToLower().IndexOf("asc") != -1 || sortItem[1].Trim().ToLower().IndexOf("desc") != -1)
                            {
                                var p = new SortSqlParameterInfo
                                {
                                    ColumnName = sortItem[0],
                                    SortDirection = sortItem[1].Trim().ToLower().IndexOf("asc") != -1 ? SortSqlParameterInfo.SortType.ASC : SortSqlParameterInfo.SortType.DESC
                                };
                                sorts.Add(p);
                            }
                        }
                    }
                    return sorts;
                }
            }
            return null;
        }


        /// <summary>
        /// 转换Search参数为查询参数数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static List<WhereSqlParameterInfo> ConvertSearchParameter(object value)
        {
            if (value == null)
            {
                return null;
            }
            var obj = value;
            if (obj as string != null)
            {
                var str = obj.ToString();
                str = !str.StartsWith(" ") ? (" " + str) : str;
                str = !str.EndsWith(" ") ? (str + " ") : str;
                var checkStrings = new string[] { " and ", " or ", " like ", " not like ", "'", " is null", " is not null", "=", "<>", "<=", ">=", ">", "<" };
                var isFound = false;
                checkStrings.ToList().ForEach(f =>
                {
                    if (isFound)
                    {
                        return;
                    }
                    if (str.IndexOf(f, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        isFound = true;
                    }
                });
                if (isFound) // 如果是拼接字符串
                {
                    var wheres = new List<WhereSqlParameterInfo>();
                    // 处理拼接字符串
                    var andList = new List<string>();
                    var orList = new List<string>();
                    foreach (var item in Regex.Split(str, " and ", RegexOptions.IgnoreCase)) // 分拆AND关键字
                    {
                        if (string.IsNullOrEmpty(item.Trim()))
                        {
                            continue;
                        }
                        var ors = Regex.Split(item, " or ", RegexOptions.IgnoreCase); // 分拆OR关键字
                        ors = ors.Where(o => !string.IsNullOrEmpty(o.Trim())).ToArray();
                        if (ors.Length > 1)
                        {
                            orList.AddRange(ors.Skip(1).ToList());
                            andList.Add(ors.First());
                        }
                        else
                        {
                            andList.Add(item);
                        }
                    }
                    if (andList.Any())
                    {
                        var ands = andList.Where(d => !string.IsNullOrEmpty(d))
                            .Select(d => ConvertSQLText2SQLParameters(d.ToClearSqlInj(false)))
                            .Where(d => d != null).ToList();
                        ands.ForEach(d => { d.Relationship = WhereSqlParameterInfo.RelationshipType.And; });
                        wheres.AddRange(ands);
                    }
                    if (orList.Any())
                    {
                        var ands = orList.Where(d => !string.IsNullOrEmpty(d))
                            .Select(d => ConvertSQLText2SQLParameters(d.ToClearSqlInj(false)))
                            .Where(d => d != null).ToList();
                        ands.ForEach(d => { d.Relationship = WhereSqlParameterInfo.RelationshipType.Or; });
                        wheres.AddRange(ands);
                    }
                    return wheres;
                }
            }
            return null;
        }

        /// <summary>
        /// 转换Search参数为查询参数数据
        /// </summary>
        /// <param name="property"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static List<WhereMySqlParameterInfo> ConvertMySqlSearchParameter(object value)
        {
            if (value == null)
            {
                return null;
            }
            var obj = value;
            if (obj as string != null)
            {
                var str = obj.ToString();
                str = !str.StartsWith(" ") ? (" " + str) : str;
                str = !str.EndsWith(" ") ? (str + " ") : str;
                var checkStrings = new string[] { " and ", " or ", " like ", " not like ", "'", " is null", " is not null", "=", "<>", "<=", ">=", ">", "<" };
                var isFound = false;
                checkStrings.ToList().ForEach(f =>
                {
                    if (isFound)
                    {
                        return;
                    }
                    if (str.IndexOf(f, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        isFound = true;
                    }
                });
                if (isFound) // 如果是拼接字符串
                {
                    var wheres = new List<WhereMySqlParameterInfo>();
                    // 处理拼接字符串
                    var andList = new List<string>();
                    var orList = new List<string>();
                    foreach (var item in Regex.Split(str, " and ", RegexOptions.IgnoreCase)) // 分拆AND关键字
                    {
                        if (string.IsNullOrEmpty(item.Trim()))
                        {
                            continue;
                        }
                        var ors = Regex.Split(item, " or ", RegexOptions.IgnoreCase); // 分拆OR关键字
                        ors = ors.Where(o => !string.IsNullOrEmpty(o.Trim())).ToArray();
                        if (ors.Length > 1)
                        {
                            orList.AddRange(ors.Skip(1).ToList());
                            andList.Add(ors.First());
                        }
                        else
                        {
                            andList.Add(item);
                        }
                    }
                    if (andList.Any())
                    {
                        var ands = andList.Where(d => !string.IsNullOrEmpty(d))
                            .Select(d => ConvertSQLText2MySQLParameters(d.ToClearSqlInj(false)))
                            .Where(d => d != null).ToList();
                        ands.ForEach(d => { d.Relationship = WhereMySqlParameterInfo.RelationshipType.And; });
                        wheres.AddRange(ands);
                    }
                    if (orList.Any())
                    {
                        var ands = orList.Where(d => !string.IsNullOrEmpty(d))
                            .Select(d => ConvertSQLText2MySQLParameters(d.ToClearSqlInj(false)))
                            .Where(d => d != null).ToList();
                        ands.ForEach(d => { d.Relationship = WhereMySqlParameterInfo.RelationshipType.Or; });
                        wheres.AddRange(ands);
                    }
                    return wheres;
                }
            }
            return null;
        }


        /// <summary>
        /// 分析并转换Where语句
        /// </summary>
        /// <param name="sqlBlock">Where条件块SQL</param>
        /// <returns></returns>
        private static WhereSqlParameterInfo ConvertSQLText2SQLParameters(string sqlBlock)
        {
            var split = new List<string>();
            var info = new WhereSqlParameterInfo();
            var parameterName = "@Search_{0}";
            if (sqlBlock.IndexOf("not like", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = Regex.Split(sqlBlock, " not like ", RegexOptions.IgnoreCase).ToList();
                if (split.Count == 2)
                {
                    var valueItem = split[1].Replace("'", "").Trim();
                    var value = valueItem.Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : valueItem.Replace("%", "").Trim();
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " NOT LIKE " + (valueItem.StartsWith("%") ? "'%' + " : "") + parameterName + (valueItem.EndsWith("%") ? " + '%'" : "");
                    info.Parameter = new SqlParameter(parameterName, value.Trim());
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("like", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = Regex.Split(sqlBlock, " like ", RegexOptions.IgnoreCase).ToList();
                if (split.Count == 2)
                {
                    var valueItem = split[1].Replace("'", "").Trim();
                    var value = valueItem.Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : valueItem.Replace("%", "").Trim();
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " LIKE " + (valueItem.StartsWith("%") ? "'%' + " : "") + parameterName + (valueItem.EndsWith("%") ? " + '%'" : "");
                    info.Parameter = new SqlParameter(parameterName, value.Trim());
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("is null", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = Regex.Split(sqlBlock, " is null", RegexOptions.IgnoreCase).ToList();
                if (split.Count >= 0)
                {
                    var value = DBNull.Value;
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " = " + parameterName;
                    info.Parameter = new SqlParameter(parameterName, value);
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("is not null", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = Regex.Split(sqlBlock, " is not null", RegexOptions.IgnoreCase).ToList();
                if (split.Count >= 0)
                {
                    var value = DBNull.Value;
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " <> " + parameterName;
                    info.Parameter = new SqlParameter(parameterName, value);
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("<=", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = sqlBlock.AsSplit("<=").ToList();
                if (split.Count == 2)
                {
                    var value = split[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : split[1].Replace("'", "");
                    var valueType = split[1].IndexOf("'") != -1 ? typeof(string) : typeof(decimal);
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " <= " + parameterName;
                    if (valueType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(value.Trim(), out d);
                        info.Parameter = new SqlParameter(parameterName, d);
                    }
                    else
                    {
                        info.Parameter = new SqlParameter(parameterName, value.Trim());
                    }
                    return info;
                }
            }
            else if (sqlBlock.IndexOf(">=", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = sqlBlock.AsSplit(">=").ToList();
                if (split.Count == 2)
                {
                    var value = split[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : split[1].Replace("'", "");
                    var valueType = split[1].IndexOf("'") != -1 ? typeof(string) : typeof(decimal);
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " >= " + parameterName;
                    if (valueType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(value.Trim(), out d);
                        info.Parameter = new SqlParameter(parameterName, d);
                    }
                    else
                    {
                        info.Parameter = new SqlParameter(parameterName, value.Trim());
                    }
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("<>", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = sqlBlock.AsSplit("<>").ToList();
                if (split.Count == 2)
                {
                    var value = split[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : split[1].Replace("'", "");
                    var valueType = split[1].IndexOf("'") != -1 ? typeof(string) : typeof(decimal);
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " <> " + parameterName;
                    if (valueType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(value.Trim(), out d);
                        info.Parameter = new SqlParameter(parameterName, d);
                    }
                    else
                    {
                        info.Parameter = new SqlParameter(parameterName, value.Trim());
                    }
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("<", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = sqlBlock.AsSplit("<").ToList();
                if (split.Count == 2)
                {
                    var value = split[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : split[1].Replace("'", "");
                    var valueType = split[1].IndexOf("'") != -1 ? typeof(string) : typeof(decimal);
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " < " + parameterName;
                    if (valueType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(value.Trim(), out d);
                        info.Parameter = new SqlParameter(parameterName, d);
                    }
                    else
                    {
                        info.Parameter = new SqlParameter(parameterName, value.Trim());
                    }
                    return info;
                }
            }
            else if (sqlBlock.IndexOf(">", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = sqlBlock.AsSplit(">").ToList();
                if (split.Count == 2)
                {
                    var value = split[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : split[1].Replace("'", "");
                    var valueType = split[1].IndexOf("'") != -1 ? typeof(string) : typeof(decimal);
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " > " + parameterName;
                    if (valueType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(value.Trim(), out d);
                        info.Parameter = new SqlParameter(parameterName, d);
                    }
                    else
                    {
                        info.Parameter = new SqlParameter(parameterName, value.Trim());
                    }
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("=", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = sqlBlock.AsSplit("=").ToList();
                if (split.Count == 2)
                {
                    var value = split[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : split[1].Replace("'", "");
                    var valueType = split[1].IndexOf("'") != -1 ? typeof(string) : typeof(decimal);
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " = " + parameterName;
                    if (valueType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(value.Trim(), out d);
                        info.Parameter = new SqlParameter(parameterName, d);
                    }
                    else
                    {
                        info.Parameter = new SqlParameter(parameterName, value.Trim());
                    }
                    return info;
                }
            }
            return null;
        }


        /// <summary>
        /// 分析并转换Where语句
        /// </summary>
        /// <param name="sqlBlock">Where条件块SQL</param>
        /// <returns></returns>
        private static WhereMySqlParameterInfo ConvertSQLText2MySQLParameters(string sqlBlock)
        {
            var split = new List<string>();
            var info = new WhereMySqlParameterInfo();
            var parameterName = "@Search_{0}";
            if (sqlBlock.IndexOf("not like", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = Regex.Split(sqlBlock, " not like ", RegexOptions.IgnoreCase).ToList();
                if (split.Count == 2)
                {
                    var valueItem = split[1].Replace("'", "").Trim();
                    var value = valueItem.Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : valueItem.Replace("%", "").Trim();
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " NOT LIKE " + (valueItem.StartsWith("%") ? "'%' + " : "") + parameterName + (valueItem.EndsWith("%") ? " + '%'" : "");
                    info.Parameter = new MySqlParameter(parameterName, value.Trim());
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("like", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = Regex.Split(sqlBlock, " like ", RegexOptions.IgnoreCase).ToList();
                if (split.Count == 2)
                {
                    var valueItem = split[1].Replace("'", "").Trim();
                    var value = valueItem.Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : valueItem.Replace("%", "").Trim();
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " LIKE " + (valueItem.StartsWith("%") ? "'%' + " : "") + parameterName + (valueItem.EndsWith("%") ? " + '%'" : "");
                    info.Parameter = new MySqlParameter(parameterName, value.Trim());
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("is null", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = Regex.Split(sqlBlock, " is null", RegexOptions.IgnoreCase).ToList();
                if (split.Count >= 0)
                {
                    var value = DBNull.Value;
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " = " + parameterName;
                    info.Parameter = new MySqlParameter(parameterName, value);
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("is not null", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = Regex.Split(sqlBlock, " is not null", RegexOptions.IgnoreCase).ToList();
                if (split.Count >= 0)
                {
                    var value = DBNull.Value;
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " <> " + parameterName;
                    info.Parameter = new MySqlParameter(parameterName, value);
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("<=", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = sqlBlock.AsSplit("<=").ToList();
                if (split.Count == 2)
                {
                    var value = split[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : split[1].Replace("'", "");
                    var valueType = split[1].IndexOf("'") != -1 ? typeof(string) : typeof(decimal);
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " <= " + parameterName;
                    if (valueType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(value.Trim(), out d);
                        info.Parameter = new MySqlParameter(parameterName, d);
                    }
                    else
                    {
                        info.Parameter = new MySqlParameter(parameterName, value.Trim());
                    }
                    return info;
                }
            }
            else if (sqlBlock.IndexOf(">=", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = sqlBlock.AsSplit(">=").ToList();
                if (split.Count == 2)
                {
                    var value = split[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : split[1].Replace("'", "");
                    var valueType = split[1].IndexOf("'") != -1 ? typeof(string) : typeof(decimal);
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " >= " + parameterName;
                    if (valueType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(value.Trim(), out d);
                        info.Parameter = new MySqlParameter(parameterName, d);
                    }
                    else
                    {
                        info.Parameter = new MySqlParameter(parameterName, value.Trim());
                    }
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("<>", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = sqlBlock.AsSplit("<>").ToList();
                if (split.Count == 2)
                {
                    var value = split[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : split[1].Replace("'", "");
                    var valueType = split[1].IndexOf("'") != -1 ? typeof(string) : typeof(decimal);
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " <> " + parameterName;
                    if (valueType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(value.Trim(), out d);
                        info.Parameter = new MySqlParameter(parameterName, d);
                    }
                    else
                    {
                        info.Parameter = new MySqlParameter(parameterName, value.Trim());
                    }
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("<", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = sqlBlock.AsSplit("<").ToList();
                if (split.Count == 2)
                {
                    var value = split[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : split[1].Replace("'", "");
                    var valueType = split[1].IndexOf("'") != -1 ? typeof(string) : typeof(decimal);
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " < " + parameterName;
                    if (valueType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(value.Trim(), out d);
                        info.Parameter = new MySqlParameter(parameterName, d);
                    }
                    else
                    {
                        info.Parameter = new MySqlParameter(parameterName, value.Trim());
                    }
                    return info;
                }
            }
            else if (sqlBlock.IndexOf(">", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = sqlBlock.AsSplit(">").ToList();
                if (split.Count == 2)
                {
                    var value = split[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : split[1].Replace("'", "");
                    var valueType = split[1].IndexOf("'") != -1 ? typeof(string) : typeof(decimal);
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " > " + parameterName;
                    if (valueType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(value.Trim(), out d);
                        info.Parameter = new MySqlParameter(parameterName, d);
                    }
                    else
                    {
                        info.Parameter = new MySqlParameter(parameterName, value.Trim());
                    }
                    return info;
                }
            }
            else if (sqlBlock.IndexOf("=", StringComparison.OrdinalIgnoreCase) != -1)
            {
                split = sqlBlock.AsSplit("=").ToList();
                if (split.Count == 2)
                {
                    var value = split[1].Trim().Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : split[1].Replace("'", "");
                    var valueType = split[1].IndexOf("'") != -1 ? typeof(string) : typeof(decimal);
                    parameterName = parameterName.AsFormat(split[0].Trim());
                    info.SqlBlockText = split[0] + " = " + parameterName;
                    if (valueType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(value.Trim(), out d);
                        info.Parameter = new MySqlParameter(parameterName, d);
                    }
                    else
                    {
                        info.Parameter = new MySqlParameter(parameterName, value.Trim());
                    }
                    return info;
                }
            }
            return null;
        }

        /// <summary>
        /// 用于转换Search拼接SQL的转换类
        /// </summary>
        public class WhereSqlParameterInfo
        {
            public enum RelationshipType
            {
                None = 0,
                And = 1,
                Or = 2
            }
            /// <summary>
            /// SQL块文本
            /// </summary>
            public string SqlBlockText { get; set; }

            /// <summary>
            /// 参数
            /// </summary>
            public SqlParameter Parameter { get; set; }

            /// <summary>
            /// 关联链接字
            /// </summary>
            public RelationshipType Relationship { get; set; }

            /// <summary>
            /// 生成SQL语句
            /// </summary>
            /// <returns></returns>
            public string CreateSQL()
            {
                return " {0} {1}".AsFormat(Relationship, SqlBlockText);
            }
        }

        /// <summary>
        /// 用于转换Search拼接SQL的转换类
        /// </summary>
        public class WhereMySqlParameterInfo : WhereSqlParameterInfo
        {
            /// <summary>
            /// 参数
            /// </summary>
            public new MySqlParameter Parameter { get; set; }
        }
        /// <summary>
        /// 用于转换Order拼接SQL的转换类
        /// </summary>
        public class SortSqlParameterInfo
        {
            /// <summary>
            /// 排序类型
            /// </summary>
            public enum SortType
            {
                /// <summary>
                /// 正序
                /// </summary>
                ASC = 0,
                /// <summary>
                /// 倒序
                /// </summary>
                DESC = 1
            }

            /// <summary>
            /// 排序方向
            /// </summary>
            public SortType SortDirection { get; set; }
            /// <summary>
            /// 列名
            /// </summary>
            public string ColumnName { get; set; }

            /// <summary>
            /// 生成SQL语句
            /// </summary>
            /// <returns></returns>
            public string CreateSQL()
            {
                return " {0} {1}".AsFormat(ColumnName, SortDirection.ToString());
            }
        }

        #endregion

        #endregion

        #region 异步方法

        #region 公有方法

        /// <summary>
        /// 异步获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数对象</param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteDataTableAsync(string sql, object parameters)
        {
            if (!sql.IsNullOrEmpty())
            {
                var dt = new DataTable();
                if (_dbType == DataBaseType.MySql)
                {
                    var p = ConvertObjectToMySqlParameters(parameters);
                    return await ExecuteDataTableAsync(sql, p);
                }
                else
                {
                    var p = ConvertObjectToSqlParameters(parameters);
                    return await ExecuteDataTableAsync(sql, p);
                }
            }
            return null;
        }

        /// <summary>
        /// 异步提交变更(事务完成后的Commit提交)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数对象</param>
        /// <remarks>情勿在本操作语句后面增加新的同表操作,可能会导致异常或者脏数据</remarks>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, object parameters = null)
        {
            if (!sql.IsNullOrEmpty())
            {
                if (_dbType == DataBaseType.MySql)
                {
                    var p = ConvertObjectToMySqlParameters(parameters);
                    if (p == null)
                        p = new MySqlParameter[0];
                    return await _dbContextProvider.GetDbContext().Database.ExecuteSqlRawAsync(sql, p);
                }
                else
                {
                    var p = ConvertObjectToSqlParameters(parameters);
                    if (p == null)
                        p = new SqlParameter[0];
                    return await _dbContextProvider.GetDbContext().Database.ExecuteSqlRawAsync(sql, p);
                }
            }
            return 0;
        }

        /// <summary>
        /// 异步获取数据集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数对象</param>
        /// <returns></returns>
        public async Task<DataSet> ExecuteDataSetAsync(List<string> sql, object parameters)
        {
            if (sql != null && sql.Any())
            {
                var ds = new DataSet();
                if (_dbType == DataBaseType.MySql)
                {
                    var p = ConvertObjectToMySqlParameters(parameters);

                    foreach (var s in sql)
                    {
                        var dt = await ExecuteDataTableAsync(s, p);
                        ds.Tables.Add(dt);
                    }
                }
                else
                {
                    var p = ConvertObjectToSqlParameters(parameters);
                    foreach (var s in sql)
                    {
                        var dt = await ExecuteDataTableAsync(s, p);
                        ds.Tables.Add(dt);
                    }
                }
                return ds;
            }
            return null;
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <returns></returns>
        public string GetDataBaseName()
        {
            var paramList = ConnectionString.AsSplit(";", true);
            foreach (var p in paramList)
            {
                if (p.Trim().StartsWith("database=", StringComparison.OrdinalIgnoreCase))
                {
                    var item = p.Trim().AsSplit("=");
                    return item[1];
                }
            }
            return string.Empty;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 通过SqlDataReader建立DataTable
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private DataTable CreateTableSchema(DbDataReader reader)
        {
            DataTable schema = reader.GetSchemaTable();
            DataTable dataTable = new DataTable();
            if (schema != null)
            {
                foreach (DataRow drow in schema.Rows)
                {
                    string columnName = drow["ColumnName"].ToString();
                    DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
                    dataTable.Columns.Add(column);
                }
            }
            return dataTable;
        }

        /// <summary>
        /// 异步获取数据集（DataTable）
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteDataTableAsync(DbCommand cmd)
        {
            using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                var dataTable = CreateTableSchema(reader);
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var dataRow = dataTable.NewRow();
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        dataRow[i] = reader[i];
                    }
                    dataTable.Rows.Add(dataRow);
                }
                return dataTable;
            }
        }

        /// <summary>
        /// 根据传入的sql语句获取返回的数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private async Task<DataTable> ExecuteDataTableAsync(string sql, params object[] parameters)
        {
            var dt = new DataTable();
            if (_dbType == DataBaseType.MySql)
            {
                using (var sqlcon = new MySqlConnection(ConnectionString))
                {
                    if (sqlcon.State != ConnectionState.Open)
                    {
                        await sqlcon.OpenAsync();
                    }
                    var cmd = new MySqlCommand(sql, sqlcon);
                    if (parameters != null && parameters.Any())
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    cmd.CommandType = CommandType.Text;

                    dt = await ExecuteDataTableAsync(cmd);
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }
            else
            {
                using (var sqlcon = new SqlConnection(ConnectionString))
                {
                    if (sqlcon.State != ConnectionState.Open)
                    {
                        await sqlcon.OpenAsync();
                    }
                    SqlCommand cmd = new SqlCommand(sql, sqlcon);
                    if (parameters != null && parameters.Any())
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    cmd.CommandType = CommandType.Text;

                    dt = await ExecuteDataTableAsync(cmd);
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }


            return dt;
        }

        #endregion

        #endregion

    }
}
