using System.Reflection;
using System.Text;

namespace Reflection;

public static class CsvSerializer
{
    public static string Serialize<T>(T obj)
    {
        var sb = new StringBuilder();
        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));
        sb.AppendLine(string.Join(",", properties.Select(p => p.GetValue(obj))));

        return sb.ToString();
    }

    public static T Deserialize<T>(string csvString)
    {
        var lines = csvString.Split('\n');
        var headers = lines[0].Split(',');

        var obj = (T)Activator.CreateInstance(typeof(T));

        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            for (var j = 0; j < values.Length; j++)
            {
                var property = properties.FirstOrDefault(p => p.Name == headers[j]);
                if (property != null)
                {
                    var value = Convert.ChangeType(values[j], property.PropertyType);
                    property.SetValue(obj, value);
                }
            }
        }

        return obj;
    }
}