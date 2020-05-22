using Hanlp.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanLPDotNet
{
    /// <summary>
    /// https://github.com/hankcs/HanLP （GitHub）
    /// https://www.hankcs.com/nlp/hanlp.html（官网）
    /// 首次运行请修改 初始化 java.lang.System.getProperties().setProperty("java.class.path", @"XXXX\HanLPDotNet\Package\java\hanlp");
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //查看HanLPHelper中其他demo
            //首次运行缺少dll 请在 HanLP\HanLPDotNet\Package\java\hanlp   加载hanlp-1.7.7.dll（由hanlp-1.7.7转化获得）
            //nuget获取的IKVM.OpenJDK 如果出现版本不匹配 尝试加载IKVM.OpenJDK.Core.dll，IKVM.OpenJDK.Text.dll，IKVM.OpenJDK.Util.dll，IKVM.Runtime.dll
            
            //1.1首次运行需要下载语料库 https://file.hankcs.com/hanlp/data-for-1.7.5.zip  
            //1.2酒店评论情绪分析训练库 http://file.hankcs.com/corpus/ChnSentiCorp.zip
            //2.配置hanlp语料库文件夹，并配置hanlp.properties文件 root=G:/MN测试Project/NetCore/第三方框架/IKVMConsole/IKVMConsole/Package/java/hanlp
            //3.如出现缺少dll将ikvmbin-8.1.5717.0.rar 中相关的openjdk dll复制到bin中
            var nlpDemo = new HanLPHelper(@"XXXX\HanLPDotNet\Package\java\hanlp");
            nlpDemo.Segement("吃葡萄不吐葡萄皮，你好啊");
            // nlpDemo.Classifier_NaiveBayes();
            nlpDemo.demo_use_AhoCorasickDoubleArrayTrieSegment();
            Console.Read();
        }
    }
}
