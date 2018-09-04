using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    //The complexity in this test is in creating the objects that are required to create and use a tag helper. Tag
    //helpers use IUrlHelperFactory objects to generate Urls that target different parts of the application,
    //and I have used Moq to create an implementation of this interface and the related IUrlHelper interface
    //that provides test data.
    //The core part of the test verifies the tag helper output by using a literal string value that contains double
    //quotes.C# is perfectly capable of working with such strings, as long as the string is prefixed with @
    //and uses two sets of double quotes ("") in place of one set of double quotes.You must remember not
    //to break the literal string into separate lines unless the string you are comparing to is similarly broken.
    //For example, the literal I use in the test method has wrapped onto several lines because the width of a
    //printed page is narrow.I have not added a newline character; if I did, the test would fail.
    public class PageLinkTagHelperTests
    {
        [Fact]
        public void CanGenerate_Page_Links()
        {
            // Arrange
            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            urlHelperFactory.Setup(f =>
                f.GetUrlHelper(It.IsAny<ActionContext>()))
                    .Returns(urlHelper.Object);

            PageLinkTagHelper helper = new PageLinkTagHelper(urlHelperFactory.Object)
            {
                PageModel = new PagingInfo
                {
                    CurrentPage = 2,
                    TotalItems = 28,
                    ItemsPerPage = 10
                },
                PageAction = "Test"
            };

            TagHelperContext ctx = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(), "");

            var content = new Mock<TagHelperContent>();
            TagHelperOutput output = new TagHelperOutput("div",
                new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(content.Object));

            // Act
            helper.Process(ctx, output);

            // Assert
            Assert.Equal(@"<a href=""Test/Page1"">1</a>"
                + @"<a href=""Test/Page2"">2</a>"
                + @"<a href=""Test/Page3"">3</a>",
                 output.Content.GetContent());
        }
    }
}
