using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.IO;

using com.hankcs.hanlp.mining.cluster;

using System.Security.Policy;
using com.hankcs.hanlp.mining.word2vec;
using sun.net.idn;

using com.hankcs.hanlp.seg;
using com.hankcs.hanlp;
using com.hankcs.hanlp.seg.common;
using com.hankcs.hanlp.tokenizer;
using com.hankcs.hanlp.seg.NShort;
using com.hankcs.hanlp.seg.Dijkstra;
using com.hankcs.hanlp.seg.CRF;
using com.hankcs.hanlp.dictionary;
using com.hankcs.hanlp.collection.AhoCorasick;
using System.Net.Mime;
using java.util;
using System.Runtime.CompilerServices;
using com.hankcs.hanlp.dictionary.py;
using com.hankcs.hanlp.suggest;
using com.hankcs.hanlp.corpus.dependency.CoNll;
using JavaUtilExtension;
using com.hankcs.hanlp.model.perceptron;
using com.hankcs.hanlp.dictionary.stopword;
using com.hankcs.hanlp.corpus.occurrence;
using static com.hankcs.hanlp.collection.trie.bintrie.BaseNode;
using com.hankcs.hanlp.seg.Other;

namespace Hanlp.Extension
{
    /// <summary>
    /// @software{hanlp2,
    /// author = {Han He},
    /// title = {{HanLP: Han Language Processing}},
    /// year = {2020},
    /// url = {https://github.com/hankcs/HanLP},
    ///}
    /// </summary>
    class HanLPHelper
    {



        public HanLPHelper()
        {
            java.lang.System.getProperties().setProperty("java.class.path", @"XXXX\HanLPDotNet\Package\java\hanlp");
        }
        public HanLPHelper(string hanlpPath)
        {
            java.lang.System.getProperties().setProperty("java.class.path", hanlpPath);
        }

        #region 测试用例

        #region 分词
        /// <summary>
        /// 第一个demo
        /// </summary>
        /// <param name="str"></param>
        public void Segement(string str)
        {
            var docList = HanLP.segment(str).ToList();
            Console.WriteLine(docList);
        }

        /// <summary>
        /// 标准分词
        /// https://www.hankcs.com/nlp/segment/the-word-graph-is-generated.html
        /// </summary>
        /// <param name="str"></param>
        public void Segement_Standard(string str = "商品和服务")
        {
            var termList = StandardTokenizer.segment(str);
            var docList = termList.ToList();
            Console.WriteLine(docList);
        }

        /// <summary>
        /// NLP分词
        /// </summary>
        /// <param name="str"></param>
        public void Segement_NLP(string str = "中国科学院计算技术研究所的宗成庆教授正在教授自然语言处理课程")
        {
            var termList = NLPTokenizer.segment(str);
            var docList = termList.ToList();
            Console.WriteLine(docList);
        }

        /// <summary>
        /// 索引分词
        /// 
        /// </summary>
        /// <param name="str"></param>
        public void Segement_Index(string str = "中国科学院计算技术研究所的宗成庆教授正在教授自然语言处理课程")
        {
            var termList = IndexTokenizer.segment(str);
            var docList = termList.ToList();
            Console.WriteLine(docList);
        }


        /// <summary>
        /// N-最短路径分词
        /// https://www.hankcs.com/nlp/segment/n-shortest-path-to-the-java-implementation-and-application-segmentation.html
        /// </summary>
        /// <param name="str"></param>
        public void Segement_NShort(string[] str)
        {
            Segment nShortSegment = new NShortSegment().enableCustomDictionary(false).enablePlaceRecognize(true).enableOrganizationRecognize(true);
            Segment shortestSegment = new DijkstraSegment().enableCustomDictionary(false).enablePlaceRecognize(true).enableOrganizationRecognize(true);
            string[] testCase = new string[]{
        "今天，刘志军案的关键人物,山西女商人丁书苗在市二中院出庭受审。",
        "刘喜杰石国祥会见吴亚琴先进事迹报告团成员",
          };
            foreach (var sentence in testCase)
            {
                Console.WriteLine("N-最短分词：" + nShortSegment.seg(sentence) + "\n最短路分词：" + shortestSegment.seg(sentence));
            }
        }


        /// <summary>
        /// CRF 分词
        ///https://www.hankcs.com/nlp/segment/crf-segmentation-of-the-pure-java-implementation.html
        ///https://www.hankcs.com/nlp/the-crf-model-format-description.html
        /// </summary>
        /// <param name="str"></param>

