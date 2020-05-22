# HanlpNet
HanLp的dotNet实现,利用IKVM 调用java 开发的hanlp
强大的NLP轮子,目前在.net上暂无实现， 为方便.net使用HanLp使用收集生成1.7.7版本的dll工具类与使用demo合集（正在补充所有的demo）方便调用

原作者地址:(https://github.com/hankcs/HanLP)

中文分词 词性标注 命名实体识别 依存句法分析 语义依存分析 新词发现 关键词短语提取 自动摘要 文本分类聚类 拼音简繁转换 自然语言处理 https://bbs.hankcs.com/
# HanLP: Han Language Processing

 [English](https://github.com/hankcs/HanLP/tree/master) | [1.x版](https://github.com/hankcs/HanLP/tree/1.x) | [论坛](https://bbs.hankcs.com/) | [docker](https://github.com/WalterInSH/hanlp-jupyter-docker)

面向生产环境的多语种自然语言处理工具包，基于 TensorFlow 2.0，目标是普及落地最前沿的NLP技术。HanLP具备功能完善、性能高效、架构清晰、语料时新、可自定义的特点。内部算法经过工业界和学术界考验，配套书籍[《自然语言处理入门》](http://nlp.hankcs.com/book.php)已经出版。目前，基于深度学习的HanLP 2.0正处于alpha测试阶段，未来将实现知识图谱、问答系统、自动摘要、文本语义相似度、指代消解、三元组抽取、实体链接等功能。欢迎加入[蝴蝶效应](https://bbs.hankcs.com/)参与讨论，或者反馈bug和功能请求到[issue区](https://github.com/hankcs/HanLP/issues)。Java用户请使用[1.x分支](https://github.com/hankcs/HanLP/tree/1.x) ，经典稳定，永久维护。RESTful API正在开发中，2.0正式版将支持包括Java、Python在内的开发语言。

 ## .net 安装

#首次运行缺少dll 请在 HanLP\HanLPDotNet\Package\java\hanlp   加载hanlp-1.7.7.dll（由hanlp-1.7.7转化获得）
#nuget获取的IKVM.OpenJDK 如果出现版本不匹配 尝试加载IKVM.OpenJDK.Core.dll，IKVM.OpenJDK.Text.dll，IKVM.OpenJDK.Util.dll，IKVM.Runtime.dll
            
#1.1首次运行需要下载语料库 https://file.hankcs.com/hanlp/data-for-1.7.5.zip  
#1.2酒店评论情绪分析训练库 http://file.hankcs.com/corpus/ChnSentiCorp.zip
#2.配置hanlp语料库文件夹，并配置hanlp.properties文件 root=XXXXX/Package/java/hanlp
#3.如出现缺少dll将ikvmbin-8.1.5717.0.rar 中相关的openjdk dll复制到bin中

``` 
         
            var nlpDemo = new HanLPHelper(@"XXXX\HanLPDotNet\Package\java\hanlp");
            nlpDemo.Segement("吃葡萄不吐葡萄皮，你好啊");
            // nlpDemo.Classifier_NaiveBayes();
            //nlpDemo.demo_use_AhoCorasickDoubleArrayTrieSegment();
            Console.Read();
```
