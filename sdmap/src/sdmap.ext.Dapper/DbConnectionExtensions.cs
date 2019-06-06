using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Abstractions;
using System.Reflection;

namespace sdmap.ext.Dapper
{
    public static partial class DbConnectionExtensions
    {
        private static ISdmapEmiter SqlEmiter;

        public static void SetSqlDirectory(string sqlDirectory)
        {
            SetSqlEmiter(FileSystemSqlEmiter.FromSqlDirectory(sqlDirectory));
        }

        public static void SetSqlDirectory(string sqlDirectory, IFileSystem fileSystem)
        {
            SetSqlEmiter(FileSystemSqlEmiter.FromSqlDirectory(sqlDirectory, fileSystem));
        }

        public static void SetSqlDirectoryAndWatch(string sqlDirectory)
        {
            SetSqlEmiter(FileSystemSqlEmiter.FromSqlDirectoryAndWatch(sqlDirectory));
        }

        public static void SetSqlDirectoryAndWatch(string sqlDirectory, IFileSystem fileSystem)
        {
            SetSqlEmiter(FileSystemSqlEmiter.FromSqlDirectoryAndWatch(sqlDirectory, fileSystem));
        }

        public static void SetEmbeddedSqlAssembly(Assembly assembly)
        {
            SetSqlEmiter(EmbeddedResourceSqlEmiter.CreateFrom(assembly));
        }

        public static void SetEmbeddedSqlAssemblies(params Assembly[] assemblies)
        {
            SetSqlEmiter(MultipleAssemblyEmbeddedResourceSqlEmiter.CreateFrom(assemblies));
        }

        public static void SetSqlEmiter(ISdmapEmiter sqlEmiter)
        {
            SqlEmiter = sqlEmiter;
        }

        public static string EmitSql(string sqlMapId, object param)
        {
            return SqlEmiter.Emit(sqlMapId, param);
        }

        public static int ExecuteByMap(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        public static IDataReader ExecuteReaderByMap(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        }

        public static object ExecuteScalarByMap(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        }

        public static T ExecuteScalarByMap<T>(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public static IEnumerable<dynamic> QueryByMap(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.Query(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public static IEnumerable<T> QueryByMap<T>(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> QueryByMap<TFirst, TSecond, TReturn>(
            this IDbConnection cnn,
            string sqlMapName,
            Func<TFirst, TSecond, TReturn> map,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> QueryByMap<TFirst, TSecond, TThird, TReturn>(
            this IDbConnection cnn,
            string sqlMapName,
            Func<TFirst, TSecond, TThird, TReturn> map,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null
        )
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> QueryByMap<TFirst, TSecond, TThird, TFourth,
            TReturn>(
            this IDbConnection cnn,
            string sqlMapName,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null
        )
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> QueryByMap<TFirst, TSecond, TThird, TFourth,
            TFifth, TReturn>(
            this IDbConnection cnn,
            string sqlMapName,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> QueryByMap<TFirst, TSecond, TThird, TFourth,
            TFifth, TSixth, TReturn>(
            this IDbConnection cnn,
            string sqlMapName,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> QueryByMap<TFirst, TSecond, TThird, TFourth,
            TFifth, TSixth, TSeventh, TReturn>(
            this IDbConnection cnn,
            string sqlMapName,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static dynamic QueryFirstByMap(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryFirst(sql, param, transaction, commandTimeout, commandType);
        }

        public static object QueryFirstByMap(this IDbConnection cnn,
            Type type,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryFirst(type, sql, param, transaction, commandTimeout, commandType);
        }

        public static T QueryFirstByMap<T>(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryFirst<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public static dynamic QueryFirstOrDefaultByMap(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryFirstOrDefault(sql, param, transaction, commandTimeout, commandType);
        }

        public static object QueryFirstOrDefaultByMap(this IDbConnection cnn,
            Type type,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryFirstOrDefault(type, sql, param, transaction, commandTimeout, commandType);
        }

        public static T QueryFirstOrDefaultByMap<T>(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public static dynamic QuerySingleByMap(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QuerySingle(sql, param, transaction, commandTimeout, commandType);
        }

        public static object QuerySingleByMap(this IDbConnection cnn,
            Type type,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QuerySingle(type, sql, param, transaction, commandTimeout, commandType);
        }

        public static T QuerySingleByMap<T>(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QuerySingle<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public static dynamic QuerySingleOrDefaultByMap(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QuerySingleOrDefault(sql, param, transaction, commandTimeout, commandType);
        }

        public static object QuerySingleOrDefaultByMap(
            this IDbConnection cnn,
            Type type,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QuerySingleOrDefault(type, sql, param, transaction, commandTimeout, commandType);
        }

        public static T QuerySingleOrDefaultByMap<T>(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QuerySingleOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public static SqlMapper.GridReader QueryMultipleByMap(this IDbConnection cnn,
            string sqlMapName,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryMultiple(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
