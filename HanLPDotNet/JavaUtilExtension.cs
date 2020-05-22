using System;
using System.Collections.Generic;
 
using com.hankcs.hanlp.collection.AhoCorasick;
using java.util;

using System.Dynamic;
using static com.hankcs.hanlp.collection.trie.bintrie.BaseNode;
/// <summary>
/// 扩展类
/// </summary>
namespace JavaUtilExtension
{
    public class AhoCorasickDoubleArrayTrieHitEx : AhoCorasickDoubleArrayTrie.IHit
    {
        private string txt;
        public AhoCorasickDoubleArrayTrieHitEx(string str)
        {
            txt = str;
        }
        public void hit(int begin, int end, object value)
        {
            Console.WriteLine($"[{begin}:{end}]={txt.Substring(begin, end - begin)} {value}\n");
        }
    }

    public static class JavaListExtensiton
    {
    
        /// <summary>
        /// javaList To List
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        private static List<string> SegmentListToList(java.util.List terms)
        {
            var docList = new List<string>();
            for (int i = terms.size() - 1; i >= 0; i--)
            {
                var wordStr = terms.get(i).ToString();
                docList.Add(wordStr);
            }
            return docList;
        }
        /// <summary>
        ///  返回文本对象
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<string> ToList(this java.util.List list)
        {
            return SegmentListToList(list);
        }
   

        /// <summary>
        /// 返回bsonDoc对象
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this java.util.List terms)
        {
            var docList = new List<T>();
            for (int i = terms.size() - 1; i >= 0; i--)
            {
                var wordStr = terms.get(i);
                docList.Add((T)wordStr);
            }
            return docList;
        }


        /// <summary>
        /// 返回bsonDoc对象
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this Set collection)
        {
            var docList = new List<T>();

            var iterator = collection.iterator();
            
            while (iterator.hasNext())
            {
                var entry = iterator.next();
                docList.Add((T)entry);
            }
            return docList;
        }

        /// <summary>
        /// 返回bsonDoc对象
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TrieEntry> ToList(this Set collection)
        { 
            return collection.ToList<TrieEntry>();
        }

    }
}
