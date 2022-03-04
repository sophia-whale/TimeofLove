using System.Linq;
using NUnit.Framework;
using TabbedTemplate.Converters;
using TabbedTemplate.Renders;

namespace TabbedTemplate.UnitTest.Renders {
    public class ShareCanvasViewRenderTest {
        [Test]
        public void TestIgnoreVoidElementsInHTML() {
            var shareCanvasViewRender = new ShareCanvasViewRender();
            string html = "<p>今天<strong>​很好<em>​</em></strong></p><p><strong><em>明天<span style=\"text-decoration: underline;\">​也很好</span></em></strong></p><p><strong><span style=\"text-decoration: underline;\">后天</span></strong>​​<span style=\"color: rgb(0, 176, 159); text-decoration: inherit;\">​也不错</span></p><p><span style=\"color: rgb(0, 176, 159); text-decoration: inherit;\">昨天<span style=\"font-size: 18pt;\">​还可以</span></span></p><p><span style=\"color: rgb(0, 176, 159); text-decoration: inherit;\"><span style=\"background-color: rgb(255, 255, 254);\">​那么</span>​<span style=\"background-color: rgb(255, 217, 0);\">​未来会</span></span></p><ol><li><span style=\"color: rgb(0, 0, 0); text-decoration: inherit;\">﻿第一天</span></li><li><span style=\"color: rgb(0, 0, 0); text-decoration: inherit;\">第二天</span></li><li><span style=\"color: rgb(0, 0, 0); text-decoration: inherit;\">第三天﻿</span><span style=\"color: rgb(0, 0, 0); text-decoration: inherit;\"><span style=\"background-color: rgb(255, 255, 254);\"></span></span></li></ol>";
            var resultString =
                shareCanvasViewRender.IgnoreVoidElementsInHTML(html);
            string expectedResult = "今天​很好​明天​也很好后天​​​也不错昨天​还可以​那么​​未来会﻿第一天第二天第三天﻿";
            Assert.AreEqual(expectedResult, resultString);
        }

        [Test]
        public void TestSplitLines() {
            string str =
                "卷神怔弓萧课共浊押紫圃使菱胰且汤丝纽躯问巾酒币伦棺赫勿纂山届妨泼峰况沃于壤排玻巡沾汹纠忍白关抄卿鲜岛烃锌橡们高功愿鲁仅拜巧珊弓连抓据秘脚谨张蔑扔荣跟邵腹顺镗萌隘险就俯是诱牲腹罪氮沃氩柳须厢涂凑扑摸丽盐算蕴乔去微脆一颁芽凝寻鲍英源密爵妈儡浊滥胜埋鲜命歪率贼液竹隶稗深胡吱丙。";
            var list=ShareCanvasViewRender.SplitLines(str, 20, 2);
            var length = list.Length;
            Assert.AreEqual(14,length);
            Assert.AreEqual(20.0f,list.First().Width);
            Assert.AreEqual("稗深胡吱丙。",list.Last().Value);
        }
    }
}