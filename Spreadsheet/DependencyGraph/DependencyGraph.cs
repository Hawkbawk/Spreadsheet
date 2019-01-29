// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.

using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {

        private Dictionary<string, HashSet<string>> dependentsList;
        private Dictionary<string, HashSet<string>> dependeesList;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            Size = 0;
            dependentsList = new Dictionary<string, HashSet<string>>();
            dependeesList = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            dependentsList.TryGetValue(s, out HashSet<string> dependents);
            if (dependents != null && dependents.Count != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            dependeesList.TryGetValue(s, out HashSet<string> dependees);
            if (dependees != null && dependees.Count != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            dependentsList.TryGetValue(s, out HashSet<string> dependents);
            if (dependents != null)
            {
                return dependents;
            }
            return new HashSet<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            dependeesList.TryGetValue(s, out HashSet<string> dependees);
            if (dependees != null)
            {
                return dependees;
            }
            return new HashSet<string>();
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            HashSet<string> dependents = new HashSet<string>();
            if (dependentsList.TryGetValue(s, out dependents))
            {
            }
            else
            {
                dependents = new HashSet<string>();
                dependentsList.Add(s, dependents);
            }
            if (!dependents.Add(t))
            {
                return;
            }

            HashSet<string> dependees = new HashSet<string>();
            if (dependeesList.TryGetValue(t, out dependees))
            {
            }
            else
            {
                dependees = new HashSet<string>();
                dependeesList.Add(t, dependees);
            }

            if (!dependees.Add(s))
            {
                return;
            }
            this.Size++;
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            HashSet<string> dependents;
            if (!dependentsList.TryGetValue(s, out dependents) || !dependents.Contains(t))
            {
                return;
            }
            dependents.Remove(t);

            HashSet<string> dependees;
            if (!dependeesList.TryGetValue(t, out dependees) || !dependees.Contains(s))
            {
                return;
            }
            dependees.Remove(s);
            this.Size--;
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            HashSet<string> dependents;
            if (dependentsList.TryGetValue(s, out dependents) && dependents != null)
            {
                dependents.Clear();
            }
            else
            {
                dependentsList.Add(s, new HashSet<string>());
            }

            foreach (string dependent in newDependents)
            {
                AddDependency(s, dependent);
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            HashSet<string> dependees;
            if (dependeesList.TryGetValue(t, out dependees) && dependees != null)
            {
                dependees.Clear();
            }
            else
            {
                dependeesList.Add(t, new HashSet<string>());
            }

            foreach(string dependee in newDependees)
            {
                AddDependency(dependee, t);
            }
        }
    }
}
