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
            Assert.AreEqual(splitParts.Count, 3);
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
            Assert.AreEqual(splitParts.Count, 9);
            splitParts = converter.SplitByInlinePatterns("_Push-box 1_: one robot push a box toward the goal position");
            Assert.AreEqual(splitParts.Count, 2);
            splitParts = converter.SplitByInlinePatterns("![Mountain-car visualization](https://i.imgur.com/DHEjnJO.png)");
            Assert.AreEqual(splitParts.Count, 1);
            splitParts = converter.SplitByInlinePatterns("The name of the variable is _My_Variable_.");
            Assert.AreEqual(splitParts.Count, 3);
            Assert.AreEqual(splitParts[1], "_My_Variable_");
            Assert.AreEqual(splitParts[2], ".");
        }
    }
}
