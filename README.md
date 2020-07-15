# HanlpNet
HanLp的dotNet调用,利用IKVM 调用java 开发的hanlp.jar包
强大的NLP轮子,目前在.net上暂无实现， 为方便.net使用HanLp使用收集生成1.7.7版本的dll工具类与使用demo合集（正在补充所有的demo）方便调用

原作者地址:(https://github.com/hankcs/HanLP)

中文分词 词性标注 命名实体识别 依存句法分析 语义依存分析 新词发现 关键词短语提取 自动摘要 文本分类聚类 拼音简繁转换 自然语言处理 https://bbs.hankcs.com/
# HanLP: Han Language Processing

 [English](https://github.com/hankcs/HanLP/tree/master) | [1.x版](https://github.com/hankcs/HanLP/tree/1.x) | [论坛](https://bbs.hankcs.com/) | [docker](https://github.com/WalterInSH/hanlp-jupyter-docker)

面向生产环境的多语种自然语言处理工具包，基于 TensorFlow 2.0，目标是普及落地最前沿的NLP技术。HanLP具备功能完善、性能高效、架构清晰、语料时新、可自定义的特点。内部算法经过工业界和学术界考验，配套书籍[《自然语言处理入门》](http://nlp.hankcs.com/book.php)已经出版。目前，基于深度学习的HanLP 2.0正处于alpha测试阶段，未来将实现知识图谱、问答系统、自动摘要、文本语义相似度、指代消解、三元组抽取、实体链接等功能。欢迎加入[蝴蝶效应](https://bbs.hankcs.com/)参与讨论，或者反馈bug和功能请求到[issue区](https://github.com/hankcs/HanLP/issues)。Java用户请使用[1.x分支](https://github.com/hankcs/HanLP/tree/1.x) ，经典稳定，永久维护。RESTful API正在开发中，2.0正式版将支持包括Java、Python在内的开发语言。

 ## .net 初次食用注意事项：

首次运行缺少dll 请在 HanLP\HanLPDotNet\Package\java\hanlp   加载hanlp-1.7.7.dll（由hanlp-1.7.7转化获得）
nuget获取的IKVM.OpenJDK 如果出现版本不匹配 尝试加载IKVM.OpenJDK.Core.dll，IKVM.OpenJDK.Text.dll，IKVM.OpenJDK.Util.dll，IKVM.Runtime.dll
            
1.1首次运行需要下载语料库 (https://file.hankcs.com/hanlp/data-for-1.7.5.zip ) 
1.2酒店评论情绪分析训练库 (http://file.hankcs.com/corpus/ChnSentiCorp.zip)
2.配置hanlp语料库文件夹，并配置hanlp.properties文件 root=XXXXX/Package/java/hanlp
3.如出现缺少dll将ikvmbin-8.1.5717.0.rar 中相关的openjdk dll复制到bin中

``` 
         
            var nlpDemo = new HanLPHelper(@"XXXX\HanLPDotNet\Package\java\hanlp");
            nlpDemo.Segement("吃葡萄不吐葡萄皮，你好啊");
            //nlpDemo.Segement_Standard();
            //nlpDemo.Segement_NLP();
            //nlpDemo.Segement_Index
            //nlpDemo.demo_use_AhoCorasickDoubleArrayTrieSegment();
            Console.Read();

```
标准分词(https://www.hankcs.com/nlp/segment/the-word-graph-is-generated.html)
``` 
  		var termList = StandardTokenizer.segment("商品和服务");
```
NLP分词
``` 
  		var termList = NLPTokenizer.segment("中国科学院计算技术研究所的宗成庆教授正在教授自然语言处理课程");
```
索引分词
``` 
    	var termList = IndexTokenizer.segment("中国科学院计算技术研究所的宗成庆教授正在教授自然语言处理课程");
```
 N-最短路径分词(https://www.hankcs.com/nlp/segment/n-shortest-path-to-the-java-implementation-and-application-segmentation.html)
``` 
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
```
CRF 分词(https://www.hankcs.com/nlp/segment/crf-segmentation-of-the-pure-java-implementation.html)
``` 
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
```
极速 分词(https://www.hankcs.com/program/algorithm/aho-corasick-double-array-trie.html)
``` 
 			 String text = "江西鄱阳湖干枯，中国最大淡水湖变成大草原";
            Console.WriteLine(SpeedTokenizer.segment(text));

            int pressure = 1000000;
            for (int i = 0; i < pressure; ++i)
            {
                SpeedTokenizer.segment(text);
            }
```
  CustomDictionary是一份全局的用户自定义词典，可以随时增删，影响全部分词器。
  CustomDictionary主词典文本路径是data/dictionary/custom/CustomDictionary.txt，
  用户可以在此增加自己的词语（不推荐）；
  也可以单独新建一个文本文件，
  通过配置文件CustomDictionaryPath=data/dictionary/custom/CustomDictionary.txt; 
  我的词典.txt;来追加词典（推荐）。
  始终建议将相同词性的词语放到同一个词典文件里，便于维护和分享。
  词典格式
  每一行代表一个单词，格式遵从[单词][词性A][A的频次][词性B][B的频次] ... 如果不填词性则表示采用词典的默认词性。
  词典的默认词性默认是名词n，可以通过配置文件修改：全国地名大全.txt ns; 如果词典路径后面空格紧接着词性，则该词典默认是该词性。
  关于用户词典的更多信息请参考词典说明一章。
```
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
```
中国人名识别(https://www.hankcs.com/nlp/chinese-name-recognition-in-actual-hmm-viterbi-role-labeling.html)
```
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
```
组织机构识别
```
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
```
关键字提取（https://www.hankcs.com/nlp/textrank-algorithm-to-extract-the-keywords-java-implementation.html）
```
 			content = "程序员(英文Programmer)是从事程序开发、维护的专业人员。一般将程序员分为程序设计人员和程序编码人员，但两者的界限并不非常清楚，特别是在中国。软件从业人员分为初级程序员、高级程序员、系统分析员和项目经理四大类。";
            var keywordList = HanLP.extractKeyword(content, 5);
```
 短语识别（ https://www.hankcs.com/nlp/extraction-and-identification-of-mutual-information-about-the-phrase-based-on-information-entropy.html）
 ```
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
 ```
拼音转换(https://www.hankcs.com/nlp/java-chinese-characters-to-pinyin-and-simplified-conversion-realization.html#h2-17)

```
            String text = "重载不是重任";
            List<Pinyin> pinyinList = HanLP.convertToPinyinList(text).ToList<Pinyin>();
```
简繁转换(https://www.hankcs.com/nlp/java-chinese-characters-to-pinyin-and-simplified-conversion-realization.html#h2-17)
```
            Console.WriteLine(HanLP.convertToTraditionalChinese("用笔记本电脑写程序"));
            Console.WriteLine(HanLP.convertToSimplifiedChinese("「以後等妳當上皇后，就能買士多啤梨慶祝了」"));
```
 提取关键字与摘要
 ```
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
 ```
 文本推荐(句子级别，从一系列句子中挑出与输入句子最相似的那一个)
 ```
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
 ```
 语义距离
 ```
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
 ```
依存句法分析
https://www.hankcs.com/nlp/parsing/neural-network-based-dependency-parser.html

```
			CoNLLSentence sentence = HanLP.parseDependency(str);
            Console.WriteLine(sentence);
            // 可以方便地遍历它
            foreach (CoNLLWord word in sentence)
            {
                Console.WriteLine($"{word.LEMMA} --({word.DEPREL})--> {word.HEAD.LEMMA}\n");
            }
```
文本聚类
```
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
```
感知机词法分析器为例
 [上海/ns 华安/nz 工业/n （/w 集团/n ）/w 公司/n]/nt 董事长/n 谭旭光/nr 和/c 秘书/n 胡花蕊/nr 来到/v [美国/ns 纽约/ns 现代/t 艺术/n 博物馆/n]/ns 参观/v
```
 			var analyzer = new PerceptronLexicalAnalyzer();
            var sence = analyzer.analyze("上海华安工业（集团）公司董事长谭旭光和秘书胡花蕊来到美国纽约现代艺术博物馆参观");
```
 情感分析( http://file.hankcs.com/corpus/ChnSentiCorp.zip 下载后解压)
 ```
   			var chn_senti_corp = @"XXXX\Package\java\hanlp\data\trainData\ChnSentiCorp\ChnSentiCorp情感分析酒店评论";

            var classifier = new com.hankcs.hanlp.classification.classifiers.NaiveBayesClassifier();
            // 创建分类器，更高级的功能请参考IClassifier的接口定义
            classifier.train(chn_senti_corp);
            //  训练后的模型支持持久化，下次就不必训练了
            var result = classifier.predict("前台客房服务态度非常好！早餐很丰富，房价很干净。再接再厉！");
            result = classifier.predict("结果大失所望，灯光昏暗，空间极其狭小，床垫质量恶劣，房间还伴着一股霉味。");
            result = classifier.predict("可利用文本分类实现情感分析，效果不是不行");
 ```

停用词的使用教程
```
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
```
同义词改写(此点子可以使同义词词典将一律段文本改写成意思相似之另一样段落文本，而且多符合语法)
```
			var text = "这个方法可以利用同义词词典将一段文本改写成意思相似的另一段文本，而且差不多符合语法";
            print(CoreSynonymDictionary.rewrite(text));
```
词性标注
```
 			//未标注： [教授/nnt, 正在/d, 教授/nnt, 自然语言处理/nz, 课程/n]
            // 标注后： [教授/nnt, 正在/d, 教授/v, 自然语言处理/nz, 课程/n]
            var text = "教授正在教授自然语言处理课程";
            var segment = HanLP.newSegment();
            print("未标注：", segment.seg(text));
            segment.enablePartOfSpeechTagging(true);
            print("标注后：", segment.seg(text));
```
演示数词和数量词识别
```
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
```
演示词共现统计
```
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
```
基于AhoCorasickDoubleArrayTrie的分词器，该分词器允许用户跳过核心词典，直接使用自己的词典。
```
 			// AhoCorasickDoubleArrayTrieSegment要求用户必须提供自己的词典路径
            var segment = new AhoCorasickDoubleArrayTrieSegment().loadDictionary(com.hankcs.hanlp.HanLP.Config.CustomDictionaryPath[0]);
            print(segment.seg("微观经济学继续教育循环经济"));
```
自定义停用词
```
	   		bool Filter.shouldInclude(Term term)
            {
                if (term.nature.startsWith('m')) return true; // 数词保留
                return !CoreStopWordDictionary.contains(term.word); // 停用词过滤
            }
```
hanlp功能介绍：https://www.hankcs.com/nlp/hanlp.html
 
