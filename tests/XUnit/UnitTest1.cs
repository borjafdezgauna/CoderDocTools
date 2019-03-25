using System;
using Xunit;
using MarkdownToPDF;
using System.Collections.Generic;

namespace XUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            MardownToPDFConverter converter = new MardownToPDFConverter();
            List<string> splitParts;
            splitParts = converter.SplitByInlinePatterns("_Windows_: `Start->cmd (as Administrator) -> net start|stop herdagent`");
            Assert.Equal(3, splitParts.Count);
            Assert.Equal("_Windows_", splitParts[0]);
            Assert.Equal(": ", splitParts[1]);
            Assert.Equal("`Start->cmd (as Administrator) -> net start|stop herdagent`", splitParts[2]);
            splitParts = converter.SplitByInlinePatterns(" _Linux_: `sudo /etc/init.d/herd-agent-daemon start|stop`");
            Assert.Equal(4, splitParts.Count);
            splitParts = converter.SplitByInlinePatterns("[Tutorial #0](User-Tutorial-0.-Quick-walkthrough): Quick walk-trough  ");
            Assert.Equal(2, splitParts.Count);
            splitParts = converter.SplitByInlinePatterns("`sudo chmod 770 bin/HerdAgentInstaller-linux.sh`");
            Assert.Single(splitParts);
            splitParts = converter.SplitByInlinePatterns("Windows x86/x64 service (`bin/HerdAgentInstaller.msi`)");
            Assert.Equal(3, splitParts.Count);
            splitParts = converter.SplitByInlinePatterns("1. Download the binaries[here](../ releases / latest).It includes both Windows and Linux binaries.");
            Assert.Equal(3, splitParts.Count);
            splitParts = converter.SplitByInlinePatterns("SimionZoo provides two main applications for the end-user: *Badger* and the *Herd Agent* service/daemon."
                + " Experiments are designed in _Badger_, which sends them to be run by the slave machines running the _Herd Agent_ service. This means you have decide"
                + " which machines will be used as slaves to actually run the experiments and which one will be used as master to design, send, monitor and analyze the results. "
                + "The same machine can act as master and slave at the same time.");
            Assert.Equal(9, splitParts.Count);
            splitParts = converter.SplitByInlinePatterns("_Push-box 1_: one robot push a box toward the goal position");
            Assert.Equal(2, splitParts.Count);
            splitParts = converter.SplitByInlinePatterns("![Mountain-car visualization](https://i.imgur.com/DHEjnJO.png)");
            Assert.Single(splitParts);
            splitParts = converter.SplitByInlinePatterns("The name of the variable is _My_Variable_.");
            Assert.Equal(3, splitParts.Count);
            Assert.Equal("_My_Variable_", splitParts[1]);
            Assert.Equal(".", splitParts[2]);
            splitParts = converter.SplitByInlinePatterns("The variable is very important (_My_Variable_) or not?");
            Assert.Equal(3, splitParts.Count);
            Assert.Equal("_My_Variable_", splitParts[1]);
            Assert.Equal(") or not?", splitParts[2]);
            splitParts = converter.SplitByInlinePatterns("If you use our software in your research, we kindly ask you to reference [![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.2573299.svg)](https://doi.org/10.5281/zenodo.2573299).");
            Assert.Equal(3, splitParts.Count);
            Assert.Equal("If you use our software in your research, we kindly ask you to reference ", splitParts[0]);
            Assert.Equal("[![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.2573299.svg)](https://doi.org/10.5281/zenodo.2573299)", splitParts[1]);
            Assert.Equal(".", splitParts[2]);
            splitParts = converter.SplitByInlinePatterns("pLogger= CHILD_OBJECT<Logger>(pConfigNode, \"Log\", \"The logger class\");");
            Assert.Single(splitParts);
            splitParts = converter.SplitByInlinePatterns(" Every control step, after executing the action selected by the agent _a_, the agent will learn from the last experience tuple and also from _10_ randomly selected tuples from the buffer.");
            Assert.Equal(5, splitParts.Count);
            Assert.Equal("_a_", splitParts[1]);
            Assert.Equal("_10_", splitParts[3]);
            splitParts = converter.SplitByInlinePatterns("The class can be CHILD_OBJECT or CHILD_OBJECT_FACTORY.");
            Assert.Single(splitParts);
            splitParts = converter.SplitByInlinePatterns("I will please your request (_Note: I know what this is_) but beware");
            Assert.Equal(3, splitParts.Count);
            Assert.Equal("_Note: I know what this is_", splitParts[1]);
            splitParts = converter.SplitByInlinePatterns("State variables in _s_ can be randomly initialized or reset to some initial state of the system.");
            Assert.Equal(3, splitParts.Count);
            Assert.Equal("_s_", splitParts[1]);
            splitParts = converter.SplitByInlinePatterns("the experiment off-line (_Right-click->View experiment_) or the value functions learned by the agents(_Right - click->View functions_).");
            Assert.Equal(5, splitParts.Count);
            Assert.Equal("_Right-click->View experiment_", splitParts[1]);
            Assert.Equal("_Right - click->View functions_", splitParts[3]);
        }
    }
}
