using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PS4DevelopmentTests
{
    [TestClass]
    public class DevelopmentTests
    {
        /// <summary>
        /// Tests to see that the copy method is actually copying size correctly.
        /// </summary>
        [TestMethod]
        public void Copy1()
        {
            var d1 = new DependencyGraph();
            var d2 = new DependencyGraph(d1);
            Assert.AreEqual(0, d1.Size);
            Assert.AreEqual(0, d2.Size);
        }

        /// <summary>
        /// Tests to see that we aren't simply copying references, but actually
        /// creating a deep copy, and that size is updating correctly.
        /// </summary>
        [TestMethod]
        public void Copy2()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.AddDependency("a", "b");
            d1.AddDependency("b", "c");
            DependencyGraph d2 = new DependencyGraph(d1);
            d2.RemoveDependency("b", "c");
            Assert.IsTrue(d1.HasDependents("b"));
            Assert.AreEqual(2, d1.Size);
            Assert.IsFalse(d2.HasDependents("b"));
            Assert.AreEqual(1, d2.Size);
            d1.AddDependency("z", "a");
            Assert.AreEqual(3, d1.Size);
            Assert.IsFalse(d2.HasDependents("z"));
        }

        /// <summary>
        /// Again, checks to see that we're creating a deep copy and that the
        /// number of dependents/dependees is changing correctly.
        /// </summary>
        [TestMethod]
        public void Copy5()
        {
            var d1 = new DependencyGraph();
            d1.AddDependency("a", "b");
            d1.AddDependency("d", "e");
            var d2 = new DependencyGraph(d1);
            d1.AddDependency("a", "c");
            d2.AddDependency("d", "f");
            Assert.AreEqual(2, new List<string>(d1.GetDependents("a")).Count);
            Assert.AreEqual(1, new List<string>(d1.GetDependents("d")).Count);
            Assert.AreEqual(2, new List<string>(d2.GetDependents("d")).Count);
            Assert.AreEqual(1, new List<string>(d2.GetDependents("a")).Count);
        }

        /// <summary>
        /// Asserts that the add dependency method throws if a null argument is
        /// passed in.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null1()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", null);
        }

        /// <summary>
        /// Asserts that null replacements throw an error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null10()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");

            var ls = new List<string>();
            ls.Add(null);
            d.ReplaceDependees("a", ls);
        }

        /// <summary>
        /// Asserts that you can't pass null parameters in to the
        /// ReplaceDependents method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null11()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.ReplaceDependents(null, new HashSet<string>());
        }

        /// <summary>
        /// Asserts that you can't pass null parameters in to the
        /// ReplaceDependents method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null12()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.ReplaceDependents("t", null);
        }

        /// <summary>
        /// Asserts that you can't replace dependents with null objects.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null13()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");

            var ls = new List<string>();
            ls.Add(null);
            d.ReplaceDependents("a", ls);
        }

        /// <summary>
        /// Asserts that you can't pass a null argument to the GetDependees
        /// method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.GetDependees(null);
        }

        /// <summary>
        /// Asserts that you can't pass a null argument to the GetDependents
        /// method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null3()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.GetDependents(null);
        }

        /// <summary>
        /// Asserts that you can't pass a null argument to the HasDependees
        /// method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null4()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.HasDependees(null);
        }

        /// <summary>
        /// Asserts that you can't pass a null argument to the HasDependents
        /// method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null5()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.HasDependents(null);
        }

        /// <summary>
        /// Asserts that you can't pass a null argument to the RemoveDependency
        /// method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null6()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.RemoveDependency(null, "t");
        }

        /// <summary>
        /// Asserts that you can't pass a null argument to the RemoveDependency
        /// method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null7()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.RemoveDependency("t", null);
        }

        /// <summary>
        /// Asserts that you can't pass a null argument to the ReplaceDependees
        /// method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null8()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.ReplaceDependees(null, new HashSet<string>());
        }

        /// <summary>
        /// Asserts that you can't pass a null argument to the ReplaceDependees
        /// method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null9()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.ReplaceDependees("t", null);
        }
    }
}