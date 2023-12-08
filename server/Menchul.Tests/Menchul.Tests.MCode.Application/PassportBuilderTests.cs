using Menchul.MCode.Application.Companies.Dto;
using Menchul.MCode.Application.Products.Dto;
using Menchul.MCode.Application.Services;
using Menchul.MCode.Core.Enums;
using Menchul.MCode.Core.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Menchul.Tests.MCode.Application
{
    public class PassportBuilderTests
    {
        [Fact]
        public void Given_SubParentProductWithBrandOwnerCompany_When_PassportBuilt_Then_PassportContainsBrandCompany()
        {
            // Arrange
            const string email = "email@mail.com";

            var product = new ProductDto
            {
                ParentProduct = new ProductDto
                {
                    ParentProduct = new ProductDto
                    {
                        BrandOwnerCompany = new CompanyDto{Email = email}
                    }
                }
            };

            var builder = new PassportBuilder();

            // Act
            var passport = builder.Append(product).Build();

            // Assert
            Assert.Equal(email, passport.BrandOwnerCompany?.Email);
        }

        [Fact]
        public void Given_BothProductsWithBrandOwnerCompany_When_PassportBuilt_Then_BrandOwnerCompanyNotOverwritten()
        {
            // Arrange
            const string email1 = "email1@mail.com";
            const string email2 = "email2@mail.com";

            var product = new ProductDto
            {
                ParentProduct = new ProductDto
                {
                    BrandOwnerCompany = new CompanyDto{Email = email1},
                    ParentProduct = new ProductDto
                    {
                        BrandOwnerCompany = new CompanyDto{Email = email2}
                    }
                }
            };

            var builder = new PassportBuilder();

            // Act
            var passport = builder.Append(product).Build();

            // Assert
            Assert.Equal(email1, passport.BrandOwnerCompany?.Email);
        }

        [Fact]
        public void Given_IdInChildAndParentProducts_When_PassportBuilt_Then_ChildIdIsUsed()
        {
            // Arrange
            const string id1 = "ID1";
            const string id2 = "ID2";

            var product = new ProductDto
            {
                Id = id1,
                ParentProduct = new ProductDto
                {
                    Id = id2
                }
            };

            var builder = new PassportBuilder();

            // Act
            var passport = builder.Append(product).Build();

            // Assert
            Assert.Equal(id1, passport.Id);
        }

        [Fact]
        public void Given_IdTwoParentProducts_When_PassportBuiltForBothProducts_Then_IdOfTheSecondProductIsUsed()
        {
            // Arrange
            const string id1 = "ID1";
            const string id2 = "ID2";

            var product1 = new ProductDto
            {
                Id = id1
            };

            var product2 = new ProductDto
            {
                Id = id2
            };

            var builder = new PassportBuilder();

            // Act
            var passport = builder
                .Append(product1)
                .Append(product2)
                .Build();

            // Assert
            Assert.Equal(id2, passport.Id);
        }

        [Fact]
        public void Given_UrlInChildAndParentProducts_When_PassportBuilt_Then_UrlsAreMerged()
        {
            // Arrange
            var url1 = new Url("https://menchul.com/1", UrlType.General, "en");
            var url2 = new Url("https://menchul.com/2", UrlType.General, "en");

            var product = new ProductDto
            {
                Urls = new List<Url>{url1},
                ParentProduct = new ProductDto
                {
                    Urls = new List<Url>{url1, url2}
                }
            };

            var builder = new PassportBuilder();

            // Act
            var passport = builder.Append(product).Build();

            // Assert
            Assert.NotEmpty(passport.Urls);
            Assert.Equal(2, passport.Urls.Count);

            Assert.Contains(url1, passport.Urls);
            Assert.Contains(url2, passport.Urls);
        }

        [Fact]
        public void Given_UrlInSecondProductIsNull_When_PassportBuilt_Then_UrlIsNotOverwritten()
        {
            // Arrange
            const string url1 = "https://menchul.com/1";

            var product1 = new ProductDto {Urls = new List<Url>{new(url1, UrlType.General, "en")}};

            var product2 = new ProductDto {Urls = null};

            var builder = new PassportBuilder();

            // Act
            var passport = builder
                .Append(product1)
                .Append(product2)
                .Build();

            // Assert
            Assert.Equal(url1, passport.Urls.FirstOrDefault()?.Address);
        }
    }
}