        public void Segement_CRF(string[] str)
        {
            HanLP.Config.ShowTermNature = false;    // 关闭词性显示
            Segment segment = new CRFSegment();
            String[] sentenceArray = new String[]
                    {
                        "HanLP是由一系列模型与算法组成的Java工具包，目标是普及自然语言处理在生产环境中的应用。",
                        "鐵桿部隊憤怒情緒集結 馬英九腹背受敵",           // 繁体无压力
                        "馬英九回應連勝文“丐幫說”：稱黨內同志談話應謹慎",
                        "高锰酸钾，强氧化剂，紫红色晶体，可溶于水，遇乙醇即被还原。常用作消毒剂、水净化剂、氧化剂、漂白剂、毒气吸收剂、二氧化碳精制剂等。", // 专业名词有一定辨识能力
                        "《夜晚的骰子》通过描述浅草的舞女在暗夜中扔骰子的情景,寄托了作者对庶民生活区的情感",    // 非新闻语料
                        "这个像是真的[委屈]前面那个打扮太江户了，一点不上品...@hankcs",                       // 微博
                        "鼎泰丰的小笼一点味道也没有...每样都淡淡的...淡淡的，哪有食堂2A的好次",
                        "克里斯蒂娜·克罗尔说：不，我不是虎妈。我全家都热爱音乐，我也鼓励他们这么做。",
                        "今日APPS：Sago Mini Toolbox培养孩子动手能力",
                        "财政部副部长王保安调任国家统计局党组书记",
                        "2.34米男子娶1.53米女粉丝 称夫妻生活没问题",
                        "你看过穆赫兰道吗",
                        "乐视超级手机能否承载贾布斯的生态梦"
                    };
            foreach (var sentence in sentenceArray)
            {
                var termList = segment.seg(sentence);
                Console.WriteLine(termList);
            }
        }


        /// <summary>
        /// 极速 分词
        ///https://www.hankcs.com/program/algorithm/aho-corasick-double-array-trie.html
        ///极速分词是词典最长分词，速度极其快，精度一般。
        ///在i7上跑出了2000万字每秒的速度。
        /// </summary>
        /// <param name="str"></param>

