using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiTests
{
    [TestClass]
    public class ConvertParams
    {
        [TestMethod]
        public void Convert()
        {
            var array1 = new Stack<string>( new[] { "leads", "update", "0", "custom", "0", "id" }.Reverse());
            var array2 = new Stack<string>( new[] { "leads", "update", "0", "custom", "0", "name" }.Reverse());
            var array3 = new Stack<string>( new[] { "leads", "update", "0", "custom", "1", "id" }.Reverse());
            var array4 = new Stack<string>( new[] { "leads", "update", "0", "custom", "1", "name" }.Reverse());

            var arrays = new List<Stack<string>> { array1, array2, array3, array4 };


            var sdf = ZAZAZ(0, arrays).ToString();

        }



        JContainer ZAZAZ(int indx, IEnumerable<Stack<string>> arrays)
        {
            var aaaa = arrays.Select(arr => {
                if (arr.ElementAtOrDefault(indx) != null) return arr.ElementAt(indx);
                else return null; })
                .Distinct()
                .Where(i => i != null)
                .ToList();

            if (aaaa.Count == 0) return new JObject(new JProperty("end", 123));

            JContainer result;

            //if (aaaa.Count() > 1)
            //{
            //    var arrr = new JArray();

            //    foreach (var i in aaaa)
            //    {
            //        var tmp = arrays.Where(arr => arr.ElementAt(indx) == i);

            //        var obj = new JObject(new )

            //        var aaa = new JProperty(i, ZAZAZ(indx + 1, rest));

            //        arrr.Add(new JObject(aaa));
            //    }
            //}

                //    result = arrr;
                //}
                //else
                //{
                //    var rest = Grouping(aaaa.First(), arrays);

                //}

                result = new JObject(new JProperty(aaaa.First(), ZAZAZ(indx + 1, arrays)));


            return result;
        }

  










        //JContainer Prepare(IEnumerable<List<string>> arrays)
        //{
        //    if (arrays.Count() == 0) return new JProperty("12323", "234234234234");

        //    var a = arrays.Select(arr => { if (arr.ElementAtOrDefault(0) != null) return arr[0]; else return null; }).Distinct().Where(i => i != null);

        //    return null;
        //}






        IEnumerable<Stack<string>> Grouping(string name, IEnumerable<Stack<string>> arrays)
        {
            var result = arrays.Where(arr => arr.ElementAt(0) == name);
            return result;
        }

        void RemoveFirst(IEnumerable<Stack<string>> arrays)
        {
            foreach (var t in arrays)
            {
                if (t.ElementAtOrDefault(0) != null) t.Pop();
            }
        }

    }











    [TestClass]
    public class ParamRecursion2
    {
        Stack<string> paramsArray = new Stack<string>(new string[] { "leads", "update", "0", "custom", "0", "id" }.Reverse());
        Stack<string> paramsArray2 = new Stack<string>(new string[] { "leads", "update", "0", "custom", "0", "idw" }.Reverse());

        JObject result = new JObject();

        Stack<string> arrays = new Stack<string>(new string[] { "leads", "update", "custom", "id" }.Reverse());
        Stack<string> arrays1 = new Stack<string>(new string[] { "leads", "update", "custom", "id" }.Reverse());

        public ParamRecursion2()
        {
            
        }

        [TestMethod]
        public void GetResult()
        {
            //var rrrrrrr = Recurs(paramsArray);

            //var ffff = Recurs(paramsArray2);

            //rrrrrrr.Merge(ffff, new JsonMergeSettings
            //{
            //    MergeArrayHandling = MergeArrayHandling.Union
            //});

            //var r = rrrrrrr.ToString();

            IEnumerable<Stack<string>> arr = new List<Stack<string>> { arrays, arrays1 };

            //var ttttttt = Rec(arr);



            Assert.IsInstanceOfType(result, typeof(JObject));
        }

        public JObject Recursion(Stack<string> array)
        {
            if (array.Count == 0) return new JObject(new JProperty("end", "ddddd" ));

            return new JObject(new JProperty(array.Pop(), Recursion(array)));
        }



        public JContainer Recurs(Stack<string> array)
        {
            var item = array.Pop();

            if (array.Count == 0)  return new JObject(new JProperty(item, "end"));
            if (isArray(array))  return new JObject(new JProperty(item, new JArray(Recurs(array))));

            return new JObject(new JProperty(item, Recurs(array)));

            bool isArray(Stack<string> arr)
            {
                if (arr.Count == 0) return false;

                var it = arr.Pop();

                var res = isNumber(it);

                if(!res) arr.Push(it);

                return res;
            }

            bool isNumber(string itm)
            {
                return Int32.TryParse(itm, out int nmbr);
            }

        }


    
    }    
}
