using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscrmTools.ComponentComparer.AppCode
{
    public static class ModernAppHelper
    {
        public static string OptimizeAppDescriptor(string json, IOrganizationService service, List<EntityMetadata> emds)
        {
            var content = JObject.Parse(json);

            var components = new JArray(((JArray)((JObject)content["appInfo"])["Components"]).OrderBy(obj => obj.Value<int>("Type")).ThenBy(obj => obj.Value<string>("Id")));

            foreach (var obj in components.Where(c => c.Value<int>("Type") == 1))
            {
                var entityName = emds.FirstOrDefault(e => e.MetadataId.Equals(new Guid(obj.Value<string>("Id"))))?.LogicalName;
                ((JObject)obj).Remove("Id");
                ((JObject)obj).Add("EntityName", entityName);
            }

            var viewIds = components.Where(c => c.Value<int>("Type") == 26).Select(c => new Guid(c.Value<string>("Id"))).ToList();
            var chartIds = components.Where(c => c.Value<int>("Type") == 59).Select(c => new Guid(c.Value<string>("Id"))).ToList();
            var formIds = components.Where(c => c.Value<int>("Type") == 60).Select(c => new Guid(c.Value<string>("Id"))).ToList();

            if (viewIds.Count > 0)
            {
                var views = GetRecords(service, "savedquery", new ConditionExpression("savedqueryid", ConditionOperator.In, viewIds.ToArray()), "name", "returnedtypecode");

                foreach (var obj in components.Where(c => c.Value<int>("Type") == 26))
                {
                    ((JObject)obj).Add("TypeName", "View");
                    ((JObject)obj).Add("EntityName", views.Entities.First(v => v.Id.Equals(new Guid(obj.Value<string>("Id")))).GetAttributeValue<string>("returnedtypecode"));
                    ((JObject)obj).Add("ViewName", views.Entities.First(v => v.Id.Equals(new Guid(obj.Value<string>("Id")))).GetAttributeValue<string>("name"));
                }
            }

            if (chartIds.Count > 0)
            {
                var views = GetRecords(service, "savedqueryvisualization", new ConditionExpression("savedqueryvisualizationid", ConditionOperator.In, chartIds.ToArray()), "name");

                foreach (var obj in components.Where(c => c.Value<int>("Type") == 59))
                {
                    ((JObject)obj).Add("TypeName", "Chart");
                    ((JObject)obj).Add("ChartName", views.Entities.First(v => v.Id.Equals(new Guid(obj.Value<string>("Id")))).GetAttributeValue<string>("name"));
                }
            }

            if (formIds.Count > 0)
            {
                var views = GetRecords(service, "systemform", new ConditionExpression("formid", ConditionOperator.In, formIds.ToArray()), "name", "objecttypecode");

                foreach (var obj in components.Where(c => c.Value<int>("Type") == 60))
                {
                    ((JObject)obj).Add("TypeName", "Form");
                    ((JObject)obj).Add("EntityName", views.Entities.First(v => v.Id.Equals(new Guid(obj.Value<string>("Id")))).GetAttributeValue<string>("objecttypecode"));
                    ((JObject)obj).Add("FormName", views.Entities.First(v => v.Id.Equals(new Guid(obj.Value<string>("Id")))).GetAttributeValue<string>("name"));
                }
            }

            components = new JArray(components.OrderBy(obj => obj.Value<int>("Type")).ThenBy(obj => obj.Value<string>("EntityName")).ThenBy(obj => obj.Value<string>("Id")));

            ((JObject)content["appInfo"])["Components"] = components;

            var appComponents = new JArray(((JArray)((JObject)((JObject)content["appInfo"])["AppComponents"])["Entities"]).OrderBy(obj => obj.Value<string>("LogicalName")));
            foreach (var obj in appComponents)
            {
                ((JObject)obj).Remove("Id");
            }

       ((JObject)((JObject)content["appInfo"])["AppComponents"])["Entities"] = appComponents;

            return content.ToString();
        }

        private static EntityCollection GetRecords(IOrganizationService service, string entityName, ConditionExpression condition, params string[] columns)
        {
            return service.RetrieveMultiple(new QueryExpression(entityName)
            {
                ColumnSet = new ColumnSet(columns),
                Criteria = new FilterExpression
                {
                    Conditions = { condition }
                }
            });
        }
    }
}