        public void Segement_HighSpeed(string[] str)
        {
            String text = "江西鄱阳湖干枯，中国最大淡水湖变成大草原";
            Console.WriteLine(SpeedTokenizer.segment(text));

            int pressure = 1000000;
            for (int i = 0; i < pressure; ++i)
            {
                SpeedTokenizer.segment(text);
            }

        }
        /// <summary>
        /// CustomDictionary是一份全局的用户自定义词典，可以随时增删，影响全部分词器。
        /// CustomDictionary主词典文本路径是data/dictionary/custom/CustomDictionary.txt，
        /// 用户可以在此增加自己的词语（不推荐）；
        /// 也可以单独新建一个文本文件，
        /// 通过配置文件CustomDictionaryPath=data/dictionary/custom/CustomDictionary.txt; 
        /// 我的词典.txt;来追加词典（推荐）。
        /// 始终建议将相同词性的词语放到同一个词典文件里，便于维护和分享。
        /// 词典格式
        ///每一行代表一个单词，格式遵从[单词][词性A][A的频次][词性B][B的频次] ... 如果不填词性则表示采用词典的默认词性。
        ///词典的默认词性默认是名词n，可以通过配置文件修改：全国地名大全.txt ns; 如果词典路径后面空格紧接着词性，则该词典默认是该词性。
        ///关于用户词典的更多信息请参考词典说明一章。
        ///《Trie树分词》https://www.hankcs.com/program/java/tire-tree-participle.html
        ///《Aho Corasick自动机结合DoubleArrayTrie极速多模式匹配》
        ///https://www.hankcs.com/program/algorithm/aho-corasick-double-array-trie.html
        /// </summary>
        /// <param name="str"></param>
        public void Segement_CustomerDic(string str)
        {
            // 动态增加
            CustomDictionary.add("攻城狮");
            // 强行插入
            CustomDictionary.insert("白富美", "nz 1024");
            // 删除词语（注释掉试试）
            //        CustomDictionary.remove("攻城狮");
            Console.WriteLine(CustomDictionary.add("单身狗", "nz 1024 n 1"));
            Console.WriteLine(CustomDictionary.get("单身狗"));

            string text = "攻城狮逆袭单身狗，迎娶白富美，走上人生巅峰";  // 怎么可能噗哈哈！

            // DoubleArrayTrie分词
            var arrayTrieHit = new AhoCorasickDoubleArrayTrieHitEx(text);
            CustomDictionary.parseText(text, arrayTrieHit);
            // 首字哈希之后二分的trie树分词
            BaseSearcher searcher = CustomDictionary.getSearcher(text);
            var entry = searcher.next();
            while (entry != null)
            {
                Console.WriteLine(entry);
                entry = searcher.next();
            }

            // 标准分词
            Console.WriteLine(HanLP.segment(text));

            // Note:动态增删不会影响词典文件
            // 目前CustomDictionary使用DAT储存词典文件中的词语，用BinTrie储存动态加入的词语，前者性能高，后者性能低
            // 之所以保留动态增删功能，一方面是历史遗留特性，另一方面是调试用；未来可能会去掉动态增删特性。
        }
        #endregion
        #region 名词识别
        /// <summary>
        /// 中国人名识别
        /// 目前分词器基本上都默认开启了中国人名识别，比如HanLP.segment()接口中使用的分词器等等，用户不必手动开启；
        /// 代码只是为了强调。
        ///有一定的误命中率，比如误命中关键年，则可以通过在data/dictionary/person/nr.txt加入一条关键
        ///年 A 1来排除关键年作为人名的可能性，也可以将关键年作为新词登记到自定义词典中。
        ///《实战HMM-Viterbi角色标注中国人名识别》
        ///https://www.hankcs.com/nlp/chinese-name-recognition-in-actual-hmm-viterbi-role-labeling.html
        /// </summary>
        /// <param name="str"></param>
        public void NameRecognize(string str)
        {
            string[] testCase = new string[]{
        "签约仪式前，秦光荣、李纪恒、仇和等一同会见了参加签约的企业家。",
        "王国强、高峰、汪洋、张朝阳光着头、韩寒、小四",
        "张浩和胡健康复员回家了",
        "王总和小丽结婚了",
        "编剧邵钧林和稽道青说",
        "这里有关天培的有关事迹",
        "龚学平等领导,邓颖超生前",
        };
            Segment segment = HanLP.newSegment().enableNameRecognize(true);
            foreach (string sentence in testCase)
            {
                var termList = segment.seg(sentence);
                Console.WriteLine(termList);
            }
        }
        /// <summary>
        /// 目前分词器基本上都默认开启了音译人名识别，用户不必手动开启；上面的代码只是为了强调。
        /// 《层叠隐马模型下的音译人名和日本人名识别》
        /// https://www.hankcs.com/nlp/name-transliteration-cascaded-hidden-markov-model-and-japanese-personal-names-recognition.html
        /// </summary>
        /// <param name="str"></param>
        public void TranslatedNameRecognize(string str)
        {
            string[] testCase = new string[]{
                "一桶冰水当头倒下，微软的比尔盖茨、Facebook的扎克伯格跟桑德博格、亚马逊的贝索斯、苹果的库克全都不惜湿身入镜，这些硅谷的科技人，飞蛾扑火似地牺牲演出，其实全为了慈善。",
                "世界上最长的姓名是简森·乔伊·亚历山大·比基·卡利斯勒·达夫·埃利奥特·福克斯·伊维鲁莫·马尔尼·梅尔斯·帕特森·汤普森·华莱士·普雷斯顿。",
        };
            Segment segment = HanLP.newSegment().enableTranslatedNameRecognize(true);
            foreach (var sentence in testCase)
            {
                var termList = segment.seg(sentence);
                Console.WriteLine(termList);
            }
        }

        /// <summary>
        /// 日本人名识别
        /// 目前标准分词器默认关闭了日本人名识别，用户需要手动开启；这是因为日本人名的出现频率较低，但是又消耗性能。
        /// 《层叠隐马模型下的音译人名和日本人名识别》
        /// https://www.hankcs.com/nlp/name-transliteration-cascaded-hidden-markov-model-and-japanese-personal-names-recognition.html
        /// </summary>
        /// <param name="str"></param>
        public void JapaneseNameRecognize(string str)
        {
            string[] testCase = new string[]{
                "一桶冰水当头倒下，微软的比尔盖茨、Facebook的扎克伯格跟桑德博格、亚马逊的贝索斯、苹果的库克全都不惜湿身入镜，这些硅谷的科技人，飞蛾扑火似地牺牲演出，其实全为了慈善。",
                "世界上最长的姓名是简森·乔伊·亚历山大·比基·卡利斯勒·达夫·埃利奥特·福克斯·伊维鲁莫·马尔尼·梅尔斯·帕特森·汤普森·华莱士·普雷斯顿。",
        };
            Segment segment = HanLP.newSegment().enableJapaneseNameRecognize(true);
            foreach (var sentence in testCase)
            {
                var termList = segment.seg(sentence);
                Console.WriteLine(termList);
            }
        }

