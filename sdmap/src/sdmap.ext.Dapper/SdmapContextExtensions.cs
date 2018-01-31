using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace sdmap.ext
{
    public static class SdmapContextExtensions
    {
        public static int Execute(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.Execute(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static IDataReader ExecuteReader(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.ExecuteReader(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static object ExecuteScalar(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.ExecuteReader(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static T ExecuteScalar<T>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.ExecuteScalar<T>(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static IEnumerable<dynamic> Query(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.Query(statement, parameters, transaction, buffered, commandTimeout, commandType);
        }

        public static IEnumerable<T> Query<T>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.Query<T>(statement, parameters, transaction, buffered, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Func<TFirst, TSecond, TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.Query(statement, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Func<TFirst, TSecond, TThird, TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null
        )
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.Query(statement, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth,
            TReturn>(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null
        )
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.Query(statement, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth,
            TFifth, TReturn>(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.Query(statement, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth,
            TFifth, TSixth, TReturn>(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.Query(statement, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth,
            TFifth, TSixth, TSeventh, TReturn>(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.Query(statement, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static dynamic QueryFirst(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryFirst(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static object QueryFirst(this SdmapContext ctx, IDbConnection cnn,
            Type type,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryFirst(type, statement, parameters, transaction, commandTimeout, commandType);
        }

        public static T QueryFirst<T>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryFirst<T>(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static dynamic QueryFirstOrDefault(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryFirstOrDefault(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static object QueryFirstOrDefault(this SdmapContext ctx, IDbConnection cnn,
            Type type,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryFirstOrDefault(type, statement, parameters, transaction, commandTimeout, commandType);
        }

        public static T QueryFirstOrDefault<T>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryFirstOrDefault<T>(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static dynamic QuerySingle(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QuerySingle(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static object QuerySingle(this SdmapContext ctx, IDbConnection cnn,
            Type type,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QuerySingle(type, statement, parameters, transaction, commandTimeout, commandType);
        }

        public static T QuerySingle<T>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QuerySingle<T>(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static dynamic QuerySingleOrDefault(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QuerySingleOrDefault(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static object QuerySingleOrDefault(
            this SdmapContext ctx, IDbConnection cnn,
            Type type,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QuerySingleOrDefault(type, statement, parameters, transaction, commandTimeout, commandType);
        }

        public static T QuerySingleOrDefault<T>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QuerySingleOrDefault<T>(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static SqlMapper.GridReader QueryMultiple(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryMultiple(statement, parameters, transaction, commandTimeout, commandType);
        }

        // asyncs
        public static Task<IEnumerable<dynamic>> QueryAsync(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryAsync(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<IEnumerable<T>> QueryAsync<T>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryAsync<T>(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<T> QueryFirstAsync<T>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryFirstAsync<T>(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<T> QueryFirstOrDefaultAsync<T>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryFirstOrDefaultAsync<T>(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<T> QuerySingleAsync<T>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QuerySingleAsync<T>(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<T> QuerySingleOrDefaultAsync<T>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QuerySingleOrDefaultAsync<T>(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<IEnumerable<object>> QueryAsync(this SdmapContext ctx, IDbConnection cnn,
            Type type,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryAsync(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<object> QueryFirstAsync(this SdmapContext ctx, IDbConnection cnn,
            Type type,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryFirstAsync(type, statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<object> QueryFirstOrDefaultAsync(this SdmapContext ctx, IDbConnection cnn,
            Type type,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryFirstOrDefaultAsync(type, statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<object> QuerySingleAsync(this SdmapContext ctx, IDbConnection cnn,
            Type type,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QuerySingleAsync(type, statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<object> QuerySingleOrDefaultAsync(this SdmapContext ctx, IDbConnection cnn,
            Type type,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QuerySingleOrDefaultAsync(type, statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<int> ExecuteAsync(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.ExecuteAsync(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static async Task<T> ExecuteScalarAsync<T>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return await cnn.ExecuteScalarAsync<T>(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Func<TFirst, TSecond, TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryAsync(statement, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Func<TFirst, TSecond, TThird, TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryAsync(statement, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryAsync(statement, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryAsync(statement, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryAsync(statement, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryAsync(statement, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TReturn>(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            Type[] types,
            Func<object[], TReturn> map,
            object parameters = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryAsync(statement, types, map, parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static Task<SqlMapper.GridReader> QueryMultipleAsync(
            this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.QueryMultipleAsync(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<IDataReader> ExecuteReaderAsync(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.ExecuteReaderAsync(statement, parameters, transaction, commandTimeout, commandType);
        }

        public static Task<object> ExecuteScalarAsync(this SdmapContext ctx, IDbConnection cnn,
            string statementId,
            object parameters = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var statement = ctx.Emit(statementId, parameters);
            return cnn.ExecuteScalarAsync(statement, parameters, transaction, commandTimeout, commandType);
        }
    }
}
