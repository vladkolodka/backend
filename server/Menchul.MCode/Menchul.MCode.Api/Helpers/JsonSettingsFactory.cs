using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Open.Serialization.Json;
using Open.Serialization.Json.Newtonsoft;

namespace Menchul.MCode.Api.Helpers
{
    public static class JsonSettingsFactory
    {
        // TODO
        public static IJsonSerializer ConstructSerializer()
        {
            // var factory = new JsonSerializerFactory(new JsonSerializerSettings
            // {
            //     // ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //     Converters = {new StringEnumConverter(/*new CamelCaseNamingStrategy()*/)},
            //     DateFormatString = "O",
            //     // NullValueHandling = NullValueHandling.Ignore,
            //     ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            // });

            return new JsonSerializerSettings
            {
                // ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = {new StringEnumConverter( /*new CamelCaseNamingStrategy()*/)},
                DateFormatString = "O",
                // NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            }.GetSerializerFactory().GetSerializer(new JsonSerializationOptions
            {
                OmitNull = true, CamelCaseKeys = true, CamelCaseProperties = true
            }, caseSensitive: false);

            // return factory.GetSerializer(new JsonSerializationOptions
            // {
            //     OmitNull = true,
            //     CamelCaseKeys = true,
            //     CamelCaseProperties = true
            // });
        }

        // .AddNewtonsoftJson(options =>
        // {
        //     options.SerializerSettings.DateFormatString = CommonHelpers.NewtonsoftJsonSerializerSettings.DateFormatString;
        //     options.SerializerSettings.NullValueHandling = CommonHelpers.NewtonsoftJsonSerializerSettings.NullValueHandling;
        //     options.SerializerSettings.ReferenceLoopHandling = CommonHelpers.NewtonsoftJsonSerializerSettings.ReferenceLoopHandling;
        //     options.SerializerSettings.ContractResolver = CommonHelpers.NewtonsoftJsonSerializerSettings.ContractResolver;
        //     options.UseMemberCasing();
        // })
        // .AddJsonOptions(configure =>
        // {
        //     configure.JsonSerializerOptions.IgnoreNullValues = CommonHelpers.JsonSerializerOptions.IgnoreNullValues;
        //     configure.JsonSerializerOptions.DictionaryKeyPolicy = CommonHelpers.JsonSerializerOptions.DictionaryKeyPolicy;
        //     configure.JsonSerializerOptions.PropertyNameCaseInsensitive = CommonHelpers.JsonSerializerOptions.PropertyNameCaseInsensitive;
        //
        // })
    }
}