using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.ext
{
    public static partial class SdmapExtensions
    {
        public static Task<IEnumerable<dynamic>> QueryByMapAsync(this IDbConnection cnn, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryAsync(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<T>> QueryByMapAsync<T>(this IDbConnection cnn, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<T> QueryFirstByMapAsync<T>(this IDbConnection cnn, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryFirstAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<T> QueryFirstOrDefaultByMapAsync<T>(this IDbConnection cnn, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<T> QuerySingleByMapAsync<T>(this IDbConnection cnn, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QuerySingleAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<T> QuerySingleOrDefaultByMapAsync<T>(this IDbConnection cnn, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<object>> QueryByMapAsync(this IDbConnection cnn, 
            Type type, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryAsync(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<object> QueryFirstByMapAsync(this IDbConnection cnn, 
            Type type, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryFirstAsync(type, sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<object> QueryFirstOrDefaultByMapAsync(this IDbConnection cnn, 
            Type type, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryFirstOrDefaultAsync(type, sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<object> QuerySingleByMapAsync(this IDbConnection cnn, 
            Type type, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QuerySingleAsync(type, sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<object> QuerySingleOrDefaultByMapAsync(this IDbConnection cnn, 
            Type type, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QuerySingleOrDefaultAsync(type, sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<int> ExecuteByMapAsync(this IDbConnection cnn, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryByMapAsync<TFirst, TSecond, TReturn>(
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
            return cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<TReturn>> QueryByMapAsync<TFirst, TSecond, TThird, TReturn>(
            this IDbConnection cnn, 
            string sqlMapName, 
            Func<TFirst, TSecond, TThird, TReturn> map, 
            object param = null, 
            IDbTransaction transaction = null, 
            bool buffered = true, 
            string splitOn = "Id", 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<TReturn>> QueryByMapAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
            this IDbConnection cnn, 
            string sqlMapName, 
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map, 
            object param = null, 
            IDbTransaction transaction = null, 
            bool buffered = true, 
            string splitOn = "Id", 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<TReturn>> QueryByMapAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
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
            return cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<TReturn>> QueryByMapAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
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
            return cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<TReturn>> QueryByMapAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
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
            return cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<TReturn>> QueryByMapAsync<TReturn>(this IDbConnection cnn, 
            string sqlMapName, 
            Type[] types, 
            Func<object[], TReturn> map, 
            object param = null, 
            IDbTransaction transaction = null, 
            bool buffered = true, 
            string splitOn = "Id", 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryAsync(sql, types, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<SqlMapper.GridReader> QueryMultipleByMapAsync(
            this IDbConnection cnn, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<IDataReader> ExecuteReaderByMapAsync(this IDbConnection cnn, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.ExecuteReaderAsync(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<object> ExecuteScalarByMapAsync(this IDbConnection cnn, 
            string sqlMapName, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlMapName, param);
            return cnn.ExecuteScalarAsync(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
