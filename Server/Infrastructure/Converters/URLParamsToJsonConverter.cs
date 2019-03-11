using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.Converters
{
    public class URLParamsToJsonConverter
    {

        public JContainer Covert(string urlParams)
        {
            var arrayParams = new List<List<string>>();

            foreach (var e in urlParams.Split('&'))
            {
                var ar = new List<string>(e.Split("[", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Replace("]", "")).ToArray());
                arrayParams.Add(ar);
            }

            try
            {
                return Recursion(arrayParams);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private JContainer Recursion(IEnumerable<IEnumerable<string>> arrays)
        {
            var cursor = GetRootElements(arrays);

            JContainer result;

            if (cursor.All(a => Int32.TryParse(a, out int nbr)))
            {
                result = new JArray();

                foreach (var name in cursor)
                {
                    var childrensGroups = GroupByCursor(name, arrays);
                    var childrenArray = CutParent(childrensGroups);

                    result.Add(Recursion(childrenArray));
                }
            }
            else
            {
                result = new JObject();

                foreach (var name in cursor)
                {
                    if (name.Contains("="))
                    {
                        var value = name.Split('=');
                        result.Merge(new JObject(new JProperty(value[0], value[1])));
                    }
                    else
                    {
                        var childrensGroups = GroupByCursor(name, arrays);
                        var childrenArray = CutParent(childrensGroups);

                        result.Add(new JProperty(name, Recursion(childrenArray)));
                    }
                }
            }

            return result;
        }


        private IEnumerable<string> GetRootElements(IEnumerable<IEnumerable<string>> arrays)
        {
            return arrays.Select(arr => {
                if (arr.ElementAtOrDefault(0) != null) return arr.ElementAt(0);
                else return null;
            })
            .Distinct()
            .Where(i => i != null)
            .ToList();
        }

        private IEnumerable<IEnumerable<string>> GroupByCursor(string name, IEnumerable<IEnumerable<string>> arrays)
        {
            var result = arrays.Where(arr => arr.ElementAt(0) == name);
            return result;
        }

        private IEnumerable<IEnumerable<string>> CutParent(IEnumerable<IEnumerable<string>> arrays)
        {
            var res = new List<List<string>>();

            foreach (var array in arrays)
            {
                var arr = new Stack<string>(array.Reverse());
                if (arr.Count > 0) arr.Pop();
                res.Add(arr.ToList<string>());
            }

            return res;
        }
    }
}