        /// <summary>
        /// 目前分词器默认关闭了机构名识别，用户需要手动开启；这是因为消耗性能，其实常用机构名都收录在核心词典和用户自定义词典中。
        /// HanLP的目的不是演示动态识别，在生产环境中，能靠词典解决的问题就靠词典解决，这是最高效稳定的方法。
        /// https://www.hankcs.com/nlp/name-transliteration-cascaded-hidden-markov-model-and-japanese-personal-names-recognition.html
        /// </summary>
        /// <param name="str"></param>
        public void OrganizationRecognize(string str)
        {
            string[] testCase = new string[]{
               "我在上海林原科技有限公司兼职工作，",
        "我经常在台川喜宴餐厅吃饭，",
        "偶尔去地中海影城看电影。",
        };
            Segment segment = HanLP.newSegment().enableOrganizationRecognize(true);
            foreach (var sentence in testCase)
            {
                var termList = segment.seg(sentence);
                Console.WriteLine(termList);
            }
        }
        #endregion

        #region 关键字
        /// <summary>
        /// 内部采用TextRankKeyword实现，用户可以直接调用TextRankKeyword.getKeywordList(document, size)
        /// 《TextRank算法提取关键词的Java实现》
        /// https://www.hankcs.com/nlp/textrank-algorithm-to-extract-the-keywords-java-implementation.html
        /// </summary>
        /// <param name="content"></param>
        public void ExtractKeyword(string content)
        {
            content = "程序员(英文Programmer)是从事程序开发、维护的专业人员。一般将程序员分为程序设计人员和程序编码人员，但两者的界限并不非常清楚，特别是在中国。软件从业人员分为初级程序员、高级程序员、系统分析员和项目经理四大类。";
            var keywordList = HanLP.extractKeyword(content, 5);
            Console.WriteLine(keywordList);
        }

        /// <summary>
        /// 短语识别
        /// 内部采用MutualInformationEntropyPhraseExtractor实现，用户可以直接调用MutualInformationEntropyPhraseExtractor..extractPhrase(text, size)。
        /// 《基于互信息和左右信息熵的短语提取识别》
        /// https://www.hankcs.com/nlp/extraction-and-identification-of-mutual-information-about-the-phrase-based-on-information-entropy.html
        /// </summary>
        /// <param name="content"></param>
        public void extractPhrase(string content)
        {
            content = "算法工程师\n" +
        "算法（Algorithm）是一系列解决问题的清晰指令，也就是说，能够对一定规范的输入，在有限时间内获得所要求的输出。" +
        "如果一个算法有缺陷，或不适合于某个问题，执行这个算法将不会解决这个问题。不同的算法可能用不同的时间、" +
        "空间或效率来完成同样的任务。一个算法的优劣可以用空间复杂度与时间复杂度来衡量。算法工程师就是利用算法处理事物的人。\n" +
        "\n" +
        "1职位简介\n" +
        "算法工程师是一个非常高端的职位；\n" +
        "专业要求：计算机、电子、通信、数学等相关专业；\n" +
        "学历要求：本科及其以上的学历，大多数是硕士学历及其以上；\n" +
        "语言要求：英语要求是熟练，基本上能阅读国外专业书刊；\n" +
        "必须掌握计算机相关知识，熟练使用仿真工具MATLAB等，必须会一门编程语言。\n" +
        "\n" +
        "2研究方向\n" +
        "视频算法工程师、图像处理算法工程师、音频算法工程师 通信基带算法工程师\n" +
        "\n" +
        "3目前国内外状况\n" +
        "目前国内从事算法研究的工程师不少，但是高级算法工程师却很少，是一个非常紧缺的专业工程师。" +
        "算法工程师根据研究领域来分主要有音频/视频算法处理、图像技术方面的二维信息算法处理和通信物理层、" +
        "雷达信号处理、生物医学信号处理等领域的一维信息算法处理。\n" +
        "在计算机音视频和图形图像技术等二维信息算法处理方面目前比较先进的视频处理算法：机器视觉成为此类算法研究的核心；" +
        "另外还有2D转3D算法(2D-to-3D conversion)，去隔行算法(de-interlacing)，运动估计运动补偿算法" +
        "(Motion estimation/Motion Compensation)，去噪算法(Noise Reduction)，缩放算法(scaling)，" +
        "锐化处理算法(Sharpness)，超分辨率算法(Super Resolution),手势识别(gesture recognition),人脸识别(face recognition)。\n" +
        "在通信物理层等一维信息领域目前常用的算法：无线领域的RRM、RTT，传送领域的调制解调、信道均衡、信号检测、网络优化、信号分解等。\n" +
        "另外数据挖掘、互联网搜索算法也成为当今的热门方向。\n" +
        "算法工程师逐渐往人工智能方向发展。";
            var phraseList = HanLP.extractPhrase(content, 5).ToList();

            Console.WriteLine(phraseList);
        }


