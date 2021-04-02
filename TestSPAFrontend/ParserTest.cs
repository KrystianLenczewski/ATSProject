using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
using PKB;
using Xunit;

namespace TestSPAFrontend
{
    public class ParserTest
    {
        [Theory, AutoMockData]
        public void ParseCodeTest(PKBStore pkb, string code)
        {
        }
    }
}
