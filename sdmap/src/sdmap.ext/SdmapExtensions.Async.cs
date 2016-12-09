using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Extensions
{
    public static partial class SdmapExtensions
    {
        public static Task<IEnumerable<dynamic>> QueryByIdAsync(this IDbConnection cnn, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryAsync(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<T>> QueryByIdAsync<T>(this IDbConnection cnn, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<T> QueryFirstByIdAsync<T>(this IDbConnection cnn, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryFirstAsync<T>(sqlId, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<T> QueryFirstOrDefaultByIdAsync<T>(this IDbConnection cnn, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<T> QuerySingleByIdAsync<T>(this IDbConnection cnn, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QuerySingleAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<T> QuerySingleOrDefaultByIdAsync<T>(this IDbConnection cnn, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<object>> QueryByIdAsync(this IDbConnection cnn, 
            Type type, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryAsync(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<object> QueryFirstByIdAsync(this IDbConnection cnn, 
            Type type, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryFirstAsync(type, sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<object> QueryFirstOrDefaultByIdAsync(this IDbConnection cnn, 
            Type type, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryFirstOrDefaultAsync(type, sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<object> QuerySingleByIdAsync(this IDbConnection cnn, 
            Type type, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QuerySingleAsync(type, sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<object> QuerySingleOrDefaultByIdAsync(this IDbConnection cnn, 
            Type type, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QuerySingleOrDefaultAsync(type, sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<int> ExecuteByIdAsync(this IDbConnection cnn, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryByIdAsync<TFirst, TSecond, TReturn>(
            this IDbConnection cnn, 
            string sqlId, 
            Func<TFirst, TSecond, TReturn> map, 
            object param = null, 
            IDbTransaction transaction = null, 
            bool buffered = true, 
            string splitOn = "Id", 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<TReturn>> QueryByIdAsync<TFirst, TSecond, TThird, TReturn>(
            this IDbConnection cnn, 
            string sqlId, 
            Func<TFirst, TSecond, TThird, TReturn> map, 
            object param = null, 
            IDbTransaction transaction = null, 
            bool buffered = true, 
            string splitOn = "Id", 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<TReturn>> QueryByIdAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
            this IDbConnection cnn, 
            string sqlId, 
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map, 
            object param = null, 
            IDbTransaction transaction = null, 
            bool buffered = true, 
            string splitOn = "Id", 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<TReturn>> QueryByIdAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            this IDbConnection cnn, 
            string sqlId, 
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, 
            object param = null, 
            IDbTransaction transaction = null, 
            bool buffered = true, 
            string splitOn = "Id", 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<TReturn>> QueryByIdAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            this IDbConnection cnn, 
            string sqlId, 
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, 
            object param = null, 
            IDbTransaction transaction = null, 
            bool buffered = true, 
            string splitOn = "Id", 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<TReturn>> QueryByIdAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            this IDbConnection cnn, 
            string sqlId, 
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            object param = null, 
            IDbTransaction transaction = null, 
            bool buffered = true, 
            string splitOn = "Id", 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<IEnumerable<TReturn>> QueryByIdAsync<TReturn>(this IDbConnection cnn, 
            string sqlId, 
            Type[] types, 
            Func<object[], TReturn> map, 
            object param = null, 
            IDbTransaction transaction = null, 
            bool buffered = true, 
            string splitOn = "Id", 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryAsync(sql, types, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        
        public static Task<SqlMapper.GridReader> QueryMultipleByIdAsync(
            this IDbConnection cnn, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<IDataReader> ExecuteReaderByIdAsync(this IDbConnection cnn, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.ExecuteReaderAsync(sql, param, transaction, commandTimeout, commandType);
        }
        
        public static Task<object> ExecuteScalarByIdAsync(this IDbConnection cnn, 
            string sqlId, 
            object param = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null, 
            CommandType? commandType = null)
        {
            var sql = EmitSql(sqlId, param);
            return cnn.ExecuteScalarAsync(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