        #endregion
        #region 中文拼音 简繁转换 转换
        /// <summary>
        /// 拼音转换
        /// anLP不仅支持基础的汉字转拼音，还支持声母、韵母、音调、音标和输入法首字母首声母功能。
        ///HanLP能够识别多音字，也能给繁体中文注拼音。
        ///最重要的是，HanLP采用的模式匹配升级到AhoCorasickDoubleArrayTrie，性能大幅提升，能够提供毫秒级的响应速度！
        ///《汉字转拼音与简繁转换的Java实现》
        ///https://www.hankcs.com/nlp/java-chinese-characters-to-pinyin-and-simplified-conversion-realization.html#h2-17
        /// </summary>
        public void PinYinCollection()
        {
            String text = "重载不是重任";
            List<Pinyin> pinyinList = HanLP.convertToPinyinList(text).ToList<Pinyin>();
            Console.WriteLine($"原文,{text}");


            Console.WriteLine("拼音（数字音调）,");
            foreach (Pinyin pinyin in pinyinList)
            {
                Console.WriteLine($"{pinyin},");
            }

            Console.WriteLine("拼音（符号音调）,");
            foreach (Pinyin pinyin in pinyinList)
            {
                Console.WriteLine($"{pinyin.getPinyinWithToneMark()},");
            }

            Console.WriteLine("拼音（无音调）,");
            foreach (Pinyin pinyin in pinyinList)
            {
                Console.WriteLine($"{pinyin.getPinyinWithoutTone()},");
            }

            Console.WriteLine("声调,");
            foreach (Pinyin pinyin in pinyinList)
            {
                Console.WriteLine($"{pinyin.getTone()},");
            }

            Console.WriteLine("声母,");
            foreach (Pinyin pinyin in pinyinList)
            {
                Console.WriteLine($"{pinyin.getShengmu()},");
            }

            Console.WriteLine("韵母,");
            foreach (Pinyin pinyin in pinyinList)
            {
                Console.WriteLine($"{pinyin.getYunmu()},");
            }

            Console.WriteLine("输入法头,");
            foreach (Pinyin pinyin in pinyinList)
            {
                Console.WriteLine($"{pinyin.getHead()},");
            }

        }

        /// <summary>
        /// 简繁转换
        /// HanLP能够识别简繁分歧词，比如打印机=印表機。许多简繁转换工具不能区分“以后”“皇后”中的两个“后”字，HanLP可以。
        /// 《汉字转拼音与简繁转换的Java实现》https://www.hankcs.com/nlp/java-chinese-characters-to-pinyin-and-simplified-conversion-realization.html#h2-17
        /// </summary>
        public void TraditionalChineseCollection()
        {
            Console.WriteLine(HanLP.convertToTraditionalChinese("用笔记本电脑写程序"));
            Console.WriteLine(HanLP.convertToSimplifiedChinese("「以後等妳當上皇后，就能買士多啤梨慶祝了」"));
        }



        #endregion

        #region 文本识别 提取关键字，摘要，依存句法
        /// <summary>
        /// 提取关键字与寨蒿
        /// </summary>
        public void ExtractSummary()
        {
            var content = "商品和服务,";
            content += "结婚的和尚未结婚的确实在干扰分词啊,";
            content += "买水果然后来世博园最后去世博会,";
            content += "中国的首都是北京,";
            content += "欢迎新老师生前来就餐,";
            content += "工信处女干事每月经过下属科室都要亲口交代24口交换机等技术性器件的安装工作,";
            content += "随着页游兴起到现在的页游繁盛，依赖于存档进行逻辑判断的设计减少了，但这块也不能完全忽略掉。";
            //提取关键字
            Console.WriteLine(HanLP.extractKeyword(content, 2));
            // 自动摘要
            Console.WriteLine(HanLP.extractSummary(content, 2));
            Console.WriteLine(HanLP.parseDependency("徐先生还具体帮助他确定了把画雄鹰、松鼠和麻雀作为主攻目标。"));

        }
        /// <summary>
        /// 文本推荐(句子级别，从一系列句子中挑出与输入句子最相似的那一个)
        /// 在搜索引擎的输入框中，用户输入一个词，搜索引擎会联想出最合适的搜索词，HanLP实现了类似的功能。
        /// 可以动态调节每种识别器的权重
        /// 搜索推荐
        /// </summary>
        public void TxtSuggest()
        {
            Suggester suggester = new Suggester();
            string[] titleArray =
            (
                    "威廉王子发表演说 呼吁保护野生动物\n" +
                    "《时代》年度人物最终入围名单出炉 普京马云入选\n" +
                    "“黑格比”横扫菲：菲吸取“海燕”经验及早疏散\n" +
                    "日本保密法将正式生效 日媒指其损害国民知情权\n" +
                    "英报告说空气污染带来“公共健康危机”"
            ).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var title in titleArray)
            {
                suggester.addSentence(title);
            }
            Console.WriteLine(suggester.suggest("发言", 1));       // 语义
            Console.WriteLine(suggester.suggest("危机公共", 1));   // 字符
            Console.WriteLine(suggester.suggest("mayun", 1));      // 拼音
        }

