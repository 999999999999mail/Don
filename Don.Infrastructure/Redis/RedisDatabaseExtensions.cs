using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Don.Infrastructure.Redis
{
    public static class RedisDatabaseExtensions
    {
        private static Object thisLock = new Object();

        /// <summary>
        /// 保存一个对象。如果键已经拥有一个值，将被覆盖。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool ObjectSet<T>(this IDatabase database, string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            string json = SerializeObject(value);
            return database.StringSet(key, json, expiry);
        }

        /// <summary>
        /// 保存一个对象。如果键已经拥有一个值，将被覆盖。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> ObjectSetAsync<T>(this IDatabase database, string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            string json = SerializeObject(value);
            return await database.StringSetAsync(key, json, expiry);
        }

        /// <summary>
        /// 获取键对应的值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ObjectGet<T>(this IDatabase database, string key)
        {
            var json = database.StringGet(key);
            return DeserializeObject<T>(json);
        }

        /// <summary>
        /// 获取键对应的值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> ObjectGetAsync<T>(this IDatabase database, string key)
        {
            var json = await database.StringGetAsync(key);
            return DeserializeObject<T>(json);
        }

        /// <summary>
        /// 删除所有与指定值（如果值为对象，则将其转换成Json字符串比较相等）相等的元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>返回删除元素的数量。</returns>
        public static long ListRemove<T>(this IDatabase database, string key, T value)
        {
            string json = SerializeObject(value);
            return database.ListRemove(key, json);
        }

        /// <summary>
        /// 删除所有与指定值（如果值为对象，则将其转换成Json字符串比较相等）相等的元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>返回删除元素的数量。</returns>
        public static async Task<long> ListRemoveAsync<T>(this IDatabase database, string key, T value)
        {
            string json = SerializeObject(value);
            return await database.ListRemoveAsync(key, json);
        }

        /// <summary>
        /// 返回键中存储的列表的所有元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IList<T> ListGet<T>(this IDatabase database, string key)
        {
            var values = database.ListRange(key);
            IList<T> list = null;
            foreach (var item in values)
            {
                var obj = DeserializeObject<T>(item);
                if (list == null)
                {
                    list = new List<T>();
                }
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// 返回键中存储的列表的所有元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<IList<T>> ListGetAsync<T>(this IDatabase database, string key)
        {
            var values = await database.ListRangeAsync(key);
            IList<T> list = null;
            foreach (var item in values)
            {
                var obj = DeserializeObject<T>(item);
                if (list == null)
                {
                    list = new List<T>();
                }
                list.Add(obj);
            }
            return list;
        }

        public static IList<T> ListRange<T>(this IDatabase database, string key)
        {
            var values = database.ListRange(key);

            IList<T> list = new List<T>();
            foreach (var item in values)
            {
                var obj = DeserializeObject<T>(item);
                list.Add(obj);
            }
            return list;
        }

        public static async Task<IList<T>> ListRangeAsync<T>(this IDatabase database, string key)
        {
            var values = await database.ListRangeAsync(key);

            IList<T> list = new List<T>();
            foreach (var item in values)
            {
                var obj = DeserializeObject<T>(item);
                list.Add(obj);
            }
            return list;
        }


        /// <summary>
        /// 将指定的值插入存储在键的列表尾部。如果键不存在，则在执行push操作之前创建为空列表。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>返回在push操作之后列表的长度。</returns>
        public static long ListRightPush<T>(this IDatabase database, string key, T value)
        {
            string json = SerializeObject(value);
            return database.ListRightPush(key, json);
        }

        /// <summary>
        /// 将指定的值插入存储在键的列表尾部。如果键不存在，则在执行push操作之前创建为空列表。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns>返回在push操作之后列表的长度。</returns>
        public static long ListRightPush<T>(this IDatabase database, string key, IList<T> values)
        {
            long length = database.ListLength(key);

            foreach (var item in values)
            {
                string json = SerializeObject(item);
                length = database.ListRightPush(key, json);
            }

            return length;
        }

        public static long ListSet<T>(this IDatabase database, string key, IList<T> values)
        {
            lock (thisLock)
            {
                database.KeyDelete(key);

                foreach (var item in values)
                {
                    string json = SerializeObject(item);
                    database.ListRightPush(key, json);
                }

                return values.Count;
            }
        }

        public static async Task<long> ListSetAsync<T>(this IDatabase database, string key, IList<T> values)
        {
            foreach (var item in values)
            {
                string json = SerializeObject(item);
                await database.ListRemoveAsync(key, json);
                await database.ListRightPushAsync(key, json);
            }

            return values.Count;
        }

        /// <summary>
        /// 将指定的值插入存储在键的列表尾部。如果键不存在，则在执行push操作之前创建为空列表。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>返回在push操作之后列表的长度。</returns>
        public static async Task<long> ListRightPushAsync<T>(this IDatabase database, string key, T value)
        {
            string json = SerializeObject(value);
            return await database.ListRightPushAsync(key, json);
        }

        /// <summary>
        /// 将指定的值插入存储在键的列表尾部。如果键不存在，则在执行push操作之前创建为空列表。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns>返回在push操作之后列表的长度。</returns>
        public static async Task<long> ListRightPushAsync<T>(this IDatabase database, string key, IList<T> values)
        {
            long length = await database.ListLengthAsync(key);

            foreach (var item in values)
            {
                string json = SerializeObject(item);
                length = await database.ListRightPushAsync(key, json);
            }

            return length;
        }

        /// <summary>
        /// 删除并返回键中存储的列表的最后一个元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ListRightPop<T>(this IDatabase database, string key)
        {
            var json = database.ListRightPop(key);
            return DeserializeObject<T>(json);
        }

        /// <summary>
        /// 删除并返回键中存储的列表的最后一个元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> ListRightPopAsync<T>(this IDatabase database, string key)
        {
            var json = await database.ListRightPopAsync(key);
            return DeserializeObject<T>(json);
        }

        /// <summary>
        /// 将指定的值插入存储在key的列表的头部。如果键不存在，则在执行push操作之前创建为空列表。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ListLeftPush<T>(this IDatabase database, string key, T value)
        {
            string json = SerializeObject(value);
            return database.ListLeftPush(key, json);
        }

        /// <summary>
        /// 将指定的值插入存储在key的列表的头部。如果键不存在，则在执行push操作之前创建为空列表。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static long ListLeftPush<T>(this IDatabase database, string key, IList<T> values)
        {
            long length = database.ListLength(key);

            foreach (var item in values)
            {
                string json = SerializeObject(item);
                length = database.ListLeftPush(key, json);
            }

            return length;
        }

        /// <summary>
        /// 将指定的值插入存储在key的列表的头部。如果键不存在，则在执行push操作之前创建为空列表。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>返回在push操作之后列表的长度。</returns>
        public static async Task<long> ListLeftPushAsync<T>(this IDatabase database, string key, T value)
        {
            string json = SerializeObject(value);
            return await database.ListLeftPushAsync(key, json);
        }

        /// <summary>
        /// 将指定的值插入存储在key的列表的头部。如果键不存在，则在执行push操作之前创建为空列表。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns>返回在push操作之后列表的长度。</returns>
        public static async Task<long> ListLeftPushAsync<T>(this IDatabase database, string key, IList<T> values)
        {
            long length = await database.ListLengthAsync(key);

            for (int i = values.Count - 1; i >= 0; i--)
            {
                string json = SerializeObject(values[i]);
                length = await database.ListLeftPushAsync(key, json);
            }
            //foreach (var item in values)
            //{
            //    string json = SerializeObject(item);
            //    length = await database.ListLeftPushAsync(key, json);
            //}

            return length;
        }

        /// <summary>
        /// 删除并返回键中存储的列表的第一个元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ListLeftPop<T>(this IDatabase database, string key)
        {
            var json = database.ListLeftPop(key);
            return DeserializeObject<T>(json);
        }

        /// <summary>
        /// 删除并返回键中存储的列表的第一个元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> ListLeftPopAsync<T>(this IDatabase database, string key)
        {
            var json = await database.ListLeftPopAsync(key);
            return DeserializeObject<T>(json);
        }
        /// <summary>
        /// 根据索引更改列表中某项数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public static void ListSetByIndex<T>(this IDatabase database, string key, long index, T value)
        {
            string json = SerializeObject(value);
            database.ListSetByIndex(key, index, json);
        }
        /// <summary>
        /// 根据索引更改列表中某项数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task ListSetByIndexAsync<T>(this IDatabase database, string key, long index, T value)
        {
            string json = SerializeObject(value);
            await database.ListSetByIndexAsync(key, index, json);
        }
        /// <summary>
        /// 根据索引获取列表中某项数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T ListGetByIndex<T>(this IDatabase database, string key, long index)
        {
            var json = database.ListGetByIndex(key, index);
            return DeserializeObject<T>(json);
        }
        /// <summary>
        /// 根据索引获取列表中某项数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static async Task<T> ListGetByIndexAsync<T>(this IDatabase database, string key, long index)
        {
            var json = await database.ListGetByIndexAsync(key, index);
            return DeserializeObject<T>(json);
        }
        /// <summary>
        /// 将指定的对象序列化为JSON字符串。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string SerializeObject(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将JSON反序列化为指定的. net类型。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        private static T DeserializeObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                var data = memoryStream.ToArray();
                return data;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(data))
            {
                var result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
    }
}
