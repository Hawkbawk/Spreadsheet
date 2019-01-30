using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphs
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        [TestMethod()]
        public void EmptyTest1()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }

        [TestMethod()]
        public void EmptyTest2()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependees("x"));
            Assert.IsFalse(t.HasDependents("x"));
            t.AddDependency("x", "y");
            Assert.IsTrue(t.HasDependents("x"));
            Assert.IsTrue(t.HasDependees("y"));
        }

        [TestMethod()]
        public void EmptyTest7()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }

        [TestMethod()]
        public void EmptyTest8()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.IsTrue(t.HasDependees("y"));
            Assert.IsTrue(t.HasDependents("x"));
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.HasDependees("y"));
            Assert.IsFalse(t.HasDependents("x"));
        }

        [TestMethod()]
        public void EmptyTest11()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }

        [TestMethod()]
        public void NonEmptyTest1()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }

        [TestMethod()]
        public void NonEmptyTest4()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        [TestMethod()]
        public void NonEmptyTest6()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "b");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.AddDependency("c", "b");
            Assert.AreEqual(4, t.Size);
        }

        [TestMethod()]
        public void NonEmptyTest8()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "b");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.AddDependency("c", "b");
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsFalse(t.HasDependees("a"));
            Assert.IsTrue(t.HasDependents("b"));
            Assert.IsTrue(t.HasDependees("b"));
        }

        [TestMethod()]
        public void NonEmptyTest9()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "b");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.AddDependency("c", "b");

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        [TestMethod()]
        public void NonEmptyTest10()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "b");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.AddDependency("c", "b");

            IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "b") && (s2 == "c")) || ((s1 == "c") && (s2 == "b")));

            e = t.GetDependents("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("d", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependents("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependents("d").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
        }

        [TestMethod()]
        public void NonEmptyTest18()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsFalse(t.HasDependees("a"));
            Assert.IsTrue(t.HasDependents("b"));
            Assert.IsTrue(t.HasDependees("b"));
        }
        /// <summary>
        /// Ensures that the ReplaceDependees method performs as specified.
        /// Specifically checks to see if size is updating correctly and that
        /// the dependencies are being correctly removed.
        /// </summary>
        [TestMethod]
        public void TestReplaceDependeesSizeChange()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("t", "y");
            t.AddDependency("r", "y");
            t.AddDependency("e", "y");
            t.AddDependency("w", "y");
            t.AddDependency("q", "y");
            t.AddDependency("a", "y");
            t.AddDependency("z", "y");
            t.ReplaceDependees("y", new HashSet<string>());
            Assert.AreEqual(0, t.Size);
            Assert.IsFalse(t.HasDependents("x"));
            Assert.IsFalse(t.HasDependents("t"));
            Assert.IsFalse(t.HasDependents("r"));
            Assert.IsFalse(t.HasDependents("e"));
            Assert.IsFalse(t.HasDependents("w"));
            Assert.IsFalse(t.HasDependents("q"));
            Assert.IsFalse(t.HasDependents("a"));
            Assert.IsFalse(t.HasDependents("z"));
            Assert.IsFalse(t.HasDependees("y"));
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestReplaceDependentsSizeChange()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("x", "t");
            t.AddDependency("x", "e");
            t.AddDependency("x", "r");
            t.AddDependency("x", "w");
            t.AddDependency("x", "q");
            t.AddDependency("x", "a");
            t.AddDependency("x", "s");
            t.AddDependency("x", "d");
            t.AddDependency("x", "f");
            t.AddDependency("x", "g");

            t.ReplaceDependents("x", new HashSet<string>());
            Assert.AreEqual(0, t.Size);
            Assert.IsFalse(t.HasDependents("x"));
            Assert.IsFalse(t.HasDependees("y"));
            Assert.IsFalse(t.HasDependees("t"));
            Assert.IsFalse(t.HasDependees("e"));
            Assert.IsFalse(t.HasDependees("r"));
            Assert.IsFalse(t.HasDependees("w"));
            Assert.IsFalse(t.HasDependees("q"));
            Assert.IsFalse(t.HasDependees("a"));
            Assert.IsFalse(t.HasDependees("s"));
            Assert.IsFalse(t.HasDependees("d"));
            Assert.IsFalse(t.HasDependees("f"));
            Assert.IsFalse(t.HasDependees("g"));

        }
        /// <summary>
        /// Stress tests the AddDependency method by adding 1,000,000 
        /// dependencies, all with the same dependent.
        /// </summary>
        [TestMethod]
        public void StressTestAddDependency()
        {
            DependencyGraph t = new DependencyGraph();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1_000_000; i++)
            {

                t.AddDependency("s", RandomStringGenerator(15, 7));
            }
            if (sw.ElapsedMilliseconds >= 10_000)
            {
                Assert.Fail();
            }
        }
        /// <summary>
        /// Stress tests the ReplaceDependents method by adding 1,000,000
        /// dependencies to the DependencyGraph and then replacing all of them
        /// with a million different dependencies.
        /// </summary>
        [TestMethod]
        public void StressTestReplaceDependents()
        {
            DependencyGraph t = new DependencyGraph();

            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < 1_000_000; i++)
            {

                t.AddDependency("s", RandomStringGenerator(15, 7));
            }

            List<string> ls = new List<string>(1_000_000);
            for (int i = 0; i < 1_000_000; i++)
            {
                ls.Add(RandomStringGenerator(15, 7));
            }

            sw.Start();
            t.ReplaceDependents("s", ls);

            if (sw.ElapsedMilliseconds >= 10_000)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Stress tests the ReplaceDependees method by adding 1,000,000
        /// dependencies to the DependencyGraph and then replacing all of them
        /// with a million different dependencies.
        /// </summary>
        [TestMethod]
        public void StressTestReplaceDependees()
        {
            DependencyGraph t = new DependencyGraph();

            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < 1_000_000; i++)
            {

                t.AddDependency(RandomStringGenerator(15, 7), "s");
            }

            List<string> ls = new List<string>(1_000_000);
            for (int i = 0; i < 1_000_000; i++)
            {
                ls.Add(RandomStringGenerator(15, 7));
            }

            sw.Start();
            t.ReplaceDependees("s", ls);

            if (sw.ElapsedMilliseconds >= 10_000)
            {
                Assert.Fail();
            }
        }
        /// <summary>
        /// Generates random strings for stress testing the DependencyGraph 
        /// class
        /// </summary>
        /// <param name="length">
        /// The desired length of the random string.
        /// </param>
        /// <returns>
        /// A random string made of lowercase English letters.
        /// </returns>
        private static string RandomStringGenerator(int length, int seed)
        {
            StringBuilder str = new StringBuilder();
            Random r = new Random(seed);
            for (int i = 0; i < length; i++)
            {
                str.Append((char)r.Next(97, 123));
            }

            return str.ToString();
        }
    }
}