        /// <summary>
        /// 语义距离
        /// 设想的应用场景是搜索引擎对词义的理解，词与词并不只存在“同义词”与“非同义词”的关系，就算是同义词，它们之间的意义也是有微妙的差别的。
        ///为每个词分配一个语义ID，词与词的距离通过语义ID的差得到。语义ID通过《同义词词林扩展版》计算而来。
        /// </summary>
        public void WordDistance()
        {
            String[] wordArray = new String[]
               {
                        "香蕉",
                        "苹果",
                        "白菜",
                        "水果",
                        "蔬菜",
                        "自行车",
                        "公交车",
                        "飞机",
                        "买",
                        "卖",
                        "购入",
                        "新年",
                        "春节",
                        "丢失",
                        "补办",
                        "办理",
                        "送给",
                        "寻找",
                        "孩子",
                        "教室",
                        "教师",
                        "会计",
               };
            foreach (var a in wordArray)
            {
                foreach (var b in wordArray)
                {
                    Console.WriteLine(a + "\t" + b + "\t之间的距离是\t" + CoreSynonymDictionary.distance(a, b));
                }
            }
        }
        /// <summary>
        /// 依存句法分析（神经网络句法模型需要-Xms1g -Xmx1g -Xmn512m）
        /// 内部采用NeuralNetworkDependencyParser实现，用户可以直接调用NeuralNetworkDependencyParser.compute(sentence)
        /// 也可以调用基于MaxEnt的依存句法分析器MaxEntDependencyParser.compute(sentence)
        /// 《基于神经网络的高性能依存句法分析器》
        ///https://www.hankcs.com/nlp/parsing/neural-network-based-dependency-parser.html
        ///《最大熵依存句法分析器的实现》
        ///https://www.hankcs.com/nlp/parsing/to-achieve-the-maximum-entropy-of-the-dependency-parser.html
        ///《基于CRF序列标注的中文依存句法分析器的Java实现》
        ///https://www.hankcs.com/nlp/parsing/crf-sequence-annotation-chinese-dependency-parser-implementation-based-on-java.html
        /// </summary>
        public void DependencyParser()
        {
            CoNLLSentence sentence = HanLP.parseDependency("徐先生还具体帮助他确定了把画雄鹰、松鼠和麻雀作为主攻目标。");
            Console.WriteLine(sentence);
            // 可以方便地遍历它
            foreach (CoNLLWord word in sentence)
            {
                Console.WriteLine($"{word.LEMMA} --({word.DEPREL})--> {word.HEAD.LEMMA}\n");
            }
            // 也可以直接拿到数组，任意顺序或逆序遍历
            CoNLLWord[] wordArray = sentence.getWordArray();
            for (int i = wordArray.Length - 1; i >= 0; i--)
            {
                CoNLLWord word = wordArray[i];
                Console.WriteLine($"{word.LEMMA} --({word.DEPREL})-->{word.HEAD.LEMMA}\n");
            }
            // 还可以直接遍历子树，从某棵子树的某个节点一路遍历到虚根
            CoNLLWord head = wordArray[12];
            while ((head = head.HEAD) != null)
            {
                if (head == CoNLLWord.ROOT)
                {
                    Console.WriteLine(head.LEMMA);
                }
                else
                {
                    Console.WriteLine($"{head.LEMMA} --({head.DEPREL})--> ");
                }
            }
        }



        #endregion

