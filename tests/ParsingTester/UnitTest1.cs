using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GitHubWikiToPDF;
using System.Collections.Generic;

namespace ParsingTester
{
    [TestClass]
    public class InlineParsingTester
    {
        [TestMethod]
        public void InlineSplitter()
        {
            WikiToPDFConverter converter = new WikiToPDFConverter();
            List<string> splitParts;
            splitParts = converter.SplitByInlinePatterns("_Windows_: `Start->cmd (as Administrator) -> net start|stop herdagent`");
            Assert.AreEqual(3, splitParts.Count);
            Assert.AreEqual("_Windows_", splitParts[0]);
            Assert.AreEqual(": ", splitParts[1]);
            Assert.AreEqual("`Start->cmd (as Administrator) -> net start|stop herdagent`", splitParts[2]);
            splitParts = converter.SplitByInlinePatterns(" _Linux_: `sudo /etc/init.d/herd-agent-daemon start|stop`");
            Assert.AreEqual(splitParts.Count, 4);
            splitParts = converter.SplitByInlinePatterns("[Tutorial #0](User-Tutorial-0.-Quick-walkthrough): Quick walk-trough  ");
            Assert.AreEqual(splitParts.Count, 2);
            splitParts = converter.SplitByInlinePatterns("`sudo chmod 770 bin/HerdAgentInstaller-linux.sh`");
            Assert.AreEqual(splitParts.Count, 1);
            splitParts = converter.SplitByInlinePatterns("Windows x86/x64 service (`bin/HerdAgentInstaller.msi`)");
            Assert.AreEqual(splitParts.Count, 3);
            splitParts = converter.SplitByInlinePatterns("1. Download the binaries[here](../ releases / latest).It includes both Windows and Linux binaries.");
            Assert.AreEqual(splitParts.Count, 3);
            splitParts = converter.SplitByInlinePatterns("SimionZoo provides two main applications for the end-user: *Badger* and the *Herd Agent* service/daemon."
                + " Experiments are designed in _Badger_, which sends them to be run by the slave machines running the _Herd Agent_ service. This means you have decide"
                + " which machines will be used as slaves to actually run the experiments and which one will be used as master to design, send, monitor and analyze the results. "
                + "The same machine can act as master and slave at the same time.");
            Assert.AreEqual(9, splitParts.Count);
            splitParts = converter.SplitByInlinePatterns("_Push-box 1_: one robot push a box toward the goal position");
            Assert.AreEqual(2, splitParts.Count);
            splitParts = converter.SplitByInlinePatterns("![Mountain-car visualization](https://i.imgur.com/DHEjnJO.png)");
            Assert.AreEqual(1, splitParts.Count);
            splitParts = converter.SplitByInlinePatterns("The name of the variable is _My_Variable_.");
            Assert.AreEqual(3, splitParts.Count);
            Assert.AreEqual("_My_Variable_", splitParts[1]);
            Assert.AreEqual(".", splitParts[2]);
            splitParts = converter.SplitByInlinePatterns("The variable is very important (_My_Variable_) or not?");
            Assert.AreEqual(3, splitParts.Count);
            Assert.AreEqual("_My_Variable_", splitParts[1]);
            Assert.AreEqual(") or not?", splitParts[2]);
            splitParts = converter.SplitByInlinePatterns("If you use our software in your research, we kindly ask you to reference [![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.2573299.svg)](https://doi.org/10.5281/zenodo.2573299).");
            Assert.AreEqual(3, splitParts.Count);
            Assert.AreEqual("If you use our software in your research, we kindly ask you to reference ", splitParts[0]);
            Assert.AreEqual("[![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.2573299.svg)](https://doi.org/10.5281/zenodo.2573299)", splitParts[1]);
            Assert.AreEqual(".", splitParts[2]);
            splitParts = converter.SplitByInlinePatterns("pLogger= CHILD_OBJECT<Logger>(pConfigNode, \"Log\", \"The logger class\");");
            Assert.AreEqual(1, splitParts.Count);
            splitParts = converter.SplitByInlinePatterns(" Every control step, after executing the action selected by the agent _a_, the agent will learn from the last experience tuple and also from _10_ randomly selected tuples from the buffer.");
            Assert.AreEqual(5, splitParts.Count);
            Assert.AreEqual("_a_", splitParts[1]);
            Assert.AreEqual("_10_", splitParts[3]);
            splitParts = converter.SplitByInlinePatterns("The class can be CHILD_OBJECT or CHILD_OBJECT_FACTORY.");
            Assert.AreEqual(1, splitParts.Count);
            splitParts = converter.SplitByInlinePatterns("I will please your request (_Note: I know what this is_) but beware");
            Assert.AreEqual(3, splitParts.Count);
            Assert.AreEqual("_Note: I know what this is_", splitParts[1]);
            splitParts = converter.SplitByInlinePatterns("State variables in _s_ can be randomly initialized or reset to some initial state of the system.");
            Assert.AreEqual(3, splitParts.Count);
            Assert.AreEqual("_s_", splitParts[1]);
        }
        [TestMethod]
        public void LinkParsing()
        {
            WikiToPDFConverter converter = new WikiToPDFConverter();
            
            converter.ParseInlineElements("1. Download the binaries[here](releases/latest).It includes both Windows and Linux binaries",0);
            Assert.AreEqual(1, converter.LinkedPages.Count);
            Assert.AreEqual("releases/latest.md", converter.LinkedPages[0]);
            converter.ParseInlineElements("1. Download the binaries[here](http://github.com).It includes both Windows and Linux binaries", 0);
            Assert.AreEqual(1, converter.LinkedPages.Count);
            converter.ParseInlineElements("1. Download the binaries[[here]].It includes both Windows and Linux binaries", 0);
            Assert.AreEqual(2, converter.LinkedPages.Count);
            Assert.AreEqual("here.md", converter.LinkedPages[1]);
        }
    }
}
