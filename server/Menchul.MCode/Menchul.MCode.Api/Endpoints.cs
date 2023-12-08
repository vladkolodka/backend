using Convey.WebApi;
using Menchul.Base.Constants;
using Menchul.Base.Extensions;
using Menchul.Convey;
using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Common.Enums;
using Menchul.MCode.Application.Common.Queries;
using Menchul.MCode.Application.Companies.Commands.CreateClientCompany;
using Menchul.MCode.Application.Companies.Commands.CreateCompany;
using Menchul.MCode.Application.Companies.Commands.DeleteCompany;
using Menchul.MCode.Application.Companies.Commands.UpdateCompany;
using Menchul.MCode.Application.Companies.Dto;
using Menchul.MCode.Application.Companies.Queries;
using Menchul.MCode.Application.Packages.Commands;
using Menchul.MCode.Application.Packages.Dto;
using Menchul.MCode.Application.Packages.Queries;
using Menchul.MCode.Application.Products.Commands.CreateProduct;
using Menchul.MCode.Application.Products.Commands.DeleteProduct;
using Menchul.MCode.Application.Products.Commands.UpdateProduct;
using Menchul.MCode.Application.Products.Dto;
using Menchul.MCode.Application.Products.Queries;
using Menchul.MCode.Application.ProductUnits.Commands;
using Menchul.MCode.Application.ProductUnits.Queries;
using Menchul.MCode.Application.ReferenceBooks.Queries.Countries;
using Menchul.MCode.Application.ReferenceBooks.Queries.Currencies;
using Menchul.MCode.Application.Test;
using Menchul.MCode.Infrastructure;
using Menchul.Resources.ReferenceBooks.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Api
{
    public static class Endpoints
    {
        public static class Constraints
        {
            public const string BoolIsProvided = "BoolIfExists";
        }

        private static class Parameters
        {
            public const string EAN = "{ean}";
            public const string Id = "{id}";
            public static readonly string Deep = $"{{deep:{Constraints.BoolIsProvided}?}}";
        }

        private static class Routes
        {
            public const string Root = "";

            public const string ClientCompanies = Root + "/clients";

            public const string Companies = Root + "/companies";

            public const string Products = Root + "/products";
            public const string ProductsEAN = Products + "/ean";

            private const string Codes = Root + "/codes";
            public const string UnitCode = Codes + "/unit";
            public const string PackageCode = Codes + "/package";

            private const string ReferenceBooks = Root + "/reference-books";
            public const string ReferenceBooksCountries = ReferenceBooks + "/countries";
            public const string ReferenceBooksCurrencies = ReferenceBooks + "/currencies";

            public static readonly string ProductUnitQrV1 = $"{Root}/{QrCodeType.ProductUnitV1.Path}";
            public static readonly string ProductUnitQrV2 = $"{Root}/{QrCodeType.ProductUnitV2.Path}";
            public static readonly string PackageQR = $"{Root}/{QrCodeType.PackageV1.Path}";

            public const string Test = Root + "/test";
        }

        private static Task ParseUiHeader(QueryBase query, HttpContext context)
        {
            const string modeHeader = "X-MCode-Mode";

            if (context.Request.Headers.ContainsKey(modeHeader))
            {
                var headerValue = context.Request.Headers[modeHeader].FirstOrDefault();
                query.UseUiMode = string.Equals(headerValue?.ToUpper(), QueryBase.UiModeValue);
            }

            return Task.CompletedTask;
        }

        // TODO Response with Location for MQR and PQR
        public static IApplicationBuilder UseApplicationEndpoints(this IApplicationBuilder app,
            IConfiguration configuration) => app
            .UseDispatcherEndpoints(endpoints =>
            {
                string[] Scoped(string policy) => new[] {Policies.ScopeMCode, policy};

                endpoints
                    // companies
                    .Get<GetOwnedCompaniesQuery, List<CompanyDto>>(Routes.Companies, auth: true)
                    .Get<GetCompanyByIdQuery, CompanyDto>($"{Routes.Companies}/{Parameters.Id}", auth: true)
                    .Put<UpdateCompanyCommand>(Routes.Companies, auth: true)
                    .Delete<DeleteCompanyCommand>($"{Routes.Companies}/{Parameters.Id}", auth: true)

                    // products
                    .Get<GetProductByEanQuery, ProductDto>($"{Routes.ProductsEAN}/{Parameters.EAN}")
                    .Get<GetProductByIdQuery, ProductDto>($"{Routes.Products}/{Parameters.Id}")
                    .Get<GetOwnedProductsQuery, List<ProductDto>>(Routes.Products, auth: true)
                    .Put<UpdateProductCommand>(Routes.Products,
                        policies: Scoped(Policies.RoleUpdateProduct))
                    .Delete<DeleteProductCommand>($"{Routes.Products}/{Parameters.Id}",
                        policies: Scoped(Policies.RoleDeleteProduct))

                    // product units
                    .Get<GetProductUnitByHashQuery, PassportDto>($"{Routes.ProductUnitQrV1}/{Parameters.EAN}",
                        beforeDispatch: (query, _) => Complete(() => query.CodeType = QrCodeType.ProductUnitV1),
                        auth: false)
                    .Get<GetProductUnitByHashQuery, PassportDto>($"{Routes.ProductUnitQrV2}/{Parameters.EAN}",
                        beforeDispatch: (query, _) => Complete(() => query.CodeType = QrCodeType.ProductUnitV2),
                        auth: false)

                    // packages
                    .Get<GetPackageByHashQuery, PackageDto>($"{Routes.PackageQR}/{Parameters.Deep}",
                        auth: false)

                    // reference books
                    .Get<SearchCountriesQuery, List<CountryModel>>(Routes.ReferenceBooksCountries)
                    .Get<SearchCurrenciesQuery, List<CurrencyModel>>(Routes.ReferenceBooksCurrencies)

                    // disabled routes
                    .Get<GetProductUnitByIdQuery, PassportDto>($"{Routes.UnitCode}/{Parameters.Id}",
                        beforeDispatch: ParseUiHeader,
                        auth: false)
                    .Get<GetPackageByIdQuery, PackageDto>($"{Routes.PackageCode}/{Parameters.Id}/{Parameters.Deep}",
                        beforeDispatch: ParseUiHeader,
                        auth: false);

                endpoints
                    // companies
                    .Post<CreateClientCompanyCommand, Guid>(Routes.ClientCompanies,
                        policies: Policies.RoleCreateClientCompany)
                    .Post<CreateCompanyCommand, Guid>(Routes.Companies, auth: true)

                    // products
                    .Post<CreateProductCommand, string>(Routes.Products,
                        policies: Scoped(Policies.RoleCreateProduct),
                        afterDispatch: (i, ct) => ct.Response.Created($"{Routes.ProductsEAN}/{i.Command.EAN}"))

                    // product units
                    .Post<GenerateMQRCommand, QrResultDto>(Routes.UnitCode,
                        policies: Scoped(Policies.RoleCreatePassport))

                    // packages
                    .Post<GeneratePQRCommand, QrResultDto>(Routes.PackageCode,
                        policies: Scoped(Policies.RoleCreatePackage))

                    .Post<TestCommand, string>(Routes.Test, auth: false);
            }, useAuthorization: true)
            .MapAppInfoEndpoint(
                route: Routes.Root,
                appNameProvider: c => c.GetAppName(),
                showAssembly: configuration.DetailedErrors(),
                detailedErrors: configuration.DetailedErrors()
            );

        private static Task Complete(Action action)
        {
            action.Invoke();
            return Task.CompletedTask;
        }
    }
}