        #region 文本聚类
        /// <summary>
        ///  文本聚类
        /// </summary>
        public void TextClusterAnalyzer()
        {
            ClusterAnalyzer analyzer = new ClusterAnalyzer();
            analyzer.addDocument("赵一", "流行, 流行, 流行, 流行, 流行, 流行, 流行, 流行, 流行, 流行, 蓝调, 蓝调, 蓝调, 蓝调, 蓝调, 蓝调, 摇滚, 摇滚, 摇滚, 摇滚");
            analyzer.addDocument("钱二", "爵士, 爵士, 爵士, 爵士, 爵士, 爵士, 爵士, 爵士, 舞曲, 舞曲, 舞曲, 舞曲, 舞曲, 舞曲, 舞曲, 舞曲, 舞曲");
            analyzer.addDocument("张三", "古典, 古典, 古典, 古典, 民谣, 民谣, 民谣, 民谣");
            analyzer.addDocument("李四", "爵士, 爵士, 爵士, 爵士, 爵士, 爵士, 爵士, 爵士, 爵士, 金属, 金属, 舞曲, 舞曲, 舞曲, 舞曲, 舞曲, 舞曲");
            analyzer.addDocument("王五", "流行, 流行, 流行, 流行, 摇滚, 摇滚, 摇滚, 嘻哈, 嘻哈, 嘻哈");
            analyzer.addDocument("马六", "古典, 古典, 古典, 古典, 古典, 古典, 古典, 古典, 摇滚");

            analyzer.kmeans(3);
            analyzer.repeatedBisection(3);
            analyzer.repeatedBisection(1.0);
            //var obj = analyzer.kmeans(3);
            //  var repeated1 = analyzer.repeatedBisection(100);//重复二分聚类
            var repeated1 = analyzer.repeatedBisection(1.0);  //自动判断聚类数量k
            var result = repeated1.ToList();
            Console.WriteLine(result);
        }

        #endregion
        #endregion
        #region  其他
        /// <summary>
        /// 感知机词法分析器为例
        /// [上海/ns 华安/nz 工业/n （/w 集团/n ）/w 公司/n]/nt 董事长/n 谭旭光/nr 和/c 秘书/n 胡花蕊/nr 来到/v [美国/ns 纽约/ns 现代/t 艺术/n 博物馆/n]/ns 参观/v
        /// </summary>
        public void PerceptronLexicalAnalyzer()
        {
            var analyzer = new PerceptronLexicalAnalyzer();
            var sence = analyzer.analyze("上海华安工业（集团）公司董事长谭旭光和秘书胡花蕊来到美国纽约现代艺术博物馆参观");
        }
        /// <summary>
        /// 情感分析
        /// http://file.hankcs.com/corpus/ChnSentiCorp.zip 下载后解压
        /// </summary>
        public void Classifier_NaiveBayes()
        {
            var chn_senti_corp = @"XXXX\Package\java\hanlp\data\trainData\ChnSentiCorp\ChnSentiCorp情感分析酒店评论";

            var classifier = new com.hankcs.hanlp.classification.classifiers.NaiveBayesClassifier();
            // 创建分类器，更高级的功能请参考IClassifier的接口定义
            classifier.train(chn_senti_corp);
            //  训练后的模型支持持久化，下次就不必训练了
            var result = classifier.predict("前台客房服务态度非常好！早餐很丰富，房价很干净。再接再厉！");
            result = classifier.predict("结果大失所望，灯光昏暗，空间极其狭小，床垫质量恶劣，房间还伴着一股霉味。");
            result = classifier.predict("可利用文本分类实现情感分析，效果不是不行");
        }
        /// <summary>
        ///  停用词的使用教程
        /// </summary>
        public void CoreStopWordDictionaryDemo()
        {

            // var coreStopWordDictionary = new com.hankcs.hanlp.dictionary.stopword.CoreStopWordDictionary();
            //var Filter = new com.hankcs.hanlp.dictionary.stopword.Filter();
            //var Term = new com.hankcs.hanlp.seg.common.Term();
            var BasicTokenizer = new com.hankcs.hanlp.tokenizer.BasicTokenizer();
            var NotionalTokenizer = new com.hankcs.hanlp.tokenizer.NotionalTokenizer();

            var text = "小区居民有的反对喂养流浪猫，而有的居民却赞成喂养这些小宝贝";
            // 可以动态修改停用词词典
            CoreStopWordDictionary.add("居民");
            print(NotionalTokenizer.segment(text));
            CoreStopWordDictionary.remove("居民");
            print(NotionalTokenizer.segment(text));

            //可以对任意分词器的结果执行过滤
            var term_list = BasicTokenizer.segment(text);
            print(term_list);
            CoreStopWordDictionary.apply(term_list);
            print(term_list);

            // 还可以自定义过滤逻辑
            var MyFilter = new MyStopWordFilter();
            CoreStopWordDictionary.FILTER = MyFilter;
            print(NotionalTokenizer.segment("数字123的保留"));
            // “的”位于stopwords.txt所以被过滤，数字得到保留
        }
        /// <summary>
        /// 同义词改写
        /// 此点子可以使同义词词典将一律段文本改写成意思相似之另一样段落文本，而且多符合语法
        /// </summary>
        public void TextRewrite()
        {
            var text = "这个方法可以利用同义词词典将一段文本改写成意思相似的另一段文本，而且差不多符合语法";
            print(CoreSynonymDictionary.rewrite(text));
        }

