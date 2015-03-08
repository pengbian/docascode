using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClass1
{
    /// <summary>
    /// This is a *Class1* @TestClass1.Class1 in <see cref="Test1"/> **TestClass1**
    /// </summary>
    public class Class1
    {
        /// <summary>
        /// This is a *test* with no return
        /// </summary>
        public void Test1()
        {

        }

        /// <summary>
        /// This is a *test* with return and should fail compilation
        /// This is a multiline test
        /// </summary>
        /// <returns></returns>
        public Tuple<string, int> Test2()
        {
            return null;
        }

        /// <summary>
        /// This is an async *test*
        /// </summary>
        /// <returns>Task</returns>
        public async Task<Tuple<string, int>> Test3()
        {
            return await Task.FromResult(Tuple.Create("1", 1));
        }

        /// <summary>
        /// This is a generic *test*
        /// </summary>
        /// <typeparam name="T">This is the type param</typeparam>
        /// <returns>null</returns>
        public void Test4<T>()
        {

        }
    }
}