        #region  demo
        /// <summary>
        /// 词性标注
        /// </summary>
        public void demo_pos_tagging()
        {
            //未标注： [教授/nnt, 正在/d, 教授/nnt, 自然语言处理/nz, 课程/n]
            // 标注后： [教授/nnt, 正在/d, 教授/v, 自然语言处理/nz, 课程/n]
            var text = "教授正在教授自然语言处理课程";
            var segment = HanLP.newSegment();
            print("未标注：", segment.seg(text));
            segment.enablePartOfSpeechTagging(true);
            print("标注后：", segment.seg(text));

        }

        /// <summary>
        /// 演示数词和数量词识别
        /// </summary>
        public void demo_number_and_quantifier_recognition()
        {
            var sentences = new string[] {
                "十九元套餐包括什么",
                "九千九百九十九朵玫瑰",
                "壹佰块都不给我",
                "９０１２３４５６７８只蚂蚁",
                "牛奶三〇〇克*2",
                "ChinaJoy“扫黄”细则露胸超2厘米罚款" };
            StandardTokenizer.SEGMENT.enableNumberQuantifierRecognize(true);
            foreach (var sentence in sentences)
            {
                print(StandardTokenizer.segment(sentence));
            }
        }
        /// <summary>
        /// 演示词共现统计
        /// </summary>
        public void demo_occurrence()
        {
            var occurrence = new Occurrence();
            occurrence.addAll("在计算机音视频和图形图像技术等二维信息算法处理方面目前比较先进的视频处理算法");
            occurrence.compute();
            var unigram = occurrence.getUniGram().ToList<TrieEntry>();

            foreach (var entry in unigram)
            {
                print($"{entry.getKey()}{entry.getValue()}");
            }


            var bigram = occurrence.getBiGram().ToList<TrieEntry>();
            foreach (var entry in bigram)
            {
                print($"{entry.getKey()}{entry.getValue()}");
            }
            var trigram = occurrence.getTriGram().ToList<TrieEntry>();
            foreach (var entry in trigram)
            {
                print($"{entry.getKey()}{entry.getValue()}");

            }

        }

        /// <summary>
        /// 基于AhoCorasickDoubleArrayTrie的分词器，该分词器允许用户跳过核心词典，直接使用自己的词典。
        /// 需要注意的是，自己的词典必须遵守HanLP词典格式。
        /// </summary>
        public void demo_use_AhoCorasickDoubleArrayTrieSegment()
        {

            // AhoCorasickDoubleArrayTrieSegment要求用户必须提供自己的词典路径
            var segment = new AhoCorasickDoubleArrayTrieSegment().loadDictionary(com.hankcs.hanlp.HanLP.Config.CustomDictionaryPath[0]);
            print(segment.seg("微观经济学继续教育循环经济"));

        }
        /// <summary>
        /// 语义距离
        /// </summary>
        public void demo_word_distance()
        {
            var word_array = new string[] {"香蕉",
           "苹果",
           "白菜",
           "水果",
           "蔬菜",
           "自行车",
           "公交车",
           "飞机",
           "买",
           "卖",
           "购入",
           "新年",
           "春节",
           "丢失",
           "补办",
           "办理",
           "送给",
           "寻找",
           "孩子",
           "教室",
           "教师",
           "会计",};

            print($"词A\t词B\t语义距离\t语义相似度\n");
            foreach (var a in word_array)
            {
                foreach (var b in word_array)
                {
                    print($"{a}\t{ b}\t{CoreSynonymDictionary.distance(a, b)}\t{CoreSynonymDictionary.similarity(a, b)}");

                }
            }
        }

        #endregion

        private void print(object obj)
        {
            Console.WriteLine(obj);
        }
        private void print(string str, object obj)
        {
            Console.WriteLine($"{str}{obj}");
        }
        /// <summary>
        /// 自定义停用词
        /// </summary>
        public class MyStopWordFilter : Filter
        {
            bool Filter.shouldInclude(Term term)
            {
                if (term.nature.startsWith('m')) return true; // 数词保留
                return !CoreStopWordDictionary.contains(term.word); // 停用词过滤
            }
        }
        #endregion

    }